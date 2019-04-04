using ALISAPI.Errors;
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
                book = ViewFactory.GetBookSimpleViewWithAvailabilityStatus(id);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            if (book == null) return ALISErrorFactory.CreateError("B001", Request);
            return ALISResponseFactory.CreateResponse(book, Request);
        }
        /// <summary>
        /// Получает сведения об электронной копии по Id книги
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Books/ElectronicCopy/{id}")]
        [ResponseType(typeof(ElectronicCopyFullView))]
        public HttpResponseMessage GetElectronicCopyFullView(string id)
        {
            ElectronicCopyFullView book;
            try
            {
                book = ViewFactory.GetElectronicCopyFullView(id);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            if (book == null) return ALISErrorFactory.CreateError("B001", Request);
            return ALISResponseFactory.CreateResponse(book, Request);
        }

    }
}
