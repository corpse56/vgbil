using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using System;
using System.Collections.Generic;
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

        public List<BookBase> GetAllBooksInHall(BJUserInfo bjUser)
        {
            //здесь написать условия 
            return csl_.GetAllBooksInHall(bjUser);
        }

        public int GetReadersRecievedBookCount(DateTime startDate, DateTime endDate, BJUserInfo bjUser)
        {
            return csl_.GetReadersRecievedBookCount(startDate, endDate, bjUser);

        }
    }
}
