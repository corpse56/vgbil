using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
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
    public partial class fInvNumberOrdersHistory : Form
    {
        public fInvNumberOrdersHistory()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CirculationInfo ci = new CirculationInfo();
            OrderInfo selectedOrder = ci.GetOrder(Convert.ToInt32(lbOrders.SelectedItem));
            List<OrderFlowInfo> flow = ci.GetOrdersFlowByOrderId(selectedOrder.OrderId);

            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "date", "Дата"),
                new KeyValuePair<string, string> ( "status", "Действие"),
                new KeyValuePair<string, string> ( "employee", "Сотрудник"),
                new KeyValuePair<string, string> ( "dep", "Отдел"),
                new KeyValuePair<string, string> ( "reader", "Читатель"),
            };
            dgOrderFlow.Columns.Clear();
            foreach (var c in columns)
                dgOrderFlow.Columns.Add(c.Key, c.Value);

            dgOrderFlow.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgOrderFlow.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgOrderFlow.Columns["date"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            dgOrderFlow.Columns["status"].Width = 180;
            dgOrderFlow.Columns["employee"].Width = 100;
            dgOrderFlow.Columns["dep"].Width = 200;
            dgOrderFlow.Columns["reader"].Width = 80;

            foreach (OrderFlowInfo fi in flow)
            {
                dgOrderFlow.Rows.Add();
                var row = dgOrderFlow.Rows[dgOrderFlow.Rows.Count - 1];
                row.Cells["date"].Value = fi.Changed;
                row.Cells["status"].Value = fi.StatusName;
                BJUserInfo user = BJUserInfo.GetUserById(fi.Changer);
                string emp = (user == null) ? "неизвестно" : user.FIO;
                row.Cells["employee"].Value = emp;
                row.Cells["dep"].Value = KeyValueMapping.LocationCodeToName[fi.DepartmentId];
                row.Cells["reader"].Value = selectedOrder.ReaderId;

            }

        }

        private void bShowHistory_Click(object sender, EventArgs e)
        {
            dgOrderFlow.Rows.Clear();
            lbOrders.Items.Clear();

            //GetBookInfoByInventoryNumber - это полный шлак а не метод... переписать, чтобы сам искал по всех фондах!
            BJBookInfo book = BJBookInfo.GetBookInfoByInventoryNumber(tbInvNumber.Text, "BJVVV");
            if (book == null)
            {
                book = BJBookInfo.GetBookInfoByInventoryNumber(tbInvNumber.Text, "REDKOSTJ");
            }
            if (book == null)
            {
                book = BJBookInfo.GetBookInfoByInventoryNumber(tbInvNumber.Text, "BJACC");
            }
            if (book == null)
            {
                book = BJBookInfo.GetBookInfoByInventoryNumber(tbInvNumber.Text, "BJFCC");
            }
            if (book == null)
            {
                book = BJBookInfo.GetBookInfoByInventoryNumber(tbInvNumber.Text, "BJSCC");
            }
            if (book == null)
            {
                label3.Text = "Не найдено";
                lbOrders.Items.Clear();
                dgOrderFlow.Rows.Clear();
                MessageBox.Show("Инвентарный номер не найден ни в одной базе!");
                return;
            }

            BJExemplarInfo exemplar = (BJExemplarInfo)book.Exemplars.Find(x=> ((BJExemplarInfo)x).Fields["899$p"].ToString() == tbInvNumber.Text);
            if (exemplar == null)
            {
                label3.Text = "Не найдено";
                lbOrders.Items.Clear();
                dgOrderFlow.Rows.Clear();
                MessageBox.Show("Инвентарный номер не найден ни в одной базе!");
                return;
            }
            else
            {

                label3.Text = book.Fields["200$a"].ToString();
                CirculationInfo ci = new CirculationInfo();
                List<OrderInfo> orders = ci.GetOrders(exemplar.IdData, exemplar.Fund);
                lbOrders.Items.Clear();
                foreach (OrderInfo order in orders)
                {
                    lbOrders.Items.Add(order.OrderId);
                }
                if (lbOrders.Items.Count != 0)
                {
                    lbOrders.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Инвентарный номер найден, но заказов на него не было.");
                }
            }


        }
    }
}
