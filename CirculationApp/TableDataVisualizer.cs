using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace CirculationApp
{
    public enum ReferenceType { HallService, ActiveHallOrders, FinishedHallOrders, AllBooksInHall, DebtorList, ReaderRegistration
                                ,OrdersCountBySubject }

    public partial class TableDataVisualizer : Form
    {
        private fDatePeriod dp_;
        private CirculationStatisticsManager csm_ = new CirculationStatisticsManager();
        private BJUserInfo bjUser_;
        private List<OrderInfo> orders_;
        private List<BookBase> books_;
        private List<BookExemplarBase> exemplars_;
        private ReferenceType rt_;
        private DataTable tableToDisplay_;
        public TableDataVisualizer()
        {
            InitializeComponent();
        }

        public TableDataVisualizer(fDatePeriod dp, BJUserInfo bjUser, ReferenceType rt )
        {
            this.dp_ = dp;
            this.bjUser_ = bjUser;
            this.rt_ = rt;
            InitializeComponent();

            switch (rt)
            {
                case ReferenceType.HallService:
                    HallService();
                    break;
                case ReferenceType.ActiveHallOrders:
                    orders_ = csm_.GetActiveHallOrders(bjUser);
                    FillStatuses();
                    ShowOrders(CirculationStatuses.IssuedInHall.Value);
                    break;
                case ReferenceType.FinishedHallOrders:
                    orders_ = csm_.GetFinishedHallOrders(bjUser);
                    ShowOrders(CirculationStatuses.Finished.Value);
                    break;
                case ReferenceType.AllBooksInHall:
                    exemplars_ = csm_.GetAllBooksInHall(bjUser);
                    ShowBooks();
                    break;
                case ReferenceType.DebtorList:
                    orders_ = csm_.GetDebtorsInHall(bjUser);
                    ShowDebtors();
                    break;
                case ReferenceType.ReaderRegistration:
                    ReaderRegistration();
                    break;
                case ReferenceType.OrdersCountBySubject:
                    tableToDisplay_ = csm_.GetOrdersCountBySubject(bjUser, dp_.StartDate, dp.EndDate);
                    OrdersBySubject();
                    break;
            }
        }

        private void OrdersBySubject()
        {
            this.Text =  $"Выдачи по тематике за период с {dp_.StartDate.ToString("dd.MM.yyyy")} по {dp_.EndDate.ToString("dd.MM.yyyy")}";
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "tema", "Тематика"),
                new KeyValuePair<string, string> ( "issueCount", "Количество выдач"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);
            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgViewer.Columns["tema"].Width = 300;
            dgViewer.Columns["issueCount"].Width = 80;
            foreach (DataRow r in tableToDisplay_.Rows)
            {
                dgViewer.Rows.Add();
                var row = dgViewer.Rows[dgViewer.Rows.Count - 1];

                row.Cells["tema"].Value = r[0].ToString();
                row.Cells["issueCount"].Value = r[1].ToString();
            }
        }

        private void ReaderRegistration()
        {
            this.Text = $"Статистика регистрации читателей за период с {dp_.StartDate.ToString("dd.MM.yyyy")} по {dp_.EndDate.ToString("dd.MM.yyyy")}";

            int RegisteredReadersAllCount       = csm_.RegisteredReadersAllCount(dp_.StartDate, dp_.EndDate);
            int RegisteredReadersRemoteCount    = csm_.RegisteredReadersRemoteCount(dp_.StartDate, dp_.EndDate);
            int LitresAccountAssignedCount      = csm_.LitresAccountAssignedCount(dp_.StartDate, dp_.EndDate);

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "id" , "п.п." ),
                new KeyValuePair<string, string> ( "name", "Наименование показателя"),
                new KeyValuePair<string, string> ( "value", "Значение показателя"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);

            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgViewer.Columns["id"].Width = 50;
            dgViewer.Columns["name"].Width = 200;
            dgViewer.Columns["value"].Width = 100;

            dgViewer.Rows.Add();
            var row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 1;
            row.Cells["name"].Value = "Всего зарегистрировано читателей";
            row.Cells["value"].Value = RegisteredReadersAllCount;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 2;
            row.Cells["name"].Value = "Зарегистрировано удалённых читателей";
            row.Cells["value"].Value = RegisteredReadersRemoteCount;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 3;
            row.Cells["name"].Value = "Выдано аккаунтов Литрес";
            row.Cells["value"].Value = LitresAccountAssignedCount;

        }

        private void ShowDebtors()
        {
            this.Text = "Список нарушителей";
            bSendEmail.Visible = true;
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "NN", "№№"),
                new KeyValuePair<string, string> ( "fio", "ФИО"),
                new KeyValuePair<string, string> ( "readerId", "Номер читателя"),
                new KeyValuePair<string, string> ( "readerRights", "Права читателя"),
                new KeyValuePair<string, string> ( "inventoryNumber", "Инв номер"),
                new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                new KeyValuePair<string, string> ( "phone", "Телефон"),
                new KeyValuePair<string, string> ( "email", "Email"),
                new KeyValuePair<string, string> ( "address", "Адрес"),
                new KeyValuePair<string, string> ( "issueDate", "Дата выдачи"),
                new KeyValuePair<string, string> ( "returnDate", "Дата возврата"),
                new KeyValuePair<string, string> ( "daysOverdue", "Дней просрочено"),
                new KeyValuePair<string, string> ( "orderStatus", "Статус заказа"),
                new KeyValuePair<string, string> ( "db", "Фонд БД"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);
            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgViewer.Columns["NN"].Width = 30;
            dgViewer.Columns["fio"].Width = 100;
            dgViewer.Columns["readerId"].Width = 100;
            dgViewer.Columns["readerRights"].Width = 300;
            dgViewer.Columns["inventoryNumber"].Width = 100;
            dgViewer.Columns["phone"].Width = 100;
            dgViewer.Columns["email"].Width = 100;
            dgViewer.Columns["address"].Width = 100;
            dgViewer.Columns["issueDate"].Width = 90;
            dgViewer.Columns["returnDate"].Width = 90;
            dgViewer.Columns["daysOverdue"].Width = 50;
            dgViewer.Columns["orderStatus"].Width = 90;
            dgViewer.Columns["db"].Width = 50;

            int i = 1;
            foreach (OrderInfo order in orders_)
            {
                dgViewer.Rows.Add();
                var row = dgViewer.Rows[dgViewer.Rows.Count - 1];

                ReaderInfo reader = ReaderInfo.GetReader(order.ReaderId);
                BookExemplarBase exemplarBase = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);

                if (exemplarBase is BJExemplarInfo == false)
                {
                    row.Cells["NN"].Value = i++;
                    row.Cells["fio"].Value = reader.FIO;
                    row.Cells["readerId"].Value = order.ReaderId;
                    row.Cells["readerRights"].Value = reader.Rights.ToString();
                    row.Cells["inventoryNumber"].Value = "Неопознанный экземпляр";
                    continue;
                }
                BJExemplarInfo exemplar = (BJExemplarInfo)exemplarBase;



                row.Cells["NN"].Value = i++;
                row.Cells["fio"].Value = reader.FIO;
                row.Cells["readerId"].Value = order.ReaderId;
                row.Cells["readerRights"].Value = reader.Rights.ToString();
                row.Cells["inventoryNumber"].Value = $"{exemplar.Fields["899$p"].ToString()}";
                row.Cells["bar"].Value = $"{exemplar.Fields["899$w"].ToString()}";
                row.Cells["phone"].Value = reader.MobileTelephone;
                row.Cells["email"].Value = reader.Email;
                row.Cells["address"].Value = $"{reader.RegistrationCity}, {reader.RegistrationStreet}";
                row.Cells["issueDate"].Value = (order.IssueDate.HasValue) ? order.IssueDate.Value.ToString("dd.MM.yyyy") : "<ещё не выдавалась>";
                row.Cells["returnDate"].Value = order.ReturnDate.ToString("dd.MM.yyyy");
                row.Cells["orderStatus"].Value = order.StatusName;
                int daysOverdue = (DateTime.Now - order.ReturnDate).Days;
                row.Cells["daysOverdue"].Value = daysOverdue;
                row.Cells["db"].Value = order.Fund;
                
            }
            if (dgViewer.Rows.Count != 0)
            {
                if (dgViewer.SelectedRows.Count == 0)
                {
                    dgViewer.Rows[0].Selected = true;
                }
            }
        }


        private void ShowBooks()
        {
            this.Text = "Все книги текущего зала.";
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "NN", "№№"),
                new KeyValuePair<string, string> ( "author", "Автор"),
                new KeyValuePair<string, string> ( "title", "Заглавие"),
                new KeyValuePair<string, string> ( "inventoryNumber", "Инвентарный номер"),
                new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                new KeyValuePair<string, string> ( "rack", "Полка"),
                new KeyValuePair<string, string> ( "db", "Фонд БД"),
                new KeyValuePair<string, string> ( "lang", "Язык"),
                new KeyValuePair<string, string> ( "tema", "Тематика"),
                new KeyValuePair<string, string> ( "status", "Статус"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);
            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgViewer.Columns["NN"].Width = 80;
            dgViewer.Columns["author"].Width = 250;
            dgViewer.Columns["title"].Width = 400;
            dgViewer.Columns["inventoryNumber"].Width = 130;
            dgViewer.Columns["bar"].Width = 130;
            dgViewer.Columns["rack"].Width = 200;
            dgViewer.Columns["db"].Width = 50;
            dgViewer.Columns["lang"].Width = 80;
            dgViewer.Columns["tema"].Width = 120;
            dgViewer.Columns["status"].Width = 80;


            int i = 1;
            foreach (BookExemplarBase exemplar in exemplars_)
            {
                dgViewer.Rows.Add();
                var row = dgViewer.Rows[dgViewer.Rows.Count - 1];


                if (exemplar is BJExemplarInfo)
                {
                    BJExemplarInfo bjExemplar = (BJExemplarInfo)exemplar;
                    BJBookInfo bjBook = (BJBookInfo)BookFactory.CreateBook(bjExemplar.IDMAIN, bjExemplar.Fund);
                    row.Cells["NN"].Value = i++;
                    row.Cells["author"].Value = bjExemplar.Author(); //bjBook.Fields["700$a"].ToString();
                    row.Cells["title"].Value = bjExemplar.Title();//bjBook.Fields["200$a"].ToString();
                    row.Cells["inventoryNumber"].Value = bjExemplar.Fields["899$p"].ToString();
                    row.Cells["bar"].Value = bjExemplar.Fields["899$w"].ToString();
                    row.Cells["rack"].Value = bjExemplar.Fields["899$c"].ToString();
                    row.Cells["db"].Value = bjExemplar.Fund;
                    row.Cells["lang"].Value = bjBook.Fields["101$a"].ToString();
                    row.Cells["tema"].Value = bjBook.Fields["922$e"].ToString();
                    CirculationInfo ci = new CirculationInfo();
                    OrderInfo order = ci.GetLastOrder(bjExemplar.IdData, bjExemplar.Fund);
                    row.Cells["status"].Value = (order == null ||
                                                 order.StatusName.In(CirculationStatuses.Finished.Value, 
                                                                     CirculationStatuses.SelfOrder.Value,
                                                                     CirculationStatuses.Refusual.Value))? "Свободно" : order.StatusName;
                }
            }
            autoNumber();
        }

        private void FillStatuses()
        {
            label1.Visible = true;
            cbStatuses.Visible = true;
            cbStatuses.Items.Add(CirculationStatuses.IssuedInHall.Value);
            cbStatuses.Items.Add(CirculationStatuses.EmployeeLookingForBook.Value);
            cbStatuses.Items.Add(CirculationStatuses.ForReturnToBookStorage.Value);
            cbStatuses.Items.Add(CirculationStatuses.InReserve.Value);
            cbStatuses.Items.Add(CirculationStatuses.IssuedAtHome.Value);
            cbStatuses.Items.Add(CirculationStatuses.OrderIsFormed.Value);
            cbStatuses.Items.Add(CirculationStatuses.Refusual.Value);
            cbStatuses.Items.Add(CirculationStatuses.SelfOrder.Value);
            cbStatuses.Items.Add(CirculationStatuses.WaitingFirstIssue.Value);
            cbStatuses.SelectedItem = CirculationStatuses.IssuedInHall.Value;
        }

        private void ShowOrders(string statusNameToShow)
        {
            this.Text = (statusNameToShow == CirculationStatuses.Finished.Value)? "Завершённые заказы" : $"Активные заказы зала";

            List<OrderInfo> ordersToShow = orders_.FindAll(x => x.StatusName == statusNameToShow);
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "readerId", "Читатель"),
                new KeyValuePair<string, string> ( "inventoryNumber", "Инвентарный номер"),
                new KeyValuePair<string, string> ( "author", "Автор"),
                new KeyValuePair<string, string> ( "title", "Заглавие"),
                new KeyValuePair<string, string> ( "IssueHall", "Зал выдачи"),
                new KeyValuePair<string, string> ( "location", "Местонахождение"),
                new KeyValuePair<string, string> ( "IssueDate", "Дата выдачи"),
                new KeyValuePair<string, string> ( "FactReturnDate", "Фактическая дата возврата"),
                new KeyValuePair<string, string> ( "cipher", "Расст. шифр"),
                new KeyValuePair<string, string> ( "bar", "Штрихкод"),
                new KeyValuePair<string, string> ( "ReturnDate", "Предполагаемая дата возврата"),
                new KeyValuePair<string, string> ( "ReturnHall", "Зал возврата"),
                new KeyValuePair<string, string> ( "orderId" , "Номер заказа" ),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);

            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            //dgViewer.Columns["id"].Visible = false;

            dgViewer.Columns["orderId"].Width = 70;
            dgViewer.Columns["author"].Width = 120;
            dgViewer.Columns["title"].Width = 250;
            dgViewer.Columns["IssueHall"].Width = 140;
            dgViewer.Columns["ReturnHall"].Width = 140;
            dgViewer.Columns["IssueDate"].Width = 80;
            dgViewer.Columns["IssueDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgViewer.Columns["ReturnDate"].Width = 80;
            dgViewer.Columns["ReturnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgViewer.Columns["FactReturnDate"].Width = 80;
            dgViewer.Columns["FactReturnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgViewer.Columns["inventoryNumber"].Width = 80;
            dgViewer.Columns["bar"].Width = 70;
            dgViewer.Columns["location"].Width = 150;
            dgViewer.Columns["readerId"].Width = 70;

            foreach (OrderInfo order in ordersToShow)
            {
                dgViewer.Rows.Add();
                var row = dgViewer.Rows[dgViewer.Rows.Count - 1];
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                BJBookInfo book = BJBookInfo.GetBookInfoByPIN(exemplar.IDMAIN, exemplar.Fund);
                row.Cells["orderId"].Value = order.OrderId;
                row.Cells["bar"].Value = exemplar.Fields["899$w"].ToString();
                row.Cells["inventoryNumber"].Value = exemplar.Fields["899$p"].ToString();
                row.Cells["author"].Value = book.Fields["700$a"].ToString();
                row.Cells["title"].Value = book.Fields["200$a"].ToString();
                row.Cells["IssueDate"].Value = order.IssueDate;
                row.Cells["ReturnDate"].Value = order.ReturnDate;
                row.Cells["cipher"].Value = exemplar.Cipher;
                //row.Cells["baseName"].Value = exemplar.Fund;
                //row.Cells["status"].Value = order.StatusName;
                //row.Cells["rack"].Value = exemplar.Fields["899$c"].ToString();
                row.Cells["IssueHall"].Value = string.IsNullOrEmpty(order.IssueDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(order.IssueDep)];
                row.Cells["ReturnHall"].Value = string.IsNullOrEmpty(order.ReturnDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(order.ReturnDep)];
                row.Cells["location"].Value = exemplar.Fields["899$a"].ToString();
                row.Cells["readerId"].Value = order.ReaderId;
                row.Cells["orderId"].Value = order.OrderId;
            }
        }
    

        private void HallService()
        {
            this.Text = $"Обслуживание в залах за период с {dp_.StartDate.ToString("dd.MM.yyyy")} по {dp_.EndDate.ToString("dd.MM.yyyy")}";

            int booksIssuedFromHallCount = csm_.GetBooksIssuedFromHallCount(dp_.StartDate, dp_.EndDate, bjUser_);
            int booksIssuedFromBookKeepingCount = csm_.GetBooksIssuedFromBookkeepingCount(dp_.StartDate, dp_.EndDate, bjUser_);
            int hallAttendance = csm_.GetAttendance(dp_.StartDate, dp_.EndDate, bjUser_);
            int readersRecievedBookCount = csm_.GetReadersRecievedBookCount(dp_.StartDate, dp_.EndDate, bjUser_);

            //int BooksIssuedFromReserveCount = csm_.GetBooksIssuedFromReserveCount(dp_.StartDate, dp_.EndDate, bjUser_);

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "id" , "п.п." ),
                new KeyValuePair<string, string> ( "name", "Наименование показателя"),
                new KeyValuePair<string, string> ( "value", "Значение показателя"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);

            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgViewer.Columns["id"].Width = 50;
            dgViewer.Columns["name"].Width = 200;
            dgViewer.Columns["value"].Width = 100;

            dgViewer.Rows.Add();
            var row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 1;
            row.Cells["name"].Value = "Выдано из зала";
            row.Cells["value"].Value = booksIssuedFromHallCount;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 2;
            row.Cells["name"].Value = "Выдано из книгохранения";
            row.Cells["value"].Value = booksIssuedFromBookKeepingCount;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 3;
            row.Cells["name"].Value = "Читатели, посетившие зал";
            row.Cells["value"].Value = hallAttendance;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 4;
            row.Cells["name"].Value = "Уникальных читателей, получившие книги";
            row.Cells["value"].Value = readersRecievedBookCount;



        }

        private void cbStatuses_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowOrders(cbStatuses.SelectedItem.ToString());
        }

        private void bSaveToFile_Click(object sender, EventArgs e)
        {
            if (dgViewer.Rows.Count == 0)
            {
                MessageBox.Show("Нечего экспортировать!");
                return;
            }
            DataTable dt = (DataTable)dgViewer.DataSource;
            StringBuilder fileContent = new StringBuilder();
            foreach (DataGridViewColumn dc in dgViewer.Columns)
            {
                fileContent.Append(dc.HeaderText + ";");
            }
            fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);
            foreach (DataGridViewRow dr in dgViewer.Rows)
            {
                foreach (DataGridViewCell cell in dr.Cells)
                {
                    fileContent.Append("\"" + ((cell.Value == null) ? string.Empty : cell.Value.ToString()) + "\";");
                }
                fileContent.Replace(";", System.Environment.NewLine, fileContent.Length - 1, 1);
            }

            //string tmp = this.Text + "_" + DateTime.Now.ToString("hh:mm:ss.nnn") + ".csv";
            string tmp = this.Text + "_" + DateTime.Now.Ticks.ToString() + ".csv";
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Сохранить в файл";
            sd.Filter = "csv files (*.csv)|*.csv";
            sd.FilterIndex = 1;
            sd.FileName = tmp;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, fileContent.ToString(), Encoding.UTF8);
            }
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgViewer_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.rt_ == ReferenceType.AllBooksInHall)
            {
                autoNumber();
            }
        }

        private void autoNumber()
        {
            int i = 1;
            foreach(DataGridViewRow row in dgViewer.Rows)
            {
                row.Cells["NN"].Value = i++;
            }
        }

        private void bSendEmail_Click(object sender, EventArgs e)
        {
            if (dgViewer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку с должником!");
                return;
            }
            int readerId = (int)dgViewer.SelectedRows[0].Cells["readerId"].Value;

            ReaderInfo reader = ReaderInfo.GetReader(readerId);
            EmailSending es = new EmailSending(reader, bjUser_);
            es.ShowDialog();

        }

        private void dgViewer_SelectionChanged(object sender, EventArgs e)
        {
            if (this.Text != "Список нарушителей")
            {
                return;
            }
            CirculationInfo ci = new CirculationInfo();
            ReaderInfo reader = ReaderInfo.GetReader((int)dgViewer.SelectedRows[0].Cells["readerId"].Value);
            DateTime? lastEmail = ci.GetLastEmailDate(reader);
            lInfo.Text = (lastEmail.HasValue)? $"Дата последней отправки Email: {lastEmail.Value.ToString("dd.MM.yyyy hh:mm")}"
                                                : "Email не отправлялся";
        }
    }
}
