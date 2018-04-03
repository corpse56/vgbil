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
using System.Data.SqlClient;
using System.Xml;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

public partial class PersAccBrit : System.Web.UI.Page
{
    public SqlConnection Con;
    public Reader reader;
    SqlDataAdapter DA;
    private void ISS()
    {
        //SetPenalty(reader.ID);
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = Con;
        //Con.Open();
        DA.SelectCommand.CommandText = "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_VOZV vzv,A.DATE_FACT_VOZV fct,A.PENALTY,A.REMPENALTY " +
                                       " from BRIT_SOVET.dbo.ZAKAZ A " +
                                       " left join BRIT_SOVET..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BRIT_SOVET..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDMAIN != 0))";
        DataSet DS = new DataSet();
        DA.Fill(DS, "frm");
        //Con.Close();
        FillFRM(DS.Tables["frm"]);

    }
    private void HIS()
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = Con;
        //Con.Open();
        DA.SelectCommand.CommandText = "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_VOZV vzv,A.DATE_FACT_VOZV fct,A.PENALTY,A.REMPENALTY " +
                                       " from BRIT_SOVET.dbo.ZAKAZ A " +
                                       " left join BRIT_SOVET..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BRIT_SOVET..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDMAIN = 0))";
        DataSet DS = new DataSet();
        DA.Fill(DS, "frm");
        //Con.Close();
        FillHIS(DS.Tables["frm"]);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (TabContainer1.ActiveTabIndex)
        {
            case 0://выданные книги
                ISS();
                break;
            case 1://история
                HIS();
                break;

        };

    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
        reader = new Reader(this.User.Identity.Name, Request["id"]);
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        Label1.Text = "Личный кабинет пользователя : " + reader.FIO;
        TabContainer1.ActiveTabIndex = 0;
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
    public class Reader
    {
        public Reader(string login, string sess)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberReader = " + login;
            //DA.SelectCommand.Connection.Open();
            DataSet DS = new DataSet();
            int recc = DA.Fill(DS, "Reader");
            this.ID = DS.Tables["Reader"].Rows[0]["NumberReader"].ToString();
            //this.Login = DS.Tables["Reader"].Rows[0]["login"].ToString();
            this.FIO = DS.Tables["Reader"].Rows[0]["FamilyName"].ToString() + " " + DS.Tables["Reader"].Rows[0]["Name"].ToString() + " " + DS.Tables["Reader"].Rows[0]["FatherName"].ToString();
            this.Session = sess;
            //DA.SelectCommand.Connection.Close();
        }
        public string FIO;
        public string ID;
        public string Session;
    }

    void FillFRM(DataTable t)
    {
        Table1.Rows.Clear();
        Table1.BorderStyle = BorderStyle.Solid;
        Table1.BorderWidth = 2;
        Table1.BorderColor = Color.Black;
        Table1.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        tr.Height = 30;
        TableCell tc = new TableCell();
        tc.Text = "№№";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Заглавие";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Автор";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Дата выдачи";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Предполагаемая дата возврата";
        tr.Cells.Add(tc);
        tr.BackColor = Color.LightGray;
        Table1.Rows.Add(tr);
        t.Columns["vzv"].DataType = typeof(DateTime);
        t.Columns["iss"].DataType = typeof(DateTime);
        t.Columns["fct"].DataType = typeof(DateTime);
        int j = 1;
        foreach (DataRow row in t.Rows)
        {
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = (j++).ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["zag"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["avt"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["iss"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["vzv"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            Table1.Rows.Add(tr);
        }
    }
    private void FillHIS(DataTable t)
    {
        Table2.Rows.Clear();
        Table2.BorderStyle = BorderStyle.Solid;
        Table2.BorderWidth = 2;
        Table2.BorderColor = Color.Black;
        Table2.GridLines = GridLines.Both;
        TableRow tr = new TableRow();
        tr.Height = 30;
        TableCell tc = new TableCell();
        tc.Text = "№№";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Заглавие";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Автор";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Дата выдачи";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Предполагаемая дата возврата";
        tr.Cells.Add(tc);
        tr.BackColor = Color.LightGray;
        Table2.Rows.Add(tr);
        t.Columns["vzv"].DataType = typeof(DateTime);
        t.Columns["iss"].DataType = typeof(DateTime);
        t.Columns["fct"].DataType = typeof(DateTime);
        int j = 1;
        foreach (DataRow row in t.Rows)
        {
            tr = new TableRow();
            tc = new TableCell();
            tc.Text = (j++).ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["zag"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["avt"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["iss"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["vzv"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            Table2.Rows.Add(tr);
        }

    }


    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabContainer1.ActiveTabIndex == 2)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("loginbrit.aspx");
        }
        if (TabContainer1.ActiveTabIndex == 0)
        {
            ISS();
        }
        if (TabContainer1.ActiveTabIndex == 1)
        {
            HIS();
        }
    }
}
