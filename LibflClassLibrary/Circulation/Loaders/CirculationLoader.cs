using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation.DB;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Litres;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static LibflClassLibrary.Circulation.CirculationInfo;

namespace LibflClassLibrary.Circulation.Loaders
{
    class CirculationLoader
    {
        CirculationDBWrapper dbWrapper;
        public CirculationLoader()
        {
            dbWrapper = new CirculationDBWrapper();
        }

        internal List<BasketInfo> GetBasket(int readerId)
        {
            DataTable table  = dbWrapper.GetBasket(readerId);
            List<BasketInfo> basket = new List<BasketInfo>();
            foreach (DataRow row in table.Rows)
            {
                BasketInfo bi = new BasketInfo();
                bi.BookId = row["BookId"].ToString();
                bi.ID = (int)row["ID"];
                bi.ReaderId = (int)row["ReaderId"];
                bi.PutDate = (DateTime)row["PutDate"];
                bi.AlligatBookId = row["AlligatBookId"].ToString();
                basket.Add(bi);
            }
            return basket;
        }

        internal DateTime? GetLastEmailDate(ReaderInfo reader)
        {
            DataTable table = dbWrapper.GetLastEmailDate(reader.NumberReader);
            return (table.Rows.Count == 0) ? null : (DateTime?)table.Rows[0][0];
        }

        internal bool IsExemplarIssued(ExemplarBase exemplar)
        {
            //if (!(exemplar is BJExemplarInfo)) return false;
            DataTable table = dbWrapper.IsExemplarIssued(exemplar);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool IsBookAlreadyIssuedToReader(BookBase book, ReaderInfo reader)
        {
            DataTable table = dbWrapper.IsBookAlreadyIssuedToReader(book, reader);
            return (table.Rows.Count != 0);
        }
        internal void NewOrder(ExemplarBase exemplar, ReaderInfo reader, int orderTypeId, int ReturnInDays, string AlligatBookId, int IssuingDepId)
        {
            switch (orderTypeId)
            {
                case OrderTypes.ElectronicVersion.Id:
                    BJElectronicExemplarInfo ElectronicCopy = ((BJElectronicExemplarInfo)exemplar);
                    dbWrapper.NewElectronicOrder(exemplar as BJElectronicExemplarInfo, reader);
                    break;
                case OrderTypes.PaperVersion.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, CirculationStatuses.OrderIsFormed.Value, AlligatBookId, IssuingDepId);
                    break;
                case OrderTypes.InLibrary.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, CirculationStatuses.OrderIsFormed.Value, AlligatBookId, IssuingDepId);
                    break;
                case OrderTypes.SelfOrder.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, CirculationStatuses.SelfOrder.Value, AlligatBookId, IssuingDepId);
                    break;
            }
        }

        internal bool IsTwentyFourHoursPastSinceReturn(ReaderInfo reader, BJBookInfo book)
        {
            DataTable table = dbWrapper.IsTwentyFourHoursPastSinceReturn(reader, book);
            if (table.Rows.Count == 0)
            {
                return true;
            }
            if ((DateTime.Now - (DateTime)table.Rows[0]["Changed"]).Days < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal int GetBusyExemplarsCount(BJBookInfo book)
        {
            DataTable table = dbWrapper.GetBusyExemplarsCount(book);
            return table.Rows.Count;
        }

        internal bool IsElectronicIssueAlreadyIssued(ReaderInfo reader, BookBase book)
        {
            DataTable table = dbWrapper.IsElectronicIssueAlreadyIssued(reader, book);
            return (table.Rows.Count > 0) ? true : false;
        }

        internal int ElectronicIssueCount(ReaderInfo reader)
        {
            DataTable table = dbWrapper.ElectronicIssueCount(reader);
            return table.Rows.Count;
        }

        internal void DeleteFromBasket(BasketDelete request)
        {
            dbWrapper.DeleteFromBasket(request.ReaderId, request.BooksToDelete);
        }
        internal void DeleteOrder(int orderId)
        {
            dbWrapper.DeleteOrder(orderId);
        }
        internal OrderInfo GetOrder(int orderId)
        {
            DataTable table = dbWrapper.GetOrder(orderId);
            if (table.Rows.Count == 0)
            {
                throw new Exception("C011");
            }
            OrderInfo order = FillOrderFromDataRow(table.Rows[0]);
            return order;

        }

        internal List<OrderInfo> GetOrders(int idData, string fund)
        {
            DataTable table = dbWrapper.GetOrdersByExemplar(idData, fund);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            return Orders;
        }

        internal void InsertSentEmailAction(ReaderInfo reader, BJUserInfo bjUser)
        {
            dbWrapper.InsertAdditionalAction(reader.NumberReader, CirculationAdditionalActions.EmailSent.Value, -1, bjUser.Id);

        }

        internal int GetAttendance(BJUserInfo bjUser)
        {
            return dbWrapper.GetAttendance(bjUser.SelectedUserStatus.UnifiedLocationCode).Rows.Count;
        }

        internal bool IsIssuedToReader(ExemplarBase exemplar)
        {
            DataTable table = dbWrapper.IsIssuedToReader(exemplar.Id, exemplar.Fund);
            return (table.Rows.Count != 0) ? true : false;
        }

        internal void AddAttendance(string barcode, BJUserInfo bjUser)
        {
            ReaderInfo reader = ReaderInfo.GetReaderByBar(barcode);

            int readerId = (reader == null) ? -1 : reader.NumberReader;

            dbWrapper.AddAttendance(barcode, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, readerId);
        }

        internal string GetExemplarAvailabilityStatus(int idData, string fund)
        {
            DataTable table = dbWrapper.GetExemplarAvailabilityStatus(idData, fund);
            return (table.Rows.Count == 0) ? "Available" : "Unavailable";
        }

        internal bool IsAlreadyVisitedToday(string barcode, BJUserInfo bjUser)
        {
            DataTable table = dbWrapper.IsAlreadyVisitedToday(barcode, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return (table.Rows.Count == 0) ? false : true;

        }

        internal int GetIssuedInHallBooksCount(BJUserInfo bjUser)
        {
            DataTable table = dbWrapper.GetOrders(CirculationStatuses.IssuedInHall.Value, bjUser.SelectedUserStatus.UnifiedLocationCode);
            return table.Rows.Count;
        }

        internal List<OrderInfo> GetOrdersForStorage(int depId, string depName)
        {
            DataTable table = dbWrapper.GetOrdersForStorage(depId);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            Orders.RemoveAll(x => isWrongFloorPredicate(depName, x));
            return Orders;
        }
        internal List<OrderInfo> GetOrdersForStorage(int depId, string depName, string statusName)
        {
            DataTable table = dbWrapper.GetOrdersForStorage(depId, statusName);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            Orders.RemoveAll(x => isWrongFloorPredicate(depName, x));

            return Orders;
        }
        internal List<OrderInfo> GetOrdersHistoryForStorage(int depId, string depName)
        {
            DataTable table = dbWrapper.GetOrdersHistoryForStorage(depId);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            Orders.RemoveAll(x => isWrongFloorPredicate(depName, x));

            return Orders;
        }
        private bool isWrongFloorPredicate(string depName, OrderInfo order)
        {
            if (order.ExemplarId == 0)
            {
                return true;
            }
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
            if (exemplar == null) return true;
            if (exemplar.Fields["899$a"].ToString() == depName)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //прямая выдача книги. заказ ещё не создан
        internal void IssueBookToReader(ExemplarBase exemplar, ReaderInfo reader, IssueType issueType, BJUserInfo bjUser)
        {
            int ReturnInDays = (issueType == IssueType.AtHome) ? 30 : 10;//30 дней на дом. 10 дней бронеполка.
            string statusName = (issueType == IssueType.AtHome) ? CirculationStatuses.IssuedAtHome.Value : CirculationStatuses.IssuedInHall.Value;
            int deptId = KeyValueMapping.BJDepartmentIdToUnifiedLocationId[bjUser.SelectedUserStatus.DepId];
            dbWrapper.IssueBookToReader(exemplar, reader.NumberReader, ReturnInDays, bjUser.Id,
                                        deptId, statusName);
        }


        //выдача книги. заказ уже есть. Нужно поменять статус заказу.
        internal void IssueBookToReader(OrderInfo order, IssueType issueType, BJUserInfo bjUser)
        {
            string statusName = (issueType == IssueType.AtHome) ? CirculationStatuses.IssuedAtHome.Value : CirculationStatuses.IssuedInHall.Value;
            int ReturnInDays = (issueType == IssueType.AtHome) ? 30 : 10;//30 дней на дом. 10 дней бронеполка.
            ChangeOrderStatusIssue(bjUser, order.OrderId, statusName, ReturnInDays);
        }
        internal void ChangeOrderStatus(BJUserInfo bjUser, int orderId, string statusName)
        {
            dbWrapper.ChangeOrderStatus(orderId, statusName, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, null);
        }

        private void ChangeOrderStatusIssue(BJUserInfo bjUser, int orderId, string statusName, int returnInDays)
        {
            dbWrapper.ChangeOrderStatusIssue(orderId, statusName, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, null, returnInDays);
        }
        internal void ChangeOrderStatusReturn(BJUserInfo bjUser, int orderId, string statusName)
        {
            dbWrapper.ChangeOrderStatusReturn(orderId, statusName, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, null);
        }
        internal void ChangeOrderStatusReturnAndRemoveResponsibility(BJUserInfo bjUser, int orderId, string statusName)
        {
            dbWrapper.ChangeOrderStatusReturnAndRemoveResponsibility(orderId, statusName, bjUser.Id, bjUser.SelectedUserStatus.UnifiedLocationCode, null);
        }

        internal List<OrderInfo> GetOrders(int idReader)
        {
            DataTable table = dbWrapper.GetOrders(idReader);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach(DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            return Orders;
        }

        internal OrderInfo FindOrderByExemplar(ExemplarBase exemplar)
        {
            DataTable table = dbWrapper.FindOrderByExemplar(Convert.ToInt32(exemplar.Id), exemplar.Fund);
            if (table.Rows.Count == 0) return null;
            return FillOrderFromDataRow(table.Rows[0]);
        }

        public OrderInfo FillOrderFromDataRow(DataRow row)
        {
            OrderInfo order = new OrderInfo();
            order.AnotherReaderId = (row["AnotherReaderId"] == DBNull.Value) ? 0 : Convert.ToInt32(row["AnotherReaderId"]);
            order.BookId = row["BookId"].ToString();
            order.ExemplarId = (int)row["ExemplarId"];
            order.FactReturnDate = (row["FactReturnDate"] == DBNull.Value) ? null : (DateTime?)row["FactReturnDate"];
            order.IssueDep = row["IssueDepId"].ToString();
            order.OrderId = (int)row["ID"];
            order.ReaderId = (int)row["ReaderId"];
            order.ReturnDate = (DateTime)row["ReturnDate"];
            order.ReturnDep = row["ReturnDepId"].ToString();
            order.StartDate = (DateTime)row["StartDate"];
            order.StatusName = row["StatusName"].ToString();
            order.StatusCode = CirculationStatuses.ListView.FirstOrDefault(x => x.Value == order.StatusName).Key;
            order.Book = ViewFactory.GetBookSimpleView(order.BookId);
            order.Refusual = row["Refusual"].ToString();
            order.IssuingDepartmentId = (row["IssuingDepId"] == DBNull.Value) ? 0 : (int)row["IssuingDepId"];//где получить первый раз
            order.AlligatBookId = row["AlligatBookId"].ToString();
            order.IssueDate = (row["IssueDate"] == DBNull.Value) ? null : (DateTime?)row["IssueDate"];
            order.BookUrl = row["BookUrl"].ToString();
            order.Fund = row["Fund"].ToString();
            return order;
        }

        internal List<OrderHistoryInfo> GetOrdersHistory(int idReader)
        {
            DataTable table = dbWrapper.GetOrdersHistory(idReader);
            List<OrderHistoryInfo> OrdersHistory = new List<OrderHistoryInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderHistoryInfo order = new OrderHistoryInfo();
                order.BookId = row["BookId"].ToString();
                order.ExemplarId = (int)row["ExemplarId"];
                order.FactReturnDate = (row["FactReturnDate"] == DBNull.Value) ? (DateTime)row["ReturnDate"] : (DateTime)row["FactReturnDate"];
                order.OrderId = (int)row["ID"];
                order.ReaderId = (int)row["ReaderId"];
                order.ReturnDate = (DateTime)row["ReturnDate"];
                order.StartDate = (DateTime)row["StartDate"];
                order.IssueDate = (row["IssueDate"] == DBNull.Value) ? (DateTime)row["StartDate"] : (DateTime?)row["IssueDate"];
                order.Book = ViewFactory.GetBookSimpleView(order.BookId);
                OrdersHistory.Add(order);
            }
            return OrdersHistory;
        }

        internal List<OrderFlowInfo> GetOrdersFlow(int unifiedLocationCode)
        {
            DataTable table = dbWrapper.GetOrdersFlow(unifiedLocationCode);
            List<OrderFlowInfo> result = new List<OrderFlowInfo>();
            foreach (DataRow row in table.Rows)
            {
                OrderFlowInfo fi = new OrderFlowInfo();
                fi.Changed = (DateTime)row["Changed"];
                fi.Changer = (int)row["Changer"];
                fi.DepartmentId = unifiedLocationCode;
                fi.Id = (int)row["Id"];
                fi.OrderId = (int)row["OrderId"];
                fi.Refusual = row["Refusual"].ToString();
                fi.StatusName = row["StatusName"].ToString();
                result.Add(fi);
            }
            return result;
        }
        internal List<OrderFlowInfo> GetOrdersFlowByOrderId(int orderId)
        {
            DataTable table = dbWrapper.GetOrdersFlowByOrderId(orderId);
            List<OrderFlowInfo> result = new List<OrderFlowInfo>();
            foreach (DataRow row in table.Rows)
            {
                OrderFlowInfo fi = new OrderFlowInfo();
                fi.Changed = (DateTime)row["Changed"];
                fi.Changer = (int)row["Changer"];
                fi.DepartmentId = (int)row["DepartmentId"]; ;
                fi.Id = (int)row["Id"];
                fi.OrderId = (int)row["OrderId"];
                fi.Refusual = row["Refusual"].ToString();
                fi.StatusName = row["StatusName"].ToString();
                result.Add(fi);
            }
            return result;
        }

        internal void InsertIntoUserBasket(List<BasketInfo> request)
        {
            foreach(BasketInfo item in request)
            {
                if (!this.IsExistsInBasket(item.ReaderId, item.BookId))
                {
                    dbWrapper.InsertIntoUserBasket(item.ReaderId, item.BookId, item.AlligatBookId);
                }
            }
        }


        internal bool IsExistsInBasket(int readerId, string BookId)
        {
            DataTable table = dbWrapper.IsExistsInBasket(readerId, BookId);
            return (table.Rows.Count != 0);
        }

        internal void ProlongOrder(int orderId, int days)
        {
            dbWrapper.ProlongOrder(orderId, days);
        }

        internal int GetOrderTimesProlonged(int orderId)
        {
            DataTable table = dbWrapper.GetOrderTimesProlonged(orderId);
            return (table.Rows.Count);

        }

        internal void FinishOrder(OrderInfo order, BJUserInfo bjUser)
        {
            dbWrapper.FinishOrder(order.OrderId, bjUser.Id, KeyValueMapping.BJDepartmentIdToUnifiedLocationId[bjUser.SelectedUserStatus.DepId]);
        }



        internal void RefuseOrder(int orderId, string cause, BJUserInfo user)
        {

            dbWrapper.RefuseOrder(orderId, cause, CirculationStatuses.Refusual.Value,user.Id, KeyValueMapping.BJDepartmentIdToUnifiedLocationId[user.SelectedUserStatus.DepId]);

        }

        internal List<OrderInfo> GetOrders(string circulationStatus)
        {
            DataTable table = dbWrapper.GetOrders(circulationStatus);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            return Orders;
        }

        internal List<OrderInfo> GetOverdueOrders(string statusName)
        {
            DataTable table = dbWrapper.GetOverdueOrders(statusName);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach (DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = FillOrderFromDataRow(row);
                Orders.Add(order);
            }
            return Orders;
        }

        internal OrderInfo GetLastOrder(int idData, string fund)
        {
            DataTable table = dbWrapper.GetLastOrder(idData, fund);
            return (table.Rows.Count == 0) ? null : FillOrderFromDataRow(table.Rows[0]);
        }
    }
}
