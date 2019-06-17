using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;
using SipLibrary.Abstract;
using SipLibrary.Messages.Requests;
using SipLibrary.Messages.Responses;
using SipLibrary.Transport;
using SipLibrary.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPServer
{
    public class Dispatcher : IDispatcher
    {

        List<SipClientInfo> clients_ = new List<SipClientInfo>();


        
        public void OnConnected(Session session)
        {
            SipClientInfo client = new SipClientInfo();
            client.session = session;
            client = null;
            client = clients_.Find(x => x.session.Ip == session.Ip);
            if (client == null)
            {
                client = new SipClientInfo();
                client.session = session;
                clients_.Add(client);
                Console.WriteLine("New Client Connected");
            }
            else
            {
                Console.WriteLine("Client Has Already Connected");
            }
        }

        public void OnDisconnected(Session session)
        {
            SipClientInfo client = clients_.Find(x => x.session.Ip == session.Ip);
            if (client == null)
            {
                Console.WriteLine("Client was not found in connected clients list.");
            }
            else
            {
                clients_.Remove(client);
                Console.WriteLine("Client Disconnected");
            }
        }

        public void OnLogin(Session session, LoginRequest request, LoginResponse response)
        {
            if (clients_.Count == 0)
            {
                response.Ok = false;
                Console.WriteLine("Client was not connected. Login impossible");
                return;
            }

            SipClientInfo client = clients_.Find(x=> x.session.Ip == session.Ip);
            if (client == null)
            {
                response.Ok = false;
                Console.WriteLine("Client was not connected. Login impossible");
                return;
            }
            else
            {
                BJUserInfo bjUser = BJUserInfo.GetUserByLogin(request.LoginUserId, "BJVVV");
                if (bjUser == null)
                {
                    Console.WriteLine("Login failed. Username or password is invalid.");
                    response.Ok = false;
                }
                else
                {
                    if (BJUserInfo.HashPassword(request.LoginPassword) == bjUser.HashedPwd)
                    {
                        client.login = bjUser.Login;
                        client.bjUser = bjUser;
                        client.locationCode = "Hall 2nd floor";
                        client.bjUser.SelectedUserStatus = bjUser.UserStatus[0];
                        response.Ok = true;
                        Console.WriteLine("Successful login.");
                        return;
                    }
                }
            }
        }

        public void OnPatronInformation(Session session, PatronInformationRequest request, PatronInformationResponse response)
        {

            //Stopwatch w = new Stopwatch();
            //w.Start();
            Console.WriteLine("PatronInformation Message");
            response.PartonStatus = new PatronStatus();
            //response.PartonStatus.CardReportedLost = false;
            //response.PartonStatus.ChargePrivilegesDenied = false;
            //response.PartonStatus.ExcessiveOutstandingFees = false;
            //response.PartonStatus.ExcessiveOutstandingFines = false;
            //response.PartonStatus.HoldPrivilegesDenied = false;
            //response.PartonStatus.RecallOverdue = false;
            //response.PartonStatus.RecallPrivilegesDenied = false;
            //response.PartonStatus.RenewalPrivilegesDenied = false;
            //response.PartonStatus.TooManyClaimsOfItemsReturned = false;
            //response.PartonStatus.TooManyItemsBilled = false;
            //response.PartonStatus.TooManyItemsCharged = false;
            //response.PartonStatus.TooManyItemsLost = false;
            //response.PartonStatus.TooManyItemsOverdue = false;
            //response.PartonStatus.TooManyRenewals = false;

            response.Language = Language.Russian;
            response.TransactionDate = DateTime.Now;

            ReaderInfo reader;
            try
            {
                if (request.PatronId[0] == 'R')
                {
                    reader = ReaderInfo.GetReaderByBar(request.PatronId);
                }
                else
                {
                    reader = ReaderInfo.GetReaderByUID(request.PatronId);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //response.Ok = false;
                return;
            }
            if (reader == null)
            {
                Console.WriteLine("Читатель не найден");
                return;
            }

            CirculationInfo ci = new CirculationInfo();
            List<OrderInfo> orders = ci.GetOrders(reader.NumberReader);
            List<OrderInfo> chargedOrders = orders.FindAll(x => x.StatusCode == CirculationStatuses.IssuedAtHome.Id || x.StatusCode == CirculationStatuses.IssuedInHall.Id);
            List<OrderInfo> holdOrders = orders.FindAll(x => x.StatusCode == CirculationStatuses.EmployeeLookingForBook.Id || x.StatusCode == CirculationStatuses.OrderIsFormed.Id
                                                        || x.StatusCode == CirculationStatuses.WaitingFirstIssue.Id || x.StatusCode == CirculationStatuses.InReserve.Id);
            List<OrderInfo> overdueOrders = chargedOrders.FindAll(x => x.ReturnDate < DateTime.Now);
            response.HoldItemsCount = holdOrders.Count;
            response.OverdueItemsCount = overdueOrders.Count;
            response.ChargedItemsCount = chargedOrders.Count;
            response.FineItemsCount = 0;
            response.RecallItemsCount = 0;
            response.UnavailableHoldsCount = 0;
            response.InstitutionId = request.InstitutionId;
            response.PatronIdentifier = reader.NumberReader.ToString();
            response.PersonalName = $"{reader.FamilyName} {reader.Name} {reader.FatherName}";

            response.HoldItemsLimit = 666;
            response.OverdueItemsLimit = 667;
            response.ChargedItemsLimit = 668;

            response.ValidPatron = true;
            response.ValidPatronPassword = string.Empty;

            response.CurrencyType = Currency.RUB;

            response.FeeAmount = 0;
            response.FeeLimit = 1000;

            response.HomeAddress = reader.RegistrationCity;
            response.EmailAddress = reader.Email;
            response.HomePhoneNumber = reader.MobileTelephone;

            response.ScreenMessage = "Информация о читателе";
            response.PrintLine = "Print Line Заголовок";

            foreach (var order in chargedOrders)
            {
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                response.ChargedItems.Add(exemplar.Bar);
            }
            foreach (var order in holdOrders)
            {
                BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(order.ExemplarId, order.Fund);
                response.HoldItems.Add(exemplar.Bar);
            }

            //w.Stop();
            string sipMessage = response.ToString();
            //Console.WriteLine("Elapsed: "+ w.Elapsed.Milliseconds+" ms");
        }

        public void OnScStatus(Session session, ScStatusRequest request, AcsStatusResponse response)
        {
            Console.WriteLine("Sc Status Message");

            SipClientInfo client = clients_.Find(x => x.session.Ip == session.Ip);
            response.OnlineStatus = (client == null)?  false : true;
            response.CheckInOk = true;
            response.CheckOutOk = true;
            response.RenewalPolicy = true;
            response.StatusUpdateOk = true;
            response.OfflineOk = false;
            response.TimeOutPeriod = 100;
            response.RetiresAllowed = 3;
            response.InstitutionId = "ВГБИЛ";
            response.SupportedMessages = SupportedMessages.DefaultValue();
            response.SupportedMessages.PatronInformation = true;
            response.ProtocolVersion = ProtocolVersion.VERSION_2_00;
            //response.
            response.DateTimeSync = DateTime.Now;
        }

        public void OnItemInformation(Session session, ItemInformationRequest request, ItemInformationResponse response)
        {
            Console.WriteLine("Item Information Message");

            string bar = request.ItemIdentifier;


            BJBookInfo book;
            BJExemplarInfo exemplar;
            CirculationInfo ci = new CirculationInfo();
            OrderInfo order;
            try
            {
                exemplar = BJExemplarInfo.GetExemplarByBar(bar);
                book = BJBookInfo.GetBookInfoByPIN(exemplar.BookId);
                order = ci.FindOrderByExemplar(exemplar);
            }
            catch (Exception ex)
            {

                return;
            }


            if (order == null)
            {
                response.CirculationStatus = CirculationStatus.AVAILABLE;
            }
            else
            {
                switch (order.StatusName)
                {
                    case CirculationStatuses.ElectronicIssue.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.EmployeeLookingForBook.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.Finished.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.ForReturnToBookStorage.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.InReserve.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.IssuedAtHome.Value:
                        response.CirculationStatus = CirculationStatus.CHARGED;
                        break;
                    case CirculationStatuses.IssuedFromAnotherReserve.Value:
                        response.CirculationStatus = CirculationStatus.CHARGED;
                        break;
                    case CirculationStatuses.IssuedInHall.Value:
                        response.CirculationStatus = CirculationStatus.CHARGED;
                        break;
                    case CirculationStatuses.OrderIsFormed.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.Prolonged.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.SelfOrder.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                    case CirculationStatuses.WaitingFirstIssue.Value:
                        response.CirculationStatus = CirculationStatus.AVAILABLE;
                        break;
                }
            }
            response.SecurityMarker = SecurityMarker.NONE;
            response.FeeType = FeeType.ADMINISTRATIVE;

            response.TransactionDate = DateTime.Now;
            response.HoldQueueLength = 0;
            response.DueDate = (order == null) ? DateTime.Now : order.ReturnDate;
            response.RecallDate = (order == null) ? DateTime.Now : order.IssueDate;
            response.HoldPickupDate = (order == null) ? DateTime.Now : order.ReturnDate;
            response.ItemIdentifier = request.ItemIdentifier;
            response.TitleIdentifier = (book.Fields["200$a"].MNFIELD == 0) ? book.Fields["200$a"].ToString() : $"{book.Fields["700$a"].ToString()}; {book.Fields["200$a"].ToString()}";
            response.Owner = "ВГБИЛ";
            response.CurrencyType = Currency.RUB;
            response.FeeAmount = 0;
            response.MediaType = MediaType.BOOK;
            response.PermanentLocation = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];
            response.CurrentLocation = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];
            response.ItemProperties = string.Empty;//"The Item Properties";
            response.ScreenMessage = string.Empty;// "The Screen Message";
            response.PrintLine = string.Empty;// "The Print Line";
        }

        public void OnCheckout(Session session, CheckoutRequest request, CheckoutResponse response)
        {
            Console.WriteLine("Checkout Message");
            SipClientInfo client = clients_.Find(x => x.session.Ip == session.Ip);
            if (client == null)
            {
                response.Ok = false;
                Console.WriteLine("Client is not logged in. CheckOut operation impossible.");
                return;
            }



            BJExemplarInfo exemplar;
            CirculationInfo ci;
            OrderInfo order;
            BJBookInfo book;
            ReaderInfo reader;
            try
            {
                exemplar = BJExemplarInfo.GetExemplarByBar(request.ItemIdentifier);
                ci = new CirculationInfo();
                order = ci.FindOrderByExemplar(exemplar);
                book = BJBookInfo.GetBookInfoByPIN(exemplar.BookId);
                reader = ReaderInfo.GetReaderByBar(request.PatronIdentifier);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Ok = false;
                response.MagneticMedia = false;
                response.Desensitize = false; //снять бит или нет
                response.TransactionDate = DateTime.Now;
                response.InstitutionId = "ВГБИЛ";
                response.PatronIdentifier = request.PatronIdentifier;// + " (took from request)";
                response.ItemIdentifier = request.ItemIdentifier;// + " (also took from request)";
                //response.TitleIdentifier = book.Fields["200$a"].ToString();
                response.DueDate = DateTime.Now;
                return;
            }

            
            // обязательные поля
            response.Ok = true;
            if (order != null)
            {
                response.RenewalOk = (order.ReaderId.ToString() == request.PatronIdentifier) ? true : false;
            }
            else
            {
                response.RenewalOk = false;
            }
            if (order == null)
            {
                try
                {
                    ci.IssueBookToReader(exemplar, reader, client.bjUser);
                    order = ci.FindOrderByExemplar(exemplar);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response.Ok = false;
                    response.MagneticMedia = false;
                    response.Desensitize = false; //снять бит или нет
                    response.TransactionDate = DateTime.Now;
                    response.InstitutionId = "ВГБИЛ";
                    response.PatronIdentifier = request.PatronIdentifier;// + " (took from request)";
                    response.ItemIdentifier = request.ItemIdentifier;// + " (also took from request)";
                    response.TitleIdentifier = book.Fields["200$a"].ToString();
                    response.DueDate = DateTime.Now;
                    return;
                }
            }
            else
            {
                try
                {
                    ci.ChangeOrderStatus(client.bjUser, order.OrderId, CirculationStatuses.Finished.Value);
                    ci.IssueBookToReader(exemplar, reader, client.bjUser);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    response.Ok = false;
                    response.MagneticMedia = false;
                    response.Desensitize = false; //снять бит или нет
                    response.TransactionDate = DateTime.Now;
                    response.InstitutionId = "ВГБИЛ";
                    response.PatronIdentifier = request.PatronIdentifier;// + " (took from request)";
                    response.ItemIdentifier = request.ItemIdentifier;// + " (also took from request)";
                    response.TitleIdentifier = book.Fields["200$a"].ToString();
                    response.DueDate = DateTime.Now;
                    return;
                }
            }


            response.MagneticMedia = false;
            response.Desensitize = true; //снять бит или нет
            response.TransactionDate = DateTime.Now;
            response.InstitutionId = "ВГБИЛ";
            response.PatronIdentifier = request.PatronIdentifier;// + " (took from request)";
            response.ItemIdentifier = request.ItemIdentifier;// + " (also took from request)";
            response.TitleIdentifier = book.Fields["200$a"].ToString();
            response.DueDate = DateTime.Now;

            // необязательные поля
            response.FeeType = FeeType.OTHER_UNKNOWN;
            response.SecurityInhibit = true;
            response.CurrencyType = Currency.RUB;
            response.FeeAmount = 100;
            response.MediaType = MediaType.BOOK;
            response.ItemProperties = string.Empty;//"The SUPA DUPA Properies";
            response.TransactionId = order.OrderId.ToString();//"The Trans Id";

            response.ScreenMessage = string.Empty;// "F**K The Police";
            response.PrintLine = string.Empty;// "The Print Line";
        }

        public void OnCheckin(Session session, CheckinRequest request, CheckinResponse response)
        {
            Console.WriteLine("Checkin Message");

            SipClientInfo client = clients_.Find(x => x.session.Ip == session.Ip);
            if (client == null)
            {
                response.Ok = false;
                Console.WriteLine("Client is not logged in. CheckOut operation impossible.");
                return;
            }



            BJExemplarInfo exemplar;
            CirculationInfo ci;
            OrderInfo order;
            BJBookInfo book;
            ReaderInfo reader;
            try
            {
                exemplar = BJExemplarInfo.GetExemplarByBar(request.ItemIdentifier);
                ci = new CirculationInfo();
                order = ci.FindOrderByExemplar(exemplar);
                if (order == null)
                {
                    //никому не выдана
                    response.Ok = false;
                    response.Resensitize = true;
                    return;
                }
                book = BJBookInfo.GetBookInfoByPIN(exemplar.BookId);
                reader = ReaderInfo.GetReader(order.ReaderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Ok = false;
                return;
            }

            try
            {
                ci.RecieveBookFromReader(exemplar,order,client.bjUser);
            }
            catch (Exception ex)
            {
                response.Ok = false;
                return;
            }


            response.Ok = true;
            response.Resensitize = true;
            response.MagneticMedia = null;
            response.Alert = false;
            response.TransactionDate = DateTime.Now;
            response.InstitutionId = "ВГБИЛ";
            response.ItemIdentifier = request.ItemIdentifier;
            response.PermanentLocation = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];//"Permanent Location";
            response.TitleIdentifier = book.Fields["200$a"].ToString(); 
            response.SortBin = "Sort bin...";
            response.PatronIdentifier = reader.BarCode;//"Patron id";
            response.MediaType = MediaType.BOOK;
            response.ItemProperties = "Propst";

            response.MagneticMedia = false;

            response.ScreenMessage = "Screen message";
            response.PrintLine = "Print Line";
        }

        public void OnRenew(Session session, RenewRequest request, RenewResponse response)
        {
            Console.WriteLine("Renew Message");
            SipClientInfo client = clients_.Find(x => x.session.Ip == session.Ip);
            if (client == null)
            {
                response.Ok = false;
                Console.WriteLine("Client is not logged in. CheckOut operation impossible.");
                return;
            }

            bool result = false;
            CirculationInfo ci = new CirculationInfo();
            BJExemplarInfo exemplar = new BJExemplarInfo();
            OrderInfo order = new OrderInfo();
            BJBookInfo book = new BJBookInfo();
            try
            {
                exemplar = BJExemplarInfo.GetExemplarByBar(request.ItemIdentifier);
                order = ci.FindOrderByExemplar(exemplar);
                book = BJBookInfo.GetBookInfoByPIN(exemplar.BookId);
            }
            catch (Exception ex)
            {
                result = false;
            }

            if (order == null)
            {
                result = false;
                response.RenewalOk = false;
            }
            else
            {
                response.RenewalOk = true;
            }

            if (order != null)
            {
                try
                {
                    ci.ProlongOrder(order.OrderId);
                    order = ci.FindOrderByExemplar(exemplar);
                    response.RenewalOk = true;
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }

            order = ci.FindOrderByExemplar(exemplar);


            response.Ok = result;
            response.Desensitize = false;


            response.MagneticMedia = ThreeStateBool.False;

            response.TransactionDate = DateTime.Now;

            response.InstitutionId = "ВГБИЛ";
            response.PatronIdentifier = request.PatronIdentifier;
            response.ItemIdentifier = request.ItemIdentifier;
            response.TitleIdentifier = book.Fields["200$a"].ToString(); 

            response.DueDate = order.ReturnDate;

            response.FeeType = FeeType.OTHER_UNKNOWN;
            response.SecurityInhibit = false;
            response.CurrencyType = Currency.RUB;
            response.FeeAmount = 0;
            response.MediaType = MediaType.BOOK;

            response.ItemProperties = "Properies";
            response.TransactionId = order.OrderId.ToString();

            response.ScreenMessage = "User Screen Message";
            response.PrintLine = "The Print Line";
        }

        public void OnRenewAll(Session session, RenewAllRequest request, RenewAllResponse response)
        {
            Console.WriteLine("RenewAll Message");

            response.Ok = true;
            response.RenewedCount = 3;
            response.UnrenewedCount = 1;
            response.TransactionDate = DateTime.Now;
            response.InstitutionId = "Inst Id";

            response.RenewedItems.Add("The Renewed Item #1");
            response.RenewedItems.Add("The Renewed Item #2");
            response.RenewedItems.Add("The Renewed Item #2");

            response.UnrenewedItems.Add("The Failed Item #1");

            response.ScreenMessage = "Screen Message";
            response.PrintLine = "Printline #1";
        }

        public void OnEndPatronSession(Session session, EndPatronSessionRequest request, EndPatronSessionResponse response)
        {
            Console.WriteLine("EndPatronSession Message");

            response.EndSession = true;
            response.TransactionDate = DateTime.Now;

            response.InstitutionId = "The Institiotion Id";
            response.PatronIdentifier = request.PatronIdentifier + " (took from request)";

            response.ScreenMessage = "Screen Message";
            response.PrintLine = "Printline #1";
        }
    }
}
