using System;
using System.Web.Security;

namespace ReadersEMailCheck_2
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack)
            {
                string name = Request.Form["name"];
                string password = Request.Form["password"];
                if (name != null && password != null
#pragma warning disable CS0618 // 'FormsAuthentication.Authenticate(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.ValidateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
                        && FormsAuthentication.Authenticate(name, password))
#pragma warning restore CS0618 // 'FormsAuthentication.Authenticate(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.ValidateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
                {
                    FormsAuthentication.SetAuthCookie(name, false);
                    Response.Redirect(Request["ReturnUrl"] ?? "/");
                }
                else
                {
                    ModelState.AddModelError("fail", "Логин или пароль не правильны." +
                        "Пожалуйста введите данные заново");
                }
            }
        }
    }
}
