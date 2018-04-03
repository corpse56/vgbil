using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace Circulation
{
    public partial class Form25 : Form
    {
        DBWork dbw;
        DataTable table,filtertable;
        Form1 f1;
        public void construct()
        {
            ResGrid.Columns.Clear();
            ResGrid.AllowUserToAddRows = false;
            ResGrid.AutoGenerateColumns = true;
            ResGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            try
            {
                table = dbw.GetResIss();
                ResGrid.DataSource = table;
                filtertable = dbw.GetResIss();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResGrid.Columns.Clear();
                return;
            }
            autoinc(ResGrid);
            ResGrid.Columns[0].HeaderText = "№№";
            ResGrid.Columns[0].Width = 30;
            ResGrid.Columns[1].HeaderText = "Заглавие";
            ResGrid.Columns[1].Name = "title";
            ResGrid.Columns[1].Width = 250;
            ResGrid.Columns[2].HeaderText = "Автор";
            ResGrid.Columns[3].Width = 120;
            ResGrid.Columns[3].HeaderText = "Инв. Номер";
            ResGrid.Columns[3].Width = 80;
            ResGrid.Columns[4].HeaderText = "Выдано в зале";
            ResGrid.Columns[4].Width = 100;
            ResGrid.Columns[5].HeaderText = "Возвращено в зале";
            ResGrid.Columns[5].Width = 100;
            ResGrid.Columns[6].HeaderText = "Номер бравшего читателя";
            ResGrid.Columns[6].Width = 80;
            ResGrid.Columns[7].HeaderText = "Номер бронеполки";
            ResGrid.Columns[7].Width = 80;
            ResGrid.Columns[8].HeaderText = "Дата окончания брони";
            ResGrid.Columns[8].Width = 90;
            ResGrid.Columns[9].HeaderText = "Статус";
            ResGrid.Columns[9].Width = 70;
            ResGrid.Columns[10].Visible = false;
            ResGrid.Columns[11].Visible = false;
            ResGrid.Columns[12].Visible = false;
            textBox1.Text = "";
        }

        public Form25(DBWork DBW, Form1 _f1)
        {
            InitializeComponent();
            this.dbw = DBW;
            //this.ps = ps_;
            construct();
            this.f1 = _f1;
        }
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = (++i).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ResGrid.SelectedRows[0].Cells[9].Value.ToString() != "На бронеполке")
            {
                MessageBox.Show("Читатель сначала должен сдать книгу!");
                return;
            }
            dbBook b = new dbBook(ResGrid.SelectedRows[0].Cells[11].Value.ToString(), dbw.F1.BASENAME);
            dbw.MoveToHistory(b);
            MessageBox.Show("Запись о выдаче книги отправлена в историю! Книга может быть сдана в книгохранение или выдана другому читателю!");
            ResGrid.Rows.Remove(ResGrid.SelectedRows[0]);
            //this.construct();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                ResGrid.DataSource = table;
                return;
            }
            string f = table.Rows[0]["title"].ToString();
            bool t = f.StartsWith(textBox1.Text);

            var query = from DataRow x in table.AsEnumerable()
                        where x["title"].ToString().ToLower().Contains(textBox1.Text.ToLower())
                        //orderby x["title"]
                        select x;
            
            filtertable.Rows.Clear();
            query.CopyToDataTable(filtertable, LoadOption.OverwriteChanges);
            ResGrid.DataSource = filtertable;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //PrintDemand demand = new PrintDemand("U100121630", this.f1);
            //demand.ShowPreview();
            switch (f1.DepName)
            {
                case "…Зал… КОО Группа выдачи документов":
                    {
                        //pd.PrinterSettings.PrinterName = "Zebra  TLP2844";
                        //pd.PrinterSettings.PrinterName = "Zebra TLP2844 CSI";

                        break;
                    }
                default:
                    {
                        MessageBox.Show("У вас не установлен принтер Zebra!");
                        return;
                    }
            }

            f1.f31 = new Form31();
            f1.f31.ShowDialog();
            f1.f31 = null;

            /*if (ResGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрано ни одной строки!");
                return;
            }*/

        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

        }


        private void ResGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            autoinc(ResGrid);
        }


    }
}
