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
    public abstract class BookFactory
    {
        public abstract BookBase CreateBook(int pin, string fund);
        public BookBase CreateBook(string bar)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByBAR(bar);            
            return result ?? PeriodicBookInfo.GetBookInfoByBar(bar);
        }
        public abstract BookExemplarBase CreateExemplar(int id, string fund);
        public BookExemplarBase CreateExemplar(string bar)
        {
            BookExemplarBase result = null;
            result = BJExemplarInfo.GetExemplarByBar(bar);
            return result ?? PeriodicExemplarInfo.GetPeriodicExemplarInfo(bar);

        }
    }
}
