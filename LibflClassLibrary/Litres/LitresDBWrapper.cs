using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Litres
{
    class LitresDBWrapper
    {
        LitresQueries Queries = new LitresQueries();
        private string connectionString = AppSettings.ConnectionString;
        

        internal DataTable GetLitresAccount(int readerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_LITRES_ACCOUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void AssignLitresAccount(int readerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.ASSIGN_LITRES_ACCOUNT;
                command.Parameters.Clear();
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                command.Parameters.Add("AccountId", SqlDbType.Int).Value = GetFirstFreeLitresAccount();
                command.ExecuteNonQuery();
            }

        }

        internal void InsertNewLitresAccount(LitresInfo newAccount)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.INSERT_NEW_LITRES_ACCOUNT;
                command.Parameters.Clear();
                command.Parameters.Add("login", SqlDbType.NVarChar).Value = newAccount.Login;
                command.Parameters.Add("password", SqlDbType.NVarChar).Value = newAccount.Password;
                command.ExecuteNonQuery();
            }
        }

        private int GetFirstFreeLitresAccount()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_FIRST_FREE_LITRES_ACCOUNT, connection);
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                if (cnt == 0)
                {
                    throw new Exception("L003");
                }
                return Convert.ToInt32(table.Rows[0]["ID"]);
            }

        }
    }
}
