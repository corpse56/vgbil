using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReadersEMailCheck
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string email01 = Request["Text1"]; //или реквест форм?
                answer01.Text = email01;
            }
            if (Session["ReaderEmail"] != null)
            {
                //answer01.Text = Session["ReaderEmail"].ToString();
                //answer01.Text = "456";
            } else
            {
                //answer01.Text = "123";
                //answer01.Text = Request.Form["Text1"];
            }

        }
        protected void SignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage(); // Александр сказал, что не работает
            //Response.Redirect("~/Login.aspx");
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            //answer01.Text = "aaa";
            //Session["ReaderEmail"] = Request.Form["Text1"];
            //Session["ReaderEmail"] = "12345";
            //FormsAuthentication.RedirectToLoginPage();
            string email01 = Request.Form["Text1"];
            answer01.Text = email01;
            //answer01.Text = "Text1";
        }
    }


}