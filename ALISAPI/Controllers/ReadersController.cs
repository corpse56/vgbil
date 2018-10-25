using ALISAPI.ALISErrors;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects;
using LibflClassLibrary.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Http;

namespace ALISAPI.Controllers
{
    public class ReadersController : ApiController
    {

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "Reader1", "Reader1444" };
        //}

        // GET api/Readers/5
        /// <summary>
        /// Получает читателя по номеру читательского билета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Readers/{id}")]
        public HttpResponseMessage Get(int id)
        {
            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReader(id);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("R004", Request, HttpStatusCode.NotFound);
            }
            return ALISResponseFactory.CreateResponse(reader, Request);
        }

        /// <summary>
        /// Получить читателя по oauth-токену
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/GetByOauthToken")]
        public HttpResponseMessage GetByOauthToken()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            AccessToken request;
            try
            {
                request = JsonConvert.DeserializeObject<AccessToken>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReaderByOAuthToken(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }

            return ALISResponseFactory.CreateResponse(reader, Request);
        }

        /// <summary>
        /// Изменить пароль читателя по номеру читателя и дате его рождения
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/ChangePassword")]
        public HttpResponseMessage ChangePassword()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            ChangePassword request;
            try
            {
                request = JsonConvert.DeserializeObject<ChangePassword>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReader(request.NumberReader);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("R004", Request, HttpStatusCode.BadRequest);
            }
            try
            {
                reader.ChangePassword(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }

            return ALISResponseFactory.CreateResponse(Request);
        }


        /// <summary>
        /// Авторизовать пользователя. Если авторизация успешна, то вернуть полный профиль. В качестве логина может быть использован как номер читателя, так и Email. 
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/Authorize")]
        public HttpResponseMessage Authorize()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            AuthorizeInfo request;
            try
            {
                request = JsonConvert.DeserializeObject<AuthorizeInfo>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }
            ReaderInfo reader;

            try
            {
                reader = ReaderInfo.Authorize(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("R001", Request, HttpStatusCode.NotFound);
            }
            return ALISResponseFactory.CreateResponse(reader, Request);
        }

        /// <summary>
        /// Получить тип логина для заданного логина. 
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Readers/GetLoginType/{login}")]
        public HttpResponseMessage GetLoginType(string Login)
        {
            string result = ReaderInfo.GetLoginType(Login);
            if (result.ToLower() == "notdefined")
            {
                return ALISErrors.ALISErrorFactory.CreateError("R003", Request, HttpStatusCode.NotFound);
            }
            LoginType type = new LoginType();
            type.LoginTypeValue = result;
            return ALISResponseFactory.CreateResponse(type, Request);
        }

        //// POST api/Readers
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/Readers/5
        //public void Put(int id, [FromBody]string value)
        //{

        //}

        //// DELETE api/Readers/5
        //public void Delete(int id)
        //{
        //}
    }
}
