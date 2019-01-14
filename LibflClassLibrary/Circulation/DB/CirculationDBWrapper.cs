using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{

    class CirculationDBWrapper
    {
        private CirculationQueries Queries;
        private string connectionString;
        public CirculationDBWrapper()
        {
            connectionString = "Data Source=80.250.173.142;Initial Catalog=Circulation;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
            //connectionString = "Data Source=127.0.0.1;Initial Catalog=Circulation;Integrated Security=True;Connect Timeout=1200";
            Queries = new CirculationQueries();
        }

        internal DataTable GetBasket(int readerId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_BASKET, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetOrders(int idReader)
        {
            //////////////////////////////////////////
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = idReader;

                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal void InsertIntoUserBasket(int iDReader, string bookId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Queries.INSERT_INTO_USER_BASKET, connection);
                command.Parameters.Add("IDReader", SqlDbType.Int).Value = iDReader;
                command.Parameters.Add("BookID", SqlDbType.NVarChar).Value = bookId;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        internal DataTable IsExistsInBasket(int iDReader, string bookId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_EXISTS_IN_BASKET, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDReader", SqlDbType.Int).Value = iDReader;
                dataAdapter.SelectCommand.Parameters.Add("BookID", SqlDbType.NVarChar).Value = bookId;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void DeleteFromBasket(int readerId, List<string> booksToDelete)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                foreach (string bookId in booksToDelete)
                {
                    command.CommandText = Queries.DELETE_FROM_BASKET;
                    command.Parameters.Add("IDReader", SqlDbType.Int).Value = readerId;
                    command.Parameters.Add("BookID", SqlDbType.NVarChar).Value = bookId;
                    int idmain = int.Parse(bookId.Substring(bookId.IndexOf("_") + 1));
                    command.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                    int cnt = command.ExecuteNonQuery();
                    command.CommandText = Queries.DELETE_FROM_BASKET_RESERVATION_O;
                    cnt = command.ExecuteNonQuery();
                }
            }

        }
    }
}
