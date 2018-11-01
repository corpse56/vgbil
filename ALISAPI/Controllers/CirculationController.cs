using ALISAPI.ALISErrors;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Circulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ALISAPI.Controllers
{
    public class CirculationController : ApiController
    {
        
        /// <summary>
        /// Получает Содержимое корзины читателя по номеру читательского билета
        /// </summary>
        /// <param name="idReader">Номер читательского билета</param>
        /// <returns>Список</returns>
        [HttpGet]
        [Route("Circulation/Basket/{idReader}")]
        [ResponseType(typeof(List<BookSimpleView>))]
        public HttpResponseMessage Get([Description("Номер чит билета")]int idReader)
        {

            CirculationInfo Circulation = new CirculationInfo();
            List<BookSimpleView> result = new List<BookSimpleView>();
            List<BasketInfo> basket;
            try
            {
                basket = Circulation.GetBasket(idReader);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.NotFound);
            }
            foreach (BasketInfo bi in basket)
            {
                result.Add(bi.Book);
            }

            return ALISResponseFactory.CreateResponse(result, Request);
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
