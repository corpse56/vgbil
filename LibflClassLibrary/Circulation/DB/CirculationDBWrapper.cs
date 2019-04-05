using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Readers;

namespace LibflClassLibrary.Circulation.DB
{

    class CirculationDBWrapper
    {
        private CirculationQueries Queries;
        private string connectionString;
        public CirculationDBWrapper()
        {
            //connectionString = "Data Source=80.250.173.142;Initial Catalog=Circulation;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
            connectionString = "Data Source=192.168.1.165;Initial Catalog=Circulation;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
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
            Stopwatch w = new Stopwatch();
            w.Start();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = idReader;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int cnt = dataAdapter.Fill(table);
            }
            w.Stop();
            return table;
        }
        internal DataTable GetOrdersHistory(int idReader)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_HISTORY, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = idReader;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable IsBookAlreadyIssuedToReader(BJBookInfo book, ReaderInfo reader)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_BOOK_ALREADY_ISSUED_TO_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDReader", SqlDbType.Int).Value = reader.NumberReader;
                dataAdapter.SelectCommand.Parameters.Add("BookID", SqlDbType.NVarChar).Value = book.Id;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal void InsertIntoUserBasket(int iDReader, string bookId, string alligatBookId)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Queries.INSERT_INTO_USER_BASKET, connection);
                command.Parameters.Add("IDReader", SqlDbType.Int).Value = iDReader;
                command.Parameters.Add("BookID", SqlDbType.NVarChar).Value = bookId;
                command.Parameters.Add("AlligatBookId", SqlDbType.NVarChar).Value = (object)alligatBookId ?? DBNull.Value;
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

        internal DataTable GetOrder(int OrderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDER, connection);
                dataAdapter.SelectCommand.Parameters.Add("OrderId", SqlDbType.Int).Value = OrderId;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable IsIssuedToReader(int idData, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ISSUED_TO_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetExemplarAvailabilityStatus(int idData, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_EXEMPLAR_AVAILABILITY_STATUS, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetOrdersHistoryForStorage(int depId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_HISTORY_FOR_STORAGE, connection);
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetOrdersForStorage(int depId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_FOR_STORAGE, connection);
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.DELETE_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("OrderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                int cnt = command.ExecuteNonQuery();
            }
            this.ChangeOrderStatus(orderId, CirculationStatuses.Finished.Value, 1, 2033, null);
        }

        internal DataTable FindOrderByExemplar(int idData, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.FIND_ORDER_BY_EXEMPLAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
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
                    //command.CommandText = Queries.DELETE_FROM_BASKET_RESERVATION_O;
                    //cnt = command.ExecuteNonQuery();
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


        internal DataTable IsExemplarIssued(BJExemplarInfo bJExemplarInfo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ALREADY_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = bJExemplarInfo.BookId;
                dataAdapter.SelectCommand.Parameters.Add("ExemplarId", SqlDbType.Int).Value = bJExemplarInfo.IdData;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

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

        internal void RefuseOrder(int orderId, string cause, string value, int UserId, int DepId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.REFUSE_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("orderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("UserId", SqlDbType.Int).Value = UserId;
                command.Parameters.Add("DepId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("cause", SqlDbType.NVarChar).Value = cause;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = value;
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
        internal void ProlongOrder(int orderId, int days)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.PROLONG_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("orderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("days", SqlDbType.Int).Value = days;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Prolonged.Value;
                command.ExecuteNonQuery();                
            }

        }

        internal DataTable GetOrderTimesProlonged(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.ORDER_TIMES_PROLONGED, connection);
                dataAdapter.SelectCommand.Parameters.Add("orderId", SqlDbType.Int).Value = orderId;
                dataAdapter.SelectCommand.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Prolonged.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal int NewElectronicOrder(BJElectronicExemplarInfo exemplar, ReaderInfo reader)
        {
            int OrderId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.NEW_ELECTRONIC_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = reader.NumberReader;
                command.Parameters.Add("BookId", SqlDbType.NVarChar).Value = exemplar.BookId;
                command.Parameters.Add("Fund", SqlDbType.NVarChar).Value = exemplar.Fund;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = CirculationStatuses.ElectronicIssue.Value;
                OrderId = Convert.ToInt32(command.ExecuteScalar());
                string ViewMode = (exemplar.IsExistsLQ) ? "LQ" : "HQ";
                string url = @"http://catalog.libfl.ru/Bookreader/Viewer?OrderId=" + OrderId + "&view_mode="+ViewMode;
                command.Parameters.Add("OrderId", SqlDbType.Int).Value = OrderId;
                command.Parameters.Add("BookUrl", SqlDbType.NVarChar).Value = url;
                command.CommandText = "update Circulation..Orders set BookUrl = @BookUrl where ID = @OrderId";
                command.ExecuteNonQuery();
            }
            this.ChangeOrderStatus(OrderId, CirculationStatuses.ElectronicIssue.Value, 1, 2033, null );
            this.DeleteFromBasket(reader.NumberReader, new List<string>() { exemplar.BookId });

            return OrderId;
        }

        internal int NewOrder(BJExemplarInfo exemplar, ReaderInfo reader, int ReturnInDays, string StatusName, string AlligatBookId, int IssuingDepId)
        {
            int OrderId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.NEW_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = reader.NumberReader;
                command.Parameters.Add("BookId", SqlDbType.NVarChar).Value = exemplar.BookId;
                command.Parameters.Add("AlligatBookId", SqlDbType.NVarChar).Value = (object)AlligatBookId ?? DBNull.Value;
                command.Parameters.Add("IssuingDepId", SqlDbType.Int).Value = IssuingDepId;
                command.Parameters.Add("ExemplarId", SqlDbType.Int).Value = exemplar.IdData;
                command.Parameters.Add("ReturnInDays", SqlDbType.Int).Value = ReturnInDays;//(orderType == "На дом") ? 30 : 4;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("Fund", SqlDbType.NVarChar).Value = exemplar.Fund;
                command.Parameters.Add("Barcode", SqlDbType.NVarChar).Value = exemplar.Fields["899$w"].ToString();
                OrderId = Convert.ToInt32(command.ExecuteScalar());
            }
            this.ChangeOrderStatus(OrderId, StatusName, 1, 2033, null);
            this.DeleteFromBasket(reader.NumberReader, new List<string>() { exemplar.BookId });
            return OrderId;
        }
        public void ChangeOrderStatus(int orderId, string StatusName, int ChangerId, int DepartmentId, string Refusual)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.CHANGE_ORDER_STATUS;
                command.Parameters.Clear();
                //(@OrderId, @StatusName, @Changer, @DepartmentId, @Refusual
                command.Parameters.Add("OrderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("Changer", SqlDbType.Int).Value = ChangerId;
                command.Parameters.Add("DepartmentId", SqlDbType.Int).Value = DepartmentId;
                command.Parameters.Add("Refusual", SqlDbType.NVarChar).Value = Refusual ?? (object)DBNull.Value;

                Convert.ToInt32(command.ExecuteNonQuery());
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

        internal DataTable GetBusyExemplarsCount(BJBookInfo book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_BUSY_EXEMPLARS_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;

                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
