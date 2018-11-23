using ALISAPI.Errors;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibflClassLibrary.ALISAPI.Errors;

namespace ALISAPI.Controllers
{
    public class ReferenceBookController : ApiController
    {

        /// <summary>
        /// Получает список стран и соответствующий им код. Список необходим при регистрации.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/CountriesList")]
        [ResponseType(typeof(Dictionary<int, string>))]
        public HttpResponseMessage CountriesList()
        {
            Dictionary<int, string> Countries;
            try
            {
                Countries = ReaderInfo.GetCountriesList();
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.NotFound);
            }
            return ALISResponseFactory.CreateResponse(Countries, Request);
        }

        /// <summary>
        /// Получает список ошибок ALIS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/ALISErrorList")]
        [ResponseType(typeof(ALISErrorList))]
        public HttpResponseMessage ALISErrorList()
        {
            return ALISResponseFactory.CreateResponse(LibflClassLibrary.ALISAPI.Errors.ALISErrorList._list, Request);
        }

        //// GET: api/ReferenceBook
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/ReferenceBook/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/ReferenceBook
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ReferenceBook/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ReferenceBook/5
        //public void Delete(int id)
        //{
        //}
    }
}
