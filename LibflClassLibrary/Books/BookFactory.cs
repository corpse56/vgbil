using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodBooks;
using LibflClassLibrary.Books.PeriodicBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books
{
    public static class BookFactory
    {
        //public abstract BookBase CreateBook(int pin, string fund);
        public static BookBase CreateBookByBar(string bar)
        {
            //два баз. BJ и Periodica

            BookBase result = null;
            result = BJBookInfo.GetBookInfoByBAR(bar);            
            return result ?? PeriodicBookInfo.GetBookInfoByBar(bar);
        }
        public static BookBase CreateBookByPin(int pin, string fund)
        {
            return BookFactory.CreateBookByPin($"{fund}_{pin.ToString()}");
        }
        public static BookBase CreateBookByPin(string fullPin)
        {
            BookBase result = null;
            if (BookBase.GetFund(fullPin) == "PERIOD")
            {
                result = PeriodicBookInfo.GetBookInfoByIDZBar(fullPin);
            }
            else
            {
                result = BJBookInfo.GetBookInfoByPIN(fullPin);
            }
            return result;
        }

        public static BookBase CreateBookInfoByInventoryNumber(string invNumber)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByInventoryNumber(invNumber);
            return result ?? PeriodicBookInfo.GetBookInfoByInventoryNumber(invNumber);
        }
    }
}
