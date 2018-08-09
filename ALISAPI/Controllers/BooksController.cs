using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALISAPI.Controllers
{
    public class BooksController : ApiController
    {

        [HttpGet]
        [Route("Books/{Id}")]
        public HttpResponseMessage Get(string id)
        {
            BookBase bb = BJBookInfo.GetBookInfoByPIN(1456705, "BJVVV");
            BJBookInfo bbb = BJBookInfo.GetBookInfoByPIN(1456705, "BJVVV");

            return Request.CreateResponse(HttpStatusCode.OK, bb);
        }

    }
}
