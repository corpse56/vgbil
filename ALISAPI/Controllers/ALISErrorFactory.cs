using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Net.Http;

namespace ALISAPI.ALISErrors
{

    
    public class ALISErrorFactory
    {
        public static HttpResponseMessage CreateError(string Error, HttpRequestMessage Request, HttpStatusCode httpStatusCode)
        {
            JObject jo = new JObject();
            jo.Add("Error", $"{Error}");
            return Request.CreateResponse(httpStatusCode, jo);
        }


    }
}
