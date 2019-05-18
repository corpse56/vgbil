using Newtonsoft.Json;
using System;
using System.Web.Configuration;
using System.Web.Security;
using System.Net.Http;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ReadersEMailCheck_2
{
    internal class UserEmail
    {
        public string Email { get; internal set; }
    }
    public class ReaderSimpleView
    {
        public string Email { get; set; }
    }
    public partial class WebForm1 : System.Web.UI.Page
    {
        public Formatting ALISDateFormatJSONSettings { get; private set; }
        public ReaderSimpleView ReaderSimpleView2 { get; private set; }
        public JsonConverter ALISDateFormatJSONSettings2 { get; private set; }
        protected string EMC(string s)
        {
            if (!(new EmailAddressAttribute().IsValid(s)))
                return "ОШИБКА ВХОДНЫХ ДАННЫХ";
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
            try
            {
                ReaderSimpleView2 = JsonConvert.DeserializeObject<ReaderSimpleView>(result);
            }
            catch (JsonReaderException)
            {
                return "ОШИБКА ОТВЕТА СЕРВЕРА";
            }
            return (ReaderSimpleView2.Email != null) ? "ЧИТАТЕЛЬ ЕСТЬ" : "ЧИТАТЕЛЯ НЕТ";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string email01 = Request["Text1"];
                question01.InnerText = email01.ToUpper();
                answer01.InnerText = EMC(email01);
            }
        }
        protected void SignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage(); // кто-то сказал, что не работает
            //Response.Redirect("~/Login.aspx");
        }
    }
}