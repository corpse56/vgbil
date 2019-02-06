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
using System.Globalization;
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
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };


        readonly string ALIS_ADDRESS = "http://80.250.173.142/ALISAPI/";
        //readonly string ALIS_ADDRESS = "http://localhost:27873/";
        public Form1()
        {
            InitializeComponent();
        }

        private void ReadersGet_Click(object sender, EventArgs e)
        {
            //using (WebClient client = new WebClient())
            //{
            //    client.Encoding = Encoding.UTF8;
            //    HttpRequestHeader AcceptHeader = HttpRequestHeader.Accept;
            //    client.Headers[AcceptHeader] = "application/json";
            //    string result = client.DownloadString(ALIS_ADDRESS+"Readers/189245");
            //    ReaderInfo reader = JsonConvert.DeserializeObject<ReaderInfo>(result, ALISDateFormatJSONSettings);
            //    tbResponse.Text = result;
            //}
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                HttpRequestHeader AcceptHeader = HttpRequestHeader.Accept;
                client.Headers[AcceptHeader] = "application/json";
                string result = client.DownloadString(ALIS_ADDRESS + "Readers/ByEmail/user201125@demotest.zz");
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
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode;
                //tbResponse.Text = response.Result.Content.ToString();
            }

        }

        


        private void ReadersGetByOauthToken_Click(object sender, EventArgs e)
        {
            AccessToken request = new AccessToken();
            request.TokenValue = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpYXQiOjE1NDUyMTgyNjgsImlzcyI6ImRldi1vYXV0aC5saWJmbC5ydSIsImV4cCI6MTU0NTIyMDA2OCwidWlkIjo4ODgsImNpZCI6ImxpYmZsIn0.yA400r1Trn7ngoEaBSwBGvdX1-rkkDD8UDkTZdXutQg";
            //request.TokenValue = null;
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
            request.ReaderId = 222222;
            request.DateBirth = "1996-01-03";//new DateTime(1996, 01, 03, 0, 0, 0);
            request.NewPassword = "222222";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 333;
            request.DateBirth = "1965-11-08";// new DateTime(1965, 11, 08, 7, 7, 7);
            request.NewPassword = "333";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 555;
            request.DateBirth = "1981-03-17";// new DateTime(1981, 03, 17, 7, 7, 7);
            request.NewPassword = "555";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 777;
            request.DateBirth = "1979-03-24";// new DateTime(1979, 03, 24, 7, 7, 7);
            request.NewPassword = "777";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 888;
            request.DateBirth = "1978-05-07";// new DateTime(1978, 05, 07, 7, 7, 7);
            request.NewPassword = "888";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 999;
            request.DateBirth = "1983-09-30";//new DateTime(1983, 09, 30, 7, 7, 7);
            request.NewPassword = "999";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 200500;
            request.DateBirth = "1972-02-19";// new DateTime(1972, 02, 19, 7, 7, 7);
            request.NewPassword = "200500";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

            request.ReaderId = 214444;
            request.DateBirth = "1965-04-30";// new DateTime(1965, 04, 30, 7, 7, 7);
            request.NewPassword = "214444";
            jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ChangePasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void bInsertIntoBasket_Click(object sender, EventArgs e)
        {
            CirculationInfo circulation = new CirculationInfo();
            ImpersonalBasket basket = new ImpersonalBasket();
            basket.BookIdArray = new List<string>();
            basket.BookIdArray.AddRange(new string[] { "BJVVV_1299121", "BJVVV_1304618", "REDKOSTJ_31866", "REDKOSTJ_43090" });
            basket.ReaderId = 189245;
            circulation.InsertIntoUserBasket(basket);
        }

        private void bPreRegister_Click(object sender, EventArgs e)
        {
            PreRegisterRemoteReader request = new PreRegisterRemoteReader();
            request.BirthDate = "1994-05-05";//new DateTime(1994,5,5);
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

        private void IsBirthDateMatchReaderId_Click(object sender, EventArgs e)
        {
            BirthDateMatchReaderId request = new BirthDateMatchReaderId();
            request.ReaderId = 222222;
            request.DateBirth = "1996-01-03";//new DateTime(1996, 01, 03, 0, 0, 0);
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/IsBirthDateMatchReaderId", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        private void SetPasswordLocalReader_Click(object sender, EventArgs e)
        {
            SetPasswordLocalReader request = new SetPasswordLocalReader();
            request.ReaderId = 222222;
            request.NewPassword = "222222";//new DateTime(1996, 01, 03, 0, 0, 0);
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/SetPasswordLocalReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;
            }

        }

        private void bReadersPreregister_Click(object sender, EventArgs e)
        {
            //"FamilyName" => "Kuleba"
            //"Name" => "Maksim"
            //"FatherName" => "Aleksandrovich"
            //"BirthDate" => "1985-11-29"
            //"Email" => "maksim.kuleba@gmail.com"
            //"CountryId" => "2"
            //"MobilePhone" => "+7 (986) 985–21–71"
            //"Password" => "passwd123"

            PreRegisterRemoteReader request = new PreRegisterRemoteReader();
            request.BirthDate = "1985-11-29";
            request.CountryId = 2;
            request.Email = "maksim.kuleba@gmail.com";
            request.FamilyName = "Kuleba1";
            request.FatherName = "Aleksandrovich";
            request.MobilePhone = "+7(986)9852171";
            request.Name = "Maksim";
            request.Password = "passwd123";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/PreRegisterRemoteReader", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode; ;
            }



        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            string RegisterConnectionString = "Data Source=80.250.173.142;Initial Catalog=Readers;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
            ALISReaderRemote.ReaderRemote re = new ALISReaderRemote.ReaderRemote(RegisterConnectionString);
            PreRegisterRemoteReader request = new PreRegisterRemoteReader();
            request.BirthDate = "1985-05-05";
            request.CountryId = 2;
            request.Email = "maksim.kuleba@gmail.com";
            request.FamilyName = "Кулеба";
            request.FatherName = "Александрович";
            request.MobilePhone = "79869852171";
            request.Name = "Максим";
            request.Password = "passwd123";
            DateTime BirthDate;
            if (!DateTime.TryParseExact(request.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out BirthDate))
            {
                tbResponse.Text = "Дата в неверном формате";
            }
            try
            {
                re.RegSendEmailAndSaveTemp(request.FamilyName, request.Name, request.FatherName, BirthDate, request.Email, request.CountryId, request.MobilePhone, request.Password);
            }
            catch (Exception ex)
            {
                tbResponse.Text = ex.Message;
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string RegisterConnectionString = "Data Source=80.250.173.142;Initial Catalog=Readers;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
            ALISReaderRemote.ReaderRemote re = new ALISReaderRemote.ReaderRemote(RegisterConnectionString);
            DateTime BirthDate = new DateTime(1985,5,5);
            int CountryId = 137;
            string Email = "debarkader@gmail.com";
            string FamilyName = "Кулеба";
            string FatherName = "Александрович";
            string MobilePhone = "+7(986)9852171";
            string Name = "Максим";
            string Password = "123";
            try
            {
                re.RegSendEmailAndSaveTemp(FamilyName, Name, FatherName, BirthDate, Email, CountryId, MobilePhone, Password);
            }
            catch (Exception ex)
            {
                tbResponse.Text = ex.Message;//входная строка имела неверный формат.
            }

        }

        private void bDeleteFromBasket_Click(object sender, EventArgs e)
        {
            BasketDelete bd = new BasketDelete();
            bd.BooksToDelete.Add("BJVVV_1257907");
            bd.ReaderId = 172736; 
   
            //CirculationInfo ci = new CirculationInfo();
            //ci.DeleteFromBasket(bd);

            string jsonData = JsonConvert.SerializeObject(bd, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Circulation/DeleteFromBasket", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode; ;
            }


        }

        private void bCirculationMakeOrder_Click(object sender, EventArgs e)
        {
            CirculationInfo ci = new CirculationInfo();
            MakeOrder mo = new MakeOrder();
            //mo.BookId = "BJVVV_1007658";
            //mo.ReaderId = 100000;
            //mo.OrderType = "Электронная выдача";
            //ci.MakeOrder(mo);

            //mo.BookId = "BJVVV_1310093";
            //mo.ReaderId = 100000;
            //mo.OrderType = "На дом";
            //ci.MakeOrder(mo);

            mo.BookId = "BJVVV_1078762";
            mo.ReaderId = 100000;
            mo.OrderType = "В зал";
            //ci.MakeOrder(mo);

            string jsonData = JsonConvert.SerializeObject(mo, ALISDateFormatJSONSettings);

            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Circulation/Order", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode; ;
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
