using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.IO;
using System.Web.Security;

public partial class Login : System.Web.UI.Page
{
    Reader CurReader;

    protected void Page_Load(object sender, EventArgs e)
    {
        CurReader = new Reader("","");
        if (!Page.IsPostBack)
        {
            string f = System.AppDomain.CurrentDomain.BaseDirectory;
            Login1.Focus();
        }
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        if (Request["pin"] == null) return;
        if (Request["baza"] == null) return;

        if (Request["baza"] == "1")
        {
            DA.SelectCommand.CommandText = "select avtp.PLAIN avt,zagp.PLAIN zag from BJVVV..DATAEXT A " +
                                           " left join BJVVV..DATAEXT zag on A.IDMAIN = zag.IDMAIN and zag.MNFIELD = 200 and zag.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN zagp on zag.ID = zagp.IDDATAEXT " +
                                           " left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT " +
                                           " where A.IDMAIN = " + Request["PIN"];
        }
        else if (Request["baza"] == "2")
        {
            DA.SelectCommand.CommandText = "select avtp.PLAIN avt,zagp.PLAIN zag from REDKOSTJ..DATAEXT A " +
                                           " left join REDKOSTJ..DATAEXT zag on A.IDMAIN = zag.IDMAIN and zag.MNFIELD = 200 and zag.MSFIELD = '$a' " +
                                           " left join REDKOSTJ..DATAEXTPLAIN zagp on zag.ID = zagp.IDDATAEXT " +
                                           " left join REDKOSTJ..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' " +
                                           " left join REDKOSTJ..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT " +
                                           " where A.IDMAIN = " + Request["PIN"];
        }
        else return;
        DataSet res = new DataSet();

        int i = DA.Fill(res, "t");


        Label1.Text = "Вы собираетесь поставить в очередь на оцифровку издание: <br/> <b>Автор:</b>&nbsp" + res.Tables["t"].Rows[0]["avt"].ToString() + ";&nbsp<b>Заглавие:</b>&nbsp" + res.Tables["t"].Rows[0]["zag"].ToString() +
            "<br/><br/> Для выполнения этого действия необходимо авторизоваться <br/>";
    }
    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
    {

    }
    public class Tran
    {
        public string hfBAZA;
        public string hfIDR;
        public string hfIsRemote;
        public string hfPIN;
    }
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Parameters.Add("login", SqlDbType.Int);
        DA.SelectCommand.Parameters.Add("pass", SqlDbType.NVarChar);

        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        UInt64 res = 9999999999999999999;
        Int32 login;


        DataSet usr = new DataSet();
        int i;
        if (!UInt64.TryParse(Login1.UserName,out res))//ввели email типа. не проверяется на валидность ввода, а просто ищется то, что ввели в колонке Email
        {
            //читателя нет ни по номеру ни по социалке. ищем по email
            DA.SelectCommand.Parameters.Add("Email", SqlDbType.NVarChar);

            DA.SelectCommand.Parameters["Email"].Value = Login1.UserName;
            DA.SelectCommand.Parameters["login"].Value = 1;
            DA.SelectCommand.Parameters["pass"].Value = Login1.UserName;

            DA.SelectCommand.CommandText = "select * from Readers..Main " +
                                       " where [Email] = @Email ";

            usr = new DataSet();
            i = DA.Fill(usr);

            for (int j = 0; j < i; j++)//так как email повторяется (это временно), то искать нужно по всем.
            {
                DA.SelectCommand.Parameters["Email"].Value = usr.Tables[0].Rows[0]["Email"].ToString();
                string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
                DA.SelectCommand.Parameters["pass"].Value = pass;


                DA.SelectCommand.CommandText = "select * from Readers..Main where Email = @Email and Password = @pass";
                //DataSet usr = new DataSet();
                i = DA.Fill(usr, "t");
                if (i == 0)//email не найден
                {
                    continue;
                }
                else
                {
                    CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
                    int rtype = Convert.ToInt32(usr.Tables["t"].Rows[0]["TypeReader"]);
                    if (rtype == 0)
                    {
                        CurReader.SetReaderType(0);
                    }
                    else
                    {
                        CurReader.SetReaderType(1);
                    }
                    //FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
                    //Response.Redirect("default.aspx" + "?id=" + CurReader.ID + "&type=0&PIN=" + Request["PIN"]);
                    hfBAZA.Value = Request["baza"];
                    hfIDR.Value = CurReader.ID;
                    hfIsRemote.Value = "0";
                    hfPIN.Value = Request["PIN"];
                    //Response.Redirect("default.aspx");
                    Tran tr = new Tran();
                    tr.hfBAZA = Request["baza"];
                    tr.hfIDR = CurReader.ID;
                    tr.hfIsRemote = "0";
                    tr.hfPIN = Request["PIN"];
                    Session.Add("transdata", tr);
                    FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);

                }
            }

            return;
        }
        else if (Int32.TryParse(Login1.UserName.ToLower(), out login))//ввели номер читателя
        {
            DA.SelectCommand.Parameters["login"].Value = login;
            DA.SelectCommand.Parameters["pass"].Value = Login1.Password;

            DA.SelectCommand.CommandText = "select * from Readers..Main " +
                                       " where [NumberReader] = @login ";

            usr = new DataSet();
            i = DA.Fill(usr);
            if (i == 0)
            {//нет такого читателя
                return;
            }

            string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
            DA.SelectCommand.Parameters["pass"].Value = pass;


            //DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where [NumberReader] = @login";
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where [NumberReader] = @login and PASSWORD = @pass";

            usr = new DataSet();
            i = DA.Fill(usr, "t");
        }
        else//ввели номер социалки
        {

            DA.SelectCommand.Parameters.Add("login_sc", SqlDbType.NVarChar);
            DA.SelectCommand.Parameters["login_sc"].Value = Login1.UserName.ToLower();
            DA.SelectCommand.Parameters["pass"].Value = Login1.Password;
            DA.SelectCommand.Parameters["login"].Value = 0;
            
            DA.SelectCommand.CommandText = "select * from Readers..Main " +
                                      " where [NumberSC] = @login_sc ";

            usr = new DataSet();
            i = DA.Fill(usr);
            if (i == 0)
            {//нет такого читателя
                return;
            }

            string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
            DA.SelectCommand.Parameters["pass"].Value = pass;


            DA.SelectCommand.CommandText = "select * from Readers..Main where NumberSC = @login_sc and Password = @pass";
            usr = new DataSet();
            i = DA.Fill(usr, "t");
        }

        DA.SelectCommand.Connection.Close();

        if (i > 0)
        {
            CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
            int rtype = Convert.ToInt32(usr.Tables["t"].Rows[0]["TypeReader"]);
            if (rtype == 0)
            {
                CurReader.SetReaderType(0);
            }
            else
            {
                CurReader.SetReaderType(1);
            }
            //FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
            //Response.Redirect("default.aspx" + "?id=" + CurReader.ID + "&type=0&PIN=" + Request["PIN"]);
            hfBAZA.Value = Request["baza"];
            hfIDR.Value = CurReader.ID;
            hfIsRemote.Value = "0";
            hfPIN.Value = Request["PIN"];
            //Response.Redirect("default.aspx");
            Tran tr = new Tran();
            tr.hfBAZA = Request["baza"];
            tr.hfIDR = CurReader.ID;
            tr.hfIsRemote = "0";
            tr.hfPIN = Request["PIN"];
            Session.Add("transdata", tr);
            FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);

        }

    


        //if (RadioButton1.Checked)
        //{
        //    SqlDataAdapter DA = new SqlDataAdapter();
        //    DA.SelectCommand = new SqlCommand();
        //    DA.SelectCommand.Parameters.Add("login", SqlDbType.Int);
        //    DA.SelectCommand.Parameters.Add("pass", SqlDbType.NVarChar);

        //    DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        //    UInt64 res = 9999999999999999999;

        //    if (!UInt64.TryParse(Login1.UserName, out res))
        //    {
        //        return;
        //    }
        //    Int32 login;
        //    int i;
        //    DataSet usr;
        //    if (Int32.TryParse(Login1.UserName.ToLower(), out login))
        //    {
        //        DA.SelectCommand.Parameters["login"].Value = login;
        //        DA.SelectCommand.Parameters["pass"].Value = Login1.Password;

        //        DA.SelectCommand.CommandText = "select * from Readers..Main " +
        //                                   " where [NumberReader] = @login ";

        //        usr = new DataSet();
        //        i = DA.Fill(usr);
        //        if (i == 0)
        //        {//нет такого читателя
        //            return;
        //        }

        //        string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
        //        DA.SelectCommand.Parameters["pass"].Value = pass;


        //        DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where [NumberReader] = @login and PASSWORD = @pass";

        //        usr = new DataSet();
        //        i = DA.Fill(usr, "t");
        //    }
        //    else
        //    {

        //        DA.SelectCommand.Parameters.Add("login_sc", SqlDbType.NVarChar);
        //        DA.SelectCommand.Parameters["login_sc"].Value = Login1.UserName.ToLower();
        //        DA.SelectCommand.Parameters["pass"].Value = Login1.Password;
        //        DA.SelectCommand.Parameters["login"].Value = 0;

        //        DA.SelectCommand.CommandText = "select * from Readers..Main " +
        //                                  " where [NumberSC] = @login_sc ";

        //        usr = new DataSet();
        //        i = DA.Fill(usr);
        //        if (i == 0)
        //        {//нет такого читателя
        //            return;
        //        }

        //        string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
        //        DA.SelectCommand.Parameters["pass"].Value = pass;


        //        DA.SelectCommand.CommandText = "select * from Readers..Main where NumberSC = @login_sc and Password = @pass";
        //        usr = new DataSet();
        //        i = DA.Fill(usr, "t");
        //    }

        //    if (i == 0)
        //    {
        //        //читателя нет ни по номеру ни по социалке
        //    }
        //    //DA.SelectCommand.Connection.Close();


        //    if (i > 0)
        //    {
        //        CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
        //        CurReader.SetReaderType(0);
        //        FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
        //        //Response.Redirect("default.aspx" + "?id=" + CurReader.ID + "&type=0&PIN=" + Request["PIN"]);
        //        hfBAZA.Value = Request["baza"];
        //        hfIDR.Value = CurReader.ID;
        //        hfIsRemote.Value = "0";
        //        hfPIN.Value = Request["PIN"];
        //        //Response.Redirect("default.aspx");
        //        Tran tr = new Tran();
        //        tr.hfBAZA = Request["baza"];
        //        tr.hfIDR = CurReader.ID;
        //        tr.hfIsRemote = "0";
        //        tr.hfPIN = Request["PIN"];
        //        Session.Add("transdata", tr);
        //        FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
        //    }

        //}
        //if (RadioButton3.Checked)
        //{
        //    SqlDataAdapter DA = new SqlDataAdapter();
        //    DA.SelectCommand = new SqlCommand();
        //    DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        //    DA.SelectCommand.Parameters.Add("login", SqlDbType.NVarChar);
        //    DA.SelectCommand.Parameters.Add("pass", SqlDbType.NVarChar);

        //    Int32 login;
        //    int i;
        //    DataSet usr;
        //    //if (!Int32.TryParse(Login1.UserName.ToLower(), out login))
        //    //{
        //    //   return;
        //    //}
        //    DA.SelectCommand.Parameters["login"].Value = Login1.UserName;
        //    DA.SelectCommand.Parameters["pass"].Value = Login1.Password;


        //    DA.SelectCommand.CommandText = "select * from Readers..RemoteMain " +
        //                                   " where [LiveEmail] = @login ";//and lower(Password) = @pass";

        //    usr = new DataSet();
        //    i = DA.Fill(usr);
        //    if (i == 0)
        //    {//нет такого читателя
        //        return;
        //    }

        //    if (i > 0)
        //    {


        //        string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
        //        //DA.SelectCommand.Parameters["login"].Value = login;
        //        DA.SelectCommand.Parameters["pass"].Value = pass;
        //        DA.SelectCommand.CommandText = "select * from Readers..RemoteMain " +
        //                                       " where [LiveEmail] = @login and Password = @pass";
        //        usr = new DataSet();
        //        i = DA.Fill(usr, "t");
        //        if (i == 0)
        //        {
        //            return;
        //        }

        //        CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
        //        CurReader.SetReaderType(1);
        //        FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
        //        //Response.Redirect("default.aspx" + "?id=" + CurReader.ID + "&type=1&PIN=" + Request["PIN"]);
        //        hfBAZA.Value = Request["baza"];
        //        hfIDR.Value = CurReader.ID;
        //        hfIsRemote.Value = "1";
        //        hfPIN.Value = Request["PIN"];
        //        Tran tr = new Tran();
        //        tr.hfBAZA = Request["baza"];
        //        tr.hfIDR = CurReader.ID;
        //        tr.hfIsRemote = "1";
        //        tr.hfPIN = Request["PIN"];
        //        Session.Add("transdata", tr);
        //        //Response.Redirect("default.aspx" + "?id=" + CurReader.ID + "&type=1&PIN=" + Request["PIN"]);
        //        FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);

        //    }
        //}
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
    class Reader : object
    {

        public Reader(string _name, string _ids)
        {
            this.name = _name;
            this.idSession = _ids;
        }
        /// <summary>
        /// 0 - читатель или читатель сотрудник
        /// 1 - удалённый читатель
        /// 2 - сотрудник
        /// </summary>
        public void SetReaderType(int t)
        {
            this.ReaderType = t;
            //0 - читатель или читатель сотрудник ;
            //1 - удалённый читатель;
            //2 - сотрудник
        }
        private string name;
        public string idSession;
        public string ID;
        public int ReaderType;
    }


}
