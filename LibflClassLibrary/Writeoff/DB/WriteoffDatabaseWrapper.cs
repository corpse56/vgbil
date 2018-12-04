using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Writeoff.DB
{
    class WriteoffDatabaseWrapper
    {
        private string Fund;
        private WriteoffQueries WQueries;
        public WriteoffDatabaseWrapper(string Fund)
        {
            this.Fund = Fund;
            WQueries = new WriteoffQueries(this.Fund);
        }


        internal DataTable GetWriteoffActs(int year)
        {
            string connectionString = AppSettings.ConnectionString;
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_WRITEOFF_ACTS, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("start", SqlDbType.DateTime).Value = new DateTime(year, 1, 1);
                dataAdapter.SelectCommand.Parameters.AddWithValue("finish", SqlDbType.DateTime).Value = new DateTime(year, 12, 31);
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBooksByAct(string actNumber)
        {
            string connectionString = AppSettings.ConnectionString;
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_BOOKS_BY_DEL_ACTS, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("ActNumber", SqlDbType.NVarChar).Value = actNumber;
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetDepartments()
        {
            string connectionString = AppSettings.ConnectionString;
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_DEPARTMENTS, connection);
                int i = dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
