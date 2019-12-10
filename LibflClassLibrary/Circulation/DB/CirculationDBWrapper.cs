using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;

namespace LibflClassLibrary.Circulation.DB
{

    public class CirculationDBWrapper
    {
        private CirculationQueries Queries;
        private string connectionString;
        public CirculationDBWrapper()
        {
            connectionString = AppSettings.ConnectionString;
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
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetLastEmailDate(int numberReader)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_LAST_EMAIL_DATE, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = numberReader;
                dataAdapter.SelectCommand.Parameters.Add("EmailSentAction", SqlDbType.NVarChar).Value = CirculationAdditionalActions.EmailSent.Value;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable GetOrdersByExemplar(int idData, string fund)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_BY_EXEMPLAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int cnt = dataAdapter.Fill(table);
            }
            return table;

        }

        internal DataTable GetOrders(string circulationStatus)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_BY_STATUS, connection);
                dataAdapter.SelectCommand.Parameters.Add("circulationStatus", SqlDbType.NVarChar).Value = circulationStatus;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int cnt = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetOrders(string statusName, int unifiedLocationCode)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_BY_STATUS_AND_DEP, connection);
                dataAdapter.SelectCommand.Parameters.Add("statusName", SqlDbType.NVarChar).Value = statusName;
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = unifiedLocationCode;
                int cnt = dataAdapter.Fill(table);
            }
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

        internal DataTable IsBookAlreadyIssuedToReader(BookBase book, ReaderInfo reader)
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

        internal void InsertAdditionalAction(int readerId, string action, int orderId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Queries.INSERT_ADDITIONAL_ACTION, connection);
                command.Parameters.Add("readerId", SqlDbType.Int).Value = readerId;
                command.Parameters.Add("orderId", SqlDbType.Int).Value = (orderId == -1) ? DBNull.Value : (object)orderId;
                command.Parameters.Add("userId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("action", SqlDbType.NVarChar).Value = action;
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        internal void InsertIntoUserBasket(int iDReader, string bookId, string alligatBookId)
        {
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

        internal DataTable GetAttendance(int unifiedLocationCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_TODAY_ATTENDANCE_BY_DEP, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = unifiedLocationCode;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void AddAttendance(string barcode, int empId, int unifiedLocationCode, int readerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Queries.ADD_ATTENDANCE, connection);
                command.Parameters.Add("readerId", SqlDbType.Int).Value = readerId;
                command.Parameters.Add("barcode", SqlDbType.NVarChar).Value = barcode;
                command.Parameters.Add("empId", SqlDbType.NVarChar).Value = empId;
                command.Parameters.Add("depId", SqlDbType.NVarChar).Value = unifiedLocationCode;
                connection.Open();
                command.ExecuteNonQuery();
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


        internal DataTable IsAlreadyVisitedToday(string barcode, int unifiedLocationCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ALREADY_VISITED_TODAY, connection);
                dataAdapter.SelectCommand.Parameters.Add("barcode", SqlDbType.NVarChar).Value = barcode;
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = unifiedLocationCode;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable IsIssuedToReader(string exemplarId, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ISSUED_TO_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("exemplarId", SqlDbType.Int).Value = exemplarId;
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
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = depId;
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
        internal DataTable GetOrdersForStorage(int depId, string statusName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_FOR_STORAGE_BY_STATUS, connection);
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                dataAdapter.SelectCommand.Parameters.Add("statusName", SqlDbType.NVarChar).Value = CirculationStatuses.EmployeeLookingForBook.Value;
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

        internal void IssueBookToReader(ExemplarBase scannedExemplar, int numberReader, int returnInDays, int userId, int deptId, string statusName)
        {
            int OrderId;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.NEW_ORDER_ISSUE_BOOK;
                command.Parameters.Clear();
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = numberReader;
                command.Parameters.Add("IssueDepId", SqlDbType.Int).Value = deptId;
                command.Parameters.Add("BookId", SqlDbType.NVarChar).Value = scannedExemplar.BookId;
                command.Parameters.Add("IssuingDepId", SqlDbType.Int).Value = deptId;
                command.Parameters.Add("ExemplarId", SqlDbType.Int).Value = scannedExemplar.Id;
                command.Parameters.Add("ReturnInDays", SqlDbType.Int).Value = returnInDays;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = statusName;
                command.Parameters.Add("Fund", SqlDbType.NVarChar).Value = scannedExemplar.Fund;
                command.Parameters.Add("Barcode", SqlDbType.NVarChar).Value = scannedExemplar.Bar;
                OrderId = Convert.ToInt32(command.ExecuteScalar());
            }
            this.ChangeOrderStatus(OrderId, statusName, userId, deptId, null);
        }


        internal DataTable FindOrderByExemplar(int idData, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.FIND_ORDER_BY_EXEMPLAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
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

        internal DataTable IsElectronicIssueAlreadyIssued(ReaderInfo reader, BookBase book)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ELECTRONIC_ISSUE_ALREADY_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDReader", SqlDbType.Int).Value = reader.NumberReader;
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = book.Id;
                dataAdapter.SelectCommand.Parameters.Add("BookIdInt", SqlDbType.Int).Value = BookBase.GetPIN(book.Id);
                dataAdapter.SelectCommand.Parameters.Add("BASE", SqlDbType.Int).Value = (book.Fund == "BJVVV")? 1 : 2;

                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetOrdersFlowByOrderId(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_FLOW_BY_ORDER_ID, connection);
                dataAdapter.SelectCommand.Parameters.Add("orderId", SqlDbType.Int).Value = orderId;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetOverdueOrders(string statusName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_OVERDUE_ORDERS, connection);
                dataAdapter.SelectCommand.Parameters.Add("statusName", SqlDbType.NVarChar).Value = statusName;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetLastOrder(int idData, string fund)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_LAST_ORDER, connection);
                dataAdapter.SelectCommand.Parameters.Add("idData", SqlDbType.Int).Value = idData;
                dataAdapter.SelectCommand.Parameters.Add("fund", SqlDbType.NVarChar).Value = fund;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable GetOrdersFlow(int unifiedLocationCode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_ORDERS_FLOW, connection);
                dataAdapter.SelectCommand.Parameters.Add("depId", SqlDbType.Int).Value = unifiedLocationCode;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal DataTable IsExemplarIssued(ExemplarBase exemplar)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ALREADY_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("BookId", SqlDbType.NVarChar).Value = exemplar.BookId;
                dataAdapter.SelectCommand.Parameters.Add("ExemplarId", SqlDbType.Int).Value = exemplar.Id;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }

        internal void FinishOrder(int orderId, int userId, int deptId)
        {
            ChangeOrderStatus(orderId, CirculationStatuses.Finished.Value, userId, deptId, null);
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

        internal int NewOrder(ExemplarBase exemplar, ReaderInfo reader, int ReturnInDays, string StatusName, string AlligatBookId, int IssuingDepId)
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
                command.Parameters.Add("ExemplarId", SqlDbType.Int).Value = exemplar.Id;
                command.Parameters.Add("ReturnInDays", SqlDbType.Int).Value = ReturnInDays;//(orderType == "На дом") ? 30 : 4;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("Fund", SqlDbType.NVarChar).Value = exemplar.Fund;
                command.Parameters.Add("Barcode", SqlDbType.NVarChar).Value = exemplar.Bar;
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
        public void ChangeOrderStatusIssue(int orderId, string StatusName, int ChangerId, int DepartmentId, string Refusual, int returnInDays)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.CHANGE_ORDER_STATUS_ISSUE;
                command.Parameters.Clear();
                //(@OrderId, @StatusName, @Changer, @DepartmentId, @Refusual
                command.Parameters.Add("OrderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("Changer", SqlDbType.Int).Value = ChangerId;
                command.Parameters.Add("DepartmentId", SqlDbType.Int).Value = DepartmentId;
                command.Parameters.Add("Refusual", SqlDbType.NVarChar).Value = Refusual ?? (object)DBNull.Value;
                command.Parameters.Add("ReturnInDays", SqlDbType.Int).Value = returnInDays;

                Convert.ToInt32(command.ExecuteNonQuery());
            }
        }
        internal void ChangeOrderStatusReturn(int orderId, string StatusName, int ChangerId, int DepartmentId, string Refusual)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.CHANGE_ORDER_STATUS_RETURN;
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
        internal void ChangeOrderStatusReturnAndRemoveResponsibility(int orderId, string StatusName, int ChangerId, int DepartmentId, string Refusual)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = Queries.CHANGE_ORDER_STATUS_RETURN_AND_REMOVE_RESPONSIBILITY;
                command.Parameters.Clear();
                //(@OrderId, @StatusName, @Changer, @DepartmentId, @Refusual
                command.Parameters.Add("OrderId", SqlDbType.Int).Value = orderId;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = StatusName;
                command.Parameters.Add("StatusNameRR", SqlDbType.NVarChar).Value = CirculationStatuses.RemovedResponsibility.Value;
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
