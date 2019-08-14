using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Circulation.DB;
using LibflClassLibrary.Circulation.Loaders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    class CirculationStatisticsLoader
    {
        private CirculationStatisticsDBWrapper dbWrapper_ = new CirculationStatisticsDBWrapper();
        internal int GetBooksIssuedFromHallCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetBooksIssuedFromHallCount(startDate, endDate, bjUser.SelectedUserStatus.UnifiedLocationCode, bjUser.SelectedUserStatus.DepId);
            return table.Rows.Count;
        }

        internal int GetBooksIssuedFromBookkeepingCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetBooksIssuedFromBookkeepingCount(startDate, endDate, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return table.Rows.Count;

        }

        internal List<OrderInfo> GetActiveHallOrders(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetActiveHallOrders(bjUser.SelectedUserStatus.UnifiedLocationCode);
            List<OrderInfo> orders = new List<OrderInfo>();
            CirculationLoader cl = new CirculationLoader();
            foreach (DataRow row in table.Rows)
            {
                OrderInfo order = cl.FillOrderFromDataRow(row);
                orders.Add(order);
            }
            return orders;
        }

        internal int GetReadersRecievedBookCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetReadersRecievedBookCount(startDate, endDate, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return table.Rows.Count;
        }

        internal List<BookBase> GetAllBooksInHall(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHall(bjUser.SelectedUserStatus.DepId);



            return new List<BookBase>();
        }

        internal List<OrderInfo> GetFinishedHallOrders(BJUserInfo bjUser_)
        {
            DataTable table = dbWrapper_.GetFinishedHallOrders(bjUser_.SelectedUserStatus.UnifiedLocationCode);
            List<OrderInfo> orders = new List<OrderInfo>();
            CirculationLoader cl = new CirculationLoader();
            foreach (DataRow row in table.Rows)
            {
                OrderInfo order = cl.FillOrderFromDataRow(row);
                orders.Add(order);
            }
            return orders;
        }

        internal int GetAttendance(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAttendance(startDate, endDate, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return table.Rows.Count;
        }
    }
}
