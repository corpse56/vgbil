using DataProviderAPI.ValueObjects;
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
            ReaderInfo reader = ReaderInfo.GetReader(id);
            string json = JsonConvert.SerializeObject(reader, Formatting.Indented);

            HttpResponseMessage result = this.Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(json,Encoding.UTF8, "application/json");
            return result;
            //return Request.CreateResponse(HttpStatusCode.OK).Content. //, reader);
            //return json;

            //вот так нужно вставлять json а не строкой.
            //string yourJson = GetJsonFromSomewhere();
            //var response = this.Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(yourJson, Encoding.UTF8, "application/json");

        }



        /// <summary>
        /// Получить читателя по oauth-токену
        /// </summary>
        /// <param name="token">Токен, выданный читателю при авторизации</param>
        /// <returns>ReaderInfo</returns>
        [HttpGet]
        [Route("Readers/GetByOauthToken/{token}")]
        public HttpResponseMessage GetByOauthToken(string token)
        {
            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReaderByOAuthToken(token);
            }
            catch (Exception ex)
            {
                //HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                //{
                //    Content = new StringContent("R002"),
                //    ReasonPhrase = "Токен не найден или не действителен"
                //};
                //throw new HttpResponseException(resp);
                JObject jo = new JObject();

                jo.Add("Error", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, jo);
            }
            return Request.CreateResponse(HttpStatusCode.OK, reader);
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
                HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("R003"),
                    ReasonPhrase = "Неизвестный тип логина"
                };
                throw new HttpResponseException(resp);

            }
            JObject jo = new JObject();
            jo.Add("LoginType", result);
            //return IHttpActionResult
            return Request.CreateResponse(HttpStatusCode.OK, jo);

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
