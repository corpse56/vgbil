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
        private string connectionString = AppSettings.ConnectionString;

        public WriteoffDatabaseWrapper(string Fund)
        {
            this.Fund = Fund;
            WQueries = new WriteoffQueries(this.Fund);
        }


        internal DataTable GetWriteoffActs(int year)
        {
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
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_BOOKS_BY_DEL_ACTS, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("ActNumber", SqlDbType.NVarChar).Value = actNumber;
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBooksPerYear(int year, string prefix)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("start", SqlDbType.DateTime).Value = new DateTime(year, 1, 1);
                dataAdapter.SelectCommand.Parameters.AddWithValue("finish", SqlDbType.DateTime).Value = new DateTime(year, 12, 31);
                dataAdapter.SelectCommand.Parameters.AddWithValue("prefix", SqlDbType.NVarChar).Value = prefix;
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBooksPerYearAnotherFundholder(int year)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR_ANOTHER_FUNDHOLDER, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("start", SqlDbType.DateTime).Value = new DateTime(year, 1, 1);
                dataAdapter.SelectCommand.Parameters.AddWithValue("finish", SqlDbType.DateTime).Value = new DateTime(year, 12, 31);
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBooksOnSpecifiedActNumbers(string filter)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_BOOKS_ON_SPECIFIED_ACT_NUMBERS, connection);
                dataAdapter.SelectCommand.CommandText += filter;
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBooksPerYearInActNameOF(int year)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR_IN_ACTNAME_OF, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("year", SqlDbType.NVarChar).Value = year.ToString();
                int i = dataAdapter.Fill(table);
                return table;
            }
        }
        internal DataTable GetBooksPerYearInActNameAB(int year)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR_IN_ACTNAME_AB, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("year", SqlDbType.NVarChar).Value = year.ToString();
                int i = dataAdapter.Fill(table);
                return table;
            }
        }
        internal DataTable GetBooksPerYearInActNameAnotherFundholder(int year)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(WQueries.GET_EXEMPLARS_BY_DEL_ACTS_PER_YEAR_IN_ACTNAME_ANOTHER_FUNDHOLDER, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("year", SqlDbType.NVarChar).Value = year.ToString();
                int i = dataAdapter.Fill(table);
                return table;
            }
        }

        
        internal DataTable GetDepartments()
        {
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
