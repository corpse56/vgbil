using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;

public partial class LogIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        SqlConnection.ClearAllPools();

        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        DA.SelectCommand.Parameters.AddWithValue("login", Login1.UserName.ToLower());
        DA.SelectCommand.Parameters.AddWithValue("pass", Login1.Password.ToLower());
        //DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME uname,dpt.NAME dname from BJVVV..USERS join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = '" + Login1.UserName.ToLower() + "' and lower(PASSWORD) = '" + Login1.Password.ToLower() + "'";
        DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME uname,dpt.NAME dname from BJVVV..USERS join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = @login and lower(PASSWORD) = @pass";

        DataSet usr = new DataSet();
        int i = DA.Fill(usr);

        if (i > 0)
        {
            string ID = usr.Tables[0].Rows[0]["ID"].ToString();
            FormsAuthentication.RedirectFromLoginPage(Login1.UserName, false);
            Response.Redirect("default.aspx?uid=" +ID);

        }
    }

    public class XmlConnections
    {

        private static String filename = System.AppDomain.CurrentDomain.BaseDirectory + "DBConnections.xml";
        private static XmlDocument doc;

        public static string GetConnection(string s)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Файл с подключениями 'DBConnections.xml' не найден.");
            }
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
            XmlNode node;
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("Узел " + s + " не найден в файле DBConnections.xml"); ;
            }

            return node.InnerText;
        }
        public XmlConnections()
        {

        }
    }
}
