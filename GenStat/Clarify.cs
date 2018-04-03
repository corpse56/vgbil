using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenStat
{
    public partial class Clarify : Form
    {
        int spravka = -1;
        StatDB DB;
        private ExtGui.RoundProgress RndPrg;
        int iddep;
        public Clarify(int spravka_,StatDB DB_)
        {
            InitializeComponent();
            this.spravka = spravka_;
            this.DB = DB_;
        }

        private void Clarify_Load(object sender, EventArgs e)
        {
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;

            this.Enabled = false;

            switch (this.spravka)
            {
                case 2:
                    label1.Text = "Количество посетителей библиотеки с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Количество посетителей библиотеки с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    ShowVisitors();
                    backgroundWorker2.RunWorkerAsync();
                    break;
                case 3:

                    ClarGrid.Columns.Clear();
                    ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    label1.Text = "Количество выданной литературы с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Количество выданной литературы с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    backgroundWorker1.RunWorkerAsync();

                    break;
                case 4:
                    label1.Text = "Количество читателей получивших литературу с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Количество читателей получивших литературу с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    backgroundWorker3.RunWorkerAsync();
                    break;
                case 5:
                    label1.Text = "Кол-во посетителей зала без услуг книговыдачи с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Количество посетителей зала без услуг книговыдачи с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    backgroundWorker4.RunWorkerAsync();
                    break;
                case 6:
                    label1.Text = "Количество выданных справок с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Количество выданных справок с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    backgroundWorker5.RunWorkerAsync();
                    break;
                case 0:
                    label1.Text = "Среднее время обслуживания поэтажно с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    this.Text = "Среднее время обслуживания поэтажно с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
                    backgroundWorker6.RunWorkerAsync();
                    break;
            }
        }
        DataTable tmp;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            tmp = DB.GetAllIssuedBook();

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RndPrg.Dispose();
            this.Enabled = true;
            ClarGrid.DataSource = tmp;
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].HeaderText = "Наименование подразделения библиотеки";
            ClarGrid.Columns[1].Width = 400;
            ClarGrid.Columns[1].Name = "dept";
            ClarGrid.Columns[2].HeaderText = "ОФ";
            ClarGrid.Columns[2].Name = "ab";
            ClarGrid.Columns[3].HeaderText = "Фонд отдела (ДП)";
            ClarGrid.Columns[3].Name = "mf";
            ClarGrid.Columns[4].HeaderText = "Фонд редкой книги";
            ClarGrid.Columns[4].Name = "red";
            ClarGrid.Columns[5].HeaderText = "Фонд британского совета";
            ClarGrid.Columns[5].Name = "bs";
            ClarGrid.Columns[6].HeaderText = "Абонемент";
            ClarGrid.Columns[6].Name = "abon";
            Autoinc();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            ClarGrid.Rows[0].Cells[2].Value = DB.GetCenterEntranceCount();
            ClarGrid.Rows[1].Cells[2].Value = DB.GetCenterEntranceOnePassCount();
            ClarGrid.Rows[2].Cells[2].Value = DB.GetCenterEntranceGuestCount();
            ClarGrid.Rows[3].Cells[2].Value = DB.GetCMBCount();
            ClarGrid.Rows[4].Cells[2].Value = DB.GetVisitorsCount();

        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            RndPrg.Dispose();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            tmp = DB.GetAllReadersRecievedBooks();
        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = tmp;
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[1].HeaderText = "Наименование подразделения библиотеки";
            ClarGrid.Columns[1].Width = 400;
            ClarGrid.Columns[2].HeaderText = "Количество читателей";
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].Name = "Name";
            ClarGrid.Columns[2].Name = "Count";
            ClarGrid.Columns[3].Name = "ds";
            ClarGrid.Columns[3].HeaderText = "Количество уникальных читателей";
            Autoinc();
            this.Enabled = true;
            RndPrg.Dispose();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DB.GetAttendanceAllByDep();
            
        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = (DataTable)e.Result;
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[1].HeaderText = "Наименование подразделения библиотеки";
            ClarGrid.Columns[1].Width = 400;
            ClarGrid.Columns[2].HeaderText = "Количество посетителей";
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].Name = "Name";
            ClarGrid.Columns[2].Name = "Count";
            ClarGrid.Columns[3].Name = "ds";
            ClarGrid.Columns[3].HeaderText = "Количество уникальных посетителей";
            Autoinc();
            this.Enabled = true;
            RndPrg.Dispose();

        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DB.GetFreeServiceListByDep();
        }
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = (DataTable)e.Result;
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[0].Name = "ID";

            ClarGrid.Columns[1].HeaderText = "Наименование подразделения библиотеки";
            ClarGrid.Columns[1].Width = 300;
            ClarGrid.Columns[1].Name = "dept";
            ClarGrid.Columns[2].HeaderText = "Адресная справка";
            ClarGrid.Columns[2].Name = "c1";
            ClarGrid.Columns[3].HeaderText = "Методическая консультация";
            ClarGrid.Columns[3].Name = "c2";
            ClarGrid.Columns[4].HeaderText = "Тематическая справка";
            ClarGrid.Columns[4].Name = "c3";
            ClarGrid.Columns[5].HeaderText = "Уточняющая справка";
            ClarGrid.Columns[5].Name = "c4";
            ClarGrid.Columns[6].HeaderText = "Фактографическая справка";
            ClarGrid.Columns[6].Name = "c5";
            ClarGrid.Columns[7].HeaderText = "Удалённая справка";
            ClarGrid.Columns[7].Name = "c6";
            Autoinc();
            this.Enabled = true;
            RndPrg.Dispose();
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = DB.GetAVGTimeByFloor();
        }
        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = (DataTable)e.Result;
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[1].HeaderText = "Этаж";
            ClarGrid.Columns[1].Width = 400;
            ClarGrid.Columns[2].HeaderText = "Среднее время обслуживания";
            ClarGrid.Columns[2].Width = 200;
            ClarGrid.Columns[3].HeaderText = "Кол-во требований c 11:00 по 20:30";
            ClarGrid.Columns[3].Width = 150;
            ClarGrid.Columns[4].HeaderText = "Кол-во требований c 20:31 по 10:59";
            ClarGrid.Columns[4].Width = 150;
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].Name = "Name";
            ClarGrid.Columns[2].Name = "Count";
            ClarGrid.Columns[3].Name = "ds";
            ClarGrid.Columns[4].Name = "ds1";
            Autoinc();
            RndPrg.Dispose();
            this.Enabled = true;
        }




        private void ShowVisitors()
        {
            ClarGrid.Columns.Clear();
            ClarGrid.AutoGenerateColumns = false;
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "№№";
            col.Width = 50;
            ClarGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Наименование пункта посещения";
            col.Width = 500;
            ClarGrid.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "Количество";
            col.Width = 100;
            ClarGrid.Columns.Add(col);
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].Name = "Name";
            ClarGrid.Columns[2].Name = "Count";

            ClarGrid.Rows.Insert(0, 5);
            ClarGrid.Rows[0].Cells[0].Value = "1";
            ClarGrid.Rows[0].Cells[1].Value = "Центральный вход - читатель";
            //ClarGrid.Rows[0].Cells[2].Value = DB.GetCenterEntranceCount();

            //ClarGrid.Rows[1].Cells[0].Value = "2";
            //ClarGrid.Rows[1].Cells[1].Value = "Центральный вход - ФКЦ";
            //ClarGrid.Rows[1].Cells[2].Value = DB.GetCenterEntranceFKCCount();

            ClarGrid.Rows[1].Cells[0].Value = "2";
            ClarGrid.Rows[1].Cells[1].Value = "Центральный вход - Разовый билет";
            //ClarGrid.Rows[1].Cells[2].Value = DB.GetCenterEntranceOnePassCount();

            ClarGrid.Rows[2].Cells[0].Value = "3";
            ClarGrid.Rows[2].Cells[1].Value = "Центральный вход - Гость";
            //ClarGrid.Rows[2].Cells[2].Value = DB.GetCenterEntranceGuestCount();

            //ClarGrid.Rows[4].Cells[0].Value = "5";
            //ClarGrid.Rows[4].Cells[1].Value = "Отдел Абонемент";
            //ClarGrid.Rows[4].Cells[2].Value = DB.GetAbonementCount();

            ClarGrid.Rows[3].Cells[0].Value = "4";
            ClarGrid.Rows[3].Cells[1].Value = "МКЦ. Группа международного библиотековедения";
            //ClarGrid.Rows[3].Cells[2].Value = DB.GetCMBCount();

            ClarGrid.Rows[4].Cells[0].Value = "";
            ClarGrid.Rows[4].Cells[1].Value = "Итого:";
            ClarGrid.Rows[4].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //ClarGrid.Rows[4].Cells[2].Value = DB.GetVisitorsCount();

        }

        private void Autoinc()
        {
            int i = 0;
            foreach (DataGridViewRow row in ClarGrid.Rows)
            {
                row.Cells[0].Value = (++i).ToString();
            }
        }

        private void ClarGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Autoinc();
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            Rep repform;
            DataTable dt;
            switch (this.spravka)
            {
                case 0:
                    //ShowAVGTIME();
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text, 0);
                    repform.ShowDialog();
                    break;
                case 2:
                    //ShowVisitors();
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text,3);
                    repform.ShowDialog(); 
                    break;
                case 3:
//                    ShowIssued();
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text,6);
                    repform.ShowDialog();
                    break;
                case 4:
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text,5);
                    repform.ShowDialog(); 

  //                  ShowReadersRecievedBooks();
                    break;
                case 5:
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text,4);
                    repform.ShowDialog(); 
                    //ShowAttendance();
                    break;
                case 6:
                    //ShowFreeService();
                    dt = Form1.DGTODTConverter(ClarGrid);
                    repform = new Rep(dt, label1.Text, 7);
                    repform.ShowDialog();
                    break;
            }
        }

        private void ClarGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (label1.Text.Contains("Количество выданных справок"))
            {
                ClarifyServices cs = new ClarifyServices(ClarGrid.SelectedRows[0].Cells[1].Value.ToString(), this.DB);
                cs.ShowDialog();
            }
            if (label1.Text.Contains("Кол-во посетителей зала без услуг книговыдачи"))
            {
                ClarifyAttendance cs = new ClarifyAttendance(ClarGrid.SelectedRows[0].Cells[1].Value.ToString(), this.DB);
                //ClarifyAttendance cs = new ClarifyAttendance(ClarGrid.SelectedRows[0].Cells[1].Value.ToString(), this.DB, this.iddep);
                cs.ShowDialog();
            }
            if (label1.Text.Contains("Количество читателей получивших литературу "))
            {
                ClarifyReaders cs = new ClarifyReaders(ClarGrid.SelectedRows[0].Cells[1].Value.ToString(), this.DB);
                cs.ShowDialog();
            }
        }










    }
}
