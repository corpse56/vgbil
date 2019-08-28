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
using System.Web.Configuration;
using Utilities;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.BJUsers;

public partial class _Default : System.Web.UI.Page
{
    public SqlConnection ZCon;
    public SqlConnection BJCon;
    public SqlConnection TchCon;

    private SqlDataAdapter DABasket;
    public ReaderLib CurReader;
    private string script = "";

    BJUserInfo bjUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        ZCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["Zakaz"].ConnectionString);
        bjUser = (BJUserInfo)Session["bjUser"];
        if (bjUser == null)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
        CurReader = new ReaderLib(this.User.Identity.Name, Request["id"], bjUser);
        string ip = Server.MachineName;

        if (!Page.IsPostBack)
        {
            //Session.Clear();
            
            TabContainer1.ActiveTabIndex = 0;
            TabContainer1.Style["overflow"] = "auto";
            //переносим из безликой корзины в корзину читателя.
            Book.InsertIntoBasketE(CurReader.Session, CurReader.ID, ip);

            DABasket = new SqlDataAdapter();
            DABasket.DeleteCommand = new SqlCommand();
            DABasket.DeleteCommand.Connection = ZCon;
            ZCon.Open();
            DABasket.DeleteCommand.CommandText = "delete A from Reservation_E..Basket A, Reservation_E..Basket B WHERE (A.ID > B.ID) AND (A.IDMAIN=B.IDMAIN) and A.IDREADER=B.IDREADER";
            //удаляем дубликаты из корзины перед входом
            int i = DABasket.DeleteCommand.ExecuteNonQuery();
            ZCon.Close();
            Label2.Text = CurReader.Name + " Отдел: " + CurReader.Dep;
        }

        ShowBasketTable();

    }

    private void ShowBasketTable()
    {
        DABasket = new SqlDataAdapter();
        DABasket.SelectCommand = new SqlCommand();
        DABasket.SelectCommand.Connection = ZCon;
        DABasket.SelectCommand.CommandText = " with F0 as (" +
                                             " select ID, IDMAIN IDMAIN from Reservation_E..Basket where IDREADER = " + CurReader.ID +
                                             " union all" +
                                             " select 1 ID, ID_Book_EC IDMAIN from Reservation_E..Orders where ID_Reader = " + CurReader.ID +
                                             ") select distinct IDMAIN,ID  from F0 order by ID desc";//сортируем по IDMAIN чтобы все были в одном порядке
        DataTable table = new DataTable();
        int i = DABasket.Fill(table);
        List<BJBookInfo> basket = new List<BJBookInfo>();
        foreach (DataRow r in table.Rows)
        {
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(Convert.ToInt32(r["IDMAIN"]), "BJVVV");
            for (int j = book.Exemplars.Count - 1; j >= 0; j-- )
            {
                if (((BJExemplarInfo)book.Exemplars[j]).Fields["921$c"].ToLower() == "списано")
                {
                    book.Exemplars.RemoveAt(j);
                }
            }
            if (book.Exemplars.Count != 0)
            {
                basket.Add(book);
            }
        }
        DataTable dataSource = ConvertListToDataTable(basket);
        gwBasket.DataSource = dataSource;
        ((BoundField)gwBasket.Columns[0]).DataField = "IDMAIN";
        ((BoundField)gwBasket.Columns[1]).DataField = "num";
        ((BoundField)gwBasket.Columns[2]).DataField = "RTF";
        ((BoundField)gwBasket.Columns[3]).DataField = "title";
        ((BoundField)gwBasket.Columns[4]).DataField = "avtor";
        ((BoundField)gwBasket.Columns[5]).DataField = "inv";
        ((BoundField)gwBasket.Columns[6]).DataField = "metka";
        ((BoundField)gwBasket.Columns[7]).DataField = "status";
        ((BoundField)gwBasket.Columns[10]).DataField = "IDDATA";
        ((BoundField)gwBasket.Columns[11]).DataField = "StatusCode";
        ((BoundField)gwBasket.Columns[12]).DataField = "StatusNameInBase";
        ((BoundField)gwBasket.Columns[13]).DataField = "Rack";
        gwBasket.DataBind();
    }
    DataTable ConvertListToDataTable(List<BJBookInfo> basket)
    {
        // New table.
        DataTable table = new DataTable();
        
        // Get max columns.
        int columns = 12;

        // Add columns.
        for (int i = 0; i < columns; i++)
        {
            table.Columns.Add();
        }
        table.Columns[0].ColumnName = "IDMAIN";
        table.Columns[1].ColumnName = "num";
        table.Columns[2].ColumnName = "RTF";
        table.Columns[3].ColumnName = "title";
        table.Columns[4].ColumnName = "avtor";
        table.Columns[5].ColumnName = "inv";
        table.Columns[6].ColumnName = "metka";
        table.Columns[7].ColumnName = "status";
        table.Columns[8].ColumnName = "IDDATA";
        table.Columns[9].ColumnName = "StatusCode";
        table.Columns[10].ColumnName = "StatusNameInBase";
        table.Columns[11].ColumnName = "Rack";
        // Add rows.
        int j = 0;
        foreach (BJBookInfo book in basket)
        {
            j++;
            foreach (BJExemplarInfo exemplar in book.Exemplars)
            {
                string location = KeyValueMapping.UnifiedLocationAccess.GetValueOrDefault(exemplar.Fields["899$a"].ToString(), "не указано");
                StringBuilder Status = new StringBuilder();
                int StatusCode = 0;
                if ((location == "Книгохранение") || (location == "Книгохранение - Абонемент"))
                {
                    Status.Append("Книга свободна. Для получения нажмите ссылку \"Заказать\". Местонахождение: "+ exemplar.Fields["899$a"].ToString());
                    StatusCode = 1;
                }
                else if (location == "Служебные подразделения")
                {
                    Status.Append("Экземпляр находится в служебном подразделении. Попробуйте заказать позже.");
                    StatusCode = 2;
                }
                else
                {
                    Status.AppendFormat("Книга находится в открытом доступе в зале {0}.", location);
                    StatusCode = 3;
                }
                
                if (exemplar.IsSelfIssuedOrOrderedEmployee(int.Parse(CurReader.ID)))
                {
                    Status = new StringBuilder();
                    Status.Append("Книга уже заказана вами.");
                    StatusCode = 5;
                }
                else if (exemplar.IsIssuedOrOrderedEmployee())
                {
                    Status = new StringBuilder();
                    Status.Append("Книга уже заказана другим сотрудником.");
                    StatusCode = 4;
                }
                else if (false)//(exemplar.IsIssuedToReader())//метод проверки занятости книги идёт по старым базам. это нужно переписать на новую базу Circulation
                {
                    Status = new StringBuilder();
                    Status.Append("Книга заказана/выдана другому читателю.");
                    StatusCode = 6;
                }
                string StatusNameInBase = "";
                if (StatusCode == 5)
                {
                    StatusNameInBase = exemplar.GetEmployeeStatus();
                }
                DataRow row = table.NewRow();
                row["IDMAIN"] = book.ID;
                row["num"] = j;
                row["RTF"] = book.RTF;
                row["title"] = book.Fields["200$a"].ToString();
                row["avtor"] = book.Fields["700$a"].ToString();
                row["inv"] = (exemplar.Fields["899$x"].ToString() ==string.Empty) ? exemplar.Fields["899$p"].ToString() : exemplar.Fields["899$p"].ToString() + "\nметка: "+exemplar.Fields["899$x"].ToString();;
                row["metka"] = exemplar.Fields["899$x"].ToString();
                row["status"] = Status.ToString();//
                row["IDDATA"] = exemplar.IdData;
                row["StatusCode"] = StatusCode;
                row["StatusNameInBase"] = StatusNameInBase;
                row["Rack"] = exemplar.Fields["899$c"].ToString();
                table.Rows.Add(row);
            }
        }

        return table;
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string argument = e.CommandArgument.ToString();
        //e.CommandName;
        object check = e.CommandSource;
        switch (e.CommandName)
        {
            case "del":
                DelFromBasket(CurReader.ID, argument);
                break;
            case "ord":
                int idmain = CreateOrd(CurReader.ID, argument);
                DelFromBasket(CurReader.ID, idmain.ToString());
                break;
        }
        ShowBasketTable();
        
    }
    private void DelFromBasket(string idreader, string idmain)
    {
        SqlCommand command = new SqlCommand();
        ZCon.Open();
        command = new SqlCommand("delete from Reservation_E..Basket where IDREADER = @IDREADER and IDMAIN = @IDMAIN", ZCon);
        command.Parameters.Add("IDREADER", SqlDbType.Int).Value = int.Parse(idreader);
        command.Parameters.Add("IDMAIN", SqlDbType.Int).Value = int.Parse(idmain);
        command.ExecuteNonQuery();
        ZCon.Close();
    }
    private int CreateOrd(string idreader,string iddata)
    {
        BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData(int.Parse(iddata), "BJVVV");
        SqlCommand command = new SqlCommand();
        ZCon.Open();
        command = new SqlCommand("insert into Reservation_E..Orders (ID_Reader, ID_Book_EC, Status, Start_Date, InvNumber,  Form_Date,    IDDATA, ID_Book_CC, Duration, DepId) "+
                                                            "values (@IDREADER, @IDMAIN,     0,      getdate(), @INV, getdate(), @IDDATA          ,  0,          4 ,     @DepId   )", ZCon);
        command.Parameters.Add("IDREADER", SqlDbType.Int).Value = int.Parse(idreader);
        command.Parameters.Add("IDMAIN", SqlDbType.Int).Value = exemplar.IDMAIN;
        command.Parameters.Add("INV", SqlDbType.NVarChar).Value = exemplar.Fields["899$p"].ToString();
        command.Parameters.Add("IDDATA", SqlDbType.Int).Value = exemplar.IdData;
        command.Parameters.Add("DepId", SqlDbType.Int).Value = bjUser.SelectedUserStatus.DepId;
        command.ExecuteNonQuery();
        ZCon.Close();
        return exemplar.IDMAIN;
    }
        
    protected void gwBasket_DataBound(object sender, EventArgs e)
    {

        //мержим строки
        for (int i = gwBasket.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gwBasket.Rows[i];
            GridViewRow previousRow = gwBasket.Rows[i - 1];

            for (int j = 1; j <= 9; j++)//мержим заглавие, автора, бибописание и номер.
            {
                if ((j != 1) && (j != 2) && (j != 3) && (j != 4) && (j != 9))//мержим только эти колонки
                {
                    continue;
                }
                if (row.Cells[0].Text == previousRow.Cells[0].Text)
                {
                    if (previousRow.Cells[j].RowSpan == 0)
                    {
                        if (row.Cells[j].RowSpan == 0)
                        {
                            previousRow.Cells[j].RowSpan += 2;
                        }
                        else
                        {
                            previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                        }
                        row.Cells[j].Visible = false;
                    }
                }
            }
        }
    }
    protected void gwBasket_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == -1) return;
        int StatusCode = int.Parse(e.Row.Cells[11].Text);
        //ExemplarInfo exemplar = ExemplarInfo.GetExemplarByInventoryNumber(e.Row.Cells[5].Text.Substring(0, e.Row.Cells[5].Text.IndexOf("\n")), "BJVVV");
        //if (exemplar.Fields["899$a"].ToString().ToLower().Contains("абонемент") && exemplar.Fields["899$a"].ToString().ToLower().Contains("книгохранен"))
        //{
        //    e.Row.Cells[12].Text = exemplar.Fields["899$a"].ToString();
        //}

        switch (StatusCode)
        {
            case 1://"Нажмите ссылку \"Заказать\""
                break;
            case 2://"Экземпляр находится в службном подразделении. Попробуйте заказать позже."
                e.Row.Cells[8].Text = "";
                break;
            case 3://"Книга находится в открытом доступе в зале {0}.", location);
                e.Row.Cells[8].Text = "";
                break;
            case 4://Книга уже заказана другим сотрудником.
                e.Row.Cells[8].Text = "";
                break;
            case 5://"Книга уже заказана вами."
                e.Row.Cells[8].Text = e.Row.Cells[7].Text;
                e.Row.Cells[7].Text = e.Row.Cells[12].Text;
                e.Row.Cells[9].Text = "Нельзя удалить книгу с активным заказом";
                break;
            case 6://"Книга заказана/выдана другому читателю."
                e.Row.Cells[8].Text = "";
                break;
        }
        
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        DABasket = new SqlDataAdapter();
        DABasket.SelectCommand = new SqlCommand();
        DABasket.SelectCommand.Connection = ZCon;
        DABasket.SelectCommand.CommandText = " with F0 as (" +
                                             " select ID, IDMAIN IDMAIN from Reservation_E..Basket where IDREADER = " + CurReader.ID +
                                             " union all" +
                                             " select 1 ID, ID_Book_EC IDMAIN from Reservation_E..Orders where ID_Reader = " + CurReader.ID +
                                             ") select distinct IDMAIN,ID  from F0 order by ID desc";//сортируем по IDMAIN чтобы все были в одном порядке
        DataTable table = new DataTable();
        int i = DABasket.Fill(table);
        List<Book> Books = new List<Book>();
        foreach (DataRow r in table.Rows)
        {
            BJBookInfo bjbook = BJBookInfo.GetBookInfoByPIN(Convert.ToInt32(r["IDMAIN"]), "BJVVV");
            Book book = new Book(bjbook.Fields["200$a"].ToString(),r["IDMAIN"].ToString(), bjbook.Fields["700$a"].ToString(), "0");
            Books.Add(book);
        }

        List<KeyValuePair<string, Book>> spisok = new List<KeyValuePair<string, Book>>();


        foreach (Book b in Books)
        {
            spisok.Add(new KeyValuePair<string, Book>(b.Language, b));
        }
        spisok.Sort(BiblioDescriptionCompare);
        if (Session["spisok"] == null)
        {
            Session.Add("spisok", spisok);
        }
        else
        {
            Session["spisok"] = spisok;
        }

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Default2.aspx','_blank')", true);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
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

    Dictionary<string, int> JSInv;

    public void FillTbl1New(List<Book> BooksForTableNew)
    {
                    //if (inv.IsInRrm()) //экземпляр в читальном зале? или в производственном отделе?
                    //{
                    //    cell.Text = "Этот экземпляр можно взять без заказа в указанном зале.";
                    //}
                    //else
                    //{
                    //    cell.Text = "Этот экземпляр по техническим причинам не может быть выдан.";
                    //}
                    ////в будущем тут надо вставить проверку, а не на руках ли у читателя этот экземпляр. ну это после того как программу для кафедры сделаю

                    
                    //string Limitations = selectedInv.GetLimitation(0,2);
                    //if (Limitations != "")
                    //{
                    //    cell.Text += Limitations;
                    //    Checkboxes[i].Enabled = false;
                    //}
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
    }
 
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
            DataSet DSetBasket = new DataSet();
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

 
    protected void Button1_Click1(object sender, EventArgs e)
    {


            //    case 2:
            //        {
            //            BooksForTableNew[i].OrdE(OrderingInv, 2, SelectedDate, int.Parse(CurReader.ID));
            //            break;
            //        }

            //BooksForTableNew[i].delFromBasketE(CurReader.ID);
            
      

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
   
    protected void Button5_Click(object sender, EventArgs e)//очистить корзину
    {
        //sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where IDReader = " + b.IdBasket, ZCon);
        //sdvig.DeleteCommand.ExecuteNonQuery();
        SqlCommand command = new SqlCommand();
        ZCon.Open();
        command = new SqlCommand("delete from Reservation_E..Basket where IDREADER = @IDREADER", ZCon);
        command.Parameters.Add("IDREADER", SqlDbType.Int).Value = int.Parse(CurReader.ID);
        command.ExecuteNonQuery();
        ZCon.Close();
        ShowBasketTable();
    }
    protected void Button6_Click(object sender, EventArgs e)//удалить выбранные
    {
        //sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where ID = " + b.IdBasket, ZCon);
        //sdvig.DeleteCommand.ExecuteNonQuery();

    }

}
