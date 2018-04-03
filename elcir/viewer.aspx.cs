using System;
using System.Collections;
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
using Elcir;
using System.Text;


public partial class viewer : System.Web.UI.Page
{


    string IDMAIN;
    string IDBASE;
    BookAddInf bai;
    protected void Page_Load(object sender, EventArgs e)
    
    {
         
        /*ClientScriptManager cs = Page.ClientScript;

        // Check to see if the startup script is already registered.
        if (!cs.IsStartupScriptRegistered(this.GetType(), "startpace"))
        {
            StringBuilder cstext1 = new StringBuilder();
            cstext1.Append("<script type=text/javascript> Pace.Start </");
            cstext1.Append("script>");

            cs.RegisterStartupScript(this.GetType(), "startpace", cstext1.ToString());
        }*/
        if (!Page.IsPostBack)
        {
           // Page_Load(sender, e);
        }
        {

            IDMAIN = Request["pin"];
            IDBASE = Request["idbase"];
            int idm;
            int idb;
            hlEBook.NavigateUrl = "~/default.aspx?pin=" + IDMAIN + "&idbase=" + IDBASE;
            if (!int.TryParse(IDMAIN, out idm) || (IDMAIN == "") || (IDMAIN == null) || (IDMAIN == "0"))
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
                    //string f = HttpUtility..Urlcode((Request["vkey"]);
                    //string f =HttpUtility.UrlDecode(Request.QueryString["vkey"]);
                    string ff = Request.QueryString["vkey"];
                    string f = HttpUtility.UrlDecode(Request.QueryString["vkey"]);
                    int rt;
                    if (!int.TryParse(Request["type"], out rt))
                    {
                        rt = 0;
                    }

                    bai = new Elcir.BookAddInfBJ(idm, ff, Request["idr"], rt);
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
                    string vkey = Request["vkey"];
                    string idr = Request["idr"];
                    int rt;
                    if (!int.TryParse(Request["type"], out rt))
                    {
                        rt = 0;
                    }
                    bai = new Elcir.BookAddInfRED(idm, Request["vkey"], Request["idr"],rt);
                }
                catch (Exception ex)
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    lError.Text = "Не найдено в базе!";
                    return;
                }
            }
            if (!bai.ForAllReaders)
            {
                if ((bai.IDReader == "") || (bai.IDReader == null) || (bai.ViewKey == "") || (bai.IDReader == null))
                {
                    Panel1.Visible = false;
                    Panel2.Visible = true;
                    lError.Text = "Просмотр только через <a href=\"http://opac.libfl.ru/personal/loginemployee.aspx\">личный кабинет</a>! Поместите эту книгу в корзину в электронном каталоге и закажите её в личном кабинете!";
                       // +         " Инструкция по заказу электронных копий документов находится здесь.";
                    return;
                }
                else
                {
                    try
                    {
                        if (!bai.UserHaveOrder())
                        {
                            Panel1.Visible = false;
                            Panel2.Visible = true;
                            lError.Text = "Читатель (" + bai.IDReader + ") не заказывал данную электронную копию или истекло или не подошло время заказа. Смотрите разрешенные даты просмотра и более подробную информацию в <a href=\"http://opac.libfl.ru/personal/loginemployee.aspx\">личном кабинете</a>.";
                            return;
                        }
                        else
                        {
                        }
                    }
                    catch
                    {
                        Panel1.Visible = false;
                        Panel2.Visible = true;
                        lError.Text = "Неверные входные данные!";
                        return;
                    }
                }
                
                string FullName = bai.GetReaderNameByID();
                bool Agreement = bai.GetAgreement();
                if (Agreement)
                {
                    Panel3.Visible = false;
                    Panel1.Visible = true;
                    Panel2.Visible = false;
                }
                else
                {
                    LiteralControl objPanelText = Panel3.Controls[0] as LiteralControl;
                    objPanelText.Text = "  &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp ВНИМАНИЕ!!! <br/><br/>" +
                        "1. Вам (" + FullName + ") предоставляется доступ к электронному изданию только в личных целях " +
                    "без возможности получения личной выгоды.<br/><br/>2. Вам (" + FullName +
                    ") запрещается создавать копии произведений " +
                    "или их частей в цифровой или печатной форме с предоставленного электронного издания.<br/><br/>" +
                    "3. Вы (" + FullName + ") несете ответственность за соблюдение авторских прав в соответствии с ГК РФ часть 4, " +
                    "глава 69, ст. 1250, 1252 и 1253. <br/><br/>";
                    if (CheckBox1.Checked)
                    {
                        bAgree.Enabled = true;
                    }
                    Panel3.Visible = true;
                    Panel1.Visible = false;
                    Panel2.Visible = false;
                    return;
                }
                SlideShowExtender1.ContextKey = bai.GetPath();
                return;
            }
            if (!bai.EBook)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                lError.Text = "Электронная копия отсутствует!";
                return;
            }
            string sessionId = this.Session.SessionID;
            if (!Page.IsPostBack)
            {
                bai.InsertELOPENEDWAR(sessionId);
            }


            SlideShowExtender1.ContextKey = bai.GetPath();

        }
        /*if (!cs.IsStartupScriptRegistered(this.GetType(), "stoppace"))
        {
            StringBuilder cstext1 = new StringBuilder();
            cstext1.Append("<script type=text/javascript> Pace.Stop</");
            cstext1.Append("script>");

            cs.RegisterStartupScript(this.GetType(), "stoppace", cstext1.ToString());
        }*/


       
    }
    protected void bAgree_Click(object sender, EventArgs e)
    {
        bai.InsertAgreement();
        Page_Load(sender, e);
    }
    //protected void bCancel_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterClientScriptBlock(this, typeof(string), "tt",
    //"<script language=\"javascript\" type=\"text/javascript\">window.close();</SCRIPT>", false);

    //}
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked)
            bAgree.Enabled = true;
        else
            bAgree.Enabled = false;
    }
    protected void btnTurn_Click(object sender, EventArgs e)
    {

        string tmp = SlideShowExtender1.ContextKey;
        
    }




}
