using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationStatisticsDBWrapper
    {
        string connectionString = AppSettings.ConnectionString;
        CirculationStatisticsQueries queries_ = new CirculationStatisticsQueries();

        internal DataTable GetBooksIssuedFromHallCount(DateTime startDate, DateTime endDate, int unifiedLocationCode, int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_BOOKS_ISSUED_FROM_HALL_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                //dataAdapter.SelectCommand.Parameters.Add("status", SqlDbType.Int).Value = depId;
                //dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;

                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetBooksIssuedFromBookkeepingCount(DateTime startDate, DateTime endDate, int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_BOOKS_ISSUED_FROM_BOOKKEPING_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
    }
}
