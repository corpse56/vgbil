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

        internal DataTable GetActiveHallOrders(int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ACTIVE_HALL_ORDERS, connection);
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                //dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                dataAdapter.SelectCommand.Parameters.Add("statusRefusual", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                //dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;

                int cnt = dataAdapter.Fill(table);
            }
            return table;

        }

        internal DataTable GetReadersRecievedBookCount(DateTime startDate, DateTime endDate, int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_READERS_RECIEVED_BOOKS_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetAllBooksInHall(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetFinishedHallOrders(int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_FINISHED_HALL_ORDERS, connection);
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                //dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                dataAdapter.SelectCommand.Parameters.Add("statusRefusual", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                //dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;

                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetAttendance(DateTime startDate, DateTime endDate, int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_HALL_ATTENDANCE, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;

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
