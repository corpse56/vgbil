using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation.Loaders;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

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
            List<BasketInfo>  result = loader.GetBasket(ReaderId);
            foreach (BasketInfo basketInfo in result)
            {
                basketInfo.AcceptableOrderType = this.GetAcceptableOrderTypesForReader(basketInfo.BookId, ReaderId);
            }
            return result;
        }

        private List<int> GetAcceptableOrderTypesForReader(string bookId, int readerId)
        {
            List<int> result = new List<int>();
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(bookId);
            ReaderInfo reader = ReaderInfo.GetReader(readerId);
            foreach (BJExemplarInfo exemplar in book.Exemplars)
            {
                int AcceptableOrderType = this.GetExemplarAcceptableOrderType(exemplar);
                if (AcceptableOrderType == 0)
                {
                    continue;
                }
                if (!result.Contains(AcceptableOrderType))
                {
                    result.Add(AcceptableOrderType);
                }
            }
            if (null != reader.Rights.RightsList.FirstOrDefault(x => x.ReaderRightValue == Readers.ReadersRights.ReaderRightsEnum.Employee))
            {
                //кароче здесь надо написать логику, если это суотрудник или оплаченный абонемент, то в зал заменить на на дом
                //но так как при выдаче всё равно всё встанет на свои места, то для экономи времени пропустим это.
            }
            if (reader.IsRemoteReader)
            {
                result.Remove(OrderTypes.InLibrary.Id);
                result.Remove(OrderTypes.PaperVersion.Id);
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
            //{ 1999,   "Невозможно определить доступ"},
        }
        private int GetExemplarAcceptableOrderType(BJExemplarInfo exemplar)
        {
            return KeyValueMapping.AccessCodeToOrderTypeId.GetValueOrDefault(exemplar.ExemplarAccess.Access, 0);
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
        public List<OrderHistoryInfo> GetOrdersHistory(int idReader)
        {
            return loader.GetOrdersHistory(idReader);
        }

        public void DeleteOrder(int OrderId)
        {
            OrderInfo o = loader.GetOrder(OrderId);
            if (
                (o.StatusName == CirculationStatuses.ElectronicIssue.Value) ||
                (o.StatusName == CirculationStatuses.OrderIsFormed.Value) ||
                (o.StatusName == CirculationStatuses.Refusual.Value) ||
                (o.StatusName == CirculationStatuses.SelfOrder.Value)
               )
            {
                loader.DeleteOrder(OrderId);
            }
            else
            {
                throw new Exception("C012");
            }
        }
        public void MakeOrder(MakeOrder request)
        {
            //BookBase book = new BookBase()
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(request.BookId);
            BookSimpleView bookSimpleView = ViewFactory.GetBookSimpleView(request.BookId);

            ReaderInfo reader = ReaderInfo.GetReader(request.ReaderId);
            if (request.OrderTypeId == OrderTypes.ElectronicVersion.Id)
            {
                if (this.ElectronicIssueCount(reader) >= 5)
                {
                    throw new Exception("C001");
                }
                if (this.IsElectronicIssueAlreadyIssued(reader,book))
                {
                    throw new Exception("C002");
                }
                if (book.Exemplars.Count - this.GetBusyExemplarsCount(book) <= 0)
                {
                    throw new Exception("C003");
                }
                if (!this.IsTwentyFourHoursPastSinceReturn(reader, book))
                {
                    throw new Exception("C004");
                }
                BJElectronicExemplarInfo exemplar = new BJElectronicExemplarInfo(book.ID, book.Fund);
                //BJExemplarInfo exemplar = BJExemplarInfo(book.ID, book.Fund);
                this.NewOrder(exemplar, reader, request.OrderTypeId, 30);
            }
            else
            {
                if (this.IsBookAlreadyIssuedToReader(book, reader))
                {
                    throw new Exception("C006");
                }

                //ExemplarSimpleView exemplarSimpleView;
                bool IsOrderedSuccessfully = false;
                switch (request.OrderTypeId)
                {
                    case OrderTypes.PaperVersion.Id:
                        //приоритет для книг, которые в хранении, чтобы их принесли на кафедру для читателя
                        foreach (BJExemplarInfo e in book.Exemplars)
                        {
                            if (e.ExemplarAccess.Access == 1000)
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.PaperVersion.Id, 30);
                                    IsOrderedSuccessfully = true;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        //если свободных книг в хранении не осталось, то ищем те, которые в отрытом доступе. это будет самостоятельный заказ
                        foreach (BJExemplarInfo e in book.Exemplars)
                        {
                            if ((e.ExemplarAccess.Access == 1006))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.SelfOrder.Id, 30);
                                    IsOrderedSuccessfully = true;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("C009");
                        }

                    case OrderTypes.InLibrary.Id:
                        //тут опять приоритет у тех, которые надо заказать из книгохранения перед самостоятельным заказом
                        foreach (BJExemplarInfo e in book.Exemplars)
                        {
                            if ((e.ExemplarAccess.Access == 1005) || (e.ExemplarAccess.Access == 1012))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.InLibrary.Id, 4);
                                    IsOrderedSuccessfully = true;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        //если свободных книг в хранении не осталось, то ищем те, которые в отрытом доступе. это будет самостоятельный заказ
                        foreach (BJExemplarInfo e in book.Exemplars)
                        {
                            if ((e.ExemplarAccess.Access == 1007) || (e.ExemplarAccess.Access == 1014))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.SelfOrder.Id, 4);
                                    IsOrderedSuccessfully = true;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("C009");
                        }
                    //это никогда не придёт
                    case OrderTypes.NoActionProvided.Id:
                        throw new Exception("C008");
                    case OrderTypes.ClarifyAccess.Id:
                        throw new Exception("C008");
                    default:
                        throw new Exception("C008");

                }

                //BJExemplarInfo exemplar = this.GetFirstFreeExemplar(book, request.OrderType);
                //if (exemplar == null)
                //{
                //    throw new Exception("C005");
                //}

                //this.NewOrder(exemplar, reader, request.OrderType);
                
            }

        }

        private bool IsBookAlreadyIssuedToReader(BJBookInfo book, ReaderInfo reader)
        {
            return loader.IsBookAlreadyIssuedToReader(book, reader);
        }

        //private BJExemplarInfo GetFirstFreeExemplar(BJBookInfo book, string orderType)
        //{
        //    foreach (BJExemplarInfo exemplar in book.Exemplars)
        //    {
        //        if (loader.IsExemplarIssued(exemplar))
        //        {
        //            continue;
        //        }
        //        else
        //        {

        //            if (KeyValueMapping.AccessCodeToOrderType[exemplar.ExemplarAccess.Access] == orderType)
        //            {
        //                return exemplar;
        //            }
        //            else if ((orderType == "На дом") || (orderType == "В зал"))
        //            {
        //                return exemplar;
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //    }
        //    return null;
        //}

        private void NewOrder(BookExemplarBase exemplar, ReaderInfo reader, int orderTypeId, int ReturnInDays)
        {
            loader.NewOrder(exemplar, reader, orderTypeId, ReturnInDays);
        }

        private bool IsTwentyFourHoursPastSinceReturn(ReaderInfo reader, BJBookInfo book)
        {
            return loader.IsTwentyFourHoursPastSinceReturn(reader, book);
        }

        private int GetBusyExemplarsCount(BJBookInfo book)
        {
            return loader.GetBusyExemplarsCount(book);
        }
        private bool IsExemplarIssued(BookExemplarBase exemplar)
        {
            return loader.IsExemplarIssued(exemplar);
        }

        private bool IsElectronicIssueAlreadyIssued(ReaderInfo reader, BJBookInfo book)
        {
            return loader.IsElectronicIssueAlreadyIssued(reader, book);
        }

        private int ElectronicIssueCount(ReaderInfo reader)
        {
            return loader.ElectronicIssueCount(reader);
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
