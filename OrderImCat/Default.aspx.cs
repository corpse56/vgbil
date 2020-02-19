using ALISAPI.Errors;
using LibflClassLibrary.ALISAPI.Errors;
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
        ReaderInfo reader = ReaderInfo.GetReader(189245);
        Label2.Text = reader.FIO;


///////////////////////////////////////////////////////////////////////////////////
        JObject mockCookie = new JObject();
        mockCookie.Add("orderType", "book");
        mockCookie.Add("cardId", "000029274");
        mockCookie.Add("cardSide", "90");
        mockCookie.Add("comment", "Мне нужен 90-й том");


        string s = mockCookie.ToString();
        ICCookie mcookie = JsonConvert.DeserializeObject<ICCookie>(mockCookie.ToString());

        try
        {
            ICOrderInfo.CreateOrder(mcookie.cardId, mcookie.cardSide, reader.NumberReader, mcookie.comment);
        }
        catch (Exception ex)
        {
            ImageCardInfo card = ImageCardInfo.GetCard(mcookie.cardId, false);
            ALISError error = ALISErrorList._list.Find(x => x.Code == ex.Message);
            string userMessage = (error != null) ? error.Message : ex.Message;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", $"alert('Вы не можете заказать книгу по выбранной карточке. {userMessage}');", true);
            //return;
        }
        ImageCatalogCirculationManager circ = new ImageCatalogCirculationManager();
        List<ICOrderInfo> userOrders = circ.GetActiveOrdersByReader(reader.NumberReader);
        Session.Add("userOrders", userOrders);
        Session.Add("reader", reader);
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