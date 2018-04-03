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
    public partial class ClarifyServices : Form
    {
        string dep;
        StatDB DB;
        public ClarifyServices(string dep_,StatDB DB_)
        {
            InitializeComponent();
            this.dep = dep_;
            this.DB = DB_;
            ShowFreeServicesByUsers();
        }

        private void ShowFreeServicesByUsers()
        {
            label1.Text = "Кол-во выданных справок с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy");
            label2.Text = "в отделе - " + this.dep;
            this.Text = "Количество выданных справок с " + DB.Start.Date.ToString("dd.MM.yyyy") + " по " + DB.End.Date.ToString("dd.MM.yyyy") 
                +" в отделе "+this.dep;
            ClarGrid.Columns.Clear();
            ClarGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ClarGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            ClarGrid.DataSource = DB.GetFreeServicesByUsers(this.dep);
            ClarGrid.Columns[0].HeaderText = "№№";
            ClarGrid.Columns[0].Width = 40;
            ClarGrid.Columns[0].Name = "ID";
            ClarGrid.Columns[1].HeaderText = "Оператор";
            ClarGrid.Columns[1].Width = 350;
            ClarGrid.Columns[1].Name = "emp";
            ClarGrid.Columns[2].HeaderText = "Справка";
            ClarGrid.Columns[2].Width = 300;
            ClarGrid.Columns[2].Name = "spravka";
            ClarGrid.Columns[3].HeaderText = "Количество";
            ClarGrid.Columns[3].Name = "cnt";
            ClarGrid.Columns[4].HeaderText = "Дата";
            ClarGrid.Columns[4].Name = "dt";
            ClarGrid.Columns[4].ValueType = typeof(DateTime);
            ClarGrid.Columns[4].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
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
            repform = new Rep(dt, label1.Text + " " + label2.Text,8);
            repform.ShowDialog(); 
        }

    }
}
