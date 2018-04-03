using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataProviderAPI.ValueObjects
{
    /// <summary>
    /// Сводное описание для ExemplarInfo
    /// </summary>
    public class ExemplarInfo
    {
        public ExemplarInfo(int idData)
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


        //public ElectronicExemplarInfo ElectronicCopyInfo;


    }
   
}