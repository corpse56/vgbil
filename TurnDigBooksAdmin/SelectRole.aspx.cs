using LibflClassLibrary.BJUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelectRole : System.Web.UI.Page
{
    BJUserInfo bjUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        bjUser = (BJUserInfo)Session["bjUser"];
        foreach (UserStatus role in bjUser.UserStatus)
        {
            ddlRoles.Items.Add(role.ToString());
        }

        

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        foreach(UserStatus role in bjUser.UserStatus)
        {
            if (role.ToString() == ddlRoles.SelectedItem.Text)
            {
                bjUser.SelectedUserStatus = role;
            }
        }
        Session["bjUser"] = bjUser;
        Response.Redirect("default.aspx");
    }
}