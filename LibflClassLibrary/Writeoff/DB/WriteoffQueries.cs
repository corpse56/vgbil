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


        //основной фонд или абонемент книгохранения
        public string GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR
        {
            get
            {
                return "select distinct IDDATA from " + this.Fund + "..DATAEXT" +
                    " where MNFIELD = 929 and MSFIELD = '$b' and Created between @start and @finish " +
                    " and charindex(lower(@prefix),lower(SORT)) >0" +
                    " and charindex(lower('АКТ'),lower(SORT)) >0";
            }
        }

        //списанные не книгохранением, а отделами
        public string GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR_ANOTHER_FUNDHOLDER
        {
            get
            {
                return "select distinct IDDATA from " + this.Fund + "..DATAEXT" +
                    " where MNFIELD = 929 and MSFIELD = '$b' and Created between @start and @finish " +
                    " and charindex(lower('АКТ'),lower(SORT)) >0" +
                    " and charindex(lower('аб'),lower(SORT)) = 0" +
                    " and charindex(lower('оф'),lower(SORT)) = 0";
            }
        }
        


    }
}
