using ALISAPI.ALISErrors;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ALISAPI.Controllers
{
    public class BooksController : ApiController
    {

        /// <summary>
        /// Получает сведения о книге по ID книги
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Books/{id}")]
        [ResponseType(typeof(BookSimpleView))]
        public HttpResponseMessage Get(string id)
        {
            BookSimpleView book;
            try
            {
                book = ViewFactory.GetBookSimpleView(id);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("G002", Request, HttpStatusCode.NotFound);
            }
            if (book == null) return ALISErrorFactory.CreateError("B001", Request, HttpStatusCode.NotFound);
            return ALISResponseFactory.CreateResponse(book, Request);
        }

    }
}
