using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Writeoff.DB
{
    public class WriteoffQueries
    {
        private string Fund;
        public WriteoffQueries(string Fund)
        {
            this.Fund = Fund;
        }
        public string GET_WRITEOFF_ACTS
        {
            get
            {
                return " select distinct A.ID, B.PLAIN, B.SORT from " + this.Fund + "..AFDEL A" +
                       " inner join " + this.Fund + "..AFDELVAR B on A.ID=B.IDAF " +
                       " where DateCreate between @start and @finish" +
                       " order by A.ID";
            }
        }

        public string GET_DEPARTMENTS
        {
            get
            {
                return "select ID, NAME from "+this.Fund+"..LIST_8";
            }
        }

        public string GET_BOOKS_BY_DEL_ACTS
        {
            get
            {
                return "select distinct IDMAIN from " + this.Fund + "..DATAEXT where MNFIELD = 929 and MSFIELD = '$b' and SORT = @ActNumber";
            }
        }
    }
}
