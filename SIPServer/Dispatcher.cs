using LibflClassLibrary.Circulation;
using LibflClassLibrary.Readers;
using SipLibrary.Abstract;
using SipLibrary.Messages.Requests;
using SipLibrary.Messages.Responses;
using SipLibrary.Transport;
using SipLibrary.Types;
using System;
using System.Collections.Generic;
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
            SipClientInfo client = clients_.Find(x=> x.session.Ip == session.Ip);
            if (client == null)
            {
                response.Ok = false;
                Console.WriteLine("Client was not connected. Login impossible");
                return;
            }
            else
            {
                if (request.LoginUserId.ToLower() == "station1" && request.LoginPassword == "123")
                {
                    client.login = "station1";
                    client.password = "123";
                    client.locationCode = "Hall 2nd floor";
                    response.Ok = true;
                    Console.WriteLine("Successful login.");
                    return;
                }
                if (request.LoginUserId.ToLower() == "station2" && request.LoginPassword == "123")
                {
                    client.login = "station2";
                    client.password = "123";
                    client.locationCode = "Hall 3nd floor";
                    response.Ok = true;
                    Console.WriteLine("Successful login.");
                    return;
                }
                Console.WriteLine("Login failed. Username or password is invalid.");
                response.Ok = false;
            }
        }

        public void OnPatronInformation(Session session, PatronInformationRequest request, PatronInformationResponse response)
        {
            Console.WriteLine("PatronInformation Message");
            response.PartonStatus = new PatronStatus();
            response.PartonStatus.CardReportedLost = false;
            response.PartonStatus.ChargePrivilegesDenied = false;
            response.PartonStatus.ExcessiveOutstandingFees = false;
            response.PartonStatus.ExcessiveOutstandingFines = false;
            response.PartonStatus.HoldPrivilegesDenied = false;
            response.PartonStatus.RecallOverdue = false;
            response.PartonStatus.RecallPrivilegesDenied = false;
            response.PartonStatus.RenewalPrivilegesDenied = false;
            response.PartonStatus.TooManyClaimsOfItemsReturned = false;
            response.PartonStatus.TooManyItemsBilled = false;
            response.PartonStatus.TooManyItemsCharged = false;
            response.PartonStatus.TooManyItemsLost = false;
            response.PartonStatus.TooManyItemsOverdue = false;
            response.PartonStatus.TooManyRenewals = false;

            response.Language = Language.Russian;
            response.TransactionDate = DateTime.Now;

            ReaderInfo reader = ReaderInfo.GetReader(int.Parse(request.PatronId));
            CirculationInfo ci = new CirculationInfo();
            List<OrderInfo> orders = ci.GetOrders(reader.NumberReader);
            List<OrderInfo> holdOrders = orders.FindAll(x => x.StatusCode == CirculationStatuses.IssuedAtHome.Id || x.StatusCode == CirculationStatuses.IssuedInHall.Id);
            List<OrderInfo> overdueOrders = holdOrders.FindAll(x => x.ReturnDate < DateTime.Now);
            response.HoldItemsCount = holdOrders.Count;
            response.OverdueItemsCount = overdueOrders.Count;
            response.ChargedItemsCount = 0;
            response.FineItemsCount = 0;
            response.RecallItemsCount = 0;
            response.UnavailableHoldsCount = 0;
            response.InstitutionId = request.InstitutionId;
            response.PatronIdentifier = reader.NumberReader.ToString();
            response.PersonalName = reader.FamilyName;

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

            response.ScreenMessage = "The Screen Message of Patron Information Response";
            response.PrintLine = "Print Line";

            response.ChargedItems.Add("Charged item 1");
            response.ChargedItems.Add("Charged item 2");
        }

        public void OnScStatus(Session session, ScStatusRequest request, AcsStatusResponse response)
        {
            Console.WriteLine("Sc Status Message");

            response.OnlineStatus = true;
            response.CheckInOk = true;
            response.CheckOutOk = true;
            response.RenewalPolicy = true;
            response.StatusUpdateOk = true;
            response.OfflineOk = true;
            response.TimeOutPeriod = 66;
            response.RetiresAllowed = 3;
            response.InstitutionId = "XXX";
            response.SupportedMessages = SupportedMessages.DefaultValue();
            response.ProtocolVersion = ProtocolVersion.VERSION_2_00;

            response.DateTimeSync = DateTime.Now;
        }

        public void OnItemInformation(Session session, ItemInformationRequest request, ItemInformationResponse response)
        {
            Console.WriteLine("Item Information Message");

            response.CirculationStatus = CirculationStatus.AVAILABLE;
            response.SecurityMarker = SecurityMarker.NONE;
            response.FeeType = FeeType.ADMINISTRATIVE;

            response.TransactionDate = DateTime.Now;
            response.HoldQueueLength = 0;
            response.DueDate = DateTime.Now;
            response.RecallDate = DateTime.Now;
            response.HoldPickupDate = DateTime.Now;
            response.ItemIdentifier = request.ItemIdentifier;
            response.TitleIdentifier = "The Title identifier";
            response.Owner = "The Owner";
            response.CurrencyType = Currency.RUB;
            response.FeeAmount = 100;
            response.MediaType = MediaType.BOOK;
            response.PermanentLocation = "The Permanent Location";
            response.CurrentLocation = "The Current Location";
            response.ItemProperties = "The Item Properties";
            response.ScreenMessage = "The Screen Message";
            response.PrintLine = "The Print Line";
        }

        public void OnCheckout(Session session, CheckoutRequest request, CheckoutResponse response)
        {
            Console.WriteLine("Checkout Message");

            // обязательные поля
            response.Ok = true;
            response.RenewalOk = false;
            response.MagneticMedia = true;
            response.Desensitize = true;
            response.TransactionDate = DateTime.Now;

            response.InstitutionId = "The Institiotion Id";
            response.PatronIdentifier = request.PatronIdentifier + " (took from request)";
            response.ItemIdentifier = request.ItemIdentifier + " (also took from request)";
            response.TitleIdentifier = "The MEGA Title";
            response.DueDate = DateTime.Now;

            // необязательные поля
            response.FeeType = FeeType.OTHER_UNKNOWN;
            response.SecurityInhibit = true;
            response.CurrencyType = Currency.USD;
            response.FeeAmount = 100;
            response.MediaType = MediaType.BOOK_WITH_CD;
            response.ItemProperties = "The SUPA DUPA Properies";
            response.TransactionId = "The Trans Id";

            response.ScreenMessage = "F**K The Police";
            response.PrintLine = "The Print Line";
        }

        public void OnCheckin(Session session, CheckinRequest request, CheckinResponse response)
        {
            Console.WriteLine("Checkin Message");

            response.Ok = true;
            response.Resensitize = false;
            response.MagneticMedia = null;
            response.Alert = false;
            response.TransactionDate = DateTime.Now;
            response.InstitutionId = " -=xXx=- ";
            response.ItemIdentifier = "Item Id";
            response.PermanentLocation = "Permanent Location";
            response.TitleIdentifier = "Title Id";
            response.SortBin = "Sort bin...";
            response.PatronIdentifier = "Patron id";
            response.MediaType = MediaType.BOOK_WITH_AUDIO_TAPE;
            response.ItemProperties = "Propst";

            response.MagneticMedia = true;

            response.ScreenMessage = "Screen message";
            response.PrintLine = "Print Line";
        }

        public void OnRenew(Session session, RenewRequest request, RenewResponse response)
        {
            Console.WriteLine("Renew Message");

            response.Ok = true;
            response.RenewalOk = false;
            response.MagneticMedia = ThreeStateBool.Unknown;
            response.Desensitize = true;

            response.TransactionDate = DateTime.Now;

            response.InstitutionId = "The Institiotion Id";
            response.PatronIdentifier = request.PatronIdentifier + " (took from request)";
            response.ItemIdentifier = request.ItemIdentifier + " (also took from request)";
            response.TitleIdentifier = "The MEGA Title";

            response.DueDate = DateTime.Now;

            response.FeeType = FeeType.OTHER_UNKNOWN;
            response.SecurityInhibit = true;
            response.CurrencyType = Currency.USD;
            response.FeeAmount = 100;
            response.MediaType = MediaType.BOOK_WITH_CD;

            response.ItemProperties = "The SUPA DUPA Properies";
            response.TransactionId = "The Trans Id";

            response.ScreenMessage = "F**K The Police";
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
