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
using LibflClassLibrary.Books.BJBooks;
using ExtGui;

namespace BookkeepingForOrder
{
    public partial class Form1 : Form
    {
        public string EmpID;
        public string FIO;
        public string Floor;
        public string FloorID;
        public string ForSQL;
        public string BASE = "BJVVV";
        public string OrderTableType = "Orders";
        public SqlDataAdapter SqlDA;
        public SqlConnection SqlCon;
        private ExtGui.RoundProgress RndPrg;
        public DbForEmployee db;
        DataTable MainTable;
        DataTable HisTable;
        DataTable ReadersTable;
        DataTable ReadersHisTable;

        public BJUserInfo user;


        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool bInvert);


        public Form1()
        {
            SqlCon = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDA = new SqlDataAdapter();
            SqlDA.SelectCommand = new SqlCommand();
            SqlDA.SelectCommand.Connection = SqlCon;
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            fBJAuthorization fAuth = new fBJAuthorization("BJVVV");
            fAuth.ShowDialog();
            if (fAuth.DialogResult == DialogResult.Cancel || fAuth.User == null)
            {
                Close();
                return;
            }
            user = fAuth.User;
            label1.Text = $"{user.SelectedUserStatus.DepName} {user.FIO}";
            //для 0 и 4 этажа требования выводить в одну таблицу.
            if (user.SelectedUserStatus.DepId == 8 || user.SelectedUserStatus.DepId == 15)
            {
                this.ForSQL = $" and mhran.ID in (8, 15) ";
            }
            else
            {
                this.ForSQL = $" and mhran.ID =  {user.SelectedUserStatus.DepId}  ";
            }
            db = new DbForEmployee(this.OrderTableType, this.BASE, this);
        }
        bool InitReloadReaderOrders = true;
        private void ShowReaderOrders()
        {
            if (InitReloadReaderOrders)
            {
                KeyValuePair<string, string>[] columns =
                {
                    new KeyValuePair<string, string> ( "pin", "ПИН"),
                    new KeyValuePair<string, string> ( "author", "Автор"),
                    new KeyValuePair<string, string> ( "title", "Заглавие"),
                    new KeyValuePair<string, string> ( "inv", "Инв. номер"),
                    new KeyValuePair<string, string> ( "cipher", "Расст. шифр"),
                    new KeyValuePair<string, string> ( "readerid", "Номер читателя"),
                    new KeyValuePair<string, string> ( "fio", "ФИО читателя"),
                    new KeyValuePair<string, string> ( "startdate", "Дата формирования заказа"),
                    new KeyValuePair<string, string> ( "orderid", "orderid"),
                    new KeyValuePair<string, string> ( "status", "Статус заказа"),
                    new KeyValuePair<string, string> ( "note", "Инв. метка"),
                    new KeyValuePair<string, string> ( "pubdate", "Дата издания"),
                    new KeyValuePair<string, string> ( "refusual", "Причина отказа"),
                    new KeyValuePair<string, string> ( "iddata", "iddata")
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
                dgwReaders.Columns["startdate"].Width = 80;
                dgwReaders.Columns["orderid"].Visible = false;
                dgwReaders.Columns["status"].Width = 100;
                dgwReaders.Columns["note"].Width = 60;
                dgwReaders.Columns["pubdate"].Visible = false;
                dgwReaders.Columns["iddata"].Visible = false;
                InitReloadReaderOrders = false;
            }
            else
            {
                dgwReaders.Rows.Clear();
            }
            CirculationInfo circulation = new CirculationInfo();
            List<OrderInfo> orders = circulation.GetOrdersForStorage(user.SelectedUserStatus.DepId, user.SelectedUserStatus.DepName);
            if (user.SelectedUserStatus.DepId == 8)//0 и 4 этаж должны получать заказы в одну точку
            {
                List<OrderInfo> orders1 = circulation.GetOrdersForStorage(15, "…Хран… Сектор книгохранения - 0 этаж");
                foreach (OrderInfo o in orders1)
                    orders.Add(o);
            }
            if (user.SelectedUserStatus.DepId == 15)
            {
                List<OrderInfo> orders1 = circulation.GetOrdersForStorage(8, "…Хран… Сектор книгохранения - 4 этаж");
                foreach (OrderInfo o in orders1)
                    orders.Add(o);
            }
            foreach (var order in orders)
            {
                BookExemplarBase exemplar = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);
                ReaderInfo reader = ReaderInfo.GetReader(order.ReaderId);
                dgwReaders.Rows.Add();
                var row = dgwReaders.Rows[dgwReaders.Rows.Count - 1];

                row.Cells["startdate"].Value = order.StartDate;
                row.Cells["pin"].Value = order.BookId.Substring(order.BookId.IndexOf("_")+1);
                row.Cells["author"].Value = order.Book.Author;
                row.Cells["title"].Value = order.Book.Title;
                row.Cells["inv"].Value = exemplar.InventoryNumber;//exemplar.Fields["899$p"].ToString();
                row.Cells["cipher"].Value = exemplar.Cipher;
                row.Cells["readerid"].Value = order.ReaderId;
                
                row.Cells["fio"].Value = (string.IsNullOrEmpty(reader.FatherName)) ?    $"{reader.FamilyName} {reader.Name.Substring(0, 1)}." :
                                                                                        $"{reader.FamilyName} {reader.Name.Substring(0,1)}. { reader.FatherName.Substring(0, 1)}.";
                row.Cells["orderid"].Value = order.OrderId;
                row.Cells["status"].Value = order.StatusName;
                row.Cells["note"].Value = exemplar.Fields["899$x"].ToString();
                row.Cells["pubdate"].Value = order.Book.PublishDate;
                row.Cells["iddata"].Value = order.ExemplarId;
                row.Cells["refusual"].Value = string.IsNullOrEmpty(order.Refusual) ? "<нет>" : order.Refusual;
            }
        }
        bool InitReloadReaderHistoryOrders = true;

        public RoundProgress RndPrg1 { get => RndPrg; set => RndPrg = value; }

        private void ShowReaderHistoryOrders()
        {
            if (InitReloadReaderHistoryOrders)
            {
                KeyValuePair<string, string>[] columns =
                {
                    new KeyValuePair<string, string> ( "pin", "ПИН"),
                    new KeyValuePair<string, string> ( "author", "Автор"),
                    new KeyValuePair<string, string> ( "title", "Заглавие"),
                    new KeyValuePair<string, string> ( "inv", "Инв. номер"),
                    new KeyValuePair<string, string> ( "cipher", "Расст. шифр"),
                    new KeyValuePair<string, string> ( "readerid", "Номер читателя"),
                    new KeyValuePair<string, string> ( "fio", "ФИО читателя"),
                    new KeyValuePair<string, string> ( "startdate", "Дата формирования заказа"),
                    new KeyValuePair<string, string> ( "orderid", "orderid"),
                    new KeyValuePair<string, string> ( "status", "Статус заказа"),
                    new KeyValuePair<string, string> ( "note", "Инв. метка"),
                    new KeyValuePair<string, string> ( "pubdate", "Дата издания"),
                    new KeyValuePair<string, string> ( "refusual", "Причина отказа"),
                    new KeyValuePair<string, string> ( "iddata", "iddata")
                };
                foreach (var c in columns)
                    dgwRHis.Columns.Add(c.Key, c.Value);

                dgwRHis.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgwRHis.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                dgwRHis.Columns["startdate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                dgwRHis.Columns["pin"].Width = 74;
                dgwRHis.Columns["author"].Width = 125;
                dgwRHis.Columns["title"].Width = 265;
                dgwRHis.Columns["inv"].Width = 80;
                dgwRHis.Columns["cipher"].Width = 100;
                dgwRHis.Columns["readerid"].Width = 80;
                dgwRHis.Columns["fio"].Width = 120;
                dgwRHis.Columns["startdate"].Width = 80;
                dgwRHis.Columns["orderid"].Visible = false;
                dgwRHis.Columns["status"].Width = 100;
                dgwRHis.Columns["note"].Width = 60;
                dgwRHis.Columns["pubdate"].Visible = false;
                dgwRHis.Columns["iddata"].Visible = false;
                InitReloadReaderHistoryOrders = false;
            }
            else
            {
                dgwRHis.Rows.Clear();
            }
            CirculationInfo circulation = new CirculationInfo();
            List<OrderInfo> orders = circulation.GetOrdersHistoryForStorage(user.SelectedUserStatus.DepId, user.SelectedUserStatus.DepName);
            foreach (var order in orders)
            {
                BookExemplarBase exemplar = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);
                ReaderInfo reader = ReaderInfo.GetReader(order.ReaderId);
                dgwRHis.Rows.Add();
                var row = dgwRHis.Rows[dgwRHis.Rows.Count - 1];

                row.Cells["startdate"].Value = order.StartDate;
                row.Cells["pin"].Value = order.BookId.Substring(order.BookId.IndexOf("_") + 1);
                row.Cells["author"].Value = order.Book.Author;
                row.Cells["title"].Value = order.Book.Title;
                row.Cells["inv"].Value = exemplar.InventoryNumber;//exemplar.Fields["899$p"].ToString();
                row.Cells["cipher"].Value = exemplar.Cipher;
                row.Cells["readerid"].Value = order.ReaderId;
                row.Cells["fio"].Value = (string.IsNullOrEmpty(reader.FatherName)) ? $"{reader.FamilyName} {reader.Name.Substring(0, 1)}." :
                                                                                        $"{reader.FamilyName} {reader.Name.Substring(0, 1)}. { reader.FatherName.Substring(0, 1)}.";
                row.Cells["orderid"].Value = order.OrderId;
                row.Cells["status"].Value = order.StatusName;
                row.Cells["note"].Value = exemplar.Fields["899$x"].ToString();
                row.Cells["pubdate"].Value = order.Book.PublishDate;
                row.Cells["iddata"].Value = order.ExemplarId;
                row.Cells["refusual"].Value = string.IsNullOrEmpty(order.Refusual) ? "<нет>" : order.Refusual;
            }
        }

        //EKATERINA.A.LISOVSKAYA katya - 3 этаж

        private void FormMainTable()
        {
            MainTable = db.GetTable(this.ForSQL);
        }
        private void FormHisTable()
        {
            HisTable = db.GetHistory(this.ForSQL);
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
                            bEmployeeOrder.Enabled = false;
                        else
                            bEmployeeOrder.Enabled = true;

                        tabControl1.TabPages.RemoveByKey("tab2");
                        break;
                    }
                case "tpReaderOrders":
                    {
                        ShowReaderOrders();
                        tabControl1.TabPages.RemoveByKey("tab2");
                        break;
                    }
                case "tpReaderHistoryOrders":
                    {
                        ShowReaderHistoryOrders();
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

            FormMainTable();
            FormMainTable_Interface();

            ShowReaderOrders();
            //ShowReaderHistoryOrders();
            bool NeedFlash = false;
            foreach (DataGridViewRow row in dgwReaders.Rows)
            {
                if (row.Cells["status"].Value.ToString() == CirculationStatuses.OrderIsFormed.Value)
                {
                    NeedFlash = true;
                    break;
                }
            }

            if ((dgwEmp.Rows.Count > 0) || (NeedFlash))
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                fInfo.dwFlags = 2;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);

                bEmployeeOrder.Enabled = true;
                //bPrintReaderOrder.Enabled = true;
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
                bEmployeeOrder.Enabled = false;
                //bPrintReaderOrder.Enabled = false;
            }
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        private void bEmployeeOrder_Click(object sender, EventArgs e)//заказ сотрудников на сегодня
        {
            if (dgwEmp.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlank pb = new PrintBlank(db, dgwEmp, this.user.SelectedUserStatus.DepName); //когда принтер заработаетвключить это
            pb.Print();
            db.OrdHis(dgwEmp.SelectedRows[0].Cells["oid"].Value.ToString(), this.user.Id.ToString());
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
                bEmployeeOrder.Enabled = false;
            }
            else
                bEmployeeOrder.Enabled = true;
            //tabControl1.TabPages.Add("tab2", "Временно вместо принтера. Принтер выведет то же самое.");
            //tabControl1.TabPages["tab2"].Paint += new PaintEventHandler(tabPage2_Paint);
            //tabControl1.SelectedTab = tabControl1.TabPages["tab2"];
        }
        private void button4_Click(object sender, EventArgs e)//история сотрудников 
        {
            PrintBlank pb = new PrintBlank(db, dgwHis, this.user.SelectedUserStatus.DepName); //когда принтер заработаетвключить это
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
        private void bPrintReaderOrder_Click(object sender, EventArgs e)//заказ читателей на сегодня
        {
            if (dgwReaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgwReaders, user.SelectedUserStatus.DepName, this); //когда принтер заработаетвключить это
            pb.Print();
            ReaderInfo reader = ReaderInfo.GetReader(Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["readerid"].Value));


            if (dgwReaders.SelectedRows[0].Cells["status"].Value.ToString() == CirculationStatuses.OrderIsFormed.Value)
            {
                CirculationInfo circulation = new CirculationInfo();
                circulation.ChangeOrderStatus(user, Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["orderid"].Value), CirculationStatuses.EmployeeLookingForBook.Value);
            }
            //dgwReaders.Rows.Remove(dgwReaders.SelectedRows[0]);

            timer1_Tick(sender, e);
            bool NeedFlash = false;
            foreach (DataGridViewRow row in dgwReaders.Rows)
            {
                if (row.Cells["status"].Value.ToString() == CirculationStatuses.OrderIsFormed.Value)
                {
                    NeedFlash = true;
                    break;
                }
            }

            if (NeedFlash)
            {
                FLASHWINFO fInfo = new FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = this.Handle;
                //fInfo.dwFlags = 3 | 12;// FLASHW_TIMERNOFG;   моргать пока не не попадет на передний план
                fInfo.dwFlags = 0;// FLASHW_TIMERNOFG;   моргать always
                fInfo.uCount = UInt32.MaxValue;
                fInfo.dwTimeout = 0;
                FlashWindowEx(ref fInfo);
            }

            ShowReaderOrders();

        }
        private void bReadersHistory_Click(object sender, EventArgs e)//история читателей на сегодня
        {
            if (dgwRHis.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgwRHis, this.user.SelectedUserStatus.DepName, this); //когда принтер заработаетвключить это
            pb.Print();
            if (dgwRHis.Rows.Count == 0)
                bPrintReaderOrder.Enabled = false;
            else
                bPrintReaderOrder.Enabled = true;
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
            bEmployeeOrder.Enabled = false;

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


        private void bRefusual_Click(object sender, EventArgs e)
        {
            if (dgwReaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            if (dgwReaders.SelectedRows[0].Cells["status"].Value.ToString() != CirculationStatuses.EmployeeLookingForBook.Value)
            {
                MessageBox.Show("Вы не можете дать отказ только на заказ со статусом \"Сотрудник подбирает книгу\"!");
                return;
            }
            Refusal rf = new Refusal(dgwReaders.SelectedRows[0].Cells["orderid"].Value.ToString());
            rf.ShowDialog();
            if (rf.Cause == "")
                return;
            CirculationInfo circulation = new CirculationInfo();
            circulation.RefuseOrder(Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["orderid"].Value), rf.Cause, user);
            //db.RefusualReader(rf.Cause, dgwRHis.SelectedRows[0].Cells["oid"].Value.ToString());
            //FormReadersHisTable();
            //FormReaderHisTable_Interface();
            ShowReaderOrders();
        }

        private void bEmployeeLookingForBook_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(this, this.user);
            f3.ShowReaderOrders();
            f3.ShowDialog();
        }
    }
}