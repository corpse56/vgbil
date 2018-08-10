using LibflClassLibrary.Readers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
