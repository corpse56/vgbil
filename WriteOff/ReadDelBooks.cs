using System;
using System.Collections.Generic;
using System.Text;
using BookClasses;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
//using System.Xml.Linq;
namespace WriteOff
{
    public class ReadDelBooks
    {
        private SqlDataAdapter da_ = null;
        private Book book_;
        private string Base;
        //private List<Book> listOfBooks_;
        private DataSet ds_;
        public ReadDelBooks(string base_)
        {
            this.Base = base_;
            string connstr;

            try
            {
                connstr = XmlConnections.GetConnection("/Connections/BJVVV");
            }
            catch //(Exception e)
            {
                //MessageBox.Show(e.Message + "Программа заканчивает свою работу!");
                throw;
            }
            da_ = new SqlDataAdapter();
            da_.SelectCommand = new SqlCommand();
            da_.SelectCommand.Connection = new SqlConnection(connstr);
            da_.SelectCommand.Parameters.Add("year1", SqlDbType.DateTime);
            da_.SelectCommand.Parameters.Add("year2", SqlDbType.DateTime);
            da_.SelectCommand.Parameters["year1"].Value = DateTime.Now;
            da_.SelectCommand.Parameters["year2"].Value = DateTime.Now;

            ds_ = new DataSet();
        }
        public List<string> GetDelAtcsByYear(DateTime year)
        {
            DateTime d1 = new DateTime(year.Year, 1, 1);
            DateTime d2 = new DateTime(year.Year, 12, 31);
            da_.SelectCommand.Parameters["year1"].Value = d1;
            da_.SelectCommand.Parameters["year2"].Value = d2;
            da_.SelectCommand.CommandText = "select distinct B.PLAIN from " + this.Base + "..AFDEL A inner join " + this.Base + "..AFDELVAR B on A.ID=B.IDAF where DateCreate between @year1 and @year2";
            DataSet ds = new DataSet();
            da_.Fill(ds);
            List<string> l_ = new List<string>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l_.Add(r[0].ToString());
            }
            return l_;
        }
        private bool isExistIn(string id, List<string> Ids)
        {
            foreach (string a in Ids) //пропускаем инвентари, которые относятся к другим списанным актам
            {
                if (id == a)
                    return true;
            }
            return false;
        }
        public List<Book> GetBooksOnAct(string act)
        {
            this.da_.SelectCommand.CommandText = "with f as (select A.IDMAIN,MNFIELD,MSFIELD from "+this.Base+"..DATAEXT A " +
                                                 "join " + this.Base + "..DATAEXTPLAIN B on A.ID=B.IDDATAEXT " +
                                                 "where (A.MSFIELD = '$b' " +
                                                 "AND A.MNFIELD = 929 " +
                                                 "and B.PLAIN = '" + act + "')) " +
                                                 "SELECT distinct FF.IDMAIN,A.SORT,dtp.PLAIN,lang.NAME,A.MNFIELD,A.MSFIELD,A.IDDATA,B.SORT fnd,D.SORT mhr,E.SORT act FROM " + this.Base + "..DATAEXT A " +
                                                 "JOIN f AS FF on A.IDMAIN = FF.IDMAIN " +
                                                 "JOIN " + this.Base + "..DATAEXTPLAIN dtp on dtp.IDDATAEXT = A.ID " +
                                                 "left join " + this.Base + "..LIST_1 as lang on dtp.PLAIN = lang.SHORTNAME " +
                                                 "left join " + this.Base + "..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$b' " +
                                                 "left join " + this.Base + "..DATAEXT D on D.IDDATA = A.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                                                 "left join " + this.Base + "..DATAEXT E on E.IDDATA = A.IDDATA and E.MNFIELD = 929 and E.MSFIELD = '$b' " +
                                                 "where 	(  (A.MSFIELD = '$p' " +
                                                 "         AND A.MNFIELD = 899) " +
                                                 "          OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 700) " +
                                                 "          OR  (A.MSFIELD = '$w' " +
                                                 "             AND A.MNFIELD = 899) " +
                                                 "         OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 200) " +
                                                 "         OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 701) " +
                                                 "         OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 210) " +
                                                 "         OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 101) " +
                                                 "         OR  (A.MSFIELD = '$j' " +
                                                 "             AND A.MNFIELD = 899) " +
                                                 "         OR  (A.MSFIELD = '$x' " +
                                                 "             AND A.MNFIELD = 899) " +
                                                 "         OR  (A.MSFIELD = '$d' " +
                                                 "             AND A.MNFIELD = 710) " +
                                                 "         OR  (A.MSFIELD = '$a' " +
                                                 "             AND A.MNFIELD = 500) " +
                                                 "         OR  (A.MSFIELD = '$d' " +
                                                 "             AND A.MNFIELD = 2100) " +
                                                 "         OR  (A.MSFIELD = '$c' " +
                                                 "             AND A.MNFIELD = 922) " +
                                                 "         OR  (A.MSFIELD = '$d' " +
                                                 "             AND A.MNFIELD = 922) " +
                                                 "         OR	(A.MSFIELD = '$b' " +
                                                 "             AND A.MNFIELD = 929)) ORDER BY FF.IDMAIN,A.MSFIELD";
            this.ds_.Tables.Clear();
            da_.Fill(this.ds_);
            this.book_ = new Book();//временный объект для накопления в цикле инфы о книге
            List<string> anotherActs = new List<string>();
            if (ds_.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            string idm = ds_.Tables[0].Rows[0]["IDMAIN"].ToString();
            foreach (DataRow row in ds_.Tables[0].Rows) //ищем другие акты в текущей книге
            {
                if (idm != row["IDMAIN"].ToString())
                    break;
                if ((row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "929$b") && (row["PLAIN"].ToString() != act))
                {
                    anotherActs.Add(row["IDDATA"].ToString());
                }
            }

            List<Book> Rlist = new List<Book>();//список книг списанных по акту
            idm = ds_.Tables[0].Rows[0]["IDMAIN"].ToString();
            foreach (DataRow r in ds_.Tables[0].Rows)
            {
                if (r["IDMAIN"].ToString() == "1285608")
                {
                    int ff = 1;
                }
                if (idm != r["IDMAIN"].ToString()) 
                {
                    Rlist.Add(this.book_);
                    int fred = 1;
                    if (this.book_.Title == "Osszes versei. 2 [E.Ady]")
                    {
                        fred++;
                    }


                    anotherActs.Clear();
                    string idmtmp = r["IDMAIN"].ToString();
                    foreach (DataRow ro in ds_.Tables[0].Rows) //ищем другие акты в текущей книге
                    {
                        if (idmtmp != ro["IDMAIN"].ToString())
                        {
                            continue;
                        }
                        if ((ro["MNFIELD"].ToString() + ro["MSFIELD"].ToString() == "929$b") && (ro["PLAIN"].ToString() != act))
                        {
                            anotherActs.Add(ro["IDDATA"].ToString());
                        }
                    }
                    this.book_ = new Book();
                }
                switch (r["MNFIELD"].ToString() + r["MSFIELD"].ToString())
                {
                    case "899$p": //КАК УЗНАТЬ, ЧТО ИНВЕНТАРЬ ОФ ИЛИ АБОНЕМЕНТ?
                        {
                            if (!isExistIn(r["IDDATA"].ToString(), anotherActs))
                            {
                                if ((r["fnd"].ToString() == "Абонемент") || (r["mhr"].ToString() == "ЦДДАбонемент") || (r["act"].ToString().IndexOf("АБ") != -1))
                                {
                                    this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), false, "A"));
                                }
                                else
                                    if ((r["fnd"].ToString() == "Фондредкойкниги") || (r["mhr"].ToString() == "КнигохранениеФондредкойкниги") || (r["act"].ToString().IndexOf("R") != -1))
                                    {
                                        this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), false, "R"));
                                    }
                                    else
                                    {
                                        this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), false, "O"));
                                    }
                                this.book_.AddPrice(r["IDDATA"].ToString(), ds_.Tables[0]);
                                
                            }
                            break;
                        }
                    
                    case "899$w":
                        {
                            if (this.Base == "BRIT_SOVET")
                                this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), true, "BR"));
                            break;
                        }
                    case "929$b":
                        {
                            if (!isExistIn(r["IDDATA"].ToString(), anotherActs))
                            {
                                if ((r["fnd"].ToString() == "Абонемент") || (r["mhr"].ToString() == "ЦДДАбонемент") || (r["act"].ToString().IndexOf("АБ") != -1))
                                {
                                    this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), true, "A"));
                                }
                                else
                                    if ((r["fnd"].ToString() == "Фондредкойкниги") || (r["mhr"].ToString() == "КнигохранениеФондредкойкниги") || (r["act"].ToString().IndexOf("R") != -1))
                                    {
                                        this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), true, "R"));
                                    }
                                    else
                                    {
                                        this.book_.AddAccessionNum(new AccessionNumber(r["PLAIN"].ToString(), string.Empty, r["IDDATA"].ToString(), true, "O"));
                                    }
                            }
                            break;
                        }
                    case "899$x":
                        {
                            if (!isExistIn(r["IDDATA"].ToString(), anotherActs))
                            {
                                if ((r["fnd"].ToString() == "Абонемент") || (r["mhr"].ToString() == "ЦДДАбонемент") || (r["act"].ToString().IndexOf("АБ") != -1))
                                {
                                    this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, r["PLAIN"].ToString(), r["IDDATA"].ToString(), false, "A"));
                                }
                                else
                                    if ((r["fnd"].ToString() == "Фондредкойкниги") || (r["mhr"].ToString() == "КнигохранениеФондредкойкниги") || (r["act"].ToString().IndexOf("R") != -1))
                                    {
                                        this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, r["PLAIN"].ToString(), r["IDDATA"].ToString(), false, "R"));
                                    }
                                    else
                                    {
                                        this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, r["PLAIN"].ToString(), r["IDDATA"].ToString(), false, "O"));
                                    }
                            }
                            break;
                        }
                    case "922$c":
                        {
                            int Price = -1;
                            if (!int.TryParse(r["PLAIN"].ToString(),out Price))
                            {
                                break;
                            }
                            if (!isExistIn(r["IDDATA"].ToString(), anotherActs))
                            {
                                if ((r["fnd"].ToString() == "Абонемент") || (r["mhr"].ToString() == "ЦДДАбонемент") || (r["act"].ToString().IndexOf("АБ") != -1))
                                {
                                    this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, string.Empty, r["IDDATA"].ToString(), false, "A"));
                                }
                                else
                                    if ((r["fnd"].ToString() == "Фондредкойкниги") || (r["mhr"].ToString() == "КнигохранениеФондредкойкниги") || (r["act"].ToString().IndexOf("R") != -1))
                                    {
                                        this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, r["PLAIN"].ToString(), r["IDDATA"].ToString(), false, "R"));
                                    }
                                    else
                                    {
                                        this.book_.AddAccessionLabel(new AccessionNumber(string.Empty, r["PLAIN"].ToString(), r["IDDATA"].ToString(), false, "O"));
                                    }
                            }
                            break;
                        }
                    case "700$a":
                        {
                            this.book_.AuthorSrt = r["SORT"].ToString();
                            this.book_.Author = (this.book_.Author != null) ? r["PLAIN"].ToString() + " " + this.book_.Author + ";" : r["PLAIN"].ToString() + ";";
                            break;
                        }
                    case "701$a":
                        {
                            this.book_.Author = (this.book_.Author != null) ? this.book_.Author + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                            break;
                        }
                    case "710$a":
                        {
                            this.book_.AuthorSrt = r["SORT"].ToString();
                            this.book_.Author = (this.book_.Author != null) ? this.book_.Author + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                            break;
                        }
                    case "500$a":
                        {
                            this.book_.AuthorSrt = r["SORT"].ToString();
                            this.book_.Author = (this.book_.Author != null) ? this.book_.Author + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                            break;
                        }
                    case "200$a":
                        {
                            this.book_.TitleSrt = r["SORT"].ToString();
                            this.book_.Title = r["PLAIN"].ToString();
                            break;
                        }
                    case "210$a":
                        {
                            this.book_.PlaceOfPublish = (this.book_.PlaceOfPublish != null) ? this.book_.PlaceOfPublish + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                            break;
                        }
                    case "899$j":
                        {
                            if (this.Base == "BRIT_SOVET")
                            {
                                this.book_.ReferenceNumberOF = (this.book_.ReferenceNumberOF != null) ? this.book_.ReferenceNumberOF + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                            }
                            if (r["fnd"].ToString() == "ОФ")
                            {
                                if (this.book_.ReferenceNumberOF != null)
                                {
                                    if (this.book_.ReferenceNumberOF.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberOF = (this.book_.ReferenceNumberOF != null) ? this.book_.ReferenceNumberOF + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberOF = r["PLAIN"].ToString() + ";";
                                }
                            }
                            if (r["fnd"].ToString() == "Абонемент")
                            {
                                if (this.book_.ReferenceNumberAB != null)
                                {
                                    if (this.book_.ReferenceNumberAB.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberAB = (this.book_.ReferenceNumberAB != null) ? this.book_.ReferenceNumberAB + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberAB = r["PLAIN"].ToString() + ";";
                                }
                            }
                            if (r["fnd"].ToString() == "КнигохранениеФондредкойкниги")
                            {
                                if (this.book_.ReferenceNumberR != null)
                                {
                                    if (this.book_.ReferenceNumberR.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberR = (this.book_.ReferenceNumberR != null) ? this.book_.ReferenceNumberR + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberR = r["PLAIN"].ToString() + ";";
                                }
                            }
                            if (r["act"].ToString().IndexOf("ОФ") != -1)
                            {
                                if (this.book_.ReferenceNumberOF != null)
                                {
                                    if (this.book_.ReferenceNumberOF.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberOF = (this.book_.ReferenceNumberOF != null) ? this.book_.ReferenceNumberOF + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberOF = r["PLAIN"].ToString() + ";";
                                }
                            }
                            if (r["act"].ToString().IndexOf("АБ") != -1)
                            {
                                if (this.book_.ReferenceNumberAB != null)
                                {
                                    if (this.book_.ReferenceNumberAB.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberAB = (this.book_.ReferenceNumberAB != null) ? this.book_.ReferenceNumberAB + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberAB = r["PLAIN"].ToString() + ";";
                                }
                            }
                            if (r["act"].ToString().IndexOf("R") != -1)
                            {
                                if (this.book_.ReferenceNumberR != null)
                                {
                                    if (this.book_.ReferenceNumberR.IndexOf(r["PLAIN"].ToString()) == -1)
                                        this.book_.ReferenceNumberR = (this.book_.ReferenceNumberR != null) ? this.book_.ReferenceNumberR + " " + r["PLAIN"].ToString() + ";" : r["PLAIN"].ToString() + ";";
                                }
                                else
                                {
                                    this.book_.ReferenceNumberR = r["PLAIN"].ToString() + ";";
                                }
                            }
                            
                            break;
                        }
                    case "101$a":
                        {
                            this.book_.Language = (this.book_.Language != null) ? this.book_.Language + " " + r["NAME"].ToString() + " язык" + ";" : r["NAME"].ToString() + " язык" + ";";
                            break;
                        }
                    case "2100$d":
                        {
                            this.book_.YearOfPublish = (this.book_.YearOfPublish != null) ? this.book_.YearOfPublish + "; " + r["PLAIN"].ToString() : r["PLAIN"].ToString() + ";";
                            break;
                        }

                }                
                idm = r["IDMAIN"].ToString();
            }
            Rlist.Add(this.book_);
            foreach (Book b in Rlist)
            {
                if (b.Author != null)
                    b.Author = b.Author.Remove(b.Author.Length - 1);
                if (b.ReferenceNumberOF != null)
                    b.ReferenceNumberOF = b.ReferenceNumberOF.Remove(b.ReferenceNumberOF.Length - 1);
                if (b.PlaceOfPublish != null)
                    b.PlaceOfPublish = b.PlaceOfPublish.Remove(b.PlaceOfPublish.Length - 1);
                if (b.YearOfPublish != null)
                    b.YearOfPublish = b.YearOfPublish.Remove(b.YearOfPublish.Length - 1);
                if (b.Language != null)
                    b.Language = b.Language.Remove(b.Language.Length - 1);
                else
                {
                    b.Language = "НЕ ЗАПОЛНЕНО ПОЛЕ ЯЗЫК";
                }
                if ((b.accNums_.Count == 2) && ((b.accNums_[0].AccessionNum == "") || (b.accNums_[1].AccessionNum == "")))//это глюк базы. IDDATA могут не совпадать. такое возможно придется дописать и для тех книг у которых несколько инвентарей
                {
                    if (b.accNums_[0].IsWriteOff)
                    {
                        b.accNums_[1].IsWriteOff = true;
                        b.accNums_.Remove(b.accNums_[0]);
                    }
                    else
                    {
                        b.accNums_[0].IsWriteOff = true;
                        b.accNums_.Remove(b.accNums_[1]);
                    }
                    MessageBox.Show("Ошибка bibliojet! перепишите инвентарный номер:  " +b.accNums_[0].AccessionNum+" и обратитесь в отдел автоматизации.");
                }
                foreach (AccessionNumber a in b.accNums_)
                {
                    if ((a.AccessionNum == "") || (a.AccessionNum == null))
                    {
                        MessageBox.Show("Ошибка bibliojet! перепишите инвентарный номер:  " +a.AccessionNum+" и ID блока (IDDATA): "+a.IDDATA+" и обратитесь в отдел автоматизации. Акт будет составлен неверно.");
                    }
                }
            }
            return Rlist;
        }
        public List<string> GetDepartments()
        {
            da_.SelectCommand.CommandText = "select LIST_8.NAME from LIST_8";
            ds_.Tables.Clear();
            da_.Fill(ds_);
            List<string> l = new List<string>();
            foreach (DataRow r in ds_.Tables[0].Rows)
            {
                l.Add(r["NAME"].ToString());
            }
            return l;
        }
        public string GetInvsCountOnAct(string act)
        {
            da_.SelectCommand.CommandText = "select A.PLAIN from " + this.Base + "..AFDELVAR B " +
                                            "join " + this.Base + "..AFDELDATA A on A.IDAF = B.IDAF " +
                                            "where B.PLAIN = '"+act+"' and A.IDFIELD = 3";
            ds_.Tables.Clear();
            da_.Fill(ds_);
            return (ds_.Tables[0].Rows.Count > 0) ? ds_.Tables[0].Rows[0]["PLAIN"].ToString() : "Количество списываемых инвентарей не указано";
        }
        public string GetCause(string act)
        {
            da_.SelectCommand.CommandText = "select A.PLAIN from " + this.Base + "..AFDELVAR B " +
                                            "join " + this.Base + "..AFDELDATA A on A.IDAF = B.IDAF " +
                                            "where B.PLAIN = '" + act + "' and A.IDFIELD = 4";
            ds_.Tables.Clear();
            da_.Fill(ds_);
            return (ds_.Tables[0].Rows.Count>0) ? ds_.Tables[0].Rows[0]["PLAIN"].ToString() : "Причина не указана";
        }
        public string GetTransferDirection(string act)
        {
            da_.SelectCommand.CommandText = "select A.PLAIN from " + this.Base + "..AFDELVAR B " +
                                            "join " + this.Base + "..AFDELDATA A on A.IDAF = B.IDAF " +
                                            "where B.PLAIN = '" + act + "' and A.IDFIELD = 5";
            ds_.Tables.Clear();
            da_.Fill(ds_);
            return (ds_.Tables[0].Rows.Count > 0) ? ds_.Tables[0].Rows[0]["PLAIN"].ToString() : "Направление передачи не указано";
        }

    }
}
