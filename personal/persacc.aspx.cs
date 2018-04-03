using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Globalization;
using ReaderForOrder;
using XMLConnections;
using BookForOrder;
using InvOfBookForOrder;
using System.Runtime.InteropServices;
using System.Threading;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using System.Web;
using System.Configuration;

public partial class persacc : System.Web.UI.Page
{
    
    public SqlConnection Con;
    public Reader reader;
    private SqlDataAdapter DA;
    private DataSet DS;
    private CheckBox[] Checkboxes;
    private DropDownList[] ComboInv;
    private TextBox[] CalendarTexts;
    private Button[] bs;
    Dictionary<string, int> JSInv;
    private List<Book> BooksForTableNew;
    private string script = "";
    private List<int> SelectedInvs;
    private List<string> SelectedDates;
    private int CheckedIndex = -1;



 

    protected override void OnInit(EventArgs e)
    {


        base.OnInit(e);
        //ff = new System.Windows.Forms.Form();
        //ff.Show();
        
        //ff.InputLanguageChanged += new System.Windows.Forms.InputLanguageChangedEventHandler(ff_InputLanguageChanged);
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        if ((Request["id"] == null) || (Request["type"] == null))
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("loginemployee.aspx");
        }


        reader = new Reader(this.User.Identity.Name, Request["id"],int.Parse(Request["type"]));

        
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        Label1.Text = "Личный кабинет читателя : " + reader.FIO;
        TabContainer1.ActiveTabIndex = 0;
        string ip = Server.MachineName;
        Book.InsertIntoBasket(reader, ip);
        if (Page.IsPostBack)
        {
            Button1.Enabled = false;
        }
        if (!Page.IsPostBack)
        {
            Session.Clear();
        }

        DA = new SqlDataAdapter();
        DA.DeleteCommand = new SqlCommand();
        DA.DeleteCommand.Connection = Con;
        if (Con.State == ConnectionState.Closed)
        {
            Con.Open();
        }
        DA.DeleteCommand.CommandText = "delete A from Reservation_O..Basket A, Reservation_O..Basket B WHERE (A.ID > B.ID) AND (A.IDMAIN=B.IDMAIN) and A.IDREADER=B.IDREADER and A.R_TYPE = B.R_TYPE";
        int i = DA.DeleteCommand.ExecuteNonQuery();
        //System.Windows.Forms.MessageBox.Show(i.ToString());
        Con.Close();
        //выбранные дата и инвентари
        if (Session["SelectedList"] != null)
        {
            SelectedInvs = (List<int>)Session["SelectedList"];
        }
        else
        {
            SelectedInvs = new List<int>();
        }
        if (Session["SelectedDates"] != null)
        {
            SelectedDates = (List<string>)Session["SelectedDates"];
        }
        else
        {
            SelectedDates = new List<string>();
        }
        if (Session["CheckedIndex"] != null)
        {
            CheckedIndex = (int)Session["CheckedIndex"];
        }
        else
        {
            CheckedIndex = -1;
        }
        Con.Dispose();
        DA.Dispose();
        //Session.Clear();

    }



    protected void load_Click(object sender, EventArgs e)
    {
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            if (reader.Type == 1)
            {
                TabContainer1.Tabs[3].Visible = false;
                TabContainer1.Tabs[4].Visible = false;
                TabContainer1.Tabs[5].Visible = false;
            }
            if (Request["litres"] == "1")
            {
                //TabContainer1.ActiveTabIndex = 6;
                ShowLitRes();
            }
        }
        //this.LangChang += new System.Windows.Forms.InputLanguageChangedEventHandler(persacc_LangChang);

        //SqlDataAdapter DA = new SqlDataAdapter();
        
        DS = new DataSet();
        DA = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        switch (TabContainer1.ActiveTabIndex)
        {
            case 4://История выданных книг
                //основной фонд
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = Con;
                Con.Open();
                DA.SelectCommand.CommandText = //"with FC as (select ZAG.PLAIN zag,AVT.PLAIN avt,A.INV inv,'Абонемент' fund, A.DATE_ISSUE iss " +
                                               //" from Reservation_R.dbo.ISSUED A " +
                                               //" left join BJVVV..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                               //" left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                               //" left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                               //" left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                              // " where A.IDREADER = " + reader.ID + " and ((A.IDMAIN = 0))" +
                                              // " union all " +
                                               " select case when A.IDMAIN = -1 then pre.[NAME] else ZAG.PLAIN end zag,case when A.IDMAIN = -1 then 'год:'+pre.[YEAR] else AVT.PLAIN end avt,case when A.IDMAIN = -1 then 'не в базе' else A.INV end  inv,'Основной фонд' fund, A.DATE_ISSUE iss, A.DATE_RET  " +
                                               " from Reservation_R.dbo.ISSUED_OF_HST A " +
                                               " left join BJVVV..DATAEXT B on A.IDMAIN  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                               " left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                               " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                               " left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                               " left join Reservation_R.dbo.PreDescr pre on pre.BARCODE = A.BAR " +
                                               " where A.IDREADER = " + reader.ID + " order by iss desc";
                //union all добавить ОФ ISSUED_OF_HST
                DS = new DataSet();
                DA.Fill(DS, "frm");
                Con.Close();
                FillISSUED_HST(DS.Tables["frm"]);

                //Британский совет
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_VOZV vzv,A.DATE_FACT_VOZV fct " +
                                       " from BRIT_SOVET.dbo.ZAKAZ A " +
                                       " left join BRIT_SOVET..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BRIT_SOVET..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDMAIN = 0))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_BS_HST(DS.Tables["frm"]);

                //Центр Американской Культуры
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,ACT.DATEACTION vzv " +
                                       " from Reservation_R.dbo.ISSUED_ACC A " +
                                       " left join BJACC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJACC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJACC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJACC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " left join Reservation_R..ISSUED_ACC_ACTIONS ACT on A.ID = ACT.IDISSUED_ACC and ACT.IDACTION in (2,5) "+
                                                                                            " and ACT.ID = (select max(B.ID) from Reservation_R..ISSUED_ACC_ACTIONS B "+
                                                                                            " where B.IDISSUED_ACC = ACT.IDISSUED_ACC) " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDSTATUS in (2,5)))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_ACC_HST(DS.Tables["frm"]);

                //Французский КЦ
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,ACT.DATEACTION vzv " +
                                       " from Reservation_R.dbo.ISSUED_FCC A " +
                                       " left join BJFCC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJFCC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJFCC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJFCC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " left join Reservation_R..ISSUED_ACC_ACTIONS ACT on A.ID = ACT.IDISSUED_ACC and ACT.IDACTION in (2,5) " +
                                                                                            " and ACT.ID = (select max(B.ID) from Reservation_R..ISSUED_ACC_ACTIONS B " +
                                                                                            " where B.IDISSUED_ACC = ACT.IDISSUED_ACC) " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDSTATUS in (2,5)))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_FCC_HST(DS.Tables["frm"]);
                //Центр Славянской Культуры
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,ACT.DATEACTION vzv " +
                                       " from Reservation_R.dbo.ISSUED_SCC A " +
                                       " left join BJSCC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJSCC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJSCC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJSCC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " left join Reservation_R..ISSUED_ACC_ACTIONS ACT on A.ID = ACT.IDISSUED_ACC and ACT.IDACTION in (2,5) " +
                                                                                            " and ACT.ID = (select max(B.ID) from Reservation_R..ISSUED_ACC_ACTIONS B " +
                                                                                            " where B.IDISSUED_ACC = ACT.IDISSUED_ACC) " +
                                       " where A.IDREADER = " + reader.ID + " and A.IDSTATUS in (2,5) and BaseId = 1";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_SCC_HST(DS.Tables["frm"]);

                break;
            case 3://выданные книги
                //основной фонд
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = Con;
                Con.Open();
                DA.SelectCommand.CommandText = //"with FC as (select ZAG.PLAIN zag,AVT.PLAIN avt,A.INV inv, 'Абонемент' fund, A.DATE_ISSUE iss, '' dest, A.DATE_VOZV dret " +
                                               //" from Reservation_R.dbo.ISSUED A " +
                                               //" left join BJVVV..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                               //" left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                               //" left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                               //" left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                               //" where A.IDREADER = " + reader.ID + " and ((A.IDMAIN != 0))" +
                                              //          " union all " +
                                               " select case when A.IDMAIN = -1 then pre.[NAME] else ZAG.PLAIN end zag, "+
                                               " case when A.IDMAIN = -1 then 'год:'+pre.[YEAR] else AVT.PLAIN end avt, " +
                                               " case when A.IDMAIN = -1 then 'не в базе' else A.INV end  inv, 'Основной фонд' fund, "+
                                               " A.DATE_ISSUE iss, case when ATHOME = 0 then 'В зал' else 'На дом' end dest, " +
                                               " A.DATE_RET dret" +
                                               " from Reservation_R.dbo.ISSUED_OF A " +
                                               " left join BJVVV..DATAEXT B on A.IDMAIN  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                               " left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                               " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                               " left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                               " left join Reservation_R.dbo.PreDescr pre on pre.BARCODE = A.BAR " +
                                               " where A.IDREADER = " + reader.ID + " and A.IDMAIN != 0 order by iss desc";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                Con.Close();
                FillISSUED(DS.Tables["frm"]);


                //Британский совет
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_VOZV vzv,A.DATE_FACT_VOZV fct " +
                                       " from BRIT_SOVET.dbo.ZAKAZ A " +
                                       " left join BRIT_SOVET..DATAEXT B on A.IDMAIN_CONST  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BRIT_SOVET..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BRIT_SOVET..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDMAIN != 0))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_BS(DS.Tables["frm"]);
                //Центр Американской Культуры
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_RETURN vzv " +
                                       " from Reservation_R.dbo.ISSUED_ACC A " +
                                       " left join BJACC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJACC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJACC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJACC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDSTATUS = 1))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_ACC(DS.Tables["frm"]);
                //Французский КЦ
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_RETURN vzv " +
                                       " from Reservation_R.dbo.ISSUED_FCC A " +
                                       " left join BJFCC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJFCC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJFCC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJFCC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDSTATUS = 1))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_FCC(DS.Tables["frm"]);
                //Центр Славянской Культуры
                DA.SelectCommand.CommandText =
                                        "select ZAG.PLAIN zag,AVT.PLAIN avt, A.DATE_ISSUE iss,A.DATE_RETURN vzv " +
                                       " from Reservation_R.dbo.ISSUED_SCC A " +
                                       " left join BJSCC..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJSCC..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJSCC..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJSCC..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.IDREADER = " + reader.ID + " and ((A.IDSTATUS = 1))";
                DS = new DataSet();
                DA.Fill(DS, "frm");
                FillISSUED_SCC(DS.Tables["frm"]);

                break;
            case 1://электронные книги

                FillEBOOK(GetEBOOKTable());

                break;
            case 2://история электронные книги

                FillEBOOK_HST(GetEBOOK_HSTTable());

                break;
            case 0:
                if (BuildBasketTable(BasketTable))
                {
                    Button1.Enabled = true;
                }
                else
                {
                    Button1.Enabled = false;
                }

                BuildOrdersTable(OrdersTable);
                break;
            case 5:
                BuildOrdHisTable(tORDHIS);
                break;
            case 6:
                //TabContainer1_ActiveTabChanged(sender, e);
                ShowLitRes();
                break;
        };
        Con.Dispose();
        DA.Dispose();
    }

    private void FillISSUED_SCC_HST(DataTable dataTable)
    {
        gv_SCC_HST.DataSource = dataTable;
        ((BoundField)gv_SCC_HST.Columns[0]).DataField = "zag";
        ((BoundField)gv_SCC_HST.Columns[1]).DataField = "avt";
        ((BoundField)gv_SCC_HST.Columns[2]).DataField = "iss";
        ((BoundField)gv_SCC_HST.Columns[3]).DataField = "vzv";
        ((BoundField)gv_SCC_HST.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_SCC_HST.Columns[3]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        gv_SCC_HST.DataBind();
    }

    private void FillISSUED_FCC_HST(DataTable dataTable)
    {
        gv_FCC_HST.DataSource = dataTable;
        ((BoundField)gv_FCC_HST.Columns[0]).DataField = "zag";
        ((BoundField)gv_FCC_HST.Columns[1]).DataField = "avt";
        ((BoundField)gv_FCC_HST.Columns[2]).DataField = "iss";
        ((BoundField)gv_FCC_HST.Columns[3]).DataField = "vzv";
        ((BoundField)gv_FCC_HST.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_FCC_HST.Columns[3]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        gv_FCC_HST.DataBind();
    }

    private void FillISSUED_ACC_HST(DataTable dataTable)
    {
        gv_ACC_HST.DataSource = dataTable;
        ((BoundField)gv_ACC_HST.Columns[0]).DataField = "zag";
        ((BoundField)gv_ACC_HST.Columns[1]).DataField = "avt";
        ((BoundField)gv_ACC_HST.Columns[2]).DataField = "iss";
        ((BoundField)gv_ACC_HST.Columns[3]).DataField = "vzv";
        ((BoundField)gv_ACC_HST.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_ACC_HST.Columns[3]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        gv_ACC_HST.DataBind();
    }

    private void FillISSUED_BS_HST(DataTable dataTable)
    {
        gv_BS_HST.DataSource = dataTable;
        ((BoundField)gv_BS_HST.Columns[0]).DataField = "zag";
        ((BoundField)gv_BS_HST.Columns[1]).DataField = "avt";
        ((BoundField)gv_BS_HST.Columns[2]).DataField = "iss";
        ((BoundField)gv_BS_HST.Columns[3]).DataField = "vzv";
        ((BoundField)gv_BS_HST.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_BS_HST.Columns[3]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        gv_BS_HST.DataBind();
    }

    private void FillISSUED_SCC(DataTable dataTable)
    {
        gv_SCC.DataSource = dataTable;
        ((BoundField)gv_SCC.Columns[0]).DataField = "zag";
        ((BoundField)gv_SCC.Columns[1]).DataField = "avt";
        ((BoundField)gv_SCC.Columns[2]).DataField = "iss";
        ((BoundField)gv_SCC.Columns[3]).DataField = "vzv";
        ((BoundField)gv_SCC.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_SCC.Columns[3]).DataFormatString = "{0:dd.MM.yyyy}";
        gv_SCC.DataBind();
    }

    private void FillISSUED_FCC(DataTable dataTable)
    {
        gv_FCC.DataSource = dataTable;
        ((BoundField)gv_FCC.Columns[0]).DataField = "zag";
        ((BoundField)gv_FCC.Columns[1]).DataField = "avt";
        ((BoundField)gv_FCC.Columns[2]).DataField = "iss";
        ((BoundField)gv_FCC.Columns[3]).DataField = "vzv";
        ((BoundField)gv_FCC.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_FCC.Columns[3]).DataFormatString = "{0:dd.MM.yyyy}";
        gv_FCC.DataBind();
    }

    private void FillISSUED_ACC(DataTable dataTable)
    {
        gv_ACC.DataSource = dataTable;
        ((BoundField)gv_ACC.Columns[0]).DataField = "zag";
        ((BoundField)gv_ACC.Columns[1]).DataField = "avt";
        ((BoundField)gv_ACC.Columns[2]).DataField = "iss";
        ((BoundField)gv_ACC.Columns[3]).DataField = "vzv";
        ((BoundField)gv_ACC.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_ACC.Columns[3]).DataFormatString = "{0:dd.MM.yyyy}";
        gv_ACC.DataBind();
    }

    private void FillISSUED_BS(DataTable dataTable)
    {
        gv_BS.DataSource = dataTable;
        ((BoundField)gv_BS.Columns[0]).DataField = "zag";
        ((BoundField)gv_BS.Columns[1]).DataField = "avt";
        ((BoundField)gv_BS.Columns[2]).DataField = "iss";
        ((BoundField)gv_BS.Columns[3]).DataField = "vzv";
        ((BoundField)gv_BS.Columns[2]).DataFormatString = "{0:dd.MM.yyyy HH:mm}";
        ((BoundField)gv_BS.Columns[3]).DataFormatString = "{0:dd.MM.yyyy}";
        gv_BS.DataBind();
    }



    private DataTable GetEBOOK_HSTTable()
    {
        DS = new DataSet();
        DA = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = Con;
        Con.Open();
        DA.SelectCommand.CommandText =
                                       " select ZAG.PLAIN  zag, " +
                                       " AVT.PLAIN avt, " +
                                       " A.DATEISSUE iss," +
                                       " A.DATERETURN dret,A.IDMAIN idm,A.IDREADER idr,A.ID id" +
                                       " from Reservation_R.dbo.ELISSUED_HST A " +
                                       " left join BJVVV..DATAEXT B on A.IDMAIN  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.R_TYPE = "+reader.Type+" and A.IDREADER = " + reader.ID + " order by iss desc";
        DS = new DataSet();
        DA.Fill(DS, "frm");
        Con.Close();
        return DS.Tables["frm"];
    }
    private DataTable GetEBOOKTable()
    {
        DS = new DataSet();
        DA = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = Con;
        Con.Open();
        DA.SelectCommand.CommandText =
                                       " select ZAG.PLAIN  zag, " +
                                       " AVT.PLAIN avt, " +
                                       " A.DATEISSUE iss," +
                                       " A.DATERETURN dret, A.VIEWKEY vkey ,A.IDMAIN idm,A.IDREADER idr,A.ID id,A.R_TYPE rtype " +
                                       " from Reservation_R.dbo.ELISSUED A " +
                                       " left join BJVVV..DATAEXT B on A.IDMAIN  = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                       " left join BJVVV..DATAEXTPLAIN ZAG on ZAG.IDDATAEXT = B.ID " +
                                       " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                                       " left join BJVVV..DATAEXTPLAIN AVT on AVT.IDDATAEXT = C.ID " +
                                       " where A.R_TYPE = " + reader.Type + " and A.IDREADER = " + reader.ID + " order by iss";
        DS = new DataSet();
        DA.Fill(DS, "frm");
        Con.Close();
        return DS.Tables["frm"];
    }
    private void BuildOrdHisTable(Table tORDHIS)
    {
        tORDHIS.GridLines = GridLines.Both;
        /*for (int i = 0; i < Table2.Rows.Count; i++)
            Table2.Rows.RemoveAt(0);*/
        tORDHIS.Rows.Clear();
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        tORDHIS.Rows.Add(row);
        tORDHIS.BorderWidth = 3;
        row.Cells.Add(cell);
        tORDHIS.Rows[0].Cells[0].ColumnSpan = 4;
        tORDHIS.Rows[0].Cells[0].Text = "<b>ИСТОРИЯ ЗАКАЗОВ</b>";
        row = new TableRow();
        cell = new TableCell();
        cell.Text = "<b>Краткое библиографическое описание</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "<b>Дата</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "<b>Инвентарный номер</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "<b>Статус заказа</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        /*cell = new TableCell();
        cell.Text = "X";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);*/
        DA = new SqlDataAdapter();
        tORDHIS.Rows.Add(row);
        DA.SelectCommand = new SqlCommand();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        DA.SelectCommand.Connection = Con;
        DA.SelectCommand.CommandText = "select * from Reservation_O..OrdHis where ID = 0";
        DA.Fill(DS, "Orders");
        DA.SelectCommand.Connection.Close();
        //Checking reader = new Checking("450", HttpContext.Current.User.Identity.Name);//"1"); // читатель здесь павен не "1" а такой, который мне передадут ребята их пхп
        DA.SelectCommand.CommandText = "select O.*,DTP.PLAIN zag, RTF.RTF rtf, O.ID idord from Reservation_O..OrdHis O " +
                                                 "left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                                                 "where  DT.MSFIELD='$a' and DT.MNFIELD=200 and O.ID_Reader = " + reader.ID +
                                                 " order by O.Start_Date desc";
        if (DS.Tables["Orders"] != null) DS.Tables["Orders"].Clear();
        int tst = DA.Fill(DS.Tables["Orders"]);
        DA.SelectCommand.Connection.Close();
        for (int i = 0; i < DS.Tables["Orders"].Rows.Count; i++)
        {
            row = new TableRow();
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            row.VerticalAlign = VerticalAlign.Middle;
            tORDHIS.Rows.Add(row);
            //System.Windows.Forms.RichTextBox rt = new System.Windows.Forms.RichTextBox();
            //rt.Rtf = DS.Tables["Orders"].Rows[i]["rtf"].ToString();

            //tORDHIS.Rows[i + 2].Cells[0].Text = rt.Text;
            tORDHIS.Rows[i + 2].Cells[0].Text = GetBibDescr(DS.Tables["Orders"].Rows[i]["rtf"].ToString());
            tORDHIS.Rows[i + 2].Cells[3].Text = GetStatus(DS.Tables["Orders"].Rows[i][4].ToString(), DS.Tables["Orders"].Rows[i]["REFUSUAL"].ToString(), DS.Tables["Orders"].Rows[i]["idord"].ToString());
            if (!tORDHIS.Rows[i + 2].Cells[3].Text.Contains("Отказ"))
            {
                tORDHIS.Rows[i + 2].Cells[3].Text = "Завершено";
            }
            DateTime DT = (DateTime)DS.Tables["Orders"].Rows[i][5];
            tORDHIS.Rows[i + 2].Cells[1].Text = DT.ToShortDateString().ToString();
            tORDHIS.Rows[i + 2].Cells[2].Text = DS.Tables["Orders"].Rows[i]["InvNumber"].ToString();
        }
        Con.Dispose();
        DA.Dispose();
    }

    private bool BuildBasketTable(Table tBasket)
    {
        DA = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        DA.SelectCommand = new SqlCommand("select * from Reservation_O..Basket where IDREADER = " + reader.ID /*+ HttpContext.Current.User.Identity.Name*/, Con);
        DS = new DataSet();
        int count = DA.Fill(DS, "Basket");
        DA.SelectCommand = new SqlCommand("select distinct R.IDMAIN idm, ZAG.PLAIN zag, INV.PLAIN inv, RTF.RTF rtf, " +
                                        "MHRAN.NAME mhran, KLASS.PLAIN klass, AVT.PLAIN AVT, R.ID idbas , DALL.SORT a482,DINV.IDDATA iddata, DALL.IDDATA a_iddata, phttp.PLAIN http " +
                                        "from Reservation_O..Basket R " +
                                        "left join BJVVV..DATAEXT DZAG on R.IDMAIN = DZAG.IDMAIN and DZAG.MNFIELD = 200 and DZAG.MSFIELD = '$a' " +
                                        "left join BJVVV..DATAEXT DAVT on R.IDMAIN = DAVT.IDMAIN and DAVT.MNFIELD = 700 and DAVT.MSFIELD = '$a' " +
                                        "left join BJVVV..DATAEXT DINV on R.IDMAIN = DINV.IDMAIN and DINV.MNFIELD = 899 and DINV.MSFIELD = '$p' " +
                                        "left join BJVVV..DATAEXT DALL on R.IDMAIN = DALL.IDMAIN and DALL.MNFIELD = 482 and DALL.MSFIELD = '$a' and DALL.IDDATA = DINV.IDDATA " +
                                        "left join BJVVV..DATAEXT DMHRAN on R.IDMAIN = DMHRAN.IDMAIN and DMHRAN.MNFIELD = 899 and DMHRAN.MSFIELD = '$a' and DINV.IDDATA = DMHRAN.IDDATA " +
                                        "left join BJVVV..DATAEXTPLAIN ZAG on DZAG.ID = ZAG.IDDATAEXT " +
                                        "left join BJVVV..RTF RTF on RTF.IDMAIN = R.IDMAIN " +

                                        "left join BJVVV..DATAEXTPLAIN AVT on DAVT.ID = AVT.IDDATAEXT " +
                                        "left JOIN BJVVV..DATAEXTPLAIN INV on DINV.ID = INV.IDDATAEXT " +
                                        "left JOIN BJVVV..DATAEXTPLAIN MHRANshort on DMHRAN.ID = MHRANshort.IDDATAEXT " +
                                        "left join BJVVV..LIST_8 MHRAN on MHRANshort.PLAIN = MHRAN.SHORTNAME " +
                                        "left join BJVVV..DATAEXT DKLASS on INV.IDDATA = DKLASS.IDDATA and DKLASS.MNFIELD = 921 and DKLASS.MSFIELD = '$c'" +
                                        "left join BJVVV..DATAEXTPLAIN KLASS on DKLASS.ID = KLASS.IDDATAEXT " +
                                        " left join BJVVV..DATAEXT http on http.IDMAIN = R.IDMAIN and http.MNFIELD = 940 and http.MSFIELD = '$a' " +
                                        " left join BJVVV..DATAEXTPLAIN phttp on http.ID = phttp.IDDATAEXT " +
                                        "where (phttp.PLAIN like '%elcir%' or phttp.PLAIN is null) and R.R_TYPE = "+reader.Type+" and R.IDREADER = " + reader.ID + 
                                        " and (DALL.SORT is not null or INV.PLAIN is not null)"+
                                        " order by idm"
                                        , Con);
        DA.SelectCommand.CommandTimeout = 1200;

        int excnt = DA.Fill(DS, "ExactlyBasket");
        List<Book> BooksForTable = new List<Book>();
        BuildTopPortionBasket(tBasket);

        bool retVal;
        if (excnt != 0)
        {
            BooksForTableNew = GetBooksForBasketTable(DS.Tables["ExactlyBasket"]);
            BuildDictionary(BooksForTableNew);
            FillBasketTable(BooksForTableNew, tBasket);
            BuildScriptNew(BooksForTableNew);
            retVal = true;
        }
        else
        {
            retVal = false;
        }
        Label1.Text = "Личный кабинет читателя " + reader.FIO;
        Con.Dispose();
        DA.Dispose();
        return retVal;
    }
    private void BuildDictionary(List<Book> BooksForTable)
    {
        JSInv = new Dictionary<string, int>();
        int i=-1,map=-1;
        foreach (Book b in BooksForTable)
        {
            i++;
            //if (bs[i] == null)
               // continue;
            foreach (InvOfBook inv in b.InvsOfBook)
            {
                if (inv.ForOrder == true)
                {
                    map++;
                    try
                    {
                        if (inv.inv.Contains("Электронная"))
                        {
                            JSInv.Add(inv.IDMAIN+inv.inv, map);
                        }
                        else
                        {
                            if (inv.note != null)
                                JSInv.Add(inv.inv + " " + inv.note, map);
                            else
                                JSInv.Add(inv.inv, map);
                        }
                    }
                    catch (Exception exc)
                    {
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }
    private void BuildOrdersTable(Table tOrder)
    {
        tOrder.GridLines = GridLines.Both;
        /*for (int i = 0; i < Table2.Rows.Count; i++)
            Table2.Rows.RemoveAt(0);*/
        tOrder.Rows.Clear();
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        tOrder.Rows.Add(row);
        tOrder.BorderWidth = 3;
        row.Cells.Add(cell);
        tOrder.Rows[0].Cells[0].ColumnSpan = 4;
        tOrder.Rows[0].Cells[0].Text = "<b>ЗАКАЗЫ</b>";
        row = new TableRow();
        cell = new TableCell();
        cell.Text = "<b>Краткое библиографическое описание</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "<b>Дата</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "<b>Статус заказа</b>";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = "X";
        cell.HorizontalAlign = HorizontalAlign.Center;
        row.Cells.Add(cell);
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        DA = new SqlDataAdapter();
        tOrder.Rows.Add(row);

        DA.SelectCommand = new SqlCommand("select * from Reservation_O..Orders where ID = 0", Con);
        DA.Fill(DS, "Orders");
        DA.SelectCommand.Connection.Close();
        //Checking reader = new Checking("450", HttpContext.Current.User.Identity.Name);//"1"); // читатель здесь павен не "1" а такой, который мне передадут ребята их пхп
        DA.SelectCommand.CommandText = "select O.*,DTP.PLAIN zag, RTF.RTF rtf, O.ID idord,O.InvNumber inv, O.ID_Book_EC idm, O.IDDATA idd from Reservation_O..Orders O " +
                                                 "left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                                                 "where  DT.MSFIELD='$a' and DT.MNFIELD=200 and O.ID_Reader = " + reader.ID;//когда с читателем буду делать надо переделать
        if (DS.Tables["Orders"] != null) DS.Tables["Orders"].Clear();
        int tst = DA.Fill(DS.Tables["Orders"]);
        DA.SelectCommand.Connection.Close();
        for (int i = 0; i < DS.Tables["Orders"].Rows.Count; i++)
        {
            //Checking ch = new Checking(DSetBasket.Tables["Orders"].Rows[i][2].ToString(), reader.GetIDR());
            row = new TableRow();
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            cell = new TableCell();
            row.Cells.Add(cell);
            row.VerticalAlign = VerticalAlign.Middle;
            tOrder.Rows.Add(row);
            //System.Windows.Forms.RichTextBox rt = new System.Windows.Forms.RichTextBox();
            //rt.Rtf = DS.Tables["Orders"].Rows[i]["rtf"].ToString();
            InvOfBook inv = new InvOfBook(DS.Tables["Orders"].Rows[i]["inv"].ToString(), DS.Tables["Orders"].Rows[i]["idm"].ToString(), DS.Tables["Orders"].Rows[i]["idd"].ToString());

            tOrder.Rows[i + 2].Cells[0].Text = GetBibDescr(DS.Tables["Orders"].Rows[i]["rtf"].ToString()) + " (" + inv.inv + " " + inv.note + ")";
            tOrder.Rows[i + 2].Cells[2].Text = GetStatus(DS.Tables["Orders"].Rows[i][4].ToString(), DS.Tables["Orders"].Rows[i]["REFUSUAL"].ToString(), DS.Tables["Orders"].Rows[i]["idord"].ToString());

            //Type t = DSetBasket.Tables["Orders"].Rows[i][5].GetType();
            DateTime DT = (DateTime)DS.Tables["Orders"].Rows[i][5];
            tOrder.Rows[i + 2].Cells[1].Text = DT.ToShortDateString().ToString();
            LinkButton del2 = new LinkButton();
            del2.Text = "X";
            del2.ID = "ord" + DS.Tables["Orders"].Rows[i]["idord"].ToString();
            del2.ForeColor = Color.Red;
            del2.Click += new EventHandler(del2_Click);
            if (tOrder.Rows[i + 2].Cells[2].Text == "Поступило требование")
            {
                tOrder.Rows[i + 2].Cells[3].Controls.Add(del2);
            }
            //del2.OnClientClick = "updatepanel();";
            Button bn = new Button();

        }
        Con.Dispose();
        DA.Dispose();
    }
    public void BuildTopPortionBasket(Table tBasket)
    {
        tBasket.Rows.Clear();
        tBasket.Style["left"] = "30px";
        tBasket.Style["top"] = "50px";
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        tBasket.BorderColor = System.Drawing.Color.Black;
        tBasket.BorderWidth = 3;
        cell.ColumnSpan = 6;
        row.Cells.Add(cell);
        row.Cells[0].Text = "<b>КОРЗИНА</b>";
        tBasket.Rows.Add(row);
    }
    public void FillBasketTable(List<Book> BooksForTableNew,Table tBasket)
    {
        TableRow row = new TableRow();
        TableRow currow = new TableRow();
        TableRow tmprow = new TableRow();
        TableCell cell = new TableCell();
        int AllInvCount = BooksForTableNew.Count;//Book.CountInvsInBookList(BooksForTable);
        Checkboxes = new CheckBox[AllInvCount];
        CalendarTexts = new TextBox[AllInvCount];
        ComboInv = new DropDownList[AllInvCount];
        if (Session["SelectedList"] != null)
        {
            SelectedInvs = (List<int>)Session["SelectedList"];
        }
        else
        {
            SelectedInvs = new List<int>(AllInvCount);
            for (int si = 0; si < AllInvCount; si++)
            {
                SelectedInvs.Add(0);
            }
            //Session.Remove("SelectedList");
            Session.Add("SelectedList", SelectedInvs);
        }
        if (Session["SelectedDates"] != null)
        {
            SelectedDates = (List<string>)Session["SelectedDates"];
        }
        else
        {
            SelectedDates = new List<string>(AllInvCount);
            for (int si = 0; si < AllInvCount; si++)
            {
                SelectedDates.Add("");
            }
            //Session.Remove("SelectedList");
            Session.Add("SelectedDates", SelectedDates);
        }
        if (Session["CheckedIndex"] != null)
        {
            CheckedIndex = (int)Session["CheckedIndex"];
        }
        else
        {
            CheckedIndex = -1;
            //Session.Remove("SelectedList");
            Session.Add("CheckedIndex", CheckedIndex);
        }
        //Calendars = new CalendarExtender[AllInvCount];
        //CalendarsOrd = new CalendarExtender[AllInvCount];
        bs = new Button[AllInvCount];
        int i = -1;
        int j = 0;
        int booki = -1;
        int RowsForBook = 0;
        //Table5.Rows.Clear();
        if (BooksForTableNew != null)
            foreach (Book b in BooksForTableNew)
            {
                i++;
                bs[i] = new Button();
                CalendarTexts[i] = new TextBox();
                Checkboxes[i] = new CheckBox();
                ComboInv[i] = new DropDownList();
                row = new TableRow();
                cell = new TableCell();
                cell.ColumnSpan = 6;
                cell.Height = new Unit(6);
                row.BackColor = Color.Black;
                row.Cells.Add(cell);
                tBasket.Rows.Add(row);
                row = new TableRow();
                //row.Style["border-bottom"] = "solid";
                //row.Style["border-bottom-width"] = "3px";

                booki++;
                currow = row;

                if (i % 2 != 0)
                {
                    row.BackColor = Color.AliceBlue;
                }
                else
                {
                    row.BackColor = Color.Cornsilk;
                }
                cell = new TableCell();
                cell.Text = (i + 1).ToString();
                RowsForBook = GetRowsCntForBook(b);
                cell.RowSpan = RowsForBook + 1;

                row.Cells.Add(cell);
                cell = new TableCell();
                cell.Text = b.Name;
                cell.ColumnSpan = 4;
                row.Cells.Add(cell);
                tBasket.Rows.Add(row);
                tmprow = row;
                List<InvOfBook> InvsOfKn = new List<InvOfBook>();
                foreach (InvOfBook inv in b.InvsOfBook)
                {
                    if (inv.ForOrder == true) // если надо заказывать из книгохранения. абонемент считается как зал
                    {
                        InvsOfKn.Add(inv);
                        continue;
                    }
                    //если заказывается из зала
                    row = new TableRow();
                    booki++;
                    row.BackColor = currow.BackColor;
                    tBasket.Rows.Add(row);
                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    if (inv.note != null)
                    {
                        cell.Text = inv.inv+" " +inv.note;
                    }
                    else
                    {
                        cell.Text = inv.inv;
                    }
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";

                    string Limitations = inv.GetLimitation(int.Parse(reader.ID),reader.Type);
                    if (Limitations != "")
                    {
                        cell.Text = Limitations;
                    }

                    row.Cells.Add(cell);
                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";

                    if (inv.mhr == "error1")
                    {
                        cell.Text = "Ошибка";
                    }
                    else
                    {
                        cell.Text = inv.mhr;
                    }
                    row.Cells.Add(cell);
                }
                if (InvsOfKn.Count != 0)//если надо заказывать из книгохранения или из абонемента
                {
                    row = new TableRow();
                    booki++;
                    //row.Height = new Unit(row.Height.Value + 35);
                    row.BackColor = currow.BackColor;
                    tBasket.Rows.Add(row);
                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    Checkboxes[i] = new CheckBox();
                    Checkboxes[i].ID = "ch" + i.ToString();
                    Checkboxes[i].AutoPostBack = true;
                    Checkboxes[i].CheckedChanged += new EventHandler(persacc_CheckedChanged);

                    if (Session["CheckedIndex"] != null)
                    {
                        int tmpch = (int)Session["CheckedIndex"];
                        if (tmpch == i)
                            Checkboxes[i].Checked = true;
                    }

                    cell.Controls.Add(Checkboxes[i]);
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    ComboInv[i] = new DropDownList();
                    ComboInv[i].ID = "cmb" + i.ToString();
                    ComboInv[i].Width = 100;
                    ComboInv[i].SelectedIndexChanged += new EventHandler(_Default_SelectedIndexChanged);
                    //ComboInv[i].Attributes.Add("UseSubmitBehavior", "false");
                    ComboInv[i].AutoPostBack = true;
                    ListItemCollection lic = new ListItemCollection();
                    foreach (InvOfBook inv in InvsOfKn)
                    {
                        if (inv.note != null)
                        {
                            ListItem li = new ListItem(inv.inv+" "+inv.note, inv.iddata);
                            lic.Add(li);
                        }
                        else
                        {
                            ListItem li = new ListItem(inv.inv, inv.iddata);
                            lic.Add(li);
                        }
                    }
                    ComboInv[i].DataSource = lic;
                    ComboInv[i].DataTextField = "Text";
                    ComboInv[i].DataValueField = "Value";
                    ComboInv[i].DataBind();
                    if (Session["SelectedList"] != null)
                    {
                        List<int> tmp = (List<int>)Session["SelectedList"];
                        ComboInv[i].SelectedIndex = tmp[i];
                    }
                    /*if (Session["selectedinv"+(i).ToString()] != null)
                        ComboInv[i].SelectedIndex = (int)Session["selectedinv" + (i).ToString()];*/
                    //cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(ComboInv[i]);
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    CalendarTexts[i] = new TextBox();
                    CalendarTexts[i].ID = "txt" + i.ToString();
                    //DateTime BestDate = GetBestDateForTable(InvsOfKn[ComboInv[i].SelectedIndex]);
                    //CalendarTexts[i].Text = DateTime.Today.ToString("dd.MM.yyyy");
                    //DateTime tmp = DateTime.Today.Date;
                    //InvOfBook tmp = new InvOfBook(
                    //List<DateTime> tmpl = 
                    
                    
                    //тут сначала проверить специальная ли это дата а потом уже присваивать

                    if (CalendarTexts[i].Text == "")
                    {
                        CalendarTexts[i].Text = DateTime.Today.Date.ToString("dd.MM.yyyy");
                    }
                    
                    //CalendarTexts[i].ReadOnly = true; // вот из-за этой строки почему-то все переобновлялось как постбек, а не как в апдейтпанель!!!
                    CalendarTexts[i].Enabled = false;
                    CalendarTexts[i].ForeColor = Color.Black;
                    CalendarTexts[i].Width = 70;
                    CalendarTexts[i].Attributes.Add("AutoPostBack", "true");
                    CalendarTexts[i].TextChanged += new EventHandler(persacc_TextChanged);

                    //проверка даты на специальную. если специальная - переводим на следующую
                    InvOfBook selectedInv = new InvOfBook();
                    foreach (Book bb in BooksForTableNew)
                    {
                        foreach (InvOfBook inv in b.InvsOfBook)
                        {
                            if (ComboInv[i].SelectedItem.Value == inv.iddata)
                                selectedInv = inv;
                        }
                    }
                    List<DateTime> lbd = selectedInv.GetBusyDates();

                    
                    
                    if (Session["SelectedDates"] != null)
                    {
                        List<string> tmpd = (List<string>)Session["SelectedDates"];
                        if (tmpd[i] == "")
                        {
                            //CalendarTexts[i].Text = tmpd[i];
                            tmpd[i] = DateTime.Now.ToString("dd.MM.yyyy");
                        }
                        else
                        {
                            CalendarTexts[i].Text = tmpd[i];
                        }
                        DateTime tmp = DateTime.Parse(tmpd[i]);
                        foreach (DateTime bd in lbd)
                        {
                            if (tmp == bd)
                            {
                                tmp = tmp.AddDays(1);
                                CalendarTexts[i].Text = tmp.ToString("dd.MM.yyyy");
                                //persacc_TextChanged(CalendarTexts[i], new EventArgs());
                                //BuildBasketTable(BasketTable);
                            }
                        }

                        

                    }
                    //CalendarTexts[i].Style["z-index"] = "1";

                    bs[i] = new Button();
                    bs[i].ID = "bs" + i.ToString();
                    bs[i].Text = "...";
                    row.Cells.Add(cell);
                    tBasket.Rows[booki + i + 2].Cells[2].Controls.Add(CalendarTexts[i]);
                    tBasket.Rows[booki + i + 2].Cells[2].Controls.Add(bs[i]);
                    //bs[i].Attributes.Add("UseSubmitBehavior", "false");
                    
                    //bs[i].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + (j+ComboInv[i].SelectedIndex).ToString() + "]");
                    if (selectedInv.inv.Contains("Электронная"))

                        bs[i].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + JSInv[selectedInv.IDMAIN + ComboInv[i].SelectedItem.Text] + "]");
                    else
                        bs[i].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + JSInv[ComboInv[i].SelectedItem.Text] + "]");

                    bs[i].Attributes.Add("UseSubmitBehavior", "false");
                    bs[i].Attributes.Add("onclick", "var ct = document.getElementById('"
                        + CalendarTexts[i].ClientID.ToString()
                        + "');var butt = document.getElementById('"
                        + bs[i].ClientID.ToString()
                        + "');apiCal(id1["
                        + i.ToString()
                        + "],id2["
                        + i.ToString()
                        + "],ct.value);");
                    

                    j++;

                    cell = new TableCell();
                    cell.Style["border-bottom"] = "solid";
                    cell.Style["border-bottom-width"] = "3px";
                    //cell.ID = "cellmhr";


                    //if (reader.Type == 1)
                   // {
                   // }
                   // else
                    {
                        if (selectedInv.mhr.Contains("нигохранени"))
                        {
                            cell.Text = "<p style = \"font-weight:bold; color:Red\">Только в зале!</p><br>";
                        }
                        if (selectedInv.mhr.Contains("бонемент"))
                        {
                            cell.Text = "<p style = \"font-weight:bold; color:Blue\">На дом.</p><br>";
                        }
                        if (reader.Abonement != null)
                        {
                            if ((reader.Abonement.Contains("Персональный")) || (reader.Abonement.Contains("Коллективный")) || (reader.Abonement.Contains("Сотрудник")))
                            {
                                cell.Text = "<p style = \"font-weight:bold; color:Blue\">На дом.</p><br>";
                            }
                        }
                        if (selectedInv.inv.Contains("Электронная"))
                        {
                            cell.Text = "<p style = \"font-weight:bold; color:Green\">Электронная копия</p>";
                        }
                    }

                    string Limitations = selectedInv.GetLimitation(int.Parse(reader.ID), reader.Type);
                    
                    if (Limitations != "")
                    {
                        cell.Text += Limitations;
                        Checkboxes[i].Enabled = false;
                        /*if (selectedInv.inv.Contains("Электронная"))
                        {
                            if (cell.Text.Contains("Все экземпляры выданы"))
                            {
                                Checkboxes[i].Enabled = true;
                            }
                        }   */  
                    }
                    else
                    {
                        if (CalendarTexts[i].Text != "")
                        {
                            string TimeLimitations = selectedInv.GetTimeLimitation(DateTime.Parse(CalendarTexts[i].Text));
                            if (TimeLimitations != "")
                            {
                                cell.Text += TimeLimitations;
                                Checkboxes[i].Enabled = false;
                            }
                        }
                    }

                    row.Cells.Add(cell);
                    //string hhhh = cell.ClientID;
                }
                cell = new TableCell();
                cell.Style["border-bottom"] = "solid";
                cell.Style["border-bottom-width"] = "3px";
                cell.VerticalAlign = VerticalAlign.Middle;
                LinkButton del = new LinkButton();
                del.Text = "X";
                del.ToolTip = "Удалить из корзины";
                del.CommandArgument = i.ToString();
                del.ID = b.IdBasket;
                del.ForeColor = Color.Red;
                cell.Controls.Add(del);
                del.Click += new EventHandler(del_Click);
                cell.RowSpan = RowsForBook + 1;
                tmprow.Cells.Add(cell);


            }
        //ComboInv[0].SelectedValue;
        Button1.Attributes.Add("onmousedown", "isDateSelected()");

    }

    void persacc_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        string s = tb.ID;
        s = s.Substring(3, s.Length - 3);
        int i = int.Parse(s) + 1;
        SelectedDates[i - 1] = tb.Text;
        if (Session["SelectedDates"] != null)
            Session.Remove("SelectedDates");
        Session.Add("SelectedDates", SelectedDates);
        BuildBasketTable(BasketTable);
    }

    void persacc_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chb = (CheckBox)sender;
        string s = chb.ID;
        s = s.Substring(2, s.Length - 2);
        int i = int.Parse(s);
        if ((chb.Checked == false))
            i = -1;
        /*foreach(CheckBox chbf in Checkboxes)
        {
            chbf.Checked = false;
        }*/
        //((CheckBox)sender).Checked = true;
        if (Session["CheckedIndex"] != null)
            Session.Remove("CheckedIndex");
        Session.Add("CheckedIndex", i);
        BuildBasketTable(BasketTable);


    }
    void _Default_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        string s = ddl.ID;
        s = s.Substring(3, s.Length - 3);
        int i = int.Parse(s) + 1;
        int j = 0;
        InvOfBook selectedInv = new InvOfBook();
        bool fl = false;
        foreach (Book b in BooksForTableNew)
        {
            foreach (InvOfBook inv in b.InvsOfBook)
            {
                if (inv.note != null)
                {
                    if (ddl.SelectedItem.Text == inv.inv + " " + inv.note)
                    {
                        selectedInv = inv;
                        fl = true;
                        break;
                    }
                }
                else
                {
                    if (ddl.SelectedItem.Text == inv.inv)
                    {
                        selectedInv = inv;
                        fl = true;
                        break;
                    }
                }
                j++;
            }
            if (fl) break;
        }
        //int tmp = JSInv[ ComboInv[i - 1].SelectedItem.Text];
        if (selectedInv.inv.Contains("Электронная"))
            bs[i - 1].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + JSInv[selectedInv.IDMAIN + ComboInv[i - 1].SelectedItem.Text] + "]");
        else
            bs[i - 1].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp["+JSInv[ ComboInv[i - 1].SelectedItem.Text] + "]");


        DateTime BestDate = GetBestDateForTable(selectedInv);
        CalendarTexts[i - 1].Text = BestDate.ToString("dd.MM.yyyy");
        //сохраняем состояние выбраных инвентарей
        SelectedInvs[i - 1] = ddl.SelectedIndex;
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);
        /*if (Session["selectedinv"+(i-1).ToString()] != null)
            Session.Remove("selectedinv" + (i - 1).ToString());
        Session.Add("selectedinv" + (i - 1).ToString(), ddl.SelectedIndex);*/
        BuildBasketTable(BasketTable);

    }

    public List<Book> GetBooksForBasketTable(DataTable t)
    {
        long idmainConst;
        long idmain;
        idmainConst = (System.Int64)t.Rows[0]["idm"];
        idmain = idmainConst;
        //BooksForTable = new List<Book>();
        List<InvOfBook> InvsForDates = new List<InvOfBook>();
        List<InvOfBook> InvsForTable = new List<InvOfBook>();
        List<Book> res = new List<Book>();
        Book bookForTable = new Book(GetBibDescr(t.Rows[0]["rtf"].ToString()), idmain.ToString(), t.Rows[0]["avt"].ToString(), t.Rows[0]["idbas"].ToString());


        foreach (DataRow r in t.Rows)
        {
            idmain = (System.Int64)r["idm"];

            if (idmainConst != idmain)
            {
                idmainConst = idmain;
                if (bookForTable == null)
                {
                    continue;
                }

                res.Add(bookForTable);
                InvsForDates = new List<InvOfBook>();
                InvsForTable = new List<InvOfBook>();
                bookForTable = new Book(GetBibDescr(r["rtf"].ToString()), r["idm"].ToString(), r["avt"].ToString(), r["idbas"].ToString());
            }
            //здесь вставить проверку если это аллигат, т.е. а482!=null , то местохранение брать из главного аллигата.
            InvOfBook inv;
            string fffffff = r["a482"].ToString();
            if (r["a482"].ToString() != "")
            {
                //inv = new InvOfBook(r["a482"].ToString(), r["mhran"].ToString(), r["klass"].ToString(), r["idm"].ToString());
                string t1 = r["a482"].ToString();
                string t2 = r["idm"].ToString();
                string t3 = r["a_iddata"].ToString();
                inv = new InvOfBook(r["a482"].ToString(), r["idm"].ToString(), r["a_iddata"].ToString());

            }
            else
            {
                //inv = new InvOfBook(r["inv"].ToString(), r["mhran"].ToString(), r["klass"].ToString(), r["idm"].ToString());
                string t1 = r["inv"].ToString();
                string t2 = r["idm"].ToString();
                string t3 = r["iddata"].ToString();
                //if (t1 == "")//скорее всего серия. спросить потом может на уровне опака не давать класть в корзину?
                  //  continue;

                inv = new InvOfBook(r["inv"].ToString(), r["idm"].ToString(),r["iddata"].ToString());
            }

            if (inv.mhr == null)
            {
                //ErrorLabelOF.Text += "Инвентарный номер " + inv.inv +" имеет ошибку заполнения аллигата в базе. Обратитесь к дежурному сотруднику для заказа вручную! ";
                inv.mhr = "error1";
                
                //continue;
            }
            if (inv.mhr.Contains("нигохранени") || inv.mhr.Contains("бонемент"))
            {
                inv.ForOrder = true;
            }
            else
            {
                inv.ForOrder = false;
            }
            if (r["http"].ToString() != string.Empty)
            {

                InvOfBook inv_http = new InvOfBook(r["http"].ToString());
                inv_http.ForOrder = true;
                inv_http.inv = "Электронная копия";
                inv_http.mhr = "Книгохранение";
                inv_http.iddata = "Электронная копия";
                inv_http.IDMAIN = bookForTable.ID;
                if (!bookForTable.InvsOfBook.Contains(inv_http))
                    bookForTable.InvsOfBook.Add(inv_http);

            }
            if (inv.klass != "Списано")
                bookForTable.InvsOfBook.Add(inv);
        }
        res.Add(bookForTable);
        return res;
        /*List<Book> res1 = new List<Book>();
        foreach(Book b in res)
        {
            bool ffff = b.IsForAbonement();
            if ((b.IsForAbonement()) && (TabContainer1.ActiveTabIndex == 1))
            {
                res1.Add(b);
            }
        }
        if (TabContainer1.ActiveTabIndex == 1)
            return res1;
        else
            return res;*/

    }
    public void BuildScriptNew(List<Book> BooksForTable)
    {
        string scriptTemplate = @"
            <script language=""javascript"" type=""text/javascript"">
                var id1 = [{0}];
                var id2 = [{1}];
                var id3 = [{2}];
                var sp = [{3}];
            </script>";
        StringBuilder id1 = new StringBuilder();
        StringBuilder id2 = new StringBuilder();
        StringBuilder id3 = new StringBuilder();
        StringBuilder bd = new StringBuilder();
        String[] Month;
        Month = new String[12];
        int i = -1;
        int booki = -1;
        int map = -1;
        foreach (Book b in BooksForTable)
        {
            booki++;
            i++;
            if (bs[i] == null)
                continue;

            List<DateTime> BusyDates = new List<DateTime>();
            foreach (InvOfBook inv in b.InvsOfBook)
            {

                if (inv.ForOrder == true)
                {
                }
                else
                {
                    continue;
                }
                BusyDates = inv.GetBusyDates();
                //BusyDates = Wintellect.PowerCollections.Algorithms.RemoveDuplicates<DateTime>(BusyDates.ToArray());

                for (int h = 0; h < Month.Length; h++)
                {
                    Month[h] = "";
                }
                for (int Dat = 0; Dat < BusyDates.Count; Dat++)
                {
                    DateTime BusDat = (DateTime)BusyDates[Dat];
                    Month[BusDat.Month - 1] += BusDat.Day.ToString() + ",";

                }
                bd.Append("[");
                for (int h = 0; h < Month.Length; h++)
                {
                    if (Month[h].Length != 0) Month[h] = Month[h].Remove(Month[h].Length - 1, 1);
                    Month[h] = "[" + Month[h];
                    Month[h] += "]";
                    bd.Append(Month[h] + ",");
                }
                bd.Remove(bd.Length - 1, 1);
                bd.Append("],");

            }
            id1.AppendFormat("\"{0}\",", bs[i].ClientID);
            id2.AppendFormat("\"{0}\",", CalendarTexts[i].ClientID);
            id3.AppendFormat("\"{0}\",", Checkboxes[i].ClientID);
        }
        if (id1.Length != 0)
            id1.Remove(id1.Length - 1, 1);
        if (id2.Length != 0)
            id2.Remove(id2.Length - 1, 1);
        if (id3.Length != 0)
            id3.Remove(id3.Length - 1, 1);
        if (bd.Length != 0)
            bd.Remove(bd.Length - 1, 1);
        //string tmp = "";987
        script = string.Format(scriptTemplate, id1, id2, id3, bd);
        //ClientScript.RegisterClientScriptResource(typeof(string),script);//((.RegisterStartupScript(GetType(), "InitializeCalendars", script);
        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "initcals", script, false);
    }

    int GetRowsCntForBook(Book b)
    {
        int cnt = 0;
        int cnthr = 0;
        foreach (InvOfBook inv in b.InvsOfBook)
        {
            if (inv.mhr.Contains("Книгохран") || inv.mhr.Contains("Абонемент"))
            {
                cnthr++;
            }
            else
            {
                cnt++;
            }
        }
        if (cnthr > 0)
        {
            cnt++;
        }
        return cnt;
    }
    DateTime GetBestDateForTable(InvOfBook inv)
    {
        List<DateTime> BD = inv.GetBusyDates();
        if (BD.Count == 0)
            return DateTime.Today;
        //bool InRange = false;
        foreach (DateTime dt in BD)
        {
            if (dt.Date == DateTime.Today.Date)
            {
                return BD[BD.Count - 1].AddDays(1);
            }
        }
        return DateTime.Today.Date;
    }
    void del_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        Con.Open();
        sdvig.DeleteCommand = new SqlCommand("delete from Reservation_O..Basket where ID = " + ((LinkButton)sender).ID, Con);
        sdvig.DeleteCommand.ExecuteNonQuery();
        Con.Close();
        switch (TabContainer1.ActiveTabIndex)
        {
            case 0:

                if (BuildBasketTable(BasketTable))
                {
                    Button1.Enabled = true;
                }
                else
                {
                    Button1.Enabled = false;
                }
                BuildOrdersTable(OrdersTable);
                break;
        }
        int rid = int.Parse(((LinkButton)sender).CommandArgument);
        SelectedInvs.RemoveAt(rid);
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);
        SelectedDates.RemoveAt(rid);
        if (Session["SelectedDates"] != null)
            Session.Remove("SelectedDates");
        Session.Add("SelectedDates", SelectedDates);
        int tmp = -1;
        if (Session["CheckedIndex"] != null) 
        {
            tmp = (int)Session["CheckedIndex"];
            if (tmp > rid)
                tmp -= 1;
            else
            if (tmp == rid)
                tmp = -1;
            Session.Remove("CheckedIndex");
        }
        Session.Add("CheckedIndex", tmp);
        BuildBasketTable(BasketTable);
        Con.Dispose();
        sdvig.Dispose();
    }
    void del2_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        Con.Open();
        sdvig.DeleteCommand = new SqlCommand("delete from Reservation_O..Orders where ID = " + ((LinkButton)sender).ID.Substring(3), Con);
        sdvig.DeleteCommand.ExecuteNonQuery();
        Con.Close();
        switch (TabContainer1.ActiveTabIndex)
        {
            case 0:

                if (BuildBasketTable(BasketTable))
                {
                    Button1.Enabled = true;
                }
                else
                {
                    Button1.Enabled = false;
                }
                BuildOrdersTable(OrdersTable);
                break;
        }
        Con.Dispose();
        sdvig.Dispose();
    }
    private string GetBibDescr(string s)
    {
        //FreeTextBoxControls.FreeTextBox ftBox = new FreeTextBoxControls.FreeTextBox();
        //ftBox.ViewStateText.Text = s;
        //s = ftBox.ToString();
        IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc(s);
        string ret = "";
        foreach (IRtfVisual vt in rtfDocument.VisualContent)
        {
            if (vt.Kind == RtfVisualKind.Text)
                ret += ((IRtfVisualText)vt).Text;
        }
        //System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();
        //rtBox.Rtf = s;
        return ret;
        //return rtBox.Text;
    }
    public string GetStatus(string ids, string refu, string idord)
    {
        DataSet DS = new DataSet();
        Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_O..Status where ID = " + ids, Con);
        Status.Fill(DS, "Name");
        string ret = DS.Tables["Name"].Rows[0][1].ToString();
        if (ret == "Отказ")
        {
            ret += ": " + refu;
        }
        if (ret == "Выдана")
        {
            Status.SelectCommand.CommandText = "select * from Reservation_O..Orders where ID = " + idord;
            DS = new DataSet();
            Status.Fill(DS, "t");
            string idreader_ord = DS.Tables["t"].Rows[0]["ID_Reader"].ToString();
            Status.SelectCommand.CommandText = "select * from Reservation_R..ISSUED_OF where IDDATA = " + DS.Tables["t"].Rows[0]["IDDATA"];
            DS = new DataSet();
            int i = Status.Fill(DS, "t");
            if (i == 0)
            {
                ret = "Ошибочный заказ.";
                return ret;
            }
            string idreader_iss = DS.Tables["t"].Rows[0]["IDREADER"].ToString();
            if (idreader_iss != idreader_ord)
            {
                ret = "Выдано читателю № " + idreader_iss;
            }

        }
        Con.Dispose();
        Status.Dispose();
        return ret;
    }

    void FillISSUED(DataTable t)
    {
        tISSUED.BorderStyle = BorderStyle.Solid;
        tISSUED.BorderWidth = 2;
        tISSUED.BorderColor = Color.Black;
        tISSUED.GridLines = GridLines.Both;
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
        tc.Text = "Инвентарный номер";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Фонд";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Дата выдачи";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Назначение";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Срок возврата";
        tr.Cells.Add(tc);

        tr.BackColor = Color.LightGray;
        tISSUED.Rows.Add(tr);
        t.Columns["iss"].DataType = typeof(DateTime);
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
            tc.Text = row["inv"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["fund"].ToString();
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = ((DateTime)row["iss"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = row["dest"].ToString();
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = ((DateTime)row["dret"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);


            tISSUED.Rows.Add(tr);
        }
    }
    void FillISSUED_HST(DataTable t)
    {
        tISSUED_HST.BorderStyle = BorderStyle.Solid;
        tISSUED_HST.BorderWidth = 2;
        tISSUED_HST.BorderColor = Color.Black;
        tISSUED_HST.GridLines = GridLines.Both;
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
        tc.Text = "Инвентарный номер";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Фонд";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Дата выдачи";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Дата возврата";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tr.BackColor = Color.LightGray;
        tISSUED_HST.Rows.Add(tr);
        //t.Columns["vzv"].DataType = typeof(DateTime);
        t.Columns["iss"].DataType = typeof(DateTime);
        //t.Columns["fct"].DataType = typeof(DateTime);
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
            tc.Text = row["inv"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = row["fund"].ToString();
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["iss"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = ((DateTime)row["DATE_RET"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);

            tISSUED_HST.Rows.Add(tr);
        }
    }
    void FillEBOOK(DataTable t)
    {
        //if (t.Rows.Count == 0)
        //    Label2.Visible = true;
        //else
        Label2.Visible = false;
        tEBook.Rows.Clear();
        tEBook.BorderStyle = BorderStyle.Solid;
        tEBook.BorderWidth = 2;
        tEBook.BorderColor = Color.Black;
        tEBook.GridLines = GridLines.Both;
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
        tc.Text = "Дата возврата";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Ссылка на просмотр";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Сдача книги";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tr.BackColor = Color.LightGray;
        tEBook.Rows.Add(tr);
        //t.Columns["vzv"].DataType = typeof(DateTime);
        t.Columns["iss"].DataType = typeof(DateTime);
        //t.Columns["fct"].DataType = typeof(DateTime);
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
            tc.Text = ((DateTime)row["dret"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            tc = new TableCell();
            //tc.Text = "<a href=\"http://80.250.173.145/viewer.aspx?pin=" + row["idm"].ToString() + "&idbase=1&idr=" + row["idr"].ToString()
            //    + "&vkey=" + row["vkey"].ToString() + "\">Просмотр</a>";
            //string HostName = System.Environment.MachineName;
            string HostName = HttpContext.Current.Server.MachineName;
            //string HostName1 = Page.Server.MachineName;
            string ElBookViewerServer = "";
            if (HostName == "VGBIL-OPAC")
            {
                ElBookViewerServer = ConfigurationManager.AppSettings["ExternalElectronicBookViewer"];
            }
            else
            {
                ElBookViewerServer = ConfigurationManager.AppSettings["IndoorElectronicBookViewer"];
            }

            tc.Text = "<a href=\""+ElBookViewerServer+"?pin=" + row["idm"].ToString() + "&idbase=1&idr=" + row["idr"].ToString() +"&type="+row["rtype"]
                + "&vkey=" + HttpUtility.UrlEncode(row["vkey"].ToString()) + "\" Target = \"_blank\">Просмотр</a>";
            //tc.Text += "      " + HostName + " "+HostName1+" " + ElBookViewerServer;
            tr.Cells.Add(tc);
            tc = new TableCell();
            LinkButton del3 = new LinkButton();
            del3.Text = "Сдать";
            del3.ID = "eldel"+row["id"].ToString();
            del3.ForeColor = Color.Red;
            del3.Click += new EventHandler(del3_Click);
            tc.Controls.Add(del3);
            tr.Cells.Add(tc);

            tEBook.Rows.Add(tr);
        }
    }
    private void FillEBOOK_HST(DataTable t)
    {
        tEBook_HST.Rows.Clear();
        tEBook_HST.BorderStyle = BorderStyle.Solid;
        tEBook_HST.BorderWidth = 2;
        tEBook_HST.BorderColor = Color.Black;
        tEBook_HST.GridLines = GridLines.Both;
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
        tc.Text = "Дата возврата";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tc.Text = "Поместить в корзину для повторного заказа";
        tr.Cells.Add(tc);
        tc = new TableCell();
        tr.BackColor = Color.LightGray;
        tEBook_HST.Rows.Add(tr);
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
            tc.Text = ((DateTime)row["dret"]).ToString("dd.MM.yyyy");
            tr.Cells.Add(tc);
            tc = new TableCell();
            LinkButton elret = new LinkButton();
            elret.Text = "Поместить в корзину";
            elret.ID = "elret" + row["id"].ToString();
            elret.Click += new EventHandler(elret_Click);
            tc.Controls.Add(elret);
            tr.Cells.Add(tc);

            tEBook_HST.Rows.Add(tr);
        }
    }

    void elret_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        SqlConnection Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        Con.Open();
        sdvig.SelectCommand = new SqlCommand("select * from Reservation_R..ELISSUED_HST " +
         "where ID = " + ((LinkButton)sender).ID.Substring(5), Con);
        sdvig.Fill(DS, "t");
        DataRow r = DS.Tables["t"].Rows[0];

        sdvig.InsertCommand = new SqlCommand("insert into Reservation_O..Basket " +
            " (IDREADER,IDMAIN,R_TYPE) values (" + r["IDREADER"].ToString() + "," + r["IDMAIN"].ToString() + "," + r["R_TYPE"].ToString() + ")", Con);
        sdvig.InsertCommand.ExecuteNonQuery();

        Con.Close();
        Con.Dispose();
        sdvig.Dispose();
        ScriptManager.RegisterClientScriptBlock(this, typeof(string), "elordered",
            "<script language=\"javascript\" type=\"text/javascript\">alert('Возвращено в корзину!')</SCRIPT>", false);
        Session.Clear();
        SelectedDates = null;
        SelectedInvs = null;
        CheckedIndex = -1;

        BuildBasketTable(BasketTable);
    }
    void del3_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        SqlConnection Con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        Con.Open();
        //SqlTransaction tr = Con.BeginTransaction();
        //try
        //{
            sdvig.SelectCommand = new SqlCommand("select * from Reservation_R..ELISSUED " +
                "where ID = " + ((LinkButton)sender).ID.Substring(5), Con);
            sdvig.Fill(DS, "t");
            DataRow r = DS.Tables["t"].Rows[0];
            sdvig.InsertCommand = new SqlCommand("insert into Reservation_R..ELISSUED_HST " +
                " (IDMAIN,IDREADER,DATEISSUE,DATERETURN,BASE,R_TYPE) values (" + r["IDMAIN"].ToString() + "," + r["IDREADER"].ToString() + ",'" + r["DATEISSUE"].ToString()
                + "',getdate(),1," + r["R_TYPE"].ToString() + ")", Con);
            sdvig.InsertCommand.ExecuteNonQuery();

            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_R..AGREEMENT where IDMAIN = " + r["IDMAIN"].ToString() +
                                                   " and R_TYPE = "+r["R_TYPE"]+" and IDREADER = " + r["IDREADER"] +" and BASE = 1", Con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_R..ELISSUED where ID = " + ((LinkButton)sender).ID.Substring(5), Con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            Con.Close();
            //tr.Commit();
            Con.Dispose();
            sdvig.Dispose();
        //}
        //catch (Exception ex)
        //{
         //   tr.Rollback();
        //}
        FillEBOOK(GetEBOOKTable());
    }
    private bool temp = false;
    //int at =-1 ;
    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        //if (TabContainer1.ActiveTabIndex == 6)
        //{
        //    at = 6;
        //}
        if ((reader.Type == 1) && (TabContainer1.ActiveTabIndex ==3))
        {
            TabContainer1.ActiveTabIndex = 6;
            //TabContainer1_ActiveTabChanged(sender, e);
            
            return;
        }
        //if (temp == false)
        {
            temp = true;
            if (TabContainer1.ActiveTabIndex == 7)
            {
                if ((reader.Session != string.Empty) && (reader.Session != null))
                    DeleteSession(reader);
                FormsAuthentication.SignOut();
                Response.Redirect("loginemployee.aspx");
            }
            if (TabContainer1.ActiveTabIndex == 3)//Выданные книги
            {
                //tISSUED
            }
            if (TabContainer1.ActiveTabIndex == 4)//История выданных книг
            {

            }
            if (TabContainer1.ActiveTabIndex == 0)
            {
                //Session.Clear();
                //Page_Load(sender, e);
            }
            if (TabContainer1.ActiveTabIndex == 6)
            {
                ShowLitRes();
                
            }
            
        }
        
    }
    private void ShowLitRes()
    {
        Label3.Visible = false;
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        DA.SelectCommand.CommandText = "select * from LITRES..ACCOUNTS where IDREADER = " + reader.ID;
        DataSet ds = new DataSet();
        int j = DA.Fill(ds, "t");
        if (j > 0)
        {
            bLitres.Enabled = false;
            lblLitresLogin.Text = "Номер читательского билета Литрес: " + ds.Tables["t"].Rows[0]["LRLOGIN"].ToString();
            lblLitresPwd.Text = "Пароль для Литрес: " + ds.Tables["t"].Rows[0]["LRPWD"].ToString();

        }
        else
        {
            bLitres.Enabled = true;
            lblLitresLogin.Text = "Номер читательского билета Литрес: <не присвоено>";
            lblLitresPwd.Text = "Пароль для Литрес: <не присвоено>";
        }
    }
    protected void bLitres_Click(object sender, EventArgs e)
    {

        bLitres.Enabled = false;
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        DA.SelectCommand.CommandText = "select top 1 * from LITRES..ACCOUNTS where IDREADER is null or IDREADER = '' order by ID";
        DataSet ds = new DataSet();
        int j = DA.Fill(ds, "t");
        if (j == 0)
        {
            Label3.Visible = true;
        }
        else
        {
            DA = new SqlDataAdapter();
            DA.UpdateCommand = new SqlCommand();
            DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.UpdateCommand.Parameters.Add("idr", SqlDbType.Int);
            DA.UpdateCommand.Parameters.Add("rtype", SqlDbType.Int);
            DA.UpdateCommand.Parameters["idr"].Value = reader.ID;
            DA.UpdateCommand.Parameters["rtype"].Value = reader.Type;
            DA.UpdateCommand.Parameters.AddWithValue("ID", ds.Tables["t"].Rows[0]["ID"].ToString());
            DA.UpdateCommand.Parameters.AddWithValue("assigned", DateTime.Now);
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.CommandText = "update LITRES..ACCOUNTS set IDREADER = @idr, RTYPE = @rtype,ASSIGNED=@ASSIGNED where ID = @ID";
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();

            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.SelectCommand.CommandText = "select * from LITRES..ACCOUNTS where IDREADER = " + reader.ID;
            ds = new DataSet();
            j = DA.Fill(ds, "t");
            lblLitresLogin.Text = "Номер читательского билета Литрес: " + ds.Tables["t"].Rows[0]["LRLOGIN"].ToString();
            lblLitresPwd.Text = "Пароль для Литрес: " + ds.Tables["t"].Rows[0]["LRPWD"].ToString();


        }

    }
    private void DeleteSession(Reader reader)
    {
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.DeleteCommand = new SqlCommand();
        DA.DeleteCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        DA.DeleteCommand.Parameters.Add("sess", SqlDbType.NVarChar);
        DA.DeleteCommand.Parameters.Add("idr", SqlDbType.Int);
        DA.DeleteCommand.Parameters.Add("rtype", SqlDbType.Int);
        DA.DeleteCommand.Parameters["sess"].Value = reader.Session;
        DA.DeleteCommand.Parameters["idr"].Value = reader.ID;
        DA.DeleteCommand.Parameters["rtype"].Value = reader.Type;
        DA.DeleteCommand.Connection.Open();
        DA.DeleteCommand.CommandText = "delete from Reservation_O..USERSESSION where IDREADER = @idr and SESSION = @sess and R_TYPE = @rtype";
        int i = DA.DeleteCommand.ExecuteNonQuery();
        DA.DeleteCommand.Connection.Close();

    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        int i = -1;
        DateTime SelectedDate;
        InvOfBook OrderingInv;
        DateTime FstBsDt = new DateTime();
        List<DateTime> BusyDates;
        foreach (CheckBox chb in Checkboxes)
        {
            i++;
            if (chb == null)
            {
                continue;
            }
            if (!chb.Checked)
            {
                continue;
            }
            SelectedDate = DateTime.Parse(CalendarTexts[i].Text);
            OrderingInv = GetInv(BooksForTableNew[i].InvsOfBook, ComboInv[i].SelectedItem.Text);
            BusyDates = OrderingInv.GetBusyDates();
            
            if (BusyDates.Count == 0)
            {
                //заказать и продолжить цикл
            }
            else
            {
                FstBsDt = BusyDates[0];
            }
            TimeSpan DaysBetween = FstBsDt - SelectedDate;
            if (BooksForTableNew[i].IsHundredBooksOrdered(OrderingInv, int.Parse(reader.ID)))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "isinv",
                    "<script language=\"javascript\" type=\"text/javascript\">alert('Невозможно заказать экземпляр с инвентарным номером " + OrderingInv.inv +
                    ", т.к. Вы пытаетесь заказать более 100 экземпляров!')</SCRIPT>",
                    false);
                //System.Web.HttpContext.Current.Response.Write(
                //    "<SCRIPT LANGUAGE=\"JavaScript\">alert('Невозможно заказать экземпляр с инвентарным номером '"+OrderingInv.inv+
                //    "', т.к. Вы пытаетесь заказать более 10 экземпляров из одного сектора!')</SCRIPT>");
                continue;
            }
            string Limitations = OrderingInv.GetLimitation(int.Parse(reader.ID), reader.Type);
            if (Limitations != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "isinv",
                                    "<script language=\"javascript\" type=\"text/javascript\">alert('Невозможно заказать этот экземпляр. " + Limitations+"')</SCRIPT>",
                                    false);
                continue;
            }
            if (OrderingInv.inv.Contains("Электронная"))
            {
                if ((DaysBetween.Days <= 30) && (DaysBetween.Days >= 0))
                {

                    BooksForTableNew[i].Ord(OrderingInv, DaysBetween.Days, SelectedDate, int.Parse(reader.ID),reader.Type);
                }
                else
                {
                    BooksForTableNew[i].Ord(OrderingInv, 30, SelectedDate, int.Parse(reader.ID), reader.Type);
                }
            }
            else
            {
                if (!OrderingInv.mhr.Contains("Абонемент"))
                {
                    switch (DaysBetween.Days)
                    {
                        case 0:
                            {
                                System.Windows.Forms.MessageBox.Show("Ошибка!!! обратитесь к разработчику! " + OrderingInv.inv);
                                break;
                            }
                        case 1:
                            {
                                BooksForTableNew[i].Ord(OrderingInv, 1, SelectedDate, int.Parse(reader.ID), reader.Type);
                                break;
                            }
                        case 2:
                            {
                                BooksForTableNew[i].Ord(OrderingInv, 2, SelectedDate, int.Parse(reader.ID), reader.Type);
                                break;
                            }
                        case 3:
                            {
                                BooksForTableNew[i].Ord(OrderingInv, 3, SelectedDate, int.Parse(reader.ID), reader.Type);
                                break;
                            }
                        default:
                            {
                                BooksForTableNew[i].Ord(OrderingInv, 4, SelectedDate, int.Parse(reader.ID), reader.Type);
                                break;
                            }
                    }
                }
                else
                {
                    if ((DaysBetween.Days <= 30) && (DaysBetween.Days >= 0))
                    {
                        BooksForTableNew[i].Ord(OrderingInv, DaysBetween.Days, SelectedDate, int.Parse(reader.ID), reader.Type);
                    }
                    else
                    {
                        BooksForTableNew[i].Ord(OrderingInv, 30, SelectedDate, int.Parse(reader.ID), reader.Type);
                    }
                }
            }
            if (!OrderingInv.inv.Contains("Электронная"))
            {
                if (!OrderingInv.IS_SAME_INV_WITH_ANOTHER_NOTE())
                    BooksForTableNew[i].delFromBasket(reader.ID,reader.Type);
            }
            else
            {
                BooksForTableNew[i].delFromBasket(reader.ID, reader.Type);
                ScriptManager.RegisterClientScriptBlock(this, typeof(string), "elordered",
    "<script language=\"javascript\" type=\"text/javascript\">alert('Электронная копия заказана!Для просмотра перейдите на вкладку \"Электронные книги\"!')</SCRIPT>",
    false);

            }

        }
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        if (Session["SelectedDates"] != null)
            Session.Remove("SelectedDates");
        if (Session["CheckedIndex"] != null)
            Session.Remove("CheckedIndex");
        BuildBasketTable(BasketTable);
        BuildOrdersTable(OrdersTable);

    }


    InvOfBook GetInv(List<InvOfBook> invs, string inv)
    {
        InvOfBook ret = new InvOfBook();
        foreach (InvOfBook i in invs)
        {
            if (i.note != null)
            {
                if (i.inv + " " + i.note == inv)
                {
                    ret = i;
                    break;
                }
            }
            else
            {
                if (i.inv == inv)
                {
                    ret = i;
                    break;
                }
            }
        }
        return ret;
    }


    
}