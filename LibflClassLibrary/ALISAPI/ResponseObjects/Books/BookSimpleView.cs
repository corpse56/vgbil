using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class BookSimpleView
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Annotation { get; set; }
        public string PlaceOfPublication { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string PublishDate { get; set; }
        public string Genre { get; set; }

        public string CoverURL { get; set; }

        public bool IsExistsDigitalCopy { get; set; }
        //public DigitalCopySimpleView DigitalCopy { get; set; }

        public List<ExemplarSimpleView> Exemplars { get; set; }

        public string Fund { get; set; }//русское название фонда (источник)

        public string RTF { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AvailabilityStatus {get;set;}

    }
}
