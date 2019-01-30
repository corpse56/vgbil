using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Readers;

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
                command.Connection.Open();
                foreach (string bookId in booksToDelete)
                {
                    command.CommandText = Queries.DELETE_FROM_BASKET;
                    command.Parameters.Clear();
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

        internal DataTable ElectronicIssueCount(ReaderInfo reader)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.ELECTRONIC_ISSUE_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDReader", SqlDbType.Int).Value = reader.NumberReader;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable IsElectronicIssueAlreadyIssued(ReaderInfo reader, BJBookInfo book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ELECTRONIC_ISSUE_ALREADY_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDReader", SqlDbType.Int).Value = reader.NumberReader;
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;
                dataAdapter.SelectCommand.Parameters.Add("BookIdInt", SqlDbType.Int).Value = book.ID;
                dataAdapter.SelectCommand.Parameters.Add("BASE", SqlDbType.Int).Value = (book.Fund == "BJVVV")? 1 : 2;

                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void NewOrder(BJBookInfo book, ReaderInfo reader, string orderType)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                string StatusName = "";
                switch (orderType)
                {
                    case "Электронная выдача":
                        command.CommandText = Queries.NEW_ELECTRONIC_ORDER;
                        StatusName = "Электронная выдача";
                        break;
                    case "На дом":
                        command.CommandText = Queries.NEW_ORDER;
                        StatusName = "Заказ сформирован";
                        break;
                    case "В зал":
                        command.CommandText = Queries.NEW_ORDER;
                        StatusName = "Заказ сформирован";
                        break;

                }
                command.Parameters.Clear();
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = reader.NumberReader;
                command.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("Fund", SqlDbType.NVarChar).Value = book.Fund;



                int cnt = command.ExecuteNonQuery();
                command.CommandText = Queries.DELETE_FROM_BASKET_RESERVATION_O;
                cnt = command.ExecuteNonQuery();
            }

        }

        internal DataTable IsTwentyFourHoursPastSinceReturn(ReaderInfo reader, BJBookInfo book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_TWENTYFOUR_HOURS_PAST_SINCE_RETURN, connection);
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = reader.NumberReader;

                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable GetBusyExemplars(BJBookInfo book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_BUSY_EXEMPLARS, connection);
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;

                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
