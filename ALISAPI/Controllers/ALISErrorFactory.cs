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
                return Request.CreateResponse(error.httpStatusCode, jo);
            }
            else
            {
                jo.Add("G002", $"Необрабатываемая ошибка: {Error}");
                return Request.CreateResponse(HttpStatusCode.InternalServerError, jo);
            }
        }


    }
}
