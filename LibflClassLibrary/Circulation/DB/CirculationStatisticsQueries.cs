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

        //это полный ПЭ. как тут учитывать все базы? каааааааааааааааааааааааааааааааааааааааак
        public string GET_BOOKS_ISSUED_FROM_HALL_COUNT
        {
            get
            {
                //return "select * from Circulation..OrdersFlow A " +
                //       " left join Circulation..Orders B on A.OrderId = B.ID " +
                //       " left join BJVVV..DATAEXT C on C.IDDATA = B.ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                //       " where cast(cast(A.Changed as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                //       " and DepartmentId = @unifiedLocationCode and C.SORT not like '%нигохранени%' " +
                //       " and A.StatusName in ('"+CirculationStatuses.IssuedAtHome.Value+"', '"+ CirculationStatuses.IssuedInHall.Value + "') " +
                //       " order by OrderId";
                return
                    "with F0 as ( " +
                    "select B.ID orderId, B.Fund fund, B.ExemplarId,A.Id " +
                    "from Circulation..OrdersFlow A " +
                    "left join Circulation..Orders B on A.OrderId = B.ID " +
                    " where cast(cast(A.Changed as varchar(11)) as datetime) between " +
                    " @startDate and @endDate  " +
                    " and DepartmentId = @unifiedLocationCode " +
                    " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                    " ), " +
                    " bases as " +
                    " ( " +
                    "select orderId, fund, C.SORT sort from F0 " +
                    "left join BJVVV..DATAEXT C on C.IDDATA = ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' and fund = 'BJVVV' " +
                    "union all " +
                    "select orderId, fund, D.SORT sort from F0 " +
                    "left join REDKOSTJ..DATAEXT D on D.IDDATA = ExemplarId and D.MNFIELD = 899 and D.MSFIELD = '$a' and fund = 'REDKOSTJ' " +
                    "union all " +
                    "select orderId, fund, E.SORT sort from F0 " +
                    "left join BJFCC..DATAEXT E on E.IDDATA = ExemplarId and E.MNFIELD = 899 and E.MSFIELD = '$a' and fund = 'BJFCC' " +
                    "union all " +
                    "select orderId, fund, F.SORT collate cyrillic_general_ci_ai sort from F0 " +
                    "left join BJACC..DATAEXT F on F.IDDATA = ExemplarId and F.MNFIELD = 899 and F.MSFIELD = '$a' and fund = 'BJACC' " +
                    "union all " +
                    "select orderId, fund, G.SORT sort from F0 " +
                    "left join BJSCC..DATAEXT G on G.IDDATA = ExemplarId and G.MNFIELD = 899 and G.MSFIELD = '$a' and fund = 'BJSCC' " +
                    ") " +
                    "select* from F0 " +
                    "left join bases on F0.orderId = bases.orderId " +
                    "where bases.sort is not null " +
                    "and bases.sort not like '%нигохранени%' ";
            }
        }

        public string GET_BOOKS_ISSUED_FROM_BOOKKEPING_COUNT
        {
            get
            {
                //return "select * from Circulation..OrdersFlow A " +
                //       " left join Circulation..Orders B on A.OrderId = B.ID " +
                //       " left join BJVVV..DATAEXT C on C.IDDATA = B.ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                //       " where cast(cast(A.Changed as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                //       " and DepartmentId = @unifiedLocationCode and C.IDINLIST in (15,6,7,8,9,10,11,47,31,13,79,46) " + //всё хранение
                //       " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                //       " order by OrderId";
                return
                    "with F0 as ( " +
                    "select B.ID orderId, B.Fund fund, B.ExemplarId,A.Id " +
                    "from Circulation..OrdersFlow A " +
                    "left join Circulation..Orders B on A.OrderId = B.ID " +
                    " where cast(cast(A.Changed as varchar(11)) as datetime) between " +
                    " @startDate and @endDate  " +
                    " and DepartmentId = @unifiedLocationCode " +
                    " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                    " ), " +
                    " bases as " +
                    " ( " +
                    "select orderId, fund, C.SORT sort from F0 " +
                    "left join BJVVV..DATAEXT C on C.IDDATA = ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' and fund = 'BJVVV' " +
                    "union all " +
                    "select orderId, fund, D.SORT sort from F0 " +
                    "left join REDKOSTJ..DATAEXT D on D.IDDATA = ExemplarId and D.MNFIELD = 899 and D.MSFIELD = '$a' and fund = 'REDKOSTJ' " +
                    "union all " +
                    "select orderId, fund, E.SORT sort from F0 " +
                    "left join BJFCC..DATAEXT E on E.IDDATA = ExemplarId and E.MNFIELD = 899 and E.MSFIELD = '$a' and fund = 'BJFCC' " +
                    "union all " +
                    "select orderId, fund, F.SORT collate cyrillic_general_ci_ai sort from F0 " +
                    "left join BJACC..DATAEXT F on F.IDDATA = ExemplarId and F.MNFIELD = 899 and F.MSFIELD = '$a' and fund = 'BJACC' " +
                    "union all " +
                    "select orderId, fund, G.SORT sort from F0 " +
                    "left join BJSCC..DATAEXT G on G.IDDATA = ExemplarId and G.MNFIELD = 899 and G.MSFIELD = '$a' and fund = 'BJSCC' " +
                    ") " +
                    "select* from F0 " +
                    "left join bases on F0.orderId = bases.orderId " +
                    "where bases.sort is not null " +
                    "and bases.sort  like '%нигохранени%' ";
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

        public string GET_ALL_BOOKS_IN_HALL
        {
            get
            {
                return  " select * from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                        " where MNFIELD = =899 and MSFIELD = '$a' and IDINLIST = @depId";
            }
        }

        public string GET_READERS_RECIEVED_BOOKS_COUNT
        {
            get
            {
                return "select distinct B.ReaderId from Circulation..OrdersFlow A " +
                       " left join Circulation..Orders B on A.OrderId = B.ID " +
                       " where cast(cast(A.Changed as varchar(11)) as datetime) between cast(cast(@startDate as varchar(11)) as datetime) and cast(cast(@endDate as varchar(11)) as datetime) " +
                       " and DepartmentId = @unifiedLocationCode " +
                       " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') ";
            }
        }
    }
}
