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
                return $" select *,0 CardType from ImageCatalog..CardMain A " +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select *,3 CardType from ImageCatalog..CardAV A " +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select *,1 CardType from ImageCatalog..CardPeriodical A " +
                        " where A.FilesName = @cardFileName" +
                        " union all " +
                        " select *,2 CardType from ImageCatalog..CardSubscript A " +
                        " where A.FilesName = @cardFileName";
            }
        }

        public static string GET_IC_ORDER
        {
            get
            {
                return $" select * from Circulation..ICOrders where Id = @OrderId ";
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
                       $" and StatusName not in  (@FinishedStatusName, @RefusualStatusName) ";
            }
        }

    }
}
