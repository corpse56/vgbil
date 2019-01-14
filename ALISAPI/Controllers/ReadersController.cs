using ALISAPI.Errors;
using ALISReaderRemote;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects;
using LibflClassLibrary.ALISAPI.ResponseObjects.General;
using LibflClassLibrary.ALISAPI.ResponseObjects.Readers;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersJSONViewers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace ALISAPI.Controllers
{
    public class ReadersController : ApiController
    {

        /// <summary>
        /// Получает читателя по номеру читательского билета
        /// </summary>
        /// <param name="id">Номер читательского билета</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Readers/{id}")]
        [ResponseType(typeof(ReaderSimpleView))]
        public HttpResponseMessage Get(int id)
        {
            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReader(id);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            ReaderSimpleView result = ReaderViewFactory.GetReaderSimpleView(reader);
            return ALISResponseFactory.CreateResponse(result, Request);
        }

        /// <summary>
        /// Получает читателя по Email
        /// </summary>
        /// <param name="email">Email читателя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Readers/ByEmail/{email}")]
        [ResponseType(typeof(ReaderSimpleView))]
        public HttpResponseMessage Get(string email)
        {
            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReader(email);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            ReaderSimpleView result = ReaderViewFactory.GetReaderSimpleView(reader);

            return ALISResponseFactory.CreateResponse(result, Request);
        }

        //этот метод решили просто убрать. он не нужен, так как токены будет проверять сервер авторизации
        ///// <summary>
        ///// Получить читателя по oauth-токену
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Readers/GetByOauthToken")]
        //[ResponseType(typeof(ReaderInfo))]
        //public HttpResponseMessage GetByOauthToken()
        //{
        //    string JSONRequest = Request.Content.ReadAsStringAsync().Result;
        //    AccessToken request;
        //    try
        //    {
        //        request = JsonConvert.DeserializeObject<AccessToken>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
        //    }
        //    catch
        //    {
        //        return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
        //    }

        //    ReaderInfo reader;
        //    try
        //    {
        //        reader = ReaderInfo.GetReaderByOAuthToken(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
        //    }

        //    return ALISResponseFactory.CreateResponse(reader, Request);
        //}


        //Этот метод распался на два метода - проверка совпадения дня рождения и установление пароля
        ///// <summary>
        ///// Изменить пароль читателя по номеру читателя и дате его рождения
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Readers/ChangePasswordLocalReader")]
        ////[ResponseType(typeof(ReaderInfo))]
        //public HttpResponseMessage ChangePasswordLocalReader()
        //{
            
        //    string JSONRequest = Request.Content.ReadAsStringAsync().Result;
        //    ChangePasswordLocalReader request;
        //    try
        //    {
        //        request = JsonConvert.DeserializeObject<ChangePasswordLocalReader>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
        //    }
        //    catch
        //    {
        //        return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
        //    }

        //    ReaderInfo reader;
        //    try
        //    {
        //        reader = ReaderInfo.GetReader(request.NumberReader);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ALISErrorFactory.CreateError("R004", Request, HttpStatusCode.NotFound);
        //    }
        //    try
        //    {
        //        reader.ChangePasswordLocalReader(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
        //    }

        //    return ALISResponseFactory.CreateResponse(Request);
        //}

        /// <summary>
        /// Узнать, соответствует ли дата рождения номеру читательского билета
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/IsBirthDateMatchReaderId")]
        public HttpResponseMessage IsBirthDateMatchReaderId()
        {

            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            BirthDateMatchReaderId request;
            try
            {
                request = JsonConvert.DeserializeObject<BirthDateMatchReaderId>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
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
                return ALISErrorFactory.CreateError("R004", Request, HttpStatusCode.NotFound);
            }
            BooleanResponse result = new BooleanResponse();
            try
            {
                result.Result = reader.IsBirthDateMatchReaderId(request);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }

            return ALISResponseFactory.CreateResponse(result, Request);
        }

        /// <summary>
        /// Установить пароль читателю. Принимает номер читателя и пароль.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/SetPasswordLocalReader")]
        public HttpResponseMessage SetPasswordLocalReader()
        {

            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            SetPasswordLocalReader request;
            try
            {
                request = JsonConvert.DeserializeObject<SetPasswordLocalReader>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderInfo reader;
            try
            {
                reader = ReaderInfo.GetReader(request.ReaderId);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError("R004", Request, HttpStatusCode.NotFound);
            }
            try
            {
                reader.SetPasswordLocalReader(request, reader);
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
        /// <param name="Login">Логин. Может быть номером читательского билета либо Email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Readers/Authorize")]
        [ResponseType(typeof(ReaderSimpleView))]
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
                HttpResponseMessage rm = ALISErrorFactory.CreateError("R001", Request, HttpStatusCode.NotFound);
                return rm;
            }
            ReaderSimpleView result = ReaderViewFactory.GetReaderSimpleView(reader);

            return ALISResponseFactory.CreateResponse(result, Request);
        }



        /// <summary>
        /// Получить тип логина для заданного логина. 
        /// </summary>
        /// <param name="Login">Логин. Может быть номером читательского билета либо Email</param>
        /// <returns>string</returns>
        [HttpGet]
        [Route("Readers/GetLoginType/{login}")]
        [ResponseType(typeof(LoginType))]
        public HttpResponseMessage GetLoginType(string Login)
        {
            string result = ReaderInfo.GetLoginType(Login);
            if (result.ToLower() == "notdefined")
            {
                return ALISErrorFactory.CreateError("R003", Request, HttpStatusCode.NotFound);
            }
            LoginType type = new LoginType();
            type.LoginTypeValue = result;
            return ALISResponseFactory.CreateResponse(type, Request);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                                                              Registration
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        string RegisterConnectionString = "Data Source=80.250.173.142;Initial Catalog=Readers;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
        /// <summary>
        /// Пререгистрация удалённого читателя. Создаёт временную запись удалённого пользователя, которую нужно подтвердить. 
        /// Высылается письмо на указанный ящик со ссылкой для подтверждения регистрации.
        /// Ссылка действительна 24 часа, после чего регистрацию нужно проходить заново.
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Readers/PreRegisterRemoteReader")]
        public HttpResponseMessage PreRegisterRemoteReader()
        {
            ALISReaderRemote.ReaderRemote re = new ALISReaderRemote.ReaderRemote(RegisterConnectionString);
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            PreRegisterRemoteReader request;
            try
            {
                request = JsonConvert.DeserializeObject<PreRegisterRemoteReader>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }
            DateTime BirthDate;
            if (!DateTime.TryParseExact(request.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate))
            {
                throw new Exception("G001");
            }
            try
            {
                re.RegSendEmailAndSaveTemp(request.FamilyName,request.Name, request.FatherName, BirthDate, request.Email, request.CountryId, request.MobilePhone, request.Password);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

        /// <summary>
        /// Подверждение регистрации. Метод должен вызываться, когда читатель нажимает на ссылку, полученную на email указанный в пререгистрации.
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Readers/ConfirmRegistrationRemoteReader")]
        public HttpResponseMessage ConfirmRegistrationRemoteReader()
        {
            ReaderRemote re = new ALISReaderRemote.ReaderRemote(RegisterConnectionString);
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            ConfirmRegistrationRemoteReader request;
            
            try
            {
                request = JsonConvert.DeserializeObject<ConfirmRegistrationRemoteReader>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            try
            {
                re.RegSaveBaseAndDelTemp(request.Url);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

        /// <summary>
        /// Изменить пароль читателя с помощью Email. Отправляет письмо на указанный адрес, в котором содержится ссылка для восстановления пароля.
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Readers/ChangePasswordByEmail")]
        public HttpResponseMessage ChangePasswordByEmail()
        {

            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            ChangePasswordByEmail request;
            try
            {
                request = JsonConvert.DeserializeObject<ChangePasswordByEmail>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderRemote re = new ReaderRemote(RegisterConnectionString);

            try
            {
                re.PasSendEmailAndSaveTemp(request.Email);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

        /// <summary>
        /// Записывает пароль удалённого читателя в базу. 
        /// Необходимо указать ссылку по которой читатель пришёл для восстановления пароля и пароль.
        /// Метод относится к сценарию восстановления пароля по email. Это последний метод сценария. После того, как ссылки проверились на существование.
        /// Нельзя путать этот метод с методом для восстановления пароля по дате рождения!!! 
        /// </summary>
        /// <returns>HTTP200</returns>
        [HttpPost]
        [Route("Readers/SetPasswordRemoteReader")]
        public HttpResponseMessage SetPasswordRemoteReader()
        {

            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            SetPasswordRemoteReader request;
            try
            {
                request = JsonConvert.DeserializeObject<SetPasswordRemoteReader>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderRemote re = new ReaderRemote(RegisterConnectionString);

            try
            {
                re.PasSaveBaseAndDelTemp(request.Url,request.Password);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }

        /// <summary>
        /// Проверить ссылку для восстановления пароля на действительность.
        /// </summary>
        /// <returns>HTTP200</returns>
        /// 
        [HttpPost]
        [Route("Readers/CheckPasswordUrl")]
        public HttpResponseMessage CheckPasswordUrl()
        {
            string JSONRequest = Request.Content.ReadAsStringAsync().Result;
            CheckPasswordUrl request;
            try
            {
                request = JsonConvert.DeserializeObject<CheckPasswordUrl>(JSONRequest, ALISSettings.ALISDateFormatJSONSettings);
            }
            catch
            {
                return ALISErrorFactory.CreateError("G001", Request, HttpStatusCode.BadRequest);
            }

            ReaderRemote re = new ReaderRemote(RegisterConnectionString);

            try
            {
                re.PasGetURL(request.Url);
            }
            catch (Exception ex)
            {
                return ALISErrorFactory.CreateError(ex.Message, Request, HttpStatusCode.InternalServerError);
            }
            return ALISResponseFactory.CreateResponse(Request);
        }



    }
}
