using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodicBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books
{
    public class ExemplarFactory
    {
        public static ExemplarBase CreateExemplar(int exemplarId, string fund)
        {
            ExemplarBase result = null;
            switch (fund)
            {
                case "BJVVV":
                case "REDKOSTJ":
                case "BJACC":
                case "BJFCC":
                case "BJSCC":
                    result = BJExemplarInfo.GetExemplarByIdData(exemplarId, fund);
                    break;
                case "PERIOD":
                    result = PeriodicExemplarInfo.GetPeriodicExemplarInfoByExemplarId(exemplarId);
                    break;
            }
            return result ?? null;
        }
        public static ExemplarBase CreateExemplar(string bar)
        {
            ExemplarBase result = null;
            result = BJExemplarInfo.GetExemplarByBar(bar);
            return result ?? PeriodicExemplarInfo.GetPeriodicExemplarInfoByBar(bar)
        }

        public static ExemplarBase CreateExemplarByInventoryNumber(string inventoryNumber)
        {
            ExemplarBase result = null;
            result = BJExemplarInfo.GetExemplarByInventoryNumber(inventoryNumber);
            return result ?? PeriodicExemplarInfo.GetExemplarByInventoryNumber(inventoryNumber);

        }
    }
}
