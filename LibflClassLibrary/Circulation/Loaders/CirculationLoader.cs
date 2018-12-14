using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.Circulation.DB;
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

        internal List<OrderInfo> GetOrders(int idReader)
        {
            DataTable table = dbWrapper.GetOrders(idReader);
            List<OrderInfo> Orders = new List<OrderInfo>();
            foreach(DataRow row in table.Rows)
            {

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
