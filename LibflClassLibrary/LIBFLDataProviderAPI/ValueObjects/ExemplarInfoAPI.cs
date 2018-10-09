using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Runtime.Serialization;


namespace DataProviderAPI.ValueObjects
{
    /// <summary>
    /// Сводное описание для ExemplarInfo
    /// </summary>

    public class ExemplarInfoAPI
    {
        public ExemplarInfoAPI(int idData)
        {
            this._iddata = idData;
        }

        private int _iddata;
        public int IdData
        {
            get
            {
                return _iddata;
            }
        }

        public string InventoryNumber { get; set; }//899p
        public string EditionClass { get; set; }//921c
        public string Location { get; set; }//899a
        public string FundOrCollectionName { get; set; }//899b
        public string Barcode { get; set; } //899w
        public string PlacingCipher { get; set; }//899j
        public string InventoryNumberNote { get; set; }//899x



        //для электронных экземпдяров
        public bool IsElectronicCopy = false;

        //для информации о выдаче
        public bool IsIssued { get; set; }
        public string IDReaderTooked { get; set; }
        public List<string> IDReadersTooked { get; set; } = new List<string>();
        public string DateReturn { get; set; }



    }
   
}