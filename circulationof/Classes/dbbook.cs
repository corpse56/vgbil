using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Circulation
{
    public class dbBook
    {
        public dbBook Clone(string BASENAME)
        {
            return new dbBook(this, BASENAME);
        }
        public dbBook()
        {
            this.author = "";
            this.barcode = "";
            this.id = "";
            this.name = "";
            this.rid = "";
            this.rname = "";
            this.inv = "";
            this.vzv = new DateTime();
            this.fctvzv = new DateTime();
            this.zal = "";
            this.RESPAN = "";
            this.klass = "";
            this.FirstIssue = new DateTime();
            this.ChgKlass = new DateTime();
            this.zalid = "";
            this.idemp = "";
            this.religion = false;
            this.redk = false;
            this.additionalNumbers = "";
            this.NumbersCount = 1;
            this.iddata = "0";
            this.ord_id = "-1";
            this.ord_rid = "-1";
        }
        public dbBook(string BASENAME)
        {
            this.author = "";
            this.barcode = "";
            this.id = "";
            this.name = "";
            this.rid = "";
            this.rname = "";
            this.inv = "";
            this.vzv = new DateTime();
            this.fctvzv = new DateTime();
            this.zal = "";
            this.RESPAN = "";
            this.klass = "";
            this.FirstIssue = new DateTime();
            this.ChgKlass = new DateTime();
            this.zalid = "";
            this.idemp = "";
            this.religion = false;
            this.redk = false;
            this.additionalNumbers = "";
            this.NumbersCount = 1;
            this.BASENAME = BASENAME;
            this.iddata = "0";
            this.ord_id = "-1";
            this.ord_rid = "-1";
        }

        public dbBook(dbBook Book, string BASENAME)
        {
            this.author = Book.author;
            this.barcode = Book.barcode;
            this.id = Book.id;
            this.name = Book.name;
            this.rid = Book.rid;
            this.rname = Book.rname;
            this.inv = Book.inv;
            this.fctvzv = Book.fctvzv;
            this.vzv = Book.vzv;
            this.zal = Book.zal;
            this.RESPAN = Book.RESPAN;
            this.klass = Book.klass;
            this.FirstIssue = Book.FirstIssue;
            this.ChgKlass = Book.ChgKlass;
            this.zalid = Book.zalid;
            this.idemp = Book.idemp;
            this.religion = Book.religion;
            this.redk = Book.redk;
            this.code = Book.code;
            this.mainfund = Book.mainfund;
            this.number = Book.number;
            this.year = Book.year;
            this.additionalNumbers = Book.additionalNumbers;
            this.BASENAME = BASENAME;
            this.NumbersCount = Book.NumbersCount;
            this.iddata = Book.iddata;
            this.ord_id = Book.ord_id;
            this.ord_rid = Book.ord_rid;
        }
        public dbBook(string Bar,string BASENAME)
        {
            this.NumbersCount = 1;
            this.BASENAME = BASENAME;
            Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, SORT, IDDATA from BJVVV..DATAEXT where SORT = '" + Bar + "' and MNFIELD = 899 and MSFIELD = '$w'";
            Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
            DataSet B = new DataSet();
            int i = Conn.SQLDA.Fill(B);
            this.additionalNumbers = "";
            if (i == 0) // не найдено в основной базе
            {
                Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, SORT, IDDATA from REDKOSTJ..DATAEXT where SORT = '" + Bar + "' and MNFIELD = 899 and MSFIELD = '$w'";
                Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                int ParseResult;
                if (i == 0)//не найдено в базе редкость
                {

                    if ((Bar.IndexOf("U") == 0) && (Bar.Length == 10) && int.TryParse(Bar.Substring(1, 9),out ParseResult))
                    {
                        
                        Conn.SQLDA.SelectCommand.CommandText = "select  * from " + BASENAME + "..PreDescr where BARCODE = '" + Bar + "'";
                        Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                        B = new DataSet();
                        i = Conn.SQLDA.Fill(B);
                        if (i == 0)
                        {
                            //не найдена в таблице преописания
                            this.id = "надовводить";
                            this.barcode = Bar;
                        }
                        else
                        {
                            //найдена в таблице преописания
                            this.name = B.Tables[0].Rows[0]["NAME"].ToString();
                            this.barcode = B.Tables[0].Rows[0]["BARCODE"].ToString();
                            this.year = B.Tables[0].Rows[0]["YEAR"].ToString();
                            this.code = B.Tables[0].Rows[0]["CODE"].ToString();
                            this.number = B.Tables[0].Rows[0]["NUMBER"].ToString();
                            this.mainfund = (bool)B.Tables[0].Rows[0]["MAINFUND"];
                            this.redk = false;
                            this.religion = false;
                            this.ChgKlass = DateTime.Now;
                            this.id = "-1";
                            this.inv = "-1";
                            this.iddata = "0";
                            this.additionalNumbers = B.Tables[0].Rows[0]["ADDNUMBERS"].ToString();
                            if (this.mainfund)
                            {
                                this.klass = "Для выдачи";
                            }
                            else
                            {
                                this.klass = "ДП";
                                this.RESPAN = "ДП";
                            }
                            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..ISSUED_OF where IDMAIN_CONST = " 
                                                                    + this.id + " and BAR  = '" + this.barcode + "'";
                            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                            B = new DataSet();
                            this.rname = "";
                            this.rid = "-1";
                            try
                            {
                                i = Conn.SQLDA.Fill(B);
                                this.rid = B.Tables[0].Rows[0]["IDREADER"].ToString();

                                this.vzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_RET"].ToString());
                                //this.fctvzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_FACT_VOZV"].ToString());
                                this.FirstIssue = DateTime.Parse(B.Tables[0].Rows[0]["DATE_ISSUE"].ToString());
                                if (this.klass == "ДП")
                                {
                                    this.RESPAN = "ДП";
                                }
                                else
                                {
                                    this.RESPAN = B.Tables[0].Rows[0]["RESPAN"].ToString();
                                }
                                this.idemp = B.Tables[0].Rows[0]["IDEMP"].ToString();
                            }
                            catch
                            {
                                this.rname = "";
                            }
                            if ((this.rid != "") && (this.rid != "-1"))
                            {
                                Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + this.rid;
                                DataSet R = new DataSet();
                                Conn.ReaderDA.Fill(R);
                                string name = "";
                                string secondName = "";
                                try
                                {
                                    name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                                }
                                catch
                                {
                                    name = "";
                                }
                                try
                                {
                                    secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                                }
                                catch
                                {
                                    secondName = "";
                                }
                                this.rname = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                            }
                        }
                    }
                    else
                    {
                        this.id = "Неверный штрихкод";
                        return;
                    }
                }
                else //найдено в редкости
                {
                    this.redk = true;
                    string IDDATA = B.Tables[0].Rows[0]["IDDATA"].ToString();
                    this.iddata = IDDATA;
                    this.id = B.Tables[0].Rows[0]["IDMAIN"].ToString();
                    this.barcode = B.Tables[0].Rows[0]["SORT"].ToString();
                    Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, SORT, IDDATA from REDKOSTJ..DATAEXT where IDDATA = '" + IDDATA + "' and MNFIELD = 899 and MSFIELD = '$p'";
                    Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    string INV = B.Tables[0].Rows[0]["SORT"].ToString();

                    Conn.SQLDA.SelectCommand.CommandText = "WITH FC AS (SELECT dt.ID,dt.SORT, " +
                                                              "dt.MNFIELD, " +
                                                              "dt.MSFIELD, " +
                                                              "dt.IDMAIN, " +
                                                              "dtp.PLAIN " +
                                                       "FROM   REDKOSTJ..DATAEXT dt " +
                                                       "       JOIN REDKOSTJ..DATAEXTPLAIN dtp " +
                                                       "            ON  dt.ID = dtp.IDDATAEXT) " +
                                                       "select  COL1.PLAIN zag,dtpa.PLAIN avt from FC COL1 " +
                                                       "left join FC dtpa ON COL1.IDMAIN = dtpa.IDMAIN and dtpa.MNFIELD = 700 and dtpa.MSFIELD = '$a' " +
                                                       "where COL1.MNFIELD = 200 and COL1.MSFIELD = '$a'  and COL1.IDMAIN = " + this.id;
                    Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    this.name = B.Tables[0].Rows[0]["zag"].ToString();
                    this.author = B.Tables[0].Rows[0]["avt"].ToString();
                    Conn.SQLDA.SelectCommand.CommandText = "select B.SORT from REDKOSTJ..DATAEXT A, REDKOSTJ..DATAEXT B " +
                                                           " where A.IDMAIN  = " + this.id + " and A.SORT = '" + this.barcode +
                                                           "' and A.MSFIELD = '$w' and A.MNFIELD = 899  and " +
                                                           " A.IDDATA = B.IDDATA and B.MNFIELD= 899 and B.MSFIELD = '$p' ";

                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    this.inv = B.Tables[0].Rows[0]["SORT"].ToString();

                    //зал местонахождения книги
                    Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN from REDKOSTJ..DATAEXT A" +
                                                           " inner join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                                           " where A.IDDATA = " + IDDATA +
                                                           " and A.MNFIELD = 899 and A.MSFIELD = '$a'";
                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    this.zal = B.Tables[0].Rows[0]["PLAIN"].ToString();
                    if (this.zal.Contains("нигохранени") || this.zal.Contains("редкой книги"))
                    {
                        this.klass = "Для выдачи";
                    }
                    else
                    {
                        this.klass = "ДП";
                    }

                    //id зала местонахождения
                    Conn.SQLDA.SelectCommand.CommandText = " select * from REDKOSTJ..LIST_8 " +
                                                           " where SHORTNAME = '" + this.zal + "'";
                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    this.zalid = B.Tables[0].Rows[0]["ID"].ToString();

                    this.religion = false;

                    //класс издания
                    Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN,A.Changed from REDKOSTJ..DATAEXT A" +
                                                           " inner join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                                           " where A.IDDATA = " + IDDATA +
                                                           " and A.MNFIELD = 921 and A.MSFIELD = '$c'";
                    B = new DataSet();
                    i = Conn.SQLDA.Fill(B);
                    string o = "";
                    if (i != 0)
                    {
                        o = B.Tables[0].Rows[0]["Changed"].ToString();
                        //this.klass = B.Tables[0].Rows[0]["PLAIN"].ToString();
                    }
                    /*else
                    {
                        o = B.Tables[0].Rows[0]["Changed"].ToString();
                        //this.klass = "no_klass";
                    }*/
                    if (o != "")
                    {
                        this.ChgKlass = ((DateTime)B.Tables[0].Rows[0]["Changed"]).Date;
                    }
                    else
                    {
                        this.ChgKlass = DateTime.Now;
                    }
                    if (this.klass == "ДП")
                    {
                        this.RESPAN = "ДП";
                    }
                    else
                    {
                        this.RESPAN = "";
                    }

                    /*if (this.religion)//если книги из религизной коллекции, то выдавать и принимать как с классом ДП и только в зале рел.лит-ры
                    {
                        this.klass = "ДП";
                    }*/

                    Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..ISSUED_OF where IDMAIN_CONST = " + this.id + " and INV  = '" + INV + "'";
                    Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                    B = new DataSet();
                    this.rname = "";
                    this.rid = "-1";
                    try
                    {
                        i = Conn.SQLDA.Fill(B);
                        this.rid = B.Tables[0].Rows[0]["IDREADER"].ToString();

                        this.vzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_RET"].ToString());
                        //this.fctvzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_FACT_VOZV"].ToString());
                        this.FirstIssue = DateTime.Parse(B.Tables[0].Rows[0]["DATE_ISSUE"].ToString());
                        if (this.klass == "ДП")
                        {
                            this.RESPAN = "ДП";
                        }
                        else
                        {
                            this.RESPAN = B.Tables[0].Rows[0]["RESPAN"].ToString();
                        }
                        this.idemp = B.Tables[0].Rows[0]["IDEMP"].ToString();
                    }
                    catch
                    {
                        this.rname = "";
                    }
                    if ((this.rid != "") && (this.rid != "-1"))
                    {
                        Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + this.rid;
                        DataSet R = new DataSet();
                        Conn.ReaderDA.Fill(R);
                        string name = "";
                        string secondName = "";
                        try
                        {
                            name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                        }
                        catch
                        {
                            name = "";
                        }
                        try
                        {
                            secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                        }
                        catch
                        {
                            secondName = "";
                        }
                        this.rname = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                    }

                }
            }
            else//===============================================================================================================================
            {
                this.redk = false;
                string IDDATA = B.Tables[0].Rows[0]["IDDATA"].ToString();
                this.iddata = IDDATA;
                this.id = B.Tables[0].Rows[0]["IDMAIN"].ToString();
                this.barcode = B.Tables[0].Rows[0]["SORT"].ToString();
                Conn.SQLDA.SelectCommand.CommandText = "select  ID, IDMAIN, SORT, IDDATA from BJVVV..DATAEXT where IDDATA = '" + IDDATA + "' and MNFIELD = 899 and MSFIELD = '$p'";
                Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                string INV = B.Tables[0].Rows[0]["SORT"].ToString();

                Conn.SQLDA.SelectCommand.CommandText = "WITH FC AS (SELECT dt.ID,dt.SORT, " +
                                                          "dt.MNFIELD, " +
                                                          "dt.MSFIELD, " +
                                                          "dt.IDMAIN, " +
                                                          "dtp.PLAIN " +
                                                   "FROM   BJVVV..DATAEXT dt " +
                                                   "        JOIN BJVVV..DATAEXTPLAIN dtp " +
                                                   "            ON  dt.ID = dtp.IDDATAEXT) " +
                                                   "select  COL1.PLAIN zag,dtpa.PLAIN avt from FC COL1 " +
                                                   "left join FC dtpa ON COL1.IDMAIN = dtpa.IDMAIN and dtpa.MNFIELD = 700 and dtpa.MSFIELD = '$a' " +
                                                   "where COL1.MNFIELD = 200 and COL1.MSFIELD = '$a'  and COL1.IDMAIN = " + this.id;
                Conn.SQLDA.SelectCommand.Connection = Conn.BRIT_SOVETCon;
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                this.name = B.Tables[0].Rows[0]["zag"].ToString(); ;
                this.author = B.Tables[0].Rows[0]["avt"].ToString();
                Conn.SQLDA.SelectCommand.CommandText = "select B.SORT from BJVVV..DATAEXT A, BJVVV..DATAEXT B " +
                                                       " where A.IDMAIN  = " + this.id + " and A.SORT = '" + this.barcode +
                                                       "' and A.MSFIELD = '$w' and A.MNFIELD = 899  and " +
                                                       " A.IDDATA = B.IDDATA and B.MNFIELD= 899 and B.MSFIELD = '$p' ";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                this.inv = B.Tables[0].Rows[0]["SORT"].ToString();


                //зал местонахождения книги
                Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN from BJVVV..DATAEXT A" +
                                                       " inner join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                                       " where A.IDDATA = " + IDDATA +
                                                       " and A.MNFIELD = 899 and A.MSFIELD = '$a'";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                if (i != 0)
                {
                    this.zal = B.Tables[0].Rows[0]["PLAIN"].ToString();
                    if (this.zal.Contains("нигохранени") || this.zal.Contains("бонемент") || this.zal.Contains("абонементного"))
                    {
                        this.klass = "Для выдачи";
                    }
                    else
                    {
                        this.klass = "ДП";
                    }
                }
                else
                {
                    this.klass = "Для выдачи";
                }

                //id зала местонахождения
                Conn.SQLDA.SelectCommand.CommandText = " select * from BJVVV..LIST_8 " +
                                                       " where SHORTNAME = '" + this.zal + "'";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                if (i != 0)
                {
                    this.zalid = B.Tables[0].Rows[0]["ID"].ToString();
                }

                //принадлежит ли екнига коллекции религ. лит?
                Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN from BJVVV..DATAEXT A" +
                                                       " inner join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                                       " where A.IDDATA = " + IDDATA +
                                                       " and A.MNFIELD = 899 and A.MSFIELD = '$b' and 1=0 and" +
                                                       " (SORT = 'КоллекцияArdis' or SORT = 'КоллекцияBibliothecaHermetica' or SORT = 'КоллекцияYMCAPress' or SORT = 'КоллекцияЖизньсБогом' or SORT = 'КоллекцияЗернов')";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                this.religion = (i == 0) ? false : true;//отныне всегда false. коллекции сдают в книгохранение. пусть берут все.

                //класс издания
                Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN,A.Changed from BJVVV..DATAEXT A" +
                                                       " inner join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                                       " where A.IDDATA = " + IDDATA +
                                                       " and A.MNFIELD = 921 and A.MSFIELD = '$c'";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                string o = "";
                if (i != 0)
                {
                    o = B.Tables[0].Rows[0]["Changed"].ToString();
                    //this.klass = B.Tables[0].Rows[0]["PLAIN"].ToString();
                }
                /*else
                {
                    o = B.Tables[0].Rows[0]["Changed"].ToString();
                    //this.klass = "no_klass";
                }*/
                
                if (o != "")
                {
                    this.ChgKlass = ((DateTime)B.Tables[0].Rows[0]["Changed"]).Date;
                }
                else
                {
                    this.ChgKlass = DateTime.Now;
                }

                if (this.religion)//если книги из религизной коллекции, то выдавать и принимать как с классом ДП и только в зале рел.лит-ры
                {
                    this.klass = "ДП";
                }
                if (this.klass == "ДП")
                {
                    this.RESPAN = "ДП";
                }
                else
                {
                    this.RESPAN = "";//B.Tables[0].Rows[0]["RESPAN"].ToString();
                }
                Conn.SQLDA.SelectCommand.CommandText = " select B.PLAIN from BJVVV..DATAEXT A" +
                                       " inner join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT" +
                                       " where A.IDDATA = " + IDDATA +
                                       " and A.MNFIELD = 899 and A.MSFIELD = '$b' and " +
                                       " SORT = 'Врхр'";
                B = new DataSet();
                i = Conn.SQLDA.Fill(B);
                if (i != 0)//если временное хранение то считать ДП
                {
                    this.klass = "ДП";
                    this.RESPAN = "ДП";
                }


                Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..ISSUED_OF where BAR  = '" + this.barcode + "'";
                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                B = new DataSet();
                this.rname = "";
                this.rid = "-1";
                try
                {
                    i = Conn.SQLDA.Fill(B);
                    this.rid = B.Tables[0].Rows[0]["IDREADER"].ToString();
                    this.ISIssuedAtHome = (bool)B.Tables[0].Rows[0]["ATHOME"];
                    if (this.klass == "ДП")
                    {
                        this.RESPAN = "ДП";
                    }
                    else
                    {
                        this.RESPAN = B.Tables[0].Rows[0]["RESPAN"].ToString();
                    }

                    this.vzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_RET"].ToString());
                    //this.fctvzv = DateTime.Parse(B.Tables[0].Rows[0]["DATE_FACT_VOZV"].ToString());
                    this.FirstIssue = DateTime.Parse(B.Tables[0].Rows[0]["DATE_ISSUE"].ToString());
                    this.idemp = B.Tables[0].Rows[0]["IDEMP"].ToString();
                }
                catch
                {
                    this.rname = "";
                }
                if ((this.rid != "") && (this.rid != "-1"))
                {
                    Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberReader = " + this.rid;
                    DataSet R = new DataSet();
                    Conn.ReaderDA.Fill(R);
                    string name = "";
                    string secondName = "";
                    try
                    {
                        name = R.Tables[0].Rows[0]["Name"].ToString().Remove(1, R.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                    }
                    catch
                    {
                        name = "";
                    }
                    try
                    {
                        secondName = R.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, R.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                    }
                    catch
                    {
                        secondName = "";
                    }
                    this.rname = R.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                }
            }
            if (this.iddata != null)
            {
                Conn.SQLDA.SelectCommand.CommandText = " select ID,ID_Reader from Reservation_O..Orders " +
                                           " where IDDATA = " + this.iddata;
                B = new DataSet();
                i = Conn.SQLDA.Fill(B, "t");
                if (i != 0)
                {
                    this.ord_id = B.Tables["t"].Rows[0]["ID"].ToString();
                    this.ord_rid = B.Tables["t"].Rows[0]["ID_Reader"].ToString();
                    dbReader tmpr = new dbReader(this.ord_rid);
                    //this.ISIssuedAtHome = 
                }
                else
                {
                    this.ord_id = "-1";
                    this.ord_rid = "-1";
                }
            }
        }
        public string barcode;
        public string id;
        public string name;
        public string rid;
        public string rname;
        public string author;
        public string inv;
        public DateTime vzv;
        public DateTime fctvzv;
        public string zal;
        public string RESPAN;
        public string klass;
        public DateTime FirstIssue;
        public DateTime ChgKlass;
        public string zalid;
        public string idemp;
        public bool religion;
        public bool redk;
        public string year;
        public string code;
        public bool mainfund;
        public string number;
        public string additionalNumbers;
        private string BASENAME;
        public int NumbersCount;
        public string iddata;
        public string ord_id;
        public string ord_rid;
        public bool ISIssuedAtHome = false;
        public string GetZalRet()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..ISSUED_OF where BAR = '" + this.barcode + "' and IDMAIN = 0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["ZALRET"].ToString();
        }
        public string GetZalIss(string DEPNAME)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + BASENAME + "..ISSUED_OF where BAR = '" + this.barcode + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? DEPNAME : DS.Tables[0].Rows[0]["ZALISS"].ToString();
        }
        public string GetRealKlass()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select B.SORT from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD=  921 and B.MSFIELD = '$c'" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '"+this.barcode+"'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["SORT"].ToString();
        }
        public string GetShifr()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select " + BASENAME + ".dbo.GetSHIFRBJVVVINVIDDATA('" + this.inv + "'," + this.iddata + ") shifr";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["shifr"].ToString();
        }

        internal string getnote()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select ISNULL(B.PLAIN,'') note from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$x' and A.IDDATA = " + this.iddata;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["note"].ToString();
        }

        internal string getMIZD()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select ISNULL(B.PLAIN,'') note from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 210 and A.MSFIELD = '$a' and A.IDDATA = " + this.iddata;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["note"].ToString();

        }
        internal string get899a()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select ISNULL(B.PLAIN,'') note from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDDATA = " + this.iddata;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["note"].ToString();

        }
        internal string get899b()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select ISNULL(B.PLAIN,'') note from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$b' and A.IDDATA = " + this.iddata;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["note"].ToString();

        }

        internal string getFloor()
        {
            SqlDataAdapter dad = new SqlDataAdapter();
            dad.SelectCommand = new SqlCommand();
            dad.SelectCommand.CommandText = "select ISNULL(B.PLAIN,'') note from BJVVV..DATAEXT A " +
                        " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDDATA = " + this.iddata;
            dad.SelectCommand.Connection = Conn.BJVVVConn;
            DataSet DS = new DataSet();
            int c = dad.Fill(DS, "t");
            if (c == 0)
                return "";
            if (DS.Tables[0].Rows[0]["note"].ToString() == "…Зал… КОО Группа абонементного обслуживания")
                return "…Хран… Сектор книгохранения - Абонемент";
            return (c == 0) ? "" : DS.Tables[0].Rows[0]["note"].ToString();
        }
    }

}
