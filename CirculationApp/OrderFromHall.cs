using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CirculationApp
{
    public partial class OrderFromHall : Form
    {
        BJUserInfo bjUser_;
        public OrderFromHall(BJUserInfo bjUser)
        {
            bjUser_ = bjUser;
            InitializeComponent();
        }
        private CirculationStatisticsManager csm_ = new CirculationStatisticsManager();

        private void OrderFromHall_Load(object sender, EventArgs e)
        {

            List<OrderInfo> orders_ = csm_.GetTodaySelfOrders(bjUser_);

            List<OrderInfo> ordersToShow = orders_.FindAll(x => x.StatusName == CirculationStatuses.SelfOrder.Value);
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
            dgvHallOrders.Columns.Clear();
            foreach (var c in columns)
                dgvHallOrders.Columns.Add(c.Key, c.Value);

            dgvHallOrders.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvHallOrders.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            //dgvHallOrders.Columns["id"].Visible = false;

            dgvHallOrders.Columns["orderId"].Width = 70;
            dgvHallOrders.Columns["author"].Width = 120;
            dgvHallOrders.Columns["title"].Width = 250;
            dgvHallOrders.Columns["IssueHall"].Width = 140;
            dgvHallOrders.Columns["ReturnHall"].Width = 140;
            dgvHallOrders.Columns["IssueDate"].Width = 80;
            dgvHallOrders.Columns["IssueDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvHallOrders.Columns["ReturnDate"].Width = 80;
            dgvHallOrders.Columns["ReturnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvHallOrders.Columns["FactReturnDate"].Width = 80;
            dgvHallOrders.Columns["FactReturnDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgvHallOrders.Columns["inventoryNumber"].Width = 80;
            dgvHallOrders.Columns["bar"].Width = 70;
            dgvHallOrders.Columns["location"].Width = 150;
            dgvHallOrders.Columns["readerId"].Width = 70;

            foreach (OrderInfo order in ordersToShow)
            {
                dgvHallOrders.Rows.Add();
                var row = dgvHallOrders.Rows[dgvHallOrders.Rows.Count - 1];
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);
                //экземпляр может быть нулл... надо исправлять.
                row.Cells["orderId"].Value = order.OrderId;
                row.Cells["bar"].Value = exemplar.Bar;
                row.Cells["inventoryNumber"].Value = exemplar.InventoryNumber;
                row.Cells["author"].Value = exemplar.Author;
                row.Cells["title"].Value = exemplar.Title;
                row.Cells["IssueDate"].Value = order.IssueDate;
                row.Cells["ReturnDate"].Value = order.ReturnDate;
                row.Cells["cipher"].Value = exemplar.Cipher;
                //row.Cells["baseName"].Value = exemplar.Fund;
                //row.Cells["status"].Value = order.StatusName;
                //row.Cells["rack"].Value = exemplar.Fields["899$c"].ToString();
                row.Cells["IssueHall"].Value = string.IsNullOrEmpty(order.IssueDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(order.IssueDep)];
                row.Cells["ReturnHall"].Value = string.IsNullOrEmpty(order.ReturnDep) ? "" : KeyValueMapping.LocationCodeToName[int.Parse(order.ReturnDep)];
                row.Cells["location"].Value = exemplar.Location;
                row.Cells["readerId"].Value = order.ReaderId;
                row.Cells["orderId"].Value = order.OrderId;
            }

        }
    }
}
