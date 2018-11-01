using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Books.BookJSONViewers;
using LibflClassLibrary.Readers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALISAPI_TEST
{
    public partial class Form1 : Form
    {
        public static JsonSerializerSettings ALISDateFormatJSONSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy-MM-ddTHH:mm:sszz",
        };


        readonly string ALIS_ADDRESS = "http://80.250.173.142/ALISAPI/";
        //readonly string ALIS_ADDRESS = "http://localhost:27873/";
        public Form1()
        {
            InitializeComponent();
        }

        private void ReadersGet_Click(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                HttpRequestHeader AcceptHeader = HttpRequestHeader.Accept;
                client.Headers[AcceptHeader] = "application/json";
                string result = client.DownloadString(ALIS_ADDRESS+"Readers/189245");
                ReaderInfo reader = JsonConvert.DeserializeObject<ReaderInfo>(result, ALISDateFormatJSONSettings);
                tbResponse.Text = result;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            BJDatabaseWrapper dbWrapper = new BJDatabaseWrapper("BJVVV");
            DataTable record = dbWrapper.GetBJRecord(17132);
            int cnt = record.Rows.Count;
        }

        private void ReadersAuthorize_Click(object sender, EventArgs e)
        {
            AuthorizeInfo request = new AuthorizeInfo();
            request.login = "189245";
            request.password = "12";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS+"Readers/Authorize/", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        


        private void ReadersGetByOauthToken_Click(object sender, EventArgs e)
        {
            AccessToken request = new AccessToken();
            request.TokenValue = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1MzkzNTQzOTEsImlzcyI6Im9hdXRoLmxpYmZsLnJ1IiwiZXhwIjoxNTM5MzU0OTkxLCJ1aWQiOjE4OTI0NSwiY2lkIjoiYWdncmVnaW9uIn0.ignwAP_dJFkkl_I45VRC0HDAvKPKGDXkgdNy4XqrUY4";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS+"Readers/GetByOauthToken", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BJBookInfo b = BJBookInfo.GetBookInfoByPIN(1456705, "BJVVV");
            BookJSONShortViewer viewer = new BookJSONShortViewer();
            tbResponse.Text = viewer.GetView(b);
        }

        private void bChangePwd_Click(object sender, EventArgs e)
        {
            ChangePassword request = new ChangePassword();
            request.NumberReader = 222222;
            request.DateBirth = new DateTime(1996, 01, 03, 7, 7, 7);
            request.NewPassword = "222222";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);
            
            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BookSimpleView book = ViewFactory.GetBookSimpleView("BJVVV_34");
        }
    }

         
            //and if you wanted to POST some value:

            //using (var client = new WebClient())
            //{
            //    client.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    client.Headers[HttpRequestHeader.Accept] = "application/json";
            //    var data = Encoding.UTF8.GetBytes("{\"foo\":\"bar\"}");
            //    byte[] result = client.UploadData("http://example.com/values", "POST", data);
            //    string resultContent = Encoding.UTF8.GetString(result, 0, result.Length);
            //}

}
