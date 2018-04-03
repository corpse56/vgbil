using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PassRec_ChooseT : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (RadioButton1.Checked)
        {
            Response.Redirect("~/PassRec.aspx?id=" + Request["id"]);
        }
        else if (RadioButton2.Checked)
        {
            Response.Redirect("http://80.250.173.145/WebRemotePas/Default.aspx?id="+Request["id"]);
        }
        else return;
    }
}
