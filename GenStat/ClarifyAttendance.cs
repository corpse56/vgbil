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
    public partial class ClarifyAttendance : Form
    {
        string dep;
        StatDB DB;
        public ClarifyAttendance(string dep_,StatDB DB_)
        {
            InitializeComponent();
            this.dep = dep_;
            this.DB = DB_;
            ShowFreeServicesByUsers();
        }

        private void ShowFreeServicesByUsers()
        {
            label1.Text = "Кол-во посетителей зала без услуг книговыдачи с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
            label2.Text = "в отделе - " + this.dep;
            this.Text = "Кол-во посетителей зала без услуг книговыдачи с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy")
            + " в отделе " + this.dep;
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = DB.GetAttendanceByVarcharDEP(this.dep);
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].HeaderText = "Фамилия";
            ClarGrid.Columns[1].Width = 200;
            ClarGrid.Columns[1].Name = "fam";
            ClarGrid.Columns[2].HeaderText = "Имя";
            ClarGrid.Columns[2].Width = 200;
            ClarGrid.Columns[2].Name = "nam";
            ClarGrid.Columns[3].HeaderText = "Отчество";
            ClarGrid.Columns[3].Width = 200;
            ClarGrid.Columns[3].Name = "fnam";
            ClarGrid.Columns[4].HeaderText = "Номер читателя";
            ClarGrid.Columns[4].Name = "idr";
            ClarGrid.Columns[5].HeaderText = "Дата";
            ClarGrid.Columns[5].Name = "dt";
            ClarGrid.Columns[5].ValueType = typeof(DateTime);
            ClarGrid.Columns[5].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
            Autoinc();
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
            dt = Form1.DGTODTConverter(ClarGrid);
            repform = new Rep(dt, label1.Text + " " + label2.Text,9);
            repform.ShowDialog(); 
        }

    }
}
