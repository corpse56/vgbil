using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Threading;
using System.Runtime.InteropServices;
using LibflClassLibrary.Controls;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Readers;

namespace BookkeepingForOrder
{
    public partial class Form1 : Form
    {
        authorization auth;
        public string EmpID;
        public string FIO;
        public string Floor;
        public string FloorID;
        public string ForSQL;
        public string BASE;
        public string OrderTableType;
        public SqlDataAdapter SqlDA;
        public SqlConnection SqlCon;
        private ExtGui.RoundProgress RndPrg;
        public DbForEmployee db;
        DataTable MainTable;
        DataTable HisTable;
        DataTable ReadersTable;
        DataTable ReadersHisTable;

        BJUserInfo user;


        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


        public Form1()
        {
            SqlCon = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDA = new SqlDataAdapter();
            SqlDA.SelectCommand = new SqlCommand();
            SqlDA.SelectCommand.Connection = SqlCon;
            //auth = new authorization(this);
            //auth.ShowDialog();  //потом откоментировать обратно
            //if (auth.DialogResult != DialogResult.Cancel)
            //{
            //    db = new DbForEmployee(this.OrderTableType,this.BASE,this);
            //}


            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            fBJAuthorization fAuth = new fBJAuthorization();
            fAuth.ShowDialog();
            if (fAuth.DialogResult == DialogResult.Cancel || fAuth.User == null)
            {
                Close();
                return;
            }
            user = fAuth.User;

            //if ((this.EmpID == "") || (this.EmpID == null) || (this.Floor == "") || (this.Floor == null))
            //{
            //    MessageBox.Show("Вы не авторизованы! Программа заканчивает свою работу", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Close();
            //    return;
            //}
            //label1.Text = this.Floor + "  : " + this.FIO;
            label1.Text = $"{user.SelectedUserStatus.DepName} {user.FIO}";

        }

        private void ShowReaderOrders()
        {
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "pin", "ПИН"),
                new KeyValuePair<string, string> ( "author", "Автор"),
                new KeyValuePair<string, string> ( "title", "Заглавие"),
                new KeyValuePair<string, string> ( "inv", "Инв. номер"),
                new KeyValuePair<string, string> ( "cipher", "Расст. шифр"),
                new KeyValuePair<string, string> ( "pubdate", "Дата издания"),
                new KeyValuePair<string, string> ( "readerid", "Номер читателя"),
                new KeyValuePair<string, string> ( "fio", "ФИО читателя"),
                new KeyValuePair<string, string> ( "startdate", "Дата формирования заказа"),
                new KeyValuePair<string, string> ( "orderid", "orderid"),
                new KeyValuePair<string, string> ( "status", "Статус заказа"),
                new KeyValuePair<string, string> ( "note", "Инв. метка")
                new KeyValuePair<string, string> ( "pubdate", "pubdate")
            };
            foreach (var c in columns)
                dgwReaders.Columns.Add(c.Key, c.Value);

            dgwReaders.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgwReaders.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgwReaders.Columns["startdate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            dgwReaders.Columns["pin"].Width = 74;
            dgwReaders.Columns["author"].Width = 125;
            dgwReaders.Columns["title"].Width = 265;
            dgwReaders.Columns["inv"].Width = 80;
            dgwReaders.Columns["cipher"].Width = 100;
            dgwReaders.Columns["readerid"].Width = 80;
            dgwReaders.Columns["fio"].Width = 120;
            dgwReaders.Columns["pubdate"].Visible = false;
            dgwReaders.Columns["startdate"].Width = 80;
            dgwReaders.Columns["orderid"].Visible = false;
            dgwReaders.Columns["status"].Width = 100;
            dgwReaders.Columns["note"].Width = 60;
            dgwReaders.Columns["pubdate"].Visible = false;

            CirculationInfo circulation = new CirculationInfo();
            List<OrderInfo> orders = circulation.GetOrdersForStorage(user.SelectedUserStatus.DepId, user.SelectedUserStatus.DepName);
            foreach(var order in orders)
            {
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                dgwReaders.Rows.Add();
                var row = dgwReaders.Rows[dgwReaders.Rows.Count - 1];

                row.Cells["startdate"].Value = order.StartDate;
                row.Cells["pin"].Value = order.BookId;
                row.Cells["author"].Value = order.Book.Author;
                row.Cells["title"].Value = order.Book.Title;
                row.Cells["inv"].Value = exemplar.Fields["899$p"].ToString();
                row.Cells["cipher"].Value = exemplar.Fields["899$j"].ToString();
                row.Cells["readerid"].Value = order.ReaderId;
                row.Cells["fio"].Value = order.ReaderId;
                row.Cells["orderid"].Value = order.OrderId;
                row.Cells["status"].Value = order.StatusName;
                row.Cells["note"].Value = exemplar.Fields["899$x"].ToString();
                row.Cells["pubdate"].Value = order.Book.PublishDate;
            }
        }
        //EKATERINA.A.LISOVSKAYA katya - 3 этаж

        private void FormMainTable()
        {
            //MainTable = db.GetTable(this.ForSQL);
        }
        private void FormHisTable()
        {
            HisTable = db.GetHistory(this.ForSQL);
        }
        private void FormReadersTable()
        {
            ReadersTable = db.GetReaders(this.ForSQL);
        }
        private void FormReadersHisTable()
        {
            ReadersHisTable = db.GetReadersHistory(this.ForSQL);
        }
        private void FormReaderTable_Interface()
        {
            dgwReaders.Columns.Clear();
            dgwReaders.AutoGenerateColumns = false;
            dgwReaders.DataSource = ReadersTable;

            dgwReaders.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgwReaders.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //dgwEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwReaders.Columns.Add("NN", "ПИН");
            dgwReaders.Columns.Add("NN1", "Автор");
            dgwReaders.Columns.Add("NN2", "Заглавие");
            dgwReaders.Columns.Add("NN3", "Инв. номер");
            dgwReaders.Columns.Add("NN4", "Расст. шифр");
            dgwReaders.Columns.Add("NN5", "Дата издания");
            dgwReaders.Columns.Add("NN6", "id берущего отдела");
            dgwReaders.Columns.Add("NN7", "id заказа");
            dgwReaders.Columns.Add("NN8", "от кого");
            dgwReaders.Columns.Add("NN9", "fio");
            dgwReaders.Columns.Add("NN10", "gizd");
            dgwReaders.Columns.Add("startd", "startd");
            dgwReaders.Columns.Add("note", "Инв. метка");
            dgwReaders.Columns.Add("yaz", "yaz");
            dgwReaders.Columns["yaz"].Name = "yaz";
            dgwReaders.Columns["yaz"].Visible = false;
            dgwReaders.Columns["yaz"].DataPropertyName = "yaz";

            dgwReaders.ReadOnly = true;

            dgwReaders.Columns[0].HeaderText = "ПИН";
            dgwReaders.Columns[0].Width = 74;
            dgwReaders.Columns[0].DataPropertyName = "idm";
            dgwReaders.Columns[0].Name = "idm";
            dgwReaders.Columns[1].HeaderText = "Автор";
            dgwReaders.Columns[1].Width = 125;
            dgwReaders.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwReaders.Columns[1].DataPropertyName = "avt";
            dgwReaders.Columns[1].Name = "avt";
            dgwReaders.Columns[2].HeaderText = "Заглавие";
            dgwReaders.Columns[2].Width = 265;
            dgwReaders.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwReaders.Columns[2].DataPropertyName = "zag";
            dgwReaders.Columns[2].Name = "zag";
            dgwReaders.Columns[3].HeaderText = "Инвентар ный номер";
            dgwReaders.Columns[3].Width = 80;
            dgwReaders.Columns[3].Name = "inv";
            dgwReaders.Columns[3].DataPropertyName = "inv";
            //string d = ((DataTable)dgwReaders.DataSource).Rows[0][7].ToString();


            dgwReaders.Columns[4].HeaderText = "Расст. шифр";
            dgwReaders.Columns[4].Width = 100;
            dgwReaders.Columns[4].Name = "shifr";
            dgwReaders.Columns[4].DataPropertyName = "shifr";
            dgwReaders.Columns[5].Visible = false;
            dgwReaders.Columns[5].Name = "izd";
            dgwReaders.Columns[5].DataPropertyName = "izd";
            dgwReaders.Columns[6].Visible = false;
            dgwReaders.Columns[6].Name = "idr";
            dgwReaders.Columns[6].DataPropertyName = "idr";
            dgwReaders.Columns[7].Visible = false;
            dgwReaders.Columns[7].Name = "oid";
            dgwReaders.Columns[7].DataPropertyName = "oid";
            dgwReaders.Columns[8].HeaderText = "От кого";
            dgwReaders.Columns[8].Width = 130;
            dgwReaders.Columns[8].Name = "dp";
            //dgwReaders.Columns[8].CellTemplate.Style.WrapMode = DataGridViewTriState.True;
            dgwReaders.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwReaders.Columns[8].DataPropertyName = "dp";
            dgwReaders.Columns[9].Visible = false;
            dgwReaders.Columns[9].Name = "fio";
            dgwReaders.Columns[9].DataPropertyName = "fio";
            dgwReaders.Columns[10].Visible = false;
            dgwReaders.Columns[10].Name = "gizd";
            dgwReaders.Columns[10].DataPropertyName = "gizd";
            dgwReaders.Columns[11].ValueType = typeof(DateTime);
            dgwReaders.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgwReaders.Columns[11].HeaderText = "Дата заказа";
            dgwReaders.Columns[11].Width = 80;
            dgwReaders.Columns[11].DataPropertyName = "startd";
            dgwReaders.Columns["note"].Name = "note";
            dgwReaders.Columns["note"].DataPropertyName = "note";

            dgwReaders.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //dgwReaders.Columns[5].Visible = false;
            //dgwEmp.Columns[6].Visible = false;
        }
        private void FormReaderHisTable_Interface()
        {
            dgwRHis.Columns.Clear();
            dgwRHis.AutoGenerateColumns = false;
            dgwRHis.DataSource = ReadersHisTable;

            dgwRHis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgwRHis.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //dgwEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwRHis.Columns.Add("NN", "ПИН");
            dgwRHis.Columns.Add("NN1", "Автор");
            dgwRHis.Columns.Add("NN2", "Заглавие");
            dgwRHis.Columns.Add("NN3", "Инв. номер");
            dgwRHis.Columns.Add("NN4", "Расст. шифр");
            dgwRHis.Columns.Add("NN5", "Дата издания");
            dgwRHis.Columns.Add("NN6", "id берущего отдела");
            dgwRHis.Columns.Add("NN7", "id заказа");
            dgwRHis.Columns.Add("NN8", "от кого");
            dgwRHis.Columns.Add("NN9", "fio");
            dgwRHis.Columns.Add("NN10", "gizd");
            dgwRHis.Columns.Add("startd", "startd");
            dgwRHis.Columns.Add("NN12", "refusual");
            dgwRHis.Columns.Add("NN13", "sts");
            dgwRHis.Columns.Add("note", "Инв. метка");
            dgwRHis.Columns.Add("yaz", "yaz");
            dgwRHis.Columns["yaz"].Name = "yaz";
            dgwRHis.Columns["yaz"].Visible = false;
            dgwRHis.Columns["yaz"].DataPropertyName = "yaz";

            dgwRHis.ReadOnly = true;

            dgwRHis.Columns[0].HeaderText = "ПИН";
            dgwRHis.Columns[0].Width = 74;
            dgwRHis.Columns[0].DataPropertyName = "idm";
            dgwRHis.Columns[0].Name = "idm";
            dgwRHis.Columns[1].HeaderText = "Автор";
            dgwRHis.Columns[1].Width = 125;
            dgwRHis.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwRHis.Columns[1].DataPropertyName = "avt";
            dgwRHis.Columns[1].Name = "avt";
            dgwRHis.Columns[2].HeaderText = "Заглавие";
            dgwRHis.Columns[2].Width = 230;
            dgwRHis.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwRHis.Columns[2].DataPropertyName = "zag";
            dgwRHis.Columns[2].Name = "zag";
            dgwRHis.Columns[3].HeaderText = "Инвентар ный номер";
            dgwRHis.Columns[3].Width = 80;
            dgwRHis.Columns[3].Name = "inv";
            dgwRHis.Columns[3].DataPropertyName = "inv";
            dgwRHis.Columns[4].HeaderText = "Расст. шифр";
            dgwRHis.Columns[4].Width = 100;
            dgwRHis.Columns[4].Name = "shifr";
            dgwRHis.Columns[4].DataPropertyName = "shifr";
            dgwRHis.Columns[5].Visible = false;
            dgwRHis.Columns[5].Name = "izd";
            dgwRHis.Columns[5].DataPropertyName = "izd";
            dgwRHis.Columns[6].Visible = false;
            dgwRHis.Columns[6].Name = "idr";
            dgwRHis.Columns[6].DataPropertyName = "idr";
            dgwRHis.Columns[7].Visible = false;
            dgwRHis.Columns[7].Name = "oid";
            dgwRHis.Columns[7].DataPropertyName = "oid";
            dgwRHis.Columns[8].HeaderText = "От кого";
            dgwRHis.Columns[8].Width = 100;
            dgwRHis.Columns[8].Name = "dp";
            //dgwRHis.Columns[8].CellTemplate.Style.WrapMode = DataGridViewTriState.True;
            dgwRHis.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwRHis.Columns[8].DataPropertyName = "dp";
            dgwRHis.Columns[9].Visible = false;
            dgwRHis.Columns[9].Name = "fio";
            dgwRHis.Columns[9].DataPropertyName = "fio";
            dgwRHis.Columns[10].Visible = false;
            dgwRHis.Columns[10].Name = "gizd";
            dgwRHis.Columns[10].DataPropertyName = "gizd";
            //dgwRHis.Columns[11].Name = "startd";
            dgwRHis.Columns[11].ValueType = typeof(DateTime);
            dgwRHis.Columns[11].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgwRHis.Columns[11].HeaderText = "Дата заказа";
            dgwRHis.Columns[11].Width = 80;
            dgwRHis.Columns[11].DataPropertyName = "startd";

            //dgwRHis.Columns[12].Name = "refusual";
            dgwRHis.Columns[12].DataPropertyName = "refusual";
            dgwRHis.Columns[12].HeaderText = "Отказ";
            dgwRHis.Columns[12].Width = 70;

            dgwRHis.Columns[13].DataPropertyName = "sts";
            dgwRHis.Columns[13].HeaderText = "Статус";
            dgwRHis.Columns[12].Width = 70;

            dgwRHis.Columns["note"].Name = "note";
            dgwRHis.Columns["note"].DataPropertyName = "note";

            dgwRHis.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //dgwRHis.Columns[5].Visible = false;
            //dgwEmp.Columns[6].Visible = false;
        }

        private void FormMainTable_Interface()
        {
            dgwEmp.Columns.Clear();
            dgwEmp.AutoGenerateColumns = false;
            dgwEmp.DataSource = MainTable;

            dgwEmp.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgwEmp.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //dgwEmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwEmp.Columns.Add("NN", "ПИН");
            dgwEmp.Columns.Add("NN1", "Автор");
            dgwEmp.Columns.Add("NN2", "Заглавие");
            dgwEmp.Columns.Add("NN3", "Инв. номер");
            dgwEmp.Columns.Add("NN4", "Расст. шифр");
            dgwEmp.Columns.Add("NN5", "Дата издания");
            dgwEmp.Columns.Add("NN6", "id берущего отдела");
            dgwEmp.Columns.Add("NN7", "id заказа");
            dgwEmp.Columns.Add("NN8", "от кого");
            dgwEmp.Columns.Add("NN9", "fio");
            dgwEmp.Columns.Add("NN10", "yaz");
            dgwEmp.Columns.Add("note", "note");
            
            dgwEmp.ReadOnly = true;

            dgwEmp.Columns[0].HeaderText = "ПИН";
            dgwEmp.Columns[0].Width = 74;
            dgwEmp.Columns[0].DataPropertyName = "idm";
            dgwEmp.Columns[0].Name = "idm";
            dgwEmp.Columns[1].HeaderText = "Автор";
            dgwEmp.Columns[1].Width = 125;
            dgwEmp.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEmp.Columns[1].DataPropertyName = "avt";
            dgwEmp.Columns[1].Name = "avt";
            dgwEmp.Columns[2].HeaderText = "Заглавие";
            dgwEmp.Columns[2].Width = 265;
            dgwEmp.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEmp.Columns[2].DataPropertyName = "zag";
            dgwEmp.Columns[2].Name = "zag";
            dgwEmp.Columns[3].HeaderText = "Инвентар ный номер";
            dgwEmp.Columns[3].Width = 80;
            dgwEmp.Columns[3].Name = "inv";
            dgwEmp.Columns[3].DataPropertyName = "inv";
            //string d = ((DataTable)dgwEmp.DataSource).Rows[0][7].ToString();
            

            dgwEmp.Columns[4].HeaderText = "Расст. шифр";
            dgwEmp.Columns[4].Width = 100;
            dgwEmp.Columns[4].Name = "shifr";
            dgwEmp.Columns[4].DataPropertyName = "shifr";
            dgwEmp.Columns[5].Visible = false;
            dgwEmp.Columns[5].Name = "izd";
            dgwEmp.Columns[5].DataPropertyName = "izd";
            dgwEmp.Columns[6].Visible = false;
            dgwEmp.Columns[6].Name = "idr";
            dgwEmp.Columns[6].DataPropertyName = "idr";
            dgwEmp.Columns[7].Visible = false;
            dgwEmp.Columns[7].Name = "oid";
            dgwEmp.Columns[7].DataPropertyName = "oid";
            dgwEmp.Columns[8].HeaderText = "От кого";
            dgwEmp.Columns[8].Width = 130;
            dgwEmp.Columns[8].Name = "dp";
            //dgwEmp.Columns[8].CellTemplate.Style.WrapMode = DataGridViewTriState.True;
            dgwEmp.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwEmp.Columns[8].DataPropertyName = "dp";
            dgwEmp.Columns[9].Visible = false;
            dgwEmp.Columns[9].Name = "fio";
            dgwEmp.Columns[9].DataPropertyName = "fio";
            dgwEmp.Columns[10].DataPropertyName = "yaz";
            dgwEmp.Columns[10].Name = "yaz";
            dgwEmp.Columns[10].Visible = false;
            dgwEmp.Columns["note"].DataPropertyName = "note";
            dgwEmp.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            //dgwEmp.Columns[5].Visible = false;
            //dgwEmp.Columns[6].Visible = false;
        }
        private void FormHisTable_Interface()
        {
            dgwHis.Columns.Clear();
            dgwHis.AutoGenerateColumns = false;
            dgwHis.DataSource = HisTable;

            dgwHis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgwHis.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //dgwHis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwHis.Columns.Add("NN", "ПИН");
            dgwHis.Columns.Add("NN1", "Автор");
            dgwHis.Columns.Add("NN2", "Заглавие");
            dgwHis.Columns.Add("NN3", "Инв. номер");
            dgwHis.Columns.Add("NN4", "Расст. шифр");
            dgwHis.Columns.Add("NN5", "Дата издания");
            dgwHis.Columns.Add("NN6", "id берущего отдела");
            dgwHis.Columns.Add("NN7", "id заказа");
            dgwHis.Columns.Add("NN8", "от кого");
            dgwHis.Columns.Add("NN9", "fio");
            dgwHis.Columns.Add("NN11", "startd");
            dgwHis.Columns.Add("NN12", "refusual");
            dgwHis.Columns.Add("yaz", "yaz");
            dgwHis.Columns.Add("note", "Инв. метка");

            dgwHis.ReadOnly = true;

            dgwHis.Columns[0].HeaderText = "ПИН";
            dgwHis.Columns[0].Width = 74;
            dgwHis.Columns[0].DataPropertyName = "idm";
            dgwHis.Columns[0].Name = "idm";
            dgwHis.Columns[1].HeaderText = "Автор";
            dgwHis.Columns[1].Width = 125;
            dgwHis.Columns[1].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwHis.Columns[1].DataPropertyName = "avt";
            dgwHis.Columns[1].Name = "avt";
            dgwHis.Columns[2].HeaderText = "Заглавие";
            dgwHis.Columns[2].Width = 265;
            dgwHis.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwHis.Columns[2].DataPropertyName = "zag";
            dgwHis.Columns[2].Name = "zag";
            dgwHis.Columns[3].HeaderText = "Инвентар ный номер";
            dgwHis.Columns[3].Width = 80;
            dgwHis.Columns[3].Name = "inv";
            dgwHis.Columns[3].DataPropertyName = "inv";
            //string d = ((DataTable)dgwHis.DataSource).Rows[0][7].ToString();


            dgwHis.Columns[4].HeaderText = "Расст. шифр";
            dgwHis.Columns[4].Width = 100;
            dgwHis.Columns[4].Name = "shifr";
            dgwHis.Columns[4].DataPropertyName = "shifr";
            dgwHis.Columns[5].Visible = false;
            dgwHis.Columns[5].Name = "izd";
            dgwHis.Columns[5].DataPropertyName = "izd";
            dgwHis.Columns[6].Visible = false;
            dgwHis.Columns[6].Name = "idr";
            dgwHis.Columns[6].DataPropertyName = "idr";
            dgwHis.Columns[7].Visible = false;
            dgwHis.Columns[7].Name = "oid";
            dgwHis.Columns[7].DataPropertyName = "oid";
            dgwHis.Columns[8].HeaderText = "От кого";
            dgwHis.Columns[8].Width = 130;
            dgwHis.Columns[8].Name = "dp";
            //dgwHis.Columns[8].CellTemplate.Style.WrapMode = DataGridViewTriState.True;
            dgwHis.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgwHis.Columns[8].DataPropertyName = "dp";
            dgwHis.Columns[9].Visible = false;
            dgwHis.Columns[9].Name = "fio";
            dgwHis.Columns[9].DataPropertyName = "fio";
            //dgwHis.Columns["NN11"].Name = "startd";
            //dgwHis.Columns["NN11"].ValueType = typeof(DateTime);
            dgwHis.Columns["NN11"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgwHis.Columns["NN11"].DataPropertyName = "startd";
            dgwHis.Columns["NN11"].HeaderText = "Дата заказа";
            dgwHis.Columns["NN11"].Width = 80;
            //dgwHis.Columns["NN12"].Name = "refusual";
            dgwHis.Columns["NN12"].DataPropertyName = "refusual";
            dgwHis.Columns["NN12"].HeaderText = "Отказ";
            dgwHis.Columns["yaz"].DataPropertyName = "yaz";
            dgwHis.Columns["yaz"].HeaderText = "yazik";
            dgwHis.Columns["yaz"].Visible = false;
            dgwHis.Columns["note"].DataPropertyName = "note";
 

            dgwHis.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            //dgwHis.Columns[5].Visible = false;
            //dgwHis.Columns[6].Visible = false;
        }


        public bool Login(string login, string pass)
        {//                                    SELECT Employee.* FROM Employee WHERE (((Employee.Login)="1") AND ((Employee.Password)="1"));

            //OleDA.SelectCommand.CommandText = "SELECT * FROM Employee WHERE (((Employee.Login)='" + login + "') AND ((Employee.Password)='" + pass + "'))";
            SqlDA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME uname,dpt.NAME dname,USERS.DEPT iddp from BJVVV..USERS join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = '" + login.ToLower() + "' and lower(PASSWORD) = '" + pass.ToLower() + "'";

            //ReaderMain.Tables.Clear();
            DataSet R = new DataSet();
            if (SqlDA.Fill(R) != 0)
            {
                //F1.textBox1.Text = R.Tables[0].Rows[0]["FIO"].ToString();
                this.EmpID = R.Tables[0].Rows[0]["ID"].ToString();
                this.FIO = R.Tables[0].Rows[0]["uname"].ToString();
                this.FloorID = R.Tables[0].Rows[0]["iddp"].ToString();
                string tmp = R.Tables[0].Rows[0]["dname"].ToString();
                switch (tmp)
                {
                    case "…Хран… Сектор книгохранения - 2 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 2 этаж";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            //this.ForSQL = "and mhran.NAME = 'Книгохранение - 2 этаж' ";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 3 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 3 этаж";
                            //this.ForSQL = "and mhran.NAME = 'Книгохранение - 3 этаж' ";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 4 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 4 этаж";
                            this.ForSQL = " and ( (mhran.ID = 8 ) or (mhran.ID = 15)) ";
                            //this.ForSQL = "and ((mhran.NAME = 'Книгохранение - 4 этаж') or (mhran.NAME = 'Книгохранение - цоколь' )) ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - Новая периодика":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - Новая периодика";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 5 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 5 этаж";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            //this.ForSQL = "and mhran.NAME = 'Книгохранение - 5 этаж' ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 6 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 6 этаж";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            //this.ForSQL = "and mhran.NAME = 'Книгохранение - 6 этаж' ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 7 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 7 этаж";
                            this.ForSQL = " and mhran.ID = " + this.FloorID + " ";
                            //this.ForSQL = "and mhran.NAME = 'Книгохранение - 7 этаж' ";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - 0 этаж":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - 0 этаж";
                            this.ForSQL = " and ( (mhran.ID = 8 ) or (mhran.ID = 15)) ";
                            //this.ForSQL = "and ((mhran.NAME = 'Книгохранение - 4 этаж') or (mhran.NAME = 'Книгохранение - цоколь' ))";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    case "…Хран… Сектор книгохранения - Абонемент":
                        {
                            this.Floor = "…Хран… Сектор книгохранения - Абонемент";
                            this.ForSQL = " and ( (mhran.ID = 47 ) or (mhran.ID = 37)) ";
                            //this.ForSQL = "and ((mhran.NAME = 'Книгохранение - 4 этаж') or (mhran.NAME = 'Книгохранение - цоколь' ))";
                            this.BASE = "BJVVV";
                            this.OrderTableType = "Orders";
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Вы не сотрудник книгохранения!");
                            return false;
                        }
                }
                return true;
            }
            else
                return false;
        }



        private void tabPage2_Paint(object sender, PaintEventArgs e)
        {

            PaperSize size;
            Rectangle rectangle;
            StringFormat format;
            //string str = this.printString;
            string str = "Дата формирования заказа: " + DateTime.Now.ToString("dd.MM.yyyy HH:MM");
            size = new PaperSize("bar", 314, 492);
            Size rectsize = new Size(314, 492);
            Font printFont = new Font("code128", 10f);

            rectangle = new Rectangle(new Point(0, 0), new Size(314, 490));
            format = new StringFormat(StringFormatFlags.NoClip);// (0x4000);
            format.LineAlignment = StringAlignment.Far;
            format.Alignment = StringAlignment.Far;
            format.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;

            //format.LineAlignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
            //format.Alignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.NoClip;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawLine(Pens.Black, new Point(280, 0), new Point(280, 490));
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            
            str = "БЛАНК-ЗАКАЗ ДЛЯ ВЫДАЧИ ЛИТЕРАТУРЫ НА\r\n ДЛИТЕЛЬНОЕ ПОЛЬЗОВАНИЕ В ОТДЕЛЫ";
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            //g.DrawString(this.printString, this.printFont, Brushes.Black, rectangle, format);
            //this.printString = "";

            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawLine(Pens.Black, new Point(280, 0), new Point(280, 490));
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            format.Alignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;
            rectangle = new Rectangle(new Point(250, 0), new Size(30, 490));
            str = "Отдел: " + dgwHis.SelectedRows[0].Cells["dp"].Value.ToString() + " (" + dgwHis.SelectedRows[0].Cells["fio"].Value.ToString() + ")";//db.GetReader(dg.SelectedRows[0].Cells["idr"].Value.ToString());

            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(200, 0), new Size(30, 240));
            str = "Шифр: " + dgwHis.SelectedRows[0].Cells["shifr"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(200, 240), new Size(30, 250));
            str = "Инв. : " + dgwHis.SelectedRows[0].Cells["inv"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(170, 0), new Size(30, 490));
            str = "Автор : " + dgwHis.SelectedRows[0].Cells["avt"].Value.ToString();
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(90, 0), new Size(80, 490));
            str = "Заглавие : " + dgwHis.SelectedRows[0].Cells["zag"].Value.ToString();
            format.LineAlignment = StringAlignment.Near;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(60, 0), new Size(30, 490));
            str = "Год издания: " + dgwHis.SelectedRows[0].Cells["izd"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(20, 0), new Size(40, 490));
            str = "Получил______________________________ ";
            format.LineAlignment = StringAlignment.Far;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(0, 0), new Size(40, 490));
            str = "*123459854*";//вставить год
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.
            //e.Graphics.DrawRectangle(Pens.Red, rectangle);
            //e.Graphics.DrawString(str, new Font("C39HrP24DhTt", 36f), Brushes.Black, rectangle, format);

            //throw new Exception("The method or operation is not implemented.");

        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tpEmployeeOrders":
                    {
                        FormMainTable();
                        FormMainTable_Interface();

                        if (MainTable.Rows.Count == 0)
                            button1.Enabled = false;
                        else
                            button1.Enabled = true;

                        tabControl1.TabPages.RemoveByKey("tab2");
                        break;
                    }
                case "tpReaderOrders":
                    {
                        ShowReaderOrders();
                        //FormReadersTable();
                        //FormReaderTable_Interface();
                        //if (ReadersTable.Rows.Count == 0)
                        //    button8.Enabled = false;
                        //else
                        //    button8.Enabled = true;

                        tabControl1.TabPages.RemoveByKey("tab2");
                        break;
                    }
                case "tabPage3":
                    {
                        FormReadersHisTable();
                        FormReaderHisTable_Interface();
                        if (ReadersHisTable.Rows.Count == 0)
                            button8.Enabled = false;
                        else
                            button8.Enabled = true;

                        tabControl1.TabPages.RemoveByKey("tab2");
                        break;
                    }
                case "tabHis":
                    {
                        FormHisTable();
                        FormHisTable_Interface();
                        break;
                    }
            }
        }


        private void Form1_Shown(object sender, EventArgs e)
        {

            FormMainTable();
            FormMainTable_Interface();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //int rc = dgwEmp.Rows.Count;
            FormMainTable();
            FormMainTable_Interface();
            //int rc1 = dgwReaders.Rows.Count;
            FormReadersTable();
            FormReaderTable_Interface();

            if ((dgwEmp.Rows.Count > 0) || (dgwReaders.Rows.Count > 0))
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                fInfo.dwFlags = 2;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);

                button1.Enabled = true;
                button8.Enabled = true;
            }
            else
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                fInfo.dwFlags = 0;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);
                button1.Enabled = false;
                button8.Enabled = false;
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)//заказ сотрудников на сегодня
        {
            if (dgwEmp.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlank pb = new PrintBlank(db, dgwEmp, this.Floor); //когда принтер заработаетвключить это
            pb.Print();
            db.OrdHis(dgwEmp.SelectedRows[0].Cells["oid"].Value.ToString(), this.EmpID);
            db.delFromOrders(dgwEmp.SelectedRows[0].Cells["oid"].Value.ToString());
            dgwEmp.Rows.Remove(dgwEmp.SelectedRows[0]);
            if (dgwEmp.Rows.Count == 0)
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                //fInfo.dwFlags = 3 | 12;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.dwFlags = 0;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);
                button1.Enabled = false;
            }
            else
                button1.Enabled = true;
            //tabControl1.TabPages.Add("tab2", "Временно вместо принтера. Принтер выведет то же самое.");
            //tabControl1.TabPages["tab2"].Paint += new PaintEventHandler(tabPage2_Paint);
            //tabControl1.SelectedTab = tabControl1.TabPages["tab2"];
        }
        private void button4_Click(object sender, EventArgs e)//история сотрудников 
        {
            PrintBlank pb = new PrintBlank(db, dgwHis, this.Floor); //когда принтер заработаетвключить это
            pb.Print();
            //db.delFromOrders(dgwHis.SelectedRows[0].Cells["oid"].Value.ToString());
            //dgwHis.Rows.Remove(dgwHis.SelectedRows[0]);
            if (dgwHis.Rows.Count == 0)
                button4.Enabled = false;
            else
                button4.Enabled = true;



            //tabControl1.TabPages.Add("tab2", "Временно вместо принтера. Принтер выведет то же самое.");
            //tabControl1.TabPages["tab2"].Paint += new PaintEventHandler(tabPage2_Paint);
            //tabControl1.SelectedTab = tabControl1.TabPages["tab2"];

        }
        private void button8_Click(object sender, EventArgs e)//заказ читателей на сегодня
        {
            if (dgwReaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgwReaders, user.SelectedUserStatus.DepName, this); //когда принтер заработаетвключить это
            pb.Print();
            ReaderInfo reader = ReaderInfo.GetReader(Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["readerid"].Value));
            
            

            //db.ChangeStatus(dgwReaders.SelectedRows[0].Cells["oid"].Value.ToString(), this.EmpID, this.Floor);
            CirculationInfo circulation = new CirculationInfo();
            circulation.ChangeOrderStatus(reader,user, Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["orderid"].Value), CirculationStatuses.EmployeeLookingForBook.Value);

            dgwReaders.Rows.Remove(dgwReaders.SelectedRows[0]);
            if (dgwReaders.Rows.Count == 0)
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                //fInfo.dwFlags = 3 | 12;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.dwFlags = 0;// FLASHW_TIMERNOFG;   моргать always
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);
                button8.Enabled = false;
            }
            else
                button8.Enabled = true;
            //tabControl1.TabPages.Add("tab2", "Временно вместо принтера. Принтер выведет то же самое.");
            //tabControl1.TabPages["tab2"].Paint += new PaintEventHandler(tabPage2_Paint);
            //tabControl1.SelectedTab = tabControl1.TabPages["tab2"];

        }
        private void button9_Click(object sender, EventArgs e)//история читателей на сегодня
        {
            if (dgwRHis.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgwRHis, this.Floor, this); //когда принтер заработаетвключить это
            pb.Print();
            if (dgwRHis.Rows.Count == 0)
                button8.Enabled = false;
            else
                button8.Enabled = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //FlashWindow(this.Handle, false);

            FLASHWINFO fInfo = new FLASHWINFO();
            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = this.Handle;
            //fInfo.dwFlags = 3 | 12;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
            fInfo.dwFlags = 0;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
            fInfo.uCount = UInt32.MaxValue;
            fInfo.dwTimeout = 0;
            FlashWindowEx(ref fInfo);
            button1.Enabled = false;

        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);
        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [StructLayout(LayoutKind.Sequential)]
        struct WINDOWINFO
        {
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
                : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            int _left;
            int _top;
            int _right;
            int _bottom;

            public RECT(global::System.Drawing.Rectangle rectangle)
                : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
            {
            }
            public RECT(int left, int top, int right, int bottom)
            {
                _left = left;
                _top = top;
                _right = right;
                _bottom = bottom;
            }

            public int X
            {
                get { return Left; }
                set { Left = value; }
            }
            public int Y
            {
                get { return Top; }
                set { Top = value; }
            }
            public int Left
            {
                get { return _left; }
                set { _left = value; }
            }
            public int Top
            {
                get { return _top; }
                set { _top = value; }
            }
            public int Right
            {
                get { return _right; }
                set { _right = value; }
            }
            public int Bottom
            {
                get { return _bottom; }
                set { _bottom = value; }
            }
            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value - Top; }
            }
            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }
            public global::System.Drawing.Point Location
            {
                get { return new global::System.Drawing.Point(Left, Top); }
                set
                {
                    Left = value.X;
                    Top = value.Y;
                }
            }
            public global::System.Drawing.Size Size
            {
                get { return new global::System.Drawing.Size(Width, Height); }
                set
                {
                    Right = value.Width + Left;
                    Bottom = value.Height + Top;
                }
            }

            public global::System.Drawing.Rectangle ToRectangle()
            {
                return new global::System.Drawing.Rectangle(this.Left, this.Top, this.Width, this.Height);
            }
            public static global::System.Drawing.Rectangle ToRectangle(RECT Rectangle)
            {
                return Rectangle.ToRectangle();
            }
            public static RECT FromRectangle(global::System.Drawing.Rectangle Rectangle)
            {
                return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
            }

            public static implicit operator global::System.Drawing.Rectangle(RECT Rectangle)
            {
                return Rectangle.ToRectangle();
            }
            public static implicit operator RECT(global::System.Drawing.Rectangle Rectangle)
            {
                return new RECT(Rectangle);
            }
            public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
            {
                return Rectangle1.Equals(Rectangle2);
            }
            public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
            {
                return !Rectangle1.Equals(Rectangle2);
            }

            public override string ToString()
            {
                return "{Left: " + Left + "; " + "Top: " + Top + "; Right: " + Right + "; Bottom: " + Bottom + "}";
            }

            public bool Equals(RECT Rectangle)
            {
                return Rectangle.Left == Left && Rectangle.Top == Top && Rectangle.Right == Right && Rectangle.Bottom == Bottom;
            }
            public override bool Equals(object Object)
            {
                if (Object is RECT)
                {
                    return Equals((RECT)Object);
                }
                else if (Object is Rectangle)
                {
                    return Equals(new RECT((global::System.Drawing.Rectangle)Object));
                }

                return false;
            }

            public override int GetHashCode()
            {
                return Left.GetHashCode() ^ Right.GetHashCode() ^ Top.GetHashCode() ^ Bottom.GetHashCode();
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            
            WINDOWINFO inf = new WINDOWINFO();
            bool tip = GetWindowInfo(this.Handle, ref inf);


            if (inf.dwWindowStatus == 1)
            {
                //FlashWindow(this.Handle, true);
                MessageBox.Show("form is active");
            }
            {
                MessageBox.Show("form is inactive");
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(dgwRHis, this);
            f2.Show();

        }


        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)//отказ для сотрудников
        {
            //sddddddddddddddddddddddddddfggggggg
            if (dgwHis.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            Refusal rf = new Refusal(dgwHis.SelectedRows[0].Cells["oid"].Value.ToString());
            rf.ShowDialog();
            if (rf.Cause == "")
                return;
            db.RefusualEmployee(rf.Cause, dgwHis.SelectedRows[0].Cells["oid"].Value.ToString());
            FormHisTable();
            FormHisTable_Interface();
        }

        private void button11_Click_1(object sender, EventArgs e)//отказ для читателей
        {
            if (dgwRHis.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            if (dgwRHis.SelectedRows[0].Cells["NN13"].Value.ToString() != "Сотрудник книгохранения обрабатывает заказ")
            {
                MessageBox.Show("Вы не можете дать отказ на заказ с таким статусом!");
                return;
            }
            Refusal rf = new Refusal(dgwRHis.SelectedRows[0].Cells["oid"].Value.ToString());
            rf.ShowDialog();
            if (rf.Cause == "")
                return;
            db.RefusualReader(rf.Cause, dgwRHis.SelectedRows[0].Cells["oid"].Value.ToString());
            FormReadersHisTable();
            FormReaderHisTable_Interface();

        }


        private void button14_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(this,this.db,this.Floor);
            f3.InitForm();
            f3.ShowDialog();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            button14_Click(sender, e);
        }

    }
}