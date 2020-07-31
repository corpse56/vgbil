using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Circulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using ALISAPI.Errors;
using LibflClassLibrary.DigitizationQuene;
using LibflClassLibrary.ALISAPI.RequestObjects.DigitizationQuene;

namespace ALISAPI.Controllers
{
    public class DigitizationQueneController : ApiController
    {

        /// <summary>
        /// Получает очередь на ойифровку
        /// </summary>
        /// <returns>очередь</returns>
        [HttpGet]
        [Route("DigitizationQuene/GetQuene")]
        [ResponseType(typeof(List<DigitizationQueneItemInfo>))]
        public HttpResponseMessage GetQuene()
        {
            DigitizationQueneManager manager = new DigitizationQueneManager();

            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            try
            {
                result = manager.GetQuene();
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);

        }

        /// <summary>
        /// вставляет книгу в очередь
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("DigitizationQuene/AddToQuene")]
        public HttpResponseMessage AddToQuene()
        {
            DigitizationQueneManager manager = new DigitizationQueneManager();
            AddToQueneItemInfo request;
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            try
            {
                request = JsonConvert.DeserializeObject<AddToQueneItemInfo>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }

            try
            {
                manager.AddToQuene(request.bookId, request.readerId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

    }
}
