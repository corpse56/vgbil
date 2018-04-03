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

public partial class _Default : System.Web.UI.Page 
{
    string IDMAIN;
    string IDBASE;
    BookAddInf bai;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("~/viewer.aspx");

        IDMAIN = Request["pin"];
        IDBASE = Request["idbase"];
        int idm;
        int idb;
        if (!int.TryParse(IDMAIN, out idm) || (IDMAIN == "") || (IDMAIN == null) || (IDMAIN == "0") || (IDMAIN.Length > 7))
        {
            Panel1.Visible = false;
            Panel2.Visible = true;
            lError.Text = "Неверные входные данные!";
            return;
        }
        if (!int.TryParse(IDBASE, out idb))
        {
            if ((idb != 1) || (idb != 2))
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lError.Text = "Неверные входные данные!";
                return;
            }
        }
        
        if (idb == 1)
        {
            try
            {
                string ff = Request.QueryString["vkey"];
                string f = HttpUtility.UrlEncode(Request.QueryString["vkey"]);
                bai = new BookAddInfBJ(idm, f, Request["idr"]);
            }
            catch (Exception ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lError.Text = "Не найдено в базе!";
                return;
            }

        }
        else
        {
            try
            {
                bai = new BookAddInfRED(idm, Request["vkey"], Request["idr"]);
            }
            catch (Exception ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lError.Text = "Не найдено в базе!";
                return;
            }
        }
        if (bai.EBook)
        {
            lEBook.Visible = false;
            hlEBook.Visible = true;
            hlEBook.NavigateUrl = "~/viewer.aspx?pin=" + bai.IDMAIN.ToString() + "&idbase=" + ((int)bai.Baza).ToString();
        }
        else
        {
            hlEBook.Visible = false;
            lEBook.Visible = true;
            lEBook.Text = "Электронная копия отсутствует!";
        }
        if (bai.ForAllReaders)
        {
            lAccess.Text = "Не защищено авторским правом.";
        }
        else
        {
            lAccess.Text = "Защищено авторским правом. Для просмотра электронной копии документа вернитесь на страницу <a href=\"http://opac.libfl.ru/\">электронного каталога</a> и закажите его через личный кабинет.";
        }
        if (bai.OldBook)
        {
            lAccess.Text += " Документ можно просмотреть только в электронном виде, так как он ветхий.";
        }
        else
        {
            //lAccess.Text += " Есть возможность заказать бумажную копию документа через личный кабинет.";
        }
        lTitle.Text = "Заглавие: " +bai.GetTitle();
        lAuthor.Text = "Автор: " +(((bai.GetAuthor()=="") || (bai.GetAuthor()==null)) ? "<нет>" :bai.GetAuthor());

    }

}
