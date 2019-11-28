using LibflClassLibrary.Books.PeriodBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodicBooks
{
    class PeriodicExemplarInfo : BookExemplarBase
    {
        public string Bar { get; set; }
        public string Number { get; set; }
        public string PublishYear { get; set; }

        

        public static PeriodicExemplarInfo GetPeriodicExemplarInfo(string bar)
        {
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo result = loader.GetExemplarByBar(bar);
            return result;
        }

    }
}
