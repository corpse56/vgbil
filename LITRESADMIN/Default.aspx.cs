using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;

public partial class _Default : System.Web.UI.Page 
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
        ((BoundField)GridView1.Columns[0]).DataField = "num";   
        ((BoundField)GridView1.Columns[1]).DataField = "LRLOGIN";
        ((BoundField)GridView1.Columns[2]).DataField = "LRPWD";
        ((BoundField)GridView1.Columns[3]).DataField = "CREATED";
        ((BoundField)GridView1.Columns[4]).DataField = "ASSIGNED";
        ((BoundField)GridView1.Columns[5]).DataField = "CHANGED";
        ((BoundField)GridView1.Columns[6]).DataField = "IDREADER";
        ((BoundField)GridView1.Columns[7]).DataField = "FamilyName";
        ((BoundField)GridView1.Columns[8]).DataField = "Name";
        ((BoundField)GridView1.Columns[9]).DataField = "FatherName";
        ((BoundField)GridView1.Columns[10]).DataField = "DateBirth";

        //((BoundField)GridView1.Columns[3]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        //((BoundField)GridView1.Columns[4]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        //((BoundField)GridView1.Columns[5]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";

        GridView1.DataBind();
        if (GridView1.Rows.Count == 0)
        {
            Label8.Visible = true;
            Label8.Text = "На данный момент нет ни одной записи.";
        }
    }
    protected void Button1_Click(object sender, EventArgs e)//add
    {
        //добавление нового логина и пароля
        Label10.Visible = false;
        if ((TextBox2.Text == "") || (TextBox1.Text == ""))
        {
            Label10.Visible = true;
            mpeADD.Show();
            return;
        }
        else
        {

            SqlDataAdapter DA = new SqlDataAdapter();
            DA.InsertCommand = new SqlCommand();
            DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/litres"));
            DA.InsertCommand.Parameters.AddWithValue("LRLOGIN", TextBox2.Text);
            DA.InsertCommand.Parameters.AddWithValue("LRPWD", TextBox1.Text);
            DA.InsertCommand.Parameters.AddWithValue("CREATED", DateTime.Now);
            //DA.InsertCommand.Parameters.AddWithValue("ID", hfIDOFSELECTEDROW.Value.ToString());

            DA.InsertCommand.CommandText = "insert into LITRES..ACCOUNTS (LRLOGIN,LRPWD,CREATED) values (@LRLOGIN, @LRPWD, @CREATED) ";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
            LoadData();
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        mpeADD.Hide();
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Label10.Visible = false;
        TextBox2.Text = "";
        TextBox1.Text = "";
        mpeADD.Show();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        mpeEDIT.Hide();
    }
    internal void grid_command(Object sender, GridViewCommandEventArgs e)
    {
       
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


    protected void Button7_Click(object sender, EventArgs e)
    {
        mpeDEL.Hide();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.DeleteCommand = new SqlCommand();
        DA.DeleteCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/litres"));
        DA.DeleteCommand.Parameters.AddWithValue("ID", hfIDOFSELECTEDROW.Value.ToString());
        DA.DeleteCommand.CommandText = "delete from LITRES..ACCOUNTS  where ID = @ID";
        DA.DeleteCommand.Connection.Open();
        DA.DeleteCommand.ExecuteNonQuery();
        DA.DeleteCommand.Connection.Close();
        LoadData();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Label9.Visible = false;
        if ((TextBox3.Text == "") || (TextBox4.Text == ""))
        {
            Label9.Visible = true;
            mpeEDIT.Show();
            return;
        }
        else
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.UpdateCommand = new SqlCommand();
            DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/litres"));
            DA.UpdateCommand.Parameters.AddWithValue("LRLOGIN", TextBox3.Text);
            DA.UpdateCommand.Parameters.AddWithValue("LRPWD", TextBox4.Text);
            DA.UpdateCommand.Parameters.AddWithValue("CHANGED", DateTime.Now);
            DA.UpdateCommand.Parameters.AddWithValue("ID", hfIDOFSELECTEDROW.Value.ToString());
            DA.UpdateCommand.CommandText = "update LITRES..ACCOUNTS set LRLOGIN = @LRLOGIN, LRPWD = @LRPWD, CHANGED = @CHANGED " +
                                               " where ID = @ID";
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();
            LoadData();
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandSource.GetType() == typeof(GridView))
            return;
        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        int RowIndex = gvr.RowIndex;
        switch (e.CommandName)
        {
            case "upd":
                TextBox3.Text = GridView1.Rows[RowIndex].Cells[1].Text;
                TextBox4.Text = GridView1.Rows[RowIndex].Cells[2].Text;
                hfIDOFSELECTEDROW.Value = (Convert.ToInt32(e.CommandArgument)).ToString();
                Label9.Visible = false;
                mpeEDIT.Show();
                break;
            case "del":
                hfIDOFSELECTEDROW.Value = (Convert.ToInt32(e.CommandArgument)).ToString();
                mpeDEL.Show();
                break;
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        LoadData();
        GridView1.DataBind();

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("stats.aspx");
    }
    protected void Button10_Click(object sender, EventArgs e)
    {
        string tmp = "Litres_users" + DateTime.Now.ToString("hh:mm:ss") + ".xls";

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=" + tmp);
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

}
