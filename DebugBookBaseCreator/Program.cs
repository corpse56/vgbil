using LibflClassLibrary.Books;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DebugBookBaseCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            const string bookId = "BJVVV_1016768";
            BJVuFindConverter converter = new BJVuFindConverter(BookBase.GetFund(bookId));
            VufindDoc vfDoc = converter.CreateVufindDocument(BookBase.GetPIN(bookId));
            decimal _cost =  1036476.76m;

            int IntCost = (int)(_cost);
            Console.WriteLine(RusNumber.Str(1036476));

            //BookBase book = BookBase.

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
