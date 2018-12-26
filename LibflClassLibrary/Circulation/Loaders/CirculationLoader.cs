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
                bi.AcceptableActions = GetAcceptableActionsForReader(bi.BookId, readerId);

                basket.Add(bi);
            }
            return basket;
        }

        private List<string> GetAcceptableActionsForReader(string bookId, int readerId)
        {
            List<string> result = new List<string>();
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(bookId);
            ReaderInfo reader = ReaderInfo.GetReader(readerId);
            foreach(BJExemplarInfo exemplar in book.Exemplars)
            {
                switch (exemplar.ExemplarAccess.Access)
                {
                    case 1000:
                        // case 1006:
                        if (!result.Contains("На дом"))
                        {
                            result.Add("На дом");
                        }
                        break;
                    //case 1007:
                    case 1005:
                        if (!result.Contains("В зал"))
                        {
                            result.Add("В зал");
                        }
                        break;
                    case 1002:
                        if (!result.Contains("Электронный доступ"))
                        {
                            result.Add("Электронный доступ");
                        }
                        break;
                }
            }
            if (null != reader.Rights.RightsList.FirstOrDefault(x => x.ReaderRightValue == Readers.ReadersRights.ReaderRightsEnum.Employee))
            {
                //кароче здесь надо написать логику, если это суотрудник или оплаченный абонемент, то в зал заменить на на дом
                //но так как при выдаче всё равно всё встанет на свои места, то для экономи времени пропустим это.
            }
            if (reader.IsRemoteReader)
            {
                result.Remove("В зал");
                result.Remove("На дом");
            }

            return result;
            //{ 1000,   "Заказать через личный кабинет, для получения на дом пройти в Зал абонементного обслуживания 2 этаж"},
            //{ 1001,   "Свободый электронный доступ"},
            //{ 1002,   "Доступ через авторизацию читателя(удаленного читателя)"},
            //{ 1003,   "Электронный доступ в электронном зале НЭБ, читальные зал(3 этаж)"},
            //{ 1004,   "ЛитРес:Иностранка"},
            //{ 1005,   "Заказать через личный кабинет, для получения заказа пройти в Зал выдачи документов 2 этаж"},
            //{ 1006,   "Проследовать в зал местонахождения экземпляра для получения книги на дом"},
            //{ 1007,   "Проследовать в зал местонахождения экземпляра, взять самостоятельно для чтения книги в помещении"},
            //{ 1008,   "Pearson:Иностранка"},
            //{ 1009,   "Печать по требованию"},
            //{ 1010,   "Проследовать в зал местонахождения экземпляра. Возможность выдачи уточните у сотрудника"},
            //{ 1011,   "Книга находится на выставке в зале местонахождения экземпляра"},
            //{ 1012,   "Спец.вид. Заказать через личный кабинет, проследовать в Зал выдачи документов 2 этаж. Сотрудник поможет Вам с дополнительным оборудованием для просмотра."},
            //{ 1013,   "Книга находится в обработке"},
            //{ 1014,   "Проследовать в Зал редкой книги 4 этаж"},
            //{ 1016,   "Проследовать в Зал редкой книги 4 этаж. Возможность доступа уточните у сотрудника."},
            //{ 1017,   "Проследовать в Зал выдачи документов 2 этаж. Возможность доступа уточните у сотрудника."},
            //{ 1020,   "Экстремистская литература.Не попадает в индекс.Обрабатывать не нужно."},
            //{ 1999,   "Проследовать в Зал выдачи документов 2 этаж. Возможность доступа уточните у сотрудника."},

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
