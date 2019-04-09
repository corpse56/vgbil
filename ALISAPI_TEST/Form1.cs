using LibflClassLibrary.ALISAPI.RequestObjects.Circulation;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Books.BookJSONViewers;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Readers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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


        //readonly string ALIS_ADDRESS = "https://opac.libfl.ru/ALISAPI/";
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
            request.password = "1234";
            string jsonData = JsonConvert.SerializeObject(request, ALISDateFormatJSONSettings);
            for (int i = 0; i < 100000; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (HttpClient client = new HttpClient())
                {
                    var response = client.PostAsync(ALIS_ADDRESS + "Readers/Authorize/", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                    tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode + sw.Elapsed.Milliseconds;
                    //tbResponse.Text = response.Result.Content.ToString();
                }
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

            request.ReaderId = 100000;
            request.DateBirth = "1985-11-01";// new DateTime(1965, 11, 08, 7, 7, 7);
            request.NewPassword = "100000";
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
            mo.BookId = "REDKOSTJ_1151";
            mo.ReaderId = 15;
            mo.OrderTypeId = 2;
            ci.MakeOrder(mo);


            //CirculationInfo ci = new CirculationInfo();
            //MakeOrder mo = new MakeOrder();
            ////mo.BookId = "BJVVV_1007658";
            ////mo.ReaderId = 100000;
            ////mo.OrderType = "Электронная выдача";
            ////ci.MakeOrder(mo);

            ////mo.BookId = "BJVVV_1310093";
            ////mo.ReaderId = 100000;
            ////mo.OrderType = "На дом";
            ////ci.MakeOrder(mo);

            //mo.BookId = "BJVVV_193768";
            //mo.ReaderId = 10000;
            //mo.OrderTypeId = 2;
            ////ci.MakeOrder(mo);

            //string jsonData = JsonConvert.SerializeObject(mo, ALISDateFormatJSONSettings);

            //using (HttpClient client = new HttpClient())
            //{
            //    var response = client.PostAsync(ALIS_ADDRESS + "Circulation/Order", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            //    tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode; ;
            //}


        }

        private void bFindAllAccessCodes_Click(object sender, EventArgs e)
        {
            List<BJBookInfo> books = new List<BJBookInfo>();
            int A1000 = 0, A1001 = 0, A1002 = 0, A1003 = 0, A1004 = 0, A1005 = 0, A1006 = 0, A1007 = 0, A1008 = 0, A1009 = 0, A1010 = 0,
                A1011 = 0, A1012 = 0, A1013 = 0, A1014 = 0, A1015 = 0, A1016 = 0, A1017 = 0, A1999 = 0;
            bool flag = false;
            for (int i = 1; i < 1482287; i++)
            {
                BJBookInfo book = BJBookInfo.GetBookInfoByPIN($"BJVVV_{i.ToString()}");
                if (book.Exemplars.Count == 0)
                {
                    continue;
                }
                foreach(BookExemplarBase exemplar in book.Exemplars)
                {

                    switch (((BJExemplarInfo)exemplar).ExemplarAccess.Access)
                    {
                        case 1000:
                            if (A1000 < 2)
                            {
                                A1000++;
                                books.Add(book);
                            }
                            break;
                        case 1001:
                            if (A1001 < 2)
                            {
                                A1001++;
                                books.Add(book);
                            }
                            break;
                        case 1002:
                            if (A1002 < 2)
                            {
                                A1002++;
                                books.Add(book);
                            }
                            break;
                        case 1003:
                            if (A1003 < 2)
                            {
                                A1003++;
                                books.Add(book);
                            }
                            break;
                        case 1004:
                            if (A1004 < 2)
                            {
                                A1004++;
                                books.Add(book);
                            }
                            break;
                        case 1005:
                            if (A1005 < 2)
                            {
                                A1005++;
                                books.Add(book);
                            }
                            break;
                        case 1006:
                            if (A1006 < 2)
                            {
                                A1006++;
                                books.Add(book);
                            }
                            break;
                        case 1007:
                            if (A1007 < 2)
                            {
                                A1007++;
                                books.Add(book);
                            }
                            break;
                        case 1008:
                            if (A1008 < 2)
                            {
                                A1008++;
                                books.Add(book);
                            }
                            break;
                        case 1009:
                            if (A1009 < 2)
                            {
                                A1009++;
                                books.Add(book);
                            }
                            break;
                        case 1010:
                            if (A1010 < 2)
                            {
                                A1010++;
                                books.Add(book);
                            }
                            break;
                        case 1011:
                            if (A1011 < 2)
                            {
                                A1011++;
                                books.Add(book);
                            }
                            break;
                        case 1012:
                            if (A1012 < 2)
                            {
                                A1012++;
                                books.Add(book);
                            }
                            break;
                        case 1013:
                            if (A1013 < 2)
                            {
                                A1013++;
                                books.Add(book);
                            }
                            break;
                        case 1014:
                            if (A1014 < 2)
                            {
                                A1014++;
                                books.Add(book);
                            }
                            break;
                        case 1015:
                            if (A1015 < 2)
                            {
                                A1015++;
                                books.Add(book);
                            }
                            break;
                        case 1016:
                            if (A1016 < 2)
                            {
                                A1016++;
                                books.Add(book);
                            }
                            break;
                        case 1017:
                            if (A1017 < 2)
                            {
                                A1017++;
                                books.Add(book);
                            }
                            break;
                        case 1999:
                            if (A1999 < 2)
                            {
                                A1999++;
                                books.Add(book);
                            }
                            break;

                    }
                    if ((A1000 == 2) && (A1001 == 2) && (A1002 == 2) 
                        //&& (A1003 == 2) 
                        //&& (A1004 == 2) 
                        && (A1005 == 2) 
                        && (A1006 == 2) 
                        && (A1007 == 2) 
                        //&& (A1008 == 2) 
                        //&& (A1009 == 2) 
                        && (A1010 == 2) && (A1011 == 2) && (A1012 == 2) && (A1013 == 2) 
                        //&& (A1014 == 2) 
                        && (A1015 == 2) 
                        //&& (A1016 == 2) 
                        && (A1017 == 2) && (A1999 == 2))
                    {
                        flag = true;
                        break;
                    }

                }
                if (flag) break;
            }
            foreach (BJBookInfo b in books)
            {
                tbResponse.Text += $"{b.Id}; ";
            }
        }

        private void bCirculationGetOrders_Click(object sender, EventArgs e)
        {
            CirculationInfo ci = new CirculationInfo();
            List<OrderInfo> list = ci.GetOrders(333);
            string json = JsonConvert.SerializeObject(list);
            tbResponse.Text = json;
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
