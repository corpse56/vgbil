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
                    "select B.ID orderId, B.Fund fund, B.ExemplarId,A.Id flowId " +
                    "from Circulation..OrdersFlow A " +
                    "left join Circulation..Orders B on A.OrderId = B.ID " +
                    " where cast(cast(A.Changed as varchar(11)) as datetime) between " +
                    " @startDate and @endDate  " +
                    " and DepartmentId = @unifiedLocationCode " +
                    " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                    " ), " +
                    " bases as " +
                    " ( " +
                    "select orderId, flowId, fund, C.SORT sort from F0 " +
                    "left join BJVVV..DATAEXT C on C.IDDATA = ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' and fund = 'BJVVV' " +
                    "union all " +
                    "select orderId, flowId, fund, D.SORT sort from F0 " +
                    "left join REDKOSTJ..DATAEXT D on D.IDDATA = ExemplarId and D.MNFIELD = 899 and D.MSFIELD = '$a' and fund = 'REDKOSTJ' " +
                    "union all " +
                    "select orderId, flowId, fund, E.SORT sort from F0 " +
                    "left join BJFCC..DATAEXT E on E.IDDATA = ExemplarId and E.MNFIELD = 899 and E.MSFIELD = '$a' and fund = 'BJFCC' " +
                    "union all " +
                    "select orderId, flowId, fund, F.SORT collate cyrillic_general_ci_ai sort from F0 " +
                    "left join BJACC..DATAEXT F on F.IDDATA = ExemplarId and F.MNFIELD = 899 and F.MSFIELD = '$a' and fund = 'BJACC' " +
                    "union all " +
                    "select orderId, flowId, fund, G.SORT sort from F0 " +
                    "left join BJSCC..DATAEXT G on G.IDDATA = ExemplarId and G.MNFIELD = 899 and G.MSFIELD = '$a' and fund = 'BJSCC' " +
                    ") " +
                    "select* from F0 " +
                    "left join bases on F0.flowId = bases.flowId " +
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
                    "select B.ID orderId, B.Fund fund, B.ExemplarId,A.Id flowId " +
                    "from Circulation..OrdersFlow A " +
                    "left join Circulation..Orders B on A.OrderId = B.ID " +
                    " where cast(cast(A.Changed as varchar(11)) as datetime) between " +
                    " @startDate and @endDate  " +
                    " and DepartmentId = @unifiedLocationCode " +
                    " and A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "', '" + CirculationStatuses.IssuedInHall.Value + "') " +
                    " ), " +
                    " bases as " +
                    " ( " +
                    "select orderId, flowId, fund, C.SORT sort from F0 " +
                    "left join BJVVV..DATAEXT C on C.IDDATA = ExemplarId and C.MNFIELD = 899 and C.MSFIELD = '$a' and fund = 'BJVVV' " +
                    "union all " +
                    "select orderId, flowId, fund, D.SORT sort from F0 " +
                    "left join REDKOSTJ..DATAEXT D on D.IDDATA = ExemplarId and D.MNFIELD = 899 and D.MSFIELD = '$a' and fund = 'REDKOSTJ' " +
                    "union all " +
                    "select orderId, flowId, fund, E.SORT sort from F0 " +
                    "left join BJFCC..DATAEXT E on E.IDDATA = ExemplarId and E.MNFIELD = 899 and E.MSFIELD = '$a' and fund = 'BJFCC' " +
                    "union all " +
                    "select orderId, flowId, fund, F.SORT collate cyrillic_general_ci_ai sort from F0 " +
                    "left join BJACC..DATAEXT F on F.IDDATA = ExemplarId and F.MNFIELD = 899 and F.MSFIELD = '$a' and fund = 'BJACC' " +
                    "union all " +
                    "select orderId, flowId, fund, G.SORT sort from F0 " +
                    "left join BJSCC..DATAEXT G on G.IDDATA = ExemplarId and G.MNFIELD = 899 and G.MSFIELD = '$a' and fund = 'BJSCC' " +
                    ") " +
                    "select* from F0 " +
                    "left join bases on F0.flowId = bases.flowId " +
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
        public string GET_DEBTORS_IN_HALL
        {
            get
            {
                return " select A.*, B.Refusual from Circulation..Orders A " +
                       " left join Circulation..OrdersFlow B on A.ID = B.OrderId and B.StatusName = @statusRefusual" +
                       " where (IssuingDepId = @unifiedLocationCode and  ReturnDepId is null or ReturnDepId = @unifiedLocationCode) " +
                       " and A.StatusName not in ('Завершено','Самостоятельный заказ','Электронная выдача', 'Для возврата в хранение') " + 
                       " and cast(cast(ReturnDate as varchar(11)) as datetime) < cast(cast(getDate() as varchar(11)) as datetime)" +
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
                return " select 'BJVVV' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJVVV..DATAEXT A " +
                        //" left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = @depId";
            }
        }
        public string GET_ALL_BOOKS_IN_HALL_ACC
        {
            get
            {
                return " select 'BJVVV' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJVVV..DATAEXT A " +
                        //" left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = @depId " +
                        " union all" +
                        " select 'BJACC' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJACC..DATAEXT A " +
                        " where MNFIELD = 899 and MSFIELD = '$w' ";
            }
        }
        public string GET_ALL_BOOKS_IN_HALL_FCC
        {
            get
            {
                return " select 'BJVVV' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJVVV..DATAEXT A " +
                        //" left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = @depId " +
                        " union all" +
                        " select 'BJFCC' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJFCC..DATAEXT A " +
                        " where MNFIELD = 899 and MSFIELD = '$p' ";
            }
        }
        public string GET_ALL_BOOKS_IN_HALL_SCC
        {
            get
            {
                return " select 'BJVVV' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJVVV..DATAEXT A " +
                        //" left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = @depId " +
                        " union all" +
                        " select 'BJSCC' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJSCC..DATAEXT A " +
                        " where MNFIELD = 899 and MSFIELD = '$p' ";
            }
        }
        public string GET_ALL_BOOKS_IN_HALL_REDKOSTJ
        {
            get
            {
                return " select 'BJVVV' fund, A.IDMAIN pin, A.IDDATA exemplarId from BJVVV..DATAEXT A " +
                        //" left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$p' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = @depId " +
                        " union all" +
                        " select 'REDKOSTJ' fund, A.IDMAIN pin, A.IDDATA exemplarId from REDKOSTJ..DATAEXT A " +
                        //" left join REDKOSTJ..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$p' " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST in (20,21)";
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

        public string REGISTERED_READERS_ALL_COUNT
        {
            get
            {
                return " select 1 from Readers..Main where cast(cast(DateRegistration as varchar(11)) as datetime) between @startDate and @endDate ";
            }
        }

        public string REGISTERED_READERS_REMOTE_COUNT
        {
            get
            {
                return " select 1 from Readers..Main where cast(cast(DateRegistration as varchar(11)) as datetime) between @startDate and @endDate and TypeReader = 1 ";
            }
        }

        public string LITRES_ACCOUNT_ASSIGNED_COUNT
        {
            get
            {
                return " select 1 from LITRES..ACCOUNTS where cast(cast(ASSIGNED as varchar(11)) as datetime) between @startDate and @endDate";
            }
        }

        public string GET_ORDERS_COUNT_BY_SUBJECT
        {
            get
            {
                return " with F0 as  " +
                         "( " +
                        "select Id orderId, Fund, SUBSTRING(BookId, charindex('_', BookId, 0) + 1, len(BookId)) idmain " +
                        "from Circulation..Orders A " +
                        "where IssueDate between @startDate and @endDate and " +
                        "(IssueDepId = @unifiedLocationCode or IssuingDepId = @unifiedLocationCode) " +
                        "), " +
                        "F1 as " +
                        "( " +
                        "select orderId, vvvp.PLAIN " +
                        " from F0 A " +
                        "left join BJVVV..DATAEXT vvv on vvv.ID = (select top 1 ID " +
                                                                  "from BJVVV..DATAEXT B " +
                                                                  "where A.idmain = B.IDMAIN and A.Fund = 'BJVVV' and " +
                                                                  "B.MNFIELD = 922 and B.MSFIELD = '$e')  " +
                        "left join BJVVV..DATAEXTPLAIN vvvp on vvv.ID = vvvp.IDDATAEXT " +
                        "union all " +
                        "select orderId, redkp.PLAIN " +
                        "from F0 A " +
                        "left join REDKOSTJ..DATAEXT redk on redk.ID = (select top 1 ID " +
                                                                        "from REDKOSTJ..DATAEXT B " +
                                                                        "where A.idmain = B.IDMAIN and A.Fund = 'REDKOSTJ' and " +
                                                                        "B.MNFIELD = 922 and B.MSFIELD = '$e') " +
                        "left join REDKOSTJ..DATAEXTPLAIN redkp on redk.ID = redkp.IDDATAEXT " +
                        "union all " +
                        "select orderId, accp.PLAIN " +
                        "from F0 A " +
                        "left join BJACC..DATAEXT acc on acc.ID = (select top 1 ID " +
                                                                    "from BJACC..DATAEXT B " +
                                                                    "where A.idmain = B.IDMAIN and A.Fund = 'BJACC' and " +
                                                                    "B.MNFIELD = 922 and B.MSFIELD = '$e') " +
                        "left join BJACC..DATAEXTPLAIN accp on acc.ID = accp.IDDATAEXT " +
                        "union all " +
                        "select orderId, fccp.PLAIN " +
                        "from F0 A " +
                        "left join BJFCC..DATAEXT fcc on fcc.ID = (select top 1 ID " +
                                                                    "from BJFCC..DATAEXT B " +
                                                                    "where A.idmain = B.IDMAIN and A.Fund = 'BJFCC' and " +
                                                                    "B.MNFIELD = 922 and B.MSFIELD = '$e') " +
                        "left join BJFCC..DATAEXTPLAIN fccp on fcc.ID = fccp.IDDATAEXT " +
                        "union all " +
                        "select orderId, sccp.PLAIN collate cyrillic_general_ci_ai " +
                        "from F0 A " +
                        "left join BJSCC..DATAEXT scc on scc.ID = (select top 1 ID " +
                                                                    "from BJSCC..DATAEXT B " +
                                                                    "where A.idmain = B.IDMAIN and A.Fund = 'BJSCC' and " +
                                                                    "B.MNFIELD = 922 and B.MSFIELD = '$e') " +
                        "left join BJSCC..DATAEXTPLAIN sccp on scc.ID = sccp.IDDATAEXT " +
                         ") " +
                         "select case when PLAIN is null then 'тематика не указана в базе' else PLAIN end tema, count(orderId) cnt " +
                         "from F1 " +
                         "group by PLAIN " +
                         "order by cnt desc";
            }
        }

        public string GET_SELF_CHECK_STATION_REFERENCE
        {
            get
            {
                return  " select case when Changer = 938 then 'station1' else 'station2' end st," +
                        " B.BookId, B.ExemplarId, B.Fund " +
                        " from Circulation..OrdersFlow A " +
                        " left join Circulation..Orders B on B.Id = A.OrderId " +
                        " where cast(cast(Changed as varchar(11)) as datetime) between @startDate and @endDate" +
                        " and Changer in (@station1, @station2) and " +
                        " A.StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "','" + CirculationStatuses.IssuedInHall.Value + "')";
                        //" union all "+
                        //" select 'station2', B.BookId, B.ExemplarId, B.Fund " +
                        //" from Circulation..OrdersFlow A " +
                        //" left join Circulation..Orders B on B.Id = A.OrderId " +
                        //" where cast(cast(Changed as varchar(11)) as datetime) between @startDate and @endDate" +
                        //" and Changer = @station2 and StatusName in ('" + CirculationStatuses.IssuedAtHome.Value + "'," +
                        //" " + CirculationStatuses.IssuedInHall.Value + ")";
            }
        }

    }
}
