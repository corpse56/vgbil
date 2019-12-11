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
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByPIN(pin, fund);
            return result ?? PeriodicBookInfo.GetBookInfoByIDZBar($"PERIOD_{pin}");
        }
        public static BookBase CreateBookByPin(string fullPin)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByPIN(BookBase.GetPIN(fullPin), BookBase.GetFund(fullPin));
            return result ?? PeriodicBookInfo.GetBookInfoByIDZBar(fullPin);
        }

        public static BookBase CreateBookInfoByInventoryNumber(string invNumber, string fund)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByInventoryNumber(invNumber);
            if (result == null)
            {
                result = PeriodicBookInfo.GetBookInfoByInventoryNumber(invNumber);
            }
            return result;
        }
    }
}
