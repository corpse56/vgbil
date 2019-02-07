using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation.DB;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Data;
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
        internal void NewOrder(BookExemplarBase exemplar, ReaderInfo reader, int orderTypeId, int ReturnInDays)
        {
            switch (orderTypeId)
            {
                case OrderTypes.ElectronicVersion.Id:
                    dbWrapper.NewElectronicOrder(exemplar as BJElectronicExemplarInfo, reader);
                    break;
                case OrderTypes.PaperVersion.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, CirculationStatuses.OrderIsFormed.Value);
                    break;
                case OrderTypes.InLibrary.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, CirculationStatuses.OrderIsFormed.Value);
                    break;
                case OrderTypes.SelfOrder.Id:
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, ReturnInDays, OrderTypes.SelfOrder.Value);
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
            OrderInfo order = new OrderInfo();
            order.AnotherReaderId = (table.Rows[0]["AnotherReaderId"] == DBNull.Value) ? 0 : Convert.ToInt32(table.Rows[0]["AnotherReaderId"]);
            order.BookId = table.Rows[0]["BookId"].ToString();
            order.ExemplarId = (int)table.Rows[0]["ExemplarId"];
            order.FactReturnDate = (table.Rows[0]["FactReturnDate"] == DBNull.Value) ? null : (DateTime?)table.Rows[0]["FactReturnDate"];
            order.IssueDep = table.Rows[0]["IssueDepId"].ToString();
            order.OrderId = (int)table.Rows[0]["ID"];
            order.ReaderId = (int)table.Rows[0]["ReaderId"];
            order.ReturnDate = (DateTime)table.Rows[0]["ReturnDate"];
            order.ReturnDep = table.Rows[0]["ReturnDepId"].ToString();
            order.StartDate = (DateTime)table.Rows[0]["StartDate"];
            order.StatusName = table.Rows[0]["StatusName"].ToString();
            order.Refusual = table.Rows[0]["Refusual"].ToString();
            order.Book = ViewFactory.GetBookSimpleView(order.BookId);
            return order;

        }

        internal List<OrderInfo> GetOrders(int idReader)
        {
            DataTable table = dbWrapper.GetOrders(idReader);
            List<OrderInfo> Orders = new List<OrderInfo>();
            int i = 0;
            foreach(DataRow row in table.Rows)
            {
                i++;
                OrderInfo order = new OrderInfo();
                order.AnotherReaderId = (row["AnotherReaderId"] == DBNull.Value) ? 0 : Convert.ToInt32(row["AnotherReaderId"]);
                order.BookId = row["BookId"].ToString();
                order.ExemplarId = (int)row["ExemplarId"];
                order.FactReturnDate = (row["FactReturnDate"] == DBNull.Value) ? null : (DateTime?)row["FactReturnDate"];
                //order.FactReturnDate = (!order.FactReturnDate.HasValue) ? null : new DateTime?(new DateTime(order.FactReturnDate.Value.Ticks, DateTimeKind.Utc));
                order.IssueDep = row["IssueDepId"].ToString();
                order.OrderId = (int)row["ID"];
                order.ReaderId = (int)row["ReaderId"];
                order.ReturnDate = (DateTime)row["ReturnDate"]; 
                order.ReturnDep = row["ReturnDepId"].ToString();
                order.StartDate = (DateTime)row["StartDate"];
               // order.StartDate = order.StartDate.ToUniversalTime();//new DateTime(order.StartDate.Ticks, DateTimeKind.Utc);
                order.StatusName = row["StatusName"].ToString();
                order.Book = ViewFactory.GetBookSimpleView(order.BookId);
                order.Refusual = row["Refusual"].ToString();
                Orders.Add(order);
            }
            return Orders;
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
                order.Book = ViewFactory.GetBookSimpleView(order.BookId);
                OrdersHistory.Add(order);
            }
            return OrdersHistory;
        }


        internal void InsertIntoUserBasket(ImpersonalBasket request)
        {
            foreach(string BookId in request.BookIdArray)
            {
                if (!this.IsExistsInBasket(request.ReaderId, BookId))
                {
                    dbWrapper.InsertIntoUserBasket(request.ReaderId, BookId);
                }
            }
        }

        internal bool IsExistsInBasket(int readerId, string BookId)
        {
            DataTable table = dbWrapper.IsExistsInBasket(readerId, BookId);
            return (table.Rows.Count != 0);
        }
    }
}
