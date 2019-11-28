using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.BJBooks
{
    class BJBookFactory : BookFactory
    {
        public override BookBase CreateBook(int pin, string fund)
        {
            BookBase result = null;
            result = BJBookInfo.GetBookInfoByPIN(pin, fund);
            return result ?? null;
        }

        public override BookExemplarBase CreateExemplar(int id, string fund)
        {
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(Convert.ToInt32(id), fund);
            return exemplar;
        }

    }
}
