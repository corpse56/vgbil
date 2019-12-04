using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodBooks
{

    class PeriodicDatabaseWrapper
    {
        private PeriodicQueries queries_;
        private string connectionString;
        
        public PeriodicDatabaseWrapper()
        {
            queries_ = new PeriodicQueries();
            connectionString = AppSettings.ConnectionString;
        }

        internal DataTable GetBookByBar(string bar)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_BOOK_BY_BAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("bar", SqlDbType.NVarChar).Value = bar;
                dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetExemplarBarByInventoryNumber(string inventoryNumber)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_EXEMPLAR_BAR_BY_INVENTORYNUMBER, connection);
                dataAdapter.SelectCommand.Parameters.Add("inventoryNumber", SqlDbType.NVarChar).Value = inventoryNumber;
                dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetBookInfoByPIN(int pin)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_BOOK_BY_PIN, connection);
                dataAdapter.SelectCommand.Parameters.Add("pin", SqlDbType.NVarChar).Value = pin.ToString();
                dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable GetPeriodicExemplarBarByExemplarId(int exemplarId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(queries_.GET_BAR_BY_EXEMPLARID, connection);
                dataAdapter.SelectCommand.Parameters.Add("exemplarId", SqlDbType.Int).Value = exemplarId;
                dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
