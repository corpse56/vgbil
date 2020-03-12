using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Circulation;
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

        internal DataTable GetActiveOrdersByReader(int readerId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_ACTIVE_ORDERS_BY_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                dataAdapter.SelectCommand.Parameters.Add("FinishedStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }


        internal void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = ICQueries.DELETE_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("orderId", SqlDbType.Int).Value = orderId;
                command.ExecuteNonQuery();

            }
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

        internal void AssignCardToCatalog(ICOrderInfo ICOrder, ExemplarBase ICExemplar, BJUserInfo bjUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = ICQueries.CARD_TO_CATALOG;
                command.Parameters.Clear();
                command.Parameters.Add("CardFileName", SqlDbType.NVarChar).Value = ICOrder.CardFileName;
                command.Parameters.Add("CardTableName", SqlDbType.NVarChar).Value = ICOrder.Card.CardType.ToString();
                command.Parameters.Add("ExemplarBar", SqlDbType.NVarChar).Value = ICExemplar.Bar;
                command.Parameters.Add("BookId", SqlDbType.NVarChar).Value = ICExemplar.BookId;
                command.Parameters.Add("ExemplarId", SqlDbType.NVarChar).Value = ICExemplar.Id;
                command.Parameters.Add("UserId", SqlDbType.Int).Value = bjUser.Id;
                command.Parameters.Add("AssignDate", SqlDbType.DateTime).Value = DateTime.Now;
                command.ExecuteNonQuery();

            }

            return;
        }


        internal void ChangeOrderStatus(ICOrderInfo order, BJUserInfo bjUser, string statusName)
        {
            this.ChangeOrderStatus(order.Id, statusName, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, null);
        }

        internal DataTable GetActiveOrdersForBookkeeping()
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_ACTIVE_ORDERS_FOR_BOOKKEEPING, connection);
                dataAdapter.SelectCommand.Parameters.Add("FinishedStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }
        internal DataTable GetActiveOrdersForCafedra()
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_ACTIVE_ORDERS_FOR_CAFEDRA, connection);
                dataAdapter.SelectCommand.Parameters.Add("WaitingFirstIssue", SqlDbType.NVarChar).Value = CirculationStatuses.WaitingFirstIssue.Value;
                //dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }

        internal void RefuseOrder(ICOrderInfo order, BJUserInfo bjUser, string refusualReason)
        {
            ChangeOrderStatus(order.Id, CirculationStatuses.Refusual.Value, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, refusualReason);
        }

        internal DataTable GetHistoryOrdersByReader(int readerId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_HISTORY_ORDERS_BY_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                dataAdapter.SelectCommand.Parameters.Add("FinishedStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }

        internal void InsertOrderInDb(ICOrderInfo order)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = ICQueries.NEW_ORDER;
                command.Parameters.Clear();
                command.Parameters.Add("CardFileName", SqlDbType.NVarChar).Value = order.CardFileName;
                command.Parameters.Add("CardSide", SqlDbType.Int).Value = order.SelectedCardSide;
                command.Parameters.Add("StartDate", SqlDbType.DateTime).Value = order.StartDate;
                command.Parameters.Add("StatusName", SqlDbType.NVarChar).Value = order.StatusName;
                command.Parameters.Add("ReaderId", SqlDbType.Int).Value = order.ReaderId;
                command.Parameters.Add("Comment", SqlDbType.NVarChar).Value = order.Comment;
                order.Id = Convert.ToInt32(command.ExecuteScalar());
                
            }

            this.ChangeOrderStatus(order.Id, order.StatusName, 1, 2033, null);
            //this.DeleteFromBasket(reader.NumberReader, new List<string>() { exemplar.BookId });
            return;// order.Id;
        }

        internal DataTable GetOrdersCountForReader(int readerId)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.GET_ACTIVE_ORDERS_BY_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.NVarChar).Value = readerId;
                dataAdapter.SelectCommand.Parameters.Add("FinishedStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }

        internal DataTable IsOrderAlreadyExists(ICOrderInfo order)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ICQueries.IS_ORDER_EXISTS, connection);
                dataAdapter.SelectCommand.Parameters.Add("CardFileName", SqlDbType.NVarChar).Value = order.CardFileName;
                dataAdapter.SelectCommand.Parameters.Add("CardSide", SqlDbType.NVarChar).Value = order.SelectedCardSide;
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.NVarChar).Value = order.ReaderId;
                dataAdapter.SelectCommand.Parameters.Add("FinishedStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Finished.Value;
                dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                int i = dataAdapter.Fill(table);
            }
            return table;
        }

        private void ChangeOrderStatus(int orderId, string StatusName, int ChangerId, int DepartmentId, string Refusual)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Connection.Open();
                command.CommandText = ICQueries.CHANGE_ORDER_STATUS;
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

    }
}
