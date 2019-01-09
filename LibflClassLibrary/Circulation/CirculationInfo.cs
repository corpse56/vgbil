using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Circulation.Loaders;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation
{
    public class CirculationInfo
    {
        CirculationLoader loader;
        public CirculationInfo()
        {
            loader = new CirculationLoader();
        }
        public List<BasketInfo> GetBasket(int ReaderId)
        {
            return loader.GetBasket(ReaderId);
        }

        public void InsertIntoUserBasket(ImpersonalBasket request)
        {
            if (request.BookIdArray.Count == 0) return;
            loader.InsertIntoUserBasket(request);

        }

        public List<OrderInfo> GetOrders(int idReader)
        {
            return loader.GetOrders(idReader);
        }

        public void MakeOrder(MakeOrder request)
        {
            //BookBase book = new BookBase()
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(request.BookId);
            ReaderInfo reader = ReaderInfo.GetReader(request.ReaderId);
            if (request.OrderType == "Электронная выдача")
            {

            }
            else
            {

            }
            
        }
        private void CreateCommonBookOrder()
        {

        }
        private void CreateElectronicBookOrder(BJBookInfo book, ReaderInfo reader)
        {
            if (reader.IsFiveElBooksIssued())
            {
                throw new Exception("C001");
            }
            //reader.
        }
    }
}
