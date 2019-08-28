﻿using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
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

namespace CirculationApp
{
    public enum ReferenceType { HallService, ActiveHallOrders, FinishedHallOrders, AllBooksInHall }

    public partial class TableDataVisualizer : Form
    {
        private fDatePeriod dp_;
        private CirculationStatisticsManager csm_ = new CirculationStatisticsManager();
        private BJUserInfo bjUser_;
        private List<OrderInfo> orders_;
        private List<BookBase> books_;


        public TableDataVisualizer()
        {
            InitializeComponent();
        }

        public TableDataVisualizer(fDatePeriod dp, BJUserInfo bjUser, ReferenceType rt )
        {
            this.dp_ = dp;
            bjUser_ = bjUser;
            InitializeComponent();

            switch (rt)
            {
                case ReferenceType.HallService:
                    HallService();
                    break;
                case ReferenceType.ActiveHallOrders:
                    orders_ = csm_.GetActiveHallOrders(bjUser_);
                    FillStatuses();
                    ShowOrders(CirculationStatuses.IssuedInHall.Value);
                    break;
                case ReferenceType.FinishedHallOrders:
                    orders_ = csm_.GetFinishedHallOrders(bjUser_);
                    ShowOrders(CirculationStatuses.Finished.Value);
                    break;
                case ReferenceType.AllBooksInHall:
                    books_ = csm_.GetAllBooksInHall(bjUser_);
                    break;
            }
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
                    fileContent.Append("\"" + cell.Value.ToString() + "\";");
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
    }
}