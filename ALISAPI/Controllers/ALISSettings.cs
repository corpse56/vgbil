using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALISAPI.Controllers
{
    public class ALISSettings
    {
        public static JsonSerializerSettings ALISDateFormatJSONSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy-MM-ddTHH:mm:sszz",
        };
    }
}