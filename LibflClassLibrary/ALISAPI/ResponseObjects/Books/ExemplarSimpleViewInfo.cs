using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class ExemplarSimpleViewInfo
    {
        public string MethodOfAccess { get; set; }
        public int MethodOfAccessCode { get; set; }
        public string Access { get; set; }
        public int AccessCode { get; set; }
        public int ID { get; set; }
        public string Location { get; set; }
        public int LocationCode { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryNote { get; set; }
        public string RackLocation { get; set; }
        //public string PlacingCipher { get; set; }
        public string Barcode { get; set; }
        public string Carrier { get; set; }
        public int CarrierCode { get; set; }
        //public string EditionClass { get; set; }
        public string BookUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AvailabilityStatus { get; set; }

    }
}
