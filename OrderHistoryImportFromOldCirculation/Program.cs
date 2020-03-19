using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Circulation.Loaders;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHistoryImportFromOldCirculation
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

            //string str = "000002";
            //int i = Convert.ToInt32(str);

            CirculationInfo ci = new CirculationInfo();
            //ci.ProlongUnconditionally(45324);
            //ci.ProlongOrder(45324);
            ExemplarBase exemplar = ExemplarFactory.CreateExemplarByInventoryNumber("1224491");
            ReaderInfo reader = ReaderInfo.GetReader(184615);
            //ci.IssueBookToReader(exemplar, reader, BJUserInfo.GetAdmin());
            OrderInfo order = ci.GetLastOrder(Convert.ToInt32(exemplar.Id), exemplar.Fund);
            //ci.RecieveBookFromReader(exemplar, order, BJUserInfo.GetAdmin());
            exemplar.circulation.exemplarRecieverFromReader.RecieveBookFromReader(exemplar, order, BJUserInfo.GetAdmin(), CirculationStatuses.InReserve.Value);

            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
            //BJBookInfo book = BJBookInfo.GetBookInfoByPIN("BJVVV_1275925");
            //ReaderInfo reader = ReaderInfo.GetReader(189245);

            //BJElectronicExemplarInfo exemplar = new BJElectronicExemplarInfo(BookBase.GetPIN(book.Id), book.Fund);
            ////BJExemplarInfo exemplar = BJExemplarInfo(book.ID, book.Fund);
            //MakeOrder request = new MakeOrder();
            //request.BookId = "BJVVV_1275925";
            //request.OrderTypeId = OrderTypes.ElectronicVersion.Id;
            //request.ReaderId = 189245;
            //CirculationInfo ci = new CirculationInfo();
            //ci.NewOrder(exemplar, reader, request.OrderTypeId, 30);


            //ci.ChangeOrderStatusReturn(BJUserInfo.GetAdmin(), 73775, CirculationStatuses.Finished.Value);


        }
    }
}
