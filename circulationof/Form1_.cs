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
using System.Globalization;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using ExtGui;
using System.Diagnostics;
using System.IO.Ports;
using System.ServiceModel;
using LibflClassLibrary.Controls.Readers;
using LibflClassLibrary.Controls;
using LibflClassLibrary.BJUsers;

namespace Circulation
{
    public delegate void ScannedEventHandler(object sender, EventArgs ev);
    public delegate void HeaderClick(object sender, DataGridViewCellMouseEventArgs ev);
    public delegate void AbonChangedEventHandler(object sender, EventArgs ev);

    public partial class Form1 : Form
    {
        public DBWork dbw;
        //_BarcScan sc;
        SerialPort port;

        public string EmpID;
        public string DepID;
        public string DepName;
        private Form2 f2;
        public dbReader ReaderRecord, ReaderRecordWork, ReaderRecordFormular;
        public dbBook BookRecord, BookRecordWork;
        public dbReader ReaderSetBarcode;
        public ExtGui.RoundProgress RndPrg;
        public string BASENAME;
        BJUserInfo user;
        public Form1()
        {
            try
            {
                BASENAME = "Reservation_R";
                //f2 = new Form2(this);
                InitializeComponent();
                fBJAuthorization auth = new fBJAuthorization("BJVVV");
                auth.ShowDialog();
                if (auth.User != null) 
                {
                    this.user = auth.User;
                    this.textBox1.Text = user.FIO;//R.Tables[0].Rows[0]["NAME"].ToString();
                    this.EmpID = user.Id.ToString();// R.Tables[0].Rows[0]["ID"].ToString();
                    this.DepID = user.SelectedUserStatus.DepId.ToString();// R.Tables[0].Rows[0]["DEPT"].ToString();
                    this.DepName = user.SelectedUserStatus.DepName;//this.GetDepName(F1.DepID);
                    this.textBox2.Text = user.SelectedUserStatus.DepName;
                }


                dbw = new DBWork(this);
                //sc = new _BarcScan(this);
                this.StartPosition = FormStartPosition.CenterScreen;
                //f2.ShowDialog();


                Form1.Scanned += new ScannedEventHandler(Form1_Scanned);
                this.button2.Enabled = false;
                this.button4.Enabled = false;
                label4.Text = "Журнал событий " + DateTime.Now.ToShortDateString() + ":";
                label2.Text = "Журнал событий " + DateTime.Now.ToShortDateString() + ":";
                Formular.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Formular.Columns.Clear();
                //this.DoubleBuffered = true;
                dataGridView1.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgwHome.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgwHome.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
                label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
                dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);
                dbw.DeleteRefusual();
                port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //tabControl1.TabPages.RemoveAt(4);
        }


        public delegate void ScanFuncDelegate(object sender, EventArgs ev);
        string FromPort = "";

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            FromPort = port.ReadLine();
            FromPort = FromPort.Trim();
            port.DiscardInBuffer();
            ScanFuncDelegate ScanDelegate;
            ScanDelegate = new ScanFuncDelegate(Form1_Scanned);
            this.Invoke(ScanDelegate, new object[] { sender, e });
        }


        void FormularColumnsForming(string ReaderID)
        {
            Formular.Columns.Clear();
            Formular.AutoGenerateColumns = false;
            Formular.Columns.Add("NN", "№№");
            Formular.Columns[0].Width = 35;
            //dbw.SetPenalty(ReaderID);
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
                MessageBox.Show("За читателем не числится книг!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Formular.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Заглавие";
            col.Width = 225;
            col.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            col.ReadOnly = true;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Zag";
            col.Name = "zag";


            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "zagsort";
            col.Visible = false;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Заглавие_sort";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Автор";
            col.Width = 130;
            col.ReadOnly = true;
            col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Formular.Columns.Add(col);
            col.DataPropertyName = "Автор";
            col.Name = "avt";

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
            col.Name = "overdue";
            col.DataPropertyName = "overdue";
            Formular.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "";
            col.Visible = false;
            col.Name = "zi";
            col.DataPropertyName = "zi";
            this.Formular.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Инв номер/ шкод";
            col.ReadOnly = true;
            col.Width = 100;
            Formular.Columns.Add(col);
            col.DataPropertyName = "inv";
            col.Name = "inv";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Зал выдачи";
            col.ReadOnly = true;
            col.Width = 100;
            Formular.Columns.Add(col);
            col.DataPropertyName = "ziss";
            col.Name = "ziss";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Зал возврата";
            col.ReadOnly = true;
            col.Width = 100;
            Formular.Columns.Add(col);
            col.DataPropertyName = "zret";
            col.Name = "zret";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Статус";
            col.ReadOnly = true;
            col.Width = 100;
            Formular.Columns.Add(col);
            col.DataPropertyName = "status";
            col.Name = "status";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата окончания брони";
            col.ReadOnly = true;
            col.Width = 80;
            Formular.Columns.Add(col);
            col.DataPropertyName = "dend";
            col.ValueType = typeof(DateTime);
            col.Name = "dend";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "bar";
            col.ReadOnly = true;
            col.Width = 80;
            Formular.Columns.Add(col);
            col.DataPropertyName = "bar";
            col.Name = "bar";
            col.Visible = false;

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Дата выдачи";
            col.ReadOnly = true;
            col.Width = 80;
            Formular.Columns.Add(col);
            col.DataPropertyName = "issue";
            col.ValueType = typeof(DateTime);
            col.Name = "diss";

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Продлено, раз";
            col.ReadOnly = true;
            col.Width = 80;
            Formular.Columns.Add(col);
            col.DataPropertyName = "prolonged";
            col.Name = "prolonged";
            col.Visible = true;

            autoinc(Formular);
            PaintDebts(Formular);
        }

        private void PaintDebts(DBDataGridView Formular)
        {
            foreach (DataGridViewRow r in Formular.Rows)
            {
                if (((DateTime)r.Cells["dend"].Value < DateTime.Today) && ((int)r.Cells["overdue"].Value != 0))
                {
                    r.DefaultCellStyle.ForeColor = Color.Red;
                    label27.ForeColor = Color.Red;
                    label27.Text = "Читатель является нарушителем!";
                }
            }
        }
        Form28 f28 = null;
        dbBook previous = null;
        bool InScan = false;
        void Form1_Scanned(object sender, EventArgs ev)
        {
            if (InScan) return;
            InScan = true;
            try
            {
                //MessageBox.Show(((IOPOSScanner_1_10)sender).ScanData.ToString());
                string g = tabControl1.SelectedTab.ToString();
                switch (tabControl1.SelectedTab.Text)
                {
                    case "Формуляр читателя":
                        #region formular
                        //string _data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                        string _data = FromPort;
                        if (!dbw.isReader(_data))
                        {
                            MessageBox.Show("Неверный штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            InScan = false;
                            return;
                        }
                        /*if (_data.Length < 20)
                            _data = _data.Remove(0, 1);*/
                        //_data = _data.Remove(_data.Length - 1, 1);
                        if ((_data[0].ToString() == "V") || (_data[0].ToString() == "X"))
                        {
                            MessageBox.Show("Читатель с таким билетом не привязан к реальному билету! Попробуйте найти читателя по фамилии!");
                            InScan = false;
                            return;
                        }
                        if (_data[0].ToString() == "G")
                        {
                            ReaderRecordFormular = new dbReader(_data);
                        }
                        else
                        {
                            ReaderRecordFormular = new dbReader(_data);
                        }

                        if (ReaderRecordFormular.barcode == "notfoundbynumber")
                        {
                            MessageBox.Show("Читатель не найден, либо неверный штрихкод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        if (ReaderRecordFormular.barcode == "numsoc")
                        {
                            MessageBox.Show("Читатель не найден, либо неверный штрикод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        if (ReaderRecordFormular.barcode == "sersoc")
                        {
                            MessageBox.Show("Не соответствует серия социальной карты!Читатель заменил социальную карту!Номер социальной карты остался прежним, но сменилась серия! Новую социальную карту необходимо зарегистрировать в регистратуре!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        label20.Text = ReaderRecordFormular.Surname + " " + ReaderRecordFormular.Name + " " + ReaderRecordFormular.SecondName;
                        label25.Text = ReaderRecordFormular.id;
                        pictureBox2.Image = ReaderRecordFormular.Photo;
                        this.FormularColumnsForming(ReaderRecordFormular.id);
                        //label29.Text = dbw.GetReaderRights(ReaderRecordFormular.id);
                        readerRightsView1.Init(int.Parse(ReaderRecordFormular.id));
                        Sorting.WhatStat = Stats.Formular;
                        Sorting.AuthorSort = SortDir.None;
                        Sorting.ZagSort = SortDir.None;
                        break;
                        #endregion
                    case "Приём/выдача читателю":
                        #region priemVidacha

                        #region для печати требования
                        if (f25 != null)
                        {
                            if (f31 != null)
                            {
                                _data = FromPort;
                                //_data = _data.Remove(_data.Length - 1, 1);
                                dbBook bb = new dbBook(_data, this.BASENAME);
                                PrintDemand demand = new PrintDemand(bb, this);
                                if (demand.result == "norespan")
                                {
                                    f31.Close();
                                    f31 = null;
                                    MessageBox.Show("Для этой книги нет бронеполки! Невозможно напечатать требование!");
                                    break;
                                }
                                if (demand.result == "ok")
                                {
                                    demand.ShowPreview();
                                    f31.Close();
                                    f31 = null;
                                }
                            }
                            break;
                        }
                        #endregion

                        if (f28 != null) // если идет ввод журнала
                        {
                            MessageBox.Show("Вы считали штрихкод во время редактирования сведений об издании! Необходимо сначала закончить редактирование!");
                            break;
                        }
                        label1.Enabled = true;

                        if ((this.emul == "") || (this.emul == null))
                        {
                            //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                            _data = FromPort;
                        }
                        else
                        {
                            _data = this.emul;
                        }
                        if ((this.ReaderRecord != null) && (this.BookRecord != null))
                        {
                            MessageBox.Show("Подтвердите предыдущую операцию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.emul = "";
                            InScan = false;
                            return;
                        }
                        ReaderRecordWork = null;
                        BookRecordWork = null;
                        if (dbw.isReader(_data))//если читатетль
                        {
                            /*if (_data.Length < 20)
                                _data = _data.Remove(0, 1);*/
                            //_data = _data.Remove(_data.Length - 1, 1);
                            if ((_data[0] == 'V') || (_data[0] == 'Q'))
                            {
                                MessageBox.Show("Выдача документов читателю с таким билетом запрещена!");
                                this.emul = "";
                                InScan = false;
                                return;
                            } else
                                if ((_data[0] == 'X'))
                                {
                                    ReaderRecordWork = new dbReader(_data);
                                }
                                else
                                {
                                    ReaderRecordWork = new dbReader(_data);
                                }
                            if (ReaderRecordWork.barcode == "notfoundbynumber")
                            {
                                MessageBox.Show("Читатель не найден, либо неверный штрихкод! Возможна ошибка сканера", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if (ReaderRecordWork.barcode == "numsoc")
                            {
                                MessageBox.Show("Читатель не найден, либо неверный штрикод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if (ReaderRecordWork.barcode == "sersoc")
                            {
                                MessageBox.Show("Не соответствует серия социальной карты! Читатель заменил социальную карту! Номер социальной карты остался прежним, но сменилась серия! Новую социальную карту необходимо зарегистрировать в регистратуре!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if (this.ReaderRecord != null)
                            {
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if (this.ReaderRecord != null)
                            {
                                MessageBox.Show("Подтвердите предыдущую операцию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if (ReaderRecordWork.barcode == "error")
                            {
                                MessageBox.Show("Читатель не зарегистрирован либо не соответствует серия социальной карты!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                        }
                        else// если книга
                        {
                            //_data = _data.Remove(_data.Length - 1, 1);
                            BookRecordWork = new dbBook(_data, this.BASENAME);
                            if (BookRecordWork.zal == null)
                            {
                                BookRecordWork.zal = this.DepName;
                                BookRecordWork.zalid = this.DepID;
                            }
                            if (BookRecordWork.id == "Неверный штрихкод")
                            {
                                MessageBox.Show("Неверный штрихкод! Возможно ошибся сканер! Повторите попытку!",
                                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            if ((BookRecordWork.id == "надовводить") || ((BookRecordWork.id == "-1") && (!dbw.isBookBusy(BookRecordWork.barcode))))
                            {
                                string tmp = BookRecordWork.id;
                                BookRecordWork.barcode = _data;

                                f28 = new Form28(BookRecordWork, true, previous);
                                f28.ShowDialog();
                                if (f28.Cancel)
                                {
                                    this.emul = "";
                                    f28 = null;
                                    InScan = false;
                                    return;
                                }
                                if (!f28.book.mainfund)
                                {
                                    f28.book.RESPAN = "ДП";
                                }
                                else
                                {
                                    f28.book.RESPAN = "Для выдачи";
                                }
                                BookRecordWork = f28.book;
                                BookRecordWork.id = "-1";
                                dbw.InsertMag(BookRecordWork);
                                BookRecordWork = new dbBook(_data, this.BASENAME);
                                BookRecordWork.zal = this.DepName;
                                BookRecordWork.zalid = this.DepID;
                                BookRecordWork.NumbersCount = f28.book.NumbersCount;
                                f28 = null;
                                previous = new dbBook(_data, this.BASENAME);
                            }
                        }
                        if ((BookRecord != null) && (BookRecordWork != null))
                        {
                            MessageBox.Show("Считаны штрихкоды 2-х изданий подряд! Считайте штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.emul = "";
                            InScan = false;
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
                                    this.label12.Text = ReaderRecord.id;
                                    RPhoto.Image = ReaderRecord.Photo;
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
                                    this.emul = "";
                                    InScan = false;
                                    return;
                                }
                            }
                            else
                            {
                                if (ReaderRecordWork != null)
                                {
                                    MessageBox.Show("Считан штрихкод читателя! Считайте штрихкод издания!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.emul = "";
                                    InScan = false;
                                    return;
                                }
                                else//считан штрихкод книги
                                {
                                    string zaliss = BookRecordWork.GetZalIss(this.DepName);
                                    if (zaliss == "")
                                    {
                                        zaliss = this.DepName;
                                    }
                                    string zalret = this.DepName;
                                    if ((BookRecordWork.religion) && (this.DepID != "21"))
                                    {
                                        MessageBox.Show("Эта книга принадлежит НИО религиозной литературы и может быть сдана/выдана только в НИО религиозной литературы!");
                                        CancelIssueInterface();
                                        InScan = false;
                                        return;
                                    }
                                    if ((BookRecordWork.redk) && (this.DepID != "20"))
                                    {
                                        MessageBox.Show("Эта книга принадлежит НИО редкой книги и может быть сдана/выдана только в НИО редкой книги!");
                                        CancelIssueInterface();
                                        InScan = false;
                                        return;
                                    }
                                    if ((BookRecordWork.klass == "ДП") && (this.DepName != BookRecordWork.zal))
                                    {
                                        MessageBox.Show("Эта книга находится на ДП в зале " + BookRecordWork.zal + " и не может быть выдана в этом зале!");
                                        CancelIssueInterface();
                                        InScan = false;
                                        return;
                                    }
                                    if (BookRecordWork.zalid != "29")//если зал абонементного обслуживания, то не надо принимать книгу из к/х
                                    {
                                        if ((!dbw.IsRecievedInHall(BookRecordWork)) && (BookRecordWork.klass != "ДП"))
                                        //&& (BookRecordWork.zalid != "29"))//условие если книга не принята залом, но не книги из зала абонемента!
                                        {
                                            MessageBox.Show("Эта книга не принята залом! Перед выдачей перейдите на вкладку \"Прием кафедра/хранение\" и примите книгу из хранения!");
                                            CancelIssueInterface();
                                            InScan = false;
                                            return;
                                        }
                                    }
                                    if (dbw.isBookBusy(_data))
                                    {
                                        if ((BookRecordWork.additionalNumbers != ""))
                                        {
                                            DialogResult res = MessageBox.Show("Читатель брал следующие номера: " + BookRecordWork.additionalNumbers +
                                                ". Все ли выданные номера сдает читатель?",
                                                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                            if (res == DialogResult.No)
                                            {
                                                CancelIssueInterface();
                                                dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                                                dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);

                                                InScan = false;
                                                return;
                                            }
                                        }
                                        /*if (BookRecordWork.klass != "ДП")
                                            if (BookRecordWork.ord_rid != "-1")
                                                if ((BookRecordWork.RESPAN != BookRecordWork.ord_rid) && (BookRecordWork.GetZalIss(this.DepName) != this.DepID))
                                                {
                                                    MessageBox.Show("Читатель не может сдать книгу в этом зале, так как брал он её с чужого номера в другом зале. Отправьте читателя сдавать книгу в зал " + BookRecordWork.GetZalIss(this.DepName)+"!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    CancelIssueInterface();
                                                    dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                                                    InScan = false;
                                                    return;
                                                }*/
                                        if (dbw.setBookReturned(BookRecordWork))
                                        {
                                            CancelIssueInterface();
                                            dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                                            dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);
                                            InScan = false;
                                            return;
                                        }
                                        //подсветить если другой зал и указания по хранению
                                        int countiss = dbw.getCountIss(BookRecordWork);
                                        //label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();

                                        if (countiss > 1)
                                        {
                                            MessageBox.Show("Принимается " + countiss.ToString() + " номеров.");
                                            BookRecordWork.NumbersCount = countiss;
                                        }
                                        RemoveRow();//удаляем строку если была выдача
                                        RemoveRow_athome();
                                        FillGridReturn(zaliss, zalret);
                                        dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                                        dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);
                                        BookRecord = null;
                                        ReaderRecord = null;
                                        button2.Enabled = false;
                                        button4.Enabled = false;
                                        label1.Text = "Считайте штрихкод издания";
                                        this.emul = "";
                                        label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
                                        label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
                                        InScan = false;
                                        return;
                                    }
                                    else
                                    {
                                        this.label8.Text = BookRecordWork.author;
                                        this.label9.Text = BookRecordWork.name;
                                        this.label13.Text = BookRecordWork.barcode;
                                        this.label1.Text = "Считайте штрих код читателя";
                                        this.button4.Enabled = true;
                                        BookRecord = BookRecordWork.Clone(this.BASENAME);
                                        this.emul = "";
                                        InScan = false;
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
                                this.emul = "";
                                InScan = false;
                                return;
                            }
                            else
                            {
                                this.label1.Text = "Считайте штрих код издания!";
                                BookRecord = null;
                                ReaderRecord = null;
                            }
                        }
                        this.emul = "";
                        break;
                        #endregion
                    case "Справка":
                        label1.Enabled = false;
                        break;
                    case "Прием кафедра/книгохранение":
                        #region Прием книг
                        if (f28 != null)
                        {
                            MessageBox.Show("Вы считали штрихкод во время редактирования сведений об издании! Необходимо сначала закончить редактирование!");
                            break;
                        }
                        if (f31 != null)
                        {
                            if ((this.emul == "") || (this.emul == null))
                            {
                                //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                                _data = FromPort;
                            }
                            else
                            {
                                _data = this.emul;
                            }
                            //_data = _data.Remove(_data.Length - 1, 1);
                            string result = dbw.ChangeHall(_data);
                            switch (result)
                            {
                                case "Эта книга на ДП! Для неё не создаются бронеполки!":
                                    MessageBox.Show("Эта книга на ДП! Для неё не создаются бронеполки!");
                                    break;
                                case "Для этой книги нет бронеполки ни в одном зале!":
                                    MessageBox.Show("Эта книга ни разу не выдавалась и теперь числится за текущим залом!");
                                    break;
                                case "Бронеполка книги успешно перенесена в текущий зал!":
                                    MessageBox.Show("Бронеполка книги успешно перенесена в текущий зал!");
                                    break;
                                case "Уже принята залом!":
                                    MessageBox.Show("Уже принята залом!");
                                    break;
                                case "книга не принята ни одним залом":
                                    MessageBox.Show("Книга не была принята ни одним залом! Нельзя изменить зал бронеполки, нужно сначала принять книгу на кафедре!");
                                    break;
                            }
                            this.emul = "";
                            f31.Close();
                            f31 = null;

                        }
                        else
                        {
                            if ((this.emul == "") || (this.emul == null))
                            {
                                //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                                _data = FromPort;
                            }
                            else
                            {
                                _data = this.emul;
                            }

                            //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();

                            //_data = _data.Remove(_data.Length - 1, 1);
                            dbBook check = new dbBook(_data, this.BASENAME);
                            if ((check.id == "надовводить") || ((check.id == "-1") && (!dbw.isBookBusy(check.barcode))))
                            {
                                string tmp = check.id;
                                check.barcode = _data;
                                f28 = new Form28(check, false, previous);
                                f28.ShowDialog();
                                if (f28.Cancel)
                                {
                                    f28 = null;
                                    this.emul = "";
                                    InScan = false;
                                    return;
                                }
                                if (!f28.book.mainfund)
                                {
                                    f28.book.RESPAN = "ДП";
                                }
                                else
                                {
                                    f28.book.RESPAN = "Для выдачи";
                                }
                                check = f28.book;
                                check.id = "-1";
                                dbw.InsertMag(check);
                                check = new dbBook(_data, this.BASENAME);
                                check.zal = this.DepName;
                                check.zalid = this.DepID;
                                f28 = null;
                                previous = new dbBook(_data, this.BASENAME);
                            }
                            if (check.klass == "ДП")
                            {
                                MessageBox.Show("Эта книга на ДП! Её не нужно принимать залом либо книгохранением!");
                                InScan = false;
                                return;
                            }

                            string result = dbw.RecieveBook(_data);
                            string depn = result.Remove(0, 6);
                            this.emul = "";
                            switch (result)
                            {
                                case "книга в зале абонементного обслуживания":
                                    MessageBox.Show("Книгу не нужно принимать из книгохранения, так как её местонахождение - Зал абонементного обслуживания");
                                    InScan = false;
                                    return;
                                case "Неверный штрихкод":
                                    MessageBox.Show("Считан неверный штрихкод!");
                                    InScan = false;
                                    return;
                                case "не найдено в принятых":
                                    MessageBox.Show("Эту книгу не удалось найти среди принятых какой-дибо кафедрой книг!");
                                    InScan = false;
                                    return;
                                case "книга успешно сдана в хранение":
                                    //MessageBox.Show("Книга успешно сдана в книгохранение!");
                                    break;
                                case "успешно принята залом":
                                    //MessageBox.Show("Книга принята текущим залом!");
                                    break;
                                case "уже принята залом":
                                    //MessageBox.Show("Книга принята текущим залом!");
                                    MessageBox.Show("Эта книга уже принята залом! Вы не можете принять её второй раз!");
                                    InScan = false;
                                    return;
                                case "Читатель не сдал книгу":
                                    MessageBox.Show("Невозможно принять книгу, так как её не провели через систему! Необходимо, чтобы зал принял книгу у читателя через систему! ");
                                    InScan = false;
                                    return;
                            }
                            InScan = false;
                            RecievedBookGUI(new dbBook(_data, this.BASENAME));
                        }
                        InScan = false;
                        break;
                        #endregion
                    case "Учет посещаемости":
                        #region учет посещаемости
                        //_data = ((IOPOSScanner_1_10)sender).ScanData.ToString();
                        _data = FromPort;
                        if (!dbw.isReader(_data))
                        {
                            MessageBox.Show("Неверный штрихкод читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            InScan = false;
                            return;
                        }
                        /*if (_data.Length < 20)
                            _data = _data.Remove(0, 1);*/
                        //_data = _data.Remove(_data.Length - 1, 1);
                        if ((_data[0] == 'G') || (_data[0] == 'V') || (_data[0] == 'Q'))
                        {
                            if (!dbw.WasTodayInCurrentDep(_data))
                            {
                                dbw.AddAttendance(dbw.GetIDGCurr(_data), _data);
                                label21.Text = "На сегодня посещаемость составляет: " + dbw.GetAttendance() + " человек(а)";
                            }
                            else
                            {
                                MessageBox.Show("Этот читатель уже посетил текущий зал сегодня!");
                            }
                            InScan = false;
                            return;
                        }
                        ReaderRecordFormular = new dbReader(_data);

                        if (ReaderRecordFormular.barcode == "notfoundbynumber")
                        {
                            MessageBox.Show("Читатель не найден, либо неверный штрихкод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        if (ReaderRecordFormular.barcode == "numsoc")
                        {
                            MessageBox.Show("Читатель не найден, либо неверный штрикод!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        if (ReaderRecordFormular.barcode == "sersoc")
                        {
                            MessageBox.Show("Не соответствует серия социальной карты!Читатель заменил социальную карту!Номер социальной карты остался прежним, но сменилась серия! Новую социальную карту необходимо зарегистрировать в регистратуре!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            InScan = false;
                            return;
                        }
                        if (!dbw.WasTodayInCurrentDep(_data))
                        {
                            dbw.AddAttendance(ReaderRecordFormular.id, _data);
                        }
                        else
                        {
                            MessageBox.Show("Этот читатель уже посетил текущий зал сегодня!");
                            InScan = false;
                            return;
                        }
                        label21.Text = "На сегодня посещаемость составляет: " + dbw.GetAttendance() + " человек(а)";
                        InScan = false;
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace + " - " + ex.Message +" \r\nЗапишите данное сообщение и обратитесь в отдел автоматизации!");
                InScan = false;
                f28 = null;
            }
            InScan = false;

        }

        private void FillGridReturn(string zaliss, string zalret)
        {
            //zaliss = BookRecordWork.GetZalIss();
            //zalret = this.DepName;
            if (BookRecordWork.ISIssuedAtHome)//прием с выдачи на дом
            {
                dbReader sr = new dbReader(int.Parse(BookRecordWork.rid));
                tabControl2.SelectedTab = tabControl2.TabPages["tabHome"];
                dgwHome.Rows.Insert(0, 1);
                if (BookRecordWork.id == "-1")//если книга не введена в базу
                {
                    dgwHome.Rows[0].Cells[0].Value = BookRecordWork.barcode;
                    dgwHome.Rows[0].Cells[1].Value = BookRecordWork.name + "; " + BookRecordWork.year + "; " + BookRecordWork.number;
                }
                else
                {
                    dgwHome.Rows[0].Cells[0].Value = BookRecordWork.inv;
                    dgwHome.Rows[0].Cells[1].Value = BookRecordWork.name + "; " + BookRecordWork.author;
                }
                dgwHome.Rows[0].Cells[2].Value = BookRecordWork.rname;
                dgwHome.Rows[0].Cells[3].Value = BookRecordWork.rid;
                dgwHome.Rows[0].Cells[4].Value = sr.getReaderRightsString();
                dgwHome.Rows[0].Cells[5].Value = BookRecordWork.FirstIssue.ToString("dd.MM.yyyy");
                dgwHome.Rows[0].Cells[6].Value = DateTime.Now.ToString("dd.MM.yyyy");
                dgwHome.Rows[0].Cells[7].Value = zaliss;
                dgwHome.Rows[0].Cells[8].Value = zalret;
                dgwHome.Rows[0].Cells[9].Value = BookRecordWork.get899a();

                if (BookRecordWork.klass != "ДП")
                {
                    dbw.InsertStatisticsRetBookAtHome(BookRecordWork);
                    //dbw.MoveToHistory(BookRecordWork);
                }
                else
                {
                    dbw.InsertStatisticsRetBookAtHomeDP(BookRecordWork);
                    //dbw.MoveToHistory(BookRecordWork);
                }
                tabControl2.SelectedTab = tabControl2.TabPages["tabHome"];
                if (dbw.ResPanORBookKeeping == DialogResult.No)
                {
                    dbw.MoveToHistory(BookRecordWork);
                }

            }
            else//прием из зала
            {
                tabControl2.SelectedTab = tabControl2.TabPages["tabHall"];

                dbBook stat = BookRecordWork.Clone(this.BASENAME);
                dataGridView1.Rows.Insert(0, 1);
                if (BookRecordWork.id == "-1")//если книга не введена в базу
                {
                    dataGridView1.Rows[0].Cells[0].Value = BookRecordWork.barcode;
                    dataGridView1.Rows[0].Cells[1].Value = BookRecordWork.name + "; " + BookRecordWork.year + "; " + BookRecordWork.number;
                }
                else
                {
                    dataGridView1.Rows[0].Cells[0].Value = BookRecordWork.inv;
                    dataGridView1.Rows[0].Cells[1].Value = BookRecordWork.name + "; " + BookRecordWork.author;
                }
                dataGridView1.Rows[0].Cells[2].Value = BookRecordWork.rname;
                dataGridView1.Rows[0].Cells[3].Value = BookRecordWork.rid;
                dataGridView1.Rows[0].Cells[4].Value = BookRecordWork.RESPAN;
                dataGridView1.Rows[0].Cells[5].Value = BookRecordWork.FirstIssue.ToShortTimeString();
                dataGridView1.Rows[0].Cells[6].Value = DateTime.Now.ToShortTimeString();
                dataGridView1.Rows[0].Cells[7].Value = zaliss;
                dataGridView1.Rows[0].Cells[8].Value = zalret;
                dataGridView1.Rows[0].Cells[9].Value = BookRecordWork.get899a();

                if (dataGridView1.Rows[0].Cells[7].Value.ToString() != dataGridView1.Rows[0].Cells[8].Value.ToString())
                {
                    dataGridView1.Rows[0].Cells[7].Style.BackColor = Color.LightSalmon;
                }
                if ((dataGridView1.Rows[0].Cells[3].Value.ToString() != dataGridView1.Rows[0].Cells[4].Value.ToString()) && (dataGridView1.Rows[0].Cells[4].Value.ToString() != "ДП"))
                {
                    dataGridView1.Rows[0].Cells[3].Style.BackColor = Color.LightSalmon;
                }
                if (BookRecordWork.klass != "ДП")
                {
                    if (BookRecordWork.rid != BookRecordWork.RESPAN)
                    {
                        dataGridView1.Rows[0].Cells[6].Style.BackColor = Color.LightBlue;
                        dbw.MoveToHistoryAlienRespan(BookRecordWork);
                    }
                    else
                    {
                        if (dbw.ResPanORBookKeeping == DialogResult.Yes)
                        {
                            dataGridView1.Rows[0].Cells[6].Style.BackColor = Color.LightBlue;
                            dbw.InsertStatisticsRespan(BookRecordWork.barcode, BookRecordWork.rid, BookRecordWork);
                        }
                        else
                        {
                            dataGridView1.Rows[0].Cells[6].Style.BackColor = Color.LightPink;
                            dbw.InsertStatisticsBookkeeping(BookRecordWork.barcode, BookRecordWork.rid, BookRecordWork);
                            dbw.MoveToHistory(BookRecordWork);

                        }
                    }
                }
                else
                {
                    dbw.InsertStatisticsDP(stat, stat.rid);
                }
                tabControl2.SelectedTab = tabControl2.TabPages["tabHall"];

            }
        }

        private void RemoveRow()
        {
            if (BookRecordWork.id != "-1")
            {
                DataGridViewRow remrow = new DataGridViewRow();
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if ((r.Cells[0].Value.ToString() == BookRecordWork.inv) && (r.Cells[8].Value.ToString() == "-"))
                    {
                        remrow = r;
                    }
                }
                if (remrow.Cells.Count > 1)
                {
                    dataGridView1.Rows.Remove(remrow);
                }
            }
            else
            {
                DataGridViewRow remrow = new DataGridViewRow();
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if ((r.Cells[0].Value.ToString() == BookRecordWork.barcode) && (r.Cells[8].Value.ToString() == "-"))
                    {
                        remrow = r;
                    }
                }
                if (remrow.Cells.Count > 1)
                {
                    dataGridView1.Rows.Remove(remrow);
                }
            }
        }
        private void RemoveRow_athome()
        {
            if (BookRecordWork.id != "-1")
            {
                DataGridViewRow remrow = new DataGridViewRow();
                foreach (DataGridViewRow r in dgwHome.Rows)
                {
                    if ((r.Cells[0].Value.ToString() == BookRecordWork.inv) && (r.Cells[8].Value.ToString() == "-"))
                    {
                        remrow = r;
                    }
                }
                if (remrow.Cells.Count > 1)
                {
                    dgwHome.Rows.Remove(remrow);
                }
            }
            else
            {
                DataGridViewRow remrow = new DataGridViewRow();
                foreach (DataGridViewRow r in dgwHome.Rows)
                {
                    if ((r.Cells[0].Value.ToString() == BookRecordWork.barcode) && (r.Cells[8].Value.ToString() == "-"))
                    {
                        remrow = r;
                    }
                }
                if (remrow.Cells.Count > 1)
                {
                    dgwHome.Rows.Remove(remrow);
                }
            }
        }

        void RecievedBookGUI(dbBook book)
        {
            RecBooks.Rows.Insert(0,1);
            //RecBooks.Rows.Add();
            RecBooks.Rows[0].Cells[1].Value = ((book.inv == "-1") || (book.inv == null))? book.barcode : book.inv;
            RecBooks.Rows[0].Cells[2].Value = book.name;
            RecBooks.Rows[0].Cells[3].Value = book.author;
            autodec(RecBooks);
        }
        public static event ScannedEventHandler Scanned;
        public void FireScan(object sender, EventArgs ev)
        {
            if (Form1.Scanned != null)
                Form1.Scanned(sender, ev);
            label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
            label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            f2.textBox2.Focus();
            f2.textBox2.Text = "";
            f2.textBox3.Text = "";
            f2.ShowDialog();
            if ((this.EmpID == "") || (this.EmpID == null))
            {
                MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
            label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label15.Text = "Книг на руках: "+dbw.getCountIssuedBooks();
            label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
        }

        public void button2_Click_1(object sender, EventArgs e)
        {
            if ((((ReaderRecord.ReaderRights & dbReader.Rights.PERS) != dbReader.Rights.PERS)) && (this.DepID == "29"))
            {
                DialogResult dr = MessageBox.Show("У читателя нет прав платного абонемента! Всё равно выдать?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.No)
                {
                    CancelIssueInterface();
                    return;
                }

            }
            if ((((ReaderRecord.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS)) && (this.DepID == "29"))
            {
                DateTime? dt = ReaderRecord.GetDateEndPersAbonement();
                if ((dt < DateTime.Now) || (dt == null))
                {
                    DialogResult dr = MessageBox.Show("У читателя закончился срок платного абонемента! Всё равно выдать?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.No)
                    {
                        CancelIssueInterface();
                        return;
                    }
                }
            }
            if ((((ReaderRecord.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)) && (this.DepID == "29"))
            {
                DateTime? dt = ReaderRecord.GetDateEndIndividualPersAbonement();
                if (dt == null)
                {
                }
                else
                {
                    if (dt < DateTime.Now)
                    {
                        DialogResult dr = MessageBox.Show("У читателя закончился срок бесплатного абонемента! Всё равно выдать?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (dr == DialogResult.No)
                        {
                            CancelIssueInterface();
                            return;
                        }
                    }
                }
            }
            if ((ReaderRecord.CanGetAtHome == false) && (ReaderRecord.Department != "1"))
            {
                MessageBox.Show("У читателя либо не проставлены права сотрудника, либо он бывший сотрудник. Необходимо обратиться в регистратуру и либо выставить права сотрудника, либо убрать отдел в котором читатель работал. В противном случае выдача не будет возможна.");
                CancelIssueInterface();
                return;
            }
            DialogResult res;
            if (dbw.ReaderHaveDedts(ReaderRecord))
            {
                res = MessageBox.Show("Этот читатель имеет задолженности! Всё равно выдать?","Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                if (res == DialogResult.No)
                {
                    CancelIssueInterface();
                    return;
                }
            }
            
            if (dbw.IsReaderEntered(ReaderRecord))
            {
                if (dbw.isBookBusy(BookRecord.barcode))
                {
                    MessageBox.Show("Книга у другого читателя (" + BookRecord.rname + ")!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CancelIssueInterface();
                    return;
                }
                else
                {
                    if (SetBookForReaderInterface())
                    {
                        BookRecord = null;
                        ReaderRecord = null;
                        return;
                    }
                }
                BookRecord = null;
                ReaderRecord = null;
                dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);
                label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
                label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
            }
            else
            {
                DialogResult dr = MessageBox.Show("Читатель не зафиксирован как вошедший через центральный вход. Возможно он вошел не по читательскому билету библиотеки! Вы все равно хотите выдать книгу?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Error,MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    if (dbw.isBookBusy(BookRecord.barcode))
                    {
                        MessageBox.Show("Книга у другого читателя (" + BookRecord.rname + ")!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        CancelIssueInterface();
                        return;
                    }
                    else
                    {
                        if (SetBookForReaderInterface())
                        {
                            BookRecord = null;
                            ReaderRecord = null;
                            return;
                        }
                    }
                    BookRecord = null;
                    ReaderRecord = null;
                    dateTimePicker1.Value = DateTime.Now.Date.AddDays(3);
                    dateTimePicker2.Value = DateTime.Now.Date.AddDays(30);
                    label15.Text = "Книг на руках: " + dbw.getCountIssuedBooks();
                    label16.Text = "Книг на руках: " + dbw.GetCountIssuedHomeBooks();
                }
                else
                {
                    CancelIssueInterface();
                    return;
                }
            }
        }
        private void CancelIssueInterface()
        {
            this.label8.Text = "";
            this.label9.Text = "";
            this.label13.Text = "";
            this.label5.Text = "";
            this.label12.Text = "";
            RPhoto.Image = null;
            BookRecord = null;
            ReaderRecord = null;
            button2.Enabled = false;
            button4.Enabled = false;
            label1.Text = "Считайте штрихкод издания";
        }
        bool wantAbonementAlienRespanOrVistavka = false;//если хочет взять книгу заказанную другим читателем с чужой бронеполки
        bool homezal = false;//false - zal
        private bool SetBookForReaderInterface()
        {
                        //вот тут начинаются чудеса))
            //if (!wantAbonementAlienRespan)
            //{
            if (((((ReaderRecord.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS) ||
               ((ReaderRecord.ReaderRights & dbReader.Rights.COLL) == dbReader.Rights.COLL))) && !homezal)//если коллективный или персональный
            {
                DialogResult colper = MessageBox.Show("Читатель с правами платного или коллективного абонемента хочет взять книгу. "+
                    "\nДа - выдать книгу на дом "+
                    "\nНет - выдать книгу в залы ",
                    "Вопрос", MessageBoxButtons.YesNo);
                if (colper == DialogResult.No)
                {
                    ReaderRecord.CanGetAtHome = false;
                    homezal = true;
                    if (SetBookForReaderInterface())
                    {
                        homezal = false;
                        return true;
                    }
                    else
                    {
                        homezal = false;
                        return false;
                    }
                }
            }

            if (((  ((ReaderRecord.CanGetAtHome) && (this.DepID != "20")) || (BookRecord.getFloor().Contains("Абонемент")) )
                    && (!wantAbonementAlienRespanOrVistavka) && !homezal)
                    || ((BookRecord.get899b().ToLower() == "вх") && (this.DepID == "22")))    //выдача домой
            {




                wantAbonementAlienRespanOrVistavka = false;
                /*if ((BookRecord.getFloor() == "Зал абонементного обслуживания") && (BookRecord.klass == "ДП"))
                {
                    DialogResult dr = MessageBox.Show("Читатель хочет взять книгу находящуюся на ДП в зале абонементного обслуживания. " +
                                                            "\nДа - выдать книгу на дом " +
                                                            "\nНет - выдать книгу в залы ",
                                                            "Вопрос", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {

                    }

                }*/
                if ((BookRecord.get899b().ToLower() == "вх") && (this.DepID == "22"))
                {
                    BookRecord.klass = "Для выдачи";
                    BookRecord.RESPAN = "";
                }

                if (BookRecord.klass == "ДП")
                {
                    //dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                    ReaderRecord.CanGetAtHome = false;
                    if (BookRecord.getFloor().Contains("Абонемент"))
                        wantAbonementAlienRespanOrVistavka = true;
                    if (SetBookForReaderInterface())
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (BookRecord.GetRealKlass() == "Выставка")
                    {
                        DialogResult dr = MessageBox.Show("Нельзя выдать на дом книгу, находящуюся на выставке. Книгу можно выдать в зал! Выдать книгу читателю в зал?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            ReaderRecord.CanGetAtHome = false;
                            if (BookRecord.getFloor().Contains("Абонемент"))
                                wantAbonementAlienRespanOrVistavka = true;

                            if (SetBookForReaderInterface())
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            CancelIssueInterface();
                            return true;
                        }
                    }
                    else
                    {
                        if (dbw.OrderedAtAll(BookRecord))//если заказана вообще 
                        {
                            if (dbw.IsOrderedByCurrentReader(BookRecord, ReaderRecord))
                            {
                                if (BookRecord.getFloor().Contains("Абонемент"))
                                {
                                    if ((ReaderRecord.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)
                                    {
                                        dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                    }
                                    else
                                    {
                                        DialogResult dr = MessageBox.Show("У читателя нет прав бесплатного абонемента. Хотите выдать ему такие права вместе с книгой?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        if (dr == DialogResult.Yes)
                                        {
                                            ReaderRecord.setReaderRight();
                                            ReaderRecord = new dbReader(ReaderRecord.IntID);

                                            if (SetBookForReaderInterface())
                                            {
                                                homezal = false;
                                                return true;
                                            }
                                            else
                                            {
                                                homezal = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            CancelIssueInterface();
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                }
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("Нельзя выдать на дом книгу, заказанную другим читателем. Книгу можно взять только до прихода другого читателя! Выдать книгу читателю?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                {
                                    ReaderRecord.CanGetAtHome = false;
                                    if (BookRecord.getFloor().Contains("Абонемент"))
                                        wantAbonementAlienRespanOrVistavka = true;
                                    if (SetBookForReaderInterface())
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                {
                                    CancelIssueInterface();
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if ((BookRecord.RESPAN == "") || (BookRecord.RESPAN == null))//нет бронеполки. выдается впервые без заказа
                            {
                                if (BookRecord.getFloor().Contains("Абонемент"))
                                {
                                    if ((ReaderRecord.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)
                                    {
                                        dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                    }
                                    else
                                    {
                                        DialogResult dr = MessageBox.Show("У читателя нет прав бесплатного абонемента. Хотите выдать ему такие права вместе с книгой?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        if (dr == DialogResult.Yes)
                                        {
                                            ReaderRecord.setReaderRight();
                                            ReaderRecord = new dbReader(ReaderRecord.IntID);
                                            dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                        }
                                        else
                                        {
                                            CancelIssueInterface();
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    if ((ReaderRecord.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)
                                    {
                                        dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                    }
                                    else
                                    {
                                        DialogResult dr = MessageBox.Show("У читателя нет прав бесплатного абонемента. Хотите выдать ему такие права вместе с книгой?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                        if (dr == DialogResult.Yes)
                                        {
                                            ReaderRecord.setReaderRight();
                                            ReaderRecord = new dbReader(ReaderRecord.IntID);
                                            dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                        }
                                        else
                                        {
                                            CancelIssueInterface();
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (BookRecord.RESPAN != ReaderRecord.id)  //чужая бронеполка
                                {
                                    DialogResult dr = MessageBox.Show("Вы не можете взять книгу на дом с чужой бронеполки. Книгу можно взять только до прихода другого читателя! Выдать книгу читателю?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dr == DialogResult.Yes)
                                    {
                                        ReaderRecord.CanGetAtHome = false;
                                        if (BookRecord.getFloor().Contains("Абонемент"))
                                            wantAbonementAlienRespanOrVistavka = true;
                                        if (SetBookForReaderInterface())
                                            return true;
                                        else
                                            return false;

                                    }
                                    else
                                    {
                                        CancelIssueInterface();
                                        return true;
                                    }
                                }
                                else//выдается по заказу
                                {
                                    if (BookRecord.getFloor().Contains("Абонемент"))
                                    {
                                        if ((ReaderRecord.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)
                                        {
                                            dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                        }
                                        else
                                        {
                                            DialogResult dr = MessageBox.Show("У читателя нет прав индивидуального абонемента. Хотите выдать ему такие права вместе с книгой?", " Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                            if (dr == DialogResult.Yes)
                                            {
                                                ReaderRecord.setReaderRight();
                                                ReaderRecord = new dbReader(ReaderRecord.IntID);
                                                dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                            }
                                            else
                                            {
                                                CancelIssueInterface();
                                                return true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dbw.setBookForReaderHome(BookRecord, ReaderRecord);
                                    }
                                }
                            }
                        }
                    }
                }
                //на дом
                dbw.InsertStatisticsIssBookAtHome(BookRecord, ReaderRecord);

                tabControl2.SelectedTab = tabControl2.TabPages["tabHome"];
                dgwHome.Rows.Insert(0, 1);
                if (BookRecord.id == "-1")
                {
                    dgwHome.Rows[0].Cells[0].Value = BookRecord.barcode;
                    dgwHome.Rows[0].Cells[1].Value = BookRecord.name + "; " + BookRecord.year + "; " + BookRecord.number;
                }
                else
                {
                    dgwHome.Rows[0].Cells[0].Value = BookRecord.inv;
                    dgwHome.Rows[0].Cells[1].Value = BookRecord.name + "; " + BookRecord.author;
                }
                //BookRecord = new dbBook(BookRecord.barcode);
                dgwHome.Rows[0].Cells[2].Value = ReaderRecord.FIO;
                dgwHome.Rows[0].Cells[3].Value = ReaderRecord.id;
                dgwHome.Rows[0].Cells[4].Value = ReaderRecord.getReaderRightsString();
                dgwHome.Rows[0].Cells[5].Value = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
                dgwHome.Rows[0].Cells[6].Value = "-";
                dgwHome.Rows[0].Cells[7].Value = BookRecord.GetZalIss(this.DepName);
                dgwHome.Rows[0].Cells[8].Value = BookRecord.GetZalRet();
                dgwHome.Rows[0].Cells[8].Value = BookRecord.get899a();
                if (dgwHome.Rows[0].Cells[8].Value.ToString() == "")
                {
                    dgwHome.Rows[0].Cells[8].Value = "-";
                }
                CancelIssueInterface();
            }
            else//выдача в залы
            {
                wantAbonementAlienRespanOrVistavka = false;
                List<dbBook> issb = dbw.FindBooksIssuedAnotherReaders(ReaderRecord);
                if (issb.Count != 0)
                {
                    string issbnames = "";
                    foreach (dbBook b in issb)
                    {
                        issbnames += b.name + "; ";
                    }
                    MessageBox.Show("С бронеполки данного читателя выдана(ы) следующая(ие) книга(и): " + issbnames);
                }
                if ((BookRecord.klass == "ДП") && (this.DepID != BookRecord.zalid))
                {
                    MessageBox.Show("Эта книга находится на ДП в другом зале(" + BookRecord.zal + "). Вы не можете выдать её в этом зале!");
                    CancelIssueInterface();
                    return true;//неудача
                }
                if ((BookRecord.klass == "Для выдачи") || (BookRecord.klass == "ДП"))
                {
                    dbBook IssB = dbw.FindBookInIssued(BookRecord, ReaderRecord);
                    if (IssB == null)
                    {
                        if (BookRecord.klass != "ДП")
                        {
                            if (dbw.OrderedAtAll(BookRecord))
                            {
                                BookRecord.RESPAN = BookRecord.ord_rid;
                                ReaderRecord.rlocation = this.DepName;
                            }
                            else
                            {
                                //Form27 f27 = new Form27(this);
                                //f27.textBox1.Text = ReaderRecord.id;
                                //f27.ShowDialog();
                                BookRecord.RESPAN = ReaderRecord.id;
                                ReaderRecord.rlocation = this.DepName;
                            }
                        }

                        dbw.setBookForReader(BookRecord, ReaderRecord);


                        if (dbw.NEED_REQUIRMENT)
                        {
                            MessageBox.Show("Необходимо распечатать требование для этой книги, т.к. её берет другой читатель, а она подготовлена к сдаче в хранение!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        if (BookRecord.RESPAN != ReaderRecord.id)
                        {
                            Form30 f30 = new Form30(this);
                            //f30.comboBox2.Text = this.DepName;
                            f30.ShowDialog();
                            ReaderRecord.rlocation = f30.rlocation;
                        }
                        if (dbw.setResBookForReader(IssB, ReaderRecord))
                        {
                            CancelIssueInterface();
                            return true;//неудача
                        }
                    }
                }
                else
                {
                    MessageBox.Show("У книги неверный класс издания! Текущий класс издания - \"" + BookRecord.klass + "\". Класс издания может быть либо \"ДП\" либо \"Для выдачи\". Выдача невозможна!");
                    CancelIssueInterface();
                    return true;//неудача
                }


                //Интерфейс
                {

                    tabControl2.SelectedTab = tabControl2.TabPages["tabHall"];
                    dataGridView1.Rows.Insert(0, 1);
                    if (BookRecord.id == "-1")
                    {
                        dataGridView1.Rows[0].Cells[0].Value = BookRecord.barcode;
                        dataGridView1.Rows[0].Cells[1].Value = BookRecord.name + "; " + BookRecord.year + "; " + BookRecord.number;
                    }
                    else
                    {
                        dataGridView1.Rows[0].Cells[0].Value = BookRecord.inv;
                        dataGridView1.Rows[0].Cells[1].Value = BookRecord.name + "; " + BookRecord.author;
                    }
                    //BookRecord = new dbBook(BookRecord.barcode);
                    dataGridView1.Rows[0].Cells[2].Value = ReaderRecord.FIO;
                    dataGridView1.Rows[0].Cells[3].Value = ReaderRecord.id;
                    dataGridView1.Rows[0].Cells[4].Value = BookRecord.RESPAN;
                    dataGridView1.Rows[0].Cells[5].Value = DateTime.Now.ToShortTimeString();
                    dataGridView1.Rows[0].Cells[6].Value = "-";
                    dataGridView1.Rows[0].Cells[7].Value = BookRecord.GetZalIss(this.DepName);
                    dataGridView1.Rows[0].Cells[8].Value = BookRecord.GetZalRet();
                    dataGridView1.Rows[0].Cells[9].Value = BookRecord.get899a();
                    if (dataGridView1.Rows[0].Cells[8].Value.ToString() == "")
                    {
                        dataGridView1.Rows[0].Cells[8].Value = "-";
                    }
                    if ((ReaderRecord.id != BookRecord.RESPAN) && (BookRecord.RESPAN != "ДП"))
                    {
                        dataGridView1.Rows[0].Cells[3].Style.BackColor = Color.LightSalmon;
                    }
                    if (dataGridView1.Rows[0].Cells[3].Style.BackColor != Color.LightSalmon)//если взял тот читатель, чья бронеполка то
                    {
                        dbw.InsertStatisticsIssuedBooks(ReaderRecord.id, BookRecord);
                    }
                    else
                    {
                        dbw.InsertStatisticsIssuedBooksAnotherReader(ReaderRecord.id, BookRecord);
                    }

                    CancelIssueInterface();
                }
            }
            return false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            CancelIssueInterface();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Text)
            {
                case "Приём/выдача изданий":
                    label1.Enabled = true;
                    label1.Text = "Считайте штрихкод издания";
                    this.AcceptButton = button2;
                    break;
                case "Справка":
                    label1.Enabled = false;
                    if (Statistics.Columns != null)
                    {
                        Statistics.Columns.Clear();
                        label19.Text = "";
                        button15.Visible = false;
                    }
                    break;
                case "Формуляр читателя":
                    label25.Text = "";
                    label20.Text = "";
                    label27.Text = "";
                    pictureBox2.Image = null;
                    Formular.Columns.Clear();
                    AcceptButton = this.button10;
                    readerRightsView1.Clear();
                    break;
                case "Учет услуг":
                    if (this.EmpID == "1")
                    {
                        button27.Enabled = true;
                        button28.Enabled = true;
                    }
                    else
                    {
                        button27.Enabled = false;
                        button28.Enabled = false;
                    }
                    ShowFreeServices();
                    ShowPaidServices();
                    break;
                case "Учет посещаемости":
                    label21.Text = "На сегодня посещаемость составляет: "+dbw.GetAttendance()+" человек(а)";
                    break;
            }
        }


        private void ShowFreeServices()
        {
            FreeServiceGrid.AutoGenerateColumns = true;
            FreeServiceGrid.DataSource = dbw.getFreeServices();
            FreeServiceGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            FreeServiceGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FreeServiceGrid.Columns[0].Width = 50;
            FreeServiceGrid.Columns[0].HeaderText = "№ п/п";
            FreeServiceGrid.Columns[1].Width = 250;
            FreeServiceGrid.Columns[1].HeaderText = "Наименование услуги";
            FreeServiceGrid.Columns[2].Width = 110;
            FreeServiceGrid.Columns[2].HeaderText = "Кол-во";
            FreeServiceGrid.ReadOnly = true;
            autoinc(FreeServiceGrid);
        }
        private void ShowPaidServices()
        {
            PaidServiceGrid.AutoGenerateColumns = true;
            PaidServiceGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            PaidServiceGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            PaidServiceGrid.DataSource = dbw.getPaidServices();
            PaidServiceGrid.Columns[0].Width = 50;
            PaidServiceGrid.Columns[0].HeaderText = "№ п/п";
            PaidServiceGrid.Columns[1].Width = 250;
            PaidServiceGrid.Columns[1].HeaderText = "Наименование услуги";
            PaidServiceGrid.Columns[2].Width = 70;
            PaidServiceGrid.Columns[2].HeaderText = "Кол-во";
            PaidServiceGrid.Columns[3].Width = 40;
            PaidServiceGrid.Columns[3].HeaderText = "Цена";
            PaidServiceGrid.Columns[4].Width = 50;
            PaidServiceGrid.Columns[4].HeaderText = "Стои мость";
            PaidServiceGrid.Columns[5].Visible = false;
            PaidServiceGrid.ReadOnly = true;
            autoinc(PaidServiceGrid);
        }
        private void button25_Click(object sender, EventArgs e)
        {
            Form35 f35 = new Form35(this,0,"0");
            f35.ShowDialog();
            ShowFreeServices();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if ((this.EmpID == "") || (this.EmpID == null) || (this.DepID == null) || (this.DepID == ""))
            {
                MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button11.Enabled = false;
            button18.Enabled = false;
            int x = this.Left + button7.Left;
            int y = this.Top + button7.Top + tabControl1.Top + 60;
            contextMenuStrip2.Show(x, y);
            Statistics.Scroll -= new ScrollEventHandler(Statistics_Scroll);
            Statistics.CellValueNeeded -= new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
        }

        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
        }
        public void autodec(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = dgv.Rows.Count - (i++);
            }
        }

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
            if (label19.Text == "Все книги текущего зала")
            {
                System.Windows.Forms.SortOrder so = Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
                Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[4].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[5].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[6].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[7].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                switch (e.ColumnIndex)
                {
                    case 1:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "zag ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "zag DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 2:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "avt ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "avt DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 3:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "inv ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "inv DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 4:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "stp ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "stp DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 5:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "stat ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "stat DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 6:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "n ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "n DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 7:
                        if ((so == System.Windows.Forms.SortOrder.None) || (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "cnt ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "cnt DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                }
                VirtualTable = VirtualTable.DefaultView.ToTable();

                Statistics.Refresh();
            }
            if (label19.Text.Contains("Обращаемость"))
            {
                System.Windows.Forms.SortOrder so = Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
                Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[4].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                switch (e.ColumnIndex)
                {
                    case 1:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "zag ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "zag DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 2:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "avt ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "avt DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 3:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "inv ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "inv DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 4:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "cnt ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "cnt DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                }
            }
            if (label19.Text.Contains("Список все книг находящихся в открытом доступе в зале абонементного обслуживания"))
            {
                System.Windows.Forms.SortOrder so = Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection;
                Statistics.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[2].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[3].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[4].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[5].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[6].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[7].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[8].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                Statistics.Columns[9].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                switch (e.ColumnIndex)
                {
                    case 1:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "pol ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "pol DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 2:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "zag ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "zag DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 3:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "avt ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "avt DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 4:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "god ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "god DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 5:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "inv ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "inv DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 6:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "spr ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "spr DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 7:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "vyd ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "vyd DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 8:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "vaj ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "vaj DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 9:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "lng ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "lng DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                    case 10:
                        if ((so == System.Windows.Forms.SortOrder.None) ||
                        (so == System.Windows.Forms.SortOrder.Descending))
                        {
                            VirtualTable.DefaultView.Sort = "tema ASC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                        }
                        else
                        {
                            VirtualTable.DefaultView.Sort = "tema DESC";
                            Statistics.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                        }
                        break;
                }
            }
                VirtualTable = VirtualTable.DefaultView.ToTable();
                autoinc(Statistics);
                Statistics.Refresh();
            
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //dbw.GetFormular("1000001");
            this.label27.Text = "";
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
            ReaderSetBarcode = new dbReader((int)numericUpDown3.Value, "formular");
            if (ReaderSetBarcode.barcode == "Читателю не присвоен штрихкод и нет социальной карты")
            {
                MessageBox.Show("Читателю не присвоен штрихкод и нет социальной карты!");
                return;
            }
            if (ReaderSetBarcode.barcode == "error")
            {
                MessageBox.Show("Читатель не найден!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (ReaderSetBarcode.barcode == "notfoundbynumber")
            {
                MessageBox.Show("Читатель не найден!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dbw.GetFormular(ReaderSetBarcode.id);
            label20.Text = ReaderSetBarcode.Surname + " " + ReaderSetBarcode.Name + " " + ReaderSetBarcode.SecondName;
            label25.Text = ReaderSetBarcode.id;
            pictureBox2.Image = ReaderSetBarcode.Photo;
            ReaderRecordFormular = new dbReader(ReaderSetBarcode);
            this.FormularColumnsForming(ReaderSetBarcode.id);
            //label29.Text = dbw.GetReaderRights(ReaderSetBarcode.id);
            readerRightsView1.Init((int)numericUpDown3.Value);
        }


        private void Formular_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex == 1) && ((Sorting.ZagSort == SortDir.Asc)))
            {
                Formular.Sort(Formular.Columns[2], ListSortDirection.Descending);
                Formular.Columns[1].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;
                Sorting.ZagSort = SortDir.Desc;
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

        System.Drawing.Printing.PrintDocument pd;
        DataGridViewPrinter prin;

        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = prin.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {

            //if (label25.Text == "")
            //{
            //    MessageBox.Show("Читатель не выбран! Сначала выберите читателя!");
            //    return;
            //}
            //dbReader reader = new dbReader(int.Parse(label25.Text));

            //Conn.ReaderDA.SelectCommand.CommandText = "select * from Main where";

            ////Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = reader.id;
            ////Conn.SQLDA.SelectCommand.CommandText = "with A as (select zagp.PLAIN zag,avtp.PLAIN avt, B.INV inv, B.DATE_ISSUE iss,B.DATE_RET vzv  " +
            ////                                       "  from BJVVV..DATAEXT A  " +
            ////                                       " inner join " + BASENAME + "..ISSUED_OF B on B.IDDATA = A.IDDATA " +
            ////                                       " left join BJVVV..DATAEXT zag on zag.MNFIELD = 200 and zag.MSFIELD = '$a' and zag.IDMAIN = A.IDMAIN " +
            ////                                       " left join BJVVV..DATAEXT avt on avt.MNFIELD = 700 and avt.MSFIELD = '$a' and avt.IDMAIN = A.IDMAIN " +
            ////                                       " left join BJVVV..DATAEXTPLAIN zagp on zagp.IDDATAEXT = zag.ID " +
            ////                                       " left join BJVVV..DATAEXTPLAIN avtp on avtp.IDDATAEXT = avt.ID " +
            ////                                       " where B.IDREADER = "+reader.id+" and B.IDMAIN != 0 and A.MNFIELD = 899 and A.MSFIELD = '$w') "+
            ////                                       " select A1.zag, (select A2.avt +'; ' from A A2 where A1.zag = A2.zag for xml path('')) avtfull, A1.inv, A1.iss, A1.vzv from A A1 group by A1.zag,A1.inv, A1.iss, A1.vzv ";


            //Conn.SQLDA.SelectCommand.CommandText = "  with A as (select zagp.PLAIN zag,avtp.PLAIN avt, B.INV inv, B.DATE_ISSUE iss,B.DATE_RET vzv , B.ID idm " +
            //"  from BJVVV..DATAEXT A  " +
            //"   inner join Reservation_R..ISSUED_OF B on B.IDDATA = A.IDDATA " +
            //"   left join BJVVV..DATAEXT zag on zag.MNFIELD = 200 and zag.MSFIELD = '$a' and zag.IDMAIN = A.IDMAIN " +
            //"   left join BJVVV..DATAEXT avt on avt.MNFIELD = 700 and avt.MSFIELD = '$a' and avt.IDMAIN = A.IDMAIN " +
            //"   left join BJVVV..DATAEXTPLAIN zagp on zagp.IDDATAEXT = zag.ID " +
            //"   left join BJVVV..DATAEXTPLAIN avtp on avtp.IDDATAEXT = avt.ID " +
            //"   where  B.IDREADER = " + reader.id + " and B.IDMAIN != 0 and A.MNFIELD = 899 and A.MSFIELD = '$w') " +

            //"   select A1.zag, " +
            //"                  (select A2.avt +'; ' " +
            //"                   from A A2 " +
            //"                   where A1.idm = A2.idm for xml path('')) avtfull, A1.inv, A1.iss, A1.vzv  " +
            //"   from A A1 " +
            //"   group by A1.zag,A1.inv, A1.iss, A1.vzv , A1.idm ";

            //Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //DataSet R = new DataSet();
            ////R.Tables.Add("form");

            //int i = Conn.SQLDA.Fill(R,"form");

            //CrystalReport1 cr1 = new CrystalReport1();
            //cr1.SetDataSource(R.Tables["form"]);
            //crystalReportViewer1.ReportSource = cr1;

            //CrystalDecisions.CrystalReports.Engine.TextObject txtReaderName;
            //CrystalDecisions.CrystalReports.Engine.TextObject txtReaderNum;
            //txtReaderName = cr1.ReportDefinition.ReportObjects["Text19"] as TextObject;
            //txtReaderNum = cr1.ReportDefinition.ReportObjects["Text20"] as TextObject;

            //txtReaderName.Text = reader.Surname + " " + reader.Name + " " + reader.SecondName;
            //txtReaderNum.Text = reader.id;
            ////crystalReportViewer1.PrintReport();
            //cr1.PrintToPrinter(1, false, 1, 99999);

            if (Formular.Rows.Count == 0)
            {
                MessageBox.Show("Нечего экспортировать!");
                return;
            }
            DataTable dt = (DataTable)Formular.DataSource;

            StringBuilder fileContent = new StringBuilder();

            foreach (DataGridViewColumn dc in Formular.Columns)
            {
                if (!dc.Visible) continue;
                fileContent.Append(dc.HeaderText + ";");
            }

            fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);



            foreach (DataGridViewRow dr in Formular.Rows)
            {

                foreach (DataGridViewCell cell in dr.Cells)
                {
                    if (!cell.OwningColumn.Visible) continue;
                    fileContent.Append("\"" + cell.Value.ToString() + "\";");
                }

                fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            string tmp = "Формуляр_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            tmp = "Формуляр_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Сохранить в файл";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            //TextWriter tw;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(sd.FileName, fileContent.ToString(), Encoding.UTF8);
            }

        }




        private void Form1_Shown(object sender, EventArgs e)
        {
            //tabControl1.TabPages.RemoveAt(1);
        }
        public string emul;
        public string pass;
        private void button14_Click(object sender, EventArgs e)
        {
            //Form20 f20 = new Form20(this);
            //f20.ShowDialog();
           // if (pass == "aa")
            {
                pass = "";
                Form19 f19 = new Form19(this);
                f19.ShowDialog();
                Form1_Scanned(sender, new EventArgs());
            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            Form7 f7 = new Form7(ReaderRecordFormular,this.BASENAME);
            f7.ShowDialog();
        }
        private void button17_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            Form9 f9 = new Form9(ReaderRecordFormular);
            f9.ShowDialog();
        }
        private void button21_Click(object sender, EventArgs e)
        {
            //поиск читателя по фамилии
            Form16 f16 = new Form16(this);
            f16.ShowDialog();
        }
        public void FrmlrFam(string id)
        {
            ReaderRecordFormular = new dbReader(int.Parse(id));
            if (ReaderRecordFormular.barcode == "Читателю не присвоен штрихкод и нет социальной карты")
            {
                MessageBox.Show("Читателю не присвоен штрихкод и нет социальной карты. ничего никогда не выдавалось и не принималось");
                return;
            }
            dbw.GetFormular(ReaderRecordFormular.id);
            ReaderSetBarcode = new dbReader(ReaderRecordFormular);
            label20.Text = ReaderRecordFormular.Surname + " " + ReaderRecordFormular.Name + " " + ReaderRecordFormular.SecondName;
            label25.Text = ReaderRecordFormular.id;
            pictureBox2.Image = ReaderRecordFormular.Photo;
            FormularColumnsForming(ReaderRecordFormular.id);
        }

        Form25 f25;
        private void button20_Click(object sender, EventArgs e)
        {
            f25 = new Form25(dbw,this);
            f25.ShowDialog();
            f25 = null;
        }

        private void книгиЛежащиеНаБронеполкахНоНиРазуНевыданныеЧитателюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги на бронеполках, но ни разу не выданные читателям";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetBooksOnRESPANButNotEverIssued();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв. номер/шкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Дата приема";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Расст. шифр";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Местона хождение";
            Statistics.Columns[6].Width = 150;
            Statistics.Columns[7].Visible = false;
            Statistics.Columns[8].HeaderText = "Для читателя №";
            Statistics.Columns[8].Width = 90;
            button1.Enabled = true;
        }





        int ProlongDays = 30;
        private void button5_Click(object sender, EventArgs e)
        {
            if (Formular.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка в таблице!");
                return;
            }
            Form29 f29 = new Form29(ProlongDays);
            f29.ShowDialog();
            if (f29.daysBookingShelf > 0)
            {
                ProlongDays = f29.days;
                foreach (DataGridViewRow r in Formular.SelectedRows)
                {
                    string zi = r.Cells["zi"].Value.ToString();
                    DateTime dend = (DateTime)r.Cells["dend"].Value;

                    if (dbw.IsAtHome(zi))
                    {
                        dend = dend.AddDays(f29.days);
                    }
                    else
                    {
                        dend = dend.AddDays(f29.daysBookingShelf);
                    }


                    if (dbw.ProlongReservation(dend, zi))
                    {
                        continue;
                    }
                    //dbReader reader  = new dbReader(int.Parse(label25.Text));
                    dbBook book = new dbBook(r.Cells["bar"].Value.ToString(),this.BASENAME);
                    dbw.InsertStatisticsReservationProlonged(book, label25.Text);
                    r.Cells["dend"].Value = dend;
                }
            }
            else
            {
                MessageBox.Show("Количество дней не может быть отрицательным.");
                return;
            }
            dbw.GetFormular(label25.Text);
            this.FormularColumnsForming(label25.Text);
        }

        private void книгиВыданныеСЧужойБронеполкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги выданные с чужой бронеполки";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooksFromAnotherRespan();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Номер бронеполки";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Дата окончания брони";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Местонахождение читателя";
            Statistics.Columns[7].Width = 90;

        }

        private void книгиНаБронеполкеСИстекшимСрокомБрониToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги на бронеполке с истекшим сроком брони";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooksWithExpiredReservation();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Номер бронеполки";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Дата окончания брони";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Местонахождение читателя";
            Statistics.Columns[7].Width = 90;
            Statistics.Columns[8].Visible = false;

            button1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form20 f20 = new Form20(this);
            f20.ShowDialog();
            if (pass == "aa")
            {
                pass = "";
                Form19 f19 = new Form19(this);
                f19.ShowDialog();
                Form1_Scanned(sender, new EventArgs());
            }
        }

        private void индивидуальнаяСправкаЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            if (Conn.SQLDA.SelectCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.SelectCommand.Connection.Open();
            }
            label19.Text = "Отчет индивидуальный за период с " + f3.StartDate.ToString("dd.MM.yy") + " по " + f3.EndDate.ToString("dd.MM.yy");
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..RecievedBooks " +
                                                   " where Cast(Cast(DATESTART As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and IDEMP = '" + this.EmpID + "'";
            DataSet DS = new DataSet();
            int s1 = Conn.SQLDA.Fill(DS, "t");//получили изданий из книгохранения
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 2 and IDEMP = '" + this.EmpID + "'";
            DS = new DataSet();
            int s2 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s2 = Conn.SQLDA.Fill(DS, "t");//выдано изданий читателю из кх в зал
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select IDREADER from " + BASENAME + "..[Statistics] " +
                                                   " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and (ACTIONTYPE = 3  or ACTIONTYPE = 20) and IDEMP = " + this.EmpID + " and IDREADER is not null " +
                                                   " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
                
            DS = new DataSet();
            int s3 = Conn.SQLDA.Fill(DS, "t");//Кол-во чит, получивших изданий
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 4 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s4 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s4 = Conn.SQLDA.Fill(DS, "t");//принято изданий от читателей на бронеполку
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 5 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s5 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s5 = Conn.SQLDA.Fill(DS, "t");//принято изданий от читателя для сдачи в книгохранение
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 6 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s6 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s6 = Conn.SQLDA.Fill(DS, "t");//Выдано изданий читателю из подсобного фонда
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 7 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s7 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s7 = Conn.SQLDA.Fill(DS, "t");//Принятно изданий от читателей в подсобный фонд
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 8 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s8 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s8 = Conn.SQLDA.Fill(DS, "t");//выдано с выставки
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 9 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s9 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s9 = Conn.SQLDA.Fill(DS, "t");//Принято изданий с выставки
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..RecievedBooks " +
                                                   " where Cast(Cast(DATESTART As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and IDEMP = '" + this.EmpID + "' and IDMAIN = -1";
            DS = new DataSet();
            int s10 = Conn.SQLDA.Fill(DS, "t");//получили изданий из книгохранения не  в базе
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 10 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s11 = Conn.SQLDA.Fill(DS, "t");//Ни разу не выданных изданий сдано в книгохранение
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 14 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s13 = Conn.SQLDA.Fill(DS, "t");//Выдано на дом с ДП
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 16 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s14 = Conn.SQLDA.Fill(DS, "t");//Принято из дома с ДП
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 15 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s15 = Conn.SQLDA.Fill(DS, "t");//Выдано на дом из книгохранения
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 17 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s16 = Conn.SQLDA.Fill(DS, "t");//Принято из дому
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select IDREADER from " + BASENAME + "..[Statistics] " +
                           " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                           " and (ACTIONTYPE = 14 or ACTIONTYPE = 15 or ACTIONTYPE = 20) and IDEMP = " + this.EmpID + " and IDREADER is not null " +
                           " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
            DS = new DataSet();
            int s17 = Conn.SQLDA.Fill(DS, "t");//Кол-во чит, получивших изданий на дом
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT " +
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and ACTIONTYPE = 14 and IDEMP = " + this.EmpID + " and IDREADER is not null " +
                  " and C.IDINLIST = " + this.DepID;
            DS = new DataSet();
            int s18 = Conn.SQLDA.Fill(DS, "t");//Выдано изданий из открытого доступа зала
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT " +
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and ACTIONTYPE = 17 and IDEMP = " + this.EmpID + " and IDREADER is not null " +
                  " and C.IDINLIST = " + this.DepID;
            DS = new DataSet();
            int s19 = Conn.SQLDA.Fill(DS, "t");//Принято изданий из открытого доступа зала
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT " +
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA " +
                  " left join Readers..ReaderRight R on R.IDReader = A.IDREADER " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and (ACTIONTYPE = 14 or ACTIONTYPE=15) and IDEMP = " + this.EmpID + " and A.IDREADER is not null and R.IDReaderRight = 3" +
                  " ";
            DS = new DataSet();
            int s20 = Conn.SQLDA.Fill(DS, "t");//Выдано книг сотрудникам на дом
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..[ISSUED_OF_ACTIONS] A " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and IDACTION = 3 and IDEMP = " + this.EmpID;

            
            DS = new DataSet();
            int s21 = Conn.SQLDA.Fill(DS, "t");//продлен срок пользования изданием
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 21 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s22 = Conn.SQLDA.Fill(DS, "t");//Продлён срок бронеполки
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 20 and IDEMP = '" + this.EmpID + "' ";
            DS = new DataSet();
            int s23 = Conn.SQLDA.Fill(DS, "t");//Продлён срок бронеполки
            //----------------------------------------------------------------------------------
            string s12 = dbw.GetAttendance(f3.StartDate, f3.EndDate);
            //Количество читателей посетивших зал, но не воспользовавшихся услугами книговыдачи
            //---------------------------------------------------------------------------------
            //11 - сменили зал бронеполки
            //12 - сменили отдел, который принял книгу из книгохранения
            //13 - выдано с чужой бронеполки
            Statistics.Columns.Clear();
            Statistics.DataSource = null;
            Statistics.AutoGenerateColumns = false;
            Statistics.Columns.Add("NN", "№№");
            Statistics.Columns[0].Width = 50;
            Statistics.Columns.Add("D", "Операция");
            Statistics.Columns[1].Width = 550;
            Statistics.Columns.Add("C", "Кол-во");
            Statistics.Columns[2].Width = 60;

            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.Rows.Add();
            Statistics.Rows[0].Cells[1].Value = "Всего получено изданий из книгохранения";
            Statistics.Rows[0].Cells[2].Value = s1;
            Statistics.Rows.Add();
            Statistics.Rows[1].Cells[1].Value = "Получено изданий из к/х не в базе";
            Statistics.Rows[1].Cells[2].Value = s10;
            Statistics.Rows.Add();
            Statistics.Rows[2].Cells[1].Value = "Выдано изданий читателю из книгохранения в зал";
            Statistics.Rows[2].Cells[2].Value = s2;
            Statistics.Rows.Add();
            Statistics.Rows[3].Cells[1].Value = "Выдано изданий читателю с бронеполки";
            Statistics.Rows[3].Cells[2].Value = s23;
            if (this.DepID == "29")//если зал абонемента
            {
                Statistics.Rows.Add();
                Statistics.Rows[4].Cells[1].Value = "Выдано изданий читателю из открытого доступа зала на дом";
                Statistics.Rows[4].Cells[2].Value = s18;
            }
            else
            {
                Statistics.Rows.Add();
                Statistics.Rows[4].Cells[1].Value = "Выдано изданий  из открытого доступа зала на дом";
                Statistics.Rows[4].Cells[2].Value = s6;
            }
            Statistics.Rows.Add();
            Statistics.Rows[5].Cells[1].Value = "Выдано изданий читателю с выставки";
            Statistics.Rows[5].Cells[2].Value = s8;
            Statistics.Rows.Add();
            Statistics.Rows[6].Cells[1].Value = "Изданий выдано на дом из книгохранения";
            Statistics.Rows[6].Cells[2].Value = s15;
            Statistics.Rows.Add();
            Statistics.Rows[7].Cells[1].Value = "Изданий выдано на дом сотрудникам";
            Statistics.Rows[7].Cells[2].Value = s20;
            Statistics.Rows.Add();
            Statistics.Rows[8].Cells[1].Value = "Принято изданий от читателя для сдачи в книгохранение всего";
            Statistics.Rows[8].Cells[2].Value = s5+s16;
            Statistics.Rows.Add();
            Statistics.Rows[9].Cells[1].Value = "Принято изданий от читателей на бронеполку";
            Statistics.Rows[9].Cells[2].Value = s4;
            
            if (this.DepID == "29")//если зал абонемента
            {
                Statistics.Rows.Add();
                Statistics.Rows[10].Cells[1].Value = "Принято изданий от читателей в открытый доступ";
                Statistics.Rows[10].Cells[2].Value = s19;
            }
            else
            {
                Statistics.Rows.Add();
                Statistics.Rows[10].Cells[1].Value = "Принято изданий от читателей в подсобный фонд";
                Statistics.Rows[10].Cells[2].Value = s7;
            }
            Statistics.Rows.Add();
            Statistics.Rows[11].Cells[1].Value = "Принято изданий с выставки";
            Statistics.Rows[11].Cells[2].Value = s9;
            Statistics.Rows.Add();
            Statistics.Rows[12].Cells[1].Value = "Изданий принято из выданных на дом";
            Statistics.Rows[12].Cells[2].Value = s16;
            Statistics.Rows.Add();
            Statistics.Rows[13].Cells[1].Value = "Продление срока пользования изданием";//такая ведётся но с середины апреля 17
            Statistics.Rows[13].Cells[2].Value = s21;
            Statistics.Rows.Add();
            Statistics.Rows[14].Cells[1].Value = "Продление срока бронирования изданием";//такая ведётся но с середины апреля 21
            Statistics.Rows[14].Cells[2].Value = s22;
            Statistics.Rows.Add();
            Statistics.Rows[15].Cells[1].Value = "Ни разу не выданных изданий сдано в книгохранение";
            Statistics.Rows[15].Cells[2].Value = s5+s11;
            Statistics.Rows.Add();
            Statistics.Rows[16].Cells[1].Value = "Количество читателей посетивших зал, но не воспользовавшихся услугами книговыдачи";
            Statistics.Rows[16].Cells[2].Value = s12;
            Statistics.Rows.Add();
            Statistics.Rows[17].Cells[1].Value = "Количество читателей, получивших издания (в залы + на дом)";
            Statistics.Rows[17].Cells[2].Value = s3;
            Statistics.Rows.Add();
            Statistics.Rows[18].Cells[1].Value = "Количество читателей, получивших издания на дом";
            Statistics.Rows[18].Cells[2].Value = s17;
            /*Statistics.Rows.Add();
            Statistics.Rows[13].Cells[1].Value = "Изданий выдано на дом с ДП";
            Statistics.Rows[13].Cells[2].Value = s13;
            Statistics.Rows.Add();
            Statistics.Rows[14].Cells[1].Value = "Изданий принято из выданных на дом с ДП";
            Statistics.Rows[14].Cells[2].Value = s14;*/
            autoinc(Statistics);
            Conn.SQLDA.SelectCommand.Connection.Close();
        }
        private DateTime StartDateReadersInHall;
        private DateTime EndDateReadersInHall;
        private void справкаОтделаЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            StartDateReadersInHall = f3.StartDate.Date;
            EndDateReadersInHall = f3.EndDate.Date;
            if (Conn.SQLDA.SelectCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.SelectCommand.Connection.Open();
            }
            //Conn.SQLDA.SelectCommand.Connection.Open();
            label19.Text = "Отчет отдела за период с " + f3.StartDate.ToString("dd.MM.yy") + " по " + f3.EndDate.ToString("dd.MM.yy");
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..RecievedBooks " +
                                                   " where Cast(Cast(DATESTART As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and STARTDEP = '" + this.DepName + "'";
            DataSet DS = new DataSet();
            int s1 = Conn.SQLDA.Fill(DS, "t");//получили изданий из книгохранения
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 2 and DEPID = " + this.DepID ;
            DS = new DataSet();
            int s2 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s2 = Conn.SQLDA.Fill(DS, "t");//выдано изданий читателю в залы
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select IDREADER from " + BASENAME + "..[Statistics] " +
                                                   " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and (ACTIONTYPE = 3  or ACTIONTYPE = 20) and DEPID = " + this.DepID + " and IDREADER is not null " +
                                                   " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
            DS = new DataSet();
            int s3 = Conn.SQLDA.Fill(DS, "t");//Кол-во чит, получивших изданий (на дом + в залы)
            s3 = DS.Tables["t"].Rows.Count;
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 4 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s4 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s4 = Conn.SQLDA.Fill(DS, "t");//принято изданий от читателей на бронеполку
            //---------------------------------------------------------------------------------
            //Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
            //                                       " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
            //                                       " and ACTIONTYPE = 5 and DEPID = " + this.DepID;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 5 and DEPID = " + this.DepID;
            DS = new DataSet();
            //int s5 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            int s5 = Conn.SQLDA.Fill(DS, "t");//принято изданий от читателя для сдачи в книгохранение
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 6 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s6 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s6 = Conn.SQLDA.Fill(DS, "t");//Выдано изданий читателю из подсобного фонда (на дом только в абонементе такое)
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 7 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s7 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s7 = Conn.SQLDA.Fill(DS, "t");//Принятно изданий от читателей в подсобный фонд
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 8 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s8 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s8 = Conn.SQLDA.Fill(DS, "t");//выдано с выставки
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select (case when sum(COUNTISS) is null then 0 else sum(COUNTISS) end) from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 9 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s9 = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            //int s9 = Conn.SQLDA.Fill(DS, "t");//принято с выставки
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..RecievedBooks " +
                                                   " where Cast(Cast(DATESTART As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and STARTDEP = '" + this.DepName + "' and IDMAIN = -1";
            DS = new DataSet();
            int s10 = Conn.SQLDA.Fill(DS, "t");//получили изданий из книгохранения не в базе
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 10 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s11 = Conn.SQLDA.Fill(DS, "t");//Ни разу не выданных изданий сдано в книгохранение
            //---------------------------------------------------------------------------------
            string s12 = dbw.GetAttendance(f3.StartDate, f3.EndDate);
            //Количество читателей посетивших зал, но не воспользовавшихся услугами книговыдачи
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 14 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s13 = Conn.SQLDA.Fill(DS, "t");//Выдано на дом с ДП
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 16 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s14 = Conn.SQLDA.Fill(DS, "t");//Принято из дома с ДП
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 15 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s15 = Conn.SQLDA.Fill(DS, "t");//Выдано на дом из кх
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 17 and DEPID = " + this.DepID;
            DS = new DataSet();
            int s16 = Conn.SQLDA.Fill(DS, "t");//Принято из дому
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select IDREADER from " + BASENAME + "..[Statistics] " +
                                       " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                       " and (ACTIONTYPE = 14 or ACTIONTYPE = 15 or ACTIONTYPE = 20) and DEPID = " + this.DepID + " and IDREADER is not null " +
                                       " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
            DS = new DataSet();
            int s17 = Conn.SQLDA.Fill(DS, "t");//Кол-во чит, получивших изданий на дом
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT "+
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA "+
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and ACTIONTYPE = 14 and DEPID = " + this.DepID + " and IDREADER is not null " +
                  " and C.IDINLIST = "+this.DepID;
            DS = new DataSet();
            int s18 = Conn.SQLDA.Fill(DS, "t");//Выдано изданий из открытого доступа зала на дом
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT " +
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and ACTIONTYPE = 17 and DEPID = " + this.DepID + " and IDREADER is not null " +
                  " and C.IDINLIST = " + this.DepID;
            DS = new DataSet();
            int s19 = Conn.SQLDA.Fill(DS, "t");//Принято изданий из открытого доступа зала
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select A.*,C.SORT from " + BASENAME + "..[Statistics] A " +
                  " left join BJVVV..DATAEXT B on B.MNFIELD = 899 and B.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = B.SORT " +
                  " left join BJVVV..DATAEXT C on C.MNFIELD = 899 and C.MSFIELD = '$a' and B.IDDATA = C.IDDATA " +
                  " left join Readers..ReaderRight R on R.IDReader = A.IDREADER " +
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and (ACTIONTYPE = 14 or ACTIONTYPE=15) and DEPID = " + this.DepID + " and A.IDREADER is not null and R.IDReaderRight = 3" +
                  " ";
            DS = new DataSet();
            int s20 = Conn.SQLDA.Fill(DS, "t");//Выдано книг сотрудникам на дом
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..[ISSUED_OF_ACTIONS] A " +
                  " left join BJVVV..USERS U on U.ID = A.IDEMP " +
                  " left join BJVVV..USERSTATUS B on B.IDUSER = U.ID "+
                  " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                  " and IDACTION = 3 and B.IDDEPT = " + this.DepID;
            DS = new DataSet();
            int s21 = Conn.SQLDA.Fill(DS, "t");//продлен срок пользования изданием
            //---------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 21 and DEPID = '" + this.DepID + "' ";
            DS = new DataSet();
            int s22 = Conn.SQLDA.Fill(DS, "t");//Продлён срок бронеполки
            //----------------------------------------------------------------------------------
            Conn.SQLDA.SelectCommand.CommandText = "select ID from " + BASENAME + "..[Statistics] " +
                                                   " where Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                                                   " and ACTIONTYPE = 20 and DEPID = '" + this.DepID + "' ";
            DS = new DataSet();
            int s23 = Conn.SQLDA.Fill(DS, "t");//Продлён срок бронеполки
            //----------------------------------------------------------------------------------


            Statistics.Columns.Clear();
            Statistics.DataSource = null;
            Statistics.AutoGenerateColumns = false;
            Statistics.Columns.Add("NN", "№№");
            Statistics.Columns[0].Width = 50;
            Statistics.Columns.Add("D", "Операция");
            Statistics.Columns[1].Width = 550;
            Statistics.Columns.Add("C", "Кол-во");
            Statistics.Columns[2].Width = 60;
            
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Statistics.Rows.Add();
            Statistics.Rows[0].Cells[1].Value = "Всего получено изданий из книгохранения";
            Statistics.Rows[0].Cells[2].Value = s1;
            Statistics.Rows.Add();
            Statistics.Rows[1].Cells[1].Value = "Получено изданий из к/х не в базе";
            Statistics.Rows[1].Cells[2].Value = s10;
            Statistics.Rows.Add();
            Statistics.Rows[2].Cells[1].Value = "Выдано изданий читателю из книгохранения в зал";
            Statistics.Rows[2].Cells[2].Value = s2;
            Statistics.Rows.Add();
            Statistics.Rows[3].Cells[1].Value = "Выдано изданий читателю c бронеполки";//такая статистика не ведётся. нужно добвлять.
            Statistics.Rows[3].Cells[2].Value = s23;
            if (this.DepID == "29")//если зал абонемента
            {
                Statistics.Rows.Add();
                Statistics.Rows[4].Cells[1].Value = "Выдано изданий читателю из открытого доступа зала на дом";
                Statistics.Rows[4].Cells[2].Value = s18;
            }
            else
            {
                Statistics.Rows.Add();
                Statistics.Rows[4].Cells[1].Value = "Выдано изданий  из открытого доступа зала";
                Statistics.Rows[4].Cells[2].Value = s6;
            }
            Statistics.Rows.Add();
            Statistics.Rows[5].Cells[1].Value = "Выдано изданий читателю с выставки";
            Statistics.Rows[5].Cells[2].Value = s8;
            Statistics.Rows.Add();
            Statistics.Rows[6].Cells[1].Value = "Изданий выдано на дом из книгохранения";
            Statistics.Rows[6].Cells[2].Value = s15;
            Statistics.Rows.Add();
            Statistics.Rows[7].Cells[1].Value = "Количество книг, выданных сотрудникам на дом";
            Statistics.Rows[7].Cells[2].Value = s20;
            Statistics.Rows.Add();
            Statistics.Rows[8].Cells[1].Value = "Принято изданий от читателя для сдачи в книгохранение всего";
            Statistics.Rows[8].Cells[2].Value = s5+s16;
            Statistics.Rows.Add();
            Statistics.Rows[9].Cells[1].Value = "Принято изданий от читателей на бронеполку";
            Statistics.Rows[9].Cells[2].Value = s4;
            if (this.DepID == "29")
            {
                Statistics.Rows.Add();
                Statistics.Rows[10].Cells[1].Value = "Принято изданий от читателей в открытый доступ";
                Statistics.Rows[10].Cells[2].Value = s19;
            }
            else
            {
                Statistics.Rows.Add();
                Statistics.Rows[10].Cells[1].Value = "Принято изданий от читателей в подсобный фонд";
                Statistics.Rows[10].Cells[2].Value = s7;
            }
            Statistics.Rows.Add();
            Statistics.Rows[11].Cells[1].Value = "Принято изданий с выставки";
            Statistics.Rows[11].Cells[2].Value = s9;
            Statistics.Rows.Add();
            Statistics.Rows[12].Cells[1].Value = "Изданий принято из выданных на дом";
            Statistics.Rows[12].Cells[2].Value = s16;
            Statistics.Rows.Add();
            Statistics.Rows[13].Cells[1].Value = "Продление срока пользования изданием";//такая ведётся но с середины апреля 17
            Statistics.Rows[13].Cells[2].Value = s21;
            Statistics.Rows.Add();
            Statistics.Rows[14].Cells[1].Value = "Продление срока бронирования изданием";//такая ведётся но с середины апреля 21
            Statistics.Rows[14].Cells[2].Value = s22;
            Statistics.Rows.Add();
            Statistics.Rows[15].Cells[1].Value = "Ни разу не выданных изданий сдано в книгохранение";
            Statistics.Rows[15].Cells[2].Value = s5+s11;
            Statistics.Rows.Add();
            Statistics.Rows[16].Cells[1].Value = "Количество читателей посетивших зал, но не воспользовавшихся услугами книговыдачи";
            Statistics.Rows[16].Cells[2].Value = s12;
            Statistics.Rows.Add();
            Statistics.Rows[17].Cells[1].Value = "Количество читателей, получивших издания (в залы + на дом)";
            Statistics.Rows[17].Cells[2].Value = s3;
            /*Statistics.Rows.Add();
            Statistics.Rows[14].Cells[1].Value = "Изданий выдано на дом с ДП";
            Statistics.Rows[14].Cells[2].Value = s13;
            Statistics.Rows.Add();
            Statistics.Rows[15].Cells[1].Value = "Изданий принято из выданных на дом с ДП";
            Statistics.Rows[15].Cells[2].Value = s14;*/
            Statistics.Rows.Add();
            Statistics.Rows[18].Cells[1].Value = "Количество читателей, получивших издания на дом";
            Statistics.Rows[18].Cells[2].Value = s17;

            autoinc(Statistics);
            Conn.SQLDA.SelectCommand.Connection.Close();
        }
        DateTime f3Start;
        DateTime f3End;
        private void справкаОтделаКнигохранениеОФЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Form3 f3 = new Form3();
            f3.ShowDialog();
            f3Start = f3.StartDate;
            f3End = f3.EndDate;
            label19.Text = "Отчет отдела Книгохранение за период с " + f3.StartDate.ToString("dd.MM.yy") + " по " + f3.EndDate.ToString("dd.MM.yy");
            Conn.SQLDA.SelectCommand.CommandText =
                "with FA as (select (case when A.IDMAIN = -1 then 'Не в базе' else A.STARTMHR end) mhr ,(A.BAR) a" +
                ",  (F0.ID) F0 " +", (F2.ID) F2 " + ", (F3.ID) F3 " + ", (F4.ID) F4 " +  ", (F5.ID) F5 " +
                ", (F6.ID) F6 " +
                ", (F7.ID) F7, (F8.ID) F8 " +
                " from " + BASENAME + "..RecievedBooks A " +
                " left join BJVVV..DATAEXT B on A.BAR collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$w' and A.IDMAIN = B.IDMAIN " +
                " left join BJVVV..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                " left join BJVVV..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                //" left join " + BASENAME + "..RecievedBooks D on  D.RETINBK = 1 " +
                                //" and (D.DATESTART between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "'"+
                                //"  and ( D.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "left join " + BASENAME + "..RecievedBooks F0 on  " +
                "    A.ID = F0.ID  and F0.STARTMHR = '…Хран… Сектор книгохранения - 0 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F2 on  " +
                "    A.ID = F2.ID and  F2.STARTMHR = '…Хран… Сектор книгохранения - 2 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F3 on  " +
                "    A.ID = F3.ID and F3.STARTMHR = '…Хран… Сектор книгохранения - 3 этаж' " +
                " left join " + BASENAME + "..RecievedBooks F4 on  " +
                "    A.ID = F4.ID  and F4.STARTMHR like '%редкой%' " +
                "left join " + BASENAME + "..RecievedBooks F5 on  " +
                "    A.ID = F5.ID and F5.STARTMHR = '…Хран… Сектор книгохранения - 5 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F6 on  " +
                "    A.ID = F6.ID  and F6.STARTMHR = '…Хран… Сектор книгохранения - 6 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F7 on  " +
                "    A.ID = F7.ID and F7.STARTMHR = '…Хран… Сектор книгохранения - 7 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F8 on  " +
                "    A.ID = F8.ID and F8.STARTMHR = '…Хран… Сектор книгохранения - Абонемент' " +
                " where  " +
                "         (cast(cast(A.DATESTART as nvarchar(11)) as datetime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')"+//  or  " +
                
                //"         A.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "and A.DEPNAME  not like '%едкой книги%') " +
                //" FB as (select * from " + BASENAME + "..RecievedBooks D where D.RETINBK = 1  and ( D.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')) "+
                " select A.mhr,count(A.a),0,count(A.F0),count(A.F2),count(A.F3),count(A.F4), "+
                " count(A.F5),count(A.F6),count(A.F7),count(A.F8) from FA A group by A.mhr";
            DataSet DS = new DataSet();
            int s6 = Conn.SQLDA.Fill(DS, "t");
            Conn.SQLDA.SelectCommand.CommandText =
                "with FA as (select (case when A.IDMAIN = -1 then 'Не в базе' else A.STARTMHR end) mhr ,(A.BAR) a" +
                ",  (F0.ID) F0 " + ", (F2.ID) F2 " + ", (F3.ID) F3 " + ", (F4.ID) F4 " + ", (F5.ID) F5 " +
                ", (F6.ID) F6 " +
                ", (F7.ID) F7, (F8.ID) F8 " +
                " from " + BASENAME + "..RecievedBooks A " +
                " left join BJVVV..DATAEXT B on A.BAR collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$w' and A.IDMAIN = B.IDMAIN " +
                " left join BJVVV..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                " left join BJVVV..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                "left join " + BASENAME + "..RecievedBooks F0 on  " +
                "    A.ID = F0.ID  and F0.RECDEPNAME = '…Хран… Сектор книгохранения - 0 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F2 on  " +
                "    A.ID = F2.ID and  F2.RECDEPNAME = '…Хран… Сектор книгохранения - 2 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F3 on  " +
                "    A.ID = F3.ID and F3.RECDEPNAME = '…Хран… Сектор книгохранения - 3 этаж' " +
                " left join " + BASENAME + "..RecievedBooks F4 on  " +
                "    A.ID = F4.ID  and F4.RECDEPNAME like '%едкой%' " +
                "left join " + BASENAME + "..RecievedBooks F5 on  " +
                "    A.ID = F5.ID and F5.RECDEPNAME = '…Хран… Сектор книгохранения - 5 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F6 on  " +
                "    A.ID = F6.ID  and F6.RECDEPNAME = '…Хран… Сектор книгохранения - 6 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F7 on  " +
                "    A.ID = F7.ID and F7.RECDEPNAME = '…Хран… Сектор книгохранения - 7 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F8 on  " +
                "    A.ID = F8.ID and F8.RECDEPNAME = '…Хран… Сектор книгохранения - Абонемент' " +
                " where  " +
                //"         (A.DATESTART between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')" +//  or  " +

                "         A.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "' " +
                "and A.DEPNAME not like '%едкой книги%') " +
                //" FB as (select * from " + BASENAME + "..RecievedBooks D where D.RETINBK = 1  and ( D.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')) "+
                " select A.mhr,count(A.a),count(A.a),count(A.F0),count(A.F2),count(A.F3),count(A.F4), " +
                " count(A.F5),count(A.F6),count(A.F7), count(A.F8) from FA A group by A.mhr";
            DataSet DS1 = new DataSet();
            s6 = Conn.SQLDA.Fill(DS1, "t");

            

            Conn.SQLDA.SelectCommand.CommandText =
                "with FA as (select (case when A.IDMAIN = -1 then 'Не в базе' else A.STARTMHR end) mhr ,(A.BAR) a" +
                ",  (F0.ID) F0 , (F2.ID) F2 , (F3.ID) F3, (F4.ID) F4 , (F5.ID) F5 " +
                ", (F6.ID) F6 " +
                ", (F7.ID) F7 " +
                " from " + BASENAME + "..RecievedBooks A " +
                " left join REDKOSTJ..DATAEXT B on A.BAR collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$w' and A.IDMAIN = B.IDMAIN " +
                " left join REDKOSTJ..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                " left join REDKOSTJ..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                //" left join " + BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RETINBK = 1 " +
                        //        " and (D.DATESTART between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") +
                         //       "'  or D.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "left join " + BASENAME + "..RecievedBooks F0 on  " +
                "    A.ID = F0.ID  and F0.STARTMHR = '…Хран… Сектор книгохранения - 0 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F2 on  " +
                "    A.ID = F2.ID and  F2.STARTMHR = '…Хран… Сектор книгохранения - 2 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F3 on  " +
                "    A.ID = F3.ID and F3.STARTMHR = '…Хран… Сектор книгохранения - 3 этаж' " +
                " left join " + BASENAME + "..RecievedBooks F4 on  " +
                "    A.ID = F4.ID  and F4.STARTMHR like '%едкой%' " +
                "left join " + BASENAME + "..RecievedBooks F5 on  " +
                "    A.ID = F5.ID and F5.STARTMHR = '…Хран… Сектор книгохранения - 5 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F6 on  " +
                "    A.ID = F6.ID  and F6.STARTMHR = '…Хран… Сектор книгохранения - 6 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F7 on  " +
                "    A.ID = F7.ID and F7.STARTMHR = '…Хран… Сектор книгохранения - 7 этаж' " +
                " where  " +
                "         ( cast(cast(A.DATESTART as nvarchar(11)) as datetime) between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')    " +
                //"         A.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "and A.DEPNAME like '%едкой%') " +
                " select A.mhr,count(A.a),0,count(A.F0),count(A.F2),count(A.F3),count(A.F4), " +
                " count(A.F5),count(A.F6),count(A.F7) from FA A group by A.mhr";

            DataSet DSRED = new DataSet();
            int sred = Conn.SQLDA.Fill(DSRED, "t");

            Conn.SQLDA.SelectCommand.CommandText =
                "with FA as (select (case when A.IDMAIN = -1 then 'Не в базе' else A.STARTMHR end) mhr ,(A.BAR) a" +
                ",  (F0.ID) F0 , (F2.ID) F2 , (F3.ID) F3, (F4.ID) F4 , (F5.ID) F5 " +
                ", (F6.ID) F6 " +
                ", (F7.ID) F7 " +
                " from " + BASENAME + "..RecievedBooks A " +
                " left join REDKOSTJ..DATAEXT B on A.BAR collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$w' and A.IDMAIN = B.IDMAIN " +
                " left join REDKOSTJ..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                " left join REDKOSTJ..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                            //" left join " + BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RETINBK = 1 " +
                            //        " and (D.DATESTART between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") +
                            //       "'  or D.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "left join " + BASENAME + "..RecievedBooks F0 on  " +
                "    A.ID = F0.ID  and F0.RECDEPNAME = '…Хран… Сектор книгохранения - 0 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F2 on  " +
                "    A.ID = F2.ID and  F2.RECDEPNAME = '…Хран… Сектор книгохранения - 2 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F3 on  " +
                "    A.ID = F3.ID and F3.RECDEPNAME = '…Хран… Сектор книгохранения - 3 этаж' " +
                " left join " + BASENAME + "..RecievedBooks F4 on  " +
                "    A.ID = F4.ID  and F4.RECDEPNAME like '%едкой%' " +
                "left join " + BASENAME + "..RecievedBooks F5 on  " +
                "    A.ID = F5.ID and F5.RECDEPNAME = '…Хран… Сектор книгохранения - 5 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F6 on  " +
                "    A.ID = F6.ID  and F6.RECDEPNAME = '…Хран… Сектор книгохранения - 6 этаж' " +
                "left join " + BASENAME + "..RecievedBooks F7 on  " +
                "    A.ID = F7.ID and F7.RECDEPNAME = '…Хран… Сектор книгохранения - 7 этаж' " +
                " where  " +
                "         (A.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "')    " +
                            //"         A.DATEFINISH between '" + f3.StartDate.ToString("yyyyMMdd") + "' and '" + f3.EndDate.ToString("yyyyMMdd") + "') " +
                "and A.DEPNAME  like '%едкой%') " +
                " select A.mhr,count(A.a),count(A.a),count(A.F0),count(A.F2),count(A.F3),count(A.F4), " +
                " count(A.F5),count(A.F6),count(A.F7) from FA A group by A.mhr";

            DataSet DSRED1 = new DataSet();
            sred = Conn.SQLDA.Fill(DSRED1, "t");

            Statistics.Columns.Clear();
            Statistics.DataSource = null;
            Statistics.AutoGenerateColumns = false;
            Statistics.Columns.Add("sektor", "Сектор");
            Statistics.Columns[0].Width = 200;
            Statistics.Columns.Add("D", "Кол-во выданных изданий");
            Statistics.Columns[1].Width = 100;
            Statistics.Columns.Add("C", "Кол-во принятых изданий");
            Statistics.Columns[2].Width = 100;

            Statistics.Columns.Add("F0", "Цоколь");
            Statistics.Columns[3].Width = 60;
            Statistics.Columns.Add("F2", "2 этаж");
            Statistics.Columns[4].Width = 60;
            Statistics.Columns.Add("F2", "3 этаж");
            Statistics.Columns[5].Width = 60;
            Statistics.Columns.Add("F2", "к/х ред. кн.");
            Statistics.Columns[6].Width = 60;
            //Statistics.Columns.Add("F2", "5 этаж");
            //Statistics.Columns[7].Width = 60;

            Statistics.Columns.Add("F2", "6 этаж");
            Statistics.Columns[7].Width = 60;
            Statistics.Columns.Add("F2", "7 этаж");
            Statistics.Columns[8].Width = 60;
            Statistics.Columns.Add("F2", "Абоне мент");
            Statistics.Columns[9].Width = 60;
            Statistics.Columns.Add("ORD", "Кол-во требо ваний");
            Statistics.Columns[10].Width = 60;
            Statistics.Columns.Add("REF", "Кол-во отказов");
            Statistics.Columns[11].Width = 60;

            string f11 = "0", f12 = "0", f21 = "0", f22 = "0", f31 = "0", f32 = "0",
                f41 = "0", f42 = "0", f51 = "0", f52 = "0", f61 = "0", f62 = "0",
                f71 = "0", f72 = "0", f81 = "0", f82 = "0", f91 = "0", f92 = "0",
                f101 = "0", f102 = "0", f111 = "0", f112 = "0",
                f121 = "0", f122 = "0";//абонемент
            string r00 = "0", r02 = "0", r03 = "0", r04 = "0", r05 = "0", r06 = "0", r07 = "0",//цоколь
                   r20 = "0", r22 = "0", r23 = "0", r24 = "0", r25 = "0", r26 = "0", r27 = "0",//2 этаж
                   r30 = "0", r32 = "0", r33 = "0", r34 = "0", r35 = "0", r36 = "0", r37 = "0",//3 этаж
                   r40 = "0", r42 = "0", r43 = "0", r44 = "0", r45 = "0", r46 = "0", r47 = "0",//4 этаж
                   r50 = "0", r52 = "0", r53 = "0", r54 = "0", r55 = "0", r56 = "0", r57 = "0",//5 этаж
                   r60 = "0", r62 = "0", r63 = "0", r64 = "0", r65 = "0", r66 = "0", r67 = "0",//6 этаж
                   r70 = "0", r72 = "0", r73 = "0", r74 = "0", r75 = "0", r76 = "0", r77 = "0",//7 этаж
                   r80 = "0", r82 = "0", r83 = "0", r84 = "0", r85 = "0", r86 = "0", r87 = "0",//не в базе
                   r90 = "0", r92 = "0", r93 = "0", r94 = "0", r95 = "0", r96 = "0", r97 = "0";//абонемент
            string rr00 = "0", rr02 = "0", rr03 = "0", rr04 = "0", rr05 = "0", rr06 = "0", rr07 = "0",
                   rr10 = "0", rr12 = "0", rr13 = "0", rr14 = "0", rr15 = "0", rr16 = "0", rr17 = "0";
            string ord0 = "0", ord1 = "0", ord2 = "0", ord3 = "0", ord4 = "0", ord5 = "0", ord6 = "0", ord7 = "-", ord8 = "0", ord9 = "-", ord10 = "-",
                ord11 = "0" /*абонемент*/;
            string ref0 = "0", ref1 = "0", ref2 = "0", ref3 = "0", ref4 = "0", ref5 = "0", ref6 = "0", ref7 = "-", ref8 = "0", ref9 = "-", ref10 = "-", 
                ref11 = "0"/*абонемент*/;
            ord0 = dbw.GetOrders0(f3.StartDate, f3.EndDate);//цоколь
            ord1 = dbw.GetOrders1(f3.StartDate, f3.EndDate);//2
            ord2 = dbw.GetOrders2(f3.StartDate, f3.EndDate);//3
            ord3 = dbw.GetOrders3(f3.StartDate, f3.EndDate);//4
            ord4 = dbw.GetOrders4(f3.StartDate, f3.EndDate);//5
            ord5 = dbw.GetOrders5(f3.StartDate, f3.EndDate);//6 этаж
            ord6 = dbw.GetOrders6(f3.StartDate, f3.EndDate);//7
            ord8 = dbw.GetOrders8(f3.StartDate, f3.EndDate);//всего
            ord11 = dbw.GetOrders11(f3.StartDate, f3.EndDate);//абонемент

            ref0 = dbw.GetResusual(f3.StartDate, f3.EndDate, "15");
            ref1 = dbw.GetResusual(f3.StartDate, f3.EndDate, "6");
            ref2 = dbw.GetResusual(f3.StartDate, f3.EndDate, "7");
            ref3 = dbw.GetResusual(f3.StartDate, f3.EndDate, "8");
            ref4 = dbw.GetResusual(f3.StartDate, f3.EndDate, "9");
            ref5 = dbw.GetResusual(f3.StartDate, f3.EndDate, "10");
            ref6 = dbw.GetResusual(f3.StartDate, f3.EndDate, "11");
            ref8 = dbw.GetResusual(f3.StartDate, f3.EndDate, "all");
            ref11 = dbw.GetResusual(f3.StartDate, f3.EndDate, "47");
            foreach (DataRow r in DS1.Tables[0].Rows)
            {
                switch (r[0].ToString())
                {
                    case "…Хран… Сектор книгохранения - Абонемент":
                        //f81 = r[1].ToString();
                        f122 = r[2].ToString();
                        r90 = r[3].ToString();
                        r92 = r[4].ToString();
                        r93 = r[5].ToString();
                        r94 = r[6].ToString();
                        r95 = r[7].ToString();
                        r96 = r[8].ToString();
                        r97 = r[10].ToString();
                        break;
                    case "Не в базе":
                        //f81 = r[1].ToString();
                        f82 = r[2].ToString();
                        r80 = r[3].ToString();
                        r82 = r[4].ToString();
                        r83 = r[5].ToString();
                        r84 = r[6].ToString();
                        r85 = r[7].ToString();
                        r86 = r[8].ToString();
                        r87 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 2 этаж":
                        //f21 = r[1].ToString();
                        f22 = r[2].ToString();
                        r20 = r[3].ToString();
                        r22 = r[4].ToString();
                        r23 = r[5].ToString();
                        r24 = r[6].ToString();
                        r25 = r[7].ToString();
                        r26 = r[8].ToString();
                        r27 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 3 этаж":
                        //f31 = r[1].ToString();
                        f32 = r[2].ToString();
                        r30 = r[3].ToString();
                        r32 = r[4].ToString();
                        r33 = r[5].ToString();
                        r34 = r[6].ToString();
                        r35 = r[7].ToString();
                        r36 = r[8].ToString();
                        r37 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 4 этаж":
                        //f41 = r[1].ToString();
                        f42 = r[2].ToString();
                        r40 = r[3].ToString();
                        r42 = r[4].ToString();
                        r43 = r[5].ToString();
                        r44 = r[6].ToString();
                        r45 = r[7].ToString();
                        r46 = r[8].ToString();
                        r47 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 5 этаж":
                        //f51 = r[1].ToString();
                        f52 = r[2].ToString();
                        r50 = r[3].ToString();
                        r52 = r[4].ToString();
                        r53 = r[5].ToString();
                        r54 = r[6].ToString();
                        r55 = r[7].ToString();
                        r56 = r[8].ToString();
                        r57 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 6 этаж":
                        //f61 = r[1].ToString();
                        f62 = r[2].ToString();
                        r60 = r[3].ToString();
                        r62 = r[4].ToString();
                        r63 = r[5].ToString();
                        r64 = r[6].ToString();
                        r65 = r[7].ToString();
                        r66 = r[8].ToString();
                        r67 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 7 этаж":
                        //f71 = r[1].ToString();
                        f72 = r[2].ToString();
                        r70 = r[3].ToString();
                        r72 = r[4].ToString();
                        r73 = r[5].ToString();
                        r74 = r[6].ToString();
                        r75 = r[7].ToString();
                        r76 = r[8].ToString();
                        r77 = r[9].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 0 этаж":
                        //f11 = r[1].ToString();
                        f12 = r[2].ToString();
                        r00 = r[3].ToString();
                        r02 = r[4].ToString();
                        r03 = r[5].ToString();
                        r04 = r[6].ToString();
                        r05 = r[7].ToString();
                        r06 = r[8].ToString();
                        r07 = r[9].ToString();
                        break;
                }
            }
            foreach (DataRow r in DS.Tables[0].Rows)
            {
                switch (r[0].ToString())
                {
                    case "Не в базе":
                        f81 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 2 этаж":
                        f21 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 3 этаж":
                        f31 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 4 этаж":
                        f41 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 5 этаж":
                        f51 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 6 этаж":
                        f61 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 7 этаж":
                        f71 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - 0 этаж":
                        f11 = r[1].ToString();
                        break;
                    case "…Хран… Сектор книгохранения - Абонемент":
                        f121 = r[1].ToString();
                        break;
                }
            }
            // "Книгохранение - Фонд редкой книги":
            foreach (DataRow r in DSRED1.Tables[0].Rows)
            {
                if (r[0].ToString().Contains("едкой книги"))
                {
                    f101 = r[1].ToString();
                    f102 = r[2].ToString();
                    rr10 = r[3].ToString();
                    rr12 = r[4].ToString();
                    rr13 = r[5].ToString();
                    rr14 = r[6].ToString();
                    rr15 = r[7].ToString();
                    rr16 = r[8].ToString();
                    rr17 = r[9].ToString();
                }
                if (r[0].ToString() == "Не в базе")
                {
                    f111 = r[1].ToString();
                    f112 = r[2].ToString();
                    rr00 = r[3].ToString();
                    rr02 = r[4].ToString();
                    rr03 = r[5].ToString();
                    rr04 = r[6].ToString();
                    rr05 = r[7].ToString();
                    rr06 = r[8].ToString();
                    rr07 = r[9].ToString();
                }
            }
            foreach (DataRow r in DSRED.Tables[0].Rows)
            {
                if (r[0].ToString().Contains("едкой книги"))
                {
                    f101 = r[1].ToString();
                }
                if (r[0].ToString() == "Не в базе")
                {
                    f111 = r[1].ToString();
                }
            }
            f91 = (int.Parse(f11) + int.Parse(f21) + int.Parse(f31) + int.Parse(f41) + int.Parse(f51) + int.Parse(f61) + int.Parse(f71) + int.Parse(f81) + int.Parse(f121)).ToString();
            f92 = (int.Parse(f12)  + int.Parse(f22) + int.Parse(f32) + int.Parse(f42) + int.Parse(f52) + int.Parse(f62) + int.Parse(f72) + int.Parse(f82) + int.Parse(f122)).ToString();
            Statistics.Rows.Add();
            Statistics.Rows[0].Cells[0].Value = "Абонемент";
            Statistics.Rows[0].Cells[1].Value = f121;
            Statistics.Rows[0].Cells[2].Value = f122;
            Statistics.Rows[0].Cells[3].Value = r90;
            Statistics.Rows[0].Cells[4].Value = r92;
            Statistics.Rows[0].Cells[5].Value = r93;
            Statistics.Rows[0].Cells[6].Value = r94;
            Statistics.Rows[0].Cells[7].Value = r95;
            Statistics.Rows[0].Cells[8].Value = r96;
            Statistics.Rows[0].Cells[9].Value = r97;
            Statistics.Rows[0].Cells[10].Value = ord11;
            Statistics.Rows[0].Cells[11].Value = ref11;
            Statistics.Rows.Add();
            Statistics.Rows[1].Cells[0].Value = "Цоколь";
            Statistics.Rows[1].Cells[1].Value = f11;
            Statistics.Rows[1].Cells[2].Value = f12;
            Statistics.Rows[1].Cells[3].Value = r00;
            Statistics.Rows[1].Cells[4].Value = r02;
            Statistics.Rows[1].Cells[5].Value = r03;
            Statistics.Rows[1].Cells[6].Value = r04;
            Statistics.Rows[1].Cells[7].Value = r05;
            Statistics.Rows[1].Cells[8].Value = r06;
            Statistics.Rows[1].Cells[9].Value = r07;
            Statistics.Rows[1].Cells[10].Value = ord0;
            Statistics.Rows[1].Cells[11].Value = ref0;
            Statistics.Rows.Add();
            Statistics.Rows[2].Cells[0].Value = "2 этаж";
            Statistics.Rows[2].Cells[1].Value = f21;
            Statistics.Rows[2].Cells[2].Value = f22;
            Statistics.Rows[2].Cells[3].Value = r20;
            Statistics.Rows[2].Cells[4].Value = r22;
            Statistics.Rows[2].Cells[5].Value = r23;
            Statistics.Rows[2].Cells[6].Value = r24;
            Statistics.Rows[2].Cells[7].Value = r25;
            Statistics.Rows[2].Cells[8].Value = r26;
            Statistics.Rows[2].Cells[9].Value = r27;
            Statistics.Rows[2].Cells[10].Value = ord1;
            Statistics.Rows[2].Cells[11].Value = ref1;
            Statistics.Rows.Add();
            Statistics.Rows[3].Cells[0].Value = "3 этаж";
            Statistics.Rows[3].Cells[1].Value = f31;
            Statistics.Rows[3].Cells[2].Value = f32;
            Statistics.Rows[3].Cells[3].Value = r30;
            Statistics.Rows[3].Cells[4].Value = r32;
            Statistics.Rows[3].Cells[5].Value = r33;
            Statistics.Rows[3].Cells[6].Value = r34;
            Statistics.Rows[3].Cells[7].Value = r35;
            Statistics.Rows[3].Cells[8].Value = r36;
            Statistics.Rows[3].Cells[9].Value = r37;
            Statistics.Rows[3].Cells[10].Value = ord2;
            Statistics.Rows[3].Cells[11].Value = ref2;
            Statistics.Rows.Add();
            Statistics.Rows[4].Cells[0].Value = "4 этаж";
            Statistics.Rows[4].Cells[1].Value = f41;
            Statistics.Rows[4].Cells[2].Value = f42;
            Statistics.Rows[4].Cells[3].Value = r40;
            Statistics.Rows[4].Cells[4].Value = r42;
            Statistics.Rows[4].Cells[5].Value = r43;
            Statistics.Rows[4].Cells[6].Value = r44;
            Statistics.Rows[4].Cells[7].Value = r45;
            Statistics.Rows[4].Cells[8].Value = r46;
            Statistics.Rows[4].Cells[9].Value = r47;
            Statistics.Rows[4].Cells[10].Value = ord3;
            Statistics.Rows[4].Cells[11].Value = ref3;
            Statistics.Rows.Add();
            Statistics.Rows[5].Cells[0].Value = "5 этаж";
            Statistics.Rows[5].Cells[1].Value = f51;
            Statistics.Rows[5].Cells[2].Value = f52;
            Statistics.Rows[5].Cells[3].Value = r50;
            Statistics.Rows[5].Cells[4].Value = r52;
            Statistics.Rows[5].Cells[5].Value = r53;
            Statistics.Rows[5].Cells[6].Value = r54;
            Statistics.Rows[5].Cells[7].Value = r55;
            Statistics.Rows[5].Cells[8].Value = r56;
            Statistics.Rows[5].Cells[9].Value = r57;
            Statistics.Rows[5].Cells[10].Value = ord4;
            Statistics.Rows[5].Cells[11].Value = ref4;
            Statistics.Rows.Add();
            Statistics.Rows[6].Cells[0].Value = "6 этаж";
            Statistics.Rows[6].Cells[1].Value = f61;
            Statistics.Rows[6].Cells[2].Value = f62;
            Statistics.Rows[6].Cells[3].Value = r60;
            Statistics.Rows[6].Cells[4].Value = r62;
            Statistics.Rows[6].Cells[5].Value = r63;
            Statistics.Rows[6].Cells[6].Value = r64;
            Statistics.Rows[6].Cells[7].Value = r65;
            Statistics.Rows[6].Cells[8].Value = r66;
            Statistics.Rows[6].Cells[9].Value = r67;
            Statistics.Rows[6].Cells[10].Value = ord5;
            Statistics.Rows[6].Cells[11].Value = ref5;
            Statistics.Rows.Add();
            Statistics.Rows[7].Cells[0].Value = "7 этаж";
            Statistics.Rows[7].Cells[1].Value = f71;
            Statistics.Rows[7].Cells[2].Value = f72;
            Statistics.Rows[7].Cells[3].Value = r70;
            Statistics.Rows[7].Cells[4].Value = r72;
            Statistics.Rows[7].Cells[5].Value = r73;
            Statistics.Rows[7].Cells[6].Value = r74;
            Statistics.Rows[7].Cells[7].Value = r75;
            Statistics.Rows[7].Cells[8].Value = r76;
            Statistics.Rows[7].Cells[9].Value = r77;
            Statistics.Rows[7].Cells[10].Value = ord6;
            Statistics.Rows[7].Cells[11].Value = ref6;
            Statistics.Rows.Add();
            Statistics.Rows[8].Cells[0].Value = "Не в базе (ОФ)";
            Statistics.Rows[8].Cells[1].Value = f81;
            Statistics.Rows[8].Cells[2].Value = f82;
            Statistics.Rows[8].Cells[3].Value = r80;
            Statistics.Rows[8].Cells[4].Value = r82;
            Statistics.Rows[8].Cells[5].Value = r83;
            Statistics.Rows[8].Cells[6].Value = r84;
            Statistics.Rows[8].Cells[7].Value = r85;
            Statistics.Rows[8].Cells[8].Value = r86;
            Statistics.Rows[8].Cells[9].Value = r87;
            Statistics.Rows[8].Cells[10].Value = ord7;
            Statistics.Rows[8].Cells[11].Value = ref7;
            Statistics.Rows.Add();
            Statistics.Rows[9].Cells[0].Value = "Всего (без редкой книги)";
            Statistics.Rows[9].Cells[1].Value = f91;
            Statistics.Rows[9].Cells[2].Value = f92;
            Statistics.Rows[9].Cells[3].Value = (int.Parse(r00) + int.Parse(r20) + int.Parse(r30) + int.Parse(r40) + int.Parse(r50) + int.Parse(r60) + int.Parse(r70) + int.Parse(r80) + int.Parse(r90)).ToString();
            Statistics.Rows[9].Cells[4].Value = (int.Parse(r02) + int.Parse(r22) + int.Parse(r32) + int.Parse(r42) + int.Parse(r52) + int.Parse(r62) + int.Parse(r72) + int.Parse(r82) + int.Parse(r92)).ToString();
            Statistics.Rows[9].Cells[5].Value = (int.Parse(r03) + int.Parse(r23) + int.Parse(r33) + int.Parse(r43) + int.Parse(r53) + int.Parse(r63) + int.Parse(r73) + int.Parse(r83) + int.Parse(r93)).ToString();
            Statistics.Rows[9].Cells[6].Value = (int.Parse(r04) + int.Parse(r24) + int.Parse(r34) + int.Parse(r44) + int.Parse(r54) + int.Parse(r64) + int.Parse(r74) + int.Parse(r84) + int.Parse(r94)).ToString();
            Statistics.Rows[9].Cells[7].Value = (int.Parse(r05) + int.Parse(r25) + int.Parse(r35) + int.Parse(r45) + int.Parse(r55) + int.Parse(r65) + int.Parse(r75) + int.Parse(r85) + int.Parse(r95)).ToString();
            Statistics.Rows[9].Cells[8].Value = (int.Parse(r06) + int.Parse(r26) + int.Parse(r36) + int.Parse(r46) + int.Parse(r56) + int.Parse(r66) + int.Parse(r76) + int.Parse(r86) + int.Parse(r96)).ToString();
            Statistics.Rows[9].Cells[9].Value = (int.Parse(r07) + int.Parse(r27) + int.Parse(r37) + int.Parse(r47) + int.Parse(r57) + int.Parse(r67) + int.Parse(r77) + int.Parse(r87) + int.Parse(r97)).ToString();
            Statistics.Rows[9].Cells[10].Value = ord8;
            Statistics.Rows[9].Cells[11].Value = ref8;
            Statistics.Rows.Add();
            Statistics.Rows[10].Cells[0].Value = "Редкой книги";
            Statistics.Rows[10].Cells[1].Value = f101;
            Statistics.Rows[10].Cells[2].Value = f102;
            Statistics.Rows[10].Cells[3].Value = rr10;
            Statistics.Rows[10].Cells[4].Value = rr12;
            Statistics.Rows[10].Cells[5].Value = rr13;
            Statistics.Rows[10].Cells[6].Value = rr14;
            Statistics.Rows[10].Cells[7].Value = rr15;
            Statistics.Rows[10].Cells[8].Value = rr16;
            Statistics.Rows[10].Cells[9].Value = rr17;
            Statistics.Rows[10].Cells[10].Value = ord9;
            Statistics.Rows[10].Cells[11].Value = ref9;
            Statistics.Rows.Add();
            Statistics.Rows[11].Cells[0].Value = "Не в базе (Фонд редкой книги)";
            Statistics.Rows[11].Cells[1].Value = f111;
            Statistics.Rows[11].Cells[2].Value = f112;
            Statistics.Rows[11].Cells[3].Value = rr00;
            Statistics.Rows[11].Cells[4].Value = rr02;
            Statistics.Rows[11].Cells[5].Value = rr03;
            Statistics.Rows[11].Cells[6].Value = rr04;
            Statistics.Rows[11].Cells[7].Value = rr05;
            Statistics.Rows[11].Cells[8].Value = rr06;
            Statistics.Rows[11].Cells[9].Value = rr07;
            Statistics.Rows[11].Cells[10].Value = ord10;
            Statistics.Rows[11].Cells[11].Value = ref10;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (Statistics.Rows.Count == 0)
            {
                MessageBox.Show("Нечего экспортировать!");
                return;
            }
            DataTable dt = (DataTable)Statistics.DataSource;

            StringBuilder fileContent = new StringBuilder();

            foreach (DataGridViewColumn dc in Statistics.Columns)
            {
                fileContent.Append(dc.HeaderText + ";");
            }

            fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);



            foreach (DataGridViewRow dr in Statistics.Rows)
            {

                foreach (DataGridViewCell cell in dr.Cells)
                {
                    fileContent.Append("\"" + cell.Value.ToString() + "\";");
                }

                fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            string tmp = label19.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            tmp = label19.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Сохранить в файл";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            TextWriter tw;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(sd.FileName, fileContent.ToString(), Encoding.UTF8);
            }
            //string strExport = "";
            ////Loop through all the columns in DataGridView to Set the 
            ////Column Heading
            //foreach (DataGridViewColumn dc in Statistics.Columns)
            //{
            //    strExport += dc.HeaderText.Replace(";", " ") + "  ; ";
            //}
            //strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString();
            ////Loop through all the row and append the value with 3 spaces
            //foreach (DataGridViewRow dr in Statistics.Rows)
            //{
            //    foreach (DataGridViewCell dc in dr.Cells)
            //    {
            //        if (dc.Value != null)
            //        {
            //            strExport += dc.FormattedValue.ToString().Replace(";", " ") + " ;  ";
            //        }
            //    }
            //    strExport += Environment.NewLine.ToString();
            //}
            //strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString() + Environment.NewLine.ToString() + DateTime.Now.ToString("dd.MM.yyyy")+"  "+this.DepName + " - " + this.textBox1.Text;
            ////Create a TextWrite object to write to file, select a file name with .csv extention
            //string tmp = "D:\\" + label19.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            //tmp = label19.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            //SaveFileDialog sd = new SaveFileDialog();
            //sd.Title = "Сохранить в файл";
            //sd.Filter = "csv files (*.csv)|*.csv";
            //sd.FilterIndex = 1;
            //TextWriter tw;
            //sd.FileName = tmp;
            //if (sd.ShowDialog() == DialogResult.OK)
            //{
            //        tmp = sd.FileName;
            //        tw = new System.IO.StreamWriter(tmp, false, Encoding.UTF8);
            //        //Write the Text to file
            //        //tw.Encoding = Encoding.Unicode;
            //        tw.Write(strExport);
            //        //Close the Textwrite
            //        tw.Close();
            //}
        }

        private delegate object GetAllBooksInHallDelegate();
        private delegate void EndGetAllBooksInHallDelegate(DataTable t);

        private void button1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
            {
                if (Statistics.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Не выбрано ни одной записи!");
                    return;
                }

                dbBook b = new dbBook(Statistics.SelectedRows[0].Cells["bar"].Value.ToString(), this.BASENAME);
                dbw.InsertStatisticsBookkeeping(Statistics.SelectedRows[0].Cells["bar"].Value.ToString(), "",b);
                dbw.MoveToHistory(b);
                Statistics.Rows.Remove(Statistics.SelectedRows[0]);
                MessageBox.Show("Книга успешно сдана в хранение!");

            }
            if (tabControl1.SelectedIndex == 1)
            {
                if (Formular.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Не выбрано ни одной записи!");
                    return;
                }
                if (Formular.SelectedRows[0].Cells["status"].Value.ToString() == "На бронеполке")
                {
                    dbBook b = new dbBook(Formular.SelectedRows[0].Cells["bar"].Value.ToString(), this.BASENAME);
                    dbw.InsertStatisticsBookkeeping(Formular.SelectedRows[0].Cells["bar"].Value.ToString(), "",b);
                    dbw.MoveToHistory(b);
                    Formular.Rows.Remove(Formular.SelectedRows[0]);
                    MessageBox.Show("Книга успешно сдана в хранение!");
                }
                else
                {
                    MessageBox.Show("Книга не может быть сдана, т.к. она на руках у читателя!");
                    return;
                }
            }
        }

        private void книгиПодготовленныеКСдачеВХранениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AllowUserToDeleteRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги подготовленные к сдаче в хранение";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetPREPFORBKBooks();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Штрихкод";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Место хранения";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Расстано вочный шифр";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].Visible = false;
            button11.Enabled = true;
        }
        public Form31 f31;
        private void button9_Click(object sender, EventArgs e)
        {
            f31 = new Form31();
            f31.ShowDialog();
            f31 = null;
        }

        private void историяИнвентаряToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Form32 f32 = new Form32();
            f32.ShowDialog();
            if (f32.textBox1.Text == "")
            {
                return;
            }
            string inv = f32.textBox1.Text;
            Form33 f33 = new Form33(inv,dbw);
            if (f33.CanShow)
            {
                f33.ShowDialog();
            }
            else
            {
                MessageBox.Show("Инвентарный номер не найден в базе!");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int x = this.Left + button11.Left;
            int y = this.Top + button11.Top + tabControl1.Top + 60;
            cmsBack.Show(x, y);


        }
        private void вернутьНаБронеполкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Rows.Count == 0)
            {
                MessageBox.Show("Не выбрано ни одной записи!");
                return;
            }
            dbBook b = new dbBook(Statistics.SelectedRows[0].Cells["bar"].Value.ToString(), this.BASENAME);
            b.RESPAN = Statistics.SelectedRows[0].Cells["respan"].Value.ToString();
            dbw.ReturnOnRESPAN(b);
            Statistics.Rows.Remove(Statistics.SelectedRows[0]);
            MessageBox.Show("Книга успешно возвращена на бронеполку текущего читателя!");
        }

        private void подготовитьДляВыдачиНовомуЧитателюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Statistics.Rows.Count == 0)
            {
                MessageBox.Show("Не выбрано ни одной записи!");
                return;
            }
            dbBook b = new dbBook(Statistics.SelectedRows[0].Cells["bar"].Value.ToString(), this.BASENAME);
            b.RESPAN = Statistics.SelectedRows[0].Cells["respan"].Value.ToString();
            dbw.ReturnOnRESPANForNewReader(b);
            Statistics.Rows.Remove(Statistics.SelectedRows[0]);
            MessageBox.Show("Книга успешно подготовлена для выдачи новому читателю!");
        }

        private void Statistics_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Form3 f3 = new Form3();
            if (label19.Text.Contains("Отчет отдела Книгохранение за период"))
            {
                if (Convert.ToInt32(Statistics.Rows[e.RowIndex].Cells[1].Value) <= Convert.ToInt32(Statistics.Rows[e.RowIndex].Cells[2].Value))
                {
                    MessageBox.Show("Разница не может быть показана! Количество выданных книг меньше либо равно количеству принятых книг!");
                    return;
                }
                DataTable delta = new DataTable();
                switch (e.RowIndex)
                {
                    case 0://Цоколь
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 0 этаж", f3Start, f3End);
                        break;
                    case 1:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 2 этаж", f3Start, f3End);
                        break;
                    case 2:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 3 этаж", f3Start, f3End);
                        break;
                    case 3:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 4 этаж", f3Start, f3End);
                        break;
                    case 4:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 5 этаж", f3Start, f3End);
                        break;
                    case 5:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 6 этаж", f3Start, f3End);
                        break;
                    case 6:
                        delta = dbw.GetDeltaISS_REC("…Хран… Сектор книгохранения - 7 этаж", f3Start, f3End);
                        break;
                    case 7:
                        delta = dbw.GetDeltaISS_REC("Не в базе", f3Start, f3End);
                        break;
                    case 8:
                        delta = dbw.GetDeltaISS_REC("Всего", f3Start, f3End);
                        break;
                    case 9:
                        delta = dbw.GetDeltaISS_REC("…Хран… КНИО Группа хранения редкой книги", f3Start, f3End);
                        break;
                    case 10:
                        delta = dbw.GetDeltaISS_REC("Не в базе редкой книги", f3Start, f3End);
                        break;
                }
                //BackTable =  ((DataTable)Statistics.DataSource).Copy();
                backRows = new DataGridViewRow[Statistics.Rows.Count];
                Statistics.Rows.CopyTo(backRows, 0);
                backLabelText = label19.Text;
                label19.Text = "Разница выданных и принятых книг: " + Statistics.Rows[e.RowIndex].Cells[0].Value.ToString();
                Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                Statistics.Columns.Clear();
                Statistics.AutoGenerateColumns = true;
                Statistics.DataSource = delta;
                autoinc(Statistics);
                Statistics.Columns[0].HeaderText = "№№";
                Statistics.Columns[0].Width = 40;
                Statistics.Columns[1].HeaderText = "Заглавие";
                Statistics.Columns[1].Width = 300;
                Statistics.Columns[2].HeaderText = "Автор";
                Statistics.Columns[3].Width = 150;
                Statistics.Columns[3].HeaderText = "Инв номер";
                Statistics.Columns[3].Width = 100;
                Statistics.Columns[4].HeaderText = "Отдел принявший из к/х";
                Statistics.Columns[4].Width = 150;
                Statistics.Columns[5].Visible = false;
                Statistics.Columns[5].Width = 100;
                Statistics.Columns[6].HeaderText = "Расс. шифр";
                Statistics.Columns[6].Width = 100;
                Statistics.Columns[7].HeaderText = "Дата приема в отдел";
                Statistics.Columns[8].Visible = false;
                Statistics.Columns[9].Visible = false;
                button15.Visible = true;
            }
            else
            {
                if (label19.Text.Contains("Разница выданных и принятых книг:"))
                {
                    Form33 f33 = new Form33(Statistics.Rows[e.RowIndex].Cells[3].Value.ToString(),dbw);
                    if (f33.CanShow)
                    {
                        f33.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Инвентарный номер не найден в базе!");
                    }
                }
                else
                {
                    if (label19.Text.Contains("Отчет отдела за период") && (e.RowIndex == 5))
                    {
                        Statistics.Columns.Clear();
                        Statistics.AllowUserToAddRows = false;
                        Statistics.AutoGenerateColumns = true;
                        //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                        label19.Text = "Список читателей зала за период с "+StartDateReadersInHall.ToString("dd.MM.yyyy")
                            + " по " + EndDateReadersInHall.ToString("dd.MM.yyyy");
                        Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                        Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                        try
                        {
                            Statistics.DataSource = dbw.GetReaders(StartDateReadersInHall, EndDateReadersInHall);
                        }
                        catch (IndexOutOfRangeException ev)
                        {
                            string s = ev.Message;
                            MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Statistics.Columns.Clear();
                            return;
                        }
                        autoinc(Statistics);
                        //((DataTable)Statistics.DataSource).Columns[5].DateTimeMode = DataSetDateTime.Local;
                        Statistics.Columns[0].HeaderText = "№№";
                        Statistics.Columns[0].Width = 40;
                        Statistics.Columns[1].HeaderText = "Фамилия";
                        Statistics.Columns[1].Width = 200;
                        Statistics.Columns[2].HeaderText = "Имя";
                        Statistics.Columns[2].Width = 150;
                        Statistics.Columns[3].HeaderText = "Отчество";
                        Statistics.Columns[3].Width = 150;
                        Statistics.Columns[4].HeaderText = "Номер читателя";
                        Statistics.Columns[4].Width = 120;
                        Statistics.Columns[5].HeaderText = "Дата/время";
                        Statistics.Columns[5].ValueType = typeof(DateTime);
                        
                        Statistics.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy";
                        Statistics.Columns[5].Width = 160;
                    }
                    else
                    {
                    }
                    //return;
                }
            }
            if (label19.Text.Contains("Книги на руках у сотрудников поэтажно"))
            {
                //Statistics.Columns.Clear();
                Statistics.AllowUserToAddRows = false;
                Statistics.AutoGenerateColumns = true;
                //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                label19.Text = "Список книг на руках у сторудников " + Statistics.Rows[e.RowIndex].Cells[0].Value;
                Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                try
                {
                    Statistics.DataSource = dbw.GetFloorAtHome(Statistics.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                catch (IndexOutOfRangeException ev)
                {
                    string s = ev.Message;
                    MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Statistics.Columns.Clear();
                    return;
                }
                autoinc(Statistics);
                Statistics.Columns[0].HeaderText = "№№";
                Statistics.Columns[0].Width = 30;
                Statistics.Columns[1].HeaderText = "Заглавие";
                Statistics.Columns[1].Width = 250;
                Statistics.Columns[2].HeaderText = "Автор";
                Statistics.Columns[2].Width = 120;
                Statistics.Columns[3].HeaderText = "Инв номер/ шкод";
                Statistics.Columns[3].Width = 100;
                Statistics.Columns[4].HeaderText = "Номер читателя";
                Statistics.Columns[4].Width = 100;
                Statistics.Columns[5].HeaderText = "Кафедра выдачи";
                Statistics.Columns[5].Width = 100;
                Statistics.Columns[6].HeaderText = "Дата выдачи";
                Statistics.Columns[6].Width = 90;
                Statistics.Columns[7].HeaderText = "Дата возврата";
                Statistics.Columns[7].Width = 90;
                Statistics.Columns[8].HeaderText = "Дней просро чено";
                Statistics.Columns[8].Width = 60;
            }
        }
        string backLabelText;
        DataGridViewRow[] backRows;
        private void button15_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.DataSource = null;
            Statistics.AutoGenerateColumns = false;
            Statistics.Columns.Add("sektor", "Сектор");
            Statistics.Columns[0].Width = 250;
            Statistics.Columns.Add("D", "Кол-во выданных изданий");
            Statistics.Columns[1].Width = 150;
            Statistics.Columns.Add("C", "Кол-во принятых изданий");
            Statistics.Columns[2].Width = 150;
            Statistics.Columns.Add("F0", "Цоколь");
            Statistics.Columns[3].Width = 70;
            Statistics.Columns.Add("F2", "2 этаж");
            Statistics.Columns[4].Width = 70;
            Statistics.Columns.Add("F3", "3 этаж");
            Statistics.Columns[5].Width = 70;
            Statistics.Columns.Add("F4", "к/х ред. кн.");
            Statistics.Columns[6].Width = 70;
            Statistics.Columns.Add("F5", "5 этаж");
            Statistics.Columns[7].Width = 70;
            Statistics.Columns.Add("F6", "6 этаж");
            Statistics.Columns[8].Width = 70;
            Statistics.Columns.Add("F7", "7 этаж");
            Statistics.Columns[9].Width = 70;

            Statistics.Rows.AddRange(backRows);

            label19.Text = backLabelText;
            button15.Visible = false;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void книгиПодготовленныеКСдачеНаТекущийЭтажКнигохраненияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.DepName.Contains("Книгохране"))
            {
                MessageBox.Show("Вы не сотрудник книгохранения!");
                return;
            }
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AllowUserToDeleteRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги подготовленные к сдаче в хранение на текущий этаж (без книг, которые не в базе)";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetBooksForCurrentFloor();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Штрихкод";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Место хранения";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Расстано вочный шифр";
            Statistics.Columns[6].Width = 100;
            //Statistics.Columns[7].Visible = false;
                

        }

        private void button27_Click(object sender, EventArgs e)
        {
            Form38 f38 = new Form38(this,0);
            f38.ShowDialog();
            ShowFreeServices();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string id = PaidServiceGrid.SelectedRows[0].Cells[5].Value.ToString();
            Form35 f35 = new Form35(this,1,id);
            f35.ShowDialog();
            ShowPaidServices();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            Form38 f38 = new Form38(this,1);
            f38.ShowDialog();
            ShowPaidServices();
        }

        private void индивидуальнаяСправкаПоВыданнымБесплатнымСправкамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Отчет индивидуальный по выданным б/п услугам за период c " + f3.StartDate.ToString("dd.MM.yyyy") 
                +" по " +f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetFreeIndividual(f3.StartDate, f3.EndDate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Наименование справки";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 150;
        }

        private void индивидуальнаяСправкаПоВыданнымПлатнымСправкамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Отчет индивидуальный по выданным платным услугам за период c " + f3.StartDate.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetPaidIndividual(f3.StartDate, f3.EndDate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Наименование справки";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Цена";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Стоимость";
            Statistics.Columns[4].Width = 130;
        }

        private void справкаОтделаПоВыданнымБесплатнымСправкамЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Отчет отдела по выданным бесплатным услугам за период c " + f3.StartDate.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetFreeDep(f3.StartDate, f3.EndDate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Наименование справки";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 150;

        }

        private void справкаОтделаПоВыданнымПлатнымСправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Отчет отдела по выданным платным услугам за период с " + f3.StartDate.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetPaidDep(f3.StartDate, f3.EndDate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Наименование справки";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Количество";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Цена";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Стоимость";
            Statistics.Columns[4].Width = 130;
        }

        private void посеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Список посетителей зала без книговыдачи c " + f3.StartDate.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetAttendace(f3.StartDate.Date, f3.EndDate.Date);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            //((DataTable)Statistics.DataSource).Columns[5].DateTimeMode = DataSetDateTime.Local;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Фамилия";
            Statistics.Columns[1].Width = 200;
            Statistics.Columns[2].HeaderText = "Имя";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Отчество";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 120;
            Statistics.Columns[5].HeaderText = "Дата/время";
            Statistics.Columns[5].ValueType = typeof(DateTime);
            
            Statistics.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            Statistics.Columns[5].Width = 160;
        }

        private void списокУникальныхПосетителейЗалаБезУслугКниговыдачиЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Список уникальных посетителей зала без книговыдачи c " + f3.StartDate.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetAttendaceUnique(f3.StartDate.Date, f3.EndDate.Date);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            //((DataTable)Statistics.DataSource).Columns[5].DateTimeMode = DataSetDateTime.Local;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Фамилия";
            Statistics.Columns[1].Width = 200;
            Statistics.Columns[2].HeaderText = "Имя";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Отчество";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 120;
            Statistics.Columns[5].HeaderText = "Количество посещений";
            //Statistics.Columns[5].ValueType = typeof(DateTime);

            //Statistics.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            Statistics.Columns[5].Width = 160;
        }

        private void списокУникальныхПосетителейЗалаПолучившихЛитературуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            //Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Список уникальных читателей зала за период с " + f3.StartDate.Date.ToString("dd.MM.yyyy")
                + " по " + f3.EndDate.Date.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            try
            {
                Statistics.DataSource = dbw.GetReadersUnique(f3.StartDate.Date, f3.EndDate.Date);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Ошибка.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            //((DataTable)Statistics.DataSource).Columns[5].DateTimeMode = DataSetDateTime.Local;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Фамилия";
            Statistics.Columns[1].Width = 200;
            Statistics.Columns[2].HeaderText = "Имя";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Отчество";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 120;
            Statistics.Columns[5].HeaderText = "Количество";
            //Statistics.Columns[5].ValueType = typeof(DateTime);

            //Statistics.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy";
            Statistics.Columns[5].Width = 160;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }

            Orders o = new Orders(ReaderRecordFormular.id);
            o.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //BasicHttpBinding basicHttpBinding = new BasicHttpBinding();

            //basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;

            //basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;

            //EndpointAddress endpoint = new EndpointAddress("http://opac.libfl.ru/LibFLDataProviderAPI");

            ////List.ListsSoapClient client = new ConsoleApplication1.List.ListsSoapClient();
            //LibFLDataProviderAPI.ServiceSoapClient client = new Circulation.LibFLDataProviderAPI.ServiceSoapClient(basicHttpBinding, endpoint);

            //client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;

            //client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            //client.ChannelFactory.Credentials.Windows.ClientCredential.UserName= "Admin";
            //client.ChannelFactory.Credentials.Windows.ClientCredential.Password = "Boss_1215";





            //client.ClientCredentials.Windows.ClientCredential.UserName = "Admin";
            //client.ClientCredentials.Windows.ClientCredential.Password = "Boss_1215";
            LibFLDataProviderAPI.ServiceSoapClient client = new Circulation.LibFLDataProviderAPI.ServiceSoapClient();
            string ri = client.GetReaderInfo("1").FamilyName;
            ri = client.Authorize("189245", "PfrdfcrF");
            int i = 3 + 2;
            dbBook b ;
            dbReader r ;

            
            
            //FromPort = "U100427371";//зал абонемента
            //Form1_Scanned(sender, e);
            //FromPort = "R1027501";
            //Form1_Scanned(sender, e);


            //FromPort = "U100427372";//книгохранение абонемента
            //Form1_Scanned(sender, e);
            //FromPort = "R1027501";
            //Form1_Scanned(sender, e);


            

            //dbBook b = new dbBook("U100200485", this.BASENAME);
            //dbReader r = new dbReader(113940);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100076706", this.BASENAME);
            //r = new dbReader(113940);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100002166", this.BASENAME);
            //r = new dbReader(113940);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100202658", this.BASENAME);
            //r = new dbReader(113940);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100272868", this.BASENAME);
            //r = new dbReader(113940);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100087239", this.BASENAME);
            //r = new dbReader(163709);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100340590", this.BASENAME);
            //r = new dbReader(163709);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100165509", this.BASENAME);
            //r = new dbReader(181555);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100340419", this.BASENAME);
            //r = new dbReader(165808);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100272473", this.BASENAME);
            //r = new dbReader(186531);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100278480", this.BASENAME);
            //r = new dbReader(149559);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100089214", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100381841", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100089084", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100075949", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100075218", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100095562", this.BASENAME);
            //r = new dbReader(182445);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100110790", this.BASENAME);
            //r = new dbReader(188666);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100271390", this.BASENAME);
            //r = new dbReader(182178);
            //dbw.setBookForReaderHome(b, r);
            //b = new dbBook("U100232867", this.BASENAME);
            //r = new dbReader(182178);
            //dbw.setBookForReaderHome(b, r);

            //b = new dbBook("U100382326", this.BASENAME);
            //r = new dbReader(151446);
            //dbw.setBookForReaderHome(b, r);

            ////string s = dbw.getReaderIDFromOrders(b);
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            f25 = new Form25(dbw, this);
            f25.ShowDialog();
            f25 = null;
        }



        private void button18_Click(object sender, EventArgs e)
        {
            if (label19.Text == "Список нарушителей сроков пользования документов из основного фонда")
            {
                if (Statistics.SelectedRows.Count != 0)
                {
                    FormularColumnsForming(Statistics.SelectedRows[0].Cells[2].Value.ToString());
                    OverdueEmail oe = new OverdueEmail(this, new dbReader((int)Statistics.SelectedRows[0].Cells[2].Value), this.Formular, dbw);
                    if (oe.canshow)
                        oe.ShowDialog();
                }
            }
            if (label19.Text == "Книги на руках, с просроченным сроком сдачи")
            {
                if (Statistics.SelectedRows.Count != 0)
                {
                    FormularColumnsForming(Statistics.SelectedRows[0].Cells[3].Value.ToString());
                    OverdueEmail oe = new OverdueEmail(this, new dbReader((int)Statistics.SelectedRows[0].Cells[3].Value), this.Formular, dbw);
                    if (oe.canshow)
                        oe.ShowDialog();
                }
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //dbw.InsertTEST1();
            //Form28 f28 = new Form28(



        }

        private void button29_Click(object sender, EventArgs e)
        {
            Form20 f20 = new Form20(this);
            f20.ShowDialog();
            if (f20.pass != "aa")
            {
                MessageBox.Show("Неверный пароль");
                return;
            }
            
            
            //LostBook lb = new LostBook(
            if (Formular.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка в таблице!");
                return;
            }
            if (MessageBox.Show("Вы действительно хотите снять ответственность за выбранную книгу с читателя?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            dbBook b = new dbBook(Formular.SelectedRows[0].Cells["bar"].Value.ToString(), this.BASENAME);
            dbw.RemoveResponsibility(b);

            this.FormularColumnsForming(ReaderRecordFormular.id);

        }

        private void bReaderRegistrationAndRights_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(label25.Text));
            fReaderRegistrationAndRights frr = new fReaderRegistrationAndRights();
            frr.Init(reader.ID);
            frr.ShowDialog();
        }

        private void bFormularEmail_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }

            OverdueEmail oe = new OverdueEmail(this, new dbReader(ReaderRecordFormular.IntID), this.Formular, dbw);
            if (oe.canshow)
            {
                oe.ShowDialog();
            }

        }

        //private void списокЧитателейИКнигВыданныхНаДомСПросроченнымСрокомСдачиToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //}

        private void RPhoto_Click(object sender, EventArgs e)
        {
            if (RPhoto.Image == null) return;
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(RPhoto.Image);
            fullsize.ShowDialog();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image == null) return;
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(pictureBox2.Image);
            fullsize.ShowDialog();

        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (label25.Text == "")
            {
                MessageBox.Show("Введите номер или считайте штрихкод читателя!");
                return;
            }
            ReaderVO reader = new ReaderVO(int.Parse(label25.Text));

            ChangeComment cc = new ChangeComment(reader);
            cc.ShowDialog();

        }

        

       

      















        //private void обращаемостьКнигОФТекущегоЗалаПоPINЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        //{

       // }



    }

    public static class Conn
    {
        public static SqlConnection ReadersCon;
        public static SqlConnection ZakazCon;
        public static SqlConnection BRIT_SOVETCon;
        public static SqlConnection BJVVVConn;
        public static SqlDataAdapter ReaderDA;
        public static SqlDataAdapter SQLDA;
    }
    class DBDataGridView : System.Windows.Forms.DataGridView
    {

        public DBDataGridView()
        {
            this.DoubleBuffered = true;
        }

    }
    
    class DBNumericUpDown : NumericUpDown
    {
        public DBNumericUpDown()
        {
            this.DoubleBuffered = true;
        }
    }
}
