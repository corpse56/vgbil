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
                //if (this.IsFiveElBooksIssued(idr, rtype))
                //{
                //    return "Нельзя заказать больше 5 электронных книг! Сдайте какие-либо выданные Вам электронные копии на вкладке \"Электронные книги\" и повторите заказ! ";
                //}
                //if (this.IsELOrderedByCurrentReader(idr, rtype))
                //{
                //    return "Электронная копия этого документа уже выдана Вам!";
                //}
                //if (b.GetExemplarCount() - b.GetBusyExemplarCount() <= 0)
                //{
                //    return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право." +
                //        " Ближайшая свободная дата " + b.GetNearestFreeDate().ToString("dd.MM.yyyy") + ". Попробуйте заказать в указанную дату.";

                //}
                //if (!this.IsDayPastAfterReturn(idr, rtype))
                //{
                //    return "Вы не можете заказать эту электронную копию, поскольку запрещено заказывать ту же копию, если не прошли сутки с момента её возврата. Попробуйте на следующий день.";
                //}
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

        public void DeleteFromBasket(BasketDelete request)
        {
            loader.DeleteFromBasket(request);
        }
    }
}
