using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibflClassLibrary;
using LibflClassLibrary.ImageCatalog;

//{
//    "orderType" : "book",
//    "cardId" : "000001245",
//    "cardSide" : "7",
//    "comment" : "texttexttext"
//}

namespace ImCatOrder
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpCookie cookie = Request.Cookies.Get("");

            //здесь ещё проверка авторизации



            JObject mockCookie = new JObject();
            mockCookie.Add("orderType", "book");
            mockCookie.Add("cardId", "000029274");
            mockCookie.Add("cardSide", "90");
            mockCookie.Add("comment", "Мне нужен 90-й том");
            string s = mockCookie.ToString();
            ICCookie cookie = JsonConvert.DeserializeObject<ICCookie>(mockCookie.ToString());



            //LibflClassLibrary.ImageCatalog.
        }
    }
}