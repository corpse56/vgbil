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
using LibflClassLibrary.Litres;

namespace ALISAPI.Controllers
{
    public class LitresController : ApiController
    {

        /// <summary>
        /// Если аккаунт Литрес присвоен читателю, то возвращает учётные данные для входа в Литрес (логин и пароль)
        /// </summary>
        /// <param name="ReaderId">Номер читательского билета</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Litres/Account/{ReaderId}")]
        [ResponseType(typeof(LitresInfo))]
        public HttpResponseMessage LitresAccount([Description("Номер чит билета")]int ReaderId)
        {

            LitresAccountManager accManager = new LitresAccountManager();
            LitresInfo result;
            try
            {
                result = accManager.GetLitresAccount(ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);

        }

        /// <summary>
        /// Выдаёт аккаунт Литрес читателю. Получает номер читателя, назначает ему аккаунт литреса и возвращает его.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Litres/AssignAccount/{ReaderId}")]
        [ResponseType(typeof(LitresInfo))]
        public HttpResponseMessage AssignLitresAccount(int ReaderId)
        {

            LitresAccountManager accManager = new LitresAccountManager();
            LitresInfo result;
            try
            {
                result = accManager.AssignLitresAccount(ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);
        }


    }
}
