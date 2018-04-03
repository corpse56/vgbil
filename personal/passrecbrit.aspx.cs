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

public partial class passrecbrit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if ((TextBox1.Text == "") || (TextBox2.Text == "") || (TextBox3.Text == "") || (TextBox4.Text == "")
            || (TextBox5.Text == "") || (TextBox6.Text == "") || (Indate.Text == ""))
        {
            Label9.ForeColor = System.Drawing.Color.Red;
            Label9.Text = "Заполните все поля!";
            return;
        }
        if (TextBox5.Text != TextBox6.Text)
        {
            TextBox5.Text = "";
            TextBox6.Text = "";
            Label9.ForeColor = System.Drawing.Color.Red;
            Label9.Text = "Пароли не совпадают!";
            return;
        }
        DateTime birth;
        if (!DateTime.TryParse(Indate.Text, out birth))
        {
            Label9.ForeColor = System.Drawing.Color.Red;
            Label9.Text = "Неправильный формат даты!";
            return;
        }
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        SqlParameter par1 = new SqlParameter("fam", SqlDbType.VarChar, 100);
        SqlParameter par2 = new SqlParameter("nam", SqlDbType.VarChar, 100);
        SqlParameter par3 = new SqlParameter("otc", SqlDbType.VarChar, 100);
        SqlParameter par4 = new SqlParameter("brth", SqlDbType.DateTime);
        DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberReader = " + TextBox1.Text;
        int hit = 0;
        DataSet DS = new DataSet();
        string NumberReader = "";
        hit = DA.Fill(DS, "hit1");
        if (hit == 0)
        {
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberSC = '" + TextBox1.Text + "'";
            hit = DA.Fill(DS, "hit2");
            if (hit == 0)
            {
                Label9.ForeColor = System.Drawing.Color.Red;
                Label9.Text = "Неверные данные!";
                return;
            }
            else
            {
                NumberReader = DS.Tables["hit2"].Rows[0]["NumberReader"].ToString();
            }
        }
        else
        {
            NumberReader = DS.Tables["hit1"].Rows[0]["NumberReader"].ToString();
        }

        par1.Value = TextBox2.Text;
        par2.Value = TextBox3.Text;
        par3.Value = TextBox4.Text;
        par4.Value = birth;
        DA.SelectCommand.Parameters.Add(par1);
        DA.SelectCommand.Parameters.Add(par2);
        DA.SelectCommand.Parameters.Add(par3);
        DA.SelectCommand.Parameters.Add(par4);


        DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where FamilyName = @fam and [Name] = @nam and FatherName = @otc and  DateBirth = @brth";
        hit = DA.Fill(DS, "hit");
        if (hit == 0)
        {
            Label9.ForeColor = System.Drawing.Color.Red;
            Label9.Text = "Неверные данные!";
            return;
        }
        DA.UpdateCommand = new SqlCommand();
        DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        DA.UpdateCommand.Connection.Open();

        DA.UpdateCommand.Parameters.Add("pass", SqlDbType.NVarChar);
        DA.UpdateCommand.Parameters.Add("wordreg", SqlDbType.NVarChar);
        string wordreg = RndWordReg(32);
        string passw = HashPass(TextBox5.Text, wordreg);
        DA.UpdateCommand.Parameters["pass"].Value = passw;
        DA.UpdateCommand.Parameters["wordreg"].Value = wordreg;
        DA.UpdateCommand.CommandText = "update Readers..Main set Password = @pass,WordReg = @wordreg where NumberReader = " + NumberReader;
        DA.UpdateCommand.ExecuteNonQuery();
        DA.UpdateCommand.Connection.Close();
        Label9.ForeColor = System.Drawing.Color.Green;
        Label9.Text = "Пароль успешно изменён!";
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        Indate.Text = "";
        TextBox6.Text = "";
        //Label9.ForeColor = System.Drawing.Color.Red;




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
    public String RndWordReg(int nLength)
    {
        String WordReg = "";
        byte[] bRandom = new byte[1];
        RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
        int i = 0;
        while (i < nLength)
        {
            Gen.GetBytes(bRandom);
            if (((bRandom[0] >= 48) && (bRandom[0] <= 57)) || ((bRandom[0] >= 65) && (bRandom[0] <= 90)) ||
                ((bRandom[0] >= 97) && (bRandom[0] <= 122)))
            {
                WordReg += Convert.ToChar(bRandom[0]);
                i++;
            }
        }
        return WordReg;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("loginbrit.aspx");
    }
    public class XmlConnections
    {

        private static String filename = System.AppDomain.CurrentDomain.BaseDirectory + "DBConnections.xml";
        private static XmlDocument doc;

        public static string GetConnection(string s)
        {
            //string g =   
            //HttpRequest d = new HttpRequest(

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
