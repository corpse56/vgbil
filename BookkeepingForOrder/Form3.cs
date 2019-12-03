using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Readers;
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
        BJUserInfo bjUser;
        public Form3()
        {
            InitializeComponent();
        }
        public Form3(Form1 f1_,BJUserInfo bjUser)
        {
            InitializeComponent();
            F1 = f1_;
            this.bjUser = bjUser;
        }
        public  void ShowReaderOrders()
        {
            dgwReaders.Rows.Clear();
            dgwReaders.Columns.Clear();
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
            CirculationInfo circulation = new CirculationInfo();
            List<OrderInfo> orders = circulation.GetOrdersForStorage(bjUser.SelectedUserStatus.DepId, bjUser.SelectedUserStatus.DepName, CirculationStatuses.EmployeeLookingForBook.Value);
            if (bjUser.SelectedUserStatus.DepId == 8)//0 и 4 этаж должны получать заказы в одну точку
            {
                List<OrderInfo> orders1 = circulation.GetOrdersForStorage(15, "…Хран… Сектор книгохранения - 0 этаж");
                foreach (OrderInfo o in orders1)
                    orders.Add(o);
            }
            if (bjUser.SelectedUserStatus.DepId == 15)
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
                row.Cells["pin"].Value = order.BookId.Substring(order.BookId.IndexOf("_") + 1);
                row.Cells["author"].Value = order.Book.Author;
                row.Cells["title"].Value = order.Book.Title;
                row.Cells["inv"].Value = exemplar.Fields["899$p"].ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bPrintSelected_Click(object sender, EventArgs e)
        {
            if (dgwReaders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Не выбрана ни одна строка!");
                return;
            }
            PrintBlankReaders pb = new PrintBlankReaders(db, dgwReaders, this.Floor, F1); //когда принтер заработаетвключить это
            pb.Print();
        }

        private void bRefuseSelected_Click(object sender, EventArgs e)
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
            circulation.RefuseOrder(Convert.ToInt32(dgwReaders.SelectedRows[0].Cells["orderid"].Value), rf.Cause, bjUser);
            //db.RefusualReader(rf.Cause, dgwRHis.SelectedRows[0].Cells["oid"].Value.ToString());
            //FormReadersHisTable();
            //FormReaderHisTable_Interface();
            ShowReaderOrders();

        }
    }
}
