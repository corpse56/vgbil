using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Net.Http;
using LibflClassLibrary.ExportToVufind;
using ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.Errors;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ALISAPI.Controllers;

namespace ALISAPI.Errors
{

    
    public class ALISErrorFactory
    {
        public static HttpResponseMessage CreateError(string Error, HttpRequestMessage Request, HttpStatusCode httpStatusCode)
        {
            JObject jo = new JObject();
            ALISError error = ALISErrorList._list.Find(x => x.Code == Error);
            if (error != null)
            {
                jo.Add(error.Code, error.Message);
                string json = JsonConvert.SerializeObject(jo, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
                HttpResponseMessage result = Request.CreateResponse(httpStatusCode);
                result.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return result;
            }
            else
            {
                jo.Add("G002", $"Необрабатываемая ошибка: {Error}");
                string json = JsonConvert.SerializeObject(jo, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
                HttpResponseMessage result = Request.CreateResponse(httpStatusCode);
                result.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return result;
                //return Request.CreateResponse(HttpStatusCode.InternalServerError, jo);
            }
        }


    }
}
