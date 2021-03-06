﻿using ALISAPI.Errors;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibflClassLibrary.ALISAPI.Errors;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Circulation;

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
                return ALISErrorFactory.CreateError(ex.Message, Request);
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

        /// <summary>
        /// Получает список соответствия кодов доступа типам заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/AccessCodeToOrderType")]
        public HttpResponseMessage AccessCodeToOrderType()
        {
            return ALISResponseFactory.CreateResponse(KeyValueMapping.AccessCodeToOrderTypeId, Request);
        }

        /// <summary>
        /// Получает список соответствия кодов доступа названиям кодов доступа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/AccessCodeToNameALISVersion")]
        public HttpResponseMessage AccessCodeToName()
        {
            return ALISResponseFactory.CreateResponse(KeyValueMapping.AccessCodeToNameALISVersion, Request);
        }

        /// <summary>
        /// Получает список соответствия кодов местонахождения имени метонахождения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/LocationCodeToName")]
        public HttpResponseMessage LocationCodeToName()
        {
            return ALISResponseFactory.CreateResponse(KeyValueMapping.LocationCodeToName, Request);
        }

        /// <summary>
        /// Получает список всех возможных способов заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/OrderTypeList")]
        public HttpResponseMessage OrderTypeList()
        {
            return ALISResponseFactory.CreateResponse(OrderTypes.ListView, Request);
        }

        /// <summary>
        /// Получает список всех возможных статусов заказа
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/StatusNameList")]
        public HttpResponseMessage StatusNameList()
        {
            return ALISResponseFactory.CreateResponse(CirculationStatuses.ListView, Request);
        }

        /// <summary>
        /// Получает маппинг статусов на "сахарный" статус на UI. Нужен для информирования о готовности заказа.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ReferenceBook/StatusNameToReadiness")]
        public HttpResponseMessage StatusNameToReadiness()
        {
            return ALISResponseFactory.CreateResponse(KeyValueMapping.StatusNameToReadiness, Request);
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
