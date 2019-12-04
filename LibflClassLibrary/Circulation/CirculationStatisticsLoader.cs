using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
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
        internal List<OrderInfo> GetDebtorsInHall(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetDebtorsInHall(bjUser.SelectedUserStatus.UnifiedLocationCode);
            List<OrderInfo> orders = new List<OrderInfo>();
            CirculationLoader cl = new CirculationLoader();
            foreach (DataRow row in table.Rows)
            {
                OrderInfo order = cl.FillOrderFromDataRow(row);
                orders.Add(order);
            }
            return orders;
        }

        internal List<ExemplarBase> GetAllBooksInHallACC(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHallACC(bjUser.SelectedUserStatus.DepId);
            List<ExemplarBase> result = GetExemplarListFromPinList(table);
            return result;
        }

        internal List<ExemplarBase> GetAllBooksInHallFCC(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHallFCC(bjUser.SelectedUserStatus.DepId);
            List<ExemplarBase> result = GetExemplarListFromPinList(table);
            return result;
        }

        internal List<ExemplarBase> GetAllBooksInHallREDKOSTJ(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHallREDKOSTJ(bjUser.SelectedUserStatus.DepId);
            List<ExemplarBase> result = GetExemplarListFromPinList(table);
            return result;
        }

        internal DataTable GetOrdersCountBySubject(BJUserInfo bjUser, DateTime startDate, DateTime endDate)
        {
            return dbWrapper_.GetOrdersCountBySubject(bjUser.SelectedUserStatus.UnifiedLocationCode, startDate, endDate);
        }

        internal int RegisteredReadersRemoteCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = dbWrapper_.RegisteredReadersRemoteCount(startDate, endDate);
            return table.Rows.Count;
        }

        internal int LitresAccountAssignedCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = dbWrapper_.LitresAccountAssignedCount(startDate, endDate);
            return table.Rows.Count;
        }

        internal int RegisteredReadersAllCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = dbWrapper_.RegisteredReadersAllCount(startDate, endDate);
            return table.Rows.Count;
        }

        internal List<ExemplarBase> GetAllBooksInHallSCC(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHallSCC(bjUser.SelectedUserStatus.DepId);
            List<ExemplarBase> result = GetExemplarListFromPinList(table);
            return result;
        }
        internal List<ExemplarBase> GetAllBooksInHall(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetAllBooksInHall(bjUser.SelectedUserStatus.DepId);
            List<ExemplarBase> result = GetExemplarListFromPinList(table);
            return result;
        }

        private List<ExemplarBase> GetExemplarListFromPinList(DataTable table)
        {
            List<ExemplarBase> result = new List<ExemplarBase>();
            foreach (DataRow row in table.Rows)
            {
                ExemplarBase exemplar = ExemplarFactory.CreateExemplar(Convert.ToInt32(row["exemplarId"]), row["fund"].ToString());
                result.Add(exemplar);
            }
            return result;
        }
        private List<BookBase> GetBookListFromPinList(DataTable table)
        {
            List<BookBase> result = new List<BookBase>();
            foreach (DataRow row in table.Rows)
            {
                BookBase book = BookFactory.CreateBook(Convert.ToInt32(row["pin"]), row["fund"].ToString());
                result.Add(book);
            }
            return result;
        }
        internal int GetReadersRecievedBookCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper_.GetReadersRecievedBookCount(startDate, endDate, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return table.Rows.Count;
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
