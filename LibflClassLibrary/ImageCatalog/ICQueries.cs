using LibflClassLibrary.Circulation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    class ICQueries
    {
        string tableName_;
        public ICQueries(string tableName)
        {
            this.tableName_ = tableName;
        }
        public static string GET_CARD
        {
            get
            {
                return " select A.*,0 CardType, C.ID langId, C.NAME langName from ImageCatalog..CardMain A " +
                        " left join ImageCatalog..SeparatorMain B on A.IDSeparator = B.ID " +
                        " left join ImageCatalog..Language C on C.ID = B.IDLanguage" +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select A.*,3 CardType, C.ID langId, C.NAME langName from ImageCatalog..CardAV A " +
                        " left join ImageCatalog..SeparatorAV B on A.IDSeparator = B.ID " +
                        " left join ImageCatalog..Country C on C.ID = B.IDCountry " +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select A.*,1 CardType, 0 langId, 'Периодика' langName from ImageCatalog..CardPeriodical A " +
                        " left join ImageCatalog..SeparatorPeriodical B on A.IDSeparator = B.ID " +
                        " left join ImageCatalog..Country C on C.ID = B.IDCountry" +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select A.*,2 CardType, C.ID langId, C.NAME langName from ImageCatalog..CardSubscript A " +
                        " left join ImageCatalog..SeparatorSubscript B on A.IDSeparator = B.ID " +
                        " left join ImageCatalog..Language C on C.ID = B.IDLanguage" +
                        " where A.FilesName = @cardFileName";
            }
        }

        public static string GET_IC_ORDER
        {
            get
            {
                return $" select * from Circulation..ICOrders A " +
                       $" left join Circulation..ICOrdersFlow B on A.Id = B.OrderId and B.StatusName  = '{CirculationStatuses.Refusual.Value}' "  +
                       $" where A.Id = @OrderId ";
            }
        }

        public static string NEW_ORDER
        {
            get
            {
                return "insert into Circulation..ICOrders (CardFileName,  CardSide,  StartDate,  StatusName,  ReaderId,  Comment)" +
                       "                           values (@CardFileName, @CardSide, @StartDate, @StatusName, @ReaderId, @Comment);" +
                       "select SCOPE_IDENTITY()";
            }
        }

        public static string CHANGE_ORDER_STATUS
        {
            get
            {
                return " BEGIN TRANSACTION; " +
                       " insert into Circulation..ICOrdersFlow (OrderId, StatusName, Changed,  Changer,    DepartmentId, Refusual ) " +
                       " values                            (@OrderId, @StatusName,getdate(), @Changer, @DepartmentId, @Refusual );" +
                       " update Circulation..ICOrders set StatusName = @StatusName where ID = @OrderId;" +
                       " COMMIT; ";
            }
        }

        public static string IS_ORDER_EXISTS
        {
            get
            {
                return $" select * from Circulation..ICOrders " +
                       $" where CardFileName = @CardFileName and CardSide = @CardSide and ReaderId = @ReaderId " +
                       $" and StatusName not in  (@FinishedStatusName, @RefusualStatusName) ";
            }
        }

        public static string GET_ACTIVE_ORDERS_BY_READER
        {
            get
            {
                return $" select Id from Circulation..ICOrders " +
                       $" where ReaderId = @ReaderId " +
                       $" and (StatusName not in  (@FinishedStatusName, @RefusualStatusName) " +
                       $" or (StatusName in  (@RefusualStatusName) and StartDate > dateadd(day, -3, getdate())))";
            }
        }

        public static string DELETE_ORDER
        {
            get
            {
                return $" delete from Circulation..ICOrders " +
                       $" where Id = @orderId ";
            }
        }

        public static string GET_HISTORY_ORDERS_BY_READER
        {
            get
            {
                return $" select A.Id, B.Refusual from Circulation..ICOrders A " +
                       $" left join Circulation..ICOrdersFlow B on A.Id = B.OrderId and B.StatusName = @RefusualStatusName " +
                       $" where A.ReaderId = @ReaderId " +
                       $" and A.StatusName in  (@FinishedStatusName, @RefusualStatusName)" +
                       $" order by A.Id desc ";
            }
        }

        public static string GET_ACTIVE_ORDERS_FOR_BOOKKEEPING
        {
            get
            {
                return $" select Id from Circulation..ICOrders " +
                       $" where StatusName not in  (@FinishedStatusName,@RefusualStatusName) " +
                       $" or (StatusName in  (@RefusualStatusName) and StartDate > dateadd(day, -3, getdate()))";
            }
        }

        public static string GET_ACTIVE_ORDERS_FOR_CAFEDRA
        {
            get
            {
                return $" select Id from Circulation..ICOrders " +
                       $" where StatusName in  (@WaitingFirstIssue)";
            }
        }


        public static string CARD_TO_CATALOG
        {
            get
            {
                return $" insert into Circulation..CardToCatalog " +
                       $"        (CardFileName,  CardTableName,  ExemplarBar,  BookId,  ExemplarId,  UserId,  AssignDate) " +
                       $" values (@CardFileName, @CardTableName, @ExemplarBar, @BookId, @ExemplarId, @UserId, @AssignDate) ";
            }
        }

        public static string GET_BOOKS_ON_CARD
        {
            get
            {
                return $" select * from Circulation..CardToCatalog " +
                       $" where CardFileName = @cardFileName ";
            }
        }
    }
}
