using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ALISAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //             name: "OrdersById",
            //             routeTemplate: "Circulation/Orders/ById/{OrderId}",
            //             defaults: new { OrderId = System.Web.Http.RouteParameter.Optional }
            //           );
           // GlobalConfiguration.Configuration.Routes.MapHttpRoute(
           //  name: "GetReaderByEmail",
           //  routeTemplate: "ALISAPI/Readers/ByEmail/{email}",
           //  defaults: new { email = System.Web.Http.RouteParameter.Optional }
           //);
            // GlobalConfiguration.Configuration.Routes.MapHttpRoute(
            //  name: "GetLoginType",
            //  routeTemplate: "Readers/GetLoginType/{login}",
            //  defaults: new { login = System.Web.Http.RouteParameter.Optional }
            //);

            //var formatters = GlobalConfiguration.Configuration.Formatters;
            //formatters.Remove(formatters.XmlFormatter);

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            //json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            //json formatter settings
            //var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddThh::mm:sszz";
            //jsonFormatter.SerializerSettings.DateFormatString = "yyyy";
        }
    }
}
