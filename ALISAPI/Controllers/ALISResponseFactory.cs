using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;

namespace ALISAPI.Controllers
{
    public class ALISResponseFactory
    {
        public static HttpResponseMessage CreateResponse(object Message, HttpRequestMessage Request)
        {

            ExplicitlyMarkDateTimesAsUtcWithCollections(Message);
            string json = JsonConvert.SerializeObject(Message, Formatting.Indented,  ALISSettings.ALISDateFormatJSONSettings);
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return result;
        }
        public static HttpResponseMessage CreateResponse(HttpRequestMessage Request)
        {
            HttpResponseMessage result = Request.CreateResponse(HttpStatusCode.OK);
            return result;
        }
        //тут устанавливается тип даты Local, чтобы JSON приводил даты к UTC и сам вычитал время часового пояса
        private static void ExplicitlyMarkDateTimesAsUtcWithCollections(object Message)
        {
            if (Message is System.Collections.IEnumerable)
            {
                foreach (var item in Message as System.Collections.IEnumerable)
                {
                    ExplicitlyMarkDateTimesAsUtc(item);
                }
            }
            else
            {
                ExplicitlyMarkDateTimesAsUtc(Message);
            }
        }
        private static void ExplicitlyMarkDateTimesAsUtc<T>(T obj) where T : class
        {
            Type t = obj.GetType();
            
            // Loop through the properties.
            PropertyInfo[] props = t.GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo p = props[i];
                // If a property is DateTime or DateTime?, set DateTimeKind to DateTimeKind.Utc.
                if (p.PropertyType == typeof(DateTime))
                {
                    DateTime date = (DateTime)p.GetValue(obj, null);
                    date = DateTime.SpecifyKind(date, DateTimeKind.Local);
                    p.SetValue(obj, date, null);
                }
                // Same check for nullable DateTime.
                else if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? date = (DateTime?)p.GetValue(obj, null);
                    if (date.HasValue)
                    {
                        DateTime? newDate = DateTime.SpecifyKind(date.Value, DateTimeKind.Local);
                        p.SetValue(obj, newDate, null);
                    }
                }
            }
        }
    }
}