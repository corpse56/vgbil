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
                //bi.AcceptableOrderType = GetAcceptableOrderTypesForReader(bi.BookId, readerId);

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
        internal void NewOrder(BookExemplarBase exemplar, ReaderInfo reader, string orderType)
        {
            switch (orderType)
            {
                case "Электронная выдача":
                    dbWrapper.NewElectronicOrder(exemplar as BJElectronicExemplarInfo, reader, "Электронная выдача");
                    break;
                case "На дом":

                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, "На дом");
                    break;
                case "В зал":
                    dbWrapper.NewOrder(exemplar as BJExemplarInfo, reader, "В зал");
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
                order.OrderId = i;
                order.ReaderId = (int)row["ReaderId"];
                order.ReturnDate = (row["ReturnDate"] == DBNull.Value) ? null : (DateTime?)row["ReturnDate"]; 
                order.ReturnDep = row["ReturnDepId"].ToString();
                order.StartDate = (DateTime)row["StartDate"];
               // order.StartDate = order.StartDate.ToUniversalTime();//new DateTime(order.StartDate.Ticks, DateTimeKind.Utc);
                order.StatusName = row["StatusName"].ToString();
                order.Book = ViewFactory.GetBookSimpleView(order.BookId);
                Orders.Add(order);
            }
            return Orders;
        }

        internal void InsertIntoUserBasket(ImpersonalBasket request)
        {
            foreach(string BookId in request.BookIdArray)
            {
                if (!this.IsExistsInBasket(request.IDReader, BookId))
                {
                    dbWrapper.InsertIntoUserBasket(request.IDReader, BookId);
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
