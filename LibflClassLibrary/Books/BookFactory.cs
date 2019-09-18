using LibflClassLibrary.Books.BJBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books
{
    public class BookFactory
    {
        public static BookBase CreateBook(int pin, string fund)
        {
            BookBase result = null;
            switch (fund)
            {
                case "BJVVV": case "REDKOSTJ": case "BJACC": case "BJFCC": case "BJSCC":
                    result = BJBookInfo.GetBookInfoByPIN(pin, fund);
                    break;

            }
            return result ?? null;
        }
    }
}
