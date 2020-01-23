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
        internal DataTable GetCard(string cardId, string tableName)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.SELECT_RECORD_QUERY, connection);
                //dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                //dataAdapter.Fill(table);
                //return table;
            }
            return table;
        }
    }
}
