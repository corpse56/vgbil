using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using Test1;
using System.Globalization;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.IO;
namespace Circulation
{
    public delegate void ScannedEventHandler(object sender, EventArgs ev);
    public delegate void HeaderClick(object sender, DataGridViewCellMouseEventArgs ev);
    public delegate void AbonChangedEventHandler(object sender, EventArgs ev);

    public partial class Form1 : Form
    {
        // System.Collections.Generic.Dictionary<int, string> FFFF = new Dictionary<int, string>(8);
        public static event AbonChangedEventHandler AbonChanged;

        public DBWork dbw;
        //_BarcScan sc;
        //private string WasFirstScan = "";
        public string EmpID;
        private Form2 f2;
        private Form4 f4;
        private Form5 f5;
        SerialPort port;

        public DBWork.dbReader ReaderRecord, ReaderRecordWork, ReaderRecordFormular;
        public DBWork.dbBook BookRecord, BookRecordWork;
        public DBWork.dbReader ReaderSetBarcode;
        private System.Windows.Forms.Label label16;
        public ExtGui.RoundProgress RndPrg;
        public Form1()
        {
            //System.Collections.Generic.List<int> f = new List<int>(3);

            f2 = new Form2(this);
            InitializeComponent();
            dbw = new DBWork(this);
            //ReaderRecord = new DBWork.dbReader("9643907728022200024 " + "07020077");
            //sc = new _BarcScan(this);
            //this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            //this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.label16 = new System.Windows.Forms.Label();
            //this.Controls.Add(RndPrg);
            // 
            // label16
            // 
            this.Controls.Add(this.label16);
            f2.ShowDialog();

            /*if (EmpID == null)
            {
                MessageBox.Show("Вы не авторизованы! Программа завершает свою работу.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }*/
            //Form1.Scanned += new ScannedEventHandler(Form1_Scanned);
            Form1.AbonChanged += new AbonChangedEventHandler(Form1_AbonChanged);
            //dataGridView1.Rows.RemoveAt(0);// .Remove(dataGridView1.Rows[0].);
            //ReaderRecord = new DBWork.dbReader();
            //BookRecord = new DBWork.dbBook();    
            this.button2.Enabled = false;
            this.button4.Enabled = false;
            label4.Text = "Журнал событий " + DateTime.Now.ToShortDateString() + ":";
            //this.Controls.Add(this.panel1);
            //this.tabPage3.Controls.Remove(this.panel1);
            //MessageBox.Show(tabControl1.SelectedTab.ToString());// = tabControl1.TabPages[1];
            this.label16.AutoSize = true;
            this.label16.BringToFront();
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(328, 458);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(355, 39);
            this.label16.TabIndex = 0;
            this.label16.Text = "Считайте штрихкод";
            this.label16.Visible = false;
            this.label16.ForeColor = Color.Green;
            Formular.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Formular.Columns.Clear();
            tabControl1.TabPages.RemoveAt(2);
            //dbw = new DBWork(this);
            //Formular.Columns
            //Statistics.
            dbw.SetPenaltyAll();
            port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            //bool op = port.IsOpen;

            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            port.Handshake = Handshake.RequestToSend;
            port.NewLine = Convert.ToChar(13).ToString();

            try
            {
                port.Open();
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }





        }
        string FromPort = "";
        public delegate void ScanFuncDelegate(object sender, SerialDataReceivedEventArgs e);
        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            FromPort = port.ReadLine();
            FromPort = FromPort.Trim();
            port.DiscardInBuffer();
            ScanFuncDelegate ScanDelegate;
            ScanDelegate = new ScanFuncDelegate(Form1_Scanned);
            this.Invoke(ScanDelegate, new object[] { sender, e });
        }
        public static void FireAbon(object sender, EventArgs ev)
        {
            if (AbonChanged != null)
            {
                AbonChanged(sender, ev);
            }
        }


        void Form1_AbonChanged(object sender, EventArgs ev)
        {
            if (this.ReaderRecord != null)
            {
                this.ReaderRecord.AbonType = ((Form5)sender).abon;
            }
        }
        //public enum Regim {Knigi,vidachaShtrihkodov,} 
        public class DataGridViewDisableButtonColumn : DataGridViewButtonColumn
        {
            public DataGridViewDisableButtonColumn()
            {
                this.CellTemplate = new DataGridViewDisableButtonCell();
            }
        }
        public class DataGridViewDisableButtonCell : DataGridViewButtonCell
        {
            private bool enabledValue;
            public bool Enabled
            {
                get
                {
                    return enabledValue;
                }
                set
                {
                    enabledValue = value;
                }
            }

            // Override the Clone method so that the Enabled property is copied.
            public override object Clone()
            {
                DataGridViewDisableButtonCell cell =
                    (DataGridViewDisableButtonCell)base.Clone();
                cell.Enabled = this.Enabled;
                return cell;
            }

            // By default, enable the button cell.
            public DataGridViewDisableButtonCell()
            {
                this.enabledValue = true;
            }

            protected override void Paint(Graphics graphics,
                Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates elementState, object value,
                object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle,
                DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                // The button cell is disabled, so paint the border,  
                // background, and disabled button for the cell.
                if (!this.enabledValue)
                {
                    // Draw the cell background, if specified.
                    if ((paintParts & DataGridViewPaintParts.Background) ==
                        DataGridViewPaintParts.Background)
                    {
                        SolidBrush cellBackground =
                            new SolidBrush(cellStyle.BackColor);
                        graphics.FillRectangle(cellBackground, cellBounds);
                        cellBackground.Dispose();
                    }

                    // Draw the cell borders, if specified.
                    if ((paintParts & DataGridViewPaintParts.Border) ==
                        DataGridViewPaintParts.Border)
                    {
                        PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                            advancedBorderStyle);
                    }

                    // Calculate the area in which to draw the button.
                    Rectangle buttonArea = cellBounds;
                    Rectangle buttonAdjustment =
                        this.BorderWidths(advancedBorderStyle);
                    buttonArea.X += buttonAdjustment.X;
                    buttonArea.Y += buttonAdjustment.Y;
                    buttonArea.Height -= buttonAdjustment.Height;
                    buttonArea.Width -= buttonAdjustment.Width;

                    // Draw the disabled button.                
                    ButtonRenderer.DrawButton(graphics, buttonArea,
                        PushButtonState.Disabled);

                    // Draw the disabled button text. 
                    if (this.FormattedValue is String)
                    {
                        TextRenderer.DrawText(graphics,
                            (string)this.FormattedValue,
                            this.DataGridView.Font,
                            buttonArea, SystemColors.GrayText);
                    }
                }
                else
                {
                    // The button cell is enabled, so let the base class 
                    // handle the painting.
                    base.Paint(graphics, clipBounds, cellBounds, rowIndex,
                        elementState, value, formattedValue, errorText,
                        cellStyle, advancedBorderStyle, paintParts);
                }
            }
        }
        void FormularColumnsForming(string ReaderID)
        {
            Formular.Columns.Clear();
            Formular.AutoGenerateColumns = false;
            Formular.Columns.Add("NN", "№№");
            Formular.Columns[0].Width = 35;
            dbw.SetPenalty(ReaderID);
            try
            {
                Formular.DataSource = dbw.GetFormular(ReaderID);
                //Formular.DataMember = "tmp";
            }
            catch (IndexOutOfRangeException evv)
            {
                string d = evv.Message;
                MessageBox.Show("Читатель не является задолжником!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //Formular.Columns[1].HeaderText = "№№";
            if (Formular.Rows.Count == 0)
            {
                MessageBox.Show("За читателем не числится книг и нарушений!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Formular.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Заглавие";
            col.Width = 280;
            col.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            col.ReadOnly = true;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Zag";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "zagsort";
            col.Visible = false;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Заглавие_sort";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Автор";
            col.Width = 150;
            col.ReadOnly = true;
            col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Автор";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "";
            col.Visible = false;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Автор_Sort";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "";
            col.Visible = false;
            col.Name = "idmain";
            col.DataPropertyName = "idmain";
            Formular.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "";
            col.Visible = false;
            col.Name = "zkid";
            col.DataPropertyName = "zkid";
            this.Formular.Columns.Add(col);
            
            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "";
            col.Visible = false;
            col.Name = "zid";
            col.DataPropertyName = "zid";
            this.Formular.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Год изда ния";
            col.ReadOnly = true;
            col.Width = 45;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Год_Издания";

            /*col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Место издания";
                    
            Formular.Columns.Add(col);
            col.DataPropertyName = "Место_Издания";*/

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата выдачи";
            col.ReadOnly = true;
            col.Width = 80;
            Formular.Columns.Add(col);
            col.DataPropertyName = "issue";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Предпо лагае мая дата возврата";
            col.ReadOnly = true;
            col.Width = 80;
            col.DataPropertyName = "vozv";
            col.Name = "vozv";
            Formular.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Факти ческая дата возв рата";
            col.ReadOnly = true;
            col.Width = 80;
            col.Name = "fact";
            col.DataPropertyName = "fact";
            Formular.Columns.Add(col);

            DataGridViewCheckBoxColumn colch = new DataGridViewCheckBoxColumn();
            colch.HeaderText = "Нару шение";
            colch.Name = "pen";
            colch.Width = 50;
            colch.ReadOnly = false;
            Formular.Columns.Add(colch);
            colch.DataPropertyName = "penalty";

            colch = new DataGridViewCheckBoxColumn();
            colch.Name = "rempen";
            colch.Visible = false;
            Formular.Columns.Add(colch);
            colch.DataPropertyName = "rempenalty";

            DataGridViewDisableButtonColumn ButCol = new DataGridViewDisableButtonColumn();
            Formular.Columns.Add(ButCol);
            ButCol.Name = "but";
            ButCol.Width = 120;
            ButCol.HeaderText = "Продление срока пользо вания";
            ButCol.DefaultCellStyle.BackColor = Form1.DefaultBackColor;
            Padding myPadd = ButCol.DefaultCellStyle.Padding;
            ButCol.DefaultCellStyle.SelectionBackColor = Form1.DefaultBackColor;
            ButCol.DefaultCellStyle.SelectionForeColor = Color.Black;
            Font myF = new Font(FontFamily.GenericSansSerif, 8);
            ButCol.DefaultCellStyle.Font = myF;
            myPadd.All = 1;
            ButCol.DefaultCellStyle.Padding = myPadd;
            Formular.Columns["pen"].ReadOnly = true;

            col = new DataGridViewTextBoxColumn();
            col.Visible = false;
            col.Width = 80;
            col.Name = "BAR";
            col.DataPropertyName = "BAR";
            Formular.Columns.Add(col);


            foreach (DataGridViewRow row in Formular.Rows)
            {

                //row.Cells["pen"].ReadOnly = true;
                DataGridViewDisableButtonCell bc = (DataGridViewDisableButtonCell)row.Cells["but"]; ;
                if ((row.Cells["pen"].Value.ToString().ToLower() == "false") && (row.Cells["zkid"].Value.ToString() != "0") && (row.Cells["rempen"].Value.ToString().ToLower() == "true"))
                {
                    bc.Value = "Нет нарушения";//ранее сняли
                    bc.Enabled = false;
                }
                else
                    if ((row.Cells["pen"].Value.ToString().ToLower() == "false") && (row.Cells["rempen"].Value.ToString().ToLower() == "false"))
                    {
                        bc.Value = "Продлить";
                        bc.Enabled = true;
                        //row.Cells["pen"].ReadOnly = true;
                    }
                    else
                        if ((row.Cells["pen"].Value.ToString().ToLower() == "true") && (row.Cells["rempen"].Value.ToString().ToLower() == "false") && (row.Cells["zkid"].Value.ToString() != "0"))
                        {
                            bc.Value = "Продлить";//книга еще не возвращена
                            bc.Enabled = true;

                        }
                        else
                            if ((row.Cells["pen"].Value.ToString().ToLower() == "true") && (row.Cells["rempen"].Value.ToString().ToLower() == "false") && (row.Cells["zkid"].Value.ToString() == "0"))
                            {
                                bc.Value = "Снять нарушение";//книга возвращена, но с нарушением срока
                                bc.Enabled = true;
                            }
                            else
                                if ((row.Cells["pen"].Value.ToString().ToLower() == "true") && (row.Cells["rempen"].Value.ToString().ToLower() == "true"))
                                {
                                    bc.Value = "Нет нарушения";//такого по идее не должно быть, надо тока запретить выставлять нарушения и обсудить с СБ.
                                    bc.Enabled = false;
                                    MessageBox.Show("Программа выполнила недопустимую операцию.Такого быть не должно. Обратитесь к разработчику.");
                                }

            }
            autoinc(Formular);

        }
        void Form1_Scanned(object sender, EventArgs ev)
        {

            #region cash
            /*Singletone  - надо почитать про это
typedef std::pair<BookId, bool> BookFreePair;
typedef std::vector<BookFreePair> BooksFreeType;

в классе должен быть экземпляр BooksFreeType

BooksFreeType cache_;
...
bool isBookBusy(BookId bookId)
{
   if(iterator find = cashe_.find(bookId))
      return find->second;

   bool busy = ReadBookBusyFromBD();
   cache.push_back(std::make_pair(bookId, busy));
   return busy;
}
              */
            #endregion
            //MessageBox.Show(((IOPOSScanner_1_10)sender).ScanData.ToString());
            string g = tabControl1.SelectedTab.ToString();
            try
            {
                switch (tabControl1.SelectedTab.Text)
                {
                    case "Формуляр читателя":
                        //string _data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                        string _data = FromPort;

                        if (!dbw.isReader(_data))
                        {
                            MessageBox.Show("Неверный штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        /*if (_data.Length < 20)
                            _data = _data.Remove(0, 1);*/
                        //_data = _data.Remove(_data.Length - 1, 1);
                        ReaderRecordFormular = new DBWork.dbReader(_data);
                        if (ReaderRecordFormular.barcode == "error")
                        {
                            MessageBox.Show("Читатель не зарегистрирован либо не соответствует серия социальной карты!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        label20.Text = ReaderRecordFormular.Surname + " " + ReaderRecordFormular.Name + " " + ReaderRecordFormular.SecondName;
                        textBox6.Text = ReaderRecordFormular.AbonType;
                        label25.Text = ReaderRecordFormular.id;
                        pictureBox2.Image = ReaderRecordFormular.Photo;
                        //dbw.SetPenalty(ReaderRecordFormular.id);
                        this.FormularColumnsForming(ReaderRecordFormular.id);

                        /*Formular.Columns[1].Width = 100;
                        Formular.Columns[2].Visible = false;
                        Formular.Columns[4].Visible = false;
                        Formular.Columns[3].HeaderText = "Автор";
                        Formular.Columns[3].Width = 90;
                        Formular.Columns[5].HeaderText = "Год издания";
                        Formular.Columns[5].Width = 110;
                        Formular.Columns[7].Visible = false;
                        Formular.Columns[6].HeaderText = "Место Издания";
                        Formular.Columns[6].Width = 170;
                        Formular.Columns[8].HeaderText = "Дата выдачи";
                        Formular.Columns[8].Width = 130;
                        Formular.Columns[9].HeaderText = "Предполагаемая дата возврата";
                        Formular.Columns[9].Width = 130;
                        Formular.Columns[10].HeaderText = "Фактическая дата возврата";
                        Formular.Columns[10].Width = 130;
                        Formular.Columns[11].HeaderText = "Нарушение";
                        Formular.Columns[11].Width = 130;*/


                        //Formular.Columns[8].Visible = false;
                        //Formular.Columns[9].Visible = false;
                        Sorting.WhatStat = Stats.Formular;
                        Sorting.AuthorSort = SortDir.None;
                        Sorting.ZagSort = SortDir.None;
                        break;
                    case "Выдача штрихкода читателю":
                        //timer2.Enabled = true;
                        label16.Visible = true;

                        /*if (button6.Enabled)
                        {
                            MessageBox.Show("Сначала нажмите на кнопку \"Считать и присвоить штрихкод\"!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        button6.Enabled = false;*/
                        //if (timer2.Enabled)
                        if (label16.Visible)
                        {
                            //string BarCode = ((IOPOSScanner_1_10)sender).ScanData.ToString().Remove(((IOPOSScanner_1_10)sender).ScanData.ToString().Length - 1, 1);
                            string BarCode = FromPort;

                            switch (dbw.SetReaderBarCode(ReaderSetBarcode.id, BarCode))
                            {
                                case -5:
                                    MessageBox.Show("Считан неверный штрихкод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                case -1:
                                    MessageBox.Show("Ошибка базы данных.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                                case -2:
                                    MessageBox.Show("Читатель не найден", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                                case -4:
                                    //panel1.Visible = false;
                                    //tabControl1.Enabled = true;
                                    MessageBox.Show("Такой штрихкод уже существует в базе! Считайте другой!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    break;
                                case -3:
                                    MessageBox.Show("У читателя есть штрихкод на социальной карте! Выдача штрихкода этому читателю не требуется.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    //timer2.Enabled = false;
                                    label16.Visible = false;
                                    //tabControl1.Enabled = true;
                                    groupBox4.Enabled = true;
                                    break;
                                case 1:
                                    textBox5.Text = BarCode;
                                    //timer2.Enabled = false;
                                    label16.Visible = false;
                                    //tabControl1.Enabled = true;
                                    groupBox4.Enabled = true;
                                    label16.Visible = false;
                                    MessageBox.Show("Штрихкод успешно считан и присвоен!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    break;

                            }
                            return;
                        }
                        else
                            MessageBox.Show("Сначала найдите читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    case "Приём/выдача изданий":
                        #region priemVidacha
                        //timer1.Enabled = true;
                        label1.Enabled = true;
                        //button2.Enabled = false;
                        //button4.Enabled = false;
                        //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                        if ((this.emul == null) || (this.emul == ""))
                        {
                            _data = FromPort;
                        }
                        else
                        {
                            _data = this.emul;
                        }

                        if ((this.ReaderRecord != null) && (this.BookRecord != null))
                        {
                            MessageBox.Show("Подтвердите предыдущую операцию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //MessageBox.Show(_data);
                        ReaderRecordWork = null;
                        BookRecordWork = null;
                        if (dbw.isReader(_data))
                        {
                            /*if (_data.Length < 20)
                                _data = _data.Remove(0, 1);*/
                            //_data = _data.Remove(_data.Length - 1, 1);
                            ReaderRecordWork = new DBWork.dbReader(_data);
                            if (this.ReaderRecord != null)
                            {
                                MessageBox.Show("Подтвердите предыдущую операцию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (ReaderRecordWork.barcode == "error")
                            {
                                MessageBox.Show("Читатель не зарегистрирован либо не соответствует серия социальной карты!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            //_data = _data.Remove(_data.Length - 1, 1);
                            BookRecordWork = new DBWork.dbBook(_data);
                            if (BookRecordWork.id == "Неверный штрихкод")
                            {
                                MessageBox.Show("Считан неверный штрихкод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                        }
                        if ((BookRecord != null) && (BookRecordWork != null))
                        {
                            MessageBox.Show("Считаны штрихкоды 2-х изданий подряд! Считайте штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (ReaderRecord == null)
                        {
                            if (BookRecord != null)
                            {
                                if (ReaderRecordWork != null)
                                {
                                    ReaderRecord = ReaderRecordWork.Clone();
                                    this.label5.Text = ReaderRecord.FIO;
                                    this.RPhoto.Image = ReaderRecord.Photo;
                                    //заполнена книга и читатель ждать нажати я подтвердить или отмена
                                    button2.Enabled = true;
                                    button4.Enabled = true;
                                    button2.Focus();
                                    this.AcceptButton = button2;
                                    label1.Text = "Подтвердите выдачу";
                                }
                                else
                                {
                                    MessageBox.Show("Считаны штрихкоды 2-х изданий подряд! Считайте штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            else
                            {
                                if (ReaderRecordWork != null)
                                {
                                    MessageBox.Show("Считан штрихкод читателя! Считайте штрихкод издания!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                                else
                                {
                                    if (dbw.isBookBusy(_data))
                                    {
                                        dbw.setBookReturned(BookRecordWork.id);
                                        dataGridView1.Rows.Insert(0, 1);
                                        dataGridView1.Rows[0].Cells[0].Value = DateTime.Now.ToLongTimeString();
                                        dataGridView1.Rows[0].Cells[1].Value = BookRecordWork.name;
                                        dataGridView1.Rows[0].Cells[2].Value = BookRecordWork.rname;
                                        dataGridView1.Rows[0].Cells[3].Value = "Возвращено";
                                        dbw.InsertActionRETURNED(new DBWork.dbReader(int.Parse(BookRecordWork.rid)), BookRecordWork);
                                        BookRecord = null;
                                        ReaderRecord = null;
                                        button2.Enabled = false;
                                        button4.Enabled = false;
                                        label1.Text = "Считайте штрихкод издания";
                                        //MessageBox.Show("Книга возвращена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    else
                                    {
                                        this.label8.Text = BookRecordWork.author;
                                        this.label9.Text = BookRecordWork.name;
                                        this.label1.Text = "Считайте штрих код читателя";
                                        this.button4.Enabled = true;
                                        BookRecord = BookRecordWork.Clone();
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (BookRecord != null)
                            {
                                MessageBox.Show("Подтвердите предыдущую операцию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                button2.Enabled = true;
                                button4.Enabled = true;
                                return;
                            }
                            else
                            {
                                this.label1.Text = "Считайте штрих код издания!";
                                BookRecord = null;
                                ReaderRecord = null;
                            }
                        }

                        /*if (dbw.isReader(_data))
                            ReaderRecord = dbw.getDbReader(_data);
                        else
                            BookRecord = dbw.getDbBook(_data);*/

                        break;
                        #endregion
                    case "Справка":
                        //timer1.Enabled = false;
                        label1.Enabled = false;
                        //timer2.Enabled = false;
                        label16.Visible = false;
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " - " + ex.Message + " \r\nСделайте копию сообщения (PrintScreen) и обратитесь в отдел автоматизации!");
            }

        }

        public static event ScannedEventHandler Scanned;
        public void FireScan(object sender, EventArgs ev)
        {
            if (Form1.Scanned != null)
                Form1.Scanned(sender, ev);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {

            //dbw.setReaderRight("1000002");
            dbw.setBookReturned("503");
            //if (dataGridView1.Rows[0].Cells[0].Value != null)
            /*if ((dataGridView1.Rows.Count == 1) && (dataGridView1.Rows[0].Cells[0].Value == null))
                dataGridView1.Rows[0].Cells[0].Value = DateTime.Now.ToLongTimeString();
            else
            {
                dataGridView1.Rows.Insert(0, 1);
                dataGridView1.Rows[0].Cells[0].Value = DateTime.Now.ToLongTimeString();
            }*/
            //string d = dbw.getBookFromZAKAZ("R00063Y0803").id;
            //bool f = dbw.isReaderHaveRights("R00063Y0803", "R1000004g");
            //string f = dbw.getBookFromZAKAZ("R00063Y0803").name;
            //dbw.setBookForReader("R00063Y0803", "1234", (int)numericUpDown1.Value);

            //dataGridView1.Rows.Add(1);
            //dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[0].Value = DateTime.Now.ToShortTimeString().ToString();
            //dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1].Value = "Читатель " + dbw.getDbReader("1234").FIO + " вернул книгу.";

            //            dbw.setBookReturned("1");
            //MessageBox.Show(dbw.getDbReader("1234").barcode.ToString() + dbw.getDbReader("1234").id.ToString());
            //MessageBox.Show(dbw.getDbBook("R00063Y0803").barcode);
            //string f = dbw.isReader("R1000001");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //BookRecordWork = new DBWork.dbBook("R00063Y0803");

            f2.textBox2.Text = "";
            f2.textBox3.Text = "";
            f2.ShowDialog();
            if ((this.EmpID == "") || (this.EmpID == null))
            {
                MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Visible = !label1.Visible;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            bool set = false;
            //long copy_tStatus;
            //copy_tStatus = 0;
            if (dbw.isBookBusy(BookRecord.barcode))
            {
                MessageBox.Show("Книга у другого читателя! Дата возврата: " + dbw.GetDateRet(BookRecord.barcode) + ".", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                this.label8.Text = "";
                this.label9.Text = "";
                this.label5.Text = "";
                RPhoto.Image = null;
                BookRecord = null;
                ReaderRecord = null;
            }
            else
            {
                /*if (dbw.isReaderHaveRights(ReaderRecord))
                {
                    if (!dbw.isRightsExpired(ReaderRecord.id))
                    {
                        set = true;
                    }
                    else
                    {
                        switch (MessageBox.Show("У данного читателя закончился срок прав пользования библиотекой британского совета! Хотите продлить этому пользователю права на получение книг британского совета и выдать эту книгу?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                        {
                            case DialogResult.Yes:
                                set = true;
                                dbw.ProlongRights(ReaderRecord.id);
                                break;
                            case DialogResult.No:
                                set = false;
                                this.label8.Text = "";
                                this.label9.Text = "";
                                this.label5.Text = "";
                                BookRecord = null;
                                ReaderRecord = null;
                                button2.Enabled = false;
                                button4.Enabled = false;
                                label1.Text = "Считайте штрихкод издания";
                                break;
                            case DialogResult.Cancel:
                                set = false;
                                break;
                        }

                    }

                }
                else
                {
                    switch (MessageBox.Show("У данного читателя нет прав для получения книг британского совета! Хотите выдать этому пользователю права на получение книг британского совета и выдать эту книгу?", "Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            set = true;
                            dbw.setReaderRight(ReaderRecord.id);
                            break;
                        case DialogResult.No:
                            set = false;
                            this.label8.Text = "";
                            this.label9.Text = "";
                            this.label5.Text = "";
                            BookRecord = null;
                            ReaderRecord = null;
                            button2.Enabled = false;
                            button4.Enabled = false;
                            label1.Text = "Считайте штрихкод издания";
                            break;
                        case DialogResult.Cancel:
                            set = false;
                            break;
                    }

                }*/
                set = true;
                if (set)
                {
                    if (ReaderRecord.AbonType == "Нет значения")
                    {
                        MessageBox.Show("У данного читателя не присвоено значение типа абонемента. Выдача невозможна. Сначала присвойте читателю тип абонемента на вкладке \"Формуляр читателя\"", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (dbw.GetBookCountForReader(ReaderRecord.id) >= 3)
                    {
                        switch (MessageBox.Show("Данный читатель пытается взять более 3 книг. Хотите продолжить выдачу?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        {
                            case DialogResult.No:
                                this.label8.Text = "";
                                this.label9.Text = "";
                                this.label5.Text = "";
                                RPhoto.Image = null;
                                BookRecord = null;
                                ReaderRecord = null;
                                button2.Enabled = false;
                                button4.Enabled = false;
                                label1.Text = "Считайте штрихкод издания";
                                return;
                            case DialogResult.Yes:
                                dbw.setBookForReader(BookRecord, ReaderRecord, (int)numericUpDown1.Value);
                                dataGridView1.Rows.Insert(0, 1);
                                dataGridView1.Rows[0].Cells[0].Value = DateTime.Now.ToLongTimeString();
                                dataGridView1.Rows[0].Cells[1].Value = BookRecord.name;
                                BookRecord = new DBWork.dbBook(BookRecord.barcode);
                                dataGridView1.Rows[0].Cells[2].Value = ReaderRecord.FIO;
                                dataGridView1.Rows[0].Cells[3].Value = "Выдано";
                                this.label8.Text = "";
                                this.label9.Text = "";
                                this.label5.Text = "";
                                RPhoto.Image = null;
                                dbw.InsertActionISSUED(ReaderRecord, BookRecord);
                                BookRecord = null;
                                ReaderRecord = null;
                                button2.Enabled = false;
                                button4.Enabled = false;
                                label1.Text = "Считайте штрихкод издания";
                                break;
                        }

                    }
                    else
                    {
                        dbw.setBookForReader(BookRecord, ReaderRecord, (int)numericUpDown1.Value);
                        dataGridView1.Rows.Insert(0, 1);
                        dataGridView1.Rows[0].Cells[0].Value = DateTime.Now.ToLongTimeString();
                        dataGridView1.Rows[0].Cells[1].Value = BookRecord.name;
                        BookRecord = new DBWork.dbBook(BookRecord.barcode);
                        dataGridView1.Rows[0].Cells[2].Value = ReaderRecord.FIO;
                        dataGridView1.Rows[0].Cells[3].Value = "Выдано";
                        this.label8.Text = "";
                        this.label9.Text = "";
                        this.label5.Text = "";
                        RPhoto.Image = null;
                        dbw.InsertActionISSUED(ReaderRecord, BookRecord);
                        BookRecord = null;
                        ReaderRecord = null;
                        button2.Enabled = false;
                        button4.Enabled = false;
                        label1.Text = "Считайте штрихкод издания";
                    }

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //dbw.isBookBusy("");
            this.label8.Text = "";
            this.label9.Text = "";
            this.label5.Text = "";
            RPhoto.Image = null;
            BookRecord = null;
            ReaderRecord = null;
            label1.Text = "Считайте штрихкод издания";
            button2.Enabled = false;
            button4.Enabled = false;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReaderSetBarcode = new DBWork.dbReader((int)numericUpDown2.Value);
            if (ReaderSetBarcode.barcode == "error")
            {
                MessageBox.Show("Читатель не найден!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                button6.Enabled = false;
                return;
            }
            textBox2.Text = ReaderSetBarcode.Surname;
            textBox3.Text = ReaderSetBarcode.Name;
            textBox4.Text = ReaderSetBarcode.SecondName;
            textBox5.Text = "R" + ReaderSetBarcode.barcode;
            button8.Visible = true;
            //timer2.Enabled = true;
            label16.Visible = true;
            //tabControl1.Enabled = false;
            groupBox4.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //timer2.Enabled = true;
            label16.Visible = true;
            //tabControl1.Enabled = false;
            groupBox4.Enabled = false;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "Выдача штрихкода читателю":
                    //timer2.Enabled = false;
                    label16.Visible = false;
                    //timer1.Enabled = false;
                    label1.Enabled = false;
                    numericUpDown2.Value = 0;
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    this.label16.Text = "Считайте штрихкод";
                    button6.Enabled = false;
                    button8.Visible = false;
                    break;
                case "Приём/выдача изданий":
                    //timer1.Enabled = true;
                    label1.Enabled = true;
                    //timer2.Enabled = false;
                    label16.Visible = false;
                    
                    //this.label8.Text = "";
                    //this.label9.Text = "";
                    //this.label5.Text = "";
                    //this.label16.Text = "";
                    //this.label16.Visible = false;
                    //BookRecord = null;
                    //ReaderRecord = null;
                    label1.Text = "Считайте штрихкод издания";
                    //button2.Enabled = false;
                    //button4.Enabled = false;
                    this.AcceptButton = button5;
                    break;
                case "Справка":
                    //timer1.Enabled = false;
                    label1.Enabled = false;
                    //timer2.Enabled = false;
                    label16.Visible = false;
                    this.label16.Visible = false;
                    this.label16.Text = "";
                    break;
                case "Формуляр читателя":
                    label25.Text = "";
                    label20.Text = "";
                    textBox6.Text = "";
                    pictureBox2.Image = null;
                    Formular.Columns.Clear();
                    AcceptButton = this.button10;

                    break;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "bRIT_SOVETDataSet.ZAKAZ". При необходимости она может быть перемещена или удалена.
            //this.zAKAZTableAdapter.Fill(this.bRIT_SOVETDataSet.ZAKAZ);
            //this.EmpID = "1";
            if ((this.EmpID == "") || (this.EmpID == null))
            {
                MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            //this.reportViewer1.RefreshReport();
            //this.reportViewer2.RefreshReport();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //int Y = button7.Location.Y-20;
            button12.Enabled = false;
            int x = this.Left + button7.Left;
            int y = this.Top + button7.Top + tabControl1.Top + 60;
            //Point p = button7.PointToScreen(button7.Location);
            contextMenuStrip1.Show(x, y);
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            label16.Visible = !label16.Visible;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.Columns.Add("NN", "№ п/п");
            label19.Text = "Список нарушителей сроков из фонда Библиотеки Британского совета на " + DateTime.Now.ToShortDateString() ;
            label18.Text = "";
            //DataSet StatDS = dbw.GetDebtors();
            try
            {
                Statistics.DataSource = dbw.GetDebtors();//StatDS.Tables[0];
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Задолжников нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Дата возврата";
            //Statistics.Columns[1].ValueType = typeof(DateTime);
            //Statistics.Columns[1].CellTemplate.ValueType = typeof(DateTime); 
            Statistics.Columns[1].ValueType = typeof(DateTime);
            Statistics.Columns[1].DefaultCellStyle.Format = "dd.MM.yyyy";

            Statistics.Columns[2].HeaderText = "Номер билета";
            Statistics.Columns[2].Width = 90;
            Statistics.Columns[3].HeaderText = "ФИО";
            Statistics.Columns[3].Width = 110;
            Statistics.Columns[4].HeaderText = "Заглавие,-Автор";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[5].HeaderText = "Штрихкод";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "-----";
            Statistics.Columns[6].Visible = false;
            //Statistics.Columns[7].HeaderText = "Email";
            //Statistics.Columns[7].Width = 130;
            Statistics.Columns[7].Visible = false;

            Statistics.Columns[8].HeaderText = "Телефон";
            Statistics.Columns[8].Width = 100;
            Statistics.Columns[9].HeaderText = "Примечание";
            Statistics.Columns[9].Width = 100;
            Statistics.Columns[10].HeaderText = "Место издания";
            Statistics.Columns[10].Width = 100;
            Statistics.Columns[11].HeaderText = "Дата издания";
            Statistics.Columns[11].Width = 100;
            Statistics.Columns[12].HeaderText = "Шифр";
            Statistics.Columns[12].Width = 100;
            /*Statistics.Columns[8].Visible = false;
            Statistics.Columns[9].Visible = false;
            Statistics.Columns[10].HeaderText = "Штрихкод";
            Statistics.Columns[10].Width = 100;
            Statistics.Columns[11].Visible = false;
            Statistics.Columns[12].HeaderText = "Email";
            Statistics.Columns[12].Width = 100;
            Statistics.Columns[13].HeaderText = "Телефон";
            Statistics.Columns[13].Width = 100;*/
            Sorting.WhatStat = Stats.Debtors;
            Sorting.AuthorSort = SortDir.None;
            Sorting.ZagSort = SortDir.None;
            //Statistics.Columns[10].Visible = false;
            foreach (DataGridViewRow r in Statistics.Rows)
            {
                if (r.Cells[6].Value.ToString() == "true")
                {
                    r.DefaultCellStyle.BackColor = Color.LightYellow;
                }
            }
            //DataGridViewColumn col = new DataGridViewColumn();
            //col.HeaderText = "№/№";
            button12.Enabled = true;
            //Statistics.Columns
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.Columns.Add("NN", "№ п/п");
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            label19.Text = "Список выданных документов на " + DateTime.Now.ToShortDateString();
            label18.Text = "";
            //DataSet StatDS = dbw.GetIssuedBooks();
            Statistics.DataSource = dbw.GetIssuedBooks(); //StatDS.Tables[0];
            if (this.Statistics.Rows.Count == 0)
            {
                this.Statistics.Columns.Clear();
                MessageBox.Show("Нет выданных книг!");
                return;
            }

            autoinc(Statistics);
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 280;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[4].HeaderText = "Спрашиваемость";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[4].Visible = false;
            Statistics.Columns[3].Visible = false;
            Statistics.Columns[5].Visible = false;
            Statistics.Columns[6].Visible = false;
            Statistics.Columns[7].HeaderText = "Номер читате льского билета";
            Statistics.Columns[7].Width = 70;
            Statistics.Columns[8].HeaderText = "ФИО";
            Statistics.Columns[8].Width = 100;
            Statistics.Columns[9].HeaderText = "Тип абонемента";
            Statistics.Columns[9].Width = 100;
            Statistics.Columns[10].HeaderText = "Дата выдачи";
            Statistics.Columns[10].CellTemplate.ValueType = typeof(DateTime);
            Statistics.Columns[10].ValueType = typeof(DateTime);
            Statistics.Columns[10].Width = 85;
            Statistics.Columns[11].HeaderText = "Предпо лагаемая дата возврата";
            Statistics.Columns[11].CellTemplate.ValueType = typeof(DateTime);
            Statistics.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[10].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[11].ValueType = typeof(DateTime);
            Statistics.Columns[11].Width = 85;

            Sorting.WhatStat = Stats.IssuedBooks;
            Sorting.AuthorSort = SortDir.None;
            Sorting.ZagSort = SortDir.None;

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            label19.Text = "Количество читателей, за период с" + f3.StartDate.ToString("yyyyMMdd") + " по " + f3.EndDate.ToString("yyyyMMdd") ;
            label18.Text = dbw.GetReaderCount(f3.StartDate, f3.EndDate);
        }


        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            //label17.Text = "Количество выданных документов, за период с" + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy") + ": " + dbw.GetBooksCount(f3.StartDate, f3.EndDate);
            label19.Text = "Количество выданных документов, за период с" + f3.StartDate.ToString("yyyyMMdd") + " по " + f3.EndDate.ToString("yyyyMMdd") ;
            label18.Text = dbw.GetBooksCount(f3.StartDate, f3.EndDate);
        }
        public void autoinc(DataGridView dgv)
        {
            //listBox1.end
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
            //Statistics.Rows[Statistics.Rows.Count - 1].Cells[0].Value = "";
        }
        

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.Columns.Add("NN", "№ п/п");
            label19.Text = "Список всех документов, находящихся в наличии в фонде на " + DateTime.Now.ToShortDateString();
            label18.Text = "";
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            backgroundWorker1.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            tabControl1.TabPages[1].Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(450, 200);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            //int p1 = 0;
            //int p2 = 0;
            //Action<int>
            //backgroundWorker2.RunWorkerAsync();
            

            /*progressBar1.Invoke(delegate()
            {
                progressBar1.Value = p1;
            });
            /*delegate()
            {
                while (p1 != 100)
                {
                    p1++;
                    Thread.CurrentThread.Join(1000);

                    progressBar1.Invoke((ThreadStart)delegate()
                    {
                        progressBar1.Value = p1;
                    });
                }
            };*/

            //------------------------------------------------------

            /*new Thread(delegate()
            {
                while (p1 != 100)
                {
                    p1++;
                    Thread.CurrentThread.Join(1000);

                    progressBar1.Invoke((ThreadStart)delegate()
                    {
                        progressBar1.Value = p1;
                    });
                }
            }).Start();*/

            
            //backgroundWorker2.RunWorkerAsync(backgroundWorker1.IsBusy);
            //Statistics.DataSource = dbw.GetAllBooks();
            /*autoinc(Statistics);
            Statistics.Columns[0].Width = 50;
            Statistics.Columns[1].HeaderText = "Номер полки";
            Statistics.Columns[1].Width = 140;
            Statistics.Columns[2].HeaderText = "Штрихкод";
            Statistics.Columns[2].Visible = false;
            Statistics.Columns[3].HeaderText = "Заглавие";
            Statistics.Columns[3].Width = 330;
            Statistics.Columns[4].HeaderText = "Автор";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[5].HeaderText = "Год издания";
            Statistics.Columns[5].Width = 70;
            Statistics.Columns[6].HeaderText = "Спрашива емость";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].Visible = false;
            Statistics.Columns[8].Visible = false;
            Statistics.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.Columns[9].HeaderText = "Выдача";
            Statistics.Columns[9].Width = 100;
            Sorting.WhatStat = Stats.AllBooks;
            Sorting.AuthorSort = SortDir.None;
            Sorting.ZagSort = SortDir.None;
            //Statistics.
            //Statistics.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
            //Statistics.Columns[2].SortMode = DataGridViewColumnSortMode.;
            button12.Enabled = true;*/
        }
        //public static event HeaderClick eHeaderClick;

        public void FireHeaderClick(object sender, DataGridViewCellMouseEventArgs ev)
        {
            autoinc(Statistics);

        }
        public enum Stats { Debtors, AllBooks, IssuedBooks, Formular };
        public enum SortDir { Asc, Desc, None };
        class Sorting
        {
            private static SortDir authorSort;
            public static SortDir AuthorSort
            {
                get { return authorSort; }
                set { authorSort = value; }
            }
            private static SortDir zagSort;
            public static SortDir ZagSort
            {
                get { return zagSort; }
                set { zagSort = value; }
            }
            private static Stats whatStat;
            public static Stats WhatStat
            {
                get { return whatStat; }
                set { whatStat = value; }
            }
            private static bool sortOrd;
            public static bool SortOrd
            {
                get { return sortOrd; }
                set { sortOrd = value; }
            }


        }

        private void Statistics_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MouseEventArgs m = new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0);
            //DataGridViewCellMouseEventArgs ev = new DataGridViewCellMouseEventArgs(1, 0, 0, 0, m);
            //this.Statistics_ColumnHeaderMouseClick(Statistics, ev);// .FireHeaderClick(Statistics, ev);
            //Statistics.Columns[5].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
            DataGridView C1 = (DataGridView)sender;
            if (label18.Text.Contains("Количество действий"))
                return;
            if (label18.Text.Contains("Количество действий"))
                return;
            /*if (label19.Text.Contains("арушителе"))
            {
                autoinc(C1);
                return;
            }*/
            switch (Sorting.WhatStat)
            {
                case Stats.IssuedBooks:
                    if ((e.ColumnIndex == 1) && ((Sorting.ZagSort == SortDir.Asc)))
                    {
                        Statistics.Sort(Statistics.Columns[5], ListSortDirection.Descending);
                        Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        Sorting.ZagSort = SortDir.Desc;
                        //                        Statistics.SortOrder;
                    }
                    else
                        if ((e.ColumnIndex == 1) && ((Sorting.ZagSort == SortDir.Desc) || (Sorting.ZagSort == SortDir.None)))
                        {
                            Statistics.Sort(Statistics.Columns[5], ListSortDirection.Ascending);
                            Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            Sorting.ZagSort = SortDir.Asc;
                            if ((e.ColumnIndex == 2) && ((Sorting.AuthorSort == SortDir.Asc)))
                            {
                                Statistics.Sort(Statistics.Columns[6], ListSortDirection.Descending);
                                Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                                Sorting.AuthorSort = SortDir.Desc;
                            }
                            else
                                if ((e.ColumnIndex == 2) && ((Sorting.AuthorSort == SortDir.Desc) || (Sorting.AuthorSort == SortDir.None)))
                                {
                                    Statistics.Sort(Statistics.Columns[6], ListSortDirection.Ascending);
                                    Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                                    Sorting.AuthorSort = SortDir.Asc;
                                }
                        }

                    break;
                case Stats.AllBooks:
                    if ((e.ColumnIndex == 2) && ((Sorting.ZagSort == SortDir.Asc)))
                    {
                        Statistics.Sort(Statistics.Columns[6], ListSortDirection.Descending);
                        Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        Sorting.ZagSort = SortDir.Desc;
                        //                        Statistics.SortOrder;
                    }
                    else
                        if ((e.ColumnIndex == 2) && ((Sorting.ZagSort == SortDir.Desc) || (Sorting.ZagSort == SortDir.None)))
                        {
                            Statistics.Sort(Statistics.Columns[6], ListSortDirection.Ascending);
                            Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            Sorting.ZagSort = SortDir.Asc;
                            if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Asc)))
                            {
                                Statistics.Sort(Statistics.Columns[7], ListSortDirection.Descending);
                                Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                                Sorting.AuthorSort = SortDir.Desc;
                            }
                            else
                                if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Desc) || (Sorting.AuthorSort == SortDir.None)))
                                {
                                    Statistics.Sort(Statistics.Columns[7], ListSortDirection.Ascending);
                                    Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                                    Sorting.AuthorSort = SortDir.Asc;
                                }
                        }
                    break;
                case Stats.Debtors:
                    if ((e.ColumnIndex == 2) && ((Sorting.ZagSort == SortDir.Asc)))
                    {
                        Statistics.Sort(Statistics.Columns[5], ListSortDirection.Descending);
                        Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        Sorting.ZagSort = SortDir.Desc;
                    }
                    else
                        if ((e.ColumnIndex == 2) && ((Sorting.ZagSort == SortDir.Desc) || (Sorting.ZagSort == SortDir.None)))
                        {
                            Statistics.Sort(Statistics.Columns[5], ListSortDirection.Ascending);
                            Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            Sorting.ZagSort = SortDir.Asc;
                            if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Asc)))
                            {
                                Statistics.Sort(Statistics.Columns[6], ListSortDirection.Descending);
                                Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                                Sorting.AuthorSort = SortDir.Desc;
                            }
                            else
                                if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Desc) || (Sorting.AuthorSort == SortDir.None)))
                                {
                                    Statistics.Sort(Statistics.Columns[6], ListSortDirection.Ascending);
                                    Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                                    Sorting.AuthorSort = SortDir.Asc;
                                }
                        }

                    break;
                case Stats.Formular:
                    if ((e.ColumnIndex == 6) && ((Sorting.ZagSort == SortDir.Asc)))
                    {
                        Statistics.Sort(Statistics.Columns[8], ListSortDirection.Descending);
                        Statistics.Columns[6].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        Sorting.ZagSort = SortDir.Desc;
                    }
                    else
                        if ((e.ColumnIndex == 6) && ((Sorting.ZagSort == SortDir.Desc) || (Sorting.ZagSort == SortDir.None)))
                        {
                            Statistics.Sort(Statistics.Columns[8], ListSortDirection.Ascending);
                            Statistics.Columns[6].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            Sorting.ZagSort = SortDir.Asc;
                            if ((e.ColumnIndex == 7) && ((Sorting.AuthorSort == SortDir.Asc)))
                            {
                                Statistics.Sort(Statistics.Columns[9], ListSortDirection.Descending);
                                Statistics.Columns[7].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                                Sorting.AuthorSort = SortDir.Desc;
                            }
                            else
                                if ((e.ColumnIndex == 7) && ((Sorting.AuthorSort == SortDir.Desc) || (Sorting.AuthorSort == SortDir.None)))
                                {
                                    Statistics.Sort(Statistics.Columns[9], ListSortDirection.Ascending);
                                    Statistics.Columns[7].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                                    Sorting.AuthorSort = SortDir.Asc;
                                }
                        }

                    break;
            }

            
            /*if (e.ColumnIndex == 1)
            {
                AscDescZag = !AscDescZag;
                Statistics.Sort(Statistics.Columns[5], AscDescZag ? ListSortDirection.Ascending : ListSortDirection.Descending);
                //Statistics.SortOrder = !!!!!!!
            }
            if (e.ColumnIndex == 2)
            {
                AscDescAvt = !AscDescAvt;
                Statistics.Sort(Statistics.Columns[6], AscDescAvt ? ListSortDirection.Ascending : ListSortDirection.Descending);
            }*/
            if (label19.Text.Contains("нарушит"))
                foreach (DataGridViewRow r in Statistics.Rows)
                {
                    if (r.Cells[6].Value.ToString() == "true")
                    {
                        r.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            autoinc(Statistics);

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            button8.Visible = false;
            //timer2.Enabled = false;
            label16.Visible = false;
            //tabControl1.Enabled = true;
            groupBox4.Enabled = true;
        }

        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {

            //e.Graphics.FillRectangle(new SolidBrush(Color.SteelBlue), e.Bounds);
            e.DrawBackground();
            e.DrawBorder();
            TextRenderer.DrawText(e.Graphics, "red", e.Font, e.Bounds, Color.Red);
            //            e.DrawText();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {


        }

        private void button10_Click(object sender, EventArgs e)
        {
            //dbw.GetFormular("1000001");

            if (this.numericUpDown3.Value.ToString() == "")
            {
                MessageBox.Show("Введите номер читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (this.numericUpDown3.Value <= 0)
            {
                MessageBox.Show("Введите число больше нуля", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            ReaderSetBarcode = new DBWork.dbReader((int)numericUpDown3.Value);
            if (ReaderSetBarcode.barcode == "error")
            {
                MessageBox.Show("Читатель не найден!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            dbw.GetFormular(ReaderSetBarcode.id);
            label20.Text = ReaderSetBarcode.Surname + " " + ReaderSetBarcode.Name + " " + ReaderSetBarcode.SecondName;
            textBox6.Text = ReaderSetBarcode.AbonType;
            label25.Text = ReaderSetBarcode.id;
            pictureBox2.Image = ReaderSetBarcode.Photo;
            this.FormularColumnsForming(ReaderSetBarcode.id);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Сначала найдите читателя", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            f5 = new Form5(label25.Text);

            f5.ShowDialog();
            if (f5.abon != null)
                textBox6.Text = f5.abon;
            /*ReaderRecordFormular = new DBWork.dbReader("1000001");
            dbw.SetPenalty(ReaderRecordFormular.id);
            Formular.DataSource = dbw.GetFormular(ReaderRecordFormular.id);
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Заглавие";
            Formular.Columns.Add(col);
            col.DataPropertyName = "Zag";
            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Заглавие";
            Formular.Columns.Add(col);
            col.DataPropertyName = "issue";
            DataGridViewCheckBoxColumn colch = new DataGridViewCheckBoxColumn();
            colch.HeaderText = "Нарушение";
            colch.Name = "pen";
            Formular.Columns.Add(colch);
            Formular.Columns["pen"].ReadOnly = false;
            colch.DataPropertyName = "penalty";

            DataGridViewDisableButtonColumn ButCol = new DataGridViewDisableButtonColumn();
            Formular.Columns.Add(ButCol);
            ButCol.Name = "but";
            ButCol.HeaderText = "Снятие нарушения";
            ButCol.DefaultCellStyle.BackColor = Form1.DefaultBackColor;
            Padding myPadd = ButCol.DefaultCellStyle.Padding;
            myPadd.All = 2;
            ButCol.DefaultCellStyle.Padding = myPadd;
            foreach (DataGridViewRow row in Formular.Rows)
            {
                DataGridViewDisableButtonCell bc = (DataGridViewDisableButtonCell)row.Cells["but"]; ;
                if ((row.Cells["pen"].Value.ToString().ToLower() == "false") && (row.Cells["rempen"].Value.ToString().ToLower() == "true"))
                {
                    bc.Value = "Нет нарушения";//ранее сняли
                    bc.Enabled = false;
                }


            }

           // Formular.DataMember = "form";*/
        }

        private void Formular_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (Formular.Columns[e.ColumnIndex].Name)
            {
                case "but":
                    if (e.RowIndex == -1) break;
                    if (((DataGridViewDisableButtonCell)Formular.Rows[e.RowIndex].Cells["but"]).Value.ToString() == "Снять нарушение")
                    {
                        switch (MessageBox.Show("Вы уверены что хотите снять нарушение? После подтверждения книга исчезнет из этого списка, т.к. она возвращена и сейчас снимается нарушение.", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            case DialogResult.Yes:
                                this.dbw.RemPenalty(this.Formular.Rows[e.RowIndex].Cells["zid"].Value.ToString());
                                this.Formular.Rows.RemoveAt(e.RowIndex);
                                return;
                                //break;
                            case DialogResult.No:
                                return;
                                //break;
                        }
                    }
                        
                    f4 = new Form4();
                    f4.ShowDialog();
                    if (f4.Days == -99)
                        return;
                    if (!dbw.Prolong(f4.Days,Formular.Rows[e.RowIndex].Cells["idmain"].Value.ToString()))
                    {
                        Formular.Rows[e.RowIndex].Cells["pen"].Value = false;
                        //Formular.Rows[e.RowIndex].Cells["pen"].ReadOnly = true;
                        ((DataGridViewDisableButtonCell)Formular.Rows[e.RowIndex].Cells["but"]).Enabled = true;
                        ((DataGridViewDisableButtonCell)Formular.Rows[e.RowIndex].Cells["but"]).Value = "Продлить";
                    }
                    dbw.InsertActionProlong(new DBWork.dbReader(int.Parse(label25.Text)),new DBWork.dbBook(Formular.Rows[e.RowIndex].Cells["BAR"].Value.ToString()));
                    Formular.Rows[e.RowIndex].Cells["vozv"].Value = DateTime.Parse(Formular.Rows[e.RowIndex].Cells["vozv"].Value.ToString()).AddDays(f4.Days);
                    return;
                    //break;
                case "pen":
                    if (e.RowIndex == -1) break;
                    if (Formular.Rows[e.RowIndex].Cells["pen"].Value.ToString().ToLower() == "true")
                    {
                        if (Formular.Rows[e.RowIndex].Cells["fact"].Value.ToString() == "")
                        {
                            MessageBox.Show("Вы не можете снять нарушение вручную, т.к. книга еще не возвращена! Нарушение снимается при продлении срока.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            //switch (MessageBox.Show("Книга еще не возвращена. Вы действительно хотите снять нарушение? ", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                            //{
                            //    case DialogResult.Yes:
                            //        dbw.RemPenalty(Formular.Rows[e.RowIndex].Cells["idmain"].Value.ToString());
                            //        Formular.Rows[e.RowIndex].Cells["pen"].Value = false;
                            //        ((DataGridViewDisableButtonCell)Formular.Rows[e.RowIndex].Cells["but"]).Enabled = false;
                            //        ((DataGridViewDisableButtonCell)Formular.Rows[e.RowIndex].Cells["but"]).Value = "Нет нарушения";
                            //        break;
                            //    case DialogResult.No:
                            //        return;
                            //        //break;
                            //}
                        }
                        else
                        {
                            MessageBox.Show("Чтобы снять нарушение нажмите на кнопку \"Снять нарушение\"", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нельзя установить нарушение вручную: оно устанавливается автоматически.");
                        //Formular.Rows[e.RowIndex].Cells["pen"].Value = false;
                    }
                    break;
                
            }
        }

        private void Formular_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*switch (Formular.Columns[e.ColumnIndex].Name)
            {
                
            }*/
        }

        private void Formular_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 1) && ((Sorting.ZagSort == SortDir.Asc)))
            {
                Formular.Sort(Formular.Columns[2], ListSortDirection.Descending);
                Formular.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                Sorting.ZagSort = SortDir.Desc;
                //                        Statistics.SortOrder;
            }
            else
                if ((e.ColumnIndex == 1) && ((Sorting.ZagSort == SortDir.Desc) || (Sorting.ZagSort == SortDir.None)))
                {
                    Formular.Sort(Formular.Columns[2], ListSortDirection.Ascending);
                    Formular.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                    Sorting.ZagSort = SortDir.Asc;
                    if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Asc)))
                    {
                        Formular.Sort(Formular.Columns[4], ListSortDirection.Descending);
                        Formular.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        Sorting.AuthorSort = SortDir.Desc;
                    }
                    else
                        if ((e.ColumnIndex == 3) && ((Sorting.AuthorSort == SortDir.Desc) || (Sorting.AuthorSort == SortDir.None)))
                        {
                            Formular.Sort(Formular.Columns[4], ListSortDirection.Ascending);
                            Formular.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                            Sorting.AuthorSort = SortDir.Asc;
                        }
                }
            autoinc(Formular);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (label25.Text == string.Empty)
            {
                MessageBox.Show("Читатель не выбран!");
                return;
            }
            if (Formular.Rows.Count == 0)
            {
                MessageBox.Show("За читателем не числится ни книг ни нарушений!");
                return;
            }
            LostBook lb = new LostBook(label25.Text, this);
            lb.ShowDialog();
        }
        System.Drawing.Printing.PrintDocument pd;
        DataGridViewPrinter prin;
        DataGridView dgw2;
        private bool SetupThePrinting()
        {
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;
            pd = new System.Drawing.Printing.PrintDocument();
            pd.DocumentName = "Сверка фонда";
            //pd.PrinterSettings = MyPrintDialog.PrinterSettings;
            pd.DefaultPageSettings = pd.PrinterSettings.DefaultPageSettings;
            pd.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            prin = new DataGridViewPrinter(dgw2, pd, true, false, string.Empty, new Font("Tahoma", 18), Color.Black, false);
            

            return true;
        }

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = prin.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (Statistics.Rows.Count == 0)
            {
                MessageBox.Show("Нечего экспортировать!");
                return;
            }
            string strExport = "";
            //Loop through all the columns in DataGridView to Set the 
            //Column Heading
            foreach (DataGridViewColumn dc in Statistics.Columns)
            {
                if (dc.Visible)
                {
                    strExport += dc.HeaderText + "  ; ";
                }
            }
            strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString();
            //Loop through all the row and append the value with 3 spaces
            foreach (DataGridViewRow dr in Statistics.Rows)
            {
                foreach (DataGridViewCell dc in dr.Cells)
                {
                    if (Statistics.Columns[dc.ColumnIndex].Visible)
                    {
                        if (dc.Value != null)
                        {
                            strExport += dc.FormattedValue.ToString().Replace(";","") + " ;  ";
                        }
                    }
                }
                strExport += Environment.NewLine.ToString();
            }
            strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString() + Environment.NewLine.ToString() + DateTime.Now.ToString("dd.MM.yyyy") + "  " + " - " + this.textBox1.Text;
            //Create a TextWrite object to write to file, select a file name with .csv extention
            string tmp = "D:\\" + label19.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            tmp = label19.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Сохранить в файл";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            TextWriter tw;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                tmp = sd.FileName;
                tw = new System.IO.StreamWriter(tmp, false, Encoding.UTF8);
                //Write the Text to file
                //tw.Encoding = Encoding.Unicode;
                tw.Write(strExport);
                //Close the Textwrite
                tw.Close();
            } 


#region старый код. печатался грид. сомнительный
            //распечатать выделенное
           /* if (!label19.Text.Contains("нарушител"))
            {
                dgw2 = new DataGridView();
                //DataGridViewRow[] arr = new DataGridViewRow[Statistics.SelectedRows.Count];
                DataGridViewColumn[] arr1 = new DataGridViewColumn[Statistics.Columns.Count];
                //Statistics.SelectedRows.CopyTo(arr, 0);
                Statistics.Columns.CopyTo(arr1, 0);
                dgw2.AutoGenerateColumns = false;
                dgw2.Columns.Clear();
                dgw2.Font = new Font("Times New Roman", 10);
                //dgw2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                //dgw2.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                //dgw2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgw2.AllowUserToAddRows = false;
                //dgw2.Columns.AddRange(arr1);
                foreach (DataGridViewColumn col in arr1)
                {
                    //dgw2.Columns.Add((DataGridViewColumn)col.Clone());
                    dgw2.Columns.Add("", "");
                }
                int i = 0;
                for (int ri = Statistics.SelectedRows.Count - 1; ri >= 0; ri--)
                {

                    dgw2.Rows.Add();// (DataGridViewRow)Statistics.SelectedRows[ri].Clone();
                    DataGridViewRow clonedRow = dgw2.Rows[i];
                    for (Int32 index = 0; index < Statistics.SelectedRows[ri].Cells.Count; index++)
                    {
                        dgw2.Rows[i].Cells[index].Value = Statistics.SelectedRows[ri].Cells[index].Value;
                    }
                    //dgw2.Rows.Add(clonedRow);
                    i++;
                }
                dgw2.AutoSize = false;
                dgw2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgw2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                //Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgw2.Columns[0].Width = 50;
                dgw2.Columns[1].Width = 70;
                dgw2.Columns[2].Width = 100;
                dgw2.Columns[2].Visible = true;

                dgw2.Columns[3].Width = 500;
                dgw2.Columns[4].Width = 190;
                dgw2.Columns[5].Width = 70;
                dgw2.Columns[6].Width = 40;
                dgw2.Columns[7].Visible = false;
                dgw2.Columns[8].Visible = false;
                //Statistics.Columns[7].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgw2.Columns[9].Width = 80;
                dgw2.Columns[10].Visible = false;
                autoinc(dgw2);
            }
            else
            {
                dgw2 = new DataGridView();
                DataGridViewColumn[] arr1 = new DataGridViewColumn[Statistics.Columns.Count];
                Statistics.Columns.CopyTo(arr1, 0);
                dgw2.AutoGenerateColumns = false;
                dgw2.Columns.Clear();
                dgw2.Font = new Font("Times New Roman", 10);
                dgw2.AllowUserToAddRows = false;
                foreach (DataGridViewColumn col in arr1)
                {
                    dgw2.Columns.Add("", "");
                }
                int i = 0;
                for (int ri = Statistics.SelectedRows.Count - 1; ri >= 0; ri--)
                {

                    dgw2.Rows.Add();
                    DataGridViewRow clonedRow = dgw2.Rows[i];
                    for (Int32 index = 0; index < Statistics.SelectedRows[ri].Cells.Count; index++)
                    {
                        dgw2.Rows[i].Cells[index].Value = Statistics.SelectedRows[ri].Cells[index].Value;
                    }
                    i++;
                }
                dgw2.AutoSize = false;
                dgw2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgw2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgw2.Columns[0].Width = 50;
                dgw2.Columns[1].Width = 70;
                dgw2.Columns[2].Width = 70;
                dgw2.Columns[3].Width = 90;
                dgw2.Columns[4].Width = 90;
                dgw2.Columns[5].Width = 90;
                dgw2.Columns[6].Width = 300;
                dgw2.Columns[7].Width = 120;
                dgw2.Columns[8].Visible = false;
                dgw2.Columns[9].Visible = false;
                dgw2.Columns[10].Width = 110;
                autoinc(dgw2);
            }
            //Form tst = new Form();
            //dgw2.Width = 1270;
            //tst.Controls.Add(dgw2);
            //tst.Show();
            //tst.Width = 1500;
            //string str = dgw2.Rows[0].Cells[2].Value.ToString();
            if (SetupThePrinting())
                pd.Print();*/
            
        #endregion
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Читатель не выбран! Сначала выберите читателя!");
                return;
            }
            DBWork.dbReader reader = new DBWork.dbReader(int.Parse(label25.Text));
            
            Conn.OleDA.SelectCommand.CommandText = "select * from Main where";

            //DataSet ds = dbw.GetFormular("149921");
            //int i = ds.Tables.Count;
            //CrystalReport11.SetDataSource(dbw.GetFormular("149921"));
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = reader.id;
            Conn.SQLDA.SelectCommand.CommandText = " select " +
            "max(case when tmp.mnf = 200 then pl end) as Zag, " +
            "max(case when tmp.mnf = 200 then srt end) as zag_sort, " +
            "max(case when tmp.mnf = 700 then pl end) as avt, " +
            "max(case when tmp.mnf = 700 then srt end) as avt_sort, " +
            "max(case when tmp.mnf = 2100 then pl end) as god, " +
            "max(case when tmp.mnf = 200 then pl end) as mesto, " +
            "max(case when tmp.mnf = 200 then idm end) as idmain, " +
            "max(case when tmp.mnf = 200 then iss end) as issue, " +
            "max(case when tmp.mnf = 200 then vozv end) as vozv, " +
            "max(case when tmp.mnf = 200 then fct end) as fact, " +
            "max(case when tmp.mnf = 200 then zakid end) as zkid, " +
            "max(case when tmp.mnf = 200 then zi end) as zid, " +
            "((case when (tmp.pnlt = 'false' or tmp.pnlt is NULL) then 'false' else 'true' end)) as penalty, " +
            "((case when (tmp.rempnlt = 'false' or tmp.rempnlt is NULL) then 'false' else 'true' end)) as rempenalty " +
            "from " +
            "(select Z.ID as zi,Z.IDMAIN as zakid, Z.DATE_ISSUE as iss, Z.DATE_VOZV as vozv, Z.DATE_FACT_VOZV as fct, Z.PENALTY as pnlt,Z.REMPENALTY as rempnlt, X.IDMAIN as idm, X.PLAIN as pl, Y.SORT as srt, Y.MNFIELD as mnf " +
            "from BRIT_SOVET..DATAEXTPLAIN X " +
            "join BRIT_SOVET..DATAEXT Y on Y.ID=X.IDDATAEXT " +
            "join BRIT_SOVET..ZAKAZ Z on ((Z.IDMAIN = Y.IDMAIN) or (Z.IDMAIN_CONST=Y.IDMAIN and Z.PENALTY='true')) " +
                //"--join BRIT_SOVET..ZAKAZ ZZ on Z.IDMAIN = ZZ.IDMAIN_CONST "+
            "where (((Y.MNFIELD = 200 and Y.MSFIELD = '$a') " +
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 700) " +
            "or (Y.MSFIELD = '$d' and Y.MNFIELD = 2100) " +
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 210)) and (Z.IDREADER = @IDR) and (( ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='true')) or ((Z.IDMAIN=0)and(Z.PENALTY='true')) or ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='false'))  ) )) " +
            "group by Z.ID, Z.IDMAIN, X.PLAIN, Y.SORT, Y.MNFIELD, X.IDMAIN,Z.DATE_ISSUE,Z.DATE_VOZV, Z.DATE_FACT_VOZV,Z.PENALTY, Z.REMPENALTY " +
            ") as tmp " +
            "group by idm,pnlt,rempnlt ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            R.Tables.Add("form");

            int i = Conn.SQLDA.Fill(R.Tables["form"]);
            CrystalReport1 cr1 = new CrystalReport1();
            cr1.SetDataSource(R.Tables["form"]);
            crystalReportViewer1.ReportSource = cr1;

            CrystalDecisions.CrystalReports.Engine.TextObject txtReaderName;
            CrystalDecisions.CrystalReports.Engine.TextObject txtReaderNum;
            txtReaderName = cr1.ReportDefinition.ReportObjects["Text19"] as TextObject;
            txtReaderNum = cr1.ReportDefinition.ReportObjects["Text20"] as TextObject;

            txtReaderName.Text = reader.Surname + " " + reader.Name + " " + reader.SecondName;
            txtReaderNum.Text = reader.id;
            //crystalReportViewer1.PrintReport();
            cr1.PrintToPrinter(1, false, 0, 0);
            
        }

        private void reportDocument1_InitReport(object sender, EventArgs e)
        {

        }

        private void crystalReport11_InitReport(object sender, EventArgs e)
        {

        }

        private void CrystalReport11_InitReport_1(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = dbw.GetAllBooks();
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RndPrg.Dispose();
            Statistics.DataSource = e.Result;
            autoinc(Statistics);
            Statistics.Columns[0].Width = 50;
            Statistics.Columns[1].HeaderText = "Местонахождение стеллажа";
            Statistics.Columns[1].Width = 140;
            Statistics.Columns[2].HeaderText = "Штрихкод";
            //Statistics.Columns[2].Visible = false;
            Statistics.Columns[3].HeaderText = "Заглавие";
            Statistics.Columns[3].Width = 330;
            Statistics.Columns[4].HeaderText = "Автор";
            Statistics.Columns[4].Width = 150;
            Statistics.Columns[5].HeaderText = "Год издания";
            Statistics.Columns[5].Width = 70;
            Statistics.Columns[6].HeaderText = "Спрашива емость";
            Statistics.Columns[6].Width = 80;
            Statistics.Columns[7].Visible = false;
            Statistics.Columns[8].Visible = false;
            Statistics.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //Statistics.Columns[9].HeaderText = "Выдача";
            //Statistics.Columns[9].Width = 100;
            Sorting.WhatStat = Stats.AllBooks;
            Sorting.AuthorSort = SortDir.None;
            Sorting.ZagSort = SortDir.None;
            //Statistics.
            //Statistics.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
            //Statistics.Columns[2].SortMode = DataGridViewColumnSortMode.;
            button12.Enabled = true;
           // backgroundWorker2.CancelAsync();
        }


        void RunProgressBar()
        {
            if (progressBar1.Value == 100)
                progressBar1.Value = 0;
            progressBar1.Value += 1;
        }
        delegate void pbrun();

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            
            /*Action dlg = delegate()
            {
                if (progressBar1.Value == 100)
                    progressBar1.Value = 0;
                progressBar1.Value += 1;
            };
            while (backgroundWorker1.IsBusy)
            {
                //Thread.CurrentThread.Join(500);
                Thread.Sleep(500);
                this.Invoke(dlg);
            }*/
            Action delegProgress = delegate()
            {
                RndPrg = new ExtGui.RoundProgress();
                RndPrg.Visible = true;
                RndPrg.Name = "progress";
                tabControl1.TabPages[1].Controls.Add(RndPrg);
                RndPrg.BringToFront();
                RndPrg.Size = new Size(40, 60);
                RndPrg.Location = new Point(450, 200);
                RndPrg.BackColor = SystemColors.AppWorkspace;
            };
            this.Invoke(delegProgress);
            
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           // progressBar1.Value = e.ProgressPercentage;
           // if (progressBar1.Value == 100)
           //     progressBar1.Value = 0;

            string str = "dsdddddddddd";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            SendEmail em = new SendEmail(this, Formular, label25.Text);
            if (em.canshow)
                em.ShowDialog();

        }

        private void индивидуальнаяСтатистиказаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Columns != null)
                Statistics.Columns.Clear();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            label18.Text = "";
            label19.Text = "";
            label19.Text = "Количество действий оператора за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            try
            {
                Statistics.DataSource = dbw.GetActions(f3.StartDate, f3.EndDate, this.EmpID);//StatDS.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 70;
            Statistics.Columns[1].HeaderText = "Действие";
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].Width = 300;
            autoinc(Statistics);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            

        }
        string emul;
        private void button15_Click_1(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.ShowDialog();
            this.emul = f7.str;
            this.Form1_Scanned(sender, e);
            this.emul = "";

        }

        private void Statistics_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //DBWork.dbBook book = new DBWork.dbBook(textBox7.Text);
            //DBWork.dbReader reader = new DBWork.dbReader(int.Parse(textBox8.Text));
            //dbw.setBookForReader(book,reader,30);

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null) return;
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(pictureBox2.Image);
            fullsize.ShowDialog();
        }

        private void RPhoto_Click(object sender, EventArgs e)
        {
            if (RPhoto.Image == null) return;
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(RPhoto.Image);
            fullsize.ShowDialog();
        }

    }
    //public class _BarcScan
    //{
    //   // public OPOSScannerClass Scanner;
    //    //public event EventHandler Scanned;
    //    public Form1 F1;
    //    public _BarcScan(Form1 _F1)
    //    {

    //        F1 = _F1;
    //        //SqlConnectionStringBuilder builder =
    //        //new SqlConnectionStringBuilder(();
    //        //CrystalReport1 cr = new CrystalReport1();
    //        //cr.SetDataSource(
    //        try
    //        {
    //            this.Scanner = new OPOSScannerClass();
    //            this.Scanner.ErrorEvent += new _IOPOSScannerEvents_ErrorEventEventHandler(Scanner_ErrorEvent);
    //            this.Scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(Scanner_DataEvent);
    //            this.Scanner.Open("STI_SCANNER");
    //            //MessageBox.Show("1");
    //            ResultCodeH.Check(this.Scanner.ClaimDevice(7000));
    //            //MessageBox.Show("2");
    //            this.Scanner.DeviceEnabled = true;
    //            ResultCodeH.Check(this.Scanner.ResultCode);
    //            //MessageBox.Show("3");
    //            this.Scanner.AutoDisable = true;

    //            ResultCodeH.Check(this.Scanner.ResultCode);
    //            //MessageBox.Show("4");
    //            this.Scanner.DataEventEnabled = true;
    //            ResultCodeH.Check(this.Scanner.ResultCode);
    //            //MessageBox.Show("5");
    //        }
    //        catch (Exception _e)
    //        {
    //            MessageBox.Show(_e.Message.ToString());
    //        }
    //    }
    //    void Scanner_DataEvent(int Status)
    //    {
    //        this.Scanner.DeviceEnabled = true;
    //        this.Scanner.DataEventEnabled = true;
    //        F1.FireScan(this.Scanner, EventArgs.Empty);
    //    }

    //    void Scanner_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
    //    {
    //        pErrorResponse = (int)OPOS_Constants.OPOS_ER_CLEAR;
    //        MessageBox.Show(ResultCodeH.Message(ResultCode));
    //        this.Scanner.DeviceEnabled = true;
    //        this.Scanner.DataEventEnabled = true;
    //    }
    //}
    public static class Conn
    {
        public static SqlConnection ReadersCon;
        public static SqlConnection ZakazCon;
        public static SqlConnection BRIT_SOVETCon;
        public static SqlDataAdapter OleDA;
        public static SqlDataAdapter SQLDA;
    }
    public class DBWork
    {
        private DataSet ReaderMain;
        private DataSet Book;
        private DataSet Zakaz;
        Form1 F1;
        public DBWork()
        {
            XmlConnections xml = new XmlConnections();
            Conn.ReadersCon = new SqlConnection(xml.GetReaderCon());// ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Z:\\progs\\Circulation\\Readers.mdb");
            Conn.BRIT_SOVETCon = new SqlConnection(xml.GetBRIT_SOVETCon());// ("Data Source=192.168.3.241;Initial Catalog=BRIT_SOVET;Integrated Security=True");
            Conn.ZakazCon = new SqlConnection(xml.GetZakazCon());//("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG;Integrated Security=True");
            Conn.OleDA = new SqlDataAdapter();
            Conn.OleDA.SelectCommand = new SqlCommand("select * from main where BarCode = 19", Conn.ReadersCon);
            Conn.OleDA.SelectCommand.Connection.Open();
            Conn.SQLDA = new SqlDataAdapter();
            Conn.SQLDA.SelectCommand = new SqlCommand("select * from BARCODE_UNITS where ID = 0", Conn.BRIT_SOVETCon);
            Conn.SQLDA.SelectCommand.Connection.Open();
            Conn.SQLDA.SelectCommand.Parameters.Add("@IDR", SqlDbType.NVarChar);
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = "0";

            Book = new DataSet();
            ReaderMain = new DataSet();
            Zakaz = new DataSet();
        }
        public DBWork(Form1 f1)
        {
            F1 = f1;
            XmlConnections xml = new XmlConnections();
            Conn.ReadersCon = new SqlConnection(xml.GetReaderCon());// ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Z:\\progs\\Circulation\\Readers.mdb");
            Conn.BRIT_SOVETCon = new SqlConnection(xml.GetBRIT_SOVETCon());// ("Data Source=192.168.3.241;Initial Catalog=BRIT_SOVET;Integrated Security=True");
            Conn.ZakazCon = new SqlConnection(xml.GetZakazCon());//("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG;Integrated Security=True");
            Conn.OleDA = new SqlDataAdapter();
            Conn.OleDA.SelectCommand = new SqlCommand("select * from main where BarCode = 19", Conn.ReadersCon);
            Conn.OleDA.SelectCommand.Connection.Open();
            Conn.SQLDA = new SqlDataAdapter();
            Conn.SQLDA.SelectCommand = new SqlCommand("select * from BARCODE_UNITS where ID = 0", Conn.BRIT_SOVETCon);
            Conn.SQLDA.SelectCommand.Connection.Open();
            Conn.SQLDA.SelectCommand.Parameters.Add("@IDR", SqlDbType.NVarChar);
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = "0";
            Book = new DataSet();
            ReaderMain = new DataSet();
            Zakaz = new DataSet();
            //DR = new OleDbDataReader();
        }
        public DataSet getBooksForReader(string reader)
        {
            Conn.SQLDA.SelectCommand.CommandText = "WITH FC AS (SELECT dt.ID,dt.SORT, "+
                                                                        "dt.MNFIELD, " +
                                                                        "dt.MSFIELD, " +
                                                                        "dt.IDMAIN, " +
                                                                        "dtp.PLAIN " +
					  
                                                                   "FROM   BRIT_SOVET..DATAEXT dt "+
                                                                          "JOIN BRIT_SOVET..DATAEXTPLAIN dtp "+
                                                                          "     ON  dt.ID = dtp.IDDATAEXT) "+

                                                    "select COL1.PLAIN zag,dtpa.PLAIN avt,Z.IDREADER,Z.IDMAIN from FC COL1 "+
                                                     "left join FC dtpa ON COL1.IDMAIN = dtpa.IDMAIN and dtpa.MNFIELD = 700 and dtpa.MSFIELD = '$a' "+
                                                     "left join BRIT_SOVET..ZAKAZ Z on Z.IDMAIN = COL1.IDMAIN "+
                                                     "where COL1.MNFIELD = 200 and COL1.MSFIELD = '$a' and Z.IDREADER =  " + reader +
                                                    "and Z.IDMAIN != 0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Book.Clear();
            int i = Conn.SQLDA.Fill(Book, "booksonreader");
            return (i == 0) ? new DataSet() : Book;
        }
        public dbBook getBookFromZAKAZ(string s)
        {
            //s = s.Remove(s.Length - 1, 1);
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ where BAR = '" + s + "' and IDMAIN <> 0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Book.Clear();
            int i = Conn.SQLDA.Fill(Book, "zakbk");
            if (i != 0)
                return new dbBook(Book.Tables["zakbk"].Rows[0]["IDMAIN"].ToString(), Book.Tables["zakbk"].Rows[0]["BAR"].ToString(), "", Book.Tables["zakbk"].Rows[0]["IDREADER"].ToString());
            else
                return new dbBook();
        }

        public void setBookForReader(dbBook book, dbReader reader, int days)
        {
            //book = book.Remove(book.Length - 1, 1);
            //reader = reader.Remove(0, 1);
            //reader = reader.Remove(reader.Length - 1, 1);
            Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Zakaz = new DataSet();
            int i = 0;
            Conn.SQLDA.FillSchema(Zakaz, SchemaType.Mapped, "t");
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            Conn.SQLDA.InsertCommand = cmdBuilder.GetInsertCommand();
            DataRow row = Zakaz.Tables["t"].NewRow();
            row["IDMAIN"] = book.id;
            //book = book.Remove(book.Length - 1, 1);
            row["BAR"] = book.barcode;
            //row["DATE_VOZV"] = DateTime.Now.AddDays(days).ToShortDateString();
            row["DATE_VOZV"] = DateTime.Now.AddDays(days).ToShortDateString();
            row["IDREADER"] = reader.id;
            row["IDEMP"] = F1.EmpID;
            row["DATE_ISSUE"] = DateTime.Now.ToShortDateString();
            row["IDMAIN_CONST"] = book.id;
            row["PENALTY"] = false;
            row["REMPENALTY"] = false;
            Zakaz.Tables["t"].Rows.Add(row);
            //Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ where ID = -1";
            //Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //SQLDA.InsertCommand = cmdBuilder.GetInsertCommand();
            i = Conn.SQLDA.Update(Zakaz,"t");
        }
        public bool isRightsExpired(string s)
        {
            Conn.OleDA.SelectCommand.CommandText = "select * from ReaderRight where IDReader = " + s + " and IDReaderRight = 1";
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.OleDA);
            ReaderMain = new DataSet();
            Conn.OleDA.Fill(ReaderMain, "right");
            return ((DateTime)ReaderMain.Tables["right"].Rows[0]["DataEndReaderRight"] < DateTime.Now) ? true : false;
        }
        public void ProlongRights(string s)
        {
            Conn.OleDA.SelectCommand.CommandText = "select * from ReaderRight where IDReader = " + s + " and IDReaderRight = 1";
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.OleDA);
            ReaderMain = new DataSet();
            Conn.OleDA.UpdateCommand = cmd.GetUpdateCommand();
            Conn.OleDA.Fill(ReaderMain, "right");
            ReaderMain.Tables[0].Rows[0]["DataEndReaderRight"] = ((DateTime)ReaderMain.Tables[0].Rows[0]["DataEndReaderRight"]).AddYears(1);
            Conn.OleDA.Update(ReaderMain.Tables[0]);
        }
/*            class Class1
{
   bool  aviableToTakeABook ();
   bool isExpired ();
}

Class1 cl;

if (cl.aviableToTakeABook ())
   return;

string mes = cl.isExpired () ? "Продлить?" : "Назначить права?";

 Mor (13:31:57 12/08/2009)
Хотя и так, наверное, можно:

 Mor (13:37:05 12/08/2009)
class Rights {}
class TakeResult
{
   bool Expired;
   Rights Rights;
   bool Ok
}

class A
{
  TakeResul       TakeABook ();
}

A a;
TakeResult tr = a.TakeABook ();

if (tr.Ok)
   return;

if (tr.Expired)
  mes = "Продлить?"

if (tr.Right == null || tr.Rights == None)
  mes = "Права?"*/
        
        public void setReaderRight(string s)
        {
            Conn.OleDA.SelectCommand.CommandText = "select * from ReaderRight where IDReader = -1";
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.OleDA);
            ReaderMain = new DataSet();
            Conn.OleDA.Fill(ReaderMain, "right");
            Conn.OleDA.InsertCommand = cmd.GetInsertCommand();
            DataRow row = ReaderMain.Tables["right"].NewRow();
            row["IDReader"] = s;
            row["IDReaderRight"] = 1;
            row["DataEndReaderRight"] = DateTime.Now.AddYears(1);
            ReaderMain.Tables["right"].Rows.Add(row);

            Conn.OleDA.Update(ReaderMain, "right");
            Conn.OleDA.SelectCommand.CommandText = "select * from ReaderRight where IDReader = -1";
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;

        }
        public void setBookReturned(string s)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ where IDMAIN = '" + s + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            int i = Conn.SQLDA.Fill(B);
            B.Tables[0].Rows[0]["IDMAIN"] = "0";
            B.Tables[0].Rows[0]["DATE_FACT_VOZV"] = DateTime.Now.ToShortDateString();
            Conn.SQLDA.Update(B);

        }
        public void setBookLost(string s)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ where IDMAIN = '" + s + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            int i = Conn.SQLDA.Fill(B);
            B.Tables[0].Rows[0]["IDMAIN"] = "0";
            B.Tables[0].Rows[0]["DATE_FACT_VOZV"] = B.Tables[0].Rows[0]["DATE_ISSUE"];
            Conn.SQLDA.Update(B);

        }
        public bool isBookBusy(string s)
        {
            //s = s.Remove(s.Length - 1, 1);
            Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ where  BAR ='" + s + "' and IDMAIN <>0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //Book.Tables.Clear();
            DataSet Book = new DataSet();
            int i = Conn.SQLDA.Fill(Book);
            //string j = Book.Tables[0].Rows[0]["Creator"].ToString();
            return (i != 0) ? true : false;
        }
        public bool isReaderHaveRights(dbReader r)
        {
            //r = r.Remove(0, 1);
            //r = r.Remove(r.Length - 1, 1);
            CultureInfo ci = new CultureInfo("en-US");
            string date = DateTime.Now.ToString("d", ci);//SELECT ReaderRight.* FROM ReaderRight WHERE (((ReaderRight.DataEndReaderRight)=#10/20/2008#) AND ((ReaderRight.IDReader)=1) AND ((ReaderRight.IDReaderRight)=1));
            //Conn.OleDA.SelectCommand.CommandText = "SELECT ReaderRight.* FROM ReaderRight WHERE (((ReaderRight.DataEndReaderRight)>#" + date + "#) AND ((ReaderRight.IDReader)=" + r.id + ") AND ((ReaderRight.IDReaderRight)=1))";
            Conn.OleDA.SelectCommand.CommandText = "SELECT ReaderRight.* FROM ReaderRight WHERE ReaderRight.IDReader=" + r.id + " AND ReaderRight.IDReaderRight=1";
            //"select * from ReaderRight where IDReader = " + this.getDbReader(r).id + " and IDReaderRight = 1 and DateEndReaderRight > (#"+date +"#)";//больше текущей
            //int i = OleDA.Fill(ReaderMain, "dbr");
            DataSet R = new DataSet();
            return (Conn.OleDA.Fill(R) == 0) ? false : true;
        }
        public bool isReader(string s)
        {
            s = s.Remove(0, 1);
            //s = s.Remove(s.Length - 1, 1);
            return ((s.Length > 18) || (s.Length == 7)) ? true : false;
        }
        public dbReader getDbReader(string s)
        {
            //s = s.Remove(0, 1);
            //s = s.Remove(s.Length - 1, 1);
            if (s.Length < 19)
            {
                s = s.Remove(0, 1);
                //s = s.Remove(s.Length - 1, 1);
                Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where BarCode = " + s;
            }
            else
            {
                s = s.Remove(s.IndexOf(' '), s.Length - s.IndexOf(' '));
                Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where  NumberSC = '" + s + "'";
            }
            DataSet R = new DataSet();
            if (Conn.OleDA.Fill(R) != 0)
                return new dbReader(R.Tables[0].Rows[0][0].ToString(), R.Tables[0].Rows[0][1].ToString(), R.Tables[0].Rows[0][2].ToString() + " " + R.Tables[0].Rows[0][3].ToString().Remove(1, R.Tables[0].Rows[0][3].ToString().Length - 1) + ". " + R.Tables[0].Rows[0][4].ToString().Remove(1, R.Tables[0].Rows[0][4].ToString().Length - 1) + ".");
            else
                return new dbReader();
        }
        public dbBook getDbBook(string s)
        {
            //s = s.Remove(s.Length - 1, 1);
            Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, BARCODE from BARCODE_UNITS where BARCODE = '" + s + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
            //Book.Tables.Clear();
            DataSet B = new DataSet();
            int i = Conn.SQLDA.Fill(B);
            Conn.SQLDA.SelectCommand.CommandText = "select SORT from DATAEXT where IDMAIN = '" + B.Tables[0].Rows[0]["IDMAIN"].ToString() + "' and MNFIELD = '200' and MSFIELD = '$a'";
            Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
            DataSet Z = new DataSet();
            i = Conn.SQLDA.Fill(Z);
            //string j = Book.Tables[0].Rows[0]["Creator"].ToString();
            return new dbBook(B.Tables[0].Rows[0]["IDMAIN"].ToString(), B.Tables[0].Rows[0]["BARCODE"].ToString(), Z.Tables[0].Rows[0]["SORT"].ToString(), "");
        }
        public string GetDateRet(string s)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select DATE_VOZV from ZAKAZ where BARCODE = '" + s + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
            //Book.Tables.Clear();
            DataSet B = new DataSet();
            int i = Conn.SQLDA.Fill(B);
            return B.Tables[0].Rows[0]["DATE_VOZV"].ToString();
        }
        public int SetReaderBarCode(string ID, string barCode)
        {

            Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, NumberSC, FamilyName, Name, FatherName from main where BarCode = " + barCode.Remove(0, 1);
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;
            DataSet R = new DataSet();
            int i = 0;
            try
            {
                i = Conn.OleDA.Fill(R);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                //MessageBox.Show("Считан неверный штрихкод!");
                return -5;
            }
            if (i != 0)
                return -4;
            Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, NumberSC, FamilyName, Name, FatherName from main where NumberReader = " + ID;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.OleDA);
            R = new DataSet();
            i = 0;
            try
            {
                i = Conn.OleDA.Fill(R);
            }
            catch
            {
                return -1;
            }
            if (i == 0)
                return -2;
            if (R.Tables[0].Rows[0]["NumberSC"].ToString() != "")
                return -3;
            R.Tables[0].Rows[0]["BarCode"] = barCode.Remove(0, 1);
            Conn.OleDA.Update(R);
            return 1;
        }
        public class dbReader
        {
            public dbReader()
            {
                this.barcode = "";
                this.FIO = "";
                this.id = "";
            }
            public dbReader(int numberReader)
            {
                //Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + numberReader.ToString();
                Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType from main inner join AbonementType on main.AbonementType = AbonementType.IDAbonemetType where NumberReader = " + numberReader.ToString();
                Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;
                DataSet R = new DataSet();
                int i;
                try
                {
                    i = Conn.OleDA.Fill(R);
                }
                catch
                {
                    this.barcode = "error";
                    return;
                }
                if (i == 0)
                {
                    this.barcode = "error";
                    return;
                }
                this.Surname = R.Tables[0].Rows[0]["FamilyName"].ToString();
                this.Name = R.Tables[0].Rows[0]["Name"].ToString();
                this.SecondName = R.Tables[0].Rows[0]["FatherName"].ToString();
                string name = "";
                string secondName = "";
                try
                {
                    name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                }
                catch
                {
                    name = "";
                }
                try
                {
                    secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                }
                catch
                {
                    secondName = "";
                }
                this.FIO = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                this.id = R.Tables[0].Rows[0]["NumberReader"].ToString();
                this.barcode = R.Tables[0].Rows[0]["BarCode"].ToString();
                this.AbonType = R.Tables[0].Rows[0]["NameAbonType"].ToString();
                if (this.id != "")
                {
                    Conn.SQLDA.SelectCommand.CommandText = "select A.*,B.Photo fotka from Readers..Main A left join Readers..Photo B on A.NumberReader = B.IDReader where NumberReader = " + this.id;
                    DataSet DS = new DataSet();
                    i = Conn.SQLDA.Fill(DS, "reader");
                    if (i != 0)
                    {
                        if (DS.Tables["reader"].Rows[0]["fotka"] != DBNull.Value)
                        {

                            byte[] data = (byte[])DS.Tables["reader"].Rows[0]["fotka"];

                            if (data != null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    ms.Write(data, 0, data.Length);
                                    ms.Position = 0L;

                                    this.Photo = new Bitmap(ms);
                                }
                            }
                        }
                        else
                        {
                            this.Photo = Properties.Resources.nofoto;
                        }
                    }
                }
            }
            public dbReader(dbReader Reader)
            {
                this.barcode = Reader.barcode;
                this.FIO = Reader.FIO;
                this.id = Reader.id;
                this.Surname = Reader.Surname;
                this.Name = Reader.Name;
                this.SecondName = Reader.SecondName;
                this.AbonType = Reader.AbonType;
                this.Photo = Reader.Photo;
            }
            public dbReader Clone()
            {
                return new dbReader(this);
            }
            public dbReader(string id, string barcode, string FIO)
            {
                this.barcode = barcode;
                this.id = id;
                this.FIO = FIO;
            }
            public dbReader(string Bar)
            {
                if (Bar.Length > 18)
                {
                    if (Bar.Contains(" "))
                    {
                        Bar = Bar.Remove(19, 1);
                    } 
                    string Ser = Bar.Substring(19, 8);
                    Bar = Bar.Substring(0, 19);
                    //Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberSC = '" + Bar + "' and SerialSC = '" + Ser + "'";
                    Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType from main inner join AbonementType on main.AbonementType= AbonementType.IDAbonemetType where NumberSC = '" + Bar + "' and SerialSC = '" + Ser + "'";
                }
                else
                {
                    if (Bar[0].ToString() == "R")
                    {
                        Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType," +
                            " IDOldAbonement " +
                            " from Readers..Main " +
                            " inner join AbonementType on main.AbonementType = AbonementType.IDAbonemetType " +
                            " where BarCode = '" + Bar.Remove(0, 1) + "'";
                    }
                    else
                    {
                        Conn.OleDA.SelectCommand.CommandText =
                             " select A.NumberReader, A.BarCode, A.FamilyName, A.[Name], A.FatherName, " +
                             " A.AbonementType,B.NameAbonType, A.IDOldAbonement  " +
                             " from Readers..Main A " +
                             " inner join Readers..AbonementType B on A.AbonementType = B.IDAbonemetType  " +
                             " left join Readers..Input C on C.IDReaderInput = A.NumberReader " +
                             " where C.BarCodeInput = '" + Bar + "' and DateOutInput is null  order by C.IDInput desc";

                    }
                    /*Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName, " +
                        " AbonementType,NameAbonType from main " +
                        " inner join AbonementType on main.AbonementType = AbonementType.IDAbonemetType where BarCode =" + Bar;*/
                }

                DataSet R = new DataSet();
                int i;
                try
                {
                    i = Conn.OleDA.Fill(R);
                }
                catch
                {
                    this.barcode = "error";
                    return;
                }
                if (i == 0)
                {
                    this.barcode = "error";
                    return;
                }
                this.barcode = R.Tables[0].Rows[0]["BarCode"].ToString();
                this.id = R.Tables[0].Rows[0]["NumberReader"].ToString();
                string name = "";
                string secondName = "";
                try
                {
                    name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                }
                catch
                {
                    name = "";
                }
                try
                {
                    secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                }
                catch
                {
                    secondName = "";
                }
                this.FIO = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                this.AbonType = R.Tables[0].Rows[0]["NameAbonType"].ToString();
                this.Name = R.Tables[0].Rows[0]["Name"].ToString();
                this.Surname = R.Tables[0].Rows[0]["FamilyName"].ToString();
                this.SecondName = R.Tables[0].Rows[0]["FatherName"].ToString();
                if (this.id != "")
                {
                    Conn.SQLDA.SelectCommand.CommandText = "select A.*,B.Photo fotka from Readers..Main A left join Readers..Photo B on A.NumberReader = B.IDReader where NumberReader = " + this.id;
                    DataSet DS = new DataSet();
                    i = Conn.SQLDA.Fill(DS, "reader");
                    if (i != 0)
                    {
                        if (DS.Tables["reader"].Rows[0]["fotka"] != DBNull.Value)
                        {

                            byte[] data = (byte[])DS.Tables["reader"].Rows[0]["fotka"];

                            if (data != null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    ms.Write(data, 0, data.Length);
                                    ms.Position = 0L;

                                    this.Photo = new Bitmap(ms);
                                }
                            }
                        }
                        else
                        {
                            this.Photo = Properties.Resources.nofoto;
                        }
                    }
                }
            }
            public string barcode;
            public string id;
            public string FIO;
            public string Surname;
            public string Name;
            public string SecondName;
            public string AbonType;
            public Image Photo;

            public static bool IsValidEmail(string strIn)
            {
                // Return true if strIn is in valid e-mail format.
                return Regex.IsMatch(strIn,
                       @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            }
            internal string GetEmail()
            {
                Conn.SQLDA.SelectCommand.CommandText = "select Email from Readers..Main where NumberReader = " + this.id;
                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                DataSet D = new DataSet();
                int i = Conn.SQLDA.Fill(D);
                if (i == 0) return "";
                if (dbReader.IsValidEmail(D.Tables[0].Rows[0][0].ToString()))
                {
                    return D.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public class dbBook
        {
            public dbBook Clone()
            {
                return new dbBook(this);
            }
            public dbBook()
            {
                this.author = "";
                this.barcode = "";
                this.id = "";
                this.name = "";
                this.rname = "";
                this.rid = "";
            }

            public dbBook(dbBook Book)
            {
                this.author = Book.author;
                this.barcode = Book.barcode;
                this.id = Book.id;
                this.name = Book.name;
                this.rname = Book.rname;
                this.rid = Book.rid;
            }
            public dbBook(string id, string barcode, string name, string rname)
            {
                this.id = id;
                this.barcode = barcode;
                this.name = name;
                this.rname = rname;
                this.author = "";

            }
            public dbBook(string Bar)
            {
                Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, BARCODE from BARCODE_UNITS where BARCODE = '" + Bar + "'";
                Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                //Book.Tables.Clear();
                DataSet B = new DataSet();
                int i = Conn.SQLDA.Fill(B);
                if (i == 0)
                {
                    this.id = "Неверный штрихкод";
                    return;
                }
                this.id = B.Tables[0].Rows[0]["IDMAIN"].ToString();
                this.barcode = B.Tables[0].Rows[0]["BARCODE"].ToString();
                Conn.SQLDA.SelectCommand.CommandText = "WITH FC AS (SELECT dt.ID,dt.SORT, "+
                                                          "dt.MNFIELD, "+
                                                          "dt.MSFIELD, "+
                                                          "dt.IDMAIN, "+
                                                          "dtp.PLAIN "+
                                                   "FROM   BRIT_SOVET..DATAEXT dt "+
                                                   "       JOIN BRIT_SOVET..DATAEXTPLAIN dtp " +
                                                   "            ON  dt.ID = dtp.IDDATAEXT) "+
                                                   "select  COL1.PLAIN zag,dtpa.PLAIN avt from FC COL1 "+
                                                   "left join FC dtpa ON COL1.IDMAIN = dtpa.IDMAIN and dtpa.MNFIELD = 700 and dtpa.MSFIELD = '$a' "+
                                                   "where COL1.MNFIELD = 200 and COL1.MSFIELD = '$a'  and COL1.IDMAIN = " + this.id;
                Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                this.name = B.Tables[0].Rows[0]["zag"].ToString(); ;
                this.author = B.Tables[0].Rows[0]["avt"].ToString();
                Conn.SQLDA.SelectCommand.CommandText = "select * from ZAKAZ where IDMAIN = " + this.id;
                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                B = new DataSet();
                this.rname = "";
                try
                {
                    i = Conn.SQLDA.Fill(B);
                    this.rname = B.Tables[0].Rows[0]["IDREADER"].ToString();
                }
                catch
                {
                    this.rname = "";
                }
                this.rid = this.rname;
                if (this.rname != "")
                {
                    Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + this.rname;
                    DataSet R = new DataSet();
                    Conn.OleDA.Fill(R);
                    string name = "";
                    string secondName = "";
                    try
                    {
                        name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                    }
                    catch
                    {
                        name = "";
                    }
                    try
                    {
                        secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                    }
                    catch
                    {
                        secondName = "";
                    }
                    this.rname = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                    //this.rname = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". " + R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                }
                /*                finally
                                {
                                    this.rname = "";
                                }*/

                //this.rname = ;
            }
            public string barcode;
            public string id;
            public string name;
            public string rname;
            public string author;
            public string rid;
        }



        public bool ChangeEmployee(string login, string pass)
        {//                                    SELECT Employee.* FROM Employee WHERE (((Employee.Login)="1") AND ((Employee.Password)="1"));

            Conn.OleDA.SelectCommand.CommandText = "SELECT * FROM BJVVV..USERS WHERE lower(LOGIN)='" + login.ToLower() + "' AND lower(PASSWORD)='" + pass.ToLower() + "'";
            //ReaderMain.Tables.Clear();
            DataSet R = new DataSet();
            if (Conn.OleDA.Fill(R) != 0)
            {
                F1.textBox1.Text = R.Tables[0].Rows[0]["NAME"].ToString();
                F1.EmpID = R.Tables[0].Rows[0]["ID"].ToString();
                return true;
            }
            else
                return false;
        }

        public class XmlConnections
        {
            public XmlTextReader reader;
            static String filename = Application.StartupPath + "\\DBConnections.xml";
            public XmlDocument doc;
            public string GetReaderCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/Readers");
                return node.InnerText;
            }
            public string GetZakazCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
                return node.InnerText;
            }
            public string GetBRIT_SOVETCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/BRIT_SOVET");
                return node.InnerText;
            }

            public XmlConnections()
            {
                // Create the validating reader and specify DTD validation.
                try
                {
                    //XmlReaderSettings settings = new XmlReaderSettings();
                    //settings.ProhibitDtd = false;
                    //settings.ValidationType = ValidationType.DTD;
                    //settings.ValidationEventHandler += eventHandler;

                    //reader = new XmlTextReader(filename);

                    // Pass the validating reader to the XML document.
                    // Validation fails due to an undefined attribute, but the 
                    // data is still loaded into the document.
                    doc = new XmlDocument();
                    doc.Load(filename);// (reader);
                    //Console.WriteLine(doc.OuterXml);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }

        }

        public DataTable GetDebtors()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select A.DATE_VOZV,A.IDREADER,"+
                "isnull(B.FamilyName,'') +' '+ isnull(B.[Name],'') +' '+ isnull(B.FatherName,'') fio," +
                " C.PLAIN+',- '+D.PLAIN,A.BAR, " +
                " (case when B.Email is null then 'false' else 'true' end) email" +
                " , isnull(B.Email,'<не указан>') em " +
                " , case when rtrim(ltrim(isnull('факт.: '+B.LiveTelephone+',','') + ' ' + isnull('дом.:'+B.RegistrationTelephone+',','') + ' ')) = '' then '<не указан>' else isnull('факт.: '+B.LiveTelephone+',','') + ' ' + isnull('дом.:'+B.RegistrationTelephone+',','') end ph, " +
                " case when B.Email is not null and B.Email != '' then 'email указан' else '' end note, E.PLAIN mesto,F.PLAIN dizd,G.PLAIN shifr "+
                " from BRIT_SOVET..ZAKAZ A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BRIT_SOVET..DATAEXT CC on A.IDMAIN_CONST = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BRIT_SOVET..DATAEXT DD on A.IDMAIN_CONST = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BRIT_SOVET..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BRIT_SOVET..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BRIT_SOVET..DATAEXT EE on A.IDMAIN_CONST = EE.IDMAIN and EE.MNFIELD = 210 and EE.MSFIELD = '$a'" +
                " left join BRIT_SOVET..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BRIT_SOVET..DATAEXT FF on A.IDMAIN_CONST = FF.IDMAIN and FF.MNFIELD = 2100 and FF.MSFIELD = '$d'" +
                " left join BRIT_SOVET..DATAEXTPLAIN F on F.IDDATAEXT = FF.ID" +
                " left join BRIT_SOVET..DATAEXT GG on A.IDMAIN_CONST = GG.IDMAIN and GG.MNFIELD = 899 and GG.MSFIELD = '$j'" +
                " left join BRIT_SOVET..DATAEXTPLAIN G on G.IDDATAEXT = GG.ID" +
                " where " +
                //" A.DATE_VOZV between '" + start.ToString("yyyyMMdd") + "' and '" + finish.ToString("yyyyMMdd") + "'" +
                " A.IDMAIN != 0 and A.PENALTY = 1";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables[0];   
                                                                                                                                                                                                                                                                       // "+DateTime.Now.ToString("MM/dd/yyyy")+"                   
            /*Conn.SQLDA.SelectCommand.CommandText = "select X.IDMAIN, X.PLAIN, Y.SORT, Y.MNFIELD, Z.DATE_VOZV, Z.IDREADER, Z.BAR BAR from BRIT_SOVET..DATAEXTPLAIN X join BRIT_SOVET..DATAEXT Y on Y.ID=X.IDDATAEXT join BRIT_SOVET..ZAKAZ Z on Z.IDMAIN = Y.IDMAIN where (Z.IDMAIN <> 0) and (Z.DATE_VOZV < '" + DateTime.Now.ToString("yyyyMMdd") +"') and ((Y.MNFIELD = 200 and Y.MSFIELD = '$a') or (Y.MSFIELD = '$a' and Y.MNFIELD = 700)) order by X.IDMAIN";
            //Conn.SQLDA.SelectCommand.CommandText = "select DATE_VOZV, IDREADER from ZAKAZ where IDMAIN <> 0 and DATE_VOZV < '11.11.2008'"; //" + DateTime.Now.ToString("MM/dd/yyyy") + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            DataSet D = new DataSet();
            R.Tables.Add("vperemeshku");
            R.Tables.Add("distinct");
            int i = Conn.SQLDA.Fill(R.Tables["vperemeshku"]);
            Conn.SQLDA.SelectCommand.CommandText = "select DATE_VOZV, IDREADER from ZAKAZ where IDMAIN <> 0 and DATE_VOZV < '" + DateTime.Now.ToString("yyyyMMdd") +"' order by IDMAIN";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            i = Conn.SQLDA.Fill(R.Tables["distinct"]);

            R.Tables.Add("postolbcam");
            R.Tables["postolbcam"].Columns.Add("date");
            R.Tables["postolbcam"].Columns.Add("num");
            R.Tables["postolbcam"].Columns.Add("fam");
            R.Tables["postolbcam"].Columns.Add("name");
            R.Tables["postolbcam"].Columns.Add("secname");
            R.Tables["postolbcam"].Columns.Add("Zagl");
            R.Tables["postolbcam"].Columns.Add("Avtor");
            R.Tables["postolbcam"].Columns.Add("ZagSort");
            R.Tables["postolbcam"].Columns.Add("AvtorSort");
            R.Tables["postolbcam"].Columns.Add("BAR");

            DataRow ARow = R.Tables["postolbcam"].NewRow();
            string id = R.Tables["vperemeshku"].Rows[0]["IDMAIN"].ToString();
            ARow["date"] = DateTime.Parse(R.Tables["vperemeshku"].Rows[0]["DATE_VOZV"].ToString()).ToShortDateString();
            ARow["BAR"] = R.Tables["vperemeshku"].Rows[0]["BAR"].ToString();
            Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + R.Tables["vperemeshku"].Rows[0]["IDREADER"].ToString();
            i = Conn.OleDA.Fill(D);
            ARow["num"] = D.Tables[0].Rows[0]["NumberReader"].ToString();
            ARow["fam"] = D.Tables[0].Rows[0]["FamilyName"].ToString();
            ARow["name"] = D.Tables[0].Rows[0]["Name"].ToString();
            ARow["secname"] = D.Tables[0].Rows[0]["FatherName"].ToString();

            //ARow["sprash"] = R.Tables["vperemeshku"].Rows[0]["sp"].ToString();
            foreach (DataRow row in R.Tables["vperemeshku"].Rows)
            {
                if (id != row["IDMAIN"].ToString())
                {
                    D.Clear();
                    R.Tables["postolbcam"].Rows.Add(ARow);
                    ARow = R.Tables["postolbcam"].NewRow();
                    id = row["IDMAIN"].ToString();
                    ARow["date"] = row["DATE_VOZV"];
                    ARow["BAR"] = row["BAR"];
                    Conn.OleDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + row["IDREADER"].ToString();
                    i = Conn.OleDA.Fill(D);
                    ARow["num"] = D.Tables[0].Rows[0]["NumberReader"].ToString();
                    ARow["fam"] = D.Tables[0].Rows[0]["FamilyName"].ToString();
                    ARow["name"] = D.Tables[0].Rows[0]["Name"].ToString();
                    ARow["secname"] = D.Tables[0].Rows[0]["FatherName"].ToString();
                }

                switch (row["MNFIELD"].ToString())
                {
                    case "200":
                        ARow["Zagl"] = row["PLAIN"].ToString();
                        ARow["ZagSort"] = row["SORT"].ToString();
                        break;
                    case "700":
                        ARow["Avtor"] = row["PLAIN"].ToString();
                        ARow["AvtorSort"] = row["SORT"].ToString();
                        break;
                }
            }
            R.Tables["postolbcam"].Rows.Add(ARow);

            return R.Tables["postolbcam"];*/
        }

        public DataTable GetIssuedBooks()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select  X.IDMAIN, X.PLAIN, Y.SORT, Y.MNFIELD, (count(Z.BAR)) as sp, Z.DATE_VOZV,Z.DATE_ISSUE,Z.IDREADER from BRIT_SOVET..DATAEXTPLAIN X join BRIT_SOVET..DATAEXT Y on Y.ID=X.IDDATAEXT join BRIT_SOVET..ZAKAZ Z on Z.IDMAIN = Y.IDMAIN join BRIT_SOVET..ZAKAZ ZZ on Z.IDMAIN = ZZ.IDMAIN_CONST where ((Y.MNFIELD = 200 and Y.MSFIELD = '$a') or (Y.MSFIELD = '$a' and Y.MNFIELD = 700) or (Y.MSFIELD = '$d' and Y.MNFIELD = 2100)) group by X.PLAIN, Y.SORT, Y.MNFIELD, X.IDMAIN,Z.DATE_VOZV,Z.DATE_ISSUE,Z.IDREADER order by X.IDMAIN"; //inner join TECHNOLOG..ZAKAZ Y on Y.BAR=Z.BAR";
            //Conn.SQLDA.SelectCommand.CommandText = "select  X.PREOPS, X.PREOPSAUTHOR,count(Z.BAR) as спрашиваемость from technolog..zakaz Z inner join BRIT_SOVET..MAIN X on Z.IDMAIN_CONST = X.ID  group by X.PREOPS,X.PREOPSAUTHOR";
            //Conn.SQLDA.SelectCommand.CommandText = "select  BRIT_SOVET..MAIN.PREOPS, BRIT_SOVET..MAIN.PREOPSAUTHOR from technolog..zakaz inner join BRIT_SOVET..MAIN on ZAKAZ.IDMAIN = MAIN.ID";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            DataSet D = new DataSet();
            R.Tables.Add("vperemeshku");
            R.Tables.Add("distinct");
            int i = Conn.SQLDA.Fill(R.Tables["vperemeshku"]);
            Conn.SQLDA.SelectCommand.CommandText = "select distinct Y.IDMAIN from BRIT_SOVET..DATAEXT Y inner join BRIT_SOVET..ZAKAZ Z on Z.IDMAIN = Y.IDMAIN where Z.IDMAIN != 0 order by Y.IDMAIN";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            i = Conn.SQLDA.Fill(R.Tables["distinct"]);
            R.Tables.Add("postolbcam");
            R.Tables["postolbcam"].Columns.Add("Zagl");
            R.Tables["postolbcam"].Columns.Add("Avtor");
            R.Tables["postolbcam"].Columns.Add("God");
            R.Tables["postolbcam"].Columns.Add("sprash");
            R.Tables["postolbcam"].Columns.Add("ZagSort");
            R.Tables["postolbcam"].Columns.Add("AvtorSort");

            R.Tables["postolbcam"].Columns.Add("NN");
            R.Tables["postolbcam"].Columns.Add("FIO");
            R.Tables["postolbcam"].Columns.Add("abn");
            R.Tables["postolbcam"].Columns.Add("diss");
            R.Tables["postolbcam"].Columns.Add("dvzv");
            R.Tables["postolbcam"].Columns["diss"].DataType = typeof(DateTime);
            R.Tables["postolbcam"].Columns["dvzv"].DataType = typeof(DateTime);
            DataRow ARow = R.Tables["postolbcam"].NewRow();
            string id = R.Tables["vperemeshku"].Rows[0]["IDMAIN"].ToString();
            //ARow["dvzv"] = DateTime.Parse(R.Tables["vperemeshku"].Rows[0]["DATE_VOZV"].ToString()).ToShortDateString();
            //ARow["diss"] = DateTime.Parse(R.Tables["vperemeshku"].Rows[0]["DATE_ISSUE"].ToString()).ToShortDateString();
            ARow["dvzv"] = R.Tables["vperemeshku"].Rows[0]["DATE_VOZV"];
            ARow["diss"] = R.Tables["vperemeshku"].Rows[0]["DATE_ISSUE"];

            dbReader rdr = new dbReader(int.Parse(R.Tables["vperemeshku"].Rows[0]["IDREADER"].ToString()));
            ARow["NN"] = rdr.id;
            ARow["FIO"] = rdr.FIO;
            ARow["abn"] = rdr.AbonType;
            ARow["sprash"] = R.Tables["vperemeshku"].Rows[0]["sp"].ToString();
            foreach (DataRow row in R.Tables["vperemeshku"].Rows)
            {
                if (id != row["IDMAIN"].ToString())
                {
                    R.Tables["postolbcam"].Rows.Add(ARow);
                    ARow = R.Tables["postolbcam"].NewRow();
                    id = row["IDMAIN"].ToString();
                    ARow["sprash"] = row["sp"].ToString();
                    rdr = new dbReader(int.Parse(row["IDREADER"].ToString()));
                    ARow["NN"] = rdr.id;
                    ARow["FIO"] = rdr.FIO;
                    ARow["abn"] = rdr.AbonType;
                }

                switch (row["MNFIELD"].ToString())
                {
                    case "200":
                        ARow["Zagl"] = row["PLAIN"].ToString();
                        ARow["ZagSort"] = row["SORT"].ToString();
                        //ARow["dvzv"] = DateTime.Parse(row["DATE_VOZV"].ToString()).ToShortDateString();
                        //ARow["diss"] = DateTime.Parse(row["DATE_ISSUE"].ToString()).ToShortDateString();
                        ARow["dvzv"] = row["DATE_VOZV"];
                        ARow["diss"] = row["DATE_ISSUE"];
                        break;
                    case "700":
                        ARow["Avtor"] = row["PLAIN"].ToString();
                        ARow["AvtorSort"] = row["SORT"].ToString();
                        break;
                    case "2100":
                        ARow["God"] = row["PLAIN"].ToString();
                        break;
                }
            }
            R.Tables["postolbcam"].Rows.Add(ARow);

            return R.Tables["postolbcam"];

            /*R.Tables.Add();
            int i = Conn.SQLDA.Fill(R.Tables[0]);

            return R;*/
        }

        public string GetReaderCount(DateTime Start, DateTime End)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select distinct IDREADER,DATE_ISSUE from BRIT_SOVET..ZAKAZ where DATE_ISSUE >= '" + Start.ToString("yyyyMMdd") + "' and DATE_ISSUE <= '" + End.ToString("yyyyMMdd") +"'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();

            int i = Conn.SQLDA.Fill(R);
            return i.ToString();
        }

        public string GetBooksCount(DateTime Start, DateTime End)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select BAR from BRIT_SOVET..ZAKAZ where DATE_ISSUE >= '" + Start.ToString("yyyyMMdd") + "' and DATE_ISSUE <= '" + End.ToString("yyyyMMdd") +"'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            int i = Conn.SQLDA.Fill(R);
            //CultureInfo.CurrentCulture = ...
            return i.ToString();
        }

        public DataTable GetAllBooks()
        {
            Conn.SQLDA.SelectCommand.CommandText =
                    "with FA as ( " +
                    "select BAR,COUNT(BAR) cnt from BRIT_SOVET..ZAKAZ group by BAR " +
                    ") " +
                    "select distinct  BB.PLAIN pol, FF.PLAIN bar, CC.PLAIN zag, DD.PLAIN avt, EE.PLAIN god, " +
                    "case when Z.cnt is null then 0 else Z.cnt end sp,C.SORT,D.SORT " +
                    "from BRIT_SOVET..DATAEXT A " +
                    "left join BRIT_SOVET..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 899 and B.MSFIELD = '$c' " +
                    "left join BRIT_SOVET..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT     " +
                    "left join BRIT_SOVET..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C.MSFIELD = '$a' " +
                    "left join BRIT_SOVET..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT     " +
                    "left join BRIT_SOVET..DATAEXT D on A.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a' " +
                    "left join BRIT_SOVET..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT     " +
                    "left join BRIT_SOVET..DATAEXT E on A.IDMAIN = E.IDMAIN and E.MNFIELD = 2100 and E.MSFIELD = '$d' " +
                    "left join BRIT_SOVET..DATAEXTPLAIN EE on E.ID = EE.IDDATAEXT     " +
                    "left join BRIT_SOVET..DATAEXT F on A.IDMAIN = F.IDMAIN and F.MNFIELD = 899 and F.MSFIELD = '$w' " +
                    "left join BRIT_SOVET..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT     " +
                    "left join FA Z on F.SORT collate Cyrillic_general_CI_AI = Z.BAR " +
                    "where C.IDMAIN is not null ";
            //Conn.SQLDA.SelectCommand.CommandText = "select X.IDMAIN,X.MNFIELD, X.SORT, (count(Y.BAR)) as sp from BRIT_SOVET..DATAEXT X left join TECHNOLOG..ZAKAZ Y on Y.IDMAIN_CONST=X.IDMAIN where (X.MSFIELD = '$a' and X.MNFIELD = 200) or (X.MSFIELD = '$a' and X.MNFIELD = 700) or (X.MSFIELD = '$d' and X.MNFIELD = 2100) group by X.IDMAIN,X.SORT,X.MNFIELD";
            //Conn.SQLDA.SelectCommand.CommandText = "select IDMAIN, SORT, MNFIELD from BRIT_SOVET..DATAEXT where (MSFIELD = '$a' and MNFIELD = 200) or (MSFIELD = '$a' and MNFIELD = 700) or (MSFIELD = '$d' and MNFIELD = 2100)";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            //R.Tables.Add();
            //R.Tables.Add("distinct");
            int i = Conn.SQLDA.Fill(R,"t");
            /*Conn.SQLDA.SelectCommand.CommandText = "select distinct IDMAIN from BRIT_SOVET..DATAEXT order by IDMAIN ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            i = Conn.SQLDA.Fill(R.Tables["distinct"]);
            R.Tables.Add("postolbcam");
            R.Tables["postolbcam"].Columns.Add("Polka");
            R.Tables["postolbcam"].Columns.Add("bar");
            R.Tables["postolbcam"].Columns.Add("Zagl");
            R.Tables["postolbcam"].Columns.Add("Avtor");
            R.Tables["postolbcam"].Columns.Add("God");
            Type t = i.GetType();
            R.Tables["postolbcam"].Columns.Add("sprash",t);
            R.Tables["postolbcam"].Columns.Add("ZagSort");
            R.Tables["postolbcam"].Columns.Add("AvtorSort");
            R.Tables["postolbcam"].Columns.Add("vidacha");

            DataRow ARow = R.Tables["postolbcam"].NewRow();
            string id = R.Tables["vperemeshku"].Rows[0]["IDMAIN"].ToString();
            ARow["sprash"] = R.Tables["vperemeshku"].Rows[0]["sp"];
            //string vida = R.Tables["vperemeshku"].Rows[0]["idm"].ToString();
            ARow["vidacha"] = R.Tables["vperemeshku"].Rows[0]["vida"].ToString();
            ARow["bar"] = R.Tables["vperemeshku"].Rows[0]["bar"].ToString();
            foreach (DataRow row in R.Tables["vperemeshku"].Rows)
            {
                if (id != row["IDMAIN"].ToString())
                {
                    R.Tables["postolbcam"].Rows.Add(ARow);
                    ARow = R.Tables["postolbcam"].NewRow();
                    id = row["IDMAIN"].ToString();
                    ARow["sprash"] = row["sp"];
                    //vida = row["idm"].ToString();
                    //if (vida != "")
                        //MessageBox.Show(vida);
                    ARow["vidacha"] = row["vida"].ToString();
                    ARow["bar"] = row["bar"].ToString();
                }

                switch (row["MNFIELD"].ToString() + row["MSFIELD"].ToString())
                {
                    case "200$a":
                        ARow["Zagl"] = row["PLAIN"].ToString();
                        ARow["ZagSort"] = row["SORT"].ToString();
                        break;
                    case "700$a":
                        ARow["Avtor"] = row["PLAIN"].ToString();
                        ARow["AvtorSort"] = row["SORT"].ToString();
                        break;
                    case "2100$d":
                        ARow["God"] = row["PLAIN"].ToString();
                        break;
                    case "899$c":
                        ARow["Polka"] = row["PLAIN"].ToString();
                        break;
                    case "899$w":
                        ARow["bar"] = row["bar"].ToString();
                        break;
                }
            }
            R.Tables["postolbcam"].Rows.Add(ARow);
            */
			return R.Tables["t"];
		}

        internal DataTable GetFormular(string p)
        {
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = p;
            Conn.SQLDA.SelectCommand.CommandText = "select zagp.PLAIN zag,zag.SORT Заглавие_sort,avtp.PLAIN Автор,avt.SORT Автор_sort, " +
                                                   " B.BAR inv,B.BAR inv,zag.IDMAIN idmain, B.DATE_ISSUE issue,B.DATE_VOZV vozv,B.DATE_FACT_VOZV fact,  " +
                                                   " B.IDMAIN zkid,B.ID zid,B.PENALTY penalty,B.REMPENALTY rempenalty,B.BAR BAR " +
                                                   " from BRIT_SOVET..ZAKAZ B  " +
                                                   " left join BRIT_SOVET..DATAEXT A on B.BAR collate Cyrillic_General_CI_AI = A.SORT and A.MNFIELD = 899 and A.MSFIELD = '$w' " +
                                                   " left join BRIT_SOVET..DATAEXT zag on " +
                                                                                    " zag.MNFIELD = 200 and " +
                                                                                    " zag.MSFIELD = '$a' and " +
                                                                                    " zag.IDMAIN = A.IDMAIN " +
                                                   " left join BRIT_SOVET..DATAEXT avt on " +
                                                                                    " avt.MNFIELD = 700 and " +
                                                                                    " avt.MSFIELD = '$a' " +
                                                                                    " and avt.IDMAIN = A.IDMAIN " +
                                                   " left join BRIT_SOVET..DATAEXTPLAIN zagp on zagp.IDDATAEXT = zag.ID " +
                                                   " left join BRIT_SOVET..DATAEXTPLAIN avtp on avtp.IDDATAEXT = avt.ID " +
                                                   " where B.IDREADER = @IDR " +
                                                   " and (B.IDMAIN != 0 or (B.IDMAIN = 0 and B.PENALTY = 'true'))";

            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            R.Tables.Add("form");
            int i = Conn.SQLDA.Fill(R.Tables["form"]);

            return R.Tables["form"];
            /*Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = p;
            Conn.SQLDA.SelectCommand.CommandText =" select "+
            "max(case when tmp.mnf = 200 then pl end) as Zag, "+
            "max(case when tmp.mnf = 200 then srt end) as Заглавие_sort, "+
            "max(case when tmp.mnf = 700 then pl end) as Автор, "+
            "max(case when tmp.mnf = 700 then srt end) as Автор_sort, "+
            "max(case when tmp.mnf = 2100 then pl end) as Год_Издания, "+
            "max(case when tmp.mnf = 200 then pl end) as Место_Издания, "+
            "max(case when tmp.mnf = 200 then idm end) as idmain, "+
            "max(case when tmp.mnf = 200 then iss end) as issue, "+
            "max(case when tmp.mnf = 200 then vozv end) as vozv, "+
            "max(case when tmp.mnf = 200 then fct end) as fact, "+
            "max(case when tmp.mnf = 200 then zakid end) as zkid, "+
            "max(case when tmp.mnf = 200 then zi end) as zid, "+
            "((case when (tmp.pnlt = 'false' or tmp.pnlt is NULL) then 'false' else 'true' end)) as penalty, " +
            "((case when (tmp.rempnlt = 'false' or tmp.rempnlt is NULL) then 'false' else 'true' end)) as rempenalty " +
            "from " +
            "(select Z.ID as zi,Z.IDMAIN as zakid, Z.DATE_ISSUE as iss, Z.DATE_VOZV as vozv, Z.DATE_FACT_VOZV as fct, Z.PENALTY as pnlt,Z.REMPENALTY as rempnlt, X.IDMAIN as idm, X.PLAIN as pl, Y.SORT as srt, Y.MNFIELD as mnf " +
            "from BRIT_SOVET..DATAEXTPLAIN X "+
            "join BRIT_SOVET..DATAEXT Y on Y.ID=X.IDDATAEXT "+
            "join BRIT_SOVET..ZAKAZ Z on ((Z.IDMAIN = Y.IDMAIN) or (Z.IDMAIN_CONST=Y.IDMAIN and Z.PENALTY='true')) " +
            //"--join BRIT_SOVET..ZAKAZ ZZ on Z.IDMAIN = ZZ.IDMAIN_CONST "+
            "where (((Y.MNFIELD = 200 and Y.MSFIELD = '$a') "+
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 700) "+
            "or (Y.MSFIELD = '$d' and Y.MNFIELD = 2100) "+
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 210)) and (Z.IDREADER = @IDR) and (( ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='true')) or ((Z.IDMAIN=0)and(Z.PENALTY='true')) or ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='false'))  ) )) " +
            "group by Z.ID, Z.IDMAIN, X.PLAIN, Y.SORT, Y.MNFIELD, X.IDMAIN,Z.DATE_ISSUE,Z.DATE_VOZV, Z.DATE_FACT_VOZV,Z.PENALTY, Z.REMPENALTY " +
            ") as tmp "+
            "group by idm,pnlt,rempnlt ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            R.Tables.Add("form");
            int i = Conn.SQLDA.Fill(R.Tables["form"]);
            
            return R.Tables["form"];*/
        }
        internal DataSet GetFormularDS(string p)
        {
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = p;
            Conn.SQLDA.SelectCommand.CommandText = " select " +
            "max(case when tmp.mnf = 200 then pl end) as Zag, " +
            "max(case when tmp.mnf = 200 then srt end) as Заглавие_sort, " +
            "max(case when tmp.mnf = 700 then pl end) as Автор, " +
            "max(case when tmp.mnf = 700 then srt end) as Автор_sort, " +
            "max(case when tmp.mnf = 2100 then pl end) as Год_Издания, " +
            "max(case when tmp.mnf = 200 then pl end) as Место_Издания, " +
            "max(case when tmp.mnf = 200 then idm end) as idmain, " +
            "max(case when tmp.mnf = 200 then iss end) as issue, " +
            "max(case when tmp.mnf = 200 then vozv end) as vozv, " +
            "max(case when tmp.mnf = 200 then fct end) as fact, " +
            "max(case when tmp.mnf = 200 then zakid end) as zkid, " +
            "max(case when tmp.mnf = 200 then zi end) as zid, " +
            "((case when (tmp.pnlt = 'false' or tmp.pnlt is NULL) then 'false' else 'true' end)) as penalty, " +
            "((case when (tmp.rempnlt = 'false' or tmp.rempnlt is NULL) then 'false' else 'true' end)) as rempenalty " +
            "from " +
            "(select Z.ID as zi,Z.IDMAIN as zakid, Z.DATE_ISSUE as iss, Z.DATE_VOZV as vozv, Z.DATE_FACT_VOZV as fct, Z.PENALTY as pnlt,Z.REMPENALTY as rempnlt, X.IDMAIN as idm, X.PLAIN as pl, Y.SORT as srt, Y.MNFIELD as mnf " +
            "from BRIT_SOVET..DATAEXTPLAIN X " +
            "join BRIT_SOVET..DATAEXT Y on Y.ID=X.IDDATAEXT " +
            "join BRIT_SOVET..ZAKAZ Z on ((Z.IDMAIN = Y.IDMAIN) or (Z.IDMAIN_CONST=Y.IDMAIN and Z.PENALTY='true')) " +
                //"--join BRIT_SOVET..ZAKAZ ZZ on Z.IDMAIN = ZZ.IDMAIN_CONST "+
            "where (((Y.MNFIELD = 200 and Y.MSFIELD = '$a') " +
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 700) " +
            "or (Y.MSFIELD = '$d' and Y.MNFIELD = 2100) " +
            "or (Y.MSFIELD = '$a' and Y.MNFIELD = 210)) and (Z.IDREADER = @IDR) and (( ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='true')) or ((Z.IDMAIN=0)and(Z.PENALTY='true')) or ((Z.IDMAIN!=0)and(Z.REMPENALTY = 'false')and (Z.PENALTY='false'))  ) )) " +
            "group by Z.ID, Z.IDMAIN, X.PLAIN, Y.SORT, Y.MNFIELD, X.IDMAIN,Z.DATE_ISSUE,Z.DATE_VOZV, Z.DATE_FACT_VOZV,Z.PENALTY, Z.REMPENALTY " +
            ") as tmp " +
            "group by idm,pnlt,rempnlt ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            R.Tables.Add("form");
            int i = Conn.SQLDA.Fill(R.Tables["form"]);

            return R;
        }

        internal bool Prolong(int x, string idb)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ where IDMAIN = '" + idb +"'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            int i = Conn.SQLDA.Fill(B);
            //B.Tables[0].Rows[0]["IDMAIN"] = "0";
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            DateTime dt = DateTime.Parse(B.Tables[0].Rows[0]["DATE_VOZV"].ToString()).AddDays(x);
            B.Tables[0].Rows[0]["DATE_VOZV"] = dt;
            bool result = false;
            if (dt >= DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                result = false;
                B.Tables[0].Rows[0]["PENALTY"] = "false";
            }
            else
            {
                result = true;
                B.Tables[0].Rows[0]["PENALTY"] = "true";
            }
            Conn.SQLDA.Update(B);
            return result;
        }

        internal void SetPenalty(string idr)
        {
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = idr;
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ where IDREADER = @IDR";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            int i = Conn.SQLDA.Fill(B);
            foreach (DataRow row in B.Tables[0].Rows)
            {
                bool isReturned = (int)row["IDMAIN"] == 0;
                bool isFactReturned = (row["DATE_FACT_VOZV"].ToString() != string.Empty);//по хорошему надо узнать как правильно сравнить
                DateTime vozv = (DateTime)row["DATE_VOZV"];//здесь не сравнивается с нулом потому что типа всегда это поле долно иметь значение

                bool isRetLater = (isFactReturned) ? (DateTime)row["DATE_VOZV"] < (DateTime)row["DATE_FACT_VOZV"] : ((DateTime)row["DATE_VOZV"] < DateTime.Now) ? true : false;
                bool isTimeOver = (DateTime)row["DATE_VOZV"] < DateTime.Now && !isFactReturned;
                bool wasPenalty = (bool)row["REMPENALTY"];
                bool nowPenalty = (bool)row["PENALTY"];

                if ((isRetLater || isTimeOver) && !wasPenalty && !nowPenalty)
                {
                    row["PENALTY"] = true;
                    row["REMPENALTY"] = false;
                }
            }
            
            Conn.SQLDA.Update(B);           
        }
        internal void SetPenaltyAll()
        {
            //Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = idr;
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            int i = Conn.SQLDA.Fill(B);
            foreach (DataRow row in B.Tables[0].Rows)
            {
                bool isReturned = (int)row["IDMAIN"] == 0;
                bool isFactReturned = (row["DATE_FACT_VOZV"].ToString() != string.Empty);//по хорошему надо узнать как правильно сравнить
                DateTime vozv = (DateTime)row["DATE_VOZV"];//здесь не сравнивается с нулом потому что типа всегда это поле долно иметь значение

                bool isRetLater = (isFactReturned) ? (DateTime)row["DATE_VOZV"] < (DateTime)row["DATE_FACT_VOZV"] : ((DateTime)row["DATE_VOZV"] < DateTime.Now) ? true : false;
                bool isTimeOver = (DateTime)row["DATE_VOZV"] < DateTime.Now && !isFactReturned;
                bool wasPenalty = (bool)row["REMPENALTY"];
                bool nowPenalty = (bool)row["PENALTY"];

                if ((isRetLater || isTimeOver) && !wasPenalty && !nowPenalty)
                //if ((((row["DATE_FACT_VOZV"].ToString() == null) && (DateTime.Parse(row["DATE_VOZV"].ToString()) < DateTime.Now)) || ((DateTime.Parse(row["DATE_VOZV"].ToString()) < DateTime.Parse(row["DATE_FACT_VOZV"].ToString()) && (row["REMPENALTY"].ToString().ToLower() == "false")))))// вроде исправил
                //if ((row["IDMAIN"].ToString() != "0") && ((row["DATE_FACT_VOZV"].ToString() == string.Empty) || (DateTime.Parse(row["DATE_VOZV"].ToString()) < DateTime.Parse(row["DATE_FACT_VOZV"].ToString()))) && (DateTime.Parse(row["DATE_VOZV"].ToString()) < DateTime.Now) && (!(bool)row["REMPENALTY"]) && (!(bool)row["PENALTY"]))
                {
                    row["PENALTY"] = true;
                    row["REMPENALTY"] = false;
                    //row["REMPENALTY"] = true;
                }
            }
   

            int rn = Conn.SQLDA.Update(B);
            Conn.SQLDA.SelectCommand.Connection.Close();
        }
        internal void RemPenalty(string zid)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ where ID = '" + zid + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            Conn.SQLDA.UpdateCommand = cmdBuilder.GetUpdateCommand();
            int i = Conn.SQLDA.Fill(B);
            B.Tables[0].Rows[0]["REMPENALTY"] = true;
            B.Tables[0].Rows[0]["PENALTY"] = false;
            Conn.SQLDA.Update(B);           

            //throw new Exception("The method or operation is not implemented.");
        }

        internal int GetBookCountForReader(string idr)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from BRIT_SOVET..ZAKAZ where IDREADER = '" + idr + "' and IDMAIN != 0 and REMPENALTY = 'false'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            return Conn.SQLDA.Fill(B);
        }

        internal void SetReaderAbonement(string idr, string abt)
        {
            Conn.OleDA.SelectCommand.CommandText = "select * from Main where NumberReader = " + idr;
            Conn.OleDA.SelectCommand.Connection = Conn.ReadersCon;

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.OleDA);
            DataSet B = new DataSet();
            int i = Conn.OleDA.Fill(B);
            B.Tables[0].Rows[0]["AbonementType"] = abt;
            Conn.OleDA.Update(B);
        }

        internal string GetLastEmailDate(string num)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select max(DATEACT) from Reservation_R..BRIT_SOVETACTIONS where IDREADER = " + num + " and ACTIONTYPE = 4";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(Conn.SQLDA);
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            string ret = (B.Tables[0].Rows[0][0] == DBNull.Value) ? "<нет>" : B.Tables[0].Rows[0][0].ToString();
            return ret;
        }

        internal void InsertActionEmail(dbReader reader)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_R..BRIT_SOVETACTIONS (ACTIONTYPE,BAR,IDEMP,IDREADER,DATEACT) " +
                                                    " values (@ACTIONTYPE,@BAR,@IDEMP,@IDREADER,@DATEACT)";
            Conn.SQLDA.InsertCommand.Parameters.Add("ACTIONTYPE", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("BAR", SqlDbType.NVarChar);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDEMP", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("DATEACT", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["ACTIONTYPE"].Value = 4;
            Conn.SQLDA.InsertCommand.Parameters["BAR"].Value = "email";
            Conn.SQLDA.InsertCommand.Parameters["IDEMP"].Value = this.F1.EmpID;
            Conn.SQLDA.InsertCommand.Parameters["IDREADER"].Value = reader.id;
            Conn.SQLDA.InsertCommand.Parameters["DATEACT"].Value = DateTime.Now;
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Не сработало протоколирование действия - Отправка email. Обратитесь к разработчику.");
            }
        }
        internal void InsertActionISSUED(dbReader reader, dbBook book)
        {

            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_R..BRIT_SOVETACTIONS (ACTIONTYPE,BAR,IDEMP,IDREADER,DATEACT) " +
                                                    " values (@ACTIONTYPE,@BAR,@IDEMP,@IDREADER,@DATEACT)";
            Conn.SQLDA.InsertCommand.Parameters.Add("ACTIONTYPE", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("BAR", SqlDbType.NVarChar);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDEMP", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("DATEACT", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["ACTIONTYPE"].Value = 1;
            Conn.SQLDA.InsertCommand.Parameters["BAR"].Value = book.barcode;
            Conn.SQLDA.InsertCommand.Parameters["IDEMP"].Value = this.F1.EmpID;
            Conn.SQLDA.InsertCommand.Parameters["IDREADER"].Value = reader.id;
            Conn.SQLDA.InsertCommand.Parameters["DATEACT"].Value = DateTime.Now;
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Не сработало протоколирование действия - выдача. Обратитесь к разработчику.");
            }
        }
        internal void InsertActionRETURNED(dbReader reader, dbBook book)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_R..BRIT_SOVETACTIONS (ACTIONTYPE,BAR,IDEMP,IDREADER,DATEACT) " +
                                                    " values (@ACTIONTYPE,@BAR,@IDEMP,@IDREADER,@DATEACT)";
            Conn.SQLDA.InsertCommand.Parameters.Add("ACTIONTYPE", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("BAR", SqlDbType.NVarChar);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDEMP", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("DATEACT", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["ACTIONTYPE"].Value = 2;
            Conn.SQLDA.InsertCommand.Parameters["BAR"].Value = book.barcode;
            Conn.SQLDA.InsertCommand.Parameters["IDEMP"].Value = this.F1.EmpID;
            Conn.SQLDA.InsertCommand.Parameters["IDREADER"].Value = reader.id;
            Conn.SQLDA.InsertCommand.Parameters["DATEACT"].Value = DateTime.Now;
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Не сработало протоколирование действия - возврат. Обратитесь к разработчику.");
            }
        }
        internal void InsertActionProlong(dbReader reader, dbBook book)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_R..BRIT_SOVETACTIONS (ACTIONTYPE,BAR,IDEMP,IDREADER,DATEACT) " +
                                                    " values (@ACTIONTYPE,@BAR,@IDEMP,@IDREADER,@DATEACT)";
            Conn.SQLDA.InsertCommand.Parameters.Add("ACTIONTYPE", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("BAR", SqlDbType.NVarChar);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDEMP", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("DATEACT", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["ACTIONTYPE"].Value = 3;
            Conn.SQLDA.InsertCommand.Parameters["BAR"].Value = book.barcode;
            Conn.SQLDA.InsertCommand.Parameters["IDEMP"].Value = this.F1.EmpID;
            Conn.SQLDA.InsertCommand.Parameters["IDREADER"].Value = reader.id;
            Conn.SQLDA.InsertCommand.Parameters["DATEACT"].Value = DateTime.Now;
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ". Не сработало протоколирование действия - продление. Обратитесь к разработчику.");
            }
        }
        internal object GetActions(DateTime start, DateTime end, string userID)
        {
            //Conn.SQLDA.SelectCommand.Parameters["@IDEMP"].Value = p;
            Conn.SQLDA.SelectCommand.CommandText = "select  B.ACTION ID,B.ACTION act, " +
               " (case when count(A.ACTIONTYPE) is null  or count(A.ACTIONTYPE)= 0 then 0  else count(A.ACTIONTYPE) end) cnt" +
               " from Reservation_R..ACTIONSTYPE B  " +
               " left join Reservation_R..BRIT_SOVETACTIONS A on A.ACTIONTYPE = B.ID" +
               " and A.IDEMP = " + userID +
               " and A.DATEACT between '" + start.ToString("dd.MM.yyyy") + "' and '" + end.AddDays(1).ToString("dd.MM.yyyy") + "'" +
               " group by B.ACTION";

            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            int i = Conn.SQLDA.Fill(R);
            return R.Tables[0];
        }
    }
}
