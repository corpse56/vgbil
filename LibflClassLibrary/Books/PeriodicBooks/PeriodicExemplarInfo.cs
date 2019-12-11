using LibflClassLibrary.Books.PeriodBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodicBooks
{
    public class PeriodicExemplarInfo : ExemplarBase
    {
        public string Number { get; set; }
        public string PublishYear { get; set; }

        //виртуальные свойства
        public override string InventoryNumber { get; set; }
        public override string Cipher { get; set; }
        public override string Author { get; set; }
        public override string Title { get; set; }
        public override string Rack { get; set; }
        public override string Bar { get; set; }
        public override string Location { get; set; }
        public override string Language { get; set; }
        public override string PublicationClass { get; set; }

        public static PeriodicExemplarInfo GetPeriodicExemplarInfoByBar(string bar)
        {
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo result = loader.GetExemplarByBar(bar);
            return result;
        }

        public static PeriodicExemplarInfo GetPeriodicExemplarInfoByExemplarId(int exemplarId)
        {
            //ExemplarId для периодики - это IDZ поля штрихкод в таблице PI !!!
            //он же и является ПИНом книги для периодики!
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo result = loader.GetPeriodicExemplarInfoByExemplarId(exemplarId);
            return result;
        }

        internal static ExemplarBase GetExemplarByInventoryNumber(string inventoryNumber)
        {
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo result = loader.GetExemplarByInventoryNumber(inventoryNumber);
            return result;
        }
    }
}
