// Uncomment the following to provide samples for PageResult<T>. Must also add the Microsoft.AspNet.WebApi.OData
// package to your project.
////#define Handle_PageResultOfT

using ALISAPI.Controllers;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.ALISAPI.ResponseObjects.Readers;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersJSONViewers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
#if Handle_PageResultOfT
using System.Web.Http.OData;
#endif

namespace ALISAPI.Areas.HelpPage
{
    /// <summary>
    /// Use this class to customize the Help Page.
    /// For example you can set a custom <see cref="System.Web.Http.Description.IDocumentationProvider"/> to supply the documentation
    /// or you can provide the samples for the requests/responses.
    /// </summary>
    public static class HelpPageConfig
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters",
            MessageId = "ALISAPI.Areas.HelpPage.TextSample.#ctor(System.String)",
            Justification = "End users may choose to merge this string with existing localized resources.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "bsonspec",
            Justification = "Part of a URI.")]
        public static void Register(HttpConfiguration config)
        {
            //// Uncomment the following to use the documentation from XML documentation file.
            config.SetDocumentationProvider(new XmlDocumentationProvider(HttpContext.Current.Server.MapPath("~/App_Data/Documentation/documentation.xml")));

            //// Uncomment the following to use "sample string" as the sample for all actions that have string as the body parameter or return type.
            //// Also, the string arrays will be used for IEnumerable<string>. The sample objects will be serialized into different media type 
            //// formats by the available formatters.
            //config.SetSampleObjects(new Dictionary<Type, object>
            //{
            //    {typeof(string), "sample string"},
            //    {typeof(IEnumerable<string>), new string[]{"sample 1", "sample 2"}}
            //});

            // Extend the following to provide factories for types not handled automatically (those lacking parameterless
            // constructors) or for which you prefer to use non-default property values. Line below provides a fallback
            // since automatic handling will fail and GeneratePageResult handles only a single type.
#if Handle_PageResultOfT
            config.GetHelpPageSampleGenerator().SampleObjectFactories.Add(GeneratePageResult);
#endif

            // Extend the following to use a preset object directly as the sample for all actions that support a media
            // type, regardless of the body parameter or return type. The lines below avoid display of binary content.
            // The BsonMediaTypeFormatter (if available) is not used to serialize the TextSample object.
            config.SetSampleForMediaType(
                new TextSample("Binary JSON content. See http://bsonspec.org for details."),
                new MediaTypeHeaderValue("application/bson"));

            //// Uncomment the following to use "[0]=foo&[1]=bar" directly as the sample for all actions that support form URL encoded format
            //// and have IEnumerable<string> as the body parameter or return type.
            //config.SetSampleForType("[0]=foo&[1]=bar", new MediaTypeHeaderValue("application/x-www-form-urlencoded"), typeof(IEnumerable<string>));

            //// Uncomment the following to use "1234" directly as the request sample for media type "text/plain" on the controller named "Values"
            //// and action named "Put".
            //config.SetSampleRequest("1234", new MediaTypeHeaderValue("text/plain"), "Values", "Put");

            //// Uncomment the following to use the image on "../images/aspNetHome.png" directly as the response sample for media type "image/png"
            //// on the controller named "Values" and action named "Get" with parameter "id".
            //config.SetSampleResponse(new ImageSample("../images/aspNetHome.png"), new MediaTypeHeaderValue("image/png"), "Values", "Get", "id");

            //// Uncomment the following to correct the sample request when the action expects an HttpRequestMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Get" were having string as the body parameter.
            //config.SetActualRequestType(typeof(string), "Values", "Get");

            //// Uncomment the following to correct the sample response when the action returns an HttpResponseMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Post" were returning a string.
            //config.SetActualResponseType(typeof(string), "Values", "Post");


            ReaderInfo reader = ReaderInfo.GetReader(189245);
            //Readers.Get
            ReaderSimpleView rsv = ReaderViewFactory.GetReaderSimpleView(reader);
            string json = JsonConvert.SerializeObject(rsv, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Readers", "Get");


            //Readers.GetByOauthToken
            AccessToken token = new AccessToken();
            token.TokenValue = "jhgfjfg*%&$*FKGfkfKfI^(*&^5&^TGVfjtgfdtre$E65r86T87t)(*7goYGV986T98^&Go8yg";
            json = JsonConvert.SerializeObject(token, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "GetByOauthToken");

            json = JsonConvert.SerializeObject(rsv, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Readers", "GetByOauthToken");

            //Readers.ChangePasswordLocalReader
            ChangePasswordLocalReader password = new ChangePasswordLocalReader();
            password.DateBirth = "1984-02-14";// new DateTime(1984, 2, 14);
            password.NewPassword = "123";
            password.NumberReader = 189245;
            json = JsonConvert.SerializeObject(password, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "ChangePasswordLocalReader");

            //Readers.GetLoginType
            LoginType ltype = new LoginType();
            ltype.LoginTypeValue = "Email";
            json = JsonConvert.SerializeObject(ltype, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Readers", "GetLoginType");


            //Readers.Authorize
            AuthorizeInfo aut = new AuthorizeInfo();
            aut.login = "189245";
            aut.password = "123";
            json = JsonConvert.SerializeObject(aut, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "Authorize");
            config.SetSampleForType(aut, new MediaTypeHeaderValue("application/json"), typeof(AuthorizeInfo));
            config.SetActualRequestType(typeof(AuthorizeInfo), "Readers", "Authorize");
            json = JsonConvert.SerializeObject(rsv, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Readers", "Authorize");

            //Books.Get
            BookSimpleView book;
            book = ViewFactory.GetBookSimpleView("BJVVV_1411951");
            json = JsonConvert.SerializeObject(book, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Books", "Get");

            //Circulation.InsertIntoUserBasket
            ImpersonalBasket basket = new ImpersonalBasket();
            basket.BookIdArray = new List<string>();
            basket.BookIdArray.AddRange(new string[] { "BJVVV_1299121", "BJVVV_1304618", "REDKOSTJ_31866", "REDKOSTJ_43090" });
            basket.IDReader = 189245;
            json = JsonConvert.SerializeObject(basket, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Circulation", "InsertIntoUserBasket");

            //Circulation.Basket
            CirculationInfo circ = new CirculationInfo();
            List<BasketInfo> UserBasket = circ.GetBasket(888);
            json = JsonConvert.SerializeObject(UserBasket, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Circulation", "Basket");

            //Circulation.Orders
            List<OrderInfo> UserOrders = circ.GetOrders(888);
            json = JsonConvert.SerializeObject(UserOrders, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleResponse(json, new MediaTypeHeaderValue("application/json"), "Circulation", "Orders");


            //Readers.PreRegisterRemoteReader
            PreRegisterRemoteReader re = new PreRegisterRemoteReader();
            re.BirthDate = "1975-05-05";//new DateTime(1975, 05, 05);
            re.CountryId = 137;
            re.Email = "mail@example.com";
            re.FamilyName = "Иванов";
            re.FatherName = "Иванович";
            re.MobilePhone = "89551234567";
            re.Name = "Иван";
            re.Password = "!@#";
            json = JsonConvert.SerializeObject(re, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "PreRegisterRemoteReader");

            //Readers.ConfirmRegistrationRemoteReader
            ConfirmRegistrationRemoteReader c = new ConfirmRegistrationRemoteReader();
            c.Url = "https://oauth.libfl.ru/activate/<activation code>";
            json = JsonConvert.SerializeObject(c, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "ConfirmRegistrationRemoteReader");

            //Readers.ChangePasswordByEmail
            ChangePasswordByEmail em = new ChangePasswordByEmail();
            em.Email = "sample@example.com";
            json = JsonConvert.SerializeObject(em, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "ChangePasswordByEmail");

            //Readers.SetPasswordRemoteReader
            SetPasswordRemoteReader set = new SetPasswordRemoteReader();
            set.Url = "https://oauth.libfl.ru/recovery/<recovery code>";
            set.Password = "!@#";
            json = JsonConvert.SerializeObject(set, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "SetPasswordRemoteReader");

            //Readers.CheckPasswordUrl
            CheckPasswordUrl check = new CheckPasswordUrl();
            check.Url = "https://oauth.libfl.ru/recovery/<recovery code>";
            json = JsonConvert.SerializeObject(check, Formatting.Indented, ALISSettings.ALISDateFormatJSONSettings);
            config.SetSampleRequest(json, new MediaTypeHeaderValue("application/json"), "Readers", "CheckPasswordUrl");



        }

#if Handle_PageResultOfT
        private static object GeneratePageResult(HelpPageSampleGenerator sampleGenerator, Type type)
        {
            if (type.IsGenericType)
            {
                Type openGenericType = type.GetGenericTypeDefinition();
                if (openGenericType == typeof(PageResult<>))
                {
                    // Get the T in PageResult<T>
                    Type[] typeParameters = type.GetGenericArguments();
                    Debug.Assert(typeParameters.Length == 1);

                    // Create an enumeration to pass as the first parameter to the PageResult<T> constuctor
                    Type itemsType = typeof(List<>).MakeGenericType(typeParameters);
                    object items = sampleGenerator.GetSampleObject(itemsType);

                    // Fill in the other information needed to invoke the PageResult<T> constuctor
                    Type[] parameterTypes = new Type[] { itemsType, typeof(Uri), typeof(long?), };
                    object[] parameters = new object[] { items, null, (long)ObjectGenerator.DefaultCollectionSize, };

                    // Call PageResult(IEnumerable<T> items, Uri nextPageLink, long? count) constructor
                    ConstructorInfo constructor = type.GetConstructor(parameterTypes);
                    return constructor.Invoke(parameters);
                }
            }

            return null;
        }
#endif
    }
}