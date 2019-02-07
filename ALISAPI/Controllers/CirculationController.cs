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

namespace ALISAPI.Controllers
{
    public class CirculationController : ApiController
    {

        /// <summary>
        /// Получает содержимое корзины читателя по номеру читательского билета
        /// </summary>
        /// <param name="ReaderId">Номер читательского билета</param>
        /// <returns>Содержимое корзины</returns>
        [HttpGet]
        [Route("Circulation/Basket/{ReaderId}")]
        [ResponseType(typeof(List<BasketInfo>))]
        public HttpResponseMessage Basket([Description("Номер чит билета")]int ReaderId)
        {

            CirculationInfo Circulation = new CirculationInfo();
            List<BasketInfo> basket;
            try
            {
                basket = Circulation.GetBasket(ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }
            return ALISResponseFactory.CreateResponse(basket, Request);
        }

        /// <summary>
        /// Удаляет книги из корзины читателя. Метод принимает в теле номер читателя и ID книг, которые нужно удалить.
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Circulation/DeleteFromBasket")]
        public HttpResponseMessage DeleteFromBasket()
        {

            CirculationInfo Circulation = new CirculationInfo();
            BasketDelete request;
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            try
            {
                request = JsonConvert.DeserializeObject<BasketDelete>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }

            try
            {
                Circulation.DeleteFromBasket(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }


        /// <summary>
        /// Получает заказы читателя и их статусы. Описание книги и её экземпляры включено в объект заказа.
        /// </summary>
        /// <param name="ReaderId">Номер читательского билета</param>
        /// <returns>Заказы читателя</returns>
        [HttpGet]
        [Route("Circulation/Orders/{ReaderId}")]
        [ResponseType(typeof(List<OrderInfo>))]
        public HttpResponseMessage Orders([Description("Номер чит билета")]int ReaderId)
        {
            CirculationInfo Circulation = new CirculationInfo();
            List<OrderInfo> result = new List<OrderInfo>();
            try
            {
                result = Circulation.GetOrders(ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);
        }
        /// <summary>
        /// Получает историю заказов читателя. Описание книги и её экземпляры включено в объект истории.
        /// </summary>
        /// <param name="ReaderId">Номер читательского билета</param>
        /// <returns>Заказы читателя</returns>
        [HttpGet]
        [Route("Circulation/OrdersHistory/{ReaderId}")]
        [ResponseType(typeof(List<OrderHistoryInfo>))]
        public HttpResponseMessage OrdersHistory([Description("Номер чит билета")]int ReaderId)
        {
            CirculationInfo Circulation = new CirculationInfo();
            List<OrderHistoryInfo> result = new List<OrderHistoryInfo>();
            try
            {
                result = Circulation.GetOrdersHistory(ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);
        }

        /// <summary>
        /// Помещает заказ в историю.
        /// </summary>
        /// <param name="OrderId">Id заказа</param>
        [HttpPost]
        [Route("Circulation/MoveOrderToHistory/{OrderId}")]
        public HttpResponseMessage MoveOrderToHistory(int OrderId)
        {
            CirculationInfo Circulation = new CirculationInfo();
            try
            {
                Circulation.DeleteOrder(OrderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }


        // возможные действия вошли в корзину не надо отдельного метода.
        ///// <summary>
        ///// Выдаёт варианты действия с книгой. Их всего 4: Выдать на дом, выдать в зал, Выдать электронную копию и действия невозможны.
        ///// </summary>
        ///// <param name="idReader">Номер читательского билета</param>
        ///// <returns>Варианты действий читателя с книгой</returns>
        //[HttpPost]
        //[Route("Circulation/AcceptableActions")]
        //[ResponseType(typeof(List<BookSimpleView>))]
        //public HttpResponseMessage AcceptableActions()
        //{
        //    //получить массив ID книг и номер читателя
        //    List<string> result = new List<string>();
        //    try
        //    {
        //        //возвратить возможные действия с книгой для этого читателя
        //    }
        //    catch (Exception ex)
        //    {
        //        return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
        //    }
        //    return ALISResponseFactory.CreateResponse(result, Request);
        //}

        /// <summary>
        /// Вставить в персональную корзину читателя список id книг. Метод нужно вызывать после авторизации.
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Circulation/InsertIntoUserBasket")]
        //[ResponseType(typeof(ReaderInfo))]
        public HttpResponseMessage InsertIntoUserBasket()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            ImpersonalBasket request;
            try
            {
                request = JsonConvert.DeserializeObject<ImpersonalBasket>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }

            CirculationInfo Circulation = new CirculationInfo();


            try
            {
                Circulation.InsertIntoUserBasket(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

        /// <summary>
        /// Сделать заказ на выдачу книги.
        /// </summary>
        /// <returns>HTTP204</returns>
        [HttpPost]
        [Route("Circulation/Order")]
        //[ResponseType(typeof(ReaderInfo))]
        public HttpResponseMessage Order()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            MakeOrder request;
            try
            {
                request = JsonConvert.DeserializeObject<MakeOrder>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request);
            }

            CirculationInfo Circulation = new CirculationInfo();


            try
            {
                Circulation.MakeOrder(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }





















        //// GET: api/Circulation
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Circulation/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Circulation
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Circulation/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Circulation/5
        //public void Delete(int id)
        //{
        //}
    }
}
