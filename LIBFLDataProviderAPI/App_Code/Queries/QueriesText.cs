using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataProviderAPI.Queries
{
    public class BJBookQueries
    {
        private string _dbName;

        public BJBookQueries(string DBName)
        {
            this._dbName = DBName;
        }


        public string GET_BOOK_BY_IDMAIN 
        {
            get 
            {
                return 
                        "select A.MNFIELD, A.MSFIELD , B.PLAIN, A.IDDATA, C.IDBLOCK from " + this._dbName + "..DATAEXT A " +
                        "left join " + this._dbName + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        "left join " + this._dbName + "..FIELDS C on A.MNFIELD = C.MNFIELD and A.MSFIELD = C.MSFIELD " +
                        "where A.IDMAIN = @IDMAIN order by A.IDDATA";
            }
        }

        public string GET_IDMAIN_BY_INVNUMBER
        {
            get 
            {
                return
                        "select IDMAIN from "+this._dbName+"..DATAEXT "+
                        "where MNFIELD = 899 and MSFIELD = '$p' and SORT = @InvNumber";
            }
        }
        public string IS_HYPERLINK_EXISTS
        {
            get
            {
                return
                        "select IDMAIN from " + this._dbName + "..DATAEXT " +
                        "where MNFIELD = 940 and MSFIELD = '$a' and IDMAIN = @IDMAIN";
            }
        }
    }

    public class BJExemplarQueries
    {
        private string _dbName;

        public BJExemplarQueries(string DBName)
        {
            this._dbName = DBName;
        }


        public string GET_EXEMPLAR_BY_IDDATA
        {
            get
            {
                return
                        "select A.MNFIELD, A.MSFIELD , B.PLAIN from " + this._dbName + "..DATAEXT A " +
                        "left join " + this._dbName + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        "where A.IDDATA = @IDDATA";
            }
        }
    }
    public class ReaderQueries
    {
        //public static
    }
}