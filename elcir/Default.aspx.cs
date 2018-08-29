using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using Elcir;
using Newtonsoft.Json.Linq;

public partial class _Default : System.Web.UI.Page 
{





    string IDMAIN;
    string IDBASE;
    string R_TYPE;


    BookAddInf bai;
    protected void Page_Load(object sender, EventArgs e)
    {
        IDMAIN = Request["pin"];
        IDBASE = Request["idbase"];
        int idm;
        int idb;
        int rtype;
        if (!int.TryParse(R_TYPE, out rtype))
        {
            rtype = 0;
        }
        if (!int.TryParse(IDMAIN, out idm) || (IDMAIN == "") || (IDMAIN == null) || (IDMAIN == "0") || (IDMAIN.Length > 7))
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            pInfo.Visible = false;
            pURL.Visible = false;
            lError.Text = "Неверные входные данные!";
            return;
        }
        if (!int.TryParse(IDBASE, out idb))
        {
            if ((idb != 1) || (idb != 2))
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                pInfo.Visible = false;
                pURL.Visible = false;
                lError.Text = "Неверные входные данные!";
                return;
            }
        }
        lPIN.Text = "Номер издания (PIN): "+IDMAIN;
        if (idb == 1)
        {
            try
            {
                string ff = Request.QueryString["vkey"];
                string f = HttpUtility.UrlEncode(Request.QueryString["vkey"]);
                int rt;
                if (!int.TryParse(Request["type"], out rt))
                {
                    rt = 0;
                }

                bai = new Elcir.BookAddInfBJ(idm, f, Request["idr"],rt);
            }
            catch (Exception ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                pInfo.Visible = false;
                pURL.Visible = false;
                lError.Text = "Не найдено в базе!";
                return;
            }
            lSource.Text = "Источник: Основной фонд";
        }
        else
        {
            try
            {
                int rt;
                if (!int.TryParse(Request["type"], out rt))
                {
                    rt = 0;
                }

                bai = new BookAddInfRED(idm, Request["vkey"], Request["idr"], rt);
            }
            catch (Exception ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                pInfo.Visible = false;
                pURL.Visible = false;
                lError.Text = "Не найдено в базе!";
                return;
            }
            lSource.Text = "Источник: Фонд редкой книги";

        }
        if (bai.EBook)
        {
            //lEBook.Visible = false;
            hlEBook.Visible = true;
            hlEBook.NavigateUrl = GetRedirectUrlNewViewer();
        }
        else
        {
            hlEBook.Visible = false;
            //lEBook.Visible = true;
            //lEBook.Text = "Электронная копия отсутствует!";
        }
        if (bai.ForAllReaders)
        {
            Response.Redirect(GetRedirectUrlNewViewer());
            //lAccess.Text = "Не защищено авторским правом.";
        }
        else
        {
            //lAccess.Text = "Защищено авторским правом. Для просмотра электронной копии документа вернитесь на страницу <a href=\"http://opac.libfl.ru/\">электронного каталога</a> и закажите его через личный кабинет.";
        }
        if (bai.OldBook)
        {
            //lAccess.Text += " Документ можно просмотреть только в электронном виде, так как он ветхий.";
        }
        else
        {
            //lAccess.Text += " Есть возможность заказать бумажную копию документа через личный кабинет.";
        }
        lTitle.Text = "Заглавие: " +bai.GetTitle();
        lAuthor.Text = "Автор: " +(((bai.GetAuthor()=="") || (bai.GetAuthor()==null)) ? "<нет>" :bai.GetAuthor());



    }
    private string GetRedirectUrlNewViewer()
    {
        string result = "";
        FormsAuthentication.SignOut();
        if (HttpContext.Current.Server.MachineName == "VGBIL-OPAC")
        {
            result = @"http:\\opac.libfl.ru\personal\OrderElCopy.aspx?pin=" + bai.IDMAIN.ToString() + "&idbase=" + ((int)bai.Baza).ToString();
        }
        else
        {
            result = @"http:\\192.168.4.2\personal\OrderElCopy.aspx?pin=" + bai.IDMAIN.ToString() + "&idbase=" + ((int)bai.Baza).ToString();
        }
        if (bai.ForAllReaders)
        {
            bool IsExistsLQ = GetIsExistsLQ(IDMAIN);
            if (IsExistsLQ)
            {
                result = @"http://catalog.libfl.ru/Bookreader/Viewer?bookID=BJVVV_" + IDMAIN + "&view_mode=LQ";
            }
            else
            {
                result = @"http://catalog.libfl.ru/Bookreader/Viewer?bookID=BJVVV_" + IDMAIN + "&view_mode=HQ";
            }
            //hlEBook.NavigateUrl = @"http:\\192.168.3.128\personal\OrderElCopy.aspx?pin=" + bai.IDMAIN.ToString() + "&idbase=" + ((int)bai.Baza).ToString();
        }

        return result;
        //hlEBook.NavigateUrl = @"http://localhost:12588\personal\OrderElCopy.aspx?pin=" + bai.IDMAIN.ToString() + "&idbase=" + ((int)bai.Baza).ToString();
    }
    private bool GetIsExistsLQ(string IDMAIN)
    {
        LibflAPI.ServiceSoapClient api = new LibflAPI.ServiceSoapClient();
        string book = api.GetBookInfoByID("BJVVV_" + IDMAIN);
        JObject jbook = JObject.Parse(book);
        JArray Exemplars = (JArray)jbook["Exemplars"];
        bool IsExistsLQ = false;
        foreach (JToken exm in Exemplars)
        {
            if (exm["IsElectronicCopy"].ToString().ToLower() == "false")
            {
                continue;
            }
            if (exm["IsExistsLQ"].ToString().ToLower() == "true")
            {
                IsExistsLQ = true;
            }
        }
        return IsExistsLQ;
    }


}
