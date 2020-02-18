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
                return $" select * from Circulation..ICOrder where Id = @OrderId ";
            }
        }

    }
}
