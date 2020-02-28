using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.ImageCatalog;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersJSONViewers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class history : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie oauthCookie = Request.Cookies["ReaderToken"];
        oauthCookie = Request.Cookies.Get("ReaderToken");
        //oauthCookie = new HttpCookie("ReaderToken", "eyJpdiI6ImxmaU9GZUVPako3elF6dXFnY3pGV2c9PSIsInZhbHVlIjoiRGlodlNPZzgzYVFCZTZqeUlvVFAyNWFYRCtVODNcL0pMeGVcL1ZsQ29cL3Z2ZXFQNWdzR1p1b3ZkUExVQnRZYkZoZWN5Z1dnZWUraGdPMjk0QmRuY0FPSm8ySWFnbElTak51OTNiakNwd0Z1RkZSNGc4WEx0KzJXZzV0SnBGU25ycmVPOEJvbm51RE9QNlRKM3lYTktsVEllMkRNRWU4M04ra0QrTlU1Szh2ZThIdlV5ajNsdlQzWmYzZUVqVW5GQmNjMDhOMEw2TDZzbEZMTlhCRDRySVZJaUsySmRJZ0NyY3pncTVcL2NCNlFRUmtcL0ZNOWp0U1J6bjUrNjdvUXZBdlU3eEVLMDJQWFNPMmtjUXo1WjNHQXJvdz09IiwibWFjIjoiMDNlYjI0ZWU1OTY4MGRkZGU1ODU2YTE5ZjFkYmY4MjY4ZDAxYjYwZDMzNGMyNmYzNjUyYTY2OWM3OTVlN2IyMiJ9");
        if (oauthCookie == null)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), $"oauth", $"alert('cookie is null');", true);
            Response.Redirect("https://oauth.libfl.ru/?redirect_uri=https://opac.libfl.ru/OrderImCat/");
        }

        CheckAuthorizationToken token = new CheckAuthorizationToken();
        token.access_token = oauthCookie.Value;
        string jsonData = JsonConvert.SerializeObject(token);

        using (HttpClient client = new HttpClient())
        {
            var response = client.PostAsync("https://oauth.libfl.ru/checkToken", new StringContent(jsonData, Encoding.UTF8, "application/json"));
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), $"oauthCheckToken", $"alert('invalid token');", true);
                Response.Redirect("https://oauth.libfl.ru/?redirect_uri=https://opac.libfl.ru/OrderImCat/");
            }
        }
        ReaderSimpleView rsv = new ReaderSimpleView();
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {oauthCookie.Value}");
            var response = client.GetAsync("https://oauth.libfl.ru/api/getUser");
            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), $"authorizationError", $"alert('{response.Result.Content.ReadAsStringAsync()}');", true);
                Response.Redirect("https://oauth.libfl.ru/?redirect_uri=https://opac.libfl.ru/OrderImCat/");
            }
            else
            {
                Task<string> readerString = response.Result.Content.ReadAsStringAsync();
                readerString.Wait();
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), $"authorizationParse", $"alert('{readerString.Result} hoho');", true);
                rsv = JsonConvert.DeserializeObject<ReaderSimpleView>(readerString.Result);
            }
        }

        ReaderInfo reader = ReaderInfo.GetReader(rsv.ReaderId);
        Label2.Text = reader.FIO;
        if (Session["currentReader"] != null)
        {
            Session.Remove("currentReader");
            Session.Add("currentReader", reader);
        }
        else
        {
            Session.Add("currentReader", reader);
        }
        if (!Page.IsPostBack)
        {
            ShowHistoryOrders();
        }
    }

    private void ShowHistoryOrders()
    {
        if (Session["currentReader"] == null)
        {
            Response.Redirect("https://oauth.libfl.ru/?redirect_uri=https://opac.libfl.ru/OrderImCat/");
        }
        ReaderInfo reader = (ReaderInfo)Session["currentReader"];
        ImageCatalogCirculationManager circ = new ImageCatalogCirculationManager();
        List<ICOrderInfo> userOrders = circ.GetHistoryOrdersByReader(reader.NumberReader);
        //Session.Add("userOrders", userOrders);
        DataTable dataSource = new DataTable();
        dataSource.Columns.Add("orderId");
        dataSource.Columns.Add("num");
        dataSource.Columns.Add("comment");
        dataSource.Columns.Add("statusName");
        dataSource.Columns.Add("mainSideUrl");
        dataSource.Columns.Add("selectedSideUrl");
        int i = 1;
        foreach (ICOrderInfo order in userOrders)
        {
            DataRow row = dataSource.NewRow();
            row["orderId"] = order.Id;
            row["num"] = i++;
            row["comment"] = order.Comment;
            row["statusName"] = order.StatusName;
            row["mainSideUrl"] = order.Card.MainSideUrl;
            row["selectedSideUrl"] = order.SelectedSideUrl;
            dataSource.Rows.Add(row);
        }
        gwBasket.DataSource = dataSource;
        ((BoundField)gwBasket.Columns[0]).DataField = "orderId";
        ((BoundField)gwBasket.Columns[1]).DataField = "mainSideUrl";
        ((BoundField)gwBasket.Columns[2]).DataField = "selectedSideUrl";
        ((BoundField)gwBasket.Columns[3]).DataField = "num";
        ((BoundField)gwBasket.Columns[6]).DataField = "comment";
        ((BoundField)gwBasket.Columns[7]).DataField = "statusName";
        gwBasket.DataBind();
    }

    protected void gwBasket_DataBound(object sender, EventArgs e)
    {

    }

    protected void gwBasket_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image img = (Image)e.Row.FindControl("mainSideImage");
            img.ImageUrl = e.Row.Cells[1].Text;//order.Card.MainSideUrl;
            img = (Image)e.Row.FindControl("selectedSideImage");
            img.ImageUrl = e.Row.Cells[2].Text;// order.SelectedSideUrl;

        }
    }
}