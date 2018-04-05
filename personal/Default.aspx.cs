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
using ExportBJ_XML.ValueObjects;
using ExportBJ_XML.classes.BJ;

public partial class _Default : System.Web.UI.Page
{
    public SqlConnection ZCon;
    public SqlConnection BJCon;
    public SqlConnection TchCon;

    private SqlDataAdapter DABasket;
    public ReaderLib CurReader;
    private string script = "";


    protected void Page_Load(object sender, EventArgs e)
    {

        CurReader = new ReaderLib(this.User.Identity.Name, Request["id"]);
        string ip = Server.MachineName;

        if (!Page.IsPostBack)
        {
            Session.Clear();
            TabContainer1.ActiveTabIndex = 0;
            TabContainer1.Style["overflow"] = "auto";
            //переносим из безликой корзины в корзину читателя.
            Book.InsertIntoBasketE(CurReader.Session, CurReader.ID, ip);

            DABasket = new SqlDataAdapter();
            DABasket.DeleteCommand = new SqlCommand();
            ZCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["Zakaz"].ConnectionString);
            DABasket.DeleteCommand.Connection = ZCon;
            ZCon.Open();
            DABasket.DeleteCommand.CommandText = "delete A from Reservation_E..Basket A, Reservation_E..Basket B WHERE (A.ID > B.ID) AND (A.IDMAIN=B.IDMAIN) and A.IDREADER=B.IDREADER";
            //удаляем дубликаты из корзины перед входом
            int i = DABasket.DeleteCommand.ExecuteNonQuery();
            ZCon.Close();
        }

        ShowBasketTable();

    }

    private void ShowBasketTable()
    {
        DABasket = new SqlDataAdapter();
        DABasket.SelectCommand = new SqlCommand();
        ZCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["Zakaz"].ConnectionString);
        DABasket.SelectCommand.Connection = ZCon;
        DABasket.SelectCommand.CommandText = "select * from Reservation_E..Basket where IDREADER = " + CurReader.ID;
        DataTable table = new DataTable();
        int i = DABasket.Fill(table);
        List<BJBookInfo> basket = new List<BJBookInfo>();
        foreach (DataRow r in table.Rows)
        {
            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(Convert.ToInt32(r["IDMAIN"]), "BJVVV");
            basket.Add(book);
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
        gwBasket.DataBind();
    }
    static DataTable ConvertListToDataTable(List<BJBookInfo> basket)
    {
        // New table.
        DataTable table = new DataTable();

        // Get max columns.
        int columns = 9;

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
        // Add rows.
        int j = 0;
        foreach (BJBookInfo book in basket)
        {
            j++;
            foreach (ExemplarInfo exemplar in book.Exemplars)
            {
                DataRow row = table.NewRow();
                row[0] = book.ID;
                row[1] = j;
                row[2] = book.RTF;
                row[3] = book.Fields["200$a"].ToString();
                row[4] = book.Fields["700$a"].ToString();
                row[5] = exemplar.Fields["899$p"].ToString();
                row[6] = exemplar.Fields["899$x"].ToString();
                row[7] = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];
                table.Rows.Add(row);
            }
        }

        return table;
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

                    //if (b.IsAlreadyInOrder(selectedInv.inv))
                    //{
                    //    cell.Text = "Книга уже заказана Вами. Вы не можете заказать книгу второй раз.";
                    //    cell.ForeColor = Color.Red;
                    //    Checkboxes[i].Visible = false;
                    //}
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        //List<KeyValuePair<string,Book>> spisok = new List<KeyValuePair<string,Book>>();

        //foreach (Book b in BooksForTableNew)
        //{
        //    spisok.Add(new KeyValuePair<string, Book>(b.Language, b));
        //}
        //spisok.Sort(BiblioDescriptionCompare);
        //Session.Add("spisok", spisok);

        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('Default2.aspx','_blank')", true);
    }
    protected void Button5_Click(object sender, EventArgs e)//очистить корзину
    {
        //sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where IDReader = " + b.IdBasket, ZCon);
        //sdvig.DeleteCommand.ExecuteNonQuery();

    }
    protected void Button6_Click(object sender, EventArgs e)//удалить выбранные
    {
        //sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where ID = " + b.IdBasket, ZCon);
        //sdvig.DeleteCommand.ExecuteNonQuery();

    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        object check = e.CommandArgument;
        check = e.CommandName;
        check = e.CommandSource;
        
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void gwBasket_DataBound(object sender, EventArgs e)
    {
        for (int i = gwBasket.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gwBasket.Rows[i];
            GridViewRow previousRow = gwBasket.Rows[i - 1];
            //if (previousRow.Cells[0].Text == row.Cells[0].Text)
            //{
            //    if (previousRow.Cells[2].RowSpan == 0)
            //    {
            //        if (row.Cells[2].RowSpan == 0)
            //        {
            //            previousRow.Cells[2].RowSpan += 2;
            //        }
            //        else
            //        {
            //            previousRow.Cells[2].RowSpan = row.Cells[2].RowSpan + 1;
            //        }
            //        row.Cells[2].Visible = false;
            //    }

            //}

            for (int j = 1; j <= 4; j++)
            {
                
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
        //string currentPIN = gwBasket.Rows[0].Cells[0].Text;
        //int currentRowSpan = 0;
        //int rowCounter = 0;
        //int rowStart = 0;
        //foreach (GridViewRow row in gwBasket.Rows)
        //{

        //    if (row.Cells[0].Text != currentPIN)
        //    {
        //        gwBasket.Rows[rowStart].Cells[1].Attributes.Add("rowspan", currentRowSpan.ToString());
        //        //gwBasket.Rows[rowStart].Cells[2].Attributes.Add("rowspan", currentRowSpan.ToString());
        //        //gwBasket.Rows[rowStart].Cells[3].Attributes.Add("rowspan", currentRowSpan.ToString());
        //        currentRowSpan = 1;
        //        rowStart = rowCounter;
        //        currentPIN = row.Cells[0].Text;
        //    }
        //    else
        //    {

        //        currentRowSpan++;
        //    }
        //    rowCounter++;
        //}
        //gwBasket.Rows[3].Cells[2].Text = "111";
        //gwBasket.Rows[3].Cells[2].Attributes.Add("rowspan", "3");
        //gwBasket.Rows[4].Cells[2].Visible = false;
        //gwBasket.Rows[5].Cells[2].Visible = false;

    }
    protected void gwBasket_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex % 4 == 0)
            {
                e.Row.Cells[0].Attributes.Add("rowspan", "4");
            }
            else
            {
                e.Row.Cells[0].Visible = false;
            }
        }

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.RowIndex % 4 == 0)
        //    {
        //        e.Row.Cells[0].Attributes.Add("rowspan", "4");
        //    }
        //    else
        //    {
        //        e.Row.Cells[0].Visible = false;
        //    }
        //}
    }
}
