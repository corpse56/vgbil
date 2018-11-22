using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Books.BookJSONViewers;
using LibflClassLibrary.Circulation;
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
            request.TokenValue = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1NDI3MDk5NTQsImlzcyI6Im9hdXRoLmxpYmZsLnJ1IiwiZXhwIjoxNTQyNzExNzU0LCJ1aWQiOjIwMTEyNSwiY2lkIjoicHVibGljcnUifQ.r39ktS9enEd1dRCV9auiEmCfFxOOj7SfdcNDcEmb6z8";
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
            ChangePasswordLocalReader request = new ChangePasswordLocalReader();
            request.NumberReader = 222222;
            request.DateBirth = new DateTime(1996, 01, 03, 0, 0, 0);
            request.NewPassword = "222222";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 333;
            request.DateBirth = new DateTime(1965, 11, 08, 7, 7, 7);
            request.NewPassword = "333";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 555;
            request.DateBirth = new DateTime(1981, 03, 17, 7, 7, 7);
            request.NewPassword = "555";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 777;
            request.DateBirth = new DateTime(1979, 03, 24, 7, 7, 7);
            request.NewPassword = "777";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 888;
            request.DateBirth = new DateTime(1978, 05, 07, 7, 7, 7);
            request.NewPassword = "888";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 999;
            request.DateBirth = new DateTime(1983, 09, 30, 7, 7, 7);
            request.NewPassword = "999";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 200500;
            request.DateBirth = new DateTime(1972, 02, 19, 7, 7, 7);
            request.NewPassword = "200500";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePassword", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.NumberReader = 214444;
            request.DateBirth = new DateTime(1965, 04, 30, 7, 7, 7);
            request.NewPassword = "214444";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

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

        private void bInsertIntoBasket_Click(object sender, EventArgs e)
        {
            CirculationInfo circulation = new CirculationInfo();
            ImpersonalBasket basket = new ImpersonalBasket();
            basket.BookIdArray = new List<string>();
            basket.BookIdArray.AddRange(new string[] { "BJVVV_1299121", "BJVVV_1304618", "REDKOSTJ_31866", "REDKOSTJ_43090" });
            basket.IDReader = 189245;
            circulation.InsertIntoUserBasket(basket);
        }

        private void bPreRegister_Click(object sender, EventArgs e)
        {
            PreRegisterRemoteReader request = new PreRegisterRemoteReader();
            request.BirthDate = new DateTime(1994,5,5);
            request.CountryId = 1;
            request.Email = "nomailbox@mail.ru";
            request.FamilyName = "Kolobok";
            request.FatherName = "Kolobkovich";
            request.Name = "Kolobkov";
            request.MobilePhone = "89262878871";
            request.Password = "123";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);
            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/PreRegister", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }


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
