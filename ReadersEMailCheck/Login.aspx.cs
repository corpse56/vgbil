using LibflClassLibrary.BJUsers;
using System;
using System.Web.Security;

namespace ReadersEMailCheck_2
{
    public partial class Login : System.Web.UI.Page
    {
        private bool Authenticate(string name, string password)
        {
#pragma warning disable CS0618 // 'FormsAuthentication.Authenticate(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.ValidateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
            if (FormsAuthentication.Authenticate(name, password))
            {
                return true;
            }
#pragma warning restore CS0618 // 'FormsAuthentication.Authenticate(string, string)' is obsolete: 'The recommended alternative is to use the Membership APIs, such as Membership.ValidateUser. For more information, see http://go.microsoft.com/fwlink/?LinkId=252463.'
            BJUserInfo bui_by_login = BJUserInfo.GetUserByLogin(name, "BJVVV");
            return bui_by_login == null ? false : BJUserInfo.HashPassword(password) == bui_by_login.HashedPwd;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string name = Request.Form["name"];
                string password = Request.Form["password"];
                if (name != null && password != null && Authenticate(name, password))
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
