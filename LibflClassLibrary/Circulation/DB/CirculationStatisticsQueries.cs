using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationStatisticsQueries
    {
        public string GET_BOOKS_ISSUED_FROM_HALL_COUNT
        {
            get
            {
                return "select * from Circulation..OrdersFlow A " +
                       " left join Circulation..Orders B on A.OrderId = B.ID " +
                       " left join BJVVV..DATAEXT C on C.IDDATA = B.ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                       " where cast(cast(A.Changed as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                       " and DepartmentId = @unifiedLocationCode and C.IDINLIST = @depId " +
                       " and A.StatusName in ('"+CirculationStatuses.IssuedAtHome.Value+"', '"+ CirculationStatuses.IssuedInHall.Value + "') " +
                       " order by OrderId";

            }
        }

        public string GET_BOOKS_ISSUED_FROM_BOOKKEPING_COUNT
        {
            get
            {
                return "select * from Circulation..OrdersFlow A " +
                       " left join Circulation..Orders B on A.OrderId = B.ID " +
                       " left join BJVVV..DATAEXT C on C.IDDATA = B.ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                       " where cast(cast(A.Changed as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                       " and DepartmentId = @unifiedLocationCode and C.IDINLIST in (15,6,7,8,9,10,11,47,31,13,79,46) " + //всё хранение
                       " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                       " order by OrderId";

            }
        }

        public string GET_HALL_ATTENDANCE
        {
            get
            {
                return "select * from Circulation..Attendance A " +
                       " where cast(cast(A.AttendanceDate as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                       " and DepId = @unifiedLocationCode ";

            }
        }

        public string GET_ACTIVE_HALL_ORDERS
        {
            get
            {
                return " select A.*, B.Refusual from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @statusRefusual" +
                       " where (IssuingDepId = @unifiedLocationCode and  ReturnDepId is null or ReturnDepId = @unifiedLocationCode) and A.StatusName != 'Завершено' " +
                       " order by ID";

            }
        }
        public string GET_FINISHED_HALL_ORDERS
        {
            get
            {
                return " select A.*, B.Refusual from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @statusRefusual" +
                       " where (IssuingDepId = @unifiedLocationCode and  ReturnDepId is null or ReturnDepId = @unifiedLocationCode) and A.StatusName = 'Завершено' " +
                       " order by ID";

            }
        }

    }
}
