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
    public class BookFactory
    {
        //public abstract BookBase CreateBook(int pin, string fund);
        public BookBase CreateBook(string bar)
        {
            //два баз. BJ и Periodica

            BookBase result = null;
            result = BJBookInfo.GetBookInfoByBAR(bar);            
            return result ?? PeriodicBookInfo.GetBookInfoByBar(bar);
        }
        public BookBase CreateBook(int pin, string fund)
        {
            BookBase result = null;
            result = PeriodicBookInfo.GetBookInfoByPIN(pin, fund);
            return result ?? null;
        }

        public static BookBase CreateBookInfoByInventoryNumber(string invNumber, string fund)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByInventoryNumber(invNumber, fund);
            if (result == null)
            {
                result = PeriodicBookInfo.GetBookInfoByInventoryNumber(invNumber);
            }
            return result;
        }
    }
}
