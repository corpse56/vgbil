using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.BJUsers;
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


        internal bool IsExemplarIssued(BookExemplarBase exemplar)
        {
            if (!(exemplar is BJExemplarInfo)) return false;
            DataTable table = dbWrapper.IsExemplarIssued(exemplar as BJExemplarInfo);
            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool IsBookAlreadyIssuedToReader(BJBookInfo book, ReaderInfo reader)
        {
            DataTable table = dbWrapper.IsBookAlreadyIssuedToReader(book, reader);
            return (table.Rows.Count != 0);
        }
        internal void NewOrder(BookExemplarBase exemplar, ReaderInfo reader, int orderTypeId, int ReturnInDays, string AlligatBookId, int IssuingDepId)
        {
            switch (orderTypeId)
            {
                case OrderTypes.ElectronicVersion.Id:
                    BJElectronicExemplarInfo ElectronicCopy = ((BJElectronicExemplarInfo)exemplar);

                    //try
                    //{
                    //    ElectronicCopy.FillFileFields();
                    //}
                    //catch
                    //{
                    //    //throw new Exception("C014");
                    //}

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

        internal bool IsElectronicIssueAlreadyIssued(ReaderInfo reader, BJBookInfo book)
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

        internal string GetExemplarAvailabilityStatus(int idData, string fund)
        {
            DataTable table = dbWrapper.GetExemplarAvailabilityStatus(idData, fund);
            return (table.Rows.Count == 0) ? "Available" : "Unavailable";
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
            Predicate<OrderInfo> isWrongFloor = delegate (OrderInfo order)
            {
                if (order.ExemplarId == 0)
                {
                    return true;
                }
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                if (exemplar.Fields["899$a"].ToString() == depName)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            };
            Orders.RemoveAll(isWrongFloor);

            return Orders;
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
            Predicate<OrderInfo> isWrongFloor = delegate (OrderInfo order) 
            {
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                if (exemplar.Fields["899$a"].ToString() == depName)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            };
            Orders.RemoveAll(isWrongFloor);

            return Orders;

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
        private OrderInfo FillOrderFromDataRow(DataRow row)
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

        internal LitresInfo GetLitresAccount(int readerId)
        {
            DataTable table = dbWrapper.GetLitresAccount(readerId);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            LitresInfo result = new LitresInfo();
            result.Login = table.Rows[0]["LRLOGIN"].ToString();
            result.Password = table.Rows[0]["LRPWD"].ToString();
            return result;
        }

        internal void AssignLitresAccount(int readerId)
        {
            dbWrapper.AssignLitresAccount(readerId);

        }

        internal void ChangeOrderStatus(ReaderInfo reader, BJUserInfo user, int orderId, string status)
        {
            dbWrapper.ChangeOrderStatus(orderId, status, user.Id, KeyValueMapping.BJDepartmentIdToUnifiedLocationId[user.SelectedUserStatus.DepId], null);
        }

        internal void RefuseOrder(int orderId, string cause, BJUserInfo user)
        {

            dbWrapper.RefuseOrder(orderId, cause, CirculationStatuses.Refusual.Value,user.Id, KeyValueMapping.BJDepartmentIdToUnifiedLocationId[user.SelectedUserStatus.DepId]);

        }
    }
}
