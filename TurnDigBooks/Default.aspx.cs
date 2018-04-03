using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Drawing;
public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*if (Page.PreviousPage == null)
        {
            return;
        }
        */
        
        if (Session["transdata"] == null) return;
        object ob = Session["transdata"];
        Login.Tran tr = (Login.Tran)ob;
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.SelectCommand.CommandText = "select * from Reservation_R..TURNTODIGITIZE where IDMAIN = " + tr.hfPIN + " and BAZA = " + tr.hfBAZA+" and (DELETED = 0 or DELETED is null)";
        DataSet res = new DataSet();
        int i = DA.Fill(res, "t");
        if (i > 0)
        {
            Label1.ForeColor = Color.Red;
            Label1.Text = "Эта книга уже присутствует в очереди на оцифровку!";
            return;
        }

        //===========================================================================================================================
        if (tr.hfBAZA != "1")
        {
            Label1.ForeColor = Color.Red;
            Label1.Text = "В очередь на оцифровку принимаются издания только из основного фонда!";
            return;
        }
        //===========================================================================================================================
        DA.SelectCommand.CommandText = "select * from Reservation_R..TURNTODIGITIZE where IDREADER = " + tr.hfIDR + " and cast(cast(CREATED as varchar(11)) as datetime) = cast(cast(getdate() as varchar(11)) as datetime)";
        res = new DataSet();
        i = DA.Fill(res, "t");
        if (i >= 2)
        {
            Label1.ForeColor = Color.Red;
            Label1.Text = "Извините, но вы не можете добавить в очередь на оцифровку более двух изданий в день!";
            return;
        }
        //===========================================================================================================================
        DA.SelectCommand.CommandText = "select * from Reservation_R..TURNTODIGITIZE A where (DELETED = 0 or DELETED is null)  and not exists (select IDBook,IDBase from BookAddInf..ScanInfo CC where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase)";
        res = new DataSet();
        i = DA.Fill(res, "t");
        if (i >= 80)
        {
            Label1.ForeColor = Color.Red;
            Label1.Text = "Извините, но очередь на оцифровку переполнена! Попробуйте позже.";
            return;
        }
        //===========================================================================================================================
        DA.SelectCommand.CommandText = "select * from Reservation_R..ISSUED_OF where IDMAIN_CONST = " + tr.hfPIN;
        res = new DataSet();
        i = DA.Fill(res, "t");
        if (i >= 1)
        {
            Label1.ForeColor = Color.Red;
            Label1.Text = "Книга не может быть добавлена в очередь на оцифровку, поскольку выдана читателю или находится на его бронеполке! Предполагаемая дата возврата " + Convert.ToDateTime(res.Tables["t"].Rows[0]["DATE_RET"]).ToString("dd.MM.yyyy");
            return;
        }
//===========================================================================================================================
        //Label1.ForeColor = Color.Red;
        //Label1.Text = "Заказы из книгохранения временно приостановлены. <a href =\"http://libfl.ru/ru/event/kapitalnyy-remont-i-tehnicheskoe-pereosnashchenie-vgbil\" target=\"blank\">Дополнительная информация</a>"; 
        //return;

        DA.InsertCommand = new SqlCommand();
        DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.InsertCommand.Parameters.Add("IDMAIN", SqlDbType.Int);
        DA.InsertCommand.Parameters["IDMAIN"].Value = tr.hfPIN;
        DA.InsertCommand.Parameters.Add("CREATED", SqlDbType.DateTime);
        DA.InsertCommand.Parameters["CREATED"].Value = DateTime.Now;
        DA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
        DA.InsertCommand.Parameters["IDREADER"].Value = tr.hfIDR;
        DA.InsertCommand.Parameters.Add("ISREMOTEUSER", SqlDbType.Int);
        DA.InsertCommand.Parameters["ISREMOTEUSER"].Value = tr.hfIsRemote;
        DA.InsertCommand.Parameters.Add("BAZA", SqlDbType.Int);
        DA.InsertCommand.Parameters["BAZA"].Value = tr.hfBAZA;
        DA.InsertCommand.CommandText = "insert into Reservation_R..TURNTODIGITIZE (IDMAIN,CREATED,IDREADER,ISREMOTEUSER,BAZA) values (@IDMAIN,@CREATED,@IDREADER,@ISREMOTEUSER,@BAZA)";
        DA.InsertCommand.Connection.Open();
        DA.InsertCommand.ExecuteNonQuery();
        DA.InsertCommand.Connection.Close();

        Label1.Text = "Издание успешно добавлено в очередь на оцифровку!";


        

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
