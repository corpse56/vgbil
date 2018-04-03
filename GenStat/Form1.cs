using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GenStat
{
    public partial class Form1 : Form
    {
        private DateTime                Start;
        private DateTime                End;
        private bool                    CanShow = false;
        private RefT                    rtype;
        private StatDB                  DB;
        private ExtGui.RoundProgress    RndPrg;

        public Form1()
        {
            InitializeComponent();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            this.Start = f3.StartDate;
            this.End = f3.EndDate;
            this.CanShow = f3.CanShow;
            if (!f3.CanShow) return;
            RefType rt = new RefType();
            rt.ShowDialog();
            rtype = rt.rt;
            this.CanShow = rt.CanShow;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.CanShow)
            {
                this.Close();
                return;
            }
            DB = new StatDB(this.Start, this.End);
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            switch (rtype)
            {
                case RefT.Service:
                    label1.Text = "Справка обслуживания за период с " + Start.ToString("dd.MM.yyyy") + " по " + End.ToString("dd.MM.yyyy");
                    CreateServiceGrid();
                    this.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                    //ShowServiceGrid();
                    break;
                case RefT.BookSupply:
                    label1.Text = "Справка пополнения фондов за период с " + Start.ToString("dd.MM.yyyy") + " по " + End.ToString("dd.MM.yyyy");
                    CreateBookSupply();
                    this.Enabled = false;
                    backgroundWorker2.RunWorkerAsync();
                    //ShowBookSupply();
                    break;
                case RefT.QoS:
                    label1.Text = "Справка качества услуг за период с " + Start.ToString("dd.MM.yyyy") + " по " + End.ToString("dd.MM.yyyy");
                    CreateQoS();
                    this.Enabled = false;
                    backgroundWorker3.RunWorkerAsync();
                    //ShowBookSupply();
                    break;
            }

        }

        private void Autoinc()
        {
            int i = 0;
            foreach (DataGridViewRow row in StatGrid.Rows)
            {
                row.Cells[0].Value = (++i).ToString();
            }
        }
        private void CreateServiceGrid()
        {
            StatGrid.Columns.Clear();
            StatGrid.AutoGenerateColumns = false;
            StatGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            StatGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№№";
            col.Width = 50;
            col.Name = "ID";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Наименование справки";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Name = "Name";
            col.Width = 500;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Количество";
            col.Name = "Count";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Width = 100;
            StatGrid.Columns.Add(col);

            //DataGridViewRow row = new DataGridViewRow();
            StatGrid.Rows.Insert(0, 22);
            StatGrid.Rows[0].Cells[0].Value = "1";
            StatGrid.Rows[0].Cells[1].Value = "Количество зарегистрированных новых читателей";

            StatGrid.Rows[1].Cells[0].Value = "";
            StatGrid.Rows[1].Cells[1].Value = "1.1. Пользователь читальных залов БИЛ";
            StatGrid.Rows[2].Cells[0].Value = "";
            StatGrid.Rows[2].Cells[1].Value = "1.2. Пользователь Британского совета";
            StatGrid.Rows[3].Cells[0].Value = "";
            StatGrid.Rows[3].Cells[1].Value = "1.3. Сотрудник БИЛ";
            StatGrid.Rows[4].Cells[0].Value = "";
            StatGrid.Rows[4].Cells[1].Value = "1.4. Индивидуальный абонемент";
            StatGrid.Rows[5].Cells[0].Value = "";
            StatGrid.Rows[5].Cells[1].Value = "1.5. Персональный абонемент";
            StatGrid.Rows[6].Cells[0].Value = "";
            StatGrid.Rows[6].Cells[1].Value = "1.6. Коллективный абонемент";

            StatGrid.Rows[7].Cells[0].Value = "2";
            StatGrid.Rows[7].Cells[1].Value = "Количество перерегистрированных читателей";

            StatGrid.Rows[8].Cells[0].Value = "3";
            StatGrid.Rows[8].Cells[1].Value = "Количество посетителей библиотеки";

            StatGrid.Rows[9].Cells[0].Value = "4";
            StatGrid.Rows[9].Cells[1].Value = "Количество выданной литературы";
            StatGrid.Rows[10].Cells[0].Value = "";
            StatGrid.Rows[10].Cells[1].Value = "4.1.  До 14 лет";
            StatGrid.Rows[11].Cells[0].Value = "";
            StatGrid.Rows[11].Cells[1].Value = "4.2.  От 14 до 24 года";
            StatGrid.Rows[12].Cells[0].Value = "";
            StatGrid.Rows[12].Cells[1].Value = "4.3.  По индивидуальному абонементу";
            StatGrid.Rows[13].Cells[0].Value = "";
            StatGrid.Rows[13].Cells[1].Value = "4.4.  По персональному абонементу";
            StatGrid.Rows[14].Cells[0].Value = "";
            StatGrid.Rows[14].Cells[1].Value = "4.5.  По коллективному абонементу";
            StatGrid.Rows[15].Cells[0].Value = "";
            StatGrid.Rows[15].Cells[1].Value = "4.6.  По абонементу Британского совета";

            StatGrid.Rows[16].Cells[0].Value = "5";
            StatGrid.Rows[16].Cells[1].Value = "Количество выданной литературы удалённым пользователям (электронная книга)";
            StatGrid.Rows[17].Cells[0].Value = "";
            StatGrid.Rows[17].Cells[1].Value = "5.1. С Авторским правом";
            StatGrid.Rows[18].Cells[0].Value = "";
            StatGrid.Rows[18].Cells[1].Value = "5.2. Без авторского права";

            StatGrid.Rows[19].Cells[0].Value = "6";
            StatGrid.Rows[19].Cells[1].Value = "Количество читателей, получивших литературу";

            StatGrid.Rows[20].Cells[0].Value = "7";
            StatGrid.Rows[20].Cells[1].Value = "Количество посетителей зала без услуг книговыдачи (открытый доступ, интернет, выставки зала, консультации и т.д.)";

            StatGrid.Rows[21].Cells[0].Value = "8";
            StatGrid.Rows[21].Cells[1].Value = "Количество выданных бесплатных справок";
        }
        private void ShowServiceGrid()
        {
            StatGrid.Rows[0].Cells[2].Value = DB.GetReadersRegCount().ToString();
            StatGrid.Rows[1].Cells[2].Value = DB.GetReadersRegCountSimple().ToString();
            StatGrid.Rows[2].Cells[2].Value = DB.GetReadersRegCountBS().ToString();
            StatGrid.Rows[3].Cells[2].Value = DB.GetReadersRegCountEmpl().ToString();
            StatGrid.Rows[4].Cells[2].Value = DB.GetReadersRegCountInd().ToString();
            StatGrid.Rows[5].Cells[2].Value = DB.GetReadersRegCountPers().ToString();
            StatGrid.Rows[6].Cells[2].Value = DB.GetReadersRegCountColl().ToString();
            StatGrid.Rows[7].Cells[2].Value = DB.GetReadersReRegCount().ToString();
            StatGrid.Rows[8].Cells[2].Value = DB.GetVisitorsCount().ToString();
            StatGrid.Rows[9].Cells[2].Value = DB.GetIssuedBooksCount().ToString();
            StatGrid.Rows[10].Cells[2].Value = DB.GetIssuedBooksUnder14Count().ToString();
            StatGrid.Rows[11].Cells[2].Value = DB.GetIssuedBooksFrom14To24Count().ToString();
            StatGrid.Rows[12].Cells[2].Value = DB.GetIssuedBooksIndAbCount().ToString();
            StatGrid.Rows[13].Cells[2].Value = DB.GetIssuedBooksPersAbCount().ToString();
            StatGrid.Rows[14].Cells[2].Value = DB.GetIssuedBooksCollAbCount().ToString();
            StatGrid.Rows[15].Cells[2].Value = DB.GetIssuedBooksBSAbCount().ToString();
            StatGrid.Rows[16].Cells[2].Value = DB.GetIssuedBooksElIss().ToString();
            StatGrid.Rows[17].Cells[2].Value = DB.GetIssuedBooksElIssWITH().ToString();
            StatGrid.Rows[18].Cells[2].Value = DB.GetIssuedBooksElIssWITHOUT().ToString();
            StatGrid.Rows[19].Cells[2].Value = DB.GetReaderRecievedBookCount().ToString();
            StatGrid.Rows[20].Cells[2].Value = DB.GetAttendance().ToString();
            StatGrid.Rows[21].Cells[2].Value = DB.GetFreeServiceCount().ToString();
        }
        private void CreateBookSupply()
        {
            StatGrid.Columns.Clear();
            StatGrid.AutoGenerateColumns = false;
            StatGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            StatGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№№";
            col.Width = 50;
            col.Name = "ID";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Наименование раздела";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Name = "Name";
            col.Width = 500;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Количество";
            col.Name = "Count";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Width = 100;
            StatGrid.Columns.Add(col);

            //DataGridViewRow row = new DataGridViewRow();
            StatGrid.Rows.Insert(0, 5);
            StatGrid.Rows[0].Cells[0].Value = "1";
            StatGrid.Rows[0].Cells[1].Value = "Получено и сделано краткое описание";

            StatGrid.Rows[1].Cells[0].Value = "2";
            StatGrid.Rows[1].Cells[1].Value = "Присвоено инвентарных номеров";

            StatGrid.Rows[2].Cells[0].Value = "3";
            StatGrid.Rows[2].Cells[1].Value = "Обработано и передано в фонд (Основной + Абонемент + Продолжающиеся издания)";

            //StatGrid.Rows[3].Cells[0].Value = "4";
            //StatGrid.Rows[3].Cells[1].Value = "Обработано и передано в Основной фонд (продолжающиеся издания)";

            //StatGrid.Rows[4].Cells[0].Value = "5";
            //StatGrid.Rows[4].Cells[1].Value = "Обработано и передано в фонд Абонемент";

            StatGrid.Rows[3].Cells[0].Value = "4";
            StatGrid.Rows[3].Cells[1].Value = "Введено в фонд Редкой книги";

            StatGrid.Rows[4].Cells[0].Value = "5";
            StatGrid.Rows[4].Cells[1].Value = "Число документов, переведённых в электронную форму";

        }
        private void ShowBookSupply()
        {
            StatGrid.Rows[0].Cells[2].Value = DB.GetRecievedAndMadeShortTitle();
            StatGrid.Rows[1].Cells[2].Value = DB.InvAssignedBJVVV();
            StatGrid.Rows[2].Cells[2].Value = DB.ProccessedAndTransferedToMainFund();// +DB.ProccessedAndTransferedToMainFundPeriodic() + DB.ProccessedAndTransferedToAbonement();
            //StatGrid.Rows[3].Cells[2].Value = DB.ProccessedAndTransferedToMainFundPeriodic();
            //StatGrid.Rows[4].Cells[2].Value = DB.ProccessedAndTransferedToAbonement();
            StatGrid.Rows[3].Cells[2].Value = DB.IntroducedToREDKOSTJ().ToString();
            try
            {
                StatGrid.Rows[4].Cells[2].Value = DB.ConvertedIntoDigitalForm().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CreateQoS()
        {
            StatGrid.Columns.Clear();
            StatGrid.AutoGenerateColumns = false;
            StatGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            StatGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№№";
            col.Width = 50;
            col.Name = "ID";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Наименование справки";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Name = "Name";
            col.Width = 500;
            StatGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Количество";
            col.Name = "Count";
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.Width = 100;
            StatGrid.Columns.Add(col);

            //DataGridViewRow row = new DataGridViewRow();
            StatGrid.Rows.Insert(0, 12);
            StatGrid.Rows[0].Cells[0].Value = "1";
            StatGrid.Rows[0].Cells[1].Value = "Среднее время ожидания";

            StatGrid.Rows[1].Cells[0].Value = "2";
            StatGrid.Rows[1].Cells[1].Value = "Общее число зарегистрированных пользователей";
            StatGrid.Rows[2].Cells[0].Value = "";
            StatGrid.Rows[2].Cells[1].Value = "2.1. Пользователь читальных залов БИЛ";
            StatGrid.Rows[3].Cells[0].Value = "";
            StatGrid.Rows[3].Cells[1].Value = "2.2. Пользователь Британского совета";
            StatGrid.Rows[4].Cells[0].Value = "";
            StatGrid.Rows[4].Cells[1].Value = "2.3. Сотрудник БИЛ";
            StatGrid.Rows[5].Cells[0].Value = "";
            StatGrid.Rows[5].Cells[1].Value = "2.4. Индивидуальный абонемент";
            StatGrid.Rows[6].Cells[0].Value = "";
            StatGrid.Rows[6].Cells[1].Value = "2.5. Персональный абонемент";
            StatGrid.Rows[7].Cells[0].Value = "";
            StatGrid.Rows[7].Cells[1].Value = "2.6. Коллективный абонемент";
            StatGrid.Rows[8].Cells[0].Value = "";
            StatGrid.Rows[8].Cells[1].Value = "2.7. До 14 лет";
            StatGrid.Rows[9].Cells[0].Value = "";
            StatGrid.Rows[9].Cells[1].Value = "2.8. От 14 до 24 лет";

            StatGrid.Rows[10].Cells[0].Value = "3";
            StatGrid.Rows[10].Cells[1].Value = "Количество удовлетворенных требований";

            StatGrid.Rows[11].Cells[0].Value = "4";
            StatGrid.Rows[11].Cells[1].Value = "Общее количество требований";

        }
        private void ShowQoSGrid()
        {
            StatGrid.Rows[0].Cells[2].Value = DB.GetAverageTimeOfService().ToString();
            StatGrid.Rows[1].Cells[2].Value = DB.GetReadersRegFullCount().ToString();
            StatGrid.Rows[2].Cells[2].Value = DB.GetReadersRegFullSimple().ToString();
            StatGrid.Rows[3].Cells[2].Value = DB.GetReadersRegFullBS().ToString();
            StatGrid.Rows[4].Cells[2].Value = DB.GetReadersRegFullEmpl().ToString();
            StatGrid.Rows[5].Cells[2].Value = DB.GetReadersRegFullInd().ToString();
            StatGrid.Rows[6].Cells[2].Value = DB.GetReadersRegFullPers().ToString();
            StatGrid.Rows[7].Cells[2].Value = DB.GetReadersRegFullColl().ToString();
            StatGrid.Rows[8].Cells[2].Value = DB.GetReadersRegUnder14().ToString();
            StatGrid.Rows[9].Cells[2].Value = DB.GetReadersRegFrom14To24().ToString();
            StatGrid.Rows[10].Cells[2].Value = DB.GetIssuedBooksCount().ToString();
            StatGrid.Rows[11].Cells[2].Value = DB.GetIssuedBooksCountPlusRefusual().ToString();

        }


        private void StatGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void StatGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (label1.Text.Contains("Справка пополнения фондов за период"))
            {
                return;
            }
            if (label1.Text.Contains("Справка обслуживания за период"))
            {
                Clarify cl;
                switch (StatGrid.Rows[e.RowIndex].Cells[1].Value.ToString())
                {
                    case "Количество посетителей библиотеки":
                        cl = new Clarify(2, DB);
                        cl.ShowDialog();
                        break;
                    case "Количество выданной литературы":
                        cl = new Clarify(3, DB);
                        cl.ShowDialog();
                        break;
                    case "Количество читателей, получивших литературу":
                        cl = new Clarify(4, DB);
                        cl.ShowDialog();
                        break;
                    case "Количество посетителей зала без услуг книговыдачи (открытый доступ, интернет, выставки зала, консультации и т.д.)":
                        cl = new Clarify(5, DB);
                        cl.ShowDialog();
                        break;
                    case "Количество выданных бесплатных справок":
                        cl = new Clarify(6, DB);
                        cl.ShowDialog();
                        break;
                }
            }
            if (label1.Text.Contains("Справка качества услуг за период"))
            {
                Clarify cl;
                switch (e.RowIndex)
                {
                    case 0:
                        cl = new Clarify(0, DB);
                        cl.ShowDialog();
                        break;
                }
            }
        }

        private void StatGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Autoinc();
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowServiceGrid();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RndPrg.Dispose();
            this.Enabled = true;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowBookSupply();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RndPrg.Dispose();
            this.Enabled = true;
        }
        
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            ShowQoSGrid();
            
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RndPrg.Dispose();
            this.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.rtype == RefT.Service)
            {
                DataTable dt = Form1.DGTODTConverter(StatGrid);
                Rep repform = new Rep(dt, label1.Text, 1);
                repform.ShowDialog();
            }
            else
            {
                DataTable dt = Form1.DGTODTConverter(StatGrid);
                Rep repform = new Rep(dt, label1.Text, 2);
                repform.ShowDialog();
            }
        }
        public static DataTable DGTODTConverter(DataGridView dg)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn c in dg.Columns)
            {
                dt.Columns.Add(c.Name);
            }
            foreach (DataGridViewRow r in dg.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < r.Cells.Count; i++)
                {
                    dr[i] = r.Cells[i].Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefType rt = new RefType();
            rt.ShowDialog();
            this.rtype = rt.rt;
            Form1_Load(sender, e);
        }
        void button3_Click(object sender, System.EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
            this.Start = f3.StartDate;
            this.End = f3.EndDate;
            Form1_Load(sender,e);
            
        }
        private void StatGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
    class DBDataGridView : System.Windows.Forms.DataGridView
    {

        public DBDataGridView()
        {
            
            DoubleBuffered = true;
        }

    }
}
