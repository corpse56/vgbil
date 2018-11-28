using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Writeoff.DB
{
    class WriteoffDatabaseWrapper
    {
        WriteoffQueries Queries = new WriteoffQueries();
        internal DataTable GetWriteoffActs(int year)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_INCREMENT_UPDATE_QUERY, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

    }
}
