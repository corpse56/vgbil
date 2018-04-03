using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

public partial class loginemployee : System.Web.UI.Page
{
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
/// 
/// Позволяет получить язык ввода для любого активного окна в любой момент времени.
/// 
public static class CurrentCultureInfo
{
    #region Fields & Properties
    private static int lpdwProcessId;
    private static System.Windows.Forms.InputLanguageCollection installedInputLanguages = System.Windows.Forms.InputLanguage.InstalledInputLanguages;
    private static System.Globalization.CultureInfo currentInputLanguage;
    public static string InputLangTwoLetterISOLanguageName
    {
        get { return CurrentCultureInfo.currentInputLanguage.TwoLetterISOLanguageName; }
    }
    public static string InputLangThreeLetterWindowsLanguageName
    {
    get { return CurrentCultureInfo.currentInputLanguage.ThreeLetterWindowsLanguageName; }
    }
    public static string InputLangThreeLetterISOLanguageName
    {
    get { return CurrentCultureInfo.currentInputLanguage.ThreeLetterISOLanguageName; }
    }
    public static string InputLangNativeName
    {
    get { return CurrentCultureInfo.currentInputLanguage.NativeName; }
    }
    public static string InputLangName
    {
    get { return CurrentCultureInfo.currentInputLanguage.Name; }
    }
    public static int InputLangLCID
    {
    get { return CurrentCultureInfo.currentInputLanguage.LCID; }
    }
    public static int InputLangKeyboardLayoutId
    {
    get { return CurrentCultureInfo.currentInputLanguage.KeyboardLayoutId; }
    }
    public static string InputLangEnglishName
    {
    get { return CurrentCultureInfo.currentInputLanguage.EnglishName; }
    }
    public static string InputLangDisplayName
    {
    get { return CurrentCultureInfo.currentInputLanguage.DisplayName; }
    }
    #endregion
    /// 
    /// Получает маркер активного окна Windows.
    /// 
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();
    /// 
    /// Получает идентификационный номер потока для окна.
    /// 
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);
    /// 
    /// Получает информацию о раскладке клавиатуры.
    /// 
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);
    /// 
    /// Получает значение раскладки клавиатуры в формате кода страницы в текущий момент времени.
    /// 
    public static int GetKeyboardLayoutIdAtTime()
    {
        IntPtr hWnd = GetForegroundWindow();
        int WinThreadProcId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
        IntPtr KeybLayout = GetKeyboardLayout(WinThreadProcId);
        installedInputLanguages = System.Windows.Forms.InputLanguage.InstalledInputLanguages;

        return installedInputLanguages.Count;
        for (int i = 0; i < installedInputLanguages.Count; i++)
        {
            if (KeybLayout == installedInputLanguages[i].Handle) currentInputLanguage = installedInputLanguages[i].Culture;
        }
        return currentInputLanguage.KeyboardLayoutId;
    }
}

        Reader CurReader;
        string litres = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //mname == "VGBIL-OPAC";
            //if (Server.MachineName == "VGBIL-OPAC")
            if (Server.MachineName == "VGBIL-OPAC")
            {
                Panel1.Visible = true;

            }
            CurReader = new Reader("",Request["id"]);
            if ((Request["litres"] == null) || (Request["litres"] == "0"))
            {
                litres = "0";
            }
            if (Request["litres"] == "1")
            {
                litres = "1";
            }
            MoveToHistoryEL(CurReader);
            HyperLink2.NavigateUrl = "http://opac.libfl.ru/WebRemoteReg/Default.aspx?id="+Request["id"];
            if (!Page.IsPostBack)
            {
                string f = System.AppDomain.CurrentDomain.BaseDirectory;
                Login1.PasswordRecoveryUrl = "~/PassRec_ChooseT.aspx?id=" + Request["id"];
                Login1.Focus();
            }
        }


        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            SqlConnection.ClearAllPools();
            SqlDataAdapter DA;

            //для входа под любым читателем. не забывать закомментироват
            //CurReader.ID = "173968";
            //FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
            //Response.Redirect("persacc.aspx" + "?id=" + CurReader.idSession + "&type=0&litres=" + litres);



            if (RadioButton2.Checked)//сотрудник для ДП
            {

                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
                DA.SelectCommand.Parameters.Add("login", SqlDbType.NVarChar);
                DA.SelectCommand.Parameters.Add("pass", SqlDbType.NVarChar);
                DA.SelectCommand.Parameters["login"].Value = Login1.UserName.ToLower();
                DA.SelectCommand.Parameters["pass"].Value = Login1.Password.ToLower();



                DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME uname,dpt.NAME dname from BJVVV..USERS " +
                                               " join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = @login and lower(PASSWORD) = @pass";
                //DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME uname,dpt.NAME dname from BJVVV..USERS " +
                //                               " join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = 'admin'";

                DataSet usr = new DataSet();
                int i = DA.Fill(usr);
                if (i == 0)
                {//нет такого сотрудника

                    //   OleDA.SelectCommand.CommandText = "select * from MAIN where NumberSC = " + Login1.UserName + " and Password = '" + Login1.Password + "'";
                }
                DA.SelectCommand.Connection.Close();
                //FormsIdentity d = new FormsIdentity();

                if (i > 0)
                {
                    CurReader.ID = usr.Tables[0].Rows[0]["ID"].ToString();
                    CurReader.SetReaderType(2);
                    FormsAuthentication.RedirectFromLoginPage(Login1.UserName, false);
                    MoveToHistory();
                    Response.Redirect("default.aspx" + "?id=" + CurReader.idSession +"&type=2");
                }
            }
            if (RadioButton1.Checked)//читатель.
            {
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
                            if ((CurReader.idSession != null) && (CurReader.idSession != string.Empty))
                                InsertSession(CurReader);
                            FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
                            Response.Redirect("persacc.aspx" + "?id=" + CurReader.idSession + "&type="+rtype.ToString()+"&litres=" + litres);

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
                    //CurReader.idSession = CreateSession();
                    if ((CurReader.idSession != null) && (CurReader.idSession != string.Empty))
                        InsertSession(CurReader);
                    FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
                    Response.Redirect("persacc.aspx" + "?id=" + CurReader.idSession + "&type="+rtype.ToString()+"&litres="+litres);

                }

            }
            //if (RadioButton3.Checked)
            //{
            //    SqlDataAdapter DA = new SqlDataAdapter();
            //    DA.SelectCommand = new SqlCommand();
            //    DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            //    DA.SelectCommand.Parameters.Add("login", SqlDbType.NVarChar);
            //    DA.SelectCommand.Parameters.Add("pass", SqlDbType.NVarChar);

            //    Int32 login;
            //    int i;
            //    DataSet usr;
            //    //if (!Int32.TryParse(Login1.UserName.ToLower(), out login))
            //    //{
            //     //   return;
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
            //}
                //if (i > 0)
                //{


                //    string pass = HashPass(Login1.Password, usr.Tables[0].Rows[0]["WordReg"].ToString());
                //    //DA.SelectCommand.Parameters["login"].Value = login;
                //    DA.SelectCommand.Parameters["pass"].Value = pass;
                //    DA.SelectCommand.CommandText = "select * from Readers..RemoteMain " +
                //                                   " where [LiveEmail] = @login and Password = @pass";
                //    usr = new DataSet();
                //    i = DA.Fill(usr,"t");
                //    if (i == 0)
                //    {
                //        return;
                //    }

                //    CurReader.ID = usr.Tables["t"].Rows[0]["NumberReader"].ToString();
                //    CurReader.SetReaderType(1);
                //    //CurReader.idSession = CreateSession();
                //    if ((CurReader.idSession != null) && (CurReader.idSession != string.Empty))
                //        InsertSession(CurReader);
                //    FormsAuthentication.RedirectFromLoginPage(CurReader.ID, false);
                //    Response.Redirect("persacc.aspx" + "?id=" + CurReader.idSession + "&type=1&litres="+litres);

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
        private void InsertSession(Reader r)
        {
            SqlDataAdapter DA = new SqlDataAdapter();

            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.SelectCommand.CommandText = "select * from Reservation_O..USERSESSION where SESSION = @sess";
            DA.SelectCommand.Parameters.Add("sess", SqlDbType.NVarChar);
            DA.SelectCommand.Parameters["sess"].Value = r.idSession;
            DataSet ds = new DataSet();
            int j = DA.Fill(ds, "t");
            if (j > 0) return;

            DA.InsertCommand = new SqlCommand();
            DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.InsertCommand.Parameters.Add("sess", SqlDbType.NVarChar);
            DA.InsertCommand.Parameters.Add("idr", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("rtype", SqlDbType.Int);
            DA.InsertCommand.Parameters["sess"].Value = r.idSession;
            DA.InsertCommand.Parameters["idr"].Value = r.ID;
            DA.InsertCommand.Parameters["rtype"].Value = r.ReaderType;
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.CommandText = "insert into Reservation_O..USERSESSION (SESSION,IDREADER,DT,R_TYPE) values (@sess,@idr,getdate(),@rtype)";
            int i = DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }
        private void MoveToHistoryEL(Reader CurReader)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            DA.SelectCommand.Connection = con;
            con.Open();
            SqlTransaction tr;
            DA.SelectCommand.CommandText = "select * from Reservation_R..ELISSUED";
            DataSet DS = new DataSet();
            int i = DA.Fill(DS, "possb");
            con.Close();
            if (i > 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DateTime tmp = (DateTime)row["DATERETURN"];
                    if (tmp <= DateTime.Now.Date)
                    {
                        con.Open();

                        SqlCommand comm = new SqlCommand();
                        comm.Connection = con;
                        tr = con.BeginTransaction("movhis");
                        comm.Transaction = tr;
                        try
                        {
                            comm.CommandText = "insert into Reservation_R..ELISSUED_HST (IDMAIN, IDREADER, DATEISSUE, DATERETURN,BASE,R_TYPE) " +
                                               "values (" + row["IDMAIN"].ToString() + "," + row["IDREADER"].ToString() + ",'" +
                                               ((DateTime)row["DATEISSUE"]).ToString("yyyyMMdd") + "','" +
                                               ((DateTime)row["DATERETURN"]).ToString("yyyyMMdd") + "',1," + row["R_TYPE"] + ") ";
                            comm.ExecuteNonQuery();
                            comm.CommandText = "delete from Reservation_R..ELISSUED where ID = " + row["ID"].ToString();
                            comm.ExecuteNonQuery();
                            comm.CommandText = "delete from Reservation_R..AGREEMENT where IDREADER = " + row["IDREADER"].ToString()+ " and R_TYPE = "+row["R_TYPE"]+
                                                " and IDMAIN = " + row["IDMAIN"].ToString();
                            comm.ExecuteNonQuery();

                            tr.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
        }
        private string CreateSession()
        {
            byte[] random = new byte[30];
            //RNGCryptoServiceProvider is an implementation of a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.
            return Convert.ToBase64String(random);
        }
        void MoveToHistory()
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            DA.SelectCommand.Connection = con;
            con.Open();
            SqlTransaction tr;
            DA.SelectCommand.CommandText = "select * from Reservation_E..Orders where ID_Reader = " + CurReader.ID;
            DataSet DS = new DataSet();
            int i = DA.Fill(DS,"possb");
            con.Close();
            if (i>0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    DateTime tmp = (DateTime)row["Start_Date"];
                    tmp = tmp.AddDays((Int32)row["Duration"]-1);
                    if (tmp < DateTime.Today)
                    {
                        con.Open();

                        SqlCommand comm = new SqlCommand();
                        comm.Connection = con;
                        tr = con.BeginTransaction("movhis");
                        comm.Transaction = tr;
                        try
                        {
                            //DA.Update(DS.Tables["ordhis"]);//так тоже транзакция работает!
                            //DA.DeleteCommand.ExecuteNonQuery();
                            comm.CommandText = "insert into Reservation_E..OrdHis (ID_Reader, ID_Book_EC, ID_Book_CC, Status, Start_Date, "+
                                                " Change_Date, InvNumber, Form_Date, Duration, Who, OID) " +
                                               "values (" + row["ID_Reader"].ToString() + "," + row["ID_Book_EC"].ToString() + "," +
                                               row["ID_Book_CC"].ToString() + ",1,'" + ((DateTime)row["Start_Date"]).ToString("yyyyMMdd") + "','" +
                                               DateTime.Now.ToString("yyyyMMdd") + "','" + row["InvNumber"].ToString() + "','" +
                                               ((DateTime)row["Form_Date"]).ToString("yyyyMMdd") + "'," + row["Duration"].ToString() + "," +
                                               row["Who"].ToString() + "," + row["ID"].ToString() + ")";
                            comm.ExecuteNonQuery();
                            comm.CommandText = "delete from Reservation_E..Orders where ID = " + row["ID"].ToString();
                            comm.ExecuteNonQuery();

                            tr.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            //System.Windows.Forms.MessageBox.Show(ex.Message);
                            tr.Rollback();
                            //con.Close();
                        }
                        

                    }
                }
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

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                Login1.UserNameLabelText = "Номер читательского билета, email или номер социальной карты*: ";
            }
            if (RadioButton3.Checked)
            {
                Login1.UserNameLabelText = "Email: ";
            }
            if (RadioButton2.Checked)
            {
                Login1.UserNameLabelText = "Логин АБИС BiblioJet: ";
            }
        }
        protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                Login1.UserNameLabelText = "Номер читательского билета, email или номер социальной карты*: ";
            }
            if (RadioButton3.Checked)
            {
                Login1.UserNameLabelText = "Email: ";
            }
            if (RadioButton2.Checked)
            {
                Login1.UserNameLabelText = "Логин АБИС BiblioJet: ";
            }

        }
        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton1.Checked)
            {
                Login1.UserNameLabelText = "Номер читательского билета, email или номер социальной карты*: ";
            }
            if (RadioButton3.Checked)
            {
                Login1.UserNameLabelText = "Email: ";
            }
            if (RadioButton2.Checked)
            {
                Login1.UserNameLabelText = "Логин АБИС BiblioJet: ";
            }

        }
}
