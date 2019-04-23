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

        public BJUserDBWrapper(string fund)
        {
            BJQueries = new Bibliojet(fund);
        }

        internal DataTable GetUserByLogin(string login, string fund)
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

        internal DataTable GetUserById(int changer)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_BJVVV_USER_BY_ID, connection);
                dataAdapter.SelectCommand.Parameters.Add("id", SqlDbType.Int).Value = changer;
                dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
