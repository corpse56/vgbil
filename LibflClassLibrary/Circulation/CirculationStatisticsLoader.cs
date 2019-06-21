using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Circulation.DB;
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
    }
}
