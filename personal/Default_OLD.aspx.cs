using Wintellect.PowerCollections;
using System;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.IO.Ports;
using System.IO;
using System.Xml;
using InvOfBookForOrder;
using BookForOrder;
using Itenso.Rtf;
using Itenso.Rtf.Support;

public partial class _Default : System.Web.UI.Page
{
    public SqlConnection ZCon;
    public SqlConnection BJCon;
    private List<int> SelectedInvs;
    public SqlConnection TchCon;
    public class ReaderLib
    {
        public ReaderLib(string login, string sess)
        {
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("LOGIN", login.ToLower());
            DA.SelectCommand.CommandText = "select USERS.ID id,USERS.NAME name,USERS.LOGIN login,dpt.NAME dname from BJVVV..USERS join BJVVV..LIST_8 dpt on USERS.DEPT = dpt.ID where lower([LOGIN]) = @LOGIN";
            
            DataSet DS = new DataSet();
            int recc = DA.Fill(DS, "Employee");
            this.ID = DS.Tables["Employee"].Rows[0]["id"].ToString();
            this.Login = DS.Tables["Employee"].Rows[0]["login"].ToString();
            this.Name = DS.Tables["Employee"].Rows[0]["name"].ToString();
            this.Dep = DS.Tables["Employee"].Rows[0]["dname"].ToString();
            this.Session = sess;
            //DA.SelectCommand.Connection.Close();
        }
        public string Name;
        public string Dep;
        public string ID;
        public string Login;
        public string Session;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.User.Identity.IsAuthenticated)
        {
            
            //FormsAuthentication.SignOut();
            
            //FormsAuthentication.RedirectToLoginPage();
            //Response.Redirect("loginemployee.aspx");
        }

        ToolkitScriptManager1.RegisterAsyncPostBackControl(Button1);
        if (BuildTable1())
        {
            Button1.Enabled = true;
            Button2.Enabled = true;
        }
        else
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
        }

        BuildTable2();


    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ZCon = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
        BJCon = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        TchCon = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
        
        TabContainer1.ActiveTabIndex = 0;
        TabContainer1.Style["overflow"] = "auto";
        CurReader = new ReaderLib(this.User.Identity.Name, Request["id"]);
        string ip = Server.MachineName;
        Book.InsertIntoBasketE(CurReader.Session,CurReader.ID,ip);
        if (Page.IsPostBack)
        {
            Button1.Enabled = false;
        }
        if (!Page.IsPostBack)
        {
            Session.Clear();
        }

        DABasket = new SqlDataAdapter();
        DABasket.DeleteCommand = new SqlCommand();
        DABasket.DeleteCommand.Connection = ZCon;
        ZCon.Open();
        DABasket.DeleteCommand.CommandText = "delete A from Reservation_E..Basket A, Reservation_E..Basket B WHERE (A.ID > B.ID) AND (A.IDMAIN=B.IDMAIN) and A.IDREADER=B.IDREADER";
        int i = DABasket.DeleteCommand.ExecuteNonQuery();
        //System.Windows.Forms.MessageBox.Show(i.ToString());
        ZCon.Close();
        if (Session["SelectedList"] != null)
        {
            SelectedInvs = (List<int>)Session["SelectedList"];
        }
        else
        {
            SelectedInvs = new List<int>();
        }

    }

    public ReaderLib CurReader;
    private CheckBox[] Checkboxes;
    private DropDownList[] ComboInv;
    private TextBox[] CalendarTexts;
    protected Button[] bs;
    private string script = "";
    private List<Book> BooksForTable;
    private List<Book> BooksForTableNew;
    private String[] Clntid;
    private DataSet DSetBasket = new DataSet();
    private SqlDataAdapter DABasket = new SqlDataAdapter();
    private OleDbDataAdapter OleDA = new OleDbDataAdapter();
  /*  public class Book : IEnumerable
    {
        //private SqlConnection con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        private SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
        private DataSet DS = new DataSet();
        private SqlDataAdapter DA = new SqlDataAdapter();
        
        public Book(string zag, string id, string avt, string idbas)
        {
            this.Name = zag;
            this.Avt = avt;
            this.ID = id;
            this.InvsOfBook = new List<InvOfBook>();
            this.AllInvsOrdered = false;
            this.FoundWithoutOrder = false;
            this.FoundWithoutOrderTsokol = false;
            this.IntersectionOfBusyDates = new List<DateTime>();
            this.IdBasket = idbas;
            this.HallsCnt = 0;
            this.OtkazCnt = 0;
        }
        public string Name;
        public string Avt;
        public string ID;
        public string Halls;
        public int HallsCnt;//для определения того, скока инвентарей и скока из них по залам
        public int OtkazCnt;
        public string IdBasket;
        public List<InvOfBook> InvsOfBook;
        public List<DateTime> IntersectionOfBusyDates;
        public bool AllInvsOrdered;
        public bool FoundWithoutOrder;
        public bool FoundWithoutOrderTsokol;
        public static int CountInvsInBookList(List<Book> lb_)
        {
            int count = 0;
            foreach (Book b in lb_)
            {
                count += b.InvsOfBook.Count;
            }
            return count;
        }
        public bool InvIsInOrder(string p)
        {
            DA.SelectCommand.CommandText = "select * from Reservation_E..Orders where InvNumber = " + p;
            DA.Fill(DS, "Name");
            DA.SelectCommand.Connection.Close();
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);
        }
        public void AddInv(string inv_, bool ord, string mhr_, string klass_, string idmain_)
        {
            this.InvsOfBook.Add(new InvOfBook(inv_, mhr_, klass_, idmain_));
            
        }

        public bool IsAlreadyInOrder(Reader reader)
        {
            DataSet DS = new DataSet();
            SqlConnection con;// = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_E..Orders where ID_Book_EC = " + this.ID + " and ID_Reader = " + reader.ID, con);
            Status.Fill(DS, "Name");
            con.Close();
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);
            //return DS.Tables["Name"].Rows[0][1].ToString();
        }

        public static void InsertIntoBasket(Reader reader)
        {
            DataSet DS = new DataSet();
            SqlConnection con;// = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from TECHNOLOG_VVV..USERLIST where session = '"+reader.Session+"'", con);
            //исправлять и закрывать конекции
            //con.Open();
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            List<string> readbook = new List<string>();
            foreach (DataRow r in DS.Tables["BasketBook"].Rows)
            {
                readbook.Add(r["idbook"].ToString());
            }
            DS = new DataSet();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            sdvig = new SqlDataAdapter("select * from Reservation_E..Basket where ID = 0", con);
            //con.Open();
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            foreach (string rb in readbook)
            {
                DataRow r = DS.Tables["BasketBook"].NewRow();
                r["IDREADER"] = Convert.ToInt64(reader.ID);
                r["IDMAIN"] = Convert.ToInt64(rb);
                DS.Tables["BasketBook"].Rows.Add(r);
            }
            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);
            
            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["BasketBook"]);
            con.Close();

            DS = new DataSet();
            sdvig = new SqlDataAdapter();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from TECHNOLOG_VVV..USERLIST where session = '" + reader.Session + "'", con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();

        }
        public void Ord(InvOfBook _inv, int dur, DateTime date, int idr) //перенос из таблицы корзина в таблицу читатели
        {
            DataSet DS = new DataSet();
            //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from Reservation_E..Orders where ID_Book_EC =" + this.ID, con);
            sdvig.Fill(DS, "Name");
            con.Close();
            DataRow r = DS.Tables["Name"].NewRow();
            r["ID_Reader"] = idr;
            r["ID_Book_EC"] = ID;
            r["ID_Book_CC"] = 0;//че сюда загонять?????пока ноль. это номер книги карточного каталога
            r["Status"] = 0;//изначально статус нулевой
            r["Start_Date"] = date;
            r["Change_Date"] = date;
            r["InvNumber"] = _inv.inv;
            r["Form_Date"] = DateTime.Now;
            r["Duration"] = dur;
            r["Who"] = 0;//кто сменил статус
            r["IDDATA"] = int.Parse(_inv.iddata);
            if (_inv.IsAllig)
            {
                r["ALGIDM"] = _inv.IdmainOfMainAllig;
            }
            DS.Tables["Name"].Rows.Add(r);


            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["Name"]);
        }
        public void delFromBasket(string idr)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter sdvig = new SqlDataAdapter();
            SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where IDMAIN = " + this.ID + " and IDREADER = " + idr, con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
        }


        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


    }*/

    /*public class InvOfBook
    {
        //private SqlConnection con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        private SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
        private DataSet DS = new DataSet();
        private SqlDataAdapter DA = new SqlDataAdapter();
        public TimeSpan DaysAfterOrder;
        public InvOfBook() { } 
        public InvOfBook(string inv_, string idmain_)
        {
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select A.IDMAIN idmall, A.SORT inv,D.PLAIN mhr,E.PLAIN klass from BJVVV..DATAEXT A " +
                                           " left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXT C on A.IDDATA = C.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' " +
                                           " left join BJVVV..DATAEXTPLAIN D on B.ID = D.IDDATAEXT " +
                                           " left join BJVVV..DATAEXTPLAIN E on C.ID = E.IDDATAEXT " +
                                           " where A.IDDATA = " +
                                           "  ( " +
	                                       "     select AA.IDDATA from BJVVV..DATAEXT AA where " +
	                                       "      not exists (select 1 from BJVVV..DATAEXT B where AA.IDDATA=B.IDDATA and B.MNFIELD = 482 and B.MSFIELD = '$a') " +
	                                       "      and " +
	                                       "     (AA.MNFIELD = 899 and AA.MSFIELD = '$p' " +
	                                       "     and AA.SORT = '" + inv_ + "' ) " +
                                           "  ) and A.MNFIELD = 899 and A.MSFIELD = '$p' ";

            int cnt = 0;
            DS.Tables.Clear();
            try
            {
                cnt = DA.Fill(DS, "main");
            }
            catch (Exception ex)
            {
                Exception ex_fwd = new Exception(ex.Message+"Ошибка заполнения аллигата в базе! Обратитесь к дежурному в любом читальном зале и сообщите следующую информацию: Инвентарный номер - " + inv_);
                ex_fwd.Source = "InvOfBook";
                DA.SelectCommand.Connection.Close();
                throw ex_fwd;
            }
            if (cnt == 0)
            {
                Exception ex_fwd = new Exception("Ошибка заполнения аллигата в базе! Обратитесь к дежурному в любом читальном зале и сообщите следующую информацию: Инвентарный номер - " + inv_);
                ex_fwd.Source = "InvOfBook";
                DA.SelectCommand.Connection.Close();
                throw ex_fwd;
            }
            this.inv = inv_;
            this.IDMAIN = idmain_;
            this.klass = DS.Tables["main"].Rows[0]["klass"].ToString();
            this.mhr = DS.Tables["main"].Rows[0]["mhr"].ToString();
            this.IsAllig = true;
            this.IdmainOfMainAllig = DS.Tables["main"].Rows[0]["idmall"].ToString();
            DA.SelectCommand.Connection.Close();
        }
        public InvOfBook(string inv_, string mhr_, string klass_, string idmain_)
        {
            this.inv = inv_;
            this.IDMAIN = idmain_;
            this.mhr = mhr_;
            this.klass = klass_;
            this.Otkaz = "";
            this.Ordered = false;
            this.IsAllig = false;
        }
        public string inv;
        public string IDMAIN;
        public string mhr;
        public DateTime startDate;
        public string Duration;
        public bool Ordered;
        public string klass;
        public string Otkaz;
        public bool ForOrder;
        public bool IsAllig;
        public string IdmainOfMainAllig;
        public bool IsOrderedAsAligat()
        {
            DataSet DS = new DataSet();
            SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$a' and SORT = ";

            return true;
        }

        public bool IsInRrm()
        {
            if (this.mhr.Contains("ИЦ") ||
                        this.mhr.Contains("КОЛИ") ||
                        this.mhr.Contains("НИО религиозной литературы") ||
                        this.mhr.Contains("детской") ||
                        this.mhr.Contains("языкознанию") ||
                        this.mhr.Contains("общий") ||
                        this.mhr.Contains("ОПИ") ||
                        this.mhr.Contains("УЛЦ") ||
                        this.mhr.Contains("ЦВК") ||
                        this.mhr.Contains("ЦМБ") ||
                        this.mhr.Contains("ЦПИ"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<DateTime> GetBusyDates()
        {
            //if (!this.Ordered)
            //{
            //    return new List<DateTime>();
            //}
            SqlDataAdapter da;
            if (this.IsAllig)
            {
                da = new SqlDataAdapter("select * from Reservation_E..Orders where  ALGIDM ='" + this.IdmainOfMainAllig + "'", con);
            }
            else
            {
                da = new SqlDataAdapter("select * from Reservation_E..Orders where InvNumber = '" + this.inv + "'", con);
            }

            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            if (count == 0)
            {
                return new List<DateTime>();
            }
            List<DateTime> l = new List<DateTime>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                
                l.Add((DateTime)r["Start_Date"]);
                switch ((int)r["Duration"])
                {
                    case 0:
                        
                    case 1:
                            break;
                    case 2:
                        {
                            l.Add(((DateTime)r["Start_Date"]).AddDays(1));
                            break;
                        }
                    case 3:
                        {
                            l.Add(((DateTime)r["Start_Date"]).AddDays(1));
                            l.Add(((DateTime)r["Start_Date"]).AddDays(2));
                            break;
                        }
                    case 4:
                        {
                            l.Add(((DateTime)r["Start_Date"]).AddDays(1));
                            l.Add(((DateTime)r["Start_Date"]).AddDays(2));
                            l.Add(((DateTime)r["Start_Date"]).AddDays(3));
                            break;
                        }
                }
            }
            List<DateTime> l1 = new List<DateTime>(l);
            foreach (DateTime dt in l)
            {
                if (dt < DateTime.Today.Date)
                {
                    l1.Remove(dt);
                }
            }

            return l1;
        }
    }*/
    public string GetStatus(string ids,string refu)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_E..Status where ID = " + ids, ZCon);
        Status.Fill(DS, "Name");
        string ret = DS.Tables["Name"].Rows[0][1].ToString();
        if (ret == "Отказ")
        {
            ret += ": " + refu;
        }
        return ret;
    }
    private string GetBibDescr(string s)
    {
        IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc(s);
        string ret = "";
        foreach (IRtfVisual vt in rtfDocument.VisualContent)
        {
            if (vt.Kind == RtfVisualKind.Text)
                ret += ((IRtfVisualText)vt).Text;
        }
        return ret;

    }
    public List<Book> GetBooksForTableNew(DataTable t)
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
            if (r["a482"].ToString() != "")
            {
                //inv = new InvOfBook(r["a482"].ToString(), r["mhran"].ToString(), r["klass"].ToString(), r["idm"].ToString());
                inv = new InvOfBook(r["a482"].ToString(),r["idm"].ToString(),r["a_iddata"].ToString());

            }
            else
            {
                inv = new InvOfBook(r["inv"].ToString(), r["idm"].ToString(), r["iddata"].ToString());

                //inv = new InvOfBook(r["inv"].ToString(), r["mhran"].ToString(), r["klass"].ToString(), r["idm"].ToString());
            }
            if (inv.mhr.Contains("нигохранени"))
            {
                inv.ForOrder = true;
            }
            else
            {
                inv.ForOrder = false;
            }
            if (inv.mhr.Contains("прием"))
            {
                inv.ForOrder = false;
            }
            if (inv.mhr.Contains("ТОД"))
            {
                inv.ForOrder = false;
            }
            if (inv.mhr.Contains("Овальный"))
            {
                inv.ForOrder = false;
            }

            if (inv.mhr.Contains("бонемент"))
            {
                inv.ForOrder = true;
                bookForTable.InvsOfBook.Add(inv);
            }
            else
            {
                bookForTable.InvsOfBook.Add(inv);
            }
        }
        res.Add(bookForTable);
        return res;

    }
    public List<Book> GetBooksForTable(DataTable t)
    {
        long idmainConst;
        long idmain;
        idmainConst = (System.Int64)t.Rows[0]["idm"];
        idmain = idmainConst;
        BooksForTable = new List<Book>();
        List<InvOfBook> InvsForDates = new List<InvOfBook>();
        List<InvOfBook> InvsForTable = new List<InvOfBook>();

        Book bookForTable = new Book(GetBibDescr(t.Rows[0]["rtf"].ToString()), idmain.ToString(), t.Rows[0]["avt"].ToString(),t.Rows[0]["idbas"].ToString());
        InvOfBook InvForTableKN = null;
        InvOfBook InvForTableKNTS = null;

        foreach (DataRow r in t.Rows)
        {
            idmain = (System.Int64)r["idm"];

            if (idmainConst != idmain)
            {
                idmainConst = idmain;
                if (bookForTable == null)
                    continue;
                if (bookForTable.FoundWithoutOrder)
                {
                    InvsForTable.Add(InvForTableKN);
                    bookForTable.InvsOfBook = InvsForTable;
                }
                else
                {
                    if (bookForTable.FoundWithoutOrderTsokol)
                    {
                        InvsForTable.Add(InvForTableKNTS);
                        bookForTable.InvsOfBook = InvsForTable;
                    }
                    else
                    {
                        InvsForDates.AddRange(InvsForTable);
                        bookForTable.InvsOfBook = InvsForDates;
                    }
                }
                BooksForTable.Add(bookForTable);
                InvForTableKNTS = null;
                InvForTableKN = null;
                InvsForDates = new List<InvOfBook>();
                InvsForTable = new List<InvOfBook>();
                bookForTable = new Book(GetBibDescr(r["rtf"].ToString()), r["idm"].ToString(), r["avt"].ToString(),r["idbas"].ToString());
            }
            InvOfBook inv = new InvOfBook(r["inv"].ToString(), r["mhran"].ToString(), r["klass"].ToString(),r["idm"].ToString());
            //int test = inv.mhr.IndexOf("Книгохранение");
            //test = inv.mhr.IndexOf("этаж");
            bookForTable.InvsOfBook.Add(inv);
            if ((inv.mhr.IndexOf("Книгохранение") != -1) && (inv.mhr.IndexOf("этаж") != -1))
            {//этаж имеет преимущестов над цоколем
                inv.ForOrder = true;
                if (bookForTable.FoundWithoutOrder)
                {
                    continue;
                }
                if (inv.Ordered)
                {
                    //запомнить номер и перейти к след книге
                    InvsForDates.Add(inv);
                }
                else
                {
                    //запомнить все для занесения в таблицу.
                    InvForTableKN = inv;
                    bookForTable.FoundWithoutOrder = true;
                }
            }
            else
            {
                if ((inv.mhr.IndexOf("Книгохранение") != -1) && (inv.mhr.IndexOf("цоколь") != -1))
                {//цоколь
                    inv.ForOrder = true;
                    if (bookForTable.FoundWithoutOrder)
                    {
                        continue;
                    }
                    if (bookForTable.FoundWithoutOrderTsokol)
                    {
                        continue;
                    }
                    if (inv.Ordered)
                    {
                        //запомнить номер и перейти к след книге
                        InvsForDates.Add(inv);
                    }
                    else
                    {
                        //запомнить все для занесения в таблицу.
                        InvForTableKNTS = inv;
                        bookForTable.FoundWithoutOrderTsokol = true;
                    }
                }
                else
                {
                    if ((inv.mhr.IndexOf("Информационный") != -1) ||
                        (inv.mhr.IndexOf("Комплексный") != -1) ||
                        (inv.mhr.IndexOf("Религиозной") != -1) ||
                        (inv.mhr.IndexOf("детской") != -1) ||
                        (inv.mhr.IndexOf("языкознанию") != -1) ||
                        (inv.mhr.IndexOf("общий") != -1) ||
                        (inv.mhr.IndexOf("периодических") != -1) ||
                        (inv.mhr.IndexOf("лингвистический") != -1) ||
                        (inv.mhr.IndexOf("восточных") != -1) ||
                        (inv.mhr.IndexOf("ЦМБ") != -1) ||
                        (inv.mhr.IndexOf("правовой") != -1))
                    {
                        inv.ForOrder = false;
                        InvsForTable.Add(inv);
                        bookForTable.Halls += inv.mhr + "; ";
                        bookForTable.HallsCnt++;
                    }
                    else
                    {
                        inv.ForOrder = false;
                        inv.Otkaz = "Книга по техническим причинам временно не может быть выдана. Обратитесь к дежурному чтобы узнать о возможности заказа.";
                        //тут надо типа отказ какой-то писать.
                        InvsForTable.Add(inv);//книга в отделе из которого нельзя выдавать
                        bookForTable.OtkazCnt++;
                    }
                }
            }
        }
        BooksForTable.Add(bookForTable);
        return BooksForTable;
    }


    Dictionary<string, int> JSInv;

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
        JSInv = new Dictionary<string, int>();
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
                    BusyDates = inv.GetBusyDates();
                    map++;
                    try
                    {
                        //JSInv.Add(inv.inv, map);
                        if (inv.note != null)
                            JSInv.Add(inv.inv + " " + inv.note, map);
                        else
                            JSInv.Add(inv.inv, map);

                    }
                    catch (Exception exc)
                    {
                    }
                }
                else
                {
                    continue;
                }

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
        ClientScript.RegisterStartupScript(GetType(), "InitializeCalendars", script);
    }
    public void BuildTopPortion1New()
    {
        Table5.Rows.Clear();
        Table5.Style["left"] = "30px";
        Table5.Style["top"] = "50px";
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        Table5.BorderColor = System.Drawing.Color.Black;
        Table5.BorderWidth = 3;
        cell.ColumnSpan = 6;
        row.Cells.Add(cell);
        row.Cells[0].Text = "<b>КОРЗИНА</b>";
        Table5.Rows.Add(row);
    }
    int GetRowsCntForBook(Book b)
    {
        int cnt = 0;
        int cnthr = 0;
        foreach (InvOfBook inv in b.InvsOfBook)
        {
            if (inv.mhr.Contains("Книгохран"))
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
    public void FillTbl1New(List<Book> BooksForTableNew)
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
                Table5.Rows.Add(row);
                row = new TableRow();
                //row.Style["border-top"] = "solid";
                //row.Style["border-top-width"] = "7px";

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
                cell.Text = (i+1).ToString();
                RowsForBook = GetRowsCntForBook(b);
                cell.RowSpan = RowsForBook+1;
                
                row.Cells.Add(cell);
                cell = new TableCell();
                cell.Text = b.Name;
                cell.ColumnSpan = 4;
                row.Cells.Add(cell);
                Table5.Rows.Add(row);
                tmprow = row;
                List<InvOfBook> InvsOfKn = new List<InvOfBook>();
                foreach (InvOfBook inv in b.InvsOfBook)
                {
                    if (inv.ForOrder == true)
                    {
                        InvsOfKn.Add(inv);
                        continue;
                    }
                    row = new TableRow();
                    booki++;
                    row.BackColor = currow.BackColor;
                    Table5.Rows.Add(row);
                    cell = new TableCell();
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = inv.inv;
                    row.Cells.Add(cell);

                    cell = new TableCell();

                    if (inv.IsInRrm()) //экземпляр в читальном зале? или в производственном отделе?
                    {
                        cell.Text = "Этот экземпляр можно взять без заказа в указанном зале.";
                    }
                    else
                    {
                        cell.Text = "Этот экземпляр по техническим причинам не может быть выдан.";
                    }
                    //if (
                    //в будущем тут надо вставить проверку, а не на руках ли у читателя этот экземпляр. ну это после того как программу для кафедры сделаю
                    /*if (inv.IsOrderedAsAligat())
                    {
                        cell.Text = "Этот экземпляр - приплетен к другой книге и заказан другим читателем";
                    }
                    else
                    { }*/

                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = inv.mhr;
                    row.Cells.Add(cell);
                }
                if (InvsOfKn.Count != 0)
                {
                    row = new TableRow();
                    booki++;
                    //row.Height = new Unit(row.Height.Value + 35);
                    row.BackColor = currow.BackColor;
                    Table5.Rows.Add(row);
                    cell = new TableCell();
                    Checkboxes[i] = new CheckBox();
                    Checkboxes[i].ID = "ch" + i.ToString();
                    cell.Controls.Add(Checkboxes[i]);
                    row.Cells.Add(cell);

                    cell = new TableCell();
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
                            ListItem li = new ListItem(inv.inv + " " + inv.note, inv.iddata);
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
                    cell.Controls.Add(ComboInv[i]);
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    CalendarTexts[i] = new TextBox();
                    CalendarTexts[i].ID = "txt" + i.ToString();
                    DateTime BestDate = GetBestDateForTable(InvsOfKn[0]);
                    //CalendarTexts[i].Text = DateTime.Today.ToString("dd.MM.yyyy");
                    CalendarTexts[i].Text = BestDate.ToString("dd.MM.yyyy");
                    
                    //CalendarTexts[i].ReadOnly = true; // вот из-за этой строки почему-то все переобновлялось как постбек, а не как в апдейтпанель!!!
                    CalendarTexts[i].Enabled = false;
                    CalendarTexts[i].ForeColor = Color.Black;
                    CalendarTexts[i].Width = 70;
                    //CalendarTexts[i].Style["z-index"] = "1";

                    InvOfBook selectedInv = new InvOfBook();
                    foreach (Book bb in BooksForTableNew)
                    {
                        foreach (InvOfBook inv in b.InvsOfBook)
                        {
                            if (ComboInv[i].SelectedItem.Value == inv.iddata)
                                selectedInv = inv;
                        }
                    }


                    bs[i] = new Button();
                    bs[i].ID = "bs" + i.ToString();
                    bs[i].Text = "...";
                    row.Cells.Add(cell);
                    Table5.Rows[booki + i+2].Cells[2].Controls.Add(CalendarTexts[i]);
                    Table5.Rows[booki + i+2].Cells[2].Controls.Add(bs[i]);
                    //bs[i].Attributes.Add("UseSubmitBehavior", "false");
                    bs[i].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + j.ToString() + "]");
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
                    cell.Text = "Книгохранение. ";
                    if (b.IsAlreadyInOrder(selectedInv.inv))
                    {
                        cell.Text = "Книга уже заказана Вами. Вы не можете заказать книгу второй раз.";
                        cell.ForeColor = Color.Red;
                        Checkboxes[i].Visible = false;
                    }
                    string Limitations = selectedInv.GetLimitation(0,2);
                    if (Limitations != "")
                    {
                        cell.Text += Limitations;
                        Checkboxes[i].Enabled = false;
                    }


                    row.Cells.Add(cell);

                }
                cell = new TableCell();
                cell.VerticalAlign = VerticalAlign.Middle;
                LinkButton del = new LinkButton();
                del.Text = "X";
                del.ID = b.IdBasket;
                del.ForeColor = Color.Red;
                cell.Controls.Add(del);
                del.Click += new EventHandler(del_Click);
                del.CommandArgument = i.ToString();
                cell.RowSpan = RowsForBook + 1;
                tmprow.Cells.Add(cell);


            }
            Button1.Attributes.Add("onmousedown", "isDateSelected()");

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
    void SPDays()
    {
        int i = 0;
        foreach (Button b in bs)
        {
            if (b == null)
            {
                i++;
                continue;
            }
            //b.Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + JSInv[ComboInv[i].Text] + "]");
            i++;
        }
    }
    void _Default_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        string s = ddl.ID;
        s = s.Substring(3, s.Length-3);
        int i = int.Parse(s)+1;
        bs[i-1].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + JSInv[ComboInv[i-1].SelectedItem.Text] + "]");

        InvOfBook selectedInv = new InvOfBook();
        foreach(Book b in BooksForTableNew)
        {
            foreach (InvOfBook inv in b.InvsOfBook)
            {
                /*if (ddl.Text == inv.inv)
                    selectedInv = inv;*/
                if (inv.note != null)
                {
                    if (ddl.SelectedItem.Text == inv.inv + " " + inv.note)
                        selectedInv = inv;
                }
                else
                {
                    if (ddl.SelectedItem.Text == inv.inv)
                        selectedInv = inv;
                }
            }
        }
        DateTime BestDate = GetBestDateForTable(selectedInv);
        CalendarTexts[i - 1].Text = BestDate.ToString("dd.MM.yyyy");
        SelectedInvs[i - 1] = ddl.SelectedIndex;
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);
        
        BuildTable1();
    }

    void del_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        ZCon.Open();
        sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where ID = " + ((LinkButton)sender).ID, ZCon);
        sdvig.DeleteCommand.ExecuteNonQuery();
        ZCon.Close();
//        Response.Redirect("default.aspx");
        if (BuildTable1())
        {
            Button1.Enabled = true;
            Button2.Enabled = true;
        }
        else
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
        }

        BuildTable2();
        int rid = int.Parse(((LinkButton)sender).CommandArgument);
        SelectedInvs.RemoveAt(rid);
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);

    }
    void del2_Click(object sender, EventArgs e)
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        ZCon.Open();
        sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Orders where ID = " + ((LinkButton)sender).ID.Substring(3), ZCon);
        sdvig.DeleteCommand.ExecuteNonQuery();
        ZCon.Close();
        if (BuildTable1())
        {
            Button1.Enabled = true;
            Button2.Enabled = true;
        }
        else
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
        }

        BuildTable2();
    }
    public bool BuildTable1()
    {
        
        holder1.Controls.Clear();
        DABasket.SelectCommand = new SqlCommand("select * from Reservation_E..Basket where IDREADER = " + CurReader.ID /*+ HttpContext.Current.User.Identity.Name*/, ZCon);
        DSetBasket = new DataSet();
        int count = DABasket.Fill(DSetBasket, "Basket");
        DABasket.SelectCommand = new SqlCommand("select distinct R.IDMAIN idm, ZAG.PLAIN zag, INV.PLAIN inv, RTF.RTF rtf, " +
                                                "MHRAN.NAME mhran, KLASS.PLAIN klass, AVT.PLAIN AVT, R.ID idbas , DALL.SORT a482,INV.IDDATA iddata, DALL.IDDATA a_iddata " +
                                                "from Reservation_E..Basket R " +
                                                "left join BJVVV..DATAEXT DZAG on R.IDMAIN = DZAG.IDMAIN and DZAG.MNFIELD = 200 and DZAG.MSFIELD = '$a' " +
                                                "left join BJVVV..DATAEXT DAVT on R.IDMAIN = DAVT.IDMAIN and DAVT.MNFIELD = 700 and DAVT.MSFIELD = '$a' " +
                                                "left join BJVVV..DATAEXT DINV on R.IDMAIN = DINV.IDMAIN and DINV.MNFIELD = 899 and DINV.MSFIELD = '$p' " +
                                                "left join BJVVV..DATAEXT DALL on R.IDMAIN = DALL.IDMAIN and DALL.MNFIELD = 482 and DALL.MSFIELD = '$a'  and DALL.IDDATA = DINV.IDDATA " +
                                                "left join BJVVV..DATAEXT DMHRAN on R.IDMAIN = DMHRAN.IDMAIN and DMHRAN.MNFIELD = 899 and DMHRAN.MSFIELD = '$a' and DINV.IDDATA = DMHRAN.IDDATA " +
                                                "left join BJVVV..DATAEXTPLAIN ZAG on DZAG.ID = ZAG.IDDATAEXT " +
                                                "left join BJVVV..RTF RTF on RTF.IDMAIN = R.IDMAIN " +

                                                "left join BJVVV..DATAEXTPLAIN AVT on DAVT.ID = AVT.IDDATAEXT " +
                                                "left JOIN BJVVV..DATAEXTPLAIN INV on DINV.ID = INV.IDDATAEXT " +
                                                "left JOIN BJVVV..DATAEXTPLAIN MHRANshort on DMHRAN.ID = MHRANshort.IDDATAEXT " +
                                                "left join BJVVV..LIST_8 MHRAN on MHRANshort.PLAIN = MHRAN.SHORTNAME " +
                                                "left join BJVVV..DATAEXT DKLASS on INV.IDDATA = DKLASS.IDDATA and DKLASS.MNFIELD = 921 and DKLASS.MSFIELD = '$c'" +
                                                "left join BJVVV..DATAEXTPLAIN KLASS on DKLASS.ID = KLASS.IDDATAEXT " +
                                                "where R.IDREADER = " + CurReader.ID + " and (DALL.SORT is not null or INV.PLAIN is not null) order by idm", ZCon);

        DABasket.SelectCommand.CommandTimeout = 1200;
        int excnt = DABasket.Fill(DSetBasket, "ExactlyBasket");
        ZCon.Close();
        BuildTopPortion1New();

        bool retVal;
        if (excnt != 0)
        {
            //BooksForTable = GetBooksForTable(DSetBasket.Tables["ExactlyBasket"]);//to comment after all wil be done
            //BooksForTable = GetIntersections(BooksForTable);
            BooksForTableNew = GetBooksForTableNew(DSetBasket.Tables["ExactlyBasket"]);
            //FillTable1(BooksForTable);
            FillTbl1New(BooksForTableNew);
            BuildScriptNew(BooksForTableNew);
            SPDays();
            //BuildScript(BooksForTable);
            retVal = true;
        }
        else
        {
            retVal = false;
        }
        Label1.Text = "Личный кабинет сотрудника " + CurReader.Name + " - " + CurReader.Dep;
        return retVal;

    }
    void BuildTable2()
    {
        Table2.GridLines = GridLines.Both;
        /*for (int i = 0; i < Table2.Rows.Count; i++)
            Table2.Rows.RemoveAt(0);*/
        Table2.Rows.Clear();
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        Table2.Rows.Add(row);
        Table2.BorderWidth = 3;
        row.Cells.Add(cell);
        Table2.Rows[0].Cells[0].ColumnSpan = 4;
        Table2.Rows[0].Cells[0].Text = "<b>ЗАКАЗЫ</b>";
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

        Table2.Rows.Add(row);
        DABasket.SelectCommand.CommandText = "select * from Reservation_E..Orders where ID = 0";
        DABasket.Fill(DSetBasket, "Orders");
        DABasket.SelectCommand.Connection.Close();
        //Checking reader = new Checking("450", HttpContext.Current.User.Identity.Name);//"1"); // читатель здесь павен не "1" а такой, который мне передадут ребята их пхп
        DABasket.SelectCommand.CommandText = "select O.*,DTP.PLAIN zag, RTF.RTF rtf, O.ID idord from Reservation_E..Orders O "+
                                             "left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                                             "left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                                             "left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                                             "where  DT.MSFIELD='$a' and DT.MNFIELD=200 and ID_Reader = " + CurReader.ID;//когда с читателем буду делать надо переделать
        if (DSetBasket.Tables["Orders"] != null) DSetBasket.Tables["Orders"].Clear();
        int tst = DABasket.Fill(DSetBasket.Tables["Orders"]);
        DABasket.SelectCommand.Connection.Close();
        for (int i = 0; i < DSetBasket.Tables["Orders"].Rows.Count; i++)
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
            Table2.Rows.Add(row);
            //System.Windows.Forms.RichTextBox rt = new System.Windows.Forms.RichTextBox();
            //rt.Rtf = DSetBasket.Tables["Orders"].Rows[i]["rtf"].ToString();
            
            Table2.Rows[i + 2].Cells[0].Text = GetBibDescr(DSetBasket.Tables["Orders"].Rows[i]["rtf"].ToString());
            Table2.Rows[i + 2].Cells[2].Text = GetStatus(DSetBasket.Tables["Orders"].Rows[i][4].ToString(), DSetBasket.Tables["Orders"].Rows[i]["REFUSUAL"].ToString());
            //Type t = DSetBasket.Tables["Orders"].Rows[i][5].GetType();
            DateTime DT = (DateTime)DSetBasket.Tables["Orders"].Rows[i][5];
            Table2.Rows[i + 2].Cells[1].Text = DT.ToShortDateString().ToString();
            LinkButton del2 = new LinkButton();
            del2.Text = "X";
            del2.ID = "ord" + DSetBasket.Tables["Orders"].Rows[i]["idord"].ToString();
            del2.ForeColor = Color.Red;
            del2.Click += new EventHandler(del2_Click);
            Table2.Rows[i + 2].Cells[3].Controls.Add(del2);
            //del2.OnClientClick = "updatepanel();";
            Button bn = new Button();
            
        }

    } 

    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    Label tst = new Label();
    //    Button tb = new Button();
    //    tb.ID = "tb";
    //    tst.ID = "test";
    //    tb.Style["position"] = "absolute";
    //    tb.Style["left"] = "200px";
    //    tb.Style["z-index"] = "112";
    //    tb.Style["top"] = "600px";
    //    holder1.Controls.Add(tst);
    //    holder1.Controls.Add(tb);
    //    Label tmp = (Label)holder1.FindControl("test");
    //    Button But = (Button)holder1.FindControl("tb");
    //    But.Text = "Dynama";
    //    tst.Text = "Dynayhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhma";
    //    //        UpdatePanel2.ContentTemplateContainer.Controls.Add(But);
    //    //       UpdatePanel2.ContentTemplateContainer.Controls.Add(tb);
    //    ///But.Style = "z-index: 110;left: 165px; position: absolute; top: 400px";
    //    //tb.Click += Button1_Click1;
    //    //But.Click += Button1_Click1;
    //    But.Style["top"] = "600px";
    //    But.Style["position"] = "absolute";
    //    But.Style["left"] = "500px";
    //    But.Style["z-index"] = "112";
    //    // UpdatePanel2.Update();
    //    //   Button2.Click += Button1_Click1;
    //    // UpdatePanel2.Triggers.
    //}
    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        if (TabContainer1.ActiveTabIndex == 2)//выход
        {
            FormsAuthentication.SignOut();
            Response.Redirect("loginemployee.aspx");
        }
        if (TabContainer1.ActiveTabIndex == 1)//история заказов
        {
            Table4.Rows.Clear();
            Table4.Style["left"] = "30px";
            Table4.Style["top"] = "50px";
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Center;
            Table4.BorderColor = System.Drawing.Color.Black;
            Table4.BorderWidth = 3;

            Table4.Rows.Add(row);
            row.Cells.Add(cell);
            Table4.Rows[0].Cells[0].ColumnSpan = 6;
            Table4.Rows[0].Cells[0].Text = "<b>ИСТОРИЯ ЗАКАЗОВ</b>";
            row = new TableRow();
            cell = new TableCell();
            cell.Width = 250;
            cell.HorizontalAlign = HorizontalAlign.Center;
            //cell.ColumnSpan = 2;
            cell.Text = "<b>Название книги</b>";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "<b>Дата заказа</b>";
            cell.Width = 110;
            row.Cells.Add(cell);
            /*cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "<b>Статус</b>";
            cell.Width = 110;
            row.Cells.Add(cell);*/
            Table4.Rows.Add(row);

            //DABasket.SelectCommand.CommandText = "select * from OrdHis where ID = 0";
            //DABasket.Fill(DSetBasket, "Orders");
            //Checking reader = new Checking("450", HttpContext.Current.User.Identity.Name);//"1"); // читатель здесь павен не "1" а такой, который мне передадут ребята их пхп
            DABasket.SelectCommand.CommandText = "select top 500 O.*,DTP.PLAIN zag, RTF.RTF rtf, O.ID idord from Reservation_E..OrdHis O " +
                                                 "left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                                                 "left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                                                 "where  DT.MSFIELD='$a' and DT.MNFIELD=200 and ID_Reader = " + CurReader.ID + " order by O.Start_Date desc";//когда с читателем буду делать надо переделать
            DABasket.SelectCommand.CommandTimeout = 1200;
            if (DSetBasket.Tables["OrdHis"] != null) DSetBasket.Tables["OrdHis"].Clear();

            int tst = DABasket.Fill(DSetBasket,"OrdHis");

            for (int i = 0; i < DSetBasket.Tables["OrdHis"].Rows.Count; i++)
            {
                //Checking ch = new Checking(DSetBasket.Tables["OrdHis"].Rows[i][2].ToString(), reader.GetIDR());
                row = new TableRow();
                cell = new TableCell();
                row.Cells.Add(cell);
                cell = new TableCell();
                row.Cells.Add(cell);
                /*cell = new TableCell();
                row.Cells.Add(cell);
                row.VerticalAlign = VerticalAlign.Middle;*/
                Table4.Rows.Add(row);
                //System.Windows.Forms.RichTextBox rt = new System.Windows.Forms.RichTextBox();
                //rt.Rtf = DSetBasket.Tables["OrdHis"].Rows[i]["rtf"].ToString();

                Table4.Rows[i + 2].Cells[0].Text = GetBibDescr(DSetBasket.Tables["OrdHis"].Rows[i]["rtf"].ToString());
                //Table4.Rows[i + 2].Cells[2].Text = ch.GetStatus(DSetBasket.Tables["OrdHis"].Rows[i][4].ToString());
                //Type t = DSetBasket.Tables["Orders"].Rows[i][5].GetType();
                DateTime DT = (DateTime)DSetBasket.Tables["OrdHis"].Rows[i][5];
                Table4.Rows[i + 2].Cells[1].Text = DT.ToShortDateString();

            }
        }
    }

    protected void load_Click(object sender, EventArgs e)
    {
        //TabContainer1.ActiveTabIndex = 0;
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
            switch (DaysBetween.Days)
            {
                case 0:
                    {
                        System.Windows.Forms.MessageBox.Show("Ошибка!!! обратитесь к разработчику! " + OrderingInv.inv);
                        break;
                    }
                case 1:
                    {
                        //////////////////////////////////////////////////////////////////////////
                        //
                        //
                        //
                        //функцию заказа привязать к инвентарю и сделать виртуальной!
                        //чтобы когда заказываешь не делать кучу условий аллигат это или нет, потому что для аллигата по-другому заказ будет
                        //
                        //
                        //////////////////////////////////////////////////////////////////////////

                        BooksForTableNew[i].OrdE(OrderingInv, 1, SelectedDate, int.Parse(CurReader.ID));
                        break;
                    }
                case 2:
                    {
                        BooksForTableNew[i].OrdE(OrderingInv, 2, SelectedDate, int.Parse(CurReader.ID));
                        break;
                    }
                case 3:
                    {
                        BooksForTableNew[i].OrdE(OrderingInv, 3, SelectedDate, int.Parse(CurReader.ID));
                        break;
                    }
                default:
                    {
                        BooksForTableNew[i].OrdE(OrderingInv, 4, SelectedDate, int.Parse(CurReader.ID));
                        break;
                    }
            }
            BooksForTableNew[i].delFromBasketE(CurReader.ID);
            
        }
        BuildTable1();
        BuildTable2();
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");

    }
    InvOfBook GetInv(List<InvOfBook> invs, string inv)
    {
        /*InvOfBook ret = new InvOfBook();
        foreach (InvOfBook i in invs)
        {
            if (i.inv == inv)
            {
                ret = i;
                break;
            }
        }
        return ret;*/
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
    protected void Button2_Click2(object sender, EventArgs e)
    {
        foreach (CheckBox chb in Checkboxes)
        {
            if (chb == null)
                continue;
            chb.Checked = true;
        }

    }
    static int BiblioDescriptionCompare(KeyValuePair<string, Book> a, KeyValuePair<string, Book> b)
    {
        int keyCompareResult = a.Key.CompareTo(b.Key);
        if (keyCompareResult != 0)
        {
            return keyCompareResult;
        }
        return a.Value.Name.CompareTo(b.Value.Name);
        
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        List<KeyValuePair<string,Book>> spisok = new List<KeyValuePair<string,Book>>();

        foreach (Book b in BooksForTableNew)
        {
            spisok.Add(new KeyValuePair<string, Book>(b.Language, b));
        }
        //spisok.Sort((x, y) => x.Key.CompareTo(y.Key));
        spisok.Sort(BiblioDescriptionCompare);
        Session.Add("spisok", spisok);

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Default2.aspx','_blank')", true);
    }
    protected void Button5_Click(object sender, EventArgs e)//очистить корзину
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        ZCon.Open();
        int i = 0;
        foreach (Book b in BooksForTableNew)
        {
            if (Checkboxes[i] == null)
                continue;
            //if (Checkboxes[i].Checked)//delete all
            {
                sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where ID = " + b.IdBasket, ZCon);
                sdvig.DeleteCommand.ExecuteNonQuery();
            }
            i++;
        }

        ZCon.Close();
        if (BuildTable1())
        {
            Button1.Enabled = true;
            Button2.Enabled = true;
        }
        else
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
        }
        BuildTable2();



        SelectedInvs = new List<int>(BooksForTableNew.Count);
        for (int si = 0; si < BooksForTableNew.Count; si++)
        {
            SelectedInvs.Add(0);
        }
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);
    }
    protected void Button6_Click(object sender, EventArgs e)//удалить выбранные
    {
        DataSet DS = new DataSet();
        SqlDataAdapter sdvig = new SqlDataAdapter();
        ZCon.Open();
        int i = 0;
        foreach (Book b in BooksForTableNew)
        {
            if (Checkboxes[i] == null)
                continue;
            if (Checkboxes[i].Checked)
            {
                sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where ID = " + b.IdBasket, ZCon);
                sdvig.DeleteCommand.ExecuteNonQuery();
            }
            i++;
        }

        ZCon.Close();
        if (BuildTable1())
        {
            Button1.Enabled = true;
            Button2.Enabled = true;
        }
        else
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
        }
        BuildTable2();



        SelectedInvs = new List<int>(BooksForTableNew.Count);
        for (int si = 0; si < BooksForTableNew.Count; si++)
        {
            SelectedInvs.Add(0);
        }
        if (Session["SelectedList"] != null)
            Session.Remove("SelectedList");
        Session.Add("SelectedList", SelectedInvs);


    }
}
