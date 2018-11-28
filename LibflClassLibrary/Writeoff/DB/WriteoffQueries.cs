using System;
using System.Collections.Generic;
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
                return " select distinct B.PLAIN from " + this.Fund + "..AFDEL A" +
                       " inner join " + this.Fund + "..AFDELVAR B on A.ID=B.IDAF " +
                       " where DateCreate between @start and @finish";
            }
        }

    }
}
