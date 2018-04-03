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

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            LoadData();
            //GridView1.DataSource = SqlDataSource1;
            //GridView1.DataBind();
        }
        


    }
    private void LoadData()
    {
        
        //SqlDataAdapter DA = new SqlDataAdapter();
        //DA.SelectCommand = new SqlCommand();
        //DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        //DA.SelectCommand.CommandText = "with main as ( select row_number() over (order by (A.CREATED )) num,A.IDMAIN pin,case when A.BAZA = 1 then avtp.PLAIN else ravtp.PLAIN end avt," +
        //                                "case when A.BAZA = 1 then zagp.PLAIN else rzagp.PLAIN end zag,A.CREATED cre,case when A.BAZA = 1 then 'Основной фонд' else 'Фонд редкой книги' end baza, A.ID id " +
        //                                   " from Reservation_R..TURNTODIGITIZE A " +
        //                                   " left join BJVVV..DATAEXT zag on A.IDMAIN = zag.IDMAIN and zag.MNFIELD = 200 and zag.MSFIELD = '$a' " +
        //                                   " left join BJVVV..DATAEXTPLAIN zagp on zag.ID = zagp.IDDATAEXT " +
        //                                   " left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' " +
        //                                   " left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT " +
        //                                   " left join REDKOSTJ..DATAEXT rzag on A.IDMAIN = rzag.IDMAIN and rzag.MNFIELD = 200 and rzag.MSFIELD = '$a' " +
        //                                   " left join REDKOSTJ..DATAEXTPLAIN rzagp on zag.ID = rzagp.IDDATAEXT " +
        //                                   " left join REDKOSTJ..DATAEXT ravt on A.IDMAIN = ravt.IDMAIN and ravt.MNFIELD = 700 and ravt.MSFIELD = '$a' " +
        //                                   " left join REDKOSTJ..DATAEXTPLAIN ravtp on ravt.ID = ravtp.IDDATAEXT " +
        //                                   " where (A.DELETED is null or DELETED = 0) and not exists (select IDBook,IDBase from BookAddInf..ScanInfo CC where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase)" +
        //                                   " ) select top 100 * from main   where num<=100 order by num desc";


        //DataSet DS = new DataSet();
        //int i = DA.Fill(DS, "data");
        GridView1.DataSource = SqlDataSource1; //DS.Tables["data"];
        ((BoundField)GridView1.Columns[0]).DataField = "num";
        //((BoundField)GridView1.Columns[0]).
        ((BoundField)GridView1.Columns[1]).DataField = "baza";
        ((BoundField)GridView1.Columns[2]).DataField = "pin";
        ((BoundField)GridView1.Columns[3]).DataField = "avt";
        ((BoundField)GridView1.Columns[4]).DataField = "zag";
        ((BoundField)GridView1.Columns[5]).DataField = "cre";
        ((BoundField)GridView1.Columns[5]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        
        GridView1.DataBind();
    }
    protected void mark_click(object sender, CommandEventArgs e)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.UpdateCommand = new SqlCommand();
        DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.UpdateCommand.CommandText = "update  Reservation_R..TURNTODIGITIZE set MARK = 1 " +
                                           " where ID = " + e.CommandArgument.ToString();
        DA.UpdateCommand.Connection.Open();
        DA.UpdateCommand.ExecuteNonQuery();
        DA.UpdateCommand.Connection.Close();
        LoadData();
        //GridView1.DataBind();
        //GridView1.DataSource = SqlDataSource1;
        //GridView1.DataBind();


    }
    protected void delmark_click(object sender, CommandEventArgs e)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.UpdateCommand = new SqlCommand();
        DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.UpdateCommand.CommandText = "update  Reservation_R..TURNTODIGITIZE set MARK = 0 " +
                                           " where ID = " + e.CommandArgument.ToString();
        DA.UpdateCommand.Connection.Open();
        DA.UpdateCommand.ExecuteNonQuery();
        DA.UpdateCommand.Connection.Close();
        LoadData();
        //GridView1.DataBind();
        //GridView1.DataSource = SqlDataSource1;
        //GridView1.DataBind();

    }

    protected void delfromturn_click(object sender, CommandEventArgs e)
    {
        hfIDOFSELECTEDROW.Value = e.CommandArgument.ToString();
        TextBox1.Text = "";
        mpe.Show();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == -1) return;

        DataRowView rv = (DataRowView)e.Row.DataItem;
        string id = rv.Row.ItemArray[8].ToString();
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.SelectCommand.CommandText = "select case when MARK is null then 'false' else MARK end MARK" +
                                           " from Reservation_R..TURNTODIGITIZE A " +
                                           " where ID = " + id;
        DataSet vw = new DataSet();
        int i = DA.Fill(vw, "vw");
        if (i > 0)
        {
            if ((bool)vw.Tables["vw"].Rows[0]["MARK"] == true)
            {
                e.Row.BackColor = Color.LightGreen;
            }
        }
        else
        {
            return;
        }

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        mpe.Hide();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.UpdateCommand = new SqlCommand();
        DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.UpdateCommand.Parameters.AddWithValue("delcause", TextBox1.Text);
        DA.UpdateCommand.Parameters.AddWithValue("deldate", DateTime.Now);
        DA.UpdateCommand.CommandText = "update Reservation_R..TURNTODIGITIZE set DELETED = 1, MARK = 0, DELCAUSE = @delcause, DELDATE = @deldate " +
                                           " where ID = " + hfIDOFSELECTEDROW.Value.ToString();
        DA.UpdateCommand.Connection.Open();
        DA.UpdateCommand.ExecuteNonQuery();
        DA.UpdateCommand.Connection.Close();
        //GridView1.DataSource = SqlDataSource1;
        //GridView1.DataBind();
        LoadData();
        //UpdatePanel1.Update();
        //GridView1.DataBind();
        //LoadData();
        //SqlDataSource1.DeleteParameters["id"]. = hfIDOFSELECTEDROW.Value.ToString();
        
    }
    
  
    //private void DeleteExpired()
    //{
    //    SqlDataAdapter DA = new SqlDataAdapter();
    //    DA.UpdateCommand = new SqlCommand();
    //    DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
    //    DA.UpdateCommand.CommandText = "update Reservation_R..TURNTODIGITIZE set DELETED = 1, MARK = 0, DELCAUSE = @delcause, DELDATE = @deldate " +
    //                                       " where ID in (select ID from Reservation_R..TURNTODIGITIZE where datediff(CREATED ) ";
    //    DA.UpdateCommand.Connection.Open();
    //    DA.UpdateCommand.ExecuteNonQuery();
    //    DA.UpdateCommand.Connection.Close();
    //}
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
