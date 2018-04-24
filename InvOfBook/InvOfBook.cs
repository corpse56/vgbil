using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XMLConnections;
using BookForOrder;
using System.Collections;
namespace InvOfBookForOrder
{
    public class InvOfBook : IEquatable<InvOfBook>
    {
        //private SqlConnection con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        //private SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
        //private string constr = "Data Source=192.168.4.30;Initial Catalog=Reservation_O;Persist Security Info=True;User ID=EmplOrd;Password=Employee_Order";
        //private SqlConnection con = new SqlConnection("Data Source=192.168.4.30;Initial Catalog=Reservation_O;Persist Security Info=True;User ID=EmplOrd;Password=Employee_Order");
        private SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));

        private DataSet DS = new DataSet();
        private SqlDataAdapter DA = new SqlDataAdapter();
        public TimeSpan DaysAfterOrder;
        public InvOfBook() { }
        public InvOfBook(string http)
        {
            this.http = http;
        }
        public InvOfBook(string inv_, string idmain_, string iddata)
        {
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;

            DA.SelectCommand.CommandText = "select A.SORT bar,D.PLAIN mhr,E.PLAIN klass " +
             "from BJVVV..DATAEXT A " +
             "left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a'  " +
             "left join BJVVV..DATAEXT C on A.IDDATA = C.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c'  " +
             "left join BJVVV..DATAEXTPLAIN D on B.ID = D.IDDATAEXT  " +
             "left join BJVVV..DATAEXTPLAIN E on C.ID = E.IDDATAEXT  " +
             "where " +
             " A.MNFIELD = 899 and A.MSFIELD = '$w' and A.IDDATA = " + iddata;
            DataSet DS3 = new DataSet();
            int cnt3 = DA.Fill(DS3, "main");
            if (cnt3 == 0)//нет штрихкода
            {
                //ex_fwd.Source = "InvOfBook";
                DA.SelectCommand.Connection.Close();
                this.inv = inv_;
                //return;
            }
            else
            {
                this.inv = inv_;
                this.iddata = iddata;
                this.IDMAIN = idmain_;
                this.klass = DS3.Tables["main"].Rows[0]["klass"].ToString();
                this.mhr = DS3.Tables["main"].Rows[0]["mhr"].ToString();
                this.Bar = DS3.Tables["main"].Rows[0]["bar"].ToString();
                this.IsAllig = false;
                this.FillNote(iddata);
                return;
            }

            DA.SelectCommand.CommandText = "select * from BJVVV..DATAEXT A where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.IDDATA = '" + iddata + "' and " +
                   "exists (select 1 from BJVVV..DATAEXT B where A.IDDATA = B.IDDATA and B.MNFIELD = 482 and B.MSFIELD = '$a')";
            int i = DA.Fill(DS);
            if (i == 0)
                this.IsAllig = false;
            else
                this.IsAllig = true;
            if (this.IsAllig)
            {
                DA.SelectCommand.CommandText = "select A.IDMAIN idmall, A.SORT inv,D.PLAIN mhr,E.PLAIN klass,A.IDDATA iddata from BJVVV..DATAEXT A " +
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
                                               "  ) and A.MNFIELD = 899 and A.MSFIELD = '$p' ";//ищем главный аллигат из аллигата

            }
            else
            {
                DA.SelectCommand.CommandText = "select A.IDMAIN idmall, A.SORT inv,D.PLAIN mhr,E.PLAIN klass,A.IDDATA iddata from BJVVV..DATAEXT A " +
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
                                               "     and AA.IDDATA = '" + iddata + "' ) " +
                                               "  ) and A.MNFIELD = 899 and A.MSFIELD = '$p' ";//ищем главный аллигат из инвентаря
            }
            int cnt = 0;
            int cnt2 = 0;
            DataSet DS2 = new DataSet();
            DS.Tables.Clear();
            try
            {
                cnt = DA.Fill(DS, "main");
            }
            catch (Exception ex)//несколько претендентов на главный аллигат. проверять, есть ли штрихкод. если есть то все ок.
            {
                Exception ex_fwd = new Exception(ex.Message + "Ошибка заполнения аллигата в базе! Обратитесь к дежурному в любом читальном зале и сообщите следующую информацию: Инвентарный номер - " + inv_);
                //есть ли штрихкод. если штрихкода нет, то ошибка

                DA.SelectCommand.CommandText = "select A.SORT bar,D.PLAIN mhr,E.PLAIN klass " +
                             "from BJVVV..DATAEXT A " +
                             "left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a'  " +
                             "left join BJVVV..DATAEXT C on A.IDDATA = C.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c'  " +
                             "left join BJVVV..DATAEXTPLAIN D on B.ID = D.IDDATAEXT  " +
                             "left join BJVVV..DATAEXTPLAIN E on C.ID = E.IDDATAEXT  " +
                             "where " +
                             " A.MNFIELD = 899 and A.MSFIELD = '$w' and A.IDDATA = " + iddata;
                DS = new DataSet();
                cnt2 = DA.Fill(DS2, "main");
                if (cnt2 == 0)//нет штрихкода
                {
                    ex_fwd.Source = "InvOfBook";
                    DA.SelectCommand.Connection.Close();
                    this.inv = inv_;
                    this.FillNote(iddata);
                    return;
                }
                else
                {
                    this.inv = inv_;
                    this.iddata = iddata;
                    this.IDMAIN = idmain_;
                    this.klass = DS2.Tables["main"].Rows[0]["klass"].ToString();
                    this.mhr = DS2.Tables["main"].Rows[0]["mhr"].ToString();
                    this.Bar = DS2.Tables["main"].Rows[0]["bar"].ToString();
                    this.FillNote(iddata);
                    return;
                }
                //throw ex_fwd;
            }
            if (cnt == 0)// непонятно какой из аллигатов главный 
            {
                Exception ex_fwd = new Exception("Ошибка заполнения аллигата в базе! Обратитесь к дежурному в любом читальном зале и сообщите следующую информацию: Инвентарный номер - " + inv_);
                ex_fwd.Source = "InvOfBook";
                DA.SelectCommand.Connection.Close();
                this.inv = inv_;
                this.FillNote(iddata);
                return;
                //throw ex_fwd;
            }
            this.inv = inv_;
            this.iddata = DS.Tables["main"].Rows[0]["iddata"].ToString(); 
            this.IDMAIN = idmain_;
            this.klass = DS.Tables["main"].Rows[0]["klass"].ToString();
            this.mhr = DS.Tables["main"].Rows[0]["mhr"].ToString();
            this.IdmainOfMainAllig = DS.Tables["main"].Rows[0]["idmall"].ToString();
            this.FillNote(iddata);

            con.Dispose();
            DA.Dispose();
            //DA.SelectCommand.Connection.Close();
        }
        private void FillNote(string iddata)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select A.PLAIN note " +
             "from BJVVV..DATAEXT B " +
             "left join BJVVV..DATAEXTPLAIN A on B.ID = A.IDDATAEXT  " +
             "where " +
             " B.MNFIELD = 899 and B.MSFIELD = '$x' and B.IDDATA = " + iddata;
            DS = new DataSet();
            int cnt2 = DA.Fill(DS, "note");
            if (cnt2 != 0)
            {
                this.note = DS.Tables["note"].Rows[0]["note"].ToString();
            }
            DA.Dispose();
            con.Dispose();
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
        public string note;
        public string iddata;
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
        public string Bar;
        public string http;
        public bool IsOrderedAsAligat()
        {
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            da = new SqlDataAdapter("select * from Reservation_O..Orders where ALGIDM =" + this.IdmainOfMainAllig, con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..ISSUED where IDMAIN =" + this.IdmainOfMainAllig, con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..ISSUED_OF where IDMAIN_CONST = " + this.IdmainOfMainAllig, con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks where IDMAIN =" + this.IdmainOfMainAllig + " and RETINBK = 0", con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;
            da.Dispose();
            con.Dispose();
            return false;
        }
        public bool IsOrderedAsAligatByCurReader(string idr)
        {
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            da = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Reader = " + idr + " and ALGIDM =" + this.IdmainOfMainAllig, con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Reader = " + idr + " and ID_Book_EC =" + this.IdmainOfMainAllig, con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..ISSUED where IDREADER = " + idr + " and  IDMAIN =" + this.IdmainOfMainAllig, con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..ISSUED_OF where IDREADER = " + idr + " and  IDMAIN_CONST = " + this.IdmainOfMainAllig, con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;

            da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks where  IDREADER = " + idr + " and IDMAIN =" + this.IdmainOfMainAllig + " and RETINBK = 0", con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0) return true;
            da.Dispose();
            con.Dispose();
            return false;
        }
        public bool IsInRrm()
        {
            if (this.mhr.Contains("ИЦ") ||
                        this.mhr.Contains("…Зал… КНИО Группа искусствоведения") ||
                        this.mhr.Contains("…ЗалФ… КНИО Группа религоведения") ||
                        this.mhr.Contains("…Зал… КНИО Группа редкой книги") ||
                        this.mhr.Contains("…ЗалФ… Отдел детской книги и детских программ") ||
                        this.mhr.Contains("…Зал… КОО Группа выдачи документов") ||
                        this.mhr.Contains("…Зал… КОО Группа читального зала 3 этаж") ||
                        //this.mhr.Contains("ОПИ") ||
                        //this.mhr.Contains("УЛЦ") ||
                        //this.mhr.Contains("ЦВК") ||
                        //this.mhr.Contains("ЦМБ") ||
                                        this.mhr.Contains("…Зал… КОО Группа абонементного обслуживания") ||
                                        //this.mhr.Contains("Отдел детской книги") ||
                                        //this.mhr.Contains("Группа международного библиотековедения") ||
                                        this.mhr.Contains("…Зал… КОО Группа выдачи документов") ||
                                        this.mhr.Contains("Читальный зал") ||
                                        this.mhr.Contains("Группа искусствоведения и культурологии") ||
                                        this.mhr.Contains("Группа редкой книги и коллекций") ||
                                        this.mhr.Contains("…ЗалФ… КНИО Группа религоведения") ||
                                        this.mhr.Contains("ЦМБ") ||
                //this.mhr.Contains("Абонемент") ||
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
            if (this.inv != null)
            {
                if (this.inv.Contains("Электронная"))
                {
                    Book tmp = new Book(this.IDMAIN);
                    if (tmp.GetExemplarCount() - tmp.GetBusyExemplarCount() > 0)
                    {
                        return new List<DateTime>();
                    }
                    SqlDataAdapter dae;
                    con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
                    dae = new SqlDataAdapter("select * from Reservation_R..ELISSUED where IDMAIN = " + this.IDMAIN, con);
                    DataSet dse = new DataSet();
                    int c = dae.Fill(dse, "t");
                    if (c == 0)
                    {
                        return new List<DateTime>();
                    }
                    else
                    {
                        List<DateTime> retval = new List<DateTime>();
                        List<DateTime> d = new List<DateTime>();
                        DateTime start = (DateTime)dse.Tables["t"].Rows[0]["DATEISSUE"];
                        DateTime end = (DateTime)dse.Tables["t"].Rows[0]["DATERETURN"];
                        TimeSpan ts = end - start;
                        for (int i = 0; i <= ts.Days; i++)
                        {
                            retval.Add(start.AddDays(i));
                        }


                        foreach (DataRow r in dse.Tables["t"].Rows)
                        {
                            start = (DateTime)r["DATEISSUE"];
                            end = (DateTime)r["DATERETURN"];
                            ts = end - start;
                            for (int i = 0; i <= ts.Days; i++)
                            {
                                d.Add(start.AddDays(i));
                            }
                            retval = retval.Intersect(d.AsEnumerable<DateTime>()).ToList();
                            d = new List<DateTime>();
                        }
                        for (int i = 0; i < retval.Count; i++)
                        {
                            if (retval[i]<DateTime.Today.Date)
                            {
                                retval.Remove(retval[i]);
                                i--;
                            }
                        }
                        return retval;
                    }

                }
            }
            if (this.inv == null)
            {
                return new List<DateTime>();
            }
            //if (!this.Ordered)
            //{
            //    return new List<DateTime>();
            //}
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            if (this.IsAllig)
            {
                da = new SqlDataAdapter("select * from Reservation_O..Orders where  ALGIDM =" + this.IdmainOfMainAllig, con);
            }
            else
            {
                da = new SqlDataAdapter("select * from Reservation_O..Orders where IDDATA = "+this.iddata, con);
            }

            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            if (count == 0)
            {
                List<DateTime> l2 = new List<DateTime>();

                da = new SqlDataAdapter("select * from Reservation_R..SPECIALDATES ", con);
                ds = new DataSet();
                count = da.Fill(ds, "sd");
                if (count != 0)
                {
                    foreach (DataRow r in ds.Tables["sd"].Rows)
                    {
                        l2.Add((DateTime)r["SPDATE"]);
                    }
                }

                return l2;

            }
            List<DateTime> l = new List<DateTime>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {

                l.Add((DateTime)r["Start_Date"]);
                for (int i = 1; i < (int)r["Duration"]; i++)
                {
                    l.Add(((DateTime)r["Start_Date"]).AddDays(i));
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
            da = new SqlDataAdapter("select * from Reservation_R..SPECIALDATES " , con);
            ds = new DataSet();
            count = da.Fill(ds, "sd");
            if (count != 0)
            {
                foreach (DataRow r in ds.Tables["sd"].Rows)
                {
                    l1.Add((DateTime)r["SPDATE"]);
                }
            }

            da.Dispose();
            con.Dispose();
            return l1;

        }

        internal int IsIssued()
        {
            if (this.inv.Contains("Электронная"))
            {
                return 0;
            }
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            /*da = new SqlDataAdapter("select * from Reservation_O..Orders where InvNumber ='" + this.inv + "'", con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            if (count > 0)
                return 1;*/
            int count = 0;
            DataSet ds = new DataSet();

            /*da = new SqlDataAdapter("select * from Reservation_R..ISSUED where IDDATA ='" + this.iddata + "' and IDMAIN != 0  ", con);//абонемент
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0)
                return 1;*/

            if (this.Bar == null)
                da = new SqlDataAdapter("select * from Reservation_R..ISSUED_OF where IDDATA = " +this.iddata, con);
            else
                da = new SqlDataAdapter("select * from Reservation_R..ISSUED_OF where BAR = '"+this.Bar+"' and IDMAIN != 0 ", con);

            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0)
                return 1;

            /*if (this.Bar == null)
                da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks where INV ='" + this.inv + "' and RETINBK = 0  ", con);
            else
                da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks where BAR = '"+this.Bar+"' and INV ='" + this.inv + "' and RETINBK = 0  ", con);
            ds = new DataSet();
            count = da.Fill(ds);
            if (count > 0)
                return 1;*/
            da.Dispose();
            con.Dispose();
            return 0;
        }
        public string GetTimeLimitation(DateTime ODate)
        {
            if (this.inv.Contains("Электронная"))
            {
                return "";
            }
            if (ODate.Date > DateTime.Now.Date.AddDays(7))
            {
                return "Все даты на ближайшую неделю заняты! Попробуйте заказать книгу позже!";
            }
            if (ODate.Date != DateTime.Now.Date)
            {
                return "";
            }
            if ((DateTime.Now.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Now.DayOfWeek == DayOfWeek.Sunday))
            {
                if ((DateTime.Now.Date == new DateTime(2018, 4, 21)))
                {
                    if ((DateTime.Now.Hour >= 19) && (DateTime.Now.Minute >= 50))//библионочь
                    {
                        return "на сегодня приём заказов окончен. Выберите другую дату.";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                if ((DateTime.Now.Hour >= 17) && (DateTime.Now.Minute >= 30))//выходные
                {
                    return "на сегодня приём заказов окончен. Выберите другую дату.";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                if ((DateTime.Now.Hour >= 20) && (DateTime.Now.Minute >= 30))//будни
                {
                    return "на сегодня прием заказов окончен. Выберите другую дату.";
                }
                else
                {
                    return "";
                }
            }

        }
        public string GetLimitationForWSEBookSender()
        {
            Book b = new Book(this.IDMAIN);
            if (b.GetExemplarCount() - b.GetBusyExemplarCount() <= 0)
            {
                return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право." +
                    " Ближайшая свободная дата " + b.GetNearestFreeDate().ToString("dd.MM.yyyy") + ". Попробуйте заказать в указанную дату.";

            }
            return "";
        }
        public string GetLimitation(int idr,int rtype)
        {
            if (this.inv == "Электронная копия")
            {
                if (this.ISExtremistLiterature())
                {
                    return "Данная книга включена в список экстремистской литературы, утвержденным Минюстом РФ (<a href=\"http://minjust.ru/ru\">http://minjust.ru/ru</a>). Выдаче не подлежит";
                }
            }

            if (this.note == "Э")
            {
                return "Данная книга включена в список экстремистской литературы, утвержденным Минюстом РФ (<a href=\"http://minjust.ru/ru\">http://minjust.ru/ru</a>). Выдаче не подлежит";
            }
            if ((!this.inv.Contains("Электронная")) && (rtype == 1))
            {
                return "Вы можете заказать только электронную копию, так как являетесь удалённым читателем!";
            }
            if (this.inv.Contains("Электронная"))
            {
                Book b = new Book(this.IDMAIN);
                if (this.IsFiveElBooksIssued(idr,rtype))
                {
                    return "Нельзя заказать больше 5 электронных книг! Сдайте какие-либо выданные Вам электронные копии на вкладке \"Электронные книги\" и повторите заказ! ";
                }
                if (this.IsELOrderedByCurrentReader(idr,rtype))
                {
                    return "Электронная копия этого документа уже выдана Вам!";
                }
                if (b.GetExemplarCount() - b.GetBusyExemplarCount() <= 0)
                {
                    return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право."+
                        " Ближайшая свободная дата " + b.GetNearestFreeDate().ToString("dd.MM.yyyy")+". Попробуйте заказать в указанную дату.";

                }
                if (!this.IsDayPastAfterReturn(idr,rtype))
                {
                    return "Вы не можете заказать эту электронную копию, поскольку запрещено заказывать ту же копию, если не прошли сутки с момента её возврата. Попробуйте на следующий день.";
                }
                return "";
            }
            else
            {
                if (this.IsInRrm()) //экземпляр в читальном зале? или в производственном отделе?
                {
                    return "Этот экземпляр можно взять без заказа в указанном зале.";
                }
                else
                {
                    if (this.mhr == "error1")
                    {
                        return "Этот инвентарь имеет ошибку заполнения аллигата в базе. Обратитесь к дежурному для заказа.";
                    }
                    if (this.mhr == "Книгохранение - прием")
                    {
                        return "Этот инвентраь по техническим причинам не может быть заказан";
                    }
                    if (this.mhr == "Книгохранение - ТОД")
                    {
                        return "Этот инвентраь по техническим причинам не может быть заказан";
                    }
                    if (this.mhr == "Книгохранение - Овальный зал")
                    {
                        return "Этот инвентраь по техническим причинам не может быть заказан";
                    }
                    if (this.mhr == "Книгохранение - МБА")
                    {
                        return "Этот инвентраь по техническим причинам не может быть заказан";
                    }
                }
                if (this.IsAllig)
                {
                    if (this.IsOrderedAsAligatByCurReader(idr.ToString()))
                    {
                        return "Этот экземпляр - приплетен к другой книге и уже заказан вами";
                    }
                    if (this.IsOrderedAsAligat())
                    {
                        return "Этот экземпляр - приплетен к другой книге и заказан другим читателем";
                    }
                }
                else
                {
                    if (this.IsAlreadyInOrder(idr) && !this.IS_SAME_INV_WITH_ANOTHER_NOTE()/*такой же инвентарь в том же пине но другое примечание (другая часть)*/)
                    {
                        return "Книга уже заказана Вами. Вы не можете заказать книгу второй раз.";
                        //cell.ForeColor = Color.Red;
                    }
                    if (this.IsAlreadyInOrder(idr) && this.IS_SAME_INV_WITH_ANOTHER_NOTE())
                    {
                        if (this.IsAlreadyInOrderWithAnotherNote(idr))
                        {
                            return "Книга уже заказана Вами. Вы не можете заказать книгу второй раз.";
                        }
                    }
                    if (this.IsAlreadyInOrderAsNOTMAINALLIGAT(idr))
                    {
                        return "Этот экземпляр - приплетен к другой книге и уже заказан вами";
                    }

                    string zal = this.OrderedButNoIssued();
                    if (zal != "")
                    {
                        return "Этот экземпляр забронирован другим читателем. Есть возможность взять в зале \"" + zal + "\" до прихода читателя";
                    }
                    string rec = this.RecievedButNotIssued();
                    if (this.IsIssued() == 1)
                    {
                        return "Этот экземпляр выдан другому читателю.";
                    }
                    if (rec != "")
                    {
                        return "Этот экземпляр принят залом \"" + rec + "\", но не выдавался еще никому. Пройдите в указанный зал.";
                    }
                    string pfbk = this.PreparedForBK();
                    if (pfbk != "")
                    {
                        return "Этот экземпляр подготовлен к сдаче в книгохранение и находится в зале " + pfbk + ". Пройдите в указанный зал.";
                    }
                    if (this.IsOrdered())
                    {
                        return "Этот экземпляр уже заказан";
                    }
                }
                return "";
            }
        }
        private bool IsDayPastAfterReturn(int idr,int rtype)
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select top 1 * from Reservation_R..ELISSUED_HST where IDREADER = " +
                                                    idr.ToString() + " and R_TYPE = "+rtype+" and IDMAIN = " + this.IDMAIN + "and BASE = 1 order by ID desc"
                                                    , con);
            int i = da.Fill(DS, "t");
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                TimeSpan ts = (DateTime.Now.Date - (DateTime)r["DATERETURN"]);
                if (ts.Days >= 1) return true; else return false;
            }
            return true;

        }
        private bool IsFiveElBooksIssued(int idr, int rtype)
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select * from Reservation_R..ELISSUED where R_TYPE = "+rtype.ToString()+" and IDREADER = " + idr.ToString(), con);
            int i = da.Fill(DS, "Name");
            if (i >= 5) return true; else return false;
        }

        private bool IsELOrderedByCurrentReader(int idr, int rtype)
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select * from Reservation_R..ELISSUED where IDMAIN = " + this.IDMAIN + " and R_TYPE = " + rtype.ToString() + " and IDREADER = " + idr.ToString(), con);
            int i = da.Fill(DS, "Name");
            if (i == 0) return false; else return true;
            
        }

        private bool IsAlreadyInOrderWithAnotherNote(int reader)
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Book_EC = " + this.IDMAIN + " and ID_Reader = " + reader + " and InvNumber = '" + this.inv + "' and INOTE = '" + this.note + "'", con);
            Status.Fill(DS, "Name");
            Status.Dispose();
            con.Dispose();
            if (DS.Tables["Name"].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool IS_SAME_INV_WITH_ANOTHER_NOTE()//такой же инвентарь в том же пине но другое примечание (другая часть)
        {

            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select * from BJVVV..DATAEXT A " +

                "where MNFIELD = 899 and MSFIELD = '$p' " +
                "and exists  " +
                "(select 1 from BJVVV..DATAEXT B  " +
                "where MNFIELD = 899 and MSFIELD = '$x' and A.IDDATA = B.IDDATA) " +
                "and exists  " +
                "(select 1 from BJVVV..DATAEXT B  " +
                "where B.MNFIELD = 899 and B.MSFIELD = '$p' and A.IDMAIN = B.IDMAIN and A.IDDATA != B.IDDATA) " +
                "and A.IDDATA = '" + this.iddata + "'", con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds, "t");
            da.Dispose();
            con.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }
        public bool ISExtremistLiterature()//проверить, является ли данная электронная копия экстремистской
        {

            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select 1 from BJVVV..DATAEXT  " +

                "where IDMAIN = "+this.IDMAIN+" and MNFIELD = 899 and MSFIELD = '$x' and SORT = 'Э' ",con) ;
            DataSet ds = new DataSet();
            int count = da.Fill(ds, "t");
            da.Dispose();
            con.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }
        private string PreparedForBK()
        {
            if (this.inv.Contains("Электронная"))
            {
                return "";
            }
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks A where A.BAR = '" + this.Bar + "' and A.IDDATA ='" + this.iddata + 
                                                    "' and A.RETINBK = 0 and A.PFORBK = 1 ",con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds,"t");
            da.Dispose();
            con.Dispose();
            if (count > 0)
                return ds.Tables["t"].Rows[0]["DEPNAME"].ToString();
            else
                return "";
        }

        private bool IsOrdered()
        {
            if (this.inv.Contains("Электронная"))
            {
                return false;
            }
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            da = new SqlDataAdapter("select * from Reservation_O..Orders where IDDATA = " + this.iddata, con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            da.Dispose();
            con.Dispose();
            if (count > 0)
                return true;
            else
                return false;
        }

        private string RecievedButNotIssued()
        {
            if (this.inv.Contains("Электронная"))
            {
                return "";
            }
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            if (this.Bar == null)
                da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks where IDDATA ='" + this.iddata + "' and RETINBK = 0  ", con);
            else
                da = new SqlDataAdapter("select * from Reservation_R..RecievedBooks A where A.BAR = '" + this.Bar + "' and A.RETINBK = 0 and A.PFORBK = 0 "
                    + " and not exists (select 1 from Reservation_R..ISSUED_OF B where A.BAR = B.BAR)", con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            da.Dispose();
            con.Dispose();
            if (count > 0)
            {
                return ds.Tables[0].Rows[0]["DEPNAME"].ToString();
            }
            else
            {
                return "";
            }
        }

        private string OrderedButNoIssued()
        {
            if (this.inv.Contains("Электронная"))
            {
                return "";
            }
            SqlDataAdapter da;
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            da = new SqlDataAdapter("select * from Reservation_R..ISSUED_OF where IDMAIN = 0 and IDDATA ='" + this.iddata + "' ", con);
            DataSet ds = new DataSet();
            int count = da.Fill(ds);
            da.Dispose();
            con.Dispose();
            if (count > 0)
            {
                return ds.Tables[0].Rows[0]["ZALRET"].ToString();
            }
            else
            {
                return "";
            }
        }

        private bool IsAlreadyInOrder(int reader)
        {
            if (this.inv.Contains("Электронная"))
            {
                return false;
            }

            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DataSet DS = new DataSet();
            SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Book_EC = " + this.IDMAIN + " and ID_Reader = " + reader, con);
            Status.Fill(DS, "Name");
            con.Dispose();
            Status.Dispose();
            if (DS.Tables["Name"].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        private bool IsAlreadyInOrderAsNOTMAINALLIGAT(int reader)
        {
            if (this.inv.Contains("Электронная"))
            {
                return false;
            }

            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DataSet DS = new DataSet();
            SqlDataAdapter Status = new SqlDataAdapter("select * from Reservation_O..Orders where ALGIDM = " + this.IDMAIN + " and ID_Reader = " + reader, con);
            Status.Fill(DS, "Name");
            con.Dispose();
            Status.Dispose();
            if (DS.Tables["Name"].Rows.Count > 0)
            {
                return true;
            }
            return false;
           
        }

        #region Члены IEquatable<InvOfBook>

        public bool Equals(InvOfBook other)
        {
            return  this.iddata == other.iddata &&
                    this.inv == other.inv;
        }

        #endregion


    }

}
