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

namespace fg
{
public partial class Build
	{

    public void BuildTable1()
    {

        holder1.Controls.Clear();
        DABasket.SelectCommand = new SqlCommand("select * from Basket where IDREADER = 1" /*+ HttpContext.Current.User.Identity.Name*/, con);
        //con.Open();
        DSetBasket = new DataSet();
        DABasket.Fill(DSetBasket, "Basket");
        
        DABasket.SelectCommand = new SqlCommand("select  R.IDMAIN idm, ZAG.PLAIN zag, INV.PLAIN inv, MHRAN.NAME mhran, KLASS.PLAIN klass from Reservation..Basket R " +
                                                "join BJVVV..DATAEXT DZAG on R.IDMAIN = DZAG.IDMAIN " +
                                                "join BJVVV..DATAEXT DINV on R.IDMAIN = DINV.IDMAIN " +
                                                "join BJVVV..DATAEXT DMHRAN on R.IDMAIN = DMHRAN.IDMAIN " +
                                                "join BJVVV..DATAEXTPLAIN ZAG on DZAG.ID = ZAG.IDDATAEXT " +
                                                "JOIN BJVVV..DATAEXTPLAIN INV on DINV.ID = INV.IDDATAEXT " +
                                                "JOIN BJVVV..DATAEXTPLAIN MHRANshort on DMHRAN.ID = MHRANshort.IDDATAEXT " +
                                                "join BJVVV..LIST_8 MHRAN on MHRANshort.PLAIN = MHRAN.SHORTNAME " +
                                                "join BJVVV..DATAEXT DKLASS on R.IDMAIN = DKLASS.IDMAIN " +
                                                "join BJVVV..DATAEXTPLAIN KLASS on DKLASS.ID = KLASS.IDDATAEXT " +
                                                "where (DZAG.MNFIELD = 200 and DZAG.MSFIELD = '$a' ) " +
                                                "and (DINV.MNFIELD = 899 and DINV.MSFIELD = '$p') " +
                                                "and (DMHRAN.MNFIELD = 899 and DMHRAN.MSFIELD = '$a') " +
                                                "and (DKLASS.MNFIELD = 921 and DKLASS.MSFIELD = '$c') " +
                                                "and DINV.IDDATA	 = DMHRAN.IDDATA and R.IDREADER = 1" /*+ HttpContext.Current.User.Identity.Name*/, con);

        int excnt = DABasket.Fill(DSetBasket, "ExactlyBasket");
        
        long idmainConst = (System.Int64)DSetBasket.Tables["ExactlyBasket"].Rows[0]["idm"];
        long idmain = idmainConst;
        //bool FoundWithoutOrder = false;
        //bool FoundInRH = false;
        //bool FoundWithoutOrderTsokol = false;
        List<Book> BooksForTable = new List<Book>();
        List<InvOfBook> InvsForDates = new List<InvOfBook>();
        List<InvOfBook> InvsForTable = new List<InvOfBook>();
        Book bookForTable = null;
        InvOfBook InvForTableKN = null;
        InvOfBook InvForTableKNTS = null;
        //InvOfBook InvForTableCHZ;

        foreach (DataRow r in DSetBasket.Tables["ExactlyBasket"].Rows)//ЦИКЛ НЕ РАБОЧИЙ НАДО ИСПРАВЛЯТЬ. ПРОСКАКИВАЕТ СТРОКИ ШО ПЕПЕЦ!!!!!!!!!!!
        {
            //Type p = r["idm"].GetType();
            idmain = (System.Int64)r["idm"];///ЦИКЛ НЕ РАБОЧИЙ НАДО ИСПРАВЛЯТЬ. ПРОСКАКИВАЕТ СТРОКИ ШО ПЕПЕЦ!!!!!!!!!!!

            if (idmainConst == idmain)
            {
                InvOfBook inv = new InvOfBook(r["inv"].ToString(), r["mhran"].ToString(), r["klass"].ToString());
                int test = inv.mhr.IndexOf("Кнохранение");
                test = inv.mhr.IndexOf("этаж");
                bookForTable = new Book(r["zag"].ToString());
                bookForTable.InvsOfBook.Add(inv);
                if ((inv.mhr.IndexOf("Книгохранение") != -1) && (inv.mhr.IndexOf("этаж") != -1))
                {//этаж имеет преимущестов над цоколем
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
                        //InvsForTable = new List<InvOfBook>();
                        //InvsForTable.Add(inv);
                        InvForTableKN = inv;
                        bookForTable.FoundWithoutOrder = true;
                    }
                }
                else
                {
                    if ((inv.mhr.IndexOf("Книгохранение") != -1) && (inv.mhr.IndexOf("цоколь") != -1))
                    {//цоколь
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
                            (inv.mhr.IndexOf("общий читальный") != -1) ||
                            (inv.mhr.IndexOf("периодических") != -1) ||
                            (inv.mhr.IndexOf("лингвистический") != -1) ||
                            (inv.mhr.IndexOf("восточных") != -1) ||
                            (inv.mhr.IndexOf("правовой") != -1))
                        {
                            InvsForTable.Add(inv);
                        }
                        else
                        {
                            //книга в отделе из которого нельзя выдавать
                        }

                    }
                }
            }
            else
            {
                idmainConst = idmain;
                /*if ((bookForTable.FoundWithoutOrder) && (!bookForTable.FoundWithoutOrderTsokol))
                {
                    //все инвентари это книги заказаны при месте хранения равно книгохранение. еще будет ЧЗ и цоколь. если из хранения все заказаны, то можно и из цоколя на крайняк
                    bookForTable.InvsOfBook = InvsForDates;
                    bookForTable.AllInvsOrdered = true;
                }
                else
                {
                    bookForTable.InvsOfBook = InvsForTable;
                    bookForTable.AllInvsOrdered = false;
                }*/
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
                //bookForTable = new Book(r["zag"].ToString());
                InvsForDates = new List<InvOfBook>();
                InvsForTable = new List<InvOfBook>();
            }

        }



        DABasket.SelectCommand.CommandText = "select * from Status";
        DABasket.Fill(DSetBasket, "Status");

        DABasket.SelectCommand.CommandText = "select * from Orders";
        DABasket.Fill(DSetBasket, "Orders");

        OleDA.SelectCommand = new OleDbCommand();
        OleDA.SelectCommand.Connection = OleCon;
        OleDA.SelectCommand.CommandText = "select * from MAIN where NumberReader = 1"; //+ HttpContext.Current.User.Identity.Name;// +" and Family = '" + TextBox2.Text + "'";
        DataSet DS = new DataSet();
        int recc = OleDA.Fill(DS, "Surname");
        if (recc == 0)
        {
            OleDA.SelectCommand.CommandText = "select * from MAIN where NumberSC = " + HttpContext.Current.User.Identity.Name;// +" and Family = '" + TextBox2.Text + "'";
            recc = OleDA.Fill(DS, "Surname");
            if (recc == 0)
            {
                throw new Exception("Быть такого не может! " + HttpContext.Current.User.Identity.Name);
            }
        }

        //Label7.Text = ch.GetNameReader("1");

        //SqlDataAdapter NameReader = new SqlDataAdapter("select  r1.FamilyName from Readers..Main as r1 left join Reservation..Basket as r on r.id = r1.NumberReader where r.id = " + "1", con);
        //NameReader.Fill(DS, "Name");
        Label1.Text = "Личный кабинет пользователя " + DS.Tables["Surname"].Rows[0]["FamilyName"].ToString() + " " + DS.Tables["Surname"].Rows[0]["Name"].ToString() + " " + DS.Tables["Surname"].Rows[0]["FatherName"].ToString();

        Checkboxes = new CheckBox[DSetBasket.Tables["Basket"].Rows.Count];
        Clntid = new String[DSetBasket.Tables["Basket"].Rows.Count];
        CalendarTexts = new TextBox[DSetBasket.Tables["Basket"].Rows.Count];
        HF = new HiddenField[DSetBasket.Tables["Basket"].Rows.Count];
        Calendars = new CalendarExtender[DSetBasket.Tables["Basket"].Rows.Count];
        CalendarsOrd = new CalendarExtender[DSetBasket.Tables["Basket"].Rows.Count];
        bs = new Button[DSetBasket.Tables["Basket"].Rows.Count];


        //        for (int i = 0; i < Table1.Rows.Count; i++)
        //         Table1.Rows.RemoveAt(0);
        Table1.Rows.Clear();

        Table1.Style["left"] = "30px";
        Table1.Style["top"] = "50px";
        TableRow row = new TableRow();
        TableCell cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        Table1.BorderColor = System.Drawing.Color.Black;
        Table1.BorderWidth = 3;

        Table1.Rows.Add(row);
        row.Cells.Add(cell);
        Table1.Rows[0].Cells[0].ColumnSpan = 5;
        Table1.Rows[0].Cells[0].Text = "<b>КОРЗИНА</b>";
        row = new TableRow();
        //cell = new TableCell();
        //cell.Width = 400;
        //cell.Text = "";
        //row.Cells.Add(cell);
        cell = new TableCell();
        cell.Width = 250;
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.ColumnSpan = 2;
        cell.Text = "<b>Название книги</b>";
        row.Cells.Add(cell);
        //String f = Table1.Rows[0].Cells[0].Width.ToString();
        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = "<b>Дата заказа</b>";
        cell.Width = 110;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.HorizontalAlign = HorizontalAlign.Center;
        cell.Text = "<b>Местонахождение</b>";
        row.Cells.Add(cell);
        Table1.Rows.Add(row);

        DSetBasket.Tables.Add("Books");
        string scriptTemplate = @"
            <script language=""javascript"" type=""text/javascript"">
            
            var id1 = [{0}];
            var id2 = [{1}];
            var id3 = [{2}];
            var sp = [{3}];
            //var i = 0;
            for (i = 0; i < id1.length; i++){{
         //       var SPECIAL_DAYS = sp[i];
          //      newCal(id1[i], id2[i], i);
            //if (id2[i].value = '') id2[i].value = Date();
            //<%CalendarTexts[i].Text = %>
            }};
                        
            </script>";
        StringBuilder id1 = new StringBuilder();
        StringBuilder id2 = new StringBuilder();
        StringBuilder id3 = new StringBuilder();
        StringBuilder bd = new StringBuilder();
        String[] Month;
        Month = new String[12];

        for (int i = 0; i < DSetBasket.Tables["Basket"].Rows.Count; i++)
        {
            Checking ch = new Checking(DSetBasket.Tables["Basket"].Rows[i][2].ToString(), "1"/*HttpContext.Current.User.Identity.Name*/);//DSetBasket.Tables["Basket"].Rows[i][1].ToString());
            //ch.Ord("1111");
            row = new TableRow();
            Table1.Rows.Add(row);
            cell = new TableCell();

            //cell.Width = Unit.Percentage(40);
            row.Cells.Add(cell);

            cell = new TableCell();
            row.Cells.Add(cell);
            //cell.Width = Unit.Percentage(20);
            cell = new TableCell();
            row.Cells.Add(cell);
            //cell.Width = Unit.Percentage(40);
            cell = new TableCell();
            row.Cells.Add(cell);
            row.VerticalAlign = VerticalAlign.Middle;

            Table1.Rows[i + 2].Cells[1].Text = ch.GetZaglavie();//DSetBasket.Tables["Books"].Rows[0][1].ToString();
            Table1.Rows[i + 2].Cells[1].Width = 240;



            Checkboxes[i] = new CheckBox();

            Checkboxes[i].ID = "ch" + i.ToString();
            Clntid[i] = Checkboxes[i].ClientID;
            //holder1.Controls.Add(Checkboxes[i]);
            Table1.Rows[i + 2].Cells[0].Controls.Add(Checkboxes[i]);

            HF[i] = new HiddenField();
            HF[i].ID = "hf" + i.ToString();
            holder1.Controls.Add(HF[i]);

            CalendarTexts[i] = new TextBox();


            CalendarTexts[i].ID = "сtb" + i.ToString();
            CalendarTexts[i].Text = string.Empty; //DateTime.Today.ToString("dd.MM.yyyy");
            //CalendarTexts[i].ReadOnly = true;
            //CalendarTexts[i].Attributes.Add("onChange", "focus()");
            Table1.Rows[i + 2].Cells[2].Controls.Add(CalendarTexts[i]);
            //CalendarTexts[i].Attributes.Add("onprerender", "");
            CalendarTexts[i].Style["z-index"] = "1";
            CalendarTexts[i].Width = 70;
            //EventArgs ev = new EventArgs();
            CalendarTexts[i].TextChanged += new EventHandler(Default_TextChanged);
            //CalendarTexts[i].TextChanged += new EventHandler(ctb_TextChanged);
            //CalendarTexts[i].TextChanged += new EventHandler(_Default_TextChanged);


            //disable занятые даты
            ArrayList BusDats = ch.DisableBusyDates();
            //Session.Clear();
            for (int h = 0; h < Month.Length; h++)
            {
                Month[h] = "";
            };
            for (int Dat = 0; Dat < BusDats.Count; Dat++)
            {
                DateTime BusDat = (DateTime)BusDats[Dat];
                Month[BusDat.Month - 1] += BusDat.Day.ToString() + ",";

            };
            bd.Append("[");
            for (int h = 0; h < Month.Length; h++)
            {
                if (Month[h].Length != 0) Month[h] = Month[h].Remove(Month[h].Length - 1, 1);
                Month[h] = "[" + Month[h];
                Month[h] += "]";
                bd.Append(Month[h] + ",");

            };
            bd.Remove(bd.Length - 1, 1);
            bd.Append("],");

            //int h = Session.Count;
            /*            bs[i] = new Button();
                        bs[i].ID = "myb"+i.ToString();
                        bs[i].Width = 17;
            
                        bs[i].Text = "...";
                        //bs[i].Attributes.Add("onclick", "newCal(\"" + bs[i].ClientID + "\",\"" + CalendarTexts[i].ClientID + "\",\"" + "ff" + "\");return false;");
                        bs[i].Attributes.Add("onmouseup", "newCal(\"TabContainer1_TabPanel1_" + bs[i].ID + "\",\"TabContainer1_TabPanel1_" + CalendarTexts[i].ID + "\",\"" + "ff" + "\");return false;");
                        //bs[i].Attributes.Add("onclick","return false;");
                        //bs[i].Attributes.Add("onmouseover", "");
                        //holder1.Controls.Add(b);
                        Table1.Rows[i + 2].Cells[2].Controls.Add(bs[i]);*/

            bs[i] = new Button();
            //bs[i].Attributes.Clear();
            bs[i].ID = "bs" + i.ToString();
            bs[i].Text = "...";

            //bs[i].Attributes.Add("type", "button");
            //string s = bs[i].ClientID.ToString();
            Table1.Rows[i + 2].Cells[2].Controls.Add(bs[i]);
            //bs[i].Attributes.Add("onmouseup", "updatePanel();alert('ahh...');return false;");
            //Button1.Attributes.Add("onmouseup", "updatePanel();alert('ahh...');return false;");
            bs[i].Attributes.Add("onmouseover", "SPECIAL_DAYS = sp[" + i.ToString() + "]");
            bs[i].Attributes.Add("UseSubmitBehavior", "false");
            Button1.Attributes.Add("onmousedown", "isDateSelected()");
            //UpdatePanel1.Triggers.Add(bs[i]);
            //bs[i].Attributes["type"] = "button";                                                                                                                                                    butt.type = 'button';//в IE type нельзя менять
            bs[i].Attributes.Add("onclick", "var ct = document.getElementById('" + CalendarTexts[i].ClientID.ToString() + "');var butt = document.getElementById('" + bs[i].ClientID.ToString() + "');apiCal(id1[" + i.ToString() + "],id2[" + i.ToString() + "],ct.value);");
            //HF[i].Value = "g";
            //CalendarTexts[i].Text = CalendarTexts[i].Text;
            //CalendarTexts[i].Text = Session["my"].ToString();
            //bs[i].Attributes.Add("onmouseout", "cl.hide();");
            //Button2.Attributes.Add("onmouseover", "apiCal(" + bs[i].ClientID.ToString() + "," + CalendarTexts[i].ClientID.ToString() + ");");
            //bs[i].Attributes.Add("onload", "newCal(\"" + bs[i].ClientID + "\",\"" + CalendarTexts[i].ClientID + "\",\"" + "ff" + "\");return false;");

            id1.AppendFormat("\"{0}\",", bs[i].ClientID);
            id2.AppendFormat("\"{0}\",", CalendarTexts[i].ClientID);
            id3.AppendFormat("\"{0}\",", Checkboxes[i].ClientID);

            ////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////
            //начало алгоритма проверки возможности выдачи книги на указанное число/////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////

            /* в каком фонде книга? пока муляж не рабочий..
            if (ch.GetWhere(DSetBasket.Tables["Basket"].Rows[i][2].ToString())== "Основной фонд")
            if (ch.GetWhere(DSetBasket.Tables["Basket"].Rows[i][2].ToString())== "Подсобный фонд")
            if (ch.GetWhere(DSetBasket.Tables["Basket"].Rows[i][2].ToString())== "Открытый доступ")
            */

            //здесь проверить в свободном фонде ли? если да - не дать возможность заказать и вывести сообщение

            //проверка не заказана ли уже эта книга этим же читателем?
            if (ch.IsAlreadyInOrder())
            {
                Table1.Rows[i + 2].Cells[3].Text = "Книга уже заказана Вами. Вы не можете заказать книгу второй раз.";
                Checkboxes[i].Enabled = false;

                //bs[i].Enabled = false;
                Table1.Rows[i + 2].BackColor = ColorTranslator.FromHtml("#FFAAAA");
                continue;
            }
            //если есть свободные инвентари - то все ок

            if (ch.GetInv() != "")
            {
                Table1.Rows[i + 2].Cells[3].Text = ch.GetWhere();// "Есть возможность заказать книгу на указанную дату."; писать не статус а местонахождение
                //где то здесь надо запомнить этот свободный инвентарь
                continue;
            }
            //если нет свободных инвентарей
            //CalendarTexts[i].Text = CalendarTexts[i].Text;
            else
                if (ch.GetInv() == "")
                {
                    //есто ли какой нибудь инвентарь не попадающий на указанную дату?
                    //                if (CalendarTexts[i].Text == "") CalendarTexts[i].Text = DateTime.Today.ToString();
                    ch.FreeBestCopy(CalendarTexts[i].Text);
                    if ((ch.GetFreeBestCopy().First.ToString() == "-1") ||
                        (ch.GetFreeBestCopy().First.ToString() == "0" &&
                         ch.GetFreeBestCopy().Second.ToString() == "уже нельзя"))
                    {
                        Table1.Rows[i + 2].Cells[3].Text = ch.GetWhere(); //"Нет возможности заказать книгу на указанную дату.";
                        Checkboxes[i].Enabled = false;
                        //bs[i].Enabled = false;
                        Table1.Rows[i + 2].BackColor = ColorTranslator.FromHtml("#FFAAAA");
                    }
                    if (ch.GetFreeBestCopy().First.ToString() == "0" && ch.GetFreeBestCopy().Second.ToString() != "уже нельзя")
                    {
                        Table1.Rows[i + 2].Cells[3].Text = ch.GetWhere() + "; Книга уже заказана другим читателем, но Вы можете заказать книгу сегодня до его прихода. Для получения книги пройдите в указанный зал такойто.";
                        continue;
                    }
                    if (ch.GetFreeBestCopy().First.ToString() == "1")
                    {
                        Table1.Rows[i + 2].Cells[3].Text = "Есть возможность заказать книгу на указанную дату сроком на 1 день (сегодня).";
                        continue;
                    }
                    if (ch.GetFreeBestCopy().First.ToString() == "2")
                    {
                        Table1.Rows[i + 2].Cells[3].Text = "Есть возможность заказать книгу на указанную дату сроком на 2 дня (включая текущий).";
                        continue;
                    }
                    if (ch.GetFreeBestCopy().First.ToString() == "3")
                    {
                        Table1.Rows[i + 2].Cells[3].Text = "Есть возможность заказать книгу на указанную дату сроком на 3 дня (включая текущий).";
                        continue;
                    }
                    if (ch.GetFreeBestCopy().First.ToString() == "4")
                    {
                        Table1.Rows[i + 2].Cells[3].Text = "Есть возможность заказать книгу на указанную дату.";
                        continue;
                    }

                }

            ////////////////////////////////////////////////////////////////////////////////////////////////
            //конец алгоритма проверки возможности выдачи книги на указанное число/////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////
        }
        if (id1.Length != 0)
            id1.Remove(id1.Length - 1, 1);
        if (id2.Length != 0)
            id2.Remove(id2.Length - 1, 1);
        if (id3.Length != 0)
            id3.Remove(id3.Length - 1, 1);
        if (bd.Length != 0)
            bd.Remove(bd.Length - 1, 1);
        string tmp = "";//исправить баг чтобы даты по всем годам не ходили!!!
        script = string.Format(scriptTemplate, id1, id2, id3, bd, tmp);
        ClientScript.RegisterStartupScript(GetType(), "InitializeCalendars", script);
    }
    }
    public class Checking
    {

        //private int ability;    //0 - неопределено, 1 - есть возможность, -1 - нет возможности
        //private SerialPort serial = new SerialPort();
        private string id;
        private string idr;
        private Pair FreeBestCp;
        public SqlConnection OrderCon = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        public SqlConnection BJCon = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=BJVVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        public string GetBookID()
        {
            return this.id;
        }
        public string GetIDR()
        {
            return this.idr;
        }
        public void delFromBasket(string idm, string idr)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter sdvig = new SqlDataAdapter();
            //new SqlDataAdapter("select * from Basket where IDMAIN = " + idm + " and IDREADER = "+ idr, OrderCon);
            //SqlCommandBuilder cmdb = new SqlCommandBuilder(sdvig);
            //SqlCommand cmd = new SqlCommand("delete from Basket where IDMAIN = "+idm + " and IDREADER = "+ idr);
            sdvig.DeleteCommand = new SqlCommand("delete from Basket where IDMAIN = " + idm + " and IDREADER = " + idr, OrderCon);
            sdvig.DeleteCommand.ExecuteNonQuery();
            //            sdvig.Fill(DS, "Name");

        }
        public void Ord(string _inv, int dur, int _status, DateTime date) //перенос из таблицы корзина в таблицу читатели
        {
            DataSet DS = new DataSet();
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from Orders where ID_Book_EC =" + this.id, OrderCon);
            sdvig.Fill(DS, "Name");

            DataRow r = DS.Tables["Name"].NewRow();
            r["ID_Reader"] = this.idr;
            r["ID_Book_EC"] = this.id;
            r["ID_Book_CC"] = 0;//че сюда загонять?????пока ноль. это номер книги карточного каталога
            r["Status"] = _status;
            r["Start_Date"] = date;
            r["Change_Date"] = date;
            r["InvNumber"] = _inv;
            r["Form_Date"] = DateTime.Today;
            r["Duration"] = dur;
            r["Who"] = 0;
            DS.Tables["Name"].Rows.Add(r);


            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            /*sdvig.InsertCommand = new SqlCommand("insert into orders (ID_Reader,ID_Book_EC,ID_Book_CC,Status,Start_Date,Change_Date,InvNumber,Form_Date,Duration)"+
                                                                             "values("+DS.Tables["Name"].Rows[0][1].ToString()+
                                                                             "," + DS.Tables["Name"].Rows[0][2].ToString()+
                                                                             ",0,"+
                                                                             _status.ToString()+
                                                                             ",GETDATE(),"+
                                                                             "GETDATE(),"+
                                                                             _inv+
                                                                             ",GETDATE(),"+
                                                                             dur.ToString()+")", OrderCon);*/
            //sdvig.InsertCommand.Parameters.AddWithValue("", "");


            sdvig.Update(DS.Tables["Name"]);

            //DS.Tables["Name"].Rows.Add(new DataRow());
            //DS.Tables["Name"].Rows[0].Delete();
            //sdvig.DeleteCommand = new SqlCommand("delete from Basket where id_book = " + this.id, OrderCon);
            //sdvig.Update(DS.Tables["Name"]);//.Select(null, null,DataViewRowState.Deleted));
            //DS.Tables["Name"].AcceptChanges();

            //здесь надо удалять из корзины и в обработчике кнопки перерисовывать первую таблицу
        }
        public Pair GetFreeBestCopy()
        {

            return this.FreeBestCp;
        }
        public Checking(string _ID, string _IDR)
        {
            this.id = _ID;
            this.idr = _IDR;
            //this.ability = 0;
            this.OrderCon.Open();
            this.BJCon.Open();
            this.FreeBestCp = new Pair("", -2);
        }

        public string GetNameReader(string id)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter NameReader = new SqlDataAdapter("select  r1.FamilyName from Readers..Main as r1 left join Reservation..Basket as r on r.id = r1.NumberReader where r.id = " + id, OrderCon);
            NameReader.Fill(DS, "Name");
            return DS.Tables["Name"].Rows[0][0].ToString();
        }
        public string GetZaglavie()
        {

            DataSet DS = new DataSet();
            SqlDataAdapter Zaglavie = new SqlDataAdapter("select dp.PLAIN from BJVVV..DATAEXT as d join BJVVV..DATAEXTPLAIN dp on d.ID=dp.IDDATAEXT where d.MSFIELD = '$a' and d.MNFIELD = '200' and d.IDMAIN = " + this.id, BJCon);
            Zaglavie.Fill(DS, "Name");
            if (DS.Tables["Name"].Rows.Count != 0)
                return DS.Tables["Name"].Rows[0][0].ToString();
            else
                return "нет в базе";
        }
        public string GetStatus(string ids)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Status = new SqlDataAdapter("select * from Status where ID = " + ids, OrderCon);
            Status.Fill(DS, "Name");
            return DS.Tables["Name"].Rows[0][1].ToString();
        }
        public bool IsAlreadyInOrder()
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Status = new SqlDataAdapter("select * from Orders where ID_Book_EC = " + this.id + " and ID_Reader = 1" /*+ this.idr*/, OrderCon);
            Status.Fill(DS, "Name");
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);
            //return DS.Tables["Name"].Rows[0][1].ToString();
        }
        public void FreeBestCopy(string selected)
        {

            DataSet DS = new DataSet();
            SqlDataAdapter Busy = new SqlDataAdapter("select * from Orders where ID_Book_EC = " + this.id, OrderCon);
            Busy.Fill(DS, "Busy");
            int FreeDayCountBefore = -2;
            int FreeDayCountBeforeTmp = 0;
            string invTmp = "";
            string inv = "";
            if (selected == "") selected = DateTime.Today.ToString("dd.MM.yyyy");
            if (DS.Tables["Busy"].Rows.Count == 0)//в таблице заказов нету такой книги, значит можно выдать
            {
                this.FreeBestCp.First = "4";
                this.FreeBestCp.Second = "0";
                return;
                //return new Pair(4, "0");
            }
            for (int i = 0; i < DS.Tables["Busy"].Rows.Count; i++)
                for (int j = 0; j < int.Parse(DS.Tables["Busy"].Rows[i][9].ToString()); j++)
                {
                    if ((DateTime.Parse(DS.Tables["Busy"].Rows[i][5].ToString()).AddDays(int.Parse(DS.Tables["Busy"].Rows[i][9].ToString())) < DateTime.Parse(selected)))
                    {//если дата "на когда хочет взять"+количество дней на которые хочет взять меньше, чем выбранная дата то все в пордяке
                        FreeDayCountBefore = 4;
                        inv = DS.Tables["Busy"].Rows[i][7].ToString();
                        continue;//тут скорее всего можно закончить цикл брейком
                    }
                    if ((DateTime.Parse(DS.Tables["Busy"].Rows[i][5].ToString()).AddDays(j) == DateTime.Parse(selected)))
                    {//если дата "на когда хочет взять" + поочередное прибавление дней равно выбранной дате
                        if (DateTime.Parse(selected) == DateTime.Today)
                        {//если выбранная дата равна сегодняшней 
                            FreeDayCountBeforeTmp = 0;
                            int st = int.Parse(DS.Tables["Busy"].Rows[i][4].ToString());
                            if (st == 3 || st == 7)
                            {
                                invTmp = "уже нельзя";
                            }
                            else
                            {
                                invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                            }

                        }
                        else
                        {
                            FreeDayCountBeforeTmp = -1;
                            invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                        }
                        //здесь запомнить статус. в конце если статус "уже на руках" - пака. если статус еще не на руках - "можете взять до прихода заказазавшего читателя" 
                    }
                    else
                    {
                        //date = DateTime.Parse(selected).AddDays(1);
                        if ((DateTime)DS.Tables["Busy"].Rows[i][5] == DateTime.Parse(selected).AddDays(1))
                        {
                            FreeDayCountBeforeTmp = 1;
                            invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                        }
                        else
                        {
                            //date = DateTime.Parse(selected).AddDays(2);
                            if ((DateTime)DS.Tables["Busy"].Rows[i][5] == DateTime.Parse(selected).AddDays(2))
                            {
                                FreeDayCountBeforeTmp = 2;
                                invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                            }
                            else
                            {
                                //date = DateTime.Parse(selected).AddDays(3);
                                if ((DateTime)DS.Tables["Busy"].Rows[i][5] == DateTime.Parse(selected).AddDays(3))
                                {
                                    FreeDayCountBeforeTmp = 3;
                                    invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                                }
                                else
                                    if ((DateTime)DS.Tables["Busy"].Rows[i][5] >= DateTime.Parse(selected).AddDays(4))
                                    {
                                        FreeDayCountBeforeTmp = 4;
                                        invTmp = DS.Tables["Busy"].Rows[i][7].ToString();
                                    }
                            }
                        }
                    }

                    if (FreeDayCountBefore <= FreeDayCountBeforeTmp)
                    {
                        FreeDayCountBefore = FreeDayCountBeforeTmp;
                        inv = invTmp;
                        FreeDayCountBeforeTmp = -2;
                        invTmp = "";
                    }
                }
            this.FreeBestCp.First = FreeDayCountBefore;
            this.FreeBestCp.Second = inv;
            //return new Pair(FreeDayCountBefore, inv);

        }
        public string GetWhere()
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Where = new SqlDataAdapter("select d.SORT from BJVVV..DATAEXT d where MNFIELD =899 and d.MSFIELD ='$a' and d.IDMAIN = " + this.id, BJCon);
            Where.Fill(DS, "Where");
            return DS.Tables["Where"].Rows[0][0].ToString();
        }
        public string GetInv()
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Free = new SqlDataAdapter("select * from Reservation..Orders where ID_Book_EC = " + this.id, OrderCon);
            Free.Fill(DS, "book_in_order");
            if (DS.Tables["book_in_order"].Rows.Count == 0)
            {
                Free.SelectCommand = new SqlCommand("select d.IDMAIN, d.SORT from BJVVV..DATAEXT as d where d.MNFIELD ='899' and d.MSFIELD ='$p' and d.IDMAIN = " + this.id, BJCon);
                Free.Fill(DS, "pervii_popavshiisa_inv");
                return DS.Tables["pervii_popavshiisa_inv"].Rows[0][1].ToString();
            }

            Free.SelectCommand = new SqlCommand("select distinct  o.InvNumber, d.IDMAIN, o.ID_Book_EC, d.SORT, o.Status from BJVVV..DATAEXT as d"
                                               + " join Reservation..Orders as o "
                                               + " on d.IDMAIN = o.ID_Book_EC and d.SORT <> o.InvNumber "
                                               + " where "
                                               + " o.ID_Book_EC = " + this.id + " and "
                                               + " d.MNFIELD ='899' and d.MSFIELD ='$p'", OrderCon);
            Free.Fill(DS, "Free");
            if (DS.Tables["Free"].Rows.Count == 0) return "";
            for (int i = 0; i < DS.Tables["Free"].Rows.Count; i++)
            {
                if (DS.Tables["Free"].Rows[i][0] == DS.Tables["Free"].Rows[i][3])
                    DS.Tables["Free"].Rows[i].Delete();
            }
            if (DS.Tables["Free"].Rows.Count == 0) return "";
            return DS.Tables["Free"].Rows[0][3].ToString();

        }
        public string GetInvFreeStatus()
        {
            DataSet DS = new DataSet();
            SqlDataAdapter FreeStatus = new SqlDataAdapter("select * from Orders where ID_Book_EC = " + this.id, OrderCon);
            FreeStatus.Fill(DS, "FrStatus");
            for (int i = 1; i < DS.Tables["FrStatus"].Rows.Count; i++)
            {
                if ((int)DS.Tables["FRStatus"].Rows[i][4] != 7 && (int)DS.Tables["FRStatus"].Rows[i][4] != 3)
                    return DS.Tables["FRStatus"].Rows[i][7].ToString();
            }
            return "";
        }
        public ArrayList DisableBusyDates()
        {
            DataSet DS = new DataSet();
            SqlDataAdapter BDates = new SqlDataAdapter("select * from Orders where ID_Book_EC = " + this.id + " and ID_Reader <> 1" /*+ this.idr*/, OrderCon);
            BDates.Fill(DS, "BDates");
            //string BDate = "";
            ArrayList returnDates = new ArrayList();
            //if (this.GetInv() == "") return returnDates;
            //DateTime[] ArrDates = new DateTime[5];
            ArrayList[] retDates;
            retDates = new ArrayList[DS.Tables["BDates"].Rows.Count];

            for (int i = 0; i < DS.Tables["BDates"].Rows.Count; i++)
            {                           //DS.Tables["BDates"].Rows[i][5].ToString()    DS.Tables["BDates"].Rows[i][5].ToString()
                if (this.GetFreeBestCopy().First.ToString() == "0" && this.GetFreeBestCopy().Second.ToString() != "уже нельзя")
                    continue;
                retDates[i] = new ArrayList();
                retDates[i].Add(DateTime.Parse(DS.Tables["BDates"].Rows[i][5].ToString()));
                //retDates.Add(tmp);
                if (DS.Tables["BDates"].Rows[i][9].ToString().Trim() == "2")
                {
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                    //retDates.Add(tmp);
                }
                if (DS.Tables["BDates"].Rows[i][9].ToString() == "3")
                {
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                }
                if (DS.Tables["BDates"].Rows[i][9].ToString() == "4")
                {
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                    retDates[i].Add(DateTime.Parse(retDates[i][retDates[i].Count - 1].ToString()).AddDays(1));
                }

                //if (DS.Tables["BDates"].Rows[0][9] == )
            }
            int ArrCount = 0;
            if (retDates.Length != 0)
                //if (retDates.Length != 1 && retDates.Length != 0)
                for (int i = 0; i < retDates[0].Count; i++)
                {
                    DateTime tmp = DateTime.Parse(retDates[0][i].ToString());
                    ArrCount = 0;
                    for (int j = 1; j < retDates.Length; j++)
                    {
                        for (int k = 0; k < retDates[j].Count; k++)
                        {
                            if (tmp == DateTime.Parse(retDates[j][k].ToString()))
                            {
                                ArrCount++;
                                break;
                            }
                        }
                    }
                    if (ArrCount == retDates.Length - 1) returnDates.Add(tmp);
                }
            return returnDates;

        }


    };

}