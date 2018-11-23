using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ALISAPI.Errors
{
    public class ALISError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public HttpStatusCode httpStatusCode;

    }
}