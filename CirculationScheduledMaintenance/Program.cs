using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Circulation;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CirculationScheduledMaintenance
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            TermOfReturnElectronicCopy();


        }

        private static void TermOfReturnElectronicCopy()
        {
            logger.Info($"Начало ежедневного автоматического завершения заказов электронных копий по истечении срока сдачи. {DateTime.Now.ToString("dd.MM.yyyy")}");
            CirculationInfo ci = new CirculationInfo();
            List<OrderInfo> electronicOrders = ci.GetOverdueOrders(CirculationStatuses.ElectronicIssue.Value);
            BJUserInfo bjUser = new BJUserInfo();
            bjUser = BJUserInfo.GetUserByLogin("station1", "BJVVV");
            bjUser.SelectedUserStatus = bjUser.UserStatus[0];
            foreach (var order in electronicOrders)
            {
                ci.ChangeOrderStatusReturn(bjUser, order.OrderId, CirculationStatuses.Finished.Value);
            }
            Console.WriteLine($"Автоматически успешно завершено {electronicOrders.Count} заказов на электронные копии из-за просрочки даты возврата.");
            logger.Info($"Автоматически успешно завершено {electronicOrders.Count} заказов на электронные копии из-за просрочки даты возврата.");
        }
    }
}
