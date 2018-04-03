using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Data;
//using ClosedXML.Excel;

public partial class Stats : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            LoadData();
            
        }
    }
    private void LoadData()
    {
        Label8.Visible = false;
        GridView1.DataSource = SqlDataSource1; //DS.Tables["data"];
        
        ((BoundField)GridView1.Columns[0]).DataField = "myd";
        ((BoundField)GridView1.Columns[1]).DataField = "registered";
        ((BoundField)GridView1.Columns[2]).DataField = "asg";
        ((BoundField)GridView1.Columns[3]).DataField = "remoteregistered";
        ((BoundField)GridView1.Columns[4]).DataField = "asgr";

        ((BoundField)GridView1.Columns[0]).DataFormatString = "{0:dd.MM.yyyy}";
        //((BoundField)GridView1.Columns[4]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        //((BoundField)GridView1.Columns[5]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";

        GridView1.DataBind();
        if (GridView1.Rows.Count == 0)
        {
            Label8.Visible = true;
            Label8.Text = "На данный момент нет ни одной записи.";
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

   
   
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadData();
        //GridView1.DataBind();

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("default.aspx");

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string tmp = "Litres_stats" + DateTime.Now.ToString("hh:mm:ss") + ".xls";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename="+tmp);
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            GridView1.AllowPaging = false;
            LoadData();

            GridView1.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in GridView1.HeaderRow.Cells)
            {
                cell.BackColor = GridView1.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in GridView1.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = GridView1.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }
    public override void VerifyRenderingInServerForm(Control control) { }

    protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.CommandTimeout = 1200;
    }
}
