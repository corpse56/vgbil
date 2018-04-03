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
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public partial class loginbrit : System.Web.UI.Page
{
    class Reader : object
    {

        public Reader(string _name, string _ids)
        {
            this.name = _name;
            this.idSession = _ids;
        }
        private string name;
        public string idSession;
        public string ID;

    }

    Reader CurReader;
    protected void Page_Load(object sender, EventArgs e)
    {
        CurReader = new Reader("", Request["id"]);
        string f = System.AppDomain.CurrentDomain.BaseDirectory;
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        SqlConnection.ClearAllPools();

        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        ulong res = 0;
        if (!ulong.TryParse(Login1.UserName, out res))
        {
            return;
        }

        DA.SelectCommand.Parameters.AddWithValue("login", Login1.UserName.ToLower());
        DA.SelectCommand.Parameters["login"].SqlDbType = SqlDbType.NVarChar;
        DA.SelectCommand.Parameters.AddWithValue("pass", Login1.Password.ToLower());
        //DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where [NumberReader] = '" + Login1.UserName.ToLower() + "' and lower(PASSWORD) = '" + Login1.Password.ToLower() + "'";
        bool sc = false;
        if (Login1.UserName.Length != 19)
        {
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where [NumberReader] = @login";// and lower(PASSWORD) = @pass";
        }
        else
        {
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberSC = @login";// and Password = @pass";
            sc = true;
        }

        DataSet usr = new DataSet();
        int i = DA.Fill(usr, "t");

        if (i == 0)
        {
            //читателя нет ни по номеру ни по социалке
            return;
        }

        string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
        DA.SelectCommand.Parameters["pass"].Value = pass;

        if (sc)
            DA.SelectCommand.CommandText = "select * from Readers..Main where NumberSC = @login and Password = @pass";
        else
            DA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = @login and Password = @pass";

        usr = new DataSet();
        i = DA.Fill(usr, "t");




        DA.SelectCommand.Connection.Close();

        if (i > 0)
        {
            CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
            FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
            Response.Redirect("persaccbrit.aspx" + "?id=" + CurReader.idSession);
        }

    }
    public String HashPass(String strPassword, String strSol)
    {
        String strHashPass = String.Empty;
        byte[] bytes = Encoding.Unicode.GetBytes(strSol + strPassword);
        //создаем объект для получения средст шифрования 
        SHA256CryptoServiceProvider CSP = new SHA256CryptoServiceProvider();
        //вычисляем хеш-представление в байтах 
        byte[] byteHash = CSP.ComputeHash(bytes);
        //формируем одну цельную строку из массива 
        foreach (byte b in byteHash)
        {
            strHashPass += string.Format("{0:x2}", b);
        }
        return strHashPass;
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
