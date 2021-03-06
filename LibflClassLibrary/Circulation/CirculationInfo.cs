﻿using CirculationApp;
using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodicBooks;
using LibflClassLibrary.Circulation.CirculationService;
using LibflClassLibrary.Circulation.Loaders;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Litres;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public DateTime? GetLastEmailDate(ReaderInfo reader)
        {
            return loader.GetLastEmailDate(reader);
        }

        private List<int> GetAcceptableOrderTypesForReader(string bookId, int readerId)
        {
            List<int> result = new List<int>();
            BookBase b = BookFactory.CreateBookByPin(bookId);
            BJBookInfo book = (BJBookInfo)b;
            if (book == null)
            {
                result.Add(OrderTypes.NoActionProvided.Id);
                return result;
            }
            ReaderInfo reader = ReaderInfo.GetReader(readerId);
            foreach (ExemplarBase eee in book.Exemplars)
            {
                if (eee is PeriodicExemplarInfo)
                {
                    //это костыль для периодики
                    //заказ периодики надо будет конечно сделать, но сейчас не прокатит.
                    result.Add(OrderTypes.OrderProhibited.Id);
                    continue;
                }
                BJExemplarInfo exemplar = (BJExemplarInfo)eee;
                if (exemplar.Fields["929$b"].HasValue)//списано
                {
                    if (exemplar.Fields["921$c"].ToLower() == "списано")
                    {
                        continue;
                    }
                }
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
            if (book.IsExistsDigitalCopy && result.Count == 0)
            {
                if (!result.Contains(OrderTypes.ElectronicVersion.Id))
                {
                    result.Add(OrderTypes.ElectronicVersion.Id);
                }
            }
            if (null != reader.Rights.RightsList.FirstOrDefault(x => x.ReaderRightValue == ReaderRightsEnum.Employee))
            {
                //кароче здесь надо написать логику, если это суотрудник или оплаченный абонемент, то в зал заменить на на дом
                //но так как при выдаче всё равно всё встанет на свои места, то для экономи времени пропустим это.
            }
            if (reader.IsRemoteReader)
            {
                result.Remove(OrderTypes.InLibrary.Id);
                result.Remove(OrderTypes.PaperVersion.Id);
                result.Remove(OrderTypes.ClarifyAccess.Id);
                if (result.Count == 0)
                {
                    result.Add(OrderTypes.OrderProhibited.Id);
                }
            }
            if (book.DigitalCopy != null)
            {
                if (book.DigitalCopy.AccessInfo.Access == 1002)
                {
                    result.Add(3);
                }
                if (book.DigitalCopy.AccessInfo.Access == 1001)
                {
                    result.Add(6);
                }
            }
            return result.Distinct().ToList();
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

        public void InsertSentEmailAction(ReaderInfo reader, BJUserInfo bjUser)
        {
            loader.InsertSentEmailAction(reader, bjUser);
        }

        public List<OrderInfo> GetOrders(int idData, string fund)
        {
            return loader.GetOrders( idData,  fund);
        }

        internal int GetAttendance(BJUserInfo bjUser)
        {
            return loader.GetAttendance(bjUser);
        }

        internal void AttendanceScan(string barcode, BJUserInfo bjUser)
        {
            if (ReaderInfo.IsRightReaderBarcode(barcode))
            {
                if (this.IsAlreadyVisitedToday(barcode, bjUser))
                {
                    throw new Exception("C024");
                }
                else
                {
                    loader.AddAttendance(barcode, bjUser);
                }
            }
            else
            {
                throw new Exception("C023");
            }
        }

        private bool IsAlreadyVisitedToday(string barcode, BJUserInfo bjUser)
        {
            return loader.IsAlreadyVisitedToday(barcode, bjUser);
        }

        public void RecieveBookInBookkeeping(OrderInfo order, BJUserInfo bjUser)
        {
            if (order == null)
            {
                throw new Exception("C025");
            }

            //проверка ДП
            //BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
            ExemplarBase exemplar = ExemplarFactory.CreateExemplar(order.ExemplarId, order.Fund);
            if (exemplar.PublicationClass == "Для выдачи")
            {
                ChangeOrderStatus(bjUser, order.OrderId, CirculationStatuses.Finished.Value);
            }
            else
            {
                throw new Exception("C021");
            }
        }

        internal int GetIssuedInHallBooksCount(BJUserInfo bjUser)
        {
            return loader.GetIssuedInHallBooksCount(bjUser);
        }

        //метод надо переписать для станций самовыдачи
        public void RecieveBookFromReader(ExemplarBase exemplar, OrderInfo oi, BJUserInfo bjUser)
        {
            //exemplar.circulation.exemplarRecieverFromReader.RecieveBookFromReader(exemplar, oi, bjUser);
        }


        public void RecieveBookFromBookkeeping(OrderInfo order, BJUserInfo bjUser)
        {
            if (order == null)
            {
                throw new Exception("C026");
            }
            if (!order.StatusName.In(new[] { CirculationStatuses.EmployeeLookingForBook.Value }))
            {
                throw new Exception("C027");
            }

            ChangeOrderStatus(bjUser, order.OrderId, CirculationStatuses.WaitingFirstIssue.Value);
        }

        public enum IssueType { AtHome, InHall }

        public IssueType GetIssueType(ExemplarBase exemplar)
        {
            if (exemplar == null) return IssueType.InHall;
            if (exemplar.AccessInfo == null) return IssueType.InHall;
            if (exemplar.Location.ToLower().Contains("выст"))
            {
                return IssueType.InHall;
            }
            switch (exemplar.AccessInfo.Access)
            {
                case 1000: //1000    14  4001 Взять на дом
                    return IssueType.AtHome;
                case 1001: //1001    1   4002 Удалённый доступ
                    return IssueType.InHall;
                case 1002://1002    2   4002 Удалённый доступ
                    return IssueType.InHall;
                case 1003://1003    8   4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1004://1004    3   4002 Удалённый доступ
                    return IssueType.InHall;
                case 1005://1005    6   4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1006://1006    7   4001 Взять на дом
                    return IssueType.AtHome;
                case 1007://1007    9   4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1008://1008    4   4002 Удалённый доступ
                    return IssueType.InHall;
                case 1009://1009    5   4003 Печать по требованию
                    return IssueType.InHall;
                case 1010://1010    11  4005 Уточнить доступ
                    return IssueType.InHall;
                case 1011://1011    10  4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1012://1012    6   4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1013://1013    12  4005 Уточнить доступ
                    return IssueType.InHall;
                case 1014://1014    9   4000 В помещении бибилотеки
                    return IssueType.InHall;
                case 1016://1016    99  4005 Уточнить доступ
                    return IssueType.InHall;
                case 1017://1017    99  4005 Уточнить доступ
                    return IssueType.InHall;
                case 1020://1020    99  4005 Уточнить доступ
                    return IssueType.InHall;
                case 1999://1999    99  4005 Уточнить доступ
                    return IssueType.InHall;

            }
            return IssueType.InHall;

        }
        public IssueType GetIssueType(ExemplarBase exemplar, ReaderInfo scannedReader)
        {
            IssueType result = GetIssueType(exemplar);

            if (exemplar.Location.ToLower().Contains("выст"))
            {
                return IssueType.InHall;
            }
            if (scannedReader.Rights[ReaderRightsEnum.Partner] != null)
            {
                return IssueType.InHall;
            }

            if (scannedReader.Rights[ReaderRightsEnum.Employee] != null)
            {
                if (exemplar.PublicationClass.In("ДП", "Для длительного пользования"))
                {
                    return IssueType.InHall;
                }
                else
                {
                    return IssueType.AtHome;
                }
            }
            else if (scannedReader.Rights[ReaderRightsEnum.PaidAbonement] != null)
            {
                if (scannedReader.Rights[ReaderRightsEnum.PaidAbonement].DateEndReaderRight > DateTime.Now)
                {
                    if (exemplar.PublicationClass.In("ДП", "Для длительного пользования"))
                    {
                        return IssueType.InHall;
                    }
                    else
                    {
                        return IssueType.AtHome;
                    }
                }
                else
                {
                    return CheckFreeAbonementRights(exemplar, scannedReader);
                }
            }
            else
            {
                return CheckFreeAbonementRights(exemplar, scannedReader);
            }
            
        }

        private IssueType CheckFreeAbonementRights(ExemplarBase exemplar, ReaderInfo reader)
        {
            if (exemplar.AccessInfo.Access.In(new[] { 1000, 1006 }))
            {
                if (reader.Rights[ReaderRightsEnum.FreeAbonement] == null)
                {
                    throw new Exception("C019");
                }
                else
                {
                    if (reader.Rights[ReaderRightsEnum.FreeAbonement].DateEndReaderRight <= DateTime.Now)
                    {
                        throw new Exception("C020");
                    }
                    else
                    {
                        return IssueType.AtHome;
                    }
                }
            }
            else
            {
                return IssueType.InHall;
            }
        }

        public void IssueBookToReader(ExemplarBase exemplar, ReaderInfo reader, BJUserInfo bjUser)
        {
            //метод выдаёт книгу, либо возвращает исключения

            //если читатель не проходил через проход, то книгу не выдавать, пока не пройдёт через проход
            //это сделано для того, чтобы читатель не прошёл по гостевому, набрал бы книг, и вышел по гостевому.

            //if (!reader.IsEnteredThroughAccessControlSystem())
            //{
            //    if (reader.Rights[ReaderRightsEnum.Employee] == null)//на сотрудников не распространяется
            //    {
            //        throw new Exception("C028");
            //    }
            //}
            if (reader.Rights[ReaderRightsEnum.Employee] == null)//на сотрудников не распространяется
            {
                if (IsTenBooksAlreadyIssuedAtHome(reader))
                {
                    throw new Exception("C030");
                }
            }
            if (reader.Rights[ReaderRightsEnum.Employee] == null)
            {
                if (this.IsFiveBooksAlreadyIssuedInLibrary(reader))
                {
                    throw new Exception("C033");
                }
            }

            ////для карантиныча
            //if (IsTenBooksAlreadyIssuedAtHome(reader) && reader.NumberReader != 261116)
            //{
            //    throw new Exception("C030");
            //}

            //ищем заказ с таким экземпляром.
            OrderInfo order = this.FindOrderByExemplar(exemplar);

            //получаем способ выдачи для этого экземпляра. на дом или в зал. зависит от книги и от читателя.
            IssueType issueType = GetIssueType(exemplar, reader);


            if (order == null)//если заказа нет, то просто reader. создать заказ со статусом выдано.
            {
                loader.IssueBookToReader(exemplar, reader, issueType, bjUser);
                return;
            }

            if (order.ReaderId != reader.NumberReader)//заказ делал не этот читатель. завершить текущий заказ и выдать этому читателю
            {
                this.FinishOrder(order, bjUser);
                this.IssueBookToReader(exemplar, reader, bjUser);
                return;
            }
            switch (order.StatusName)
            {
                case CirculationStatuses.ElectronicIssue.Value:
                    //сотрудник на кафедре не может считать штрихкод электронной копии. пропускаем
                    break;
                case CirculationStatuses.EmployeeLookingForBook.Value:
                    //книга в руках у сотрудника, значит надо принять на кафедру. можно автоматически
                    loader.IssueBookToReader(order, issueType, bjUser);
                    break;
                case CirculationStatuses.ForReturnToBookStorage.Value:
                    //читатель тот же. можно просто завершить текущий заказ и снова выдать
                    this.FinishOrder(order, bjUser);
                    loader.IssueBookToReader(order, issueType, bjUser);
                    break;
                case CirculationStatuses.InReserve.Value:
                    //книга на бронеполке. читатель тот же. выдаём.
                    loader.IssueBookToReader(order, issueType, bjUser);
                    break;
                case CirculationStatuses.IssuedAtHome.Value:
                    //такого быть не может
                    break;
                case CirculationStatuses.IssuedFromAnotherReserve.Value:
                    //выдача с чужой бронеполки - это зло неимоверное...
                    //пока выдача с чужой бронеполки будет так: завершаем заказ хозяина бронеполки и выдаём другому читателю.
                    break;
                case CirculationStatuses.IssuedInHall.Value:
                    //такого быть не может
                    break;
                case CirculationStatuses.OrderIsFormed.Value:
                    // такого по идее не может быть
                    break;
                case CirculationStatuses.SelfOrder.Value:
                    loader.IssueBookToReader(order, issueType, bjUser);
                    break;
                case CirculationStatuses.WaitingFirstIssue.Value:
                    loader.IssueBookToReader(order, issueType, bjUser);
                    break;
            }



            //switch (scannedExemplar.ExemplarAccess.Access)
            //{
            //    case 1000://Взять на дом Заказать через личный кабинет, для получения на дом пройти в { { location_2006} }
            //        break;
            //    case 1001://Свободый электронный доступ
            //        break;
            //    case 1002://Электронный доступ через авторизацию читателя(удаленного читателя)
            //        break;
            //    case 1003://Электронный доступ в электронном зале НЭБ, читальные зал(3 этаж)
            //        break;
            //    case 1004://ЛитРес: Иностранка
            //        break;
            //    case 1005://В помещении бибилотеки Заказать через личный кабинет, для получения заказа пройти в { { location_2007} }
            //        break;
            //    case 1006://Взять на дом Проследовать в { { exemplar_location} } для получения книги на дом
            //        break;
            //    case 1007://В помещении бибилотеки Проследовать в { { exemplar_location} }, взять самостоятельно для чтения книги в помещении
            //        break;
            //    case 1008://Удалённый доступ   Pearson: Иностранка
            //        break;
            //    case 1009:// Печать по требованию Печать по требованию
            //        break;
            //    case 1010://Уточнить доступ    Проследовать в { { exemplar_location} }. Возможность выдачи уточните у сотрудника
            //        break;
            //    case 1011://В помещении бибилотеки Книга находится на выставке в { { exemplar_location} }
            //        break;
            //    case 1012://В помещении бибилотеки СПВ. Заказать через личный кабинет, проследовать в { { location_2007} }. Сотрудник поможет Вам с дополнительным оборудованием для просмотра.
            //        break;
            //    case 1013://Уточнить доступ    Книга находится в обработке
            //        break;
            //    case 1014://В помещении бибилотеки Редкая книга.Проследовать в { { location_2009} }
            //        break;
            //    case 1016://Уточнить доступ    Проследовать в { { location_2009} }. Возможность доступа уточните у сотрудника.
            //        break;
            //    case 1017://Уточнить доступ    Проследовать в { { location_2007} }. Возможность доступа уточните у сотрудника.
            //        break;
            //    case 1020://Уточнить доступ    Экстремистская литература. Не попадает в индекс. Обрабатывать не нужно.
            //        break;
            //    case 1999://Уточнить доступ    Проследовать в { { location_2007} }. Возможность доступа уточните у сотрудника.
            //        break;
            //}
        }

        private bool IsTenBooksAlreadyIssuedAtHome(ReaderInfo reader)
        {
            List<OrderInfo> orders = loader.GetOrders(reader.NumberReader);
            int count = 0;
            foreach (var order in orders)
            {
                if (order.StatusName == CirculationStatuses.IssuedAtHome.Value)
                {
                    count++;
                }
            }
            return (count >= 10) ? true : false;
        }

        public OrderInfo GetLastOrder(int idData, string fund)
        {
            return loader.GetLastOrder(idData, fund);
        }

        public List<OrderFlowInfo> GetOrdersFlow(int unifiedLocationCode)
        {
            return loader.GetOrdersFlow(unifiedLocationCode);
        }
        public List<OrderFlowInfo> GetOrdersFlowByOrderId(int orderId)
        {
            return loader.GetOrdersFlowByOrderId(orderId);
        }

        private void FinishOrder(OrderInfo order, BJUserInfo bjUser)//этот метод ставит заказу статус завершено.
        {
            loader.FinishOrder(order, bjUser);
        }

        public OrderInfo FindOrderByExemplar(ExemplarBase exemplar)
        {
            return loader.FindOrderByExemplar(exemplar);
        }

        internal bool IsIssuedToReader(ExemplarBase exemplar)
        {
            return loader.IsIssuedToReader(exemplar);
        }


        internal BARType CheckBAR(string data)
        {
            BookBase book = BookFactory.CreateBookByBar(data);
            if (book != null) return BARType.Book;
            ReaderInfo reader = ReaderInfo.GetReaderByBar(data);
            if (reader != null) return BARType.Reader;
            return BARType.NotExist;
        }

        internal void GetElectronicExemplarAvailabilityStatus(string iD)
        {
            throw new NotImplementedException();
        }

        internal string GetExemplarAvailabilityStatus(int idData, string fund)
        {
            return loader.GetExemplarAvailabilityStatus(idData, fund);
        }


        public List<OrderInfo> GetOrdersForStorage(int depId, string depName)
        {
            return loader.GetOrdersForStorage(depId, depName);
        }
        public List<OrderInfo> GetOrdersForStorage(int depId, string depName, string statusName)
        {
            return loader.GetOrdersForStorage(depId, depName, statusName);
        }

        public List<OrderInfo> GetOrdersHistoryForStorage(int depId, string depName)
        {
            return loader.GetOrdersHistoryForStorage(depId, depName);
        }

        public OrderInfo GetOrder(int orderId)
        {
            return loader.GetOrder(orderId);
        }

        private int GetExemplarAcceptableOrderType(ExemplarBase exemplar)
        {
            return KeyValueMapping.AccessCodeToOrderTypeId.GetValueOrDefault(exemplar.AccessInfo.Access, 0);
        }

        public void InsertIntoUserBasket(ImpersonalBasket request)
        {
            if (request.BookIdArray.Count == 0) return;
            List<BasketInfo> UserBasket = new List<BasketInfo>();
            //ищем в списке аллигаты. если такие есть, то заменяем их на конволют.
            foreach (string pin in request.BookIdArray)
            {
                //("BJVVV_1444973");   <---Аллигат
                BasketInfo item = new BasketInfo();
                item.ReaderId = request.ReaderId;
                BookBase book = BookFactory.CreateBookByPin(pin);
                if (book.Fund.In("BJVVV", "REDKOSTJ", "BJACC", "BJSCC", "BJFCC"))
                {
                    List<BJExemplarInfo> exemplars = book.Exemplars.ConvertAll(x => (BJExemplarInfo)x);
                    BJExemplarInfo convolute = exemplars.FirstOrDefault(x => x.ConvolutePin != null);
                    if (convolute != null)
                    {
                        item.BookId = convolute.ConvolutePin;
                        item.AlligatBookId = book.Id;
                    }
                    else
                    {
                        item.BookId = book.Id;
                    }
                }
                UserBasket.Add(item);
            }
            //request.BookIdArray = PinsWithConvoluteID;
            loader.InsertIntoUserBasket(UserBasket);
        }

        public List<OrderInfo> GetOrders(int idReader)
        {
            List<OrderInfo> result = loader.GetOrders(idReader);
            return result;
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
            BookBase book = BookFactory.CreateBookByPin(request.BookId);
            BookSimpleView bookSimpleView = ViewFactory.GetBookSimpleView(request.BookId);

            ReaderInfo reader = ReaderInfo.GetReader(request.ReaderId);
            List<int> acceptableOrderTypes = GetAcceptableOrderTypesForReader(request.BookId, request.ReaderId);
            if (!acceptableOrderTypes.Contains(request.OrderTypeId))
            {
                throw new Exception("C013");
            }

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
                //в простой вид книги искусственно добавляется электронный экземпляр
                if (bookSimpleView.Exemplars.Count - this.GetBusyExemplarsCount(book) <= 0)
                {
                    throw new Exception("C003");
                }
                if (!this.IsTwentyFourHoursPastSinceReturn(reader, book))
                {
                    throw new Exception("C004");
                }
                BJElectronicExemplarInfo exemplar = new BJElectronicExemplarInfo(BookBase.GetPIN(book.Id), book.Fund);
                //BJExemplarInfo exemplar = BJExemplarInfo(book.ID, book.Fund);
                this.NewOrder(exemplar, reader, request.OrderTypeId, 30);
            }
            else
            {

                if (reader.Rights[ReaderRightsEnum.Employee] == null)
                {
                    if (this.IsFiveBooksAlreadyOrderedOrIssuedInLibrary(reader))
                    {
                        throw new Exception("C033");
                    }
                }


                if (this.IsBookAlreadyIssuedToReader(book, reader))
                {
                    throw new Exception("C006");
                }

                bool IsOrderedSuccessfully = false;
                switch (request.OrderTypeId)
                {


                    case OrderTypes.PaperVersion.Id://на дом


                        //приоритет для книг, которые в хранении, чтобы их принесли на кафедру для читателя
                        foreach (ExemplarBase e in book.Exemplars)
                        {
                            //if (!e.Fields["899$a"].HasValue) continue;
                            if (string.IsNullOrWhiteSpace(e.Location)) continue;

                            if (e.AccessInfo.Access == 1000)
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.PaperVersion.Id, 4);
                                    IsOrderedSuccessfully = true;
                                    break;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        //если свободных книг в хранении не осталось, то ищем те, которые в отрытом доступе. это будет самостоятельный заказ
                        foreach (ExemplarBase e in book.Exemplars)
                        {
                            if (string.IsNullOrWhiteSpace(e.Location)) continue;

                            if ((e.AccessInfo.Access == 1006))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.SelfOrder.Id, 4);
                                    IsOrderedSuccessfully = true;
                                    break;
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
                        foreach (ExemplarBase e in book.Exemplars)
                        {
                            if (string.IsNullOrWhiteSpace(e.Location)) continue;
                            if ((e.AccessInfo.Access == 1005) || (e.AccessInfo.Access == 1012))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.InLibrary.Id, 4);
                                    IsOrderedSuccessfully = true;
                                    break;
                                }
                                CirculationInfo ci = new CirculationInfo();
                                OrderInfo order = null;
                                try
                                {
                                    order = ci.GetLastOrder(Convert.ToInt32(e.Id), e.Fund);
                                }
                                catch
                                {
                                    continue;
                                }
                                if (order.StatusName == CirculationStatuses.ForReturnToBookStorage.Value)
                                {
                                    ci.FinishOrder(order, BJUserInfo.GetAdmin());
                                    this.NewOrder(e, reader, OrderTypes.InLibrary.Id, 4);
                                    IsOrderedSuccessfully = true;
                                    break;
                                }

                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        //если свободных книг в хранении не осталось, то ищем те, которые в отрытом доступе. это будет самостоятельный заказ
                        foreach (ExemplarBase e in book.Exemplars)
                        {
                            if (string.IsNullOrWhiteSpace(e.Location)) continue;
                            if ((e.AccessInfo.Access == 1007) || (e.AccessInfo.Access == 1014))
                            {
                                if (!this.IsExemplarIssued(e))
                                {
                                    this.NewOrder(e, reader, OrderTypes.SelfOrder.Id, 4);
                                    IsOrderedSuccessfully = true;
                                    break;
                                }
                            }
                        }
                        if (IsOrderedSuccessfully)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("C010");
                        }
                    //это никогда не придёт
                    case OrderTypes.NoActionProvided.Id:
                        throw new Exception("C008");
                    case OrderTypes.ClarifyAccess.Id:
                        throw new Exception("C008");
                    default:
                        throw new Exception("C008");

                }                
            }

        }

        private bool IsFiveBooksAlreadyOrderedOrIssuedInLibrary(ReaderInfo reader)
        {
            int ordersCount = 0;
            List<OrderInfo> orders = this.GetOrders(reader.NumberReader);
            foreach (OrderInfo order in orders)
            {
                if (order.StatusName.In(CirculationStatuses.EmployeeLookingForBook.Value,
                                        CirculationStatuses.InReserve.Value,
                                        CirculationStatuses.IssuedInHall.Value,
                                        CirculationStatuses.OrderIsFormed.Value,
                                        CirculationStatuses.WaitingFirstIssue.Value))
                {
                    ordersCount++;
                }
            }
            return (ordersCount >= 5) ? true : false;
        }
        private bool IsFiveBooksAlreadyIssuedInLibrary(ReaderInfo reader)
        {
            int ordersCount = 0;
            List<OrderInfo> orders = this.GetOrders(reader.NumberReader);
            foreach (OrderInfo order in orders)
            {
                if (order.StatusName.In(CirculationStatuses.IssuedInHall.Value))
                {
                    ordersCount++;
                }
            }
            return (ordersCount >= 5) ? true : false;
        }

        public void ProlongUnconditionally(int orderId, int days)
        {
            loader.ProlongOrder(orderId, days);
        }
        public void ProlongUnconditionally(int orderId, DateTime toDate)
        {
            loader.ProlongOrder(orderId, toDate);
        }
        public void ProlongOrderByEmployee(int orderId, BJUserInfo bjUser)
        {
            OrderInfo order = this.GetOrder(orderId);
            if ((order.StatusCode == CirculationStatuses.EmployeeLookingForBook.Id) ||
                 (order.StatusCode == CirculationStatuses.Finished.Id) ||
                 (order.StatusCode == CirculationStatuses.ForReturnToBookStorage.Id) ||
                 (order.StatusCode == CirculationStatuses.IssuedFromAnotherReserve.Id) ||
                 (order.StatusCode == CirculationStatuses.OrderIsFormed.Id) ||
                 (order.StatusCode == CirculationStatuses.Refusual.Id) ||
                 (order.StatusCode == CirculationStatuses.SelfOrder.Id) ||
                 (order.StatusCode == CirculationStatuses.WaitingFirstIssue.Id)
               )
            {
                throw new Exception("C015");
            }
            else if ((order.StatusCode == CirculationStatuses.ElectronicIssue.Id) ||
                        (order.StatusCode == CirculationStatuses.IssuedAtHome.Id)
                      )
            {
                  loader.ProlongOrderByEmployee(orderId, 30, bjUser);
            }
            else if ((order.StatusCode == CirculationStatuses.IssuedInHall.Id) ||
                      (order.StatusCode == CirculationStatuses.InReserve.Id)
                    )
            {
                 loader.ProlongOrderByEmployee(orderId, 10, bjUser);
                
            }
            else
            {
                throw new Exception("C018");
            }
        }

        public void ProlongOrder(int OrderId)
        {
            OrderInfo order = this.GetOrder(OrderId);
            if ( (order.StatusCode == CirculationStatuses.EmployeeLookingForBook.Id) ||
                 (order.StatusCode == CirculationStatuses.Finished.Id) ||
                 (order.StatusCode == CirculationStatuses.ForReturnToBookStorage.Id) ||
                 (order.StatusCode == CirculationStatuses.IssuedFromAnotherReserve.Id) ||
                 (order.StatusCode == CirculationStatuses.OrderIsFormed.Id) ||
                 (order.StatusCode == CirculationStatuses.Refusual.Id) ||
                 (order.StatusCode == CirculationStatuses.SelfOrder.Id) ||
                 (order.StatusCode == CirculationStatuses.WaitingFirstIssue.Id)
               )
            {
                throw new Exception("C015");
            }
            else if ( (order.StatusCode == CirculationStatuses.ElectronicIssue.Id) ||
                        (order.StatusCode == CirculationStatuses.IssuedAtHome.Id)
                      )
            {
                int TimesProlonged = loader.GetOrderTimesProlonged(OrderId);
                if (TimesProlonged >= 1)
                {
                    throw new Exception("C016");
                }
                else
                {
                    loader.ProlongOrder(OrderId, 30);
                }
            }
            else if ( (order.StatusCode == CirculationStatuses.IssuedInHall.Id) ||
                      (order.StatusCode == CirculationStatuses.InReserve.Id)
                    )
            {
                int TimesProlonged = loader.GetOrderTimesProlonged(OrderId);
                if (TimesProlonged > 3)
                {
                    throw new Exception("C017");
                }
                else
                {
                    loader.ProlongOrder(OrderId, 10);
                }
            }
            else
            {
                throw new Exception("C018");
            }

            
        }

        public List<OrderInfo> GetOrders(string circulationStatus)
        {
            return loader.GetOrders(circulationStatus);
        }

        private bool IsBookAlreadyIssuedToReader(BookBase book, ReaderInfo reader)
        {
            return loader.IsBookAlreadyIssuedToReader(book, reader);
        }


        private void NewOrder(ExemplarBase exemplar, ReaderInfo reader, int orderTypeId, int ReturnInDays)
        {
            List<BasketInfo> basket = loader.GetBasket(reader.NumberReader);
            string bookId = exemplar.BookId;
            BasketInfo bi = basket.FirstOrDefault(x => x.BookId == bookId && x.ReaderId == reader.NumberReader);

            string alligatBookId = null;
            if (bi != null)
            {
                if (!string.IsNullOrEmpty(bi.AlligatBookId))
                {
                    alligatBookId = bi.AlligatBookId;
                }
            }
            int IssuingDepartmentId = GetFirstIssueDepartmentId(exemplar);
            loader.NewOrder(exemplar, reader, orderTypeId, ReturnInDays, alligatBookId, IssuingDepartmentId);
        }
        private int GetFirstIssueDepartmentId(ExemplarBase exemplar)
        {
            //BJExemplarInfo e = (BJExemplarInfo)exemplar;
            int IssuingDepartmentId = KeyValueMapping.AccessCodeToIssuingDeparmentId[exemplar.AccessInfo.Access];
            if (IssuingDepartmentId == 0)
            {
                //тут вопросики возникают.
                ExemplarSimpleViewInfo es = exemplar.simpleViewer.GetExemplarSimpleView(exemplar);
                if (es == null)
                {
                    IssuingDepartmentId = 2007;//по умолчанию
                }
                else
                {
                    IssuingDepartmentId = es.LocationCode;
                }
            }
            return IssuingDepartmentId;
        }
        private bool IsTwentyFourHoursPastSinceReturn(ReaderInfo reader, BookBase book)
        {
            return loader.IsTwentyFourHoursPastSinceReturn(reader, book);
        }

        public int GetBusyExemplarsCount(BookBase book)
        {
            return loader.GetBusyExemplarsCount(book);
        }
        private bool IsExemplarIssued(ExemplarBase exemplar)
        {
            return loader.IsExemplarIssued(exemplar);
        }

        private bool IsElectronicIssueAlreadyIssued(ReaderInfo reader, BookBase book)
        {
            return loader.IsElectronicIssueAlreadyIssued(reader, book);
        }

        private int ElectronicIssueCount(ReaderInfo reader)
        {
            return loader.ElectronicIssueCount(reader);
        }


        public void DeleteFromBasket(BasketDelete request)
        {
            loader.DeleteFromBasket(request);
        }


        public void ChangeOrderStatus(BJUserInfo user, int OrderId, string statusName)
        {
            loader.ChangeOrderStatus(user, OrderId, statusName);
        }
        public void ChangeOrderStatusReturn(BJUserInfo bjUser, int orderId, string status)
        {
            loader.ChangeOrderStatusReturn(bjUser, orderId, status);
        }

        public void RefuseOrder(int OrderId, string Cause, BJUserInfo user)
        {
            loader.RefuseOrder(OrderId, Cause, user);
        }

        public void RemoveResponsibility(BJUserInfo bjUser, int orderId, string status)
        {
            loader.ChangeOrderStatusReturnAndRemoveResponsibility(bjUser, orderId, status);
        }

        public List<OrderInfo> GetOverdueOrders(string statusName)
        {
            return loader.GetOverdueOrders(statusName);

        }
    }
}
