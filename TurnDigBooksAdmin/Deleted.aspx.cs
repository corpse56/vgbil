using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class _Deleted : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.SelectCommand.CommandText = "with main as ( select row_number() over (order by (A.DELDATE ) desc ) num,A.IDMAIN pin,case when A.BAZA = 1 then avtp.PLAIN else ravtp.PLAIN end avt, A.DELDATE dd," +
                                        "case when A.BAZA = 1 then zagp.PLAIN else rzagp.PLAIN end zag,A.CREATED cre,case when A.BAZA = 1 then 'Основной фонд' else 'Фонд редкой книги' end baza, A.ID id,A.DELCAUSE dc " +
                                           " from Reservation_R..TURNTODIGITIZE A " +
                                           " left join BJVVV..DATAEXT zag on A.IDMAIN = zag.IDMAIN and zag.MNFIELD = 200 and zag.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN zagp on zag.ID = zagp.IDDATAEXT " +
                                           " left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT " +
                                           " left join REDKOSTJ..DATAEXT rzag on A.IDMAIN = rzag.IDMAIN and rzag.MNFIELD = 200 and rzag.MSFIELD = '$a' " +
                                           " left join REDKOSTJ..DATAEXTPLAIN rzagp on zag.ID = rzagp.IDDATAEXT " +
                                           " left join REDKOSTJ..DATAEXT ravt on A.IDMAIN = ravt.IDMAIN and ravt.MNFIELD = 700 and ravt.MSFIELD = '$a' " +
                                           " left join REDKOSTJ..DATAEXTPLAIN ravtp on ravt.ID = ravtp.IDDATAEXT " +
                                           " where A.DELETED = 1) " +
                                           " select top 100 * from main   where num<=100 order by num ";

        DataSet DS = new DataSet();
        int i = DA.Fill(DS, "data");
        GridView1.DataSource = DS.Tables["data"];
        ((BoundField)GridView1.Columns[0]).DataField = "num";
        //((BoundField)GridView1.Columns[0]).
        ((BoundField)GridView1.Columns[1]).DataField = "baza";
        ((BoundField)GridView1.Columns[2]).DataField = "pin";
        ((BoundField)GridView1.Columns[3]).DataField = "avt";
        ((BoundField)GridView1.Columns[4]).DataField = "zag";
        ((BoundField)GridView1.Columns[5]).DataField = "cre";
        ((BoundField)GridView1.Columns[5]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)GridView1.Columns[6]).DataField = "dd";
        ((BoundField)GridView1.Columns[7]).DataField = "dc";

        GridView1.DataBind();


    }
   

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == -1) return;

        

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
