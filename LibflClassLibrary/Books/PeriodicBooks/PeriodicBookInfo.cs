using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodBooks
{
    public class PeriodicBookInfo : BookBase
    {
        public string PublishYear { get; set; }
        public string Number { get; set; }
        //это оригинальный пин. а в Id будет IDZ штрихкода, потому что с периодикой всё не так как с книгой.
        public string OriginalPin { get; set; }

        public override string Language { get; set; }
        public override string Title { get; set; }



        //получить описание периодики по оригинальному пину
        //не пригодится
        //internal static BookBase GetBookInfoByPIN(string bookId)
        //{
        //    PeriodicLoader loader = new PeriodicLoader();
        //    return loader.GetBookInfoByPIN(BookBase.GetPIN(bookId));
        //}
        //так как концепция книги-экземпляры не ложится на периодику и текущий личный кабинет, поскольку 
        //в периодике экземпляры все разные у пина, а в книгах одинаковые, то будем считать, 
        //что периодика-книга равна периодике экземпляру. и таким образом ПИНом будет являтся IDZ штрихкода
        //в таблице PI. Это равносильно экземпляру. То есть книга периордики получается по смыслу все равно что экземпляр.
        //книга будет содержать единственный экземпляр с годом и номером
        internal static PeriodicBookInfo GetBookInfoByIDZBar(string bookId)
        {
            PeriodicLoader loader = new PeriodicLoader();
            return loader.GetBookInfoByIDZBar(BookBase.GetPIN(bookId));
        }
        public static PeriodicBookInfo GetBookInfoByBar(string bar)
        {
            PeriodicLoader loader = new PeriodicLoader();
            return loader.GetBookByBar(bar);
        }

        internal static BookBase GetBookInfoByInventoryNumber(string inventoryNumber)
        {
            PeriodicLoader loader = new PeriodicLoader();
            string bar = loader.GetExemplarBarByInventoryNumber(inventoryNumber);
            if (bar == null) return null;
            return loader.GetBookByBar(bar);
        }
    }
}
