using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodBooks
{
    public class PeriodicBookInfo : BookBase
    {
        public string Title { get; set; }

        public string PublishYear { get; set; }
        public string BookId { get { return this.Id; } }
        public string Pin { get; set; }
        public string Number { get; set; }
        public static PeriodicBookInfo GetBookInfoByBar(string bar)
        {
            PeriodicLoader loader = new PeriodicLoader();
            return loader.GetBookByBar(bar);
        }

        internal static BookBase GetBookInfoByPIN(int pin, string fund)
        {
            throw new NotImplementedException();
        }

        internal static BookBase GetBookInfoByInventoryNumber(string inventoryNumber)
        {
            PeriodicLoader loader = new PeriodicLoader();
            string bar = loader.GetBookBarByInventoryNumber(inventoryNumber);
            if (bar == null) return null;
            return loader.GetBookByBar(bar);
        }
    }
}
