using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Text;

namespace ReadersEMailCheck
{

    public class ReaderSimpleView
    {
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public int ReaderId { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public bool IsRemoteReader { get; set; }
        public bool IsPasswordExists { get; set; }
        public string R004 { get; set; }
    }

    public partial class WebForm1 : System.Web.UI.Page
    {
        public Formatting ALISDateFormatJSONSettings { get; private set; }
        public ReaderSimpleView ReaderSimpleView2 { get; private set; }

        public JsonConverter ALISDateFormatJSONSettings2 { get; private set; }

        protected string EMC(string s)
        {
            string ALIS_ADDRESS = WebConfigurationManager.AppSettings["ALIS_ADDRESS"];
            UserEmail ue = new UserEmail();
            //ue.Email = "debarkader@gmail.com";
            ue.Email = s;
            string jsonData = JsonConvert.SerializeObject(ue, ALISDateFormatJSONSettings);
            string result;
            using (HttpClient client = new HttpClient())
            {
                var response = client.PostAsync(ALIS_ADDRESS + "Readers/ByEmail/", new StringContent(jsonData, Encoding.UTF8, "application/json"));
                result = response.Result.Content.ReadAsStringAsync().Result;
            }

            ReaderSimpleView2 = JsonConvert.DeserializeObject<ReaderSimpleView>(result);

            return (ReaderSimpleView2.Email != null) ? "ЧИТАТЕЛЬ ЕСТЬ" : "ЧИТАТЕЛЯ НЕТ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string email01 = Request["Text1"];
                question01.InnerText = email01.ToUpper();
                answer01.Text = EMC(email01);
            }
        }
        protected void SignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage(); // Александр сказал, что не работает
            //Response.Redirect("~/Login.aspx");
        }
    }
}