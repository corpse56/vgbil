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
        public string Pin { get; set; }
        public string Number { get; set; }
        public override string Language { get; set; }
        public override string Title { get; set; }

        public static PeriodicBookInfo GetBookInfoByBar(string bar)
        {
            PeriodicLoader loader = new PeriodicLoader();
            return loader.GetBookByBar(bar);
        }

        internal static BookBase GetBookInfoByPIN(string bookId)
        {
            PeriodicLoader loader = new PeriodicLoader();
            return loader.GetBookInfoByPIN(BookBase.GetPIN(bookId));
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
