using LibflClassLibrary.ImageCatalog;
using LibflClassLibrary.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //HttpCookie cookie = Request.Cookies["ReaderToken"];
        //cookie = Request.Cookies.Get("ReaderToken");
        ////здесь ещё проверка авторизации
        ////Label1.Text = cookie.Value;

        //CheckAuthorizationToken token = new CheckAuthorizationToken();
        ////token.access_token = cookie.Value;
        //string jsonData = JsonConvert.SerializeObject(token);

        //using (HttpClient client = new HttpClient())
        //{
        //    var response = client.PostAsync("https://oauth.libfl.ru/checkToken", new StringContent(jsonData, Encoding.UTF8, "application/json"));
        //    //tbResponse.Text = response.Result.Content.ReadAsStringAsync().Result + " " + response.Result.StatusCode; ;
        //    if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
        //    {
        //        //Response.Redirect("https://oauth.libfl.ru");
        //    }
        //    else
        //    {
        //        Label1.Text = "11111";
        //    }
        //}
        //Label1.Text = "11111";



        JObject mockCookie = new JObject();
        mockCookie.Add("orderType", "book");
        mockCookie.Add("cardId", "000029274");
        mockCookie.Add("cardSide", "90");
        mockCookie.Add("comment", "Мне нужен 90-й том");


        string s = mockCookie.ToString();
        ICCookie mcookie = JsonConvert.DeserializeObject<ICCookie>(mockCookie.ToString());

        ReaderInfo reader = ReaderInfo.GetReader(189245);
        Label2.Text = reader.FIO;
        //ImageCardInfo card = ImageCardInfo.GetCard("000029274", "090", true);
        ICOrderInfo order = ICOrderInfo.CreateOrder(mcookie.cardId, mcookie.cardSide, reader.NumberReader, mcookie.comment);
        //mainSideImage.ImageUrl = order.Card.MainSideUrl;
        //selectedSideImage.ImageUrl = order.SelectedSideUrl;



        DataTable dataSource = new DataTable();
        //dataSource.Columns.Add("mainSideImage", typeof(Image));
        dataSource.Columns.Add("num", typeof(int));
        DataRow row = dataSource.NewRow();
        row["num"] = 1;// order.Card.MainSideImage;
        gwBasket.DataSource = dataSource;
        ((BoundField)gwBasket.Columns[0]).DataField = "num";
        gwBasket.DataBind();


    }

    protected void gwBasket_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        JObject mockCookie = new JObject();
        mockCookie.Add("orderType", "book");
        mockCookie.Add("cardId", "000029274");
        mockCookie.Add("cardSide", "90");
        mockCookie.Add("comment", "Мне нужен 90-й том");


        string s = mockCookie.ToString();
        ICCookie mcookie = JsonConvert.DeserializeObject<ICCookie>(mockCookie.ToString());

        ReaderInfo reader = ReaderInfo.GetReader(189245);
        Label2.Text = reader.FIO;
        //ImageCardInfo card = ImageCardInfo.GetCard("000029274", "090", true);
        ICOrderInfo order = ICOrderInfo.CreateOrder(mcookie.cardId, mcookie.cardSide, reader.NumberReader, mcookie.comment);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image img = (Image)e.Row.FindControl("mainSideImage");

            //bool PrNoCreated = bool.Parse(gwBasket.DataKeys[e.Row.RowIndex]["PrNoCreated"].ToString());
            img.ImageUrl = order.Card.MainSideUrl;
           
        }
    }
}