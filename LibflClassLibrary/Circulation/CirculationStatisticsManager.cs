using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    public class CirculationStatisticsManager
    {
        CirculationStatisticsLoader csl_ = new CirculationStatisticsLoader();

        public int GetBooksIssuedFromHallCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            return csl_.GetBooksIssuedFromHallCount(startDate, endDate, bjUser);
        }

        public int GetBooksIssuedFromBookkeepingCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            return csl_.GetBooksIssuedFromBookkeepingCount(startDate, endDate, bjUser);

        }

        public int GetAttendance(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            return csl_.GetAttendance(startDate, endDate, bjUser);
        }

        public List<OrderInfo> GetActiveHallOrders( BJUserInfo bjUser)
        {
            return csl_.GetActiveHallOrders( bjUser);
        }

        public List<OrderInfo> GetFinishedHallOrders(BJUserInfo bjUser)
        {
            return csl_.GetFinishedHallOrders(bjUser);
        }

        public List<ExemplarBase> GetAllBooksInHall(BJUserInfo bjUser)
        {
            //так как отделы в каждой базе с разными номерами, нужно для каждого отдела, книги которого в разных базах, написать собственные условия
            List<ExemplarBase> result = new List<ExemplarBase>();
            switch (bjUser.SelectedUserStatus.DepId)
            {
                case 52:// "америка":
                    result = csl_.GetAllBooksInHallACC(bjUser);
                    break;
                case 60:// "французск":
                    result = csl_.GetAllBooksInHallFCC(bjUser);
                    break;
                case 61: // "славянски":
                    result = csl_.GetAllBooksInHallSCC(bjUser);
                    break;
                case 20: // "редк":
                    result = csl_.GetAllBooksInHallREDKOSTJ(bjUser);
                    break;
                default:
                    result = csl_.GetAllBooksInHall(bjUser);
                    break;
            }
            return result;
        }

        public List<OrderInfo> GetDebtorsInHall(BJUserInfo bjUser)
        {
            return csl_.GetDebtorsInHall(bjUser);
        }

        public int GetReadersRecievedBookCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            return csl_.GetReadersRecievedBookCount(startDate, endDate, bjUser);

        }

        public DataTable GetOrdersCountBySubject(BJUserInfo bjUser, DateTime startDate, DateTime endDate)
        {
            return csl_.GetOrdersCountBySubject(bjUser, startDate, endDate);
        }

        public DataTable GetSelfCheckStationReference(DateTime startDate, DateTime endDate)
        {
            return csl_.GetSelfCheckStationReference(startDate, endDate);
        }

        public int RegisteredReadersAllCount(DateTime startDate, DateTime endDate)
        {
            return csl_.RegisteredReadersAllCount(startDate, endDate);
        }

        public int RegisteredReadersRemoteCount(DateTime startDate, DateTime endDate)
        {
            return csl_.RegisteredReadersRemoteCount(startDate, endDate);
        }

        public int LitresAccountAssignedCount(DateTime startDate, DateTime endDate)
        {
            return csl_.LitresAccountAssignedCount(startDate, endDate);
        }
    }
}
