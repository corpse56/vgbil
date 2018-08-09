using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace LibflClassLibrary.Books.BJBooks.DB
{
    class DatabaseWrapper
    {
        //public DatabaseWrapper();

        public DataTable ExecuteSelectQuery(SqlDataAdapter da)
        {
            DataSet ds = new DataSet();
            while (true)
            {
                try
                {
                    int cnt = da.Fill(ds, "t");
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Number != -2) throw;//таймаут подключения. 

                    Thread.Sleep(5000);
                    continue;
                }
            }
            return ds.Tables[0];
        }
    }
}
