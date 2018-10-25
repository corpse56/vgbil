using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace ALISAPI.Controllers
{
    public class ALISResponseFactory
    {
        public static HttpResponseMessage CreateResponse(object Message, HttpRequestMessage Request)
        {
            string json = JsonConvert.SerializeObject(Message, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return result;
        }
        public static HttpResponseMessage CreateResponse(HttpRequestMessage Request)
        {
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            return result;
        }
    }
}