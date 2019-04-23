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
        
        public void OnConnected(Session session)
        {
            Console.WriteLine("New Client Connected");
        }

        public void OnDisconnected(Session session)
        {
            Console.WriteLine("Client Disconnected");
        }

        public void OnLogin(Session session, LoginRequest request, LoginResponse response)
        {
            Console.WriteLine("Login Message");

            response.Ok = true;
        }

        public void OnPatronInformation(Session session, PatronInformationRequest request, PatronInformationResponse response)
        {
            Console.WriteLine("PatronInformation Message");

            response.PartonStatus = new PatronStatus();
            response.Language = Language.Unknown;
            response.TransactionDate = DateTime.Now;
            response.HoldItemsCount = 1;
            response.OverdueItemsCount = 2;
            response.ChargedItemsCount = 3;
            response.FineItemsCount = 4;
            response.RecallItemsCount = 5;
            response.UnavailableHoldsCount = 6;
            response.InstitutionId = request.InstitutionId;
            response.PatronIdentifier = "The Patron";
            response.PersonalName = "The Personal Name";

            response.HoldItemsLimit = 666;
            response.OverdueItemsLimit = 667;
            response.ChargedItemsLimit = 668;

            response.ValidPatron = true;
            response.ValidPatronPassword = string.Empty;

            response.CurrencyType = Currency.RUB;

            response.FeeAmount = 0;
            response.FeeLimit = 1000;

            response.HomeAddress = "Москва";
            response.EmailAddress = "TheEmail@address.com";
            response.HomePhoneNumber = "1234567890";

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
