using LibflClassLibrary.BJUsers;
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
    }
}
