using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books
{
    public class ExemplarFactory
    {
        public static BookExemplarBase CreateExemplar(int exemplarId, string fund)
        {
            BookExemplarBase result = null;
            switch (fund)
            {
                case "BJVVV":
                case "REDKOSTJ":
                case "BJACC":
                case "BJFCC":
                case "BJSCC":
                    result = BJExemplarInfo.GetExemplarByIdData(exemplarId, fund);
                    break;

            }
            return result ?? null;
        }

    }
}
