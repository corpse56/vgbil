using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Circulation.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace CirculationAppAdmin
{
    public partial class CirculationAdmin : Form
    {
        public CirculationAdmin()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Thanks!");
            CirculationInfo ci = new CirculationInfo();
            List<OrderInfo> list = ci.GetOrders(CirculationStatuses.ForReturnToBookStorage.Value);
            foreach (OrderInfo o in list)
            {
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(o.ExemplarId, o.Fund);

                if (o.IssueDate == null)
                {
                    List<OrderFlowInfo> ofi = ci.GetOrdersFlowByOrderId(o.OrderId);
                    OrderFlowInfo f = ofi.Find(x => x.StatusName.Contains("ыдано"));
                    if (f != null)
                    {

                    }
                }



                if (exemplar.Fields["921$c"].ToString() == "ДП")
                {
                    //ci.ChangeOrderStatusReturn()
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //CirculationInfo ci = new CirculationInfo();
            //BJUserInfo bjUser = new BJUserInfo();
            //bjUser.SelectedUserStatus = new UserStatus();
            //bjUser.SelectedUserStatus.DepId = 
            CirculationDBWrapper cdb = new CirculationDBWrapper();
            //cdb.ChangeOrderStatus(587, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(3300, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(3697, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(6040, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(6048, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(6434, "Завершено", 1, 2033, null);
            //cdb.ChangeOrderStatus(6435, "Завершено", 1, 2033, null);
            cdb.ChangeOrderStatus(1755, "Завершено", 1, 2033, null);
            

        }
    }
}
