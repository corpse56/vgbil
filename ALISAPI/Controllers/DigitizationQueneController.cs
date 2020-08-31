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
using LibflClassLibrary.ALISAPI.ResponseObjects.DigitizationQuene;

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
            List<DigitizationQueneItemAPIView> resultAPIView = new List<DigitizationQueneItemAPIView>();
            try
            {
                foreach (DigitizationQueneItemInfo item in result)
                {
                    DigitizationQueneItemAPIView itemAPI = DigitizationQueneItemAPIView.GetView(item);
                    resultAPIView.Add(itemAPI);
                }
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            return ALISResponseFactory.CreateResponse(resultAPIView, Request);

        }
        /// <summary>
        /// Получает последние 400 завершившие оцифровку книги
        /// </summary>
        /// <returns>очередь</returns>
        [HttpGet]
        [Route("DigitizationQuene/GetLast400Completed")]
        [ResponseType(typeof(List<DigitizationQueneItemInfo>))]
        public HttpResponseMessage GetLast400Completed()
        {
            DigitizationQueneManager manager = new DigitizationQueneManager();

            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            try
            {
                result = manager.GetLast400Completed();
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            List<DigitizationQueneItemAPIView> resultAPIView = new List<DigitizationQueneItemAPIView>();
            try
            {
                foreach (DigitizationQueneItemInfo item in result)
                {
                    DigitizationQueneItemAPIView itemAPI = DigitizationQueneItemAPIView.GetView(item);
                    resultAPIView.Add(itemAPI);
                }
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            return ALISResponseFactory.CreateResponse(resultAPIView, Request);

        }
        /// <summary>
        /// Получает удалённые из очереди книги
        /// </summary>
        /// <returns>очередь</returns>
        [HttpGet]
        [Route("DigitizationQuene/GetDeleted")]
        [ResponseType(typeof(List<DigitizationQueneItemInfo>))]
        public HttpResponseMessage GetDeleted()
        {
            DigitizationQueneManager manager = new DigitizationQueneManager();

            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            try
            {
                result = manager.GetDeleted();
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            List<DigitizationQueneItemAPIView> resultAPIView = new List<DigitizationQueneItemAPIView>();
            try
            {
                foreach (DigitizationQueneItemInfo item in result)
                {
                    DigitizationQueneItemAPIView itemAPI = DigitizationQueneItemAPIView.GetView(item);
                    resultAPIView.Add(itemAPI);
                }
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request);
            }
            return ALISResponseFactory.CreateResponse(resultAPIView, Request);

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
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

    }
}
