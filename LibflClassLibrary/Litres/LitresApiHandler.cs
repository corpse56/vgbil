using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Litres
{
    public class LitresApiHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private const string secretKey = "LtjHQvTpX99snuUF6neP0mbe7cH08FGtvnwYAqrh8bDocEeJWIwbeFVK0v4w9BDkloxzbBQmsrqEJMqKH15QTododUuC4iMVriz5oSpGLm68gzXRWlo9jJrXqSXohzwd";
        private const string app = "560376917";

        private static string SendRequest(string jsonData)
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
               { "jdata", jsonData },
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            Task<HttpResponseMessage> response = client.PostAsync("https://catalit.litres.ru/catalitv2", content);
            return response.Result.Content.ReadAsStringAsync().Result;
        }
        internal string w_create_sid()
        {

            StringBuilder sb = new StringBuilder();
            StringWriter strwriter = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(strwriter);

            writer.WriteStartObject();

            writer.WritePropertyName("app");
            writer.WriteValue(app);

            writer.WritePropertyName("time");
            string time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz");
            writer.WriteValue(time);

            writer.WritePropertyName("sha");
            string sha = $"{time}{secretKey}";
            sha = Utilities.Extensions.sha256(sha);
            writer.WriteValue(sha);

            writer.WritePropertyName("requests");
            writer.WriteStartArray();
            writer.WriteStartObject();
            writer.WritePropertyName("func");
            writer.WriteValue("w_create_sid");
            writer.WritePropertyName("id");
            writer.WriteValue("my_sid");
            writer.WritePropertyName("param");

            writer.WriteStartObject();

            writer.WritePropertyName("login");
            writer.WriteValue("403255407");
            writer.WritePropertyName("pwd");
            writer.WriteValue("799*654");
            writer.WritePropertyName("sid");
            writer.WriteValue("78838d3c7fbf640a4c52956569bef3c685");

            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.WriteEndArray();
            writer.WriteEndObject();

            string jsonData = sb.ToString();
            string jsonResponse = LitresApiHandler.SendRequest(jsonData);
            return get_sid_from_response(jsonResponse);


        }

        private static string get_sid_from_response(string response)
        {
            //response = "{ \"success\": true, \"my_sid\" : { \"currency\" : \"RUB\", \"success\" : true, \"sid\" : \"587bd14m5q4p4m3s2vcc4q0tf5a5b927\", \"country\" : \"RUS\", \"region\" : \"Moscow\", \"city\" : \"Moscow\"}, \"time\" : \"2019-08-14T18:12:59+03:00\"}";

            JObject data = (JObject)JsonConvert.DeserializeObject(response);
            bool success = (bool)data["success"];
            if (success)
            {
                if ((bool)data["my_sid"]["success"])
                {
                    return data["my_sid"]["sid"].ToString();
                }
            }
            throw new Exception("L004");
        }
        private static LitresInfo get_account_from_response(string response)
        {
            JObject data = (JObject)JsonConvert.DeserializeObject(response);
            bool success = (bool)data["success"];
            if (success)
            {
                if ((bool)data["new_reader"]["success"])
                {
                    LitresInfo result = new LitresInfo();
                    result.Login = data["new_reader"]["lib_card"].ToString();
                    result.Password = data["new_reader"]["password"].ToString();
                    return result;
                }
            }
            throw new Exception("L004");
        }
        internal LitresInfo w_biblio_reader_create(string sid)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter strwriter = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(strwriter);

            writer.WriteStartObject();

            writer.WritePropertyName("app");
            writer.WriteValue(app);

            writer.WritePropertyName("time");
            string time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz");
            writer.WriteValue(time);

            writer.WritePropertyName("sha");
            string sha = $"{time}{secretKey}";
            sha = Utilities.Extensions.sha256(sha);
            writer.WriteValue(sha);

            writer.WritePropertyName("sid");
            writer.WriteValue(sid);

            writer.WritePropertyName("requests");
            writer.WriteStartArray();
            writer.WriteStartObject();
            writer.WritePropertyName("func");
            writer.WriteValue("w_biblio_reader_create");
            writer.WritePropertyName("id");
            writer.WriteValue("new_reader");
            writer.WritePropertyName("param");

            writer.WriteStartObject();

            writer.WritePropertyName("libhouse");
            writer.WriteValue("193956553");
            writer.WritePropertyName("name");
            writer.WriteValue("litres libfl");
            writer.WritePropertyName("birth_date");
            writer.WriteValue("2000-01-01");
            writer.WritePropertyName("mail");
            string email = $"a{DateTime.Now.ToBinary()}@libfl.ru";
            writer.WriteValue(email);
            //writer.WritePropertyName("phone");
            //writer.WriteValue("79261234567");

            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.WriteEndArray();
            writer.WriteEndObject();
           
            string jsonData = sb.ToString();
            string jsonResponse = LitresApiHandler.SendRequest(jsonData);
            return get_account_from_response(jsonResponse);
        }


    }
}
