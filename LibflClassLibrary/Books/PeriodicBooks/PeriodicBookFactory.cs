using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.PeriodBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodicBooks
{
    class PeriodicBookFactory : BookFactory
    {
        public override BookBase CreateBook(int pin, string fund)
        {
            BookBase result = null;
            result = PeriodicBookInfo.GetBookInfoByPIN(pin, fund);
            return result ?? null;
        }

        public override BookExemplarBase CreateExemplar(string id, string fund)
        {
            throw new NotImplementedException();
        }
    }
}
