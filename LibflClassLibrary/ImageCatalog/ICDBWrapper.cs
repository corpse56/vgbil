using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    class ICDBWrapper
    {
        private string connectionString;
        public ICDBWrapper()
        {
            connectionString = AppSettings.ConnectionString;
        }

        internal DataTable GetICOrderById(int id)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_IC_ORDER, connection);
                dataAdapter.SelectCommand.Parameters.Add("OrderId", SqlDbType.Int).Value = id;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetCard(string cardFileName)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_CARD, connection);
                dataAdapter.SelectCommand.Parameters.Add("cardFileName", SqlDbType.NVarChar).Value = cardFileName;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }
    }
}
