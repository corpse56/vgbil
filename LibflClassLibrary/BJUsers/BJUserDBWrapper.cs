using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.BJUsers
{
    class BJUserDBWrapper
    {
        string ConnectionString = AppSettings.ConnectionString;
        public Bibliojet BJQueries;

        internal DataTable GetUserByLogin(string login)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_BJVVV_USER_BY_LOGIN, connection);
                dataAdapter.SelectCommand.Parameters.Add("login", SqlDbType.VarChar).Value = login;
                dataAdapter.Fill(table);
                return table;
            }

        }
    }
}
