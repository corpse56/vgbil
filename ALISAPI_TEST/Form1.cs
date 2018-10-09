using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
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
                string result = client.DownloadString("http://80.250.173.142/Readers/189245");
                ReaderInfo reader = JsonConvert.DeserializeObject<ReaderInfo>(result);
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
            request.password = "123";
            string jsonData = JsonConvert.SerializeObject(request);


            //ReaderInfo r = ReaderInfo.Authorize(request);

            //using (WebClient client = new WebClient())
            //{

            //    client.Encoding = Encoding.UTF8;
            //    //HttpRequestHeader AcceptHeader = HttpRequestHeader.Accept;
            //    //client.Headers[AcceptHeader] = "application/json";
            //    HttpRequestHeader ContentType = HttpRequestHeader.ContentType;
            //    client.Headers[ContentType] = "application/json";
            //    byte[] data = Encoding.UTF8.GetBytes(jsonData);
            //    byte[] result = client.UploadData(new Uri ("http://80.250.173.142/Readers/Authorize/"), data);
            //    jsonData = result.ToString();
            //    ReaderInfo reader = JsonConvert.DeserializeObject<ReaderInfo>(jsonData);
            //    tbResponse.Text = jsonData;
            //}

            using (HttpClient client = new HttpClient())
            {

                var response = client.PostAsync("http://80.250.173.142/Readers/Authorize/", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result;

            }

            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://80.250.173.142/Readers/Authorize");
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Method = "POST";
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            //{
            //    //string json = new JavaScriptSerializer().Serialize(new
            //    //{
            //    //    Username = "myusername",
            //    //    Password = "pass"
            //    //});
            //    streamWriter.Write(jsonData);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    tbResponse.Text = result;
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BJBookInfo b = BJBookInfo.GetBookInfoByPIN(1456705,"BJVVV");
            BookJSONShortViewer viewer = new BookJSONShortViewer();
            tbResponse.Text = viewer.GetView(b);
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
