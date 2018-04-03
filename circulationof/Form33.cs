using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public partial class Form33 : Form
    {
        string inv,invs,bar,selectedid;
        public bool CanShow = false;
        public Form33(string inv_,DBWork db)
        {
            InitializeComponent();
            DataSet DS = db.CheckForTwo(inv_);
            if (DS.Tables["t"].Rows.Count > 1)
            {
                SpecifyExampler(DS);
                
            }
            if (DS.Tables["t"].Rows.Count == 0)
            {
                CanShow = false;
                return;
            }
            if (DS.Tables["t"].Rows.Count == 1)
            {
                this.selectedid = DS.Tables["t"].Rows[0][4].ToString();
                this.bar = DS.Tables["t"].Rows[0]["bar"].ToString();
            }
            inv = inv_;

            this.db = db;
            Construct();
        }

        private void SpecifyExampler(DataSet DS)
        {
            Form34 f34 = new Form34(DS);
            f34.ShowDialog();
            this.selectedid = f34.selectedid;
            this.bar = f34.bar;
        }
        private DBWork db;

        private void Construct()
        {
            //db = new DBWork();
            ConstructBook();
            ConstuctCurr();
            ConstructHist();
        }
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = ++i;
            }
        }
        private void ConstructHist()
        {
            if (this.invs == "")
            {
                //MessageBox.Show("Информация не может быть выдана, так как книга не в базе!");
                this.CanShow = false;
                return;
            }
            else
            {
                History.DataSource = db.GetHistOfINV(this.bar);
            }
            History.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            History.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            History.Columns[0].HeaderText = "№№";
            History.Columns[0].Visible = false;
            History.Columns[0].Width = 45;
            History.Columns[1].HeaderText = "Дата возврата от читателя";
            History.Columns[1].Width = 120;
            History.Columns[2].HeaderText = "Дата сдачи в к/х";
            History.Columns[2].Width = 120;
            History.Columns[3].HeaderText = "№ читателя";
            History.Columns[3].Width = 100;
            History.Columns[4].HeaderText = "Сотрудник";
            History.Columns[4].Width = 150;
            History.Columns[5].HeaderText = "Отдел выдачи";
            History.Columns[5].Width = 200;
            History.Columns[6].HeaderText = "Этаж приема изданий";
            History.Columns[6].Width = 150;
            autoinc(History);
            History.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            if ((((DataTable)History.DataSource).Rows.Count == 0) 
                && (Current.Rows[0].Cells[2].Value.ToString() == "нет")
                && (Current.Rows[1].Cells[2].Value.ToString() == "нет")
                && (Current.Rows[2].Cells[2].Value.ToString() == "нет"))
            {
                Current.Rows[3].Cells[2].Value = "да";
                DataSet DS =  db.GetCurrentPlace(this.invs);
                if (DS.Tables["t"].Rows.Count != 0)
                {
                    //Current.Rows[3].Cells[3].Value = ((DateTime)DS.Tables["t"].Rows[0][2]).ToString("dd.MM.yyyy");
                    //Current.Rows[3].Cells[5].Value = DS.Tables["t"].Rows[0][1].ToString();
                    Current.Rows[3].Cells[6].Value = DS.Tables["t"].Rows[0][0].ToString();
                }
                else
                {
                    Current.Rows[3].Cells[6].Value = Book.Rows[0].Cells[5].Value.ToString();
                }
            }
        }

        private void ConstuctCurr()
        {
            Current.Columns.Clear();
            Current.DataSource = null;
            Current.AutoGenerateColumns = false;
            Current.Columns.Add("NN", "№№");
            Current.Columns.Add("act", "Действие");
            Current.Columns.Add("res", "Результат");
            Current.Columns.Add("date", "Дата");
            Current.Columns.Add("idr", "№ читателя");
            Current.Columns.Add("emp", "Сотрудник");
            Current.Columns.Add("emp", "Отдел местона хождения");
            Current.Columns[0].Width = 70;
            Current.Columns[1].Width = 300;
            Current.Columns[5].Width = 150;
            Current.Columns[6].Width = 150;
            DataTable t = db.GetCurrentIssueByINV(invs);
            Current.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Current.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Current.Rows.Add();
            Current.Rows[0].Cells[0].Value = "1";
            Current.Rows[0].Cells[1].Value = "Книга выдана читателю";
            Current.Rows[0].Cells[2].Value = "нет";
            Current.Rows[0].Cells[3].Value = "-";
            Current.Rows[0].Cells[4].Value = "-";
            Current.Rows[0].Cells[5].Value = "-";
            Current.Rows[0].Cells[6].Value = "-";
            Current.Rows.Add();
            Current.Rows[1].Cells[0].Value = "2";
            Current.Rows[1].Cells[1].Value = "Книга возвращена на бронеполку";
            Current.Rows[1].Cells[2].Value = "нет";
            Current.Rows[1].Cells[3].Value = "-";
            Current.Rows[1].Cells[4].Value = "-";
            Current.Rows[1].Cells[5].Value = "-";
            Current.Rows[1].Cells[6].Value = "-";
            Current.Rows.Add();
            Current.Rows[2].Cells[0].Value = "3";
            Current.Rows[2].Cells[1].Value = "Книга подготовлена для сдачи в к/х";
            Current.Rows[2].Cells[2].Value = "нет";
            Current.Rows[2].Cells[3].Value = "-";
            Current.Rows[2].Cells[4].Value = "-";
            Current.Rows[2].Cells[5].Value = "-";
            Current.Rows[2].Cells[6].Value = "-";
            Current.Rows.Add();
            Current.Rows[3].Cells[0].Value = "4";
            Current.Rows[3].Cells[1].Value = "Книга ни разу не выдавалась";
            Current.Rows[3].Cells[2].Value = "нет";
            Current.Rows[3].Cells[3].Value = "-";
            Current.Rows[3].Cells[4].Value = "-";
            Current.Rows[3].Cells[5].Value = "-";
            Current.Rows[3].Cells[6].Value = "-";
            
            if (t.Rows.Count != 0)//бронеполка или выдана
            {
                if (t.Rows[0]["IDMAIN"].ToString() == "0")// бронеполка
                {
                    Current.Rows[1].Cells[2].Value = "Да";
                    Current.Rows[1].Cells[3].Value = ((DateTime)t.Rows[0]["DATE_ISSUE"]).ToString("dd.MM.yyyy");
                    Current.Rows[1].Cells[4].Value = t.Rows[0]["RESPAN"].ToString();
                    Current.Rows[1].Cells[5].Value = t.Rows[0]["NAME"].ToString();
                    Current.Rows[1].Cells[6].Value = t.Rows[0]["ZALRET"].ToString();
                }
                else//выдана
                {
                    Current.Rows[0].Cells[2].Value = "Да";
                    Current.Rows[0].Cells[3].Value = ((DateTime)t.Rows[0]["DATE_ISSUE"]).ToString("dd.MM.yyyy");
                    Current.Rows[0].Cells[4].Value = t.Rows[0]["IDREADER"].ToString();
                    Current.Rows[0].Cells[5].Value = t.Rows[0]["NAME"].ToString();
                    Current.Rows[0].Cells[6].Value = t.Rows[0]["ZALISS"].ToString();
                }
            }
            else
            {
                if (invs == "")
                {

                }
                else
                {
                    t = db.GetPREPBKByINV(invs);

                    if (t.Rows.Count != 0)//подготовлена к сдаче
                    {
                        Current.Rows[2].Cells[2].Value = "Да";
                        Current.Rows[2].Cells[3].Value = ((DateTime)t.Rows[0]["DATEV"]).ToString("dd.MM.yyyy");
                        Current.Rows[2].Cells[4].Value = t.Rows[0]["RESPAN"].ToString();
                        Current.Rows[2].Cells[5].Value = t.Rows[0]["NAME"].ToString();
                        Current.Rows[2].Cells[6].Value = t.Rows[0]["DEPNAME"].ToString();
                    }
                    else//книга либо никогда не выдавалась либо возвращена в кх
                    {
                    }
                }
            }

        }
        private void ConstructBook()
        {
            Book.DataSource = db.GetCurrentStatus(this.inv,this.selectedid);
            if (Book.Rows.Count == 0)
            {
                CanShow = false;
                return;
            }
            CanShow = true;
            Book.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            Book.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Book.Columns[0].HeaderText = "№№";
            Book.Columns[0].Visible = false;
            Book.Columns[0].Width -= 15;
            Book.Columns[1].HeaderText = "Заглавие";
            Book.Columns[1].Width = 300;
            Book.Columns[2].HeaderText = "Автор";
            Book.Columns[3].Width = 150;
            Book.Columns[3].HeaderText = "Инв номер";
            Book.Columns[3].Width = 100;
            Book.Columns[4].HeaderText = "Штрихкод";
            Book.Columns[4].Width = 120;
            Book.Columns[5].HeaderText = "Место хранения";
            Book.Columns[5].Width = 200;
            Book.Columns[6].HeaderText = "Расстано вочный шифр";
            Book.Columns[6].Width = 140;
            Book.Columns[7].Visible = false;
            //Book.Columns[8].Visible = false;
            this.invs = Book.Rows[0].Cells["invs"].Value.ToString();
        }

        private void Current_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            autoinc(Current);
        }

        private void History_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            autoinc(History);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
