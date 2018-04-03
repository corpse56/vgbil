using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BookkeepingForOrder
{
    public partial class Form3 : Form
    {
        Form1 F1;
        DataTable ReadersTable;
        DbForEmployee db;
        string Floor;
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(Form1 f1_,DbForEmployee db_,string Floor_)
        {
            InitializeComponent();
            F1 = f1_;
            db = db_;
            Floor = Floor_;
        }
        public void InitForm()
        {
            ReadersTable = F1.db.GetReadersChoosing(F1.ForSQL);
            dgw.Columns.Clear();
            dgw.AutoGenerateColumns = false;
            dgw.DataSource = ReadersTable;

            dgw.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgw.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //dgwEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgw.Columns.Add("NN", "ПИН");
            dgw.Columns.Add("NN1", "Автор");
            dgw.Columns.Add("NN2", "Заглавие");
            dgw.Columns.Add("NN3", "Инв. номер");
            dgw.Columns.Add("NN4", "Расст. шифр");
            dgw.Columns.Add("NN5", "Дата издания");
            dgw.Columns.Add("NN6", "id берущего отдела");
            dgw.Columns.Add("NN7", "id заказа");
            dgw.Columns.Add("NN8", "от кого");
            dgw.Columns.Add("NN9", "fio");
            dgw.Columns.Add("NN10", "gizd");
            dgw.Columns.Add("startd", "startd");
            dgw.Columns.Add("note", "Прим. инв. н.");
            dgw.Columns.Add("yaz", "yaz");

            dgw.ReadOnly = true;

            dgw.Columns[0].HeaderText = "ПИН";
            dgw.Columns[0].Width = 74;
            dgw.Columns[0].DataPropertyName = "idm";
            dgw.Columns[0].Name = "idm";
            dgw.Columns[1].HeaderText = "Автор";
            dgw.Columns[1].Width = 125;
            dgw.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgw.Columns[1].DataPropertyName = "avt";
            dgw.Columns[1].Name = "avt";
            dgw.Columns[2].HeaderText = "Заглавие";
            dgw.Columns[2].Width = 265;
            dgw.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgw.Columns[2].DataPropertyName = "zag";
            dgw.Columns[2].Name = "zag";
            dgw.Columns[3].HeaderText = "Инвентар ный номер";
            dgw.Columns[3].Width = 80;
            dgw.Columns[3].Name = "inv";
            dgw.Columns[3].DataPropertyName = "inv";
            //string d = ((DataTable)dgw.DataSource).Rows[0][7].ToString();


            dgw.Columns[4].HeaderText = "Расст. шифр";
            dgw.Columns[4].Width = 100;
            dgw.Columns[4].Name = "shifr";
            dgw.Columns[4].DataPropertyName = "shifr";
            dgw.Columns[5].Visible = false;
            dgw.Columns[5].Name = "izd";
            dgw.Columns[5].DataPropertyName = "izd";
            dgw.Columns[6].Visible = false;
            dgw.Columns[6].Name = "idr";
            dgw.Columns[6].DataPropertyName = "idr";
            dgw.Columns[7].Visible = false;
            dgw.Columns[7].Name = "oid";
            dgw.Columns[7].DataPropertyName = "oid";
            dgw.Columns[8].HeaderText = "От кого";
            dgw.Columns[8].Width = 130;
            dgw.Columns[8].Name = "dp";
            //dgw.Columns[8].CellTemplate.Style.WrapMode = DataGridViewTriState.True;
            dgw.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgw.Columns[8].DataPropertyName = "dp";
            dgw.Columns[9].Visible = false;
            dgw.Columns[9].Name = "fio";
            dgw.Columns[9].DataPropertyName = "fio";
            dgw.Columns[10].Visible = false;
            dgw.Columns[10].Name = "gizd";
            dgw.Columns[10].DataPropertyName = "gizd";
            dgw.Columns[11].ValueType = typeof(DateTime);
            dgw.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgw.Columns[11].HeaderText = "Дата заказа";
            dgw.Columns[11].Width = 80;
            dgw.Columns[11].DataPropertyName = "startd";
            dgw.Columns["note"].Name = "note";
            dgw.Columns["note"].DataPropertyName = "note";
            dgw.Columns["yaz"].Visible = false;

            dgw.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            if (dgw.Rows.Count == 0)
            {
                MessageBox.Show("Подбираемых заказов нет!(Заказы со статусом \"Сотрудник книгохранения обрабатывает заказ\" отсутствуют!)");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgw.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            Refusal rf = new Refusal(dgw.SelectedRows[0].Cells["oid"].Value.ToString());
            rf.ShowDialog();
            if (rf.Cause == "")
                return;
            F1.db.RefusualReader(rf.Cause, dgw.SelectedRows[0].Cells["oid"].Value.ToString());
            this.InitForm();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgw.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgw, this.Floor, F1); //когда принтер заработаетвключить это
            pb.Print();
        }
    }
}
