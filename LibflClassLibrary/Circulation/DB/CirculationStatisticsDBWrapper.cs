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
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
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
                dataAdapter.SelectCommand.Parameters.Add("statusRefusual", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int cnt = dataAdapter.Fill(table);
            }
            return table;

        }
        internal DataTable GetDebtorsInHall(int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_DEBTORS_IN_HALL, connection);
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                dataAdapter.SelectCommand.Parameters.Add("statusRefusual", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
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

        internal DataTable GetSelfCheckStationReference(DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_SELF_CHECK_STATION_REFERENCE, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("station1", SqlDbType.Int).Value = 938;
                dataAdapter.SelectCommand.Parameters.Add("station2", SqlDbType.Int).Value = 939;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetOrdersCountBySubject(int unifiedLocationCode, DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ORDERS_COUNT_BY_SUBJECT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable LitresAccountAssignedCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.LITRES_ACCOUNT_ASSIGNED_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable RegisteredReadersRemoteCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.REGISTERED_READERS_REMOTE_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable RegisteredReadersAllCount(DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.REGISTERED_READERS_ALL_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetAllBooksInHall(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetAllBooksInHallACC(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL_ACC, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetAllBooksInHallFCC(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL_FCC, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetAllBooksInHallSCC(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL_SCC, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetAllBooksInHallREDKOSTJ(int depId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_ALL_BOOKS_IN_HALL_REDKOSTJ, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
                dataAdapter.SelectCommand.CommandTimeout = 300;
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
                dataAdapter.SelectCommand.Parameters.Add("startDate", SqlDbType.DateTime).Value = startDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("endDate", SqlDbType.DateTime).Value = endDate.Date;
                dataAdapter.SelectCommand.Parameters.Add("unifiedLocationCode", SqlDbType.Int).Value = unifiedLocationCode;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
    }
}
