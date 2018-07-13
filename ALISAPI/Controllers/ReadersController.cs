using DataProviderAPI.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ALISAPI.Controllers
{
    public class ReadersController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "Reader1", "Reader1444" };
        }

        // GET api/Readers/5
        public string Get(int id)
        {
            ReaderInfo reader = ReaderInfo.GetReader(id);

            return JsonConvert.SerializeObject(reader, Formatting.Indented);
        }


        /// <summary>
        /// Получить читателя по oauth-токену
        /// </summary>
        /// <param name="token">Токен, выданный читателю при авторизации</param>
        /// <returns>ReaderInfo</returns>
        /// 
        [HttpGet]
        [Route("Readers/GetByOauthToken/{token}")]
        public string GetByOauthToken(string token)
        {
            return "reader" + token;
        }

        // POST api/Readers
        public void Post([FromBody]string value)
        {
        }

        // PUT api/Readers/5
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/Readers/5
        public void Delete(int id)
        {
        }
    }
}
