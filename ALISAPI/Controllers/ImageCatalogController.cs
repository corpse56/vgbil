using ALISAPI.Errors;
using LibflClassLibrary.ImageCatalog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ALISAPI.Controllers
{
    public class ImageCatalogController : ApiController
    {
        /// <summary>
        /// Получает информацию о книгах в электронном каталоге, которые привязаны к карточке из имидж-каталога.Выходной параметр CardTableName может принимать следующие значения: MAIN, AV, PERIODICAL, SUBSCRIPT
        /// </summary>
        /// <param name="cardFileName">Имя файла карточки</param>
        /// <returns>Привязанные книги. Выходной параметр CardTableName может принимать следующие значения: MAIN, AV, PERIODICAL, SUBSCRIPT</returns>
        [HttpGet]
        [Route("ImageCatalog/OccurencesForCardToCatalog/{cardFileName}")]
        [ResponseType(typeof(List<CardToCatalogInfo>))]
        public HttpResponseMessage OccurencesForCardToCatalog([Description("Имя файла карточки")]string cardFileName)
        {
            ImageCatalogCirculationManager cm = new ImageCatalogCirculationManager();
            List<CardToCatalogInfo> result = new List<CardToCatalogInfo>();
            
            try
            {
                result = cm.GetBooksOnCard(cardFileName);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request);
            }
            return ALISResponseFactory.CreateResponse(result, Request);
        }

    }
}
