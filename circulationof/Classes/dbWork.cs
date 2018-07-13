using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Circulation
{
    public class DBWork
    {
        private DataSet ReaderMain;
        private DataSet Book;
        private DataSet Zakaz;
        public DialogResult ResPanORBookKeeping;
        public Form1 F1;
        public DBWork()
        {
            XmlConnections xml = new XmlConnections();
            Conn.ReadersCon = new SqlConnection(xml.GetReaderCon());// ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Z:\\progs\\Circulation\\ReadersNew.mdb");
            Conn.BRIT_SOVETCon = new SqlConnection(xml.GetBRIT_SOVETCon());// ("Data Source=192.168.3.241;Initial Catalog=BRIT_SOVET;Integrated Security=True");
            Conn.BJVVVConn = new SqlConnection(xml.GetBJVVVCon());
            Conn.ZakazCon = new SqlConnection(xml.GetZakazCon());//("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG;Integrated Security=True");
            Conn.ReaderDA = new SqlDataAdapter();
            Conn.ReaderDA.SelectCommand = new SqlCommand("select * from main where BarCode = 19", Conn.ReadersCon);
            Conn.ReaderDA.SelectCommand.Connection.Open();
            Conn.SQLDA = new SqlDataAdapter();
            Conn.SQLDA.SelectCommand = new SqlCommand("select * from BARCODE_UNITS where ID = 0", Conn.BRIT_SOVETCon);
            Conn.SQLDA.SelectCommand.Connection.Open();
            Conn.SQLDA.SelectCommand.Parameters.Add("@IDR", SqlDbType.NVarChar);
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = "0";

            Book = new DataSet();
            ReaderMain = new DataSet();
            Zakaz = new DataSet();
        }
        public DBWork(Form1 f1)
        {
            F1 = f1;
            XmlConnections xml = new XmlConnections();
            Conn.ReadersCon = new SqlConnection(xml.GetReaderCon());// ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Z:\\progs\\Circulation\\ReadersNew.mdb");
            Conn.BRIT_SOVETCon = new SqlConnection(xml.GetBRIT_SOVETCon());// ("Data Source=192.168.3.241;Initial Catalog=BRIT_SOVET;Integrated Security=True");
            Conn.ZakazCon = new SqlConnection(xml.GetZakazCon());//("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG;Integrated Security=True");
            Conn.BJVVVConn = new SqlConnection(xml.GetBJVVVCon());
            Conn.ReaderDA = new SqlDataAdapter();
            Conn.ReaderDA.SelectCommand = new SqlCommand("select * from main where BarCode = 19", Conn.ReadersCon);
            Conn.ReaderDA.SelectCommand.Connection.Open();
            Conn.SQLDA = new SqlDataAdapter();
            Conn.SQLDA.SelectCommand = new SqlCommand("select * from BARCODE_UNITS where ID = 0", Conn.BRIT_SOVETCon);
            Conn.SQLDA.SelectCommand.Connection.Open();
            Conn.SQLDA.SelectCommand.Parameters.Add("@IDR", SqlDbType.NVarChar);
            Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = "0";
            Book = new DataSet();
            ReaderMain = new DataSet();
            Zakaz = new DataSet();
            //DR = new OleDbDataReader();
        }
        public bool NEED_REQUIRMENT;
        public void setBookForReader(dbBook book, dbReader reader)
        {
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF " +
                " where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            this.NEED_REQUIRMENT = false;
            SqlCommand cmd = new SqlCommand();
            Conn.SQLDA.InsertCommand = new SqlCommand();
            cmd.Connection = Conn.ZakazCon;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            SqlTransaction tran;
            tran = cmd.Connection.BeginTransaction("forreader");
            cmd.Transaction = tran;
            try
            {
                cmd.CommandText = "select * from Reservation_O..Orders where IDDATA = " + book.iddata;
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if ((r["Status"].ToString() == "8") && (r["ID_Reader"].ToString() != reader.id))
                    {
                        r.Close();

                        cmd.CommandText = "insert into Reservation_O..OrdHis (ID_Reader,ID_Book_EC,ID_Book_CC,Status,Start_Date,Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA)" +
                            " select ID_Reader,ID_Book_EC,ID_Book_CC,11,Start_Date,Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA " +
                            " from Reservation_O..Orders where IDDATA = " + book.iddata;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete from Reservation_O..Orders where IDDATA = " + book.iddata;
                        cmd.ExecuteNonQuery();
                        
                        this.NEED_REQUIRMENT = true;
                    }
                    else
                    {
                        r.Close();

                        if (book.inv != "-1")
                        {
                            cmd.CommandText = "update Reservation_O..Orders set Status = 9, REFUSUAL = '' where IDDATA = " + book.iddata;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                r.Close();
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF (IDMAIN,BAR,DATE_RET,IDREADER,IDEMP,DATE_ISSUE,DATE_CHG_KLASS, IDMAIN_CONST, " +
                                                        " INV, ZALISS, RESPAN, KLASS, RLOCATION,IDDATA) values (@IDMAIN,@BAR,@DATE_RET,@IDREADER,@IDEMP,@DATE_ISSUE,@DATE_CHG_KLASS,@IDMAIN_CONST, " +
                                                        " @INV,  @ZALISS, @RESPAN, @KLASS, @RLOCATION,@IDDATA)";
                cmd.Parameters.Add("IDMAIN", SqlDbType.Int);
                cmd.Parameters.Add("BAR", SqlDbType.NVarChar);
                cmd.Parameters.Add("DATE_RET", SqlDbType.DateTime);
                cmd.Parameters.Add("IDREADER", SqlDbType.Int);
                cmd.Parameters.Add("IDEMP", SqlDbType.Int);
                cmd.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime);
                cmd.Parameters.Add("DATE_CHG_KLASS", SqlDbType.DateTime);
                cmd.Parameters.Add("IDMAIN_CONST", SqlDbType.NVarChar);
                cmd.Parameters.Add("INV", SqlDbType.NVarChar);
                cmd.Parameters.Add("ZALISS", SqlDbType.NVarChar);
                cmd.Parameters.Add("RESPAN", SqlDbType.NVarChar);
                cmd.Parameters.Add("KLASS", SqlDbType.NVarChar);
                cmd.Parameters.Add("RLOCATION", SqlDbType.NVarChar);
                cmd.Parameters.Add("IDDATA", SqlDbType.Int);
                
                cmd.Parameters["IDMAIN"].Value                  = book.id;
                cmd.Parameters["BAR"].Value                     = book.barcode;
                if (book.klass == "ДП")
                {
                    cmd.Parameters["DATE_RET"].Value            = DateTime.Now.Date;
                }
                else
                {
                    cmd.Parameters["DATE_RET"].Value            = DateTime.Now.AddDays(4).Date;
                }
                cmd.Parameters["IDREADER"].Value                = reader.id;
                cmd.Parameters["IDEMP"].Value                   = F1.EmpID;
                cmd.Parameters["DATE_CHG_KLASS"].Value          = book.ChgKlass;
                cmd.Parameters["DATE_ISSUE"].Value              = DateTime.Now;
                cmd.Parameters["IDMAIN_CONST"].Value            = book.id;
                cmd.Parameters["INV"].Value                     = book.inv;
                cmd.Parameters["ZALISS"].Value                  = F1.DepName;
                if (book.klass == "ДП")
                {
                    cmd.Parameters["RESPAN"].Value              = "ДП";
                }
                else
                {
                    if (book.RESPAN == null)
                        book.RESPAN = "";
                    if (this.NEED_REQUIRMENT)
                    {
                        cmd.Parameters["RESPAN"].Value          = book.RESPAN;
                    }
                    else
                    {
                        if (book.ord_rid == "-1")
                            cmd.Parameters["RESPAN"].Value      = book.RESPAN;
                        else
                            cmd.Parameters["RESPAN"].Value      = book.ord_rid;
                    }
                }
                cmd.Parameters["KLASS"].Value                   = book.klass;
                cmd.Parameters["IDDATA"].Value                  = ((book.iddata == null) || (book.iddata == ""))? 0 : int.Parse(book.iddata);
                cmd.Parameters["RLOCATION"].Value               = (reader.rlocation == null) ? "" : reader.rlocation;
                cmd.CommandText += ";select cast(scope_identity() as int)";
                object p = cmd.ExecuteScalar();
                int ident = (int)p;
                cmd.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                                                       " set IDISSUED_OF = " + ident.ToString() +
                                                       " where BAR = '" + book.barcode + "' and RETINBK=0";
                int rcc = cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                    " where BAR = '" + book.barcode + "'";
                cmd.ExecuteNonQuery();
                tran.Commit();
                cmd.Connection.Close();
            }
            catch
            {
                tran.Rollback();
            }

        }
        public string GetDepName(string id)
        {
            if (id == null)
                return "";
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select * from BJVVV..LIST_8 where ID = " + id;
            Conn.SQLDA.SelectCommand.Connection = Conn.BJVVVConn;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "t");
            return DS.Tables[0].Rows[0]["SHORTNAME"].ToString();

        }
        public bool setResBookForReader(dbBook book, dbReader reader)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..ISSUED_OF where BAR = '" + book.barcode + "' and IDMAIN = 0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                MessageBox.Show("Ошибка! книга не может быть выдана! Обратитесь к разработчику!");
                return true;
            }
            DateTime dr = (DateTime)DS.Tables[0].Rows[0]["DATE_RET"];
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            if ((dr < DateTime.Now.Date) && (reader.id == book.RESPAN))
            {
                DialogResult dialr = MessageBox.Show("У читателя закончился срок бронирования " + dr.ToShortDateString() + "! Хотите продлить бронь?",
                                "Продлить бронь?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialr == DialogResult.Yes)
                {
                    dr = DateTime.Today.AddDays(3);
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                                           " set IDMAIN = IDMAIN_CONST, RLOCATION = '" + F1.DepName + "', IDEMP = " + F1.EmpID + ", DATE_ISSUE = getdate(), DATE_RET = '" + dr.ToString("yyyyMMdd") + "', ZALISS = '" + F1.DepName + "',ZALRET = '-'" +
                                                           " where ID = " + DS.Tables["t"].Rows[0]["ID"].ToString();
                    MessageBox.Show("Бронь продлена до " + dr.ToString("dd.MM.yyyy"));
                    this.InsertStatisticsReservationProlonged(book, reader.id);
                    this.InsertStatisticsIssuedFromRespan(book, reader.id);
                }
                else
                {
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                                           " set IDMAIN = IDMAIN_CONST, RLOCATION = '" + F1.DepName + "', IDEMP = " + F1.EmpID + ", DATE_ISSUE = getdate(), ZALISS = '" + F1.DepName + "',ZALRET = '-'" +
                                                           " where ID = " + DS.Tables["t"].Rows[0]["ID"].ToString();
                    this.InsertStatisticsIssuedFromRespan(book, reader.id);
                }
            }
            else
            {
                if ((dr < DateTime.Now.Date) && (reader.id != book.RESPAN))
                {
                    MessageBox.Show("На этой бронеполке закончился срок бронирования, а книгу пытается взять другой читатель! Вам небходимо освободить бронеполку с помощью программы в разделе \"Работа с бронеполками\"!");
                    return true;
                }
                else
                {
                    if ((dr >= DateTime.Now.Date) && (reader.id != book.RESPAN))
                    {
                        Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                       " set IDMAIN = IDMAIN_CONST, IDEMP = " + F1.EmpID + ",  DATE_ISSUE = getdate(),ZALISS = '" + F1.DepName + "',ZALRET = '-', IDREADER = " + reader.id +
                                       ", RLOCATION = '" + reader.rlocation + "'" +
                                       " where ID = " + DS.Tables["t"].Rows[0]["ID"].ToString();
                        this.InsertStatisticsIssuedFromRespan(book, reader.id);
                    }
                    else
                    {
                        if ((dr >= DateTime.Now.Date) && (reader.id == book.RESPAN))
                        {
                            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                       " set IDMAIN = IDMAIN_CONST,RLOCATION = '" + F1.DepName + "', IDEMP = " + F1.EmpID + ",  DATE_ISSUE = getdate(),ZALISS = '" + F1.DepName + "',ZALRET = '-',ATHOME = 0, IDREADER = " + reader.id +
                                       " where ID = " + DS.Tables["t"].Rows[0]["ID"].ToString();
                            this.InsertStatisticsIssuedFromRespan(book, reader.id);
                        }
                    }
                }
            }
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();

            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 9, REFUSUAL = '' where IDDATA = " + book.iddata;
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();

            Conn.SQLDA.UpdateCommand.Connection.Close();

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            /*if (DS.Tables[0].Rows[0]["RESPAN"].ToString() != reader.id)
            {
                MessageBox.Show("Читатель берет книгу не со своей бронеполки!",
                                "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }*/
            return false;
        }

        public dbBook FindBookInIssued(dbBook book, dbReader reader)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from [" + F1.BASENAME + "].[dbo].[ISSUED_OF] " +
                                                   " where BAR = '" + book.barcode + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "t");
            if (i == 0) return null;
            return new dbBook(DS.Tables[0].Rows[0]["BAR"].ToString(), F1.BASENAME);
        }

        public bool setBookReturned(dbBook book)//false - успех true - неудача
        {
            int rnum;
            if (int.TryParse(book.rid, out rnum))
            {
                dbReader r = new dbReader(rnum);
                if ((r.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL)
                {
                    this.MoveToHistory(book);
                    return false;
                }
            }
            if ((book.get899b().ToLower() == "вх") && (this.F1.DepID == "22"))
            {
                book.klass = "Для выдачи";
                book.RESPAN = book.rid;
            }

            
            //if (book.ISIssuedAtHome)
            //{
              //  this.MoveToHistory(book);
               // return false;
            //}
            //else
            {
                Conn.SQLDA.SelectCommand.CommandText = "select * from [" + F1.BASENAME + "].[dbo].[ISSUED_OF] where IDMAIN = " + book.id + " and BAR = '" + book.barcode + "'";
                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                DataSet B = new DataSet();
                int i = Conn.SQLDA.Fill(B, "t");
                if (i == 0)
                {
                    MessageBox.Show("Штрихкод не найден среди выданных книг!");
                    return true;
                }
                if ((B.Tables[0].Rows[0]["RESPAN"].ToString() != B.Tables[0].Rows[0]["IDREADER"].ToString()) && (B.Tables[0].Rows[0]["ZALISS"].ToString() != F1.DepName))
                {
                    MessageBox.Show("Читатель сдает книгу, которая была взята им в другом зале c чужой бронеполеки! " +
                                    "Направьте читателя сдавать эту книгу в зал " + B.Tables[0].Rows[0]["ZALISS"].ToString() + "!",
                                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return true;
                }
                if (B.Tables[0].Rows[0]["RESPAN"].ToString() == "ДП")
                {
                    if (B.Tables[0].Rows[0]["ZALISS"].ToString() != F1.DepName)
                    {
                        MessageBox.Show("Читатель сдает книгу (ДП), которая была взята им в другом зале! Направьте читателя сдавать эту книгу в зал " + B.Tables[0].Rows[0]["ZALISS"].ToString() + "!",
                                        "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return true;
                    }
                    else
                    {
                        this.MoveToHistory(book);
                        return false;
                    }
                }
                if (book.rid != book.RESPAN)
                {
                    ResPanORBookKeeping = DialogResult.Yes;
                }
                else
                {

                    ResPanORBookKeeping = MessageBox.Show("Читатель сдает книгу на бронеполку?",
                        "Бронеполка или книгохранение?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (ResPanORBookKeeping == DialogResult.Cancel)
                    {
                        return true;
                    }
                }
                Conn.SQLDA.UpdateCommand = new SqlCommand();
                Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
                if (((DateTime)B.Tables[0].Rows[0]["DATE_RET"]) < DateTime.Now.AddDays(5))//если больше 5 дней то считается что профессор взял книгу
                {
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                                           " set IDMAIN = 0, ZALRET = '" + F1.DepName +
                                                           "' ,DATE_RET = '" + F1.dateTimePicker1.Value.Date.ToString("yyyyMMdd") +
                                                           "' where ID = " + B.Tables["t"].Rows[0]["ID"].ToString();
                }
                else
                {
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF " +
                                                           " set IDMAIN = 0, ZALRET = '" + F1.DepName +
                                                           "'" +
                                                           " where ID = " + B.Tables["t"].Rows[0]["ID"].ToString();
                }
                DateTime dr = (DateTime)B.Tables[0].Rows[0]["DATE_RET"];
                if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    Conn.SQLDA.UpdateCommand.Connection.Open();
                }
                int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                //меняем статус что вернули на бронеполку.
                if (book.inv != "-1")
                {
                    Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 3 where IDDATA = " + book.iddata;
                    rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                }
                Conn.SQLDA.UpdateCommand.Connection.Close();
                if (book.GetZalIss(F1.DepName) != F1.DepName)
                {
                    MessageBox.Show("Читатель сдает книгу, которая была взята им в " + book.GetZalIss(F1.DepName) + "!",
                                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (book.rid != book.RESPAN)
                {
                    MessageBox.Show("Читатель сдает книгу, которая была взята им с чужой бронеполки!",
                                    "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.MoveToHistoryAlienRespan(book);
                }
            }
            return false; 
        }
        public void MoveToHistory(dbBook book)
        {
            if (book.idemp == null)//если ни разу не выдавалась
            {
                this.MoveToPREPBK(book, -1);
                return;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn.BJVVVConn;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            string zalretbk = "";
            if (F1.DepName.Contains("Книгохра"))
            {
                zalretbk = book.GetZalIss(F1.DepName);
            }
            else
            {
                zalretbk = F1.DepName;
            }
            if (book.ISIssuedAtHome)
            {
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF_HST " +
                                                       " (IDREADER,IDEMP,DATE_ISSUE,DATE_RET,IDMAIN,INV,BAR,ZALISS,ZALRET,ATHOME,RESPAN) values (" +
                                                       " " + book.rid +
                                                       " ," + ((book.idemp == null) ? F1.EmpID : book.idemp) +
                                                       " , '" + book.FirstIssue.ToString("yyyyMMdd") + "'" +
                                                       " , getdate()" +
                                                       " ," + book.id +
                                                       " ,'" + book.inv +
                                                       "' ,'" + book.barcode +
                                                       "' ,'" + zalretbk +
                                                       "' ,'" + zalretbk +
                                                       "' ,1,'"+book.RESPAN+"');select cast(scope_identity() as int)";
            }
            else
            {
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF_HST " +
                                                       " (IDREADER,IDEMP,DATE_ISSUE,DATE_RET,IDMAIN,INV,BAR,ZALISS,ZALRET,RESPAN) values (" +
                                                       " " + book.rid +
                                                       " ," + ((book.idemp == null) ? F1.EmpID : book.idemp) +
                                                       " , '" + book.FirstIssue.ToString("yyyyMMdd") + "'" +
                                                       " , getdate() " +
                                                       " ," + book.id +
                                                       " ,'" + book.inv +
                                                       "' ,'" + book.barcode +
                                                       "' ,'" + zalretbk +
                                                       "' ,'" + zalretbk + "','" + book.RESPAN + "');select cast(scope_identity() as int)";
            }
            object p = cmd.ExecuteScalar();
            cmd.Connection.Close();
            int ident = (int)p;
                                                   
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.ISSUED_OF where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();

            Conn.SQLDA.DeleteCommand.Connection.Close();
            this.MoveToPREPBK(book, ident);
        }
        public DataTable GetResIss()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = 
               " select A.ID,(case when A.IDMAIN_CONST = -1 "+
               " then ISNULL(E.[NAME],'')+'; '+ISNULL(E.[YEAR],'') +'; ' +ISNULL(E.NUMBER,'') + '; '+ISNULL(E.ADDNUMBERS,'') "+
               " else ISNULL(CC.PLAIN,RCC.PLAIN) end) as title, " +
               " ISNULL(DD.PLAIN,RDD.PLAIN), case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end, " +
               " A.ZALISS, A.ZALRET, A.IDREADER, A.RESPAN, A.DATE_RET, " +
               "(case when A.IDMAIN = 0 then 'На бронеполке' else 'На руках у читателя' end),A.ID,A.BAR,A.INV " +
               " from " + F1.BASENAME + "..ISSUED_OF A" +
               " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
               " left join BJVVV..DATAEXT VVV on A.IDMAIN_CONST = VVV.IDMAIN and VVV.MNFIELD = 899 and VVV.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = VVV.SORT " +
               " left join REDKOSTJ..DATAEXT RED on A.IDMAIN_CONST = RED.IDMAIN and RED.MNFIELD = 899 and RED.MSFIELD = '$w' and A.BAR collate Cyrillic_general_CI_AI = RED.SORT " +
               " left join BJVVV..DATAEXT C on VVV.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C.MSFIELD = '$a'" +
               " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
               " left join BJVVV..DATAEXT D on VVV.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
               " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
               " left join REDKOSTJ..DATAEXT RC on RED.IDMAIN = RC.IDMAIN and RC.MNFIELD = 200 and RC.MSFIELD = '$a'" +
               " left join REDKOSTJ..DATAEXTPLAIN RCC on RC.ID = RCC.IDDATAEXT" +
               " left join REDKOSTJ..DATAEXT RD on RED.IDMAIN = RD.IDMAIN and RD.MNFIELD = 700 and RD.MSFIELD = '$a'" +
               " left join REDKOSTJ..DATAEXTPLAIN RDD on RD.ID = RDD.IDDATAEXT " +
               " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
               " where  A.RESPAN != 'ДП' and (((A.ZALISS = '" + F1.DepName + "') and ((A.ZALRET is null) or (A.ZALRET = '-') )) or A.ZALRET ='" + F1.DepName + "')";
            //
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public bool isBookBusy(string s)
        {
            //s = s.Remove(s.Length - 1, 1);
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..ISSUED_OF where  BAR ='" + s + "' and IDMAIN <>0";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //Book.Tables.Clear();
            DataSet Book = new DataSet();
            int i = Conn.SQLDA.Fill(Book);
            if (i == 0) return false;
            string j = Book.Tables[0].Rows[0]["IDReader"].ToString();
            if (j == "-1") return true;
            else return true;
            //return (i != 0) ? true : false;
        }
        public bool isReader(string s)
        {
            if (s.Length > 0)
                s = s.Remove(0, 1);
            //if (s.Length > 0)
                //s = s.Remove(s.Length - 1, 1);
            return ((s.Length > 18) || (s.Length == 7)) ? true : false;
        }

        public bool ChangeEmployee(string login, string pass)
        {//                                    SELECT Employee.* FROM Employee WHERE (((Employee.Login)="1") AND ((Employee.Password)="1"));

            Conn.ReaderDA.SelectCommand.CommandText = "SELECT * FROM BJVVV..USERS WHERE lower(LOGIN)='" + login.ToLower() + "' AND lower(PASSWORD)='" + pass.ToLower() + "'";
            //ReaderMain.Tables.Clear();
            DataSet R = new DataSet();
            if (Conn.ReaderDA.Fill(R) != 0)
            {
                F1.textBox1.Text = R.Tables[0].Rows[0]["NAME"].ToString();
                F1.EmpID = R.Tables[0].Rows[0]["ID"].ToString();
                F1.DepID = R.Tables[0].Rows[0]["DEPT"].ToString();
                F1.DepName = this.GetDepName(F1.DepID);
                F1.textBox2.Text = F1.DepName;
                return true;
            }
            else
                return false;
        }

        public class XmlConnections
        {
            public XmlTextReader reader;
            static String filename = Application.StartupPath + "\\DBConnections.xml";
            public XmlDocument doc;
            public string GetReaderCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/Readers");
                return node.InnerText;
            }
            public string GetZakazCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
                return node.InnerText;
            }
            public string GetBRIT_SOVETCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
                return node.InnerText;
            }
            internal string GetBJVVVCon()
            {
                XmlNode node = this.doc.SelectSingleNode("/Connections/BJVVV");
                return node.InnerText;
            }

            public XmlConnections()
            {
                try
                {
                    doc = new XmlDocument();
                    doc.Load(filename);// (reader);
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }

        internal DataTable GetFormular(string p)
        {
            
            //Conn.SQLDA.SelectCommand.Parameters["@IDR"].Value = p;
            Conn.SQLDA.SelectCommand.CommandText =
                     "select (case when B.IDMAIN_CONST=-1  " +
			         "             then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
		             "             (case when E.ADDNUMBERS is null  " +
					 "                   then ''  " +
					 "                   else E.ADDNUMBERS end)  " +
			         "             else (case when B.ZALISS = 'НИО редкой книги'  " +
					 "                       then rzagp.PLAIN " +
				"		                    else zagp.PLAIN end) end) " +
                "     as zag,  " +
                "     (case when B.IDMAIN_CONST = -1  " +
	            "           then E.[NAME] collate Cyrillic_General_CI_AI " +
	            "           else zag.SORT end) as Заглавие_sort,  " +
                "     (case when B.ZALISS = 'НИО редкой книги' then ravtp.PLAIN else avtp.PLAIN end) Автор, " +
                "    avt.SORT Автор_sort,  " +
                "     (case when B.IDMAIN_CONST = -1 then E.[BARCODE] else B.INV end) inv,  " +
                "     zag.IDMAIN idmain, " +
                "     cast(cast(B.DATE_ISSUE as varchar(11)) as datetime) issue, " +
                "    B.DATE_RET vozv,  " +
                "      B.IDMAIN zkid,B.ID zi, " +
                "     B.ZALISS ziss, B.ZALRET zret, " +
                "    B.BAR bar, B.IDMAIN overdue  " +
                "     ,(case when B.IDMAIN = 0 then 'На бронеполке' " +
                "     else (case when ((cast(B.IDREADER as nvarchar) = B.RESPAN or B.RESPAN = 'ДП' or B.RESPAN = '') and ATHOME = 0) then 'На руках' " +
                "     else (case when ((cast(B.IDREADER as nvarchar) = B.RESPAN or B.RESPAN = 'ДП' or B.RESPAN = '') and ATHOME = 1) then 'На руках (на дом)' " +
                "     else (case when (B.IDREADER = " + p + " and B.RESPAN != '" + p + "') then 'На руках. Взято с бронеполки № '+ B.RESPAN " +
                "     else 'На руках у читателя '+cast(B.IDREADER as nvarchar) end)  end) end) end) as status, " +
                "     B.DATE_RET dend " +
                " ,Reservation_R.dbo.GetProlongedTimes(B.ID, 'BJVVV') prolonged " +
                "     from " + F1.BASENAME + "..ISSUED_OF B    " +
                "     left join BJVVV..DATAEXT zag on  zag.MNFIELD = 200 and  zag.MSFIELD = '$a' and  zag.IDMAIN = B.IDMAIN_CONST   " +
                "     left join BJVVV..DATAEXT avt on  avt.MNFIELD = 700 and  avt.MSFIELD = '$a'  and avt.IDMAIN = B.IDMAIN_CONST  " +
                "     left join BJVVV..DATAEXTPLAIN zagp on zagp.IDDATAEXT = zag.ID   " +
                "     left join BJVVV..DATAEXTPLAIN avtp on avtp.IDDATAEXT = avt.ID  " +
                "     left join REDKOSTJ..DATAEXT rzag on  rzag.MNFIELD = 200 and  rzag.MSFIELD = '$a' and  rzag.IDMAIN = B.IDMAIN_CONST   " +
                "     left join REDKOSTJ..DATAEXT ravt on  ravt.MNFIELD = 700 and  ravt.MSFIELD = '$a'  and ravt.IDMAIN = B.IDMAIN_CONST  " +
                "     left join REDKOSTJ..DATAEXTPLAIN rzagp on rzagp.IDDATAEXT = rzag.ID   " +
                "     left join REDKOSTJ..DATAEXTPLAIN ravtp on ravtp.IDDATAEXT = ravt.ID  " +
                "     left join " + F1.BASENAME + "..PreDescr E on B.BAR = E.BARCODE " +
                "     where B.RESPAN = '" + p + "' or (B.RESPAN != '" + p + "' and B.IDREADER = " + p + " and B.IDMAIN != 0)";
            
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet R = new DataSet();
            R.Tables.Add("form");
            int i = Conn.SQLDA.Fill(R.Tables["form"]);

            return R.Tables["form"];
        }

        internal void RecieveBookDP(string p)
        {
            dbBook b = new dbBook(p, F1.BASENAME);
            this.MoveToHistory(b);
        }

        internal List<dbBook> FindBooksIssuedAnotherReaders(dbReader ReaderRecord)
        {
            List<dbBook> ret = new List<dbBook>();
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select CC.PLAIN, A.BAR " +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " inner join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                   " inner join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                   " where A.RESPAN = '"+ReaderRecord.id+"' and A.IDREADER != "+ReaderRecord.id+" and A.IDMAIN != 0";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                ret.Add(new dbBook(r["BAR"].ToString(), F1.BASENAME));
            }
            return ret;
        }

        internal object GetIssBooks()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (F1.DepName == "НИО редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID,(case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+(case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), A.IDREADER, A.RESPAN, A.DATE_RET " +
                    ",  " + F1.BASENAME + ".dbo.GetReaderRightString(A.IDREADER) rights, l8.SHORTNAME dep, case when A.ATHOME = 0 then 'Выдано в залы' else 'Выдано на дом' end ath" +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join REDKOSTJ..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join REDKOSTJ..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join REDKOSTJ..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                       " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                       " where A.ZALISS = '" + dep + "' and A.IDMAIN != 0 and A.ATHOME = 0";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID, "+
                    " (case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ "+
                    " (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), "+
                    " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), A.IDREADER, A.RESPAN, A.DATE_RET, " +
                    "  " + F1.BASENAME + ".dbo.GetReaderRightString(A.IDREADER) rights, l8.SHORTNAME dep, case when A.ATHOME = 0 then 'Выдано в залы' else 'Выдано на дом' end ath" +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                       " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                       " where A.ZALISS = '" + dep + "' and A.IDMAIN != 0 and A.ATHOME = 0";
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal string RecieveBook(string BAR)
        {
            dbBook Book = new dbBook(BAR, F1.BASENAME);
            if ((Book.id == "Неверный штрихкод"))
            {
                return "Неверный штрихкод";
            }
            if (Book.zal == "…Зал… КОО Группа абонементного обслуживания")
            {
                return "книга в зале абонементного обслуживания";
            }
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..RecievedBooks where BAR = '" + Book.barcode + 
                                                   "' and RETINBK = 'false'" ;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (F1.DepName.Contains("нигохранени") || F1.DepName.Contains("…Хран… КНИО Группа хранения редкой книги")) //если это книгохранение
            {
                
                if (t == 0) //если не найдено в принятых
                {
                    return "не найдено в принятых";//книгу либо забыли принять либо уже возвратили
                }
                else
                {
                    //сдаем
                    Conn.SQLDA.SelectCommand = new SqlCommand();
                    Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
                    Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                    Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..PREPBK where BAR = '" + BAR + "'";
                    DataSet DS1 = new DataSet();
                    t = Conn.SQLDA.Fill(DS1, "t");
                    string IDHST = "0";
                    if (t != 0)
                    {
                        IDHST = DS1.Tables[0].Rows[0]["IDHST"].ToString();
                    }
                    else
                    {
                        Conn.SQLDA.SelectCommand = new SqlCommand();
                        Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
                        Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                        Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..ISSUED_OF where BAR = '" + BAR + "'";
                        DS1 = new DataSet();
                        t = Conn.SQLDA.Fill(DS1, "t");
                        if (t == 0)
                        {
                            //книга ни разу не выдавалась и не подготовлена к сдаче в кх
                            //подготавливаем к сдаче в КХ
                            dbBook b = new dbBook(BAR, this.F1.BASENAME);
                            this.MoveToHistory(b);
                        }
                        else
                        {
                            string IDMAIN = DS1.Tables[0].Rows[0]["IDMAIN"].ToString();
                            if (IDMAIN != "0")
                            {
                                return "Читатель не сдал книгу";//книгу еще не сдал книгу!
                            }
                            else
                            {
                                //книга на бронеполке, но не подготовлена к сдаче в кх
                                //подготавливаем к сдаче в КХ
                                dbBook b = new dbBook(DS1.Tables[0].Rows[0]["BAR"].ToString(), this.F1.BASENAME);
                                this.MoveToHistory(b);
                            }
                        }
                    }
                    if (IDHST == "")
                    {
                        IDHST = "0";
                    }
                    Conn.SQLDA.UpdateCommand = new SqlCommand();
                    Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.UpdateCommand.Connection.Open();
                    }
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                        " set RETINBK = 'true', DATEFINISH = '" + DateTime.Now.ToString("yyyyMMdd") +
                        "' , RECDEPNAME = '" + F1.DepName + "', RECIDEMP = " + F1.EmpID + ", IDHST = " + IDHST +
                        " where ID = " + DS.Tables[0].Rows[0]["ID"].ToString();
                    Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.UpdateCommand.Connection.Close();
                    }
                    //удаляем из подготовленных к сдаче
                    Conn.SQLDA.DeleteCommand = new SqlCommand();
                    Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Open();
                    }
                    Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                        " where BAR = '" + BAR + "'";
                    Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Close();
                    }
                    //удаляем из заказа

                    this.MoveToHistoryOrders(Book);
                    //удаляем из выданных на всякий наверное случай
                    Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Open();
                    }
                    Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.ISSUED_OF where BAR = '" + BAR + "'";
                    int rc = Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
                    Conn.SQLDA.DeleteCommand.Connection.Close();
                    return "книга успешно сдана в хранение";
                }
            }
            else//если это не книгохранение
            {
                if (t == 0)//если не найдено в принятых
                {

                    Conn.SQLDA.InsertCommand = new SqlCommand();
                    Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                    Conn.SQLDA.InsertCommand.Parameters.Add("dstart", SqlDbType.DateTime);
                    Conn.SQLDA.InsertCommand.Parameters["dstart"].Value = DateTime.Now;
                    if (Book.id == "надовводить")
                    {
                        Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..RecievedBooks "+
                            " (IDMAIN,INV,BAR,DEPNAME,DATESTART,RETINBK,IDEMP,STARTMHR,STARTDEP) values" +
                            "(-1,'-1','" + Book.barcode + "','" + F1.DepName + "',@dstart" +
                            ",'false',"+F1.EmpID+",NULL,'"+F1.DepName+"')";
                    }
                    else
                    {
                        string IDReaderOrder = this.getReaderIDFromOrders(Book);
                        bool isallig = this.GetISAllig(Book);
                        if (IDReaderOrder == "")
                            IDReaderOrder = "0";
                        Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..RecievedBooks " +
                            " (IDMAIN,INV,BAR,DEPNAME,DATESTART,RETINBK,IDEMP,STARTMHR,STARTDEP,IDREADER,ISALLIG,IDDATA) values" +
                            "(" + Book.id + ",'" + Book.inv + "','" + Book.barcode + "','" + F1.DepName + "',@dstart" +
                            ",'false'," + F1.EmpID + ",'" + Book.zal + "','" + F1.DepName + "'," +
                            IDReaderOrder + ",'" + isallig.ToString() + "'," + Book.iddata + ")";
                        this.ChangeOrderStatus(Book);
                    }

                    if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.InsertCommand.Connection.Open();
                    }
                    Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.InsertCommand.Connection.Close();
                    }
                    return "успешно принята залом";
                }
                else
                {
                    if (DS.Tables[0].Rows[0]["DEPNAME"].ToString() == F1.DepName)
                    {
                        return "уже принята залом";
                    }
                    Conn.SQLDA.UpdateCommand = new SqlCommand();
                    Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.UpdateCommand.Connection.Open();
                    }
                    Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                        " set DEPNAME = '" + F1.DepName + "'"+
                        " where ID = " + DS.Tables[0].Rows[0]["ID"].ToString();
                    Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.UpdateCommand.Connection.Close();
                    }
                    Conn.SQLDA.InsertCommand = new SqlCommand();
                    Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                    Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics]" +
                        " (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,DEP1,DEP2) values " +
                        "(" + F1.EmpID + ",getdate(),12," + F1.DepID + ",0,'" + Book.barcode + "','"
                        +DS.Tables[0].Rows[0]["DEPNAME"].ToString()+"','"+F1.DepName+"')";//сменили отдел который принял книгу из книгохранения
                    if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.InsertCommand.Connection.Open();
                    }
                    try
                    {
                        Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("Не сработала статистика (сменили отдел который принял книгу из книгохранения)");
                    }
                    Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.InsertCommand.Connection.Close();
                    }
                    return "успешно принята залом";
                }

            }
        }
        public void MoveToHistoryOrders(dbBook Book)
        {
            if (Book.iddata == null) return;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            SqlConnection con = Conn.ZakazCon;
            con.Open();
            SqlTransaction tr;
            Conn.SQLDA.SelectCommand.Connection = con;
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_O..Orders where IDDATA = " + Book.iddata;
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "possb");
            con.Close();
            if (i > 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    con.Open();

                    SqlCommand comm = new SqlCommand();
                    comm.Connection = con;
                    tr = con.BeginTransaction("movhis");
                    comm.Transaction = tr;
                    try
                    {
                        //DA.Update(DS.Tables["ordhis"]);//так тоже транзакция работает!
                        //DA.DeleteCommand.ExecuteNonQuery();
                        string idstatus = row["Status"].ToString();
                        if (idstatus != "10")//если не отказ, значит завершено
                        {
                            idstatus = "11";
                        }
                        comm.CommandText = "insert into Reservation_O..OrdHis (ID_Reader, ID_Book_EC, ID_Book_CC, Status, Start_Date, Change_Date, InvNumber, Form_Date, Duration, Who, IDDATA,REFUSUAL) " +
                                           "values (" + row["ID_Reader"].ToString() + "," + row["ID_Book_EC"].ToString() + "," +
                                           row["ID_Book_CC"].ToString() + ","+idstatus+",'" + ((DateTime)row["Start_Date"]).ToString("yyyyMMdd") + "','" +
                                           DateTime.Now.ToString("yyyyMMdd") + "','" + row["InvNumber"].ToString() + "','" +
                                           ((DateTime)row["Form_Date"]).ToString("yyyyMMdd hh:m:ss.fff tt") + "'," + row["Duration"].ToString() + "," +
                                           row["Who"].ToString() + ", "+row["IDDATA"].ToString()+",'"+row["REFUSUAL"].ToString()+"')";
                        comm.ExecuteNonQuery();
                        comm.CommandText = "delete from Reservation_O..Orders where ID = " + row["ID"].ToString();
                        comm.ExecuteNonQuery();

                        tr.Commit();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        //System.Windows.Forms.MessageBox.Show(ex.Message);
                        tr.Rollback();
                    }


                }
            }

        }
        private bool GetISAllig(dbBook Book)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_O..Orders where " +
                                                    " IDDATA = " + Book.iddata;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 1)
            {
                if (DS.Tables["t"].Rows[0]["ALGIDM"].ToString() == "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }

        private void ChangeOrderStatus(dbBook Book)
        {
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 2 " +
                " where IDDATA = " + Book.iddata;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }

        }

        public string getReaderIDFromOrders(dbBook Book)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select top 1 * from Reservation_O..Orders where "+
                                                    " IDDATA = " + Book.iddata + " order by ID desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 1)
                return DS.Tables["t"].Rows[0]["ID_Reader"].ToString();
            else
                return "";
        }
        
        internal void InsertMag(dbBook Book)
        {
            if (!Book.mainfund)
            {
                Book.mainfund = false;
            }
            else
            {
                Book.mainfund = true;
            }
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..PreDescr where BARCODE = '" + Book.barcode + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t != 0)
            {
                Conn.SQLDA.UpdateCommand = new SqlCommand();
                Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
                Conn.SQLDA.UpdateCommand.Parameters.Add("name", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["name"].Value = Book.name ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("year", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["year"].Value = Book.year ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("number", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["number"].Value = Book.number ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("code", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["code"].Value = Book.code ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("mainfund", SqlDbType.Bit);
                Conn.SQLDA.UpdateCommand.Parameters["mainfund"].Value = Book.mainfund;
                Conn.SQLDA.UpdateCommand.Parameters.Add("addn", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["addn"].Value = Book.additionalNumbers ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("bar", SqlDbType.VarChar);
                Conn.SQLDA.UpdateCommand.Parameters["bar"].Value = Book.barcode ?? (object)DBNull.Value;
                Conn.SQLDA.UpdateCommand.Parameters.Add("id", SqlDbType.Int);
                Conn.SQLDA.UpdateCommand.Parameters["id"].Value = (int)DS.Tables[0].Rows[0]["ID"];

                Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..PreDescr set NAME = @name, " +
                                                       " YEAR = @year, NUMBER = @number," +
                                                       " CODE = @code,MAINFUND = @mainfund," +
                                                       " ADDNUMBERS = @addn," +
                                                       " BARCODE = @bar where ID = @id";
                if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    Conn.SQLDA.UpdateCommand.Connection.Open();
                }
                Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
                {
                    Conn.SQLDA.UpdateCommand.Connection.Close();
                }
            }
            else
            {

                Conn.SQLDA.InsertCommand = new SqlCommand();
                Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                Conn.SQLDA.InsertCommand.Parameters.Add("name", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["name"].Value = Book.name ?? (object)DBNull.Value;
                Conn.SQLDA.InsertCommand.Parameters.Add("year", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["year"].Value = Book.year ?? (object)DBNull.Value;
                Conn.SQLDA.InsertCommand.Parameters.Add("number", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["number"].Value = Book.number ?? (object)DBNull.Value; 
                Conn.SQLDA.InsertCommand.Parameters.Add("code", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["code"].Value = Book.code ?? (object)DBNull.Value;
                Conn.SQLDA.InsertCommand.Parameters.Add("mainfund", SqlDbType.Bit);
                Conn.SQLDA.InsertCommand.Parameters["mainfund"].Value = Book.mainfund;
                Conn.SQLDA.InsertCommand.Parameters.Add("addn", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["addn"].Value = Book.additionalNumbers ?? (object)DBNull.Value;
                Conn.SQLDA.InsertCommand.Parameters.Add("bar", SqlDbType.VarChar);
                Conn.SQLDA.InsertCommand.Parameters["bar"].Value = Book.barcode ?? (object)DBNull.Value;
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..PreDescr " +
                    "(NAME,YEAR,NUMBER,CODE,MAINFUND,BARCODE,ADDNUMBERS) values" +
                    "(@name,@year,@number,@code,@mainfund,@bar,@addn)";

                if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
                {
                    Conn.SQLDA.InsertCommand.Connection.Open();
                }
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                //Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
                {
                    Conn.SQLDA.InsertCommand.Connection.Close();
                }
            }
        }

        internal object GetBooksOnRESPANButNotEverIssued()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = F1.DepName;
            if (F1.DepName == "НИО редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText =
                    "with FC as  (select " +
                    "              A.ID id, " +
                    "              (case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else case when A.ISALLIG = 1 then CC.PLAIN + ' [Сплетено]' else CC.PLAIN end end) title, " +
                    "              DD.PLAIN avt,    " +
                    "              (case when A.INV = '-1' then A.BAR else A.INV end) inv, " +
                    "              A.DATESTART dt, " +
                    "              " + F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(A.BAR) as shi, " +
                    "              FF.PLAIN mhr, " +
                    "              A.BAR bar, A.IDREADER " +
                    " from " + F1.BASENAME + "..RecievedBooks A  " +
                    " inner join REDKOSTJ..LIST_8 DEP on A.DEPNAME = DEP.SHORTNAME  " +
                    " left join REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'  " +
                    " left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT  " +
                    " left join REDKOSTJ..DATAEXT D on A.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'  " +
                    " left join REDKOSTJ..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT   " +
                    " left join REDKOSTJ..DATAEXT BAR on BAR.SORT collate Cyrillic_General_CS_AI = A.BAR and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w'  " +
                    " left join REDKOSTJ..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = A.INV and INV.MNFIELD = 899 and INV.MSFIELD = '$p' and BAR.IDDATA = INV.IDDATA " +
                    " left join REDKOSTJ..DATAEXT F on A.IDMAIN = E.IDMAIN and F.MNFIELD = 899 and F.MSFIELD = '$a' and F.IDDATA = INV.IDDATA   " +
                    " left join REDKOSTJ..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT    " +
                    " left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR" +
                    " where A.DEPNAME = '" + F1.DepName + "' and  " +
                    "  A.RETINBK = 'false' and A.IDISSUED_OF is null and A.PFORBK is null ) " +
                    " select * from FC ";

            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText =
                    "with FC as  (select " +
                    "              A.ID id, " +
                    "              (case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else case when A.ISALLIG = 1 then CC.PLAIN + ' [Сплетено]' else CC.PLAIN end end) title, " +
                    "              DD.PLAIN avt,    " +
                    "              (case when A.INV = '-1' then A.BAR else A.INV end) inv, " +
                    "              A.DATESTART dt, " +
                    "              " + F1.BASENAME + ".dbo.GetSHIFRBJVVV(A.BAR) as shi, " +
                    "              FF.PLAIN mhr, " +
                    "              A.BAR bar, A.IDREADER " +
                    " from " + F1.BASENAME + "..RecievedBooks A  " +
                    " inner join BJVVV..LIST_8 DEP on A.DEPNAME = DEP.SHORTNAME  " +
                    " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'  " +
                    " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT  " +
                    " left join BJVVV..DATAEXT D on A.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'  " +
                    " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT   " +
                    " left join BJVVV..DATAEXT BAR on BAR.SORT collate Cyrillic_General_CS_AI = A.BAR and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w'  " +
                    " left join BJVVV..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = A.INV and INV.MNFIELD = 899 and INV.MSFIELD = '$p' and BAR.IDDATA = INV.IDDATA " +
                    " left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and F.IDDATA = INV.IDDATA   " +
                    " left join BJVVV..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT    " +
                    " left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR" +
                    " where A.DEPNAME = '" + F1.DepName + "' and  " +
                    "  A.RETINBK = 'false' and A.IDISSUED_OF is null and A.PFORBK is null ) " +
                    " select * from FC" ;
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal bool ProlongReservation(DateTime p,string zi)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..ISSUED_OF_ACTIONS where IDISSUED_OF = "+zi+" and IDACTION = 3";
            DataTable table = new DataTable();
            int cnt = Conn.SQLDA.Fill(table);
            if (cnt > 0)
            {
                MessageBox.Show("Заказ можно продлевать только один раз!");
                return true;
            }

            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;

            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF set DATE_RET = '" + p.ToString("yyyyMMdd") + "' " +
                                                   " where ID = " + zi;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }
            
            
            
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.Parameters.AddWithValue("IDACTION", 3);
            Conn.SQLDA.InsertCommand.Parameters.AddWithValue("IDUSER", F1.EmpID);
            Conn.SQLDA.InsertCommand.Parameters.AddWithValue("IDISSUED_OF", zi);
            Conn.SQLDA.InsertCommand.Parameters.AddWithValue("DATEACTION", DateTime.Now);

            Conn.SQLDA.InsertCommand.CommandText = "insert into "+F1.BASENAME+"..ISSUED_OF_ACTIONS (IDACTION,IDEMP,IDISSUED_OF,DATEACTION) values " +
                                                    "(@IDACTION,@IDUSER,@IDISSUED_OF,@DATEACTION)";
            Conn.SQLDA.InsertCommand.Connection.Open();
            Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            Conn.SQLDA.InsertCommand.Connection.Close();

            return false;
        }

        internal object GetIssBooksFromAnotherRespan()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (F1.DepName == "НИО редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID,(case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+(case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), " +
                                                       " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), " +
                                                       " A.IDREADER, A.RESPAN, A.DATE_RET, A.RLOCATION " +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join REDKOSTJ..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join REDKOSTJ..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join REDKOSTJ..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " where A.ZALISS = '" + dep + "' and A.IDMAIN != 0 and A.RESPAN != 'ДП' and A.RESPAN != cast(A.IDREADER as nvarchar(50)) and A.RESPAN!=''";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID,(case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+(case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), " +
                                                       " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), " +
                                                       " A.IDREADER, A.RESPAN, A.DATE_RET, A.RLOCATION " +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " where A.ZALISS = '" + dep + "' and A.IDMAIN != 0 and A.RESPAN != 'ДП' and A.RESPAN != cast(A.IDREADER as nvarchar(50)) and A.RESPAN!=''";
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetIssBooksWithExpiredReservation()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (F1.DepName == "НИО редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID,(case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+(case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), " +
                                                       " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), " +
                                                       " A.IDREADER, A.RESPAN, A.DATE_RET, A.RLOCATION, A.BAR bar " +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join REDKOSTJ..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join REDKOSTJ..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join REDKOSTJ..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join REDKOSTJ..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " where (((A.ZALISS = '" + F1.DepName + "') and (A.ZALRET is null)) or A.ZALRET ='" + F1.DepName + "') and A.DATE_RET < getdate() and A.RESPAN != 'ДП'";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID,(case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+(case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), " +
                                                       " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), " +
                                                       " A.IDREADER, A.RESPAN, A.DATE_RET, A.RLOCATION, A.BAR bar " +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " where (((A.ZALISS = '" + F1.DepName + "') and (A.ZALRET is null)) or A.ZALRET ='" + F1.DepName + "') and A.DATE_RET < getdate() and A.RESPAN != 'ДП'";
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal string getCountIssuedBooks()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = F1.DepName;
            Conn.SQLDA.SelectCommand.CommandText = " select count(A.ID)" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" + 
                                                   " where A.ZALISS = '" + F1.DepName + "' and A.IDMAIN != 0 and A.ATHOME = 0";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"].Rows[0][0].ToString();            
        }

        internal void InsertStatisticsRespan(string bar,string idr, dbBook b)
        {
            if (idr == null)
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),4," + F1.DepID + "," + idr + ",'" + bar + "',"+b.NumbersCount+")";//Принято издание на бронеолкку
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (принято на бронеполку)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsBookkeeping(string bar, string idr, dbBook b)
        {
            if ((idr == null) || (idr == ""))
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),5," + F1.DepID + "," + idr + ",'" + bar + "'," + b.NumbersCount + ")";//принято изданий от читателя для сдачи в книгохранение
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (принято изданий от читателя для сдачи в книгохранение)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
            //dbBook b = new dbBook(bar);
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..RecievedBooks A where " +
                " A.RETINBK = 'false' and A.IDISSUED_OF is null and A.PFORBK is null and A.BAR = '" + bar + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t != 0)//если ни разу не выдана
            {
                Conn.SQLDA.InsertCommand = new SqlCommand();
                Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,BAR) values" +
                    "(" + F1.EmpID + ",getdate(),10," + F1.DepID + ",'" + bar + "')";//изданий  для сдачи в книгохранение ни разу не выданных
                if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
                {
                    Conn.SQLDA.InsertCommand.Connection.Open();
                }
                try
                {
                    Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Не сработала статистика (Ни разу не выданных изданий сдано в книгохранение)");
                }
                if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
                {
                    Conn.SQLDA.InsertCommand.Connection.Close();
                }
            }



        }

        internal void InsertStatisticsIssuedBooks(string idr,dbBook statbook)
        {
            if (idr == null)
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (statbook.GetRealKlass() == "Выставка")
            {
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                    "(" + F1.EmpID + ",getdate(),8," + F1.DepID + "," + idr + ",'" + statbook.barcode + "'," + statbook.NumbersCount.ToString() + ")";//выдана книга c выставки
            }
            else
            {
                if (statbook.RESPAN == "ДП")
                {
                    Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                        "(" + F1.EmpID + ",getdate(),6," + F1.DepID + "," + idr + ",'" + statbook.barcode + "'," + statbook.NumbersCount.ToString() + ")";//выдана книга ДП
                }
                else
                {
                    Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                        "(" + F1.EmpID + ",getdate(),2," + F1.DepID + "," + idr + ",'" + statbook.barcode + "'," + statbook.NumbersCount.ToString() + ")";//выдана книга из книгохранения
                }
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (выдана книга)");
            } 
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER, BAR) values" +
                "(" + F1.EmpID + ",getdate(),3," + F1.DepID + "," + idr + ",'" + statbook.barcode + "')";//кол-во читателей
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (кол-во читателей)");
            } 
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsIssuedFromRespan(dbBook stat, string idr)
        {
            if (idr == null)
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),20," + F1.DepID + "," + idr + ",'" + stat.barcode + "'," + stat.NumbersCount.ToString() + ")";//выдана книга c бронеполки
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (выдано с бронеполки)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsReservationProlonged(dbBook stat, string idr)
        {
            if (idr == null)
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),21," + F1.DepID + "," + idr + ",'" + stat.barcode + "'," + stat.NumbersCount.ToString() + ")";//продлён срок бронирования
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (продлён срок бронирования)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }
        internal void InsertStatisticsDP(dbBook stat, string idr)
        {
            if (idr == null)
                idr = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (stat.GetRealKlass() == "Выставка")
            {
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                    "(" + F1.EmpID + ",getdate(),9," + F1.DepID + "," + idr + ",'" + stat.barcode + "',"+stat.NumbersCount+")";//принято издание выставка
            }
            else
            {
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                    "(" + F1.EmpID + ",getdate(),7," + F1.DepID + "," + idr + ",'" + stat.barcode + "',"+stat.NumbersCount+")";//принято издание ДП
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (принята книга)");
            } 
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsIssBookAtHome(dbBook statbook, dbReader ReaderRecord)
        {

            if (ReaderRecord.id == null)
                ReaderRecord.id = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            {
                if (statbook.get899a().ToLower().Contains("зал"))
                {
                    Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                        "(" + F1.EmpID + ",getdate(),14," + F1.DepID + "," + ReaderRecord.id + ",'" + statbook.barcode + "'," + statbook.NumbersCount.ToString() + ")";//выдана книга ДП
                }
                else
                {
                    Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                        "(" + F1.EmpID + ",getdate(),15," + F1.DepID + "," + ReaderRecord.id + ",'" + statbook.barcode + "'," + statbook.NumbersCount.ToString() + ")";//выдана книга из книгохранения
                }
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (выдана книга на дом)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER, BAR) values" +
                "(" + F1.EmpID + ",getdate(),3," + F1.DepID + "," + ReaderRecord.id + ",'" + statbook.barcode + "')";//кол-во читателей
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (кол-во читателей)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsRetBookAtHome(dbBook BookRecordWork)
        {
            if (BookRecordWork.rid == null)
                BookRecordWork.rid = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),17," + F1.DepID + "," + BookRecordWork.rid + ",'" + BookRecordWork.barcode + "'," + BookRecordWork.NumbersCount + ")";//Принято издание  с дома
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (принято с дома)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InsertStatisticsRetBookAtHomeDP(dbBook BookRecordWork)
        {
            if (BookRecordWork.rid == null)
                BookRecordWork.rid = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),16," + F1.DepID + "," + BookRecordWork.rid + ",'" + BookRecordWork.barcode + "'," + BookRecordWork.NumbersCount + ")";//Принято издание с дома ДП
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (принято с дома ДП)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }
        internal object GetNegotiabilityPIN()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.IDMAIN != -1" +
                   " group by A.IDMAIN" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.IDMAIN_CONST != -1" +
                   " group by B.IDMAIN_CONST" +
                   "  )," +
                   "  FG as(" +
                   "  select FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  " +
                   "  )" +
                   "  select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetNegotiabilityINV()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                     "   group by A.INV" +
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                      "  group by B.INV" +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                     "   " +
                     "   )" +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal DataTable GetNegotiabilityINV_ABONEMENT()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                     "   group by A.INV" +
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                      "  group by B.INV" +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                     "   " +
                     "   )" +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                     "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a'" +
                     "   where D.IDINLIST = 29 "+//'ЗалКООГруппаабонементногообслуживания'" +
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetAllBooksInHall()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            string dep = this.GetDepName(F1.DepID);

            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                    " group by A.INV" +
                    " )," +
                    " FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                    " group by B.INV" +
                    " )," +
                    " FG as(" +
                    " select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                    "  left join FCC on FC.INV=FCC.INV" +
                    " union" +
                    " select * from FCC)" +
                    " select B.ID,BB.PLAIN zag,CC.PLAIN avt,INV.SORT inv,STP.PLAIN stp, " +
                    " (case when status.IDMAIN = 0 or status.IDMAIN is null then 'свободно' else 'на руках у читателя' end) stat, " +
                    " (case when status.IDMAIN = 0 or status.IDMAIN is null then null else status.IDREADER end) as n, " +
                    " (case when FG.cnt is null then '0' else FG.cnt end) as cnt" +
                    "  from BJVVV..DATAEXT MAIN" +
                    " left join BJVVV..DATAEXT DEP on DEP.IDDATA = MAIN.IDDATA and DEP.MNFIELD = 899 and DEP.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN DEPPL on DEP.ID=DEPPL.IDDATAEXT " +
                    " left join BJVVV..DATAEXT INV on INV.IDDATA = MAIN.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$p'" +
                    " left join FG on FG.INV collate Cyrillic_General_CI_AI= INV.SORT" +
                    " left join BJVVV..DATAEXT B on INV.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                    " left join BJVVV..DATAEXT C on INV.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                    " left join BJVVV..DATAEXT ST on ST.IDDATA = INV.IDDATA and ST.MNFIELD = 899 and ST.MSFIELD = '$c'" +
                    " left join BJVVV..DATAEXTPLAIN STP on ST.ID = STP.IDDATAEXT" +
                    " left join " + F1.BASENAME + "..ISSUED_OF status on status.INV collate Cyrillic_General_CI_AI = INV.SORT " +
                    " where MAIN.MNFIELD = 921 and MAIN.MSFIELD = '$c' and MAIN.SORT = 'ДП'" +
                    " and DEPPL.PLAIN = '" + F1.DepName + "'" +
                    " order by cnt desc";
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetAllBooksInHall_forissue()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            string dep = this.GetDepName(F1.DepID);

            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                    " group by A.INV" +
                    " )," +
                    " FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                    " group by B.INV" +
                    " )," +
                    " FG as(" +
                    " select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                    "  left join FCC on FC.INV=FCC.INV" +
                    " union" +
                    " select * from FCC)" +
                    " select B.ID,BB.PLAIN zag,CC.PLAIN avt,INV.SORT inv,STP.PLAIN stp, " +
                    " (case when status.IDMAIN = 0 or status.IDMAIN is null then 'свободно' else 'на руках у читателя' end) stat, " +
                    " (case when status.IDMAIN = 0 or status.IDMAIN is null then null else status.IDREADER end) as n, " +
                    " (case when FG.cnt is null then '0' else FG.cnt end) as cnt" +
                    "  from BJVVV..DATAEXT MAIN" +
                    " left join BJVVV..DATAEXT DEP on DEP.IDDATA = MAIN.IDDATA and DEP.MNFIELD = 899 and DEP.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN DEPPL on DEP.ID=DEPPL.IDDATAEXT " +
                    " left join BJVVV..DATAEXT INV on INV.IDDATA = MAIN.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$p'" +
                    " left join FG on FG.INV collate Cyrillic_General_CI_AI= INV.SORT" +
                    " left join BJVVV..DATAEXT B on INV.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                    " left join BJVVV..DATAEXT C on INV.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                    " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                    " left join BJVVV..DATAEXT ST on ST.IDDATA = INV.IDDATA and ST.MNFIELD = 899 and ST.MSFIELD = '$c'" +
                    " left join BJVVV..DATAEXTPLAIN STP on ST.ID = STP.IDDATAEXT" +
                    " left join " + F1.BASENAME + "..ISSUED_OF status on status.INV collate Cyrillic_General_CI_AI = INV.SORT " +
                    " where MAIN.MNFIELD = 921 and MAIN.MSFIELD = '$c' and MAIN.SORT = 'Длявыдачи'" +
                    " and DEPPL.PLAIN = '" + F1.DepName + "'" +
                    " order by cnt desc";
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal bool IsRecievedInHall(dbBook b)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..RecievedBooks where BAR = '" + b.barcode +
                "' and RETINBK = 0";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return (t == 0) ? false : true;
        }

        internal void MoveToPREPBK(dbBook b, int idh)
        {
            if (b.RESPAN == "ДП")
                return;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..PREPBK where BAR = '" + b.barcode + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t != 0)
                return;
            if (b.id == "надовводить")
                b.id = "-1";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[PREPBK] (IDMAIN,DATEV,INV,BAR,DEPNAME,EMPID,LOCATION,RESPAN,IDHST) values" +
                "(" + b.id + ",getdate(),'" + b.inv + "','" + b.barcode + "','" + F1.DepName + "'," + F1.EmpID + ",'" + b.zal + "','" + b.RESPAN + "'," + idh + ")";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }


            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;

            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks set PFORBK = 1, IDHST =  " + idh.ToString()+
                                                   " where BAR = '" + b.barcode+"' and RETINBK != 1";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            //меняем статус на "сдано читателем для книгохранения"
            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 8 "+
                                                   " where ID_Book_EC = " + b.id + " and IDDATA = "+b.iddata;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            if (Conn.SQLDA.UpdateCommand.ExecuteNonQuery() == 0)//если это аллигат, то ищем по аллигату и по максимальному айди, так как инвентарь аллигата может отличаться от выдаваемой книги
            {
                Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 8 " +
                                                       " where ALGIDM = " + b.id + " and ID = (select max(ID) from Reservation_O..Orders where ALGIDM = " + b.id + ")";
                Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            }
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }
        }

        internal object GetPREPFORBKBooks()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (F1.DepName == "НИО редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText =
                  "with FC as (select A.ID id, " +
                  "(case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                  "CC.PLAIN avt, " +
                  "(case when A.INV = '-1' then '' else A.INV end) inv, " +
                  "(A.BAR), " +
                  "FF.PLAIN mhr, " +
                   F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(A.BAR) as shi,A.RESPAN respan" +
                  "from " + F1.BASENAME + "..PREPBK A " +
                  "left join REDKOSTJ..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                  "left join REDKOSTJ..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                  "left join REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                  "left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                  "left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                  "left join REDKOSTJ..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and E.SORT collate Cyrillic_General_CS_AI = A.BAR " +
                  "left join REDKOSTJ..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                  "left join REDKOSTJ..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                   "left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR     " +
                  "where A.DEPNAME = '" + F1.DepName + "'" +
                  "  ) " +
                  "select * from FC ";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText =
                  "with FC as (select A.ID id, " +
                  "(case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                  "CC.PLAIN avt, " +
                  "(case when A.INV = '-1' then '' else A.INV end) inv, " +
                  "(A.BAR), " +
                  "FF.PLAIN mhr, " +
                  F1.BASENAME + ".dbo.GetSHIFRBJVVV(E.SORT) as shi,A.RESPAN respan " +
                  "from " + F1.BASENAME + "..PREPBK A " +
                  "left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                  "left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                  "left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                  "left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                  "left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                  "left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and E.SORT collate Cyrillic_General_CS_AI = A.BAR " +
                  "left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                  "left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                  "left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR     " +
                  "where A.DEPNAME = '" + F1.DepName + "'" +
                  "  ) " +
                  "select * from FC ";
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal string ChangeHall(string _data)
        {
            dbBook b = new dbBook(_data, F1.BASENAME);
            if (b.RESPAN == "ДП")
                return "Эта книга на ДП! Для неё не создаются бронеполки!";
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..ISSUED_OF where BAR = '" + b.barcode + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 0)
            {
                Conn.SQLDA.SelectCommand.CommandText =
                    " select * from " + F1.BASENAME + "..RecievedBooks where BAR = '" + b.barcode + "' and RETINBK = 0";
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
                if (t == 0)
                {
                    return "книга не принята ни одним залом";
                }
                if (DS.Tables["t"].Rows[0]["DEPNAME"].ToString() == F1.DepName)
                {
                    return "Уже принята залом!";
                }
                Conn.SQLDA.UpdateCommand = new SqlCommand();
                Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;

                Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks set DEPNAME = '" + F1.DepName + "'" +
                                                       " where BAR = '" + b.barcode + "' and RETINBK = 0";
                if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    Conn.SQLDA.UpdateCommand.Connection.Open();
                }
                Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
                {
                    Conn.SQLDA.UpdateCommand.Connection.Close();
                }
                return "Для этой книги нет бронеполки ни в одном зале!";
            }
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..RecievedBooks where BAR = '" + b.barcode + "' and RETINBK = 0";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");

            if (DS.Tables["t"].Rows[0]["DEPNAME"].ToString() == F1.DepName)
            {
               // return "Уже принята залом!";
            }
            string DEP1 = DS.Tables["t"].Rows[0]["DEPNAME"].ToString();
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;

            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..ISSUED_OF set RLOCATION = '" + F1.DepName + "', ZALRET = '" + F1.DepName + "', ZALISS = '" + F1.DepName + "' " +
                                                   " where BAR = '" + b.barcode + "'";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;

            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks set DEPNAME = '" + F1.DepName + "'" +
                                                   " where BAR = '" + b.barcode + "' and RETINBK = 0";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.UpdateCommand.Connection.Close();
            }
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] " +
                " (IDEMP,DATEACTION,ACTIONTYPE,DEPID,BAR,DEP1,DEP2) values " +
                "(" + F1.EmpID + ",getdate(),11," + F1.DepID + ",'" + b.barcode + "','" + DEP1 + "','" + F1.DepName + "')";//изменен зал бронеполки
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика смены зала бронеполки!");
            }
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            return "Бронеполка книги успешно перенесена в текущий зал!";

        }

        internal object GetCurrentStatus(string inv,string id)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                 "with FA as (  " +
                 "select A.IDMAIN idmall, A.SORT invs,FF.PLAIN inv, D.PLAIN mhr,E.PLAIN klass,A.IDDATA iddata " +
                 "    from BJVVV..DATAEXT A  " +
                 "   left join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a'  " +
                 "   left join BJVVV..DATAEXT C on A.IDDATA = C.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c'  " +
                 "   left join BJVVV..DATAEXTPLAIN D on B.ID = D.IDDATAEXT   " +
                 "   left join BJVVV..DATAEXTPLAIN E on C.ID = E.IDDATAEXT   " +
                 "   left join BJVVV..DATAEXT F on A.IDDATA = F.IDDATA and F.MNFIELD = 899 and F.MSFIELD = '$p' " +
    	         "   left join BJVVV..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT " +
                 " where A.ID = "+ id +
                 "    ),  " +
                 " FC as (select A.ID id, " +
                 "BB.PLAIN zag, " +
                 "CC.PLAIN avt, " +
                 "FA.inv inv, " +
                 "E.SORT bar, " +
                 "FF.PLAIN mhr, " +
                 "(case when " + F1.BASENAME + ".dbo.GetSHIFRBJVVV(E.SORT) = '<нет>' then " + F1.BASENAME + ".dbo.GetSHIFRBJVVVINV(A.SORT) else " + F1.BASENAME + ".dbo.GetSHIFRBJVVV(E.SORT) end) as shi, " +
                 " FA.invs invs " +
                 " from BJVVV..DATAEXT A " +
                 " inner join FA FA on A.IDMAIN = FA.idmall  " +
                 " left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                 " left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                 " left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                 " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                 " left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and A.IDDATA = E.IDDATA " +
                 " left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and A.IDDATA = F.IDDATA " +
                 " left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                 " where A.MNFIELD = 899 and A.MSFIELD = '$p' and FA.iddata = A.IDDATA  " +
                 ") " +
                 " select * from FC ";
                 //"select FC.id,FC.zag,FC.avt,FC.inv,FC.bar,FC.mhr,FC.shi,FC.invs from FC where  FC.rfn = (select min(rfn) from FC)";

            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 0)
            {
                Conn.SQLDA.SelectCommand.CommandText =
                 "with FA as (  " +
                 "select A.IDMAIN idmall, A.SORT invs,FF.PLAIN inv,D.PLAIN mhr,E.PLAIN klass,A.IDDATA iddata " +
                 "    from REDKOSTJ..DATAEXT A  " +
                 "   left join REDKOSTJ..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a'  " +
                 "   left join REDKOSTJ..DATAEXT C on A.IDDATA = C.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c'  " +
                 "   left join REDKOSTJ..DATAEXTPLAIN D on B.ID = D.IDDATAEXT   " +
                 "   left join REDKOSTJ..DATAEXTPLAIN E on C.ID = E.IDDATAEXT   " +
                 "   left join REDKOSTJ..DATAEXT F on A.IDDATA = F.IDDATA and F.MNFIELD = 899 and F.MSFIELD = '$p' " +
                 "   left join REDKOSTJ..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT " +
                 "    where A.IDDATA =  " +
                 "    (  " +
                 "       select A.IDDATA from REDKOSTJ..DATAEXTPLAIN C  " +
                 "       left join REDKOSTJ..DATAEXT A on C.IDDATAEXT = A.ID " +
                 "       where  " +
                 "        not exists  " +
                 "       (select 1 from REDKOSTJ..DATAEXT B where A.IDMAIN=B.IDMAIN and B.MNFIELD = 482 and B.MSFIELD = '$a')  " +
                 "       and " +
                 "       exists " +
                 "       (select 1 from REDKOSTJ..DATAEXT B where A.IDMAIN=B.IDMAIN and B.MNFIELD = 899 and B.MSFIELD = '$w')  " +
                 "        and  " +
//                 "       (C.PLAIN = '" + bar + "' ) and A.MNFIELD = 899 and A.MSFIELD = '$w' " +
                 " A.ID = " + id +
                 "    )  " +
                 "    and A.MNFIELD = 899 and A.MSFIELD = '$w' ), " +
                 "FC as (select A.ID id, " +
                 "BB.PLAIN zag, " +
                 "CC.PLAIN avt, " +
                 "A.SORT inv, " +
                 "E.SORT, " +
                 "FF.PLAIN mhr, " +
                 "(case when " + F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(E.SORT) = '<нет>' then " + F1.BASENAME + ".dbo.GetSHIFRREDKOSTJINV(A.SORT) else " + F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(E.SORT) end) as shi, " +
                 " FA.invs " +
                 " from REDKOSTJ..DATAEXT A " +
                 " inner join FA FA on A.IDMAIN = FA.idmall  " +
                 " left join REDKOSTJ..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                 " left join REDKOSTJ..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                 " left join REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                 " left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                 " left join REDKOSTJ..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and A.IDDATA = E.IDDATA " +
                 " left join REDKOSTJ..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and A.IDDATA = F.IDDATA " +
                 " left join REDKOSTJ..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                 " where A.MNFIELD = 899 and A.MSFIELD = '$p' and FA.iddata = A.IDDATA  " +
                 ") " +
                 "select * from FC";
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
            }
            if (t == 0)
            {//искать в преописании
                Conn.SQLDA.SelectCommand.CommandText =
                    " select 'Возможно не в базе' idmall, 'Возможно не в базе' inv,'Возможно не в базе' mhr,'Возможно не в базе' klass";
                t = Conn.SQLDA.Fill(DS, "t");
            }
            return DS.Tables["t"];
        }

        internal DataTable GetCurrentIssueByINV(string inv)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.*,B.[NAME] from " + F1.BASENAME + "..ISSUED_OF A" +
                " left join BJVVV..USERS B on A.IDEMP = B.ID"+
                " where A.INV = '" + inv + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetPREPBKByINV(string inv)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.*,B.[NAME] from " + F1.BASENAME + "..PREPBK A" +
                " left join BJVVV..USERS B on A.EMPID = B.ID" +
                " where A.INV = '" + inv + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetHistOfINV(string inv)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.DATE_ISSUE,C.DATEFINISH,A.IDREADER,B.[NAME],A.ZALRET,C.RECDEPNAME from " + F1.BASENAME + "..ISSUED_OF_HST A" +
                " left join BJVVV..USERS B on A.IDEMP = B.ID" +
                " left join " + F1.BASENAME + "..RecievedBooks C on C.IDHST = A.ID" +
                " where A.BAR = '" + inv + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void ReturnOnRESPAN(dbBook b)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + b.barcode + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            string IDHST = "";
            if (DS.Tables[0].Rows[0]["IDHST"].ToString() == "-1")//если ни разу не выдавалась
            {
                this.ReturnOn_RESPAN_NOT_EVER_ISSUED(b);
                return;
            }
            if ((t == 0) || (DS.Tables[0].Rows[0]["IDHST"].ToString() == ""))
            {
                //не найдено в подготовленных к сдаче
                this.ReturnOnRESPAN_WITHOUT_IDHIST(b);
            }
            else
            {
                IDHST = DS.Tables[0].Rows[0]["IDHST"].ToString();
                this.ReturnOnRESPAN_WITH_IDHIST(b, IDHST);
            }
            
        }
        internal void ReturnOnRESPANForNewReader(dbBook b)
        {
            this.ReturnOn_RESPAN_NOT_EVER_ISSUED(b);
        }
        private void ReturnOn_RESPAN_NOT_EVER_ISSUED(dbBook b)
        {
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                                                   " set IDISSUED_OF = null, PFORBK = null, IDREADER = 0, IDHST  =null,DEPNAME = '" +F1.DepName +"'"+
                                                   " where BAR = '" + b.barcode + "' and RETINBK=0";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            Conn.SQLDA.UpdateCommand.Connection.Close();

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.DeleteCommand.CommandText = "delete from Reservation_O..Orders "+
                                                   " where ID_Book_EC = " + b.id + " and IDDATA = "+b.iddata;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

        }

        private void ReturnOnRESPAN_WITH_IDHIST(dbBook b, string IDHST)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..ISSUED_OF_HST " +
                " where ID = " + IDHST;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }


            SqlCommand cmd = new SqlCommand();
            Conn.SQLDA.InsertCommand = new SqlCommand();
            cmd.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();
            cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF (IDMAIN,BAR,DATE_RET,IDREADER,IDEMP,DATE_ISSUE,DATE_CHG_KLASS, IDMAIN_CONST, " +
                                                    " INV, ZALISS,ZALRET, RESPAN, KLASS, RLOCATION,IDDATA) values (@IDMAIN,@BAR,@DATE_RET,@IDREADER,@IDEMP,@DATE_ISSUE,@DATE_CHG_KLASS,@IDMAIN_CONST, " +
                                                    " @INV,  @ZALISS,@ZALRET, @RESPAN, @KLASS, @RLOCATION,@IDDATA)";
            cmd.Parameters.Add("IDMAIN", SqlDbType.Int);
            cmd.Parameters.Add("BAR", SqlDbType.NVarChar);
            cmd.Parameters.Add("DATE_RET", SqlDbType.DateTime);
            cmd.Parameters.Add("IDREADER", SqlDbType.Int);
            cmd.Parameters.Add("IDEMP", SqlDbType.Int);
            cmd.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime);
            cmd.Parameters.Add("DATE_CHG_KLASS", SqlDbType.DateTime);
            cmd.Parameters.Add("IDMAIN_CONST", SqlDbType.NVarChar);
            cmd.Parameters.Add("INV", SqlDbType.NVarChar);
            cmd.Parameters.Add("ZALISS", SqlDbType.NVarChar);
            cmd.Parameters.Add("ZALRET", SqlDbType.NVarChar);
            cmd.Parameters.Add("RESPAN", SqlDbType.NVarChar);
            cmd.Parameters.Add("KLASS", SqlDbType.NVarChar);
            cmd.Parameters.Add("RLOCATION", SqlDbType.NVarChar);
            cmd.Parameters.Add("IDDATA", SqlDbType.Int);

            cmd.Parameters["IDMAIN"].Value = 0;
            cmd.Parameters["BAR"].Value = b.barcode;
            cmd.Parameters["DATE_RET"].Value = DS.Tables[0].Rows[0]["DATE_RET"];
            cmd.Parameters["IDREADER"].Value = DS.Tables[0].Rows[0]["IDREADER"].ToString();
            cmd.Parameters["IDEMP"].Value = DS.Tables[0].Rows[0]["IDEMP"].ToString();
            cmd.Parameters["DATE_CHG_KLASS"].Value = b.ChgKlass;
            cmd.Parameters["DATE_ISSUE"].Value = DS.Tables[0].Rows[0]["DATE_ISSUE"];
            cmd.Parameters["IDMAIN_CONST"].Value = b.id;
            cmd.Parameters["INV"].Value = b.inv;
            cmd.Parameters["ZALISS"].Value = F1.DepName;
            cmd.Parameters["ZALRET"].Value = F1.DepName;
            cmd.Parameters["RESPAN"].Value = DS.Tables[0].Rows[0]["RESPAN"];
            cmd.Parameters["KLASS"].Value = b.klass;
            cmd.Parameters["RLOCATION"].Value = F1.DepName;
            cmd.Parameters["IDDATA"].Value = int.Parse(b.iddata);
            cmd.CommandText += ";select cast(scope_identity() as int)";
            object p = cmd.ExecuteScalar();
            int ident = (int)p;
            cmd.Connection.Close();

            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                                                   " set IDISSUED_OF = " + ident.ToString() + ", PFORBK = null, IDHST  =null"+
                                                   " where BAR = '" + b.barcode + "' and RETINBK=0";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            Conn.SQLDA.UpdateCommand.Connection.Close();

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF_HST " +
                " where ID = " + IDHST;
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 3 " +
                                                   " where ID_Book_EC = " + b.id + " and IDDATA = " + b.iddata;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
        }

        private void ReturnOnRESPAN_WITHOUT_IDHIST(dbBook b)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select top(1) *"+
                " from " + F1.BASENAME + "..ISSUED_OF_HST  where BAR = '" + b.barcode + "'" +
                " and DATE_ISSUE = (select max(DATE_ISSUE) from " + F1.BASENAME + "..ISSUED_OF_HST where BAR = '" + b.barcode + "')" +
                " order by ID desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            string IDHST = DS.Tables[0].Rows[0]["ID"].ToString();

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }


            SqlCommand cmd = new SqlCommand();
            Conn.SQLDA.InsertCommand = new SqlCommand();
            cmd.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open) Conn.ZakazCon.Open();

            cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF (IDMAIN,BAR,DATE_RET,IDREADER,IDEMP,DATE_ISSUE,DATE_CHG_KLASS, IDMAIN_CONST, " +
                                                    " INV, ZALISS,ZALRET, RESPAN, KLASS, RLOCATION,IDDATA) values (@IDMAIN,@BAR,@DATE_RET,@IDREADER,@IDEMP,@DATE_ISSUE,@DATE_CHG_KLASS,@IDMAIN_CONST, " +
                                                    " @INV,  @ZALISS,@ZALRET, @RESPAN, @KLASS, @RLOCATION, @IDDATA)";
            cmd.Parameters.Add("IDMAIN", SqlDbType.Int);
            cmd.Parameters.Add("BAR", SqlDbType.NVarChar);
            cmd.Parameters.Add("DATE_RET", SqlDbType.DateTime);
            cmd.Parameters.Add("IDREADER", SqlDbType.Int);
            cmd.Parameters.Add("IDEMP", SqlDbType.Int);
            cmd.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime);
            cmd.Parameters.Add("DATE_CHG_KLASS", SqlDbType.DateTime);
            cmd.Parameters.Add("IDMAIN_CONST", SqlDbType.NVarChar);
            cmd.Parameters.Add("INV", SqlDbType.NVarChar);
            cmd.Parameters.Add("ZALISS", SqlDbType.NVarChar);
            cmd.Parameters.Add("ZALRET", SqlDbType.NVarChar);
            cmd.Parameters.Add("RESPAN", SqlDbType.NVarChar);
            cmd.Parameters.Add("KLASS", SqlDbType.NVarChar);
            cmd.Parameters.Add("RLOCATION", SqlDbType.NVarChar);
            cmd.Parameters.Add("IDDATA", SqlDbType.Int);

            cmd.Parameters["IDMAIN"].Value = 0;
            cmd.Parameters["BAR"].Value = b.barcode;
            cmd.Parameters["DATE_RET"].Value = DS.Tables[0].Rows[0]["DATE_RET"];
            cmd.Parameters["IDREADER"].Value = DS.Tables[0].Rows[0]["IDREADER"].ToString();
            cmd.Parameters["IDEMP"].Value = DS.Tables[0].Rows[0]["IDEMP"].ToString();
            cmd.Parameters["DATE_CHG_KLASS"].Value = b.ChgKlass;
            cmd.Parameters["DATE_ISSUE"].Value = DS.Tables[0].Rows[0]["DATE_ISSUE"];
            cmd.Parameters["IDMAIN_CONST"].Value = b.id;
            cmd.Parameters["INV"].Value = b.inv;
            cmd.Parameters["ZALISS"].Value = F1.DepName;
            cmd.Parameters["ZALRET"].Value = F1.DepName;
            cmd.Parameters["RESPAN"].Value = DS.Tables[0].Rows[0]["RESPAN"];
            cmd.Parameters["KLASS"].Value = b.klass;
            cmd.Parameters["RLOCATION"].Value = F1.DepName;
            cmd.Parameters["IDDATA"].Value = int.Parse(b.iddata);
            cmd.CommandText += ";select cast(scope_identity() as int)";
            object p = cmd.ExecuteScalar();
            int ident = (int)p;
            cmd.Connection.Close();

            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                                                   " set IDISSUED_OF = " + ident.ToString() + ", PFORBK = null, IDHST  =null"+
                                                   " where BAR = '" + b.barcode + "' and RETINBK=0";
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            Conn.SQLDA.UpdateCommand.Connection.Close();

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                " where BAR = '" + b.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF_HST " +
                " where ID = " + IDHST;
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_O..Orders set Status = 3 " +
                                                   " where ID_Book_EC = " + b.id + " and IDDATA = " + b.iddata;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
        }

        internal DataTable GetDeltaISS_REC(string filter, DateTime start,DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            if (filter == "…Хран… КНИО Группа хранения редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText =
                "with " +
                "FG1 as (select A.ID,(case when CC.PLAIN is null then 'Не в базе' else CC.PLAIN end) CC,A.INV,A.DEPNAME,A.IDMAIN,A.BAR,A.DATESTART " +
                "from " + F1.BASENAME + "..RecievedBooks A " +
                " left join REDKOSTJ..DATAEXT B on A.INV collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$p' and A.IDMAIN = B.IDMAIN " +
                " left join REDKOSTJ..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                " left join REDKOSTJ..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                " left join " + F1.BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RECDEPNAME is not null  " +
                                " and (D.DATESTART between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") +
                                "'  or D.DATEFINISH between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "') " +
                " where  " +
                "         (A.DATESTART between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'  or  " +
                "         A.DATEFINISH between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "') " +
                "             and A.RETINBK = 0 and A.STARTMHR = '" + filter + "'" +
                "), " +
                "FG as (select * from FG1 where CC = '" + filter + "')," +
                " FC as " +
                "(select A.ID id, " +
                "(case when A.INV = '-1' then D.[NAME]+'; '+D.[YEAR] +'; ' +D.NUMBER + '; '+ " +
                "(case when D.ADDNUMBERS is null then '' else D.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                "CC.PLAIN avt, " +
                "(case when A.INV = '-1' then D.BARCODE else A.INV end) inv, " +
                "(A.DEPNAME), " +
                "FF.PLAIN mhr, " +
                F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(A.BAR) as shi, " +
                "A.DATESTART, " +
                "A.BAR bar " +
                " from FG A " +
                "  left join REDKOSTJ..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                "  left join REDKOSTJ..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                "  left join REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                "  left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                "  left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                "  left join REDKOSTJ..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$p' and E.SORT collate Cyrillic_General_CS_AI = A.INV and A.IDMAIN = E.IDMAIN " +
                "  left join REDKOSTJ..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                "  left join REDKOSTJ..DATAEXT BAR on BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and E.IDDATA = BAR.IDDATA " +
                "  left join REDKOSTJ..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                "), FE as " +
                " (select FC.*, (row_number() over ( partition by FC.bar order by FC.bar)) as bnum " +
                " from FC ) select * from FE A where A.bnum in (select max(B.bnum) from FE B group by B.bar ) ";
            }
            else
            {
                if (filter == "Не в базе")
                {
                    Conn.SQLDA.SelectCommand.CommandText =
                    "with " +
                    "FG1 as (select A.ID,(case when CC.PLAIN is null then 'Не в базе' else CC.PLAIN end) CC,A.INV,A.DEPNAME,A.IDMAIN,A.BAR,A.DATESTART " +
                    "from " + F1.BASENAME + "..RecievedBooks A " +
                    " left join BJVVV..DATAEXT B on A.INV collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$p' and A.IDMAIN = B.IDMAIN " +
                    " left join BJVVV..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                    " left join BJVVV..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                        //" left join " + F1.BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RECDEPNAME is not null  " +
                    " where  " +
                    "         (A.DATESTART between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'  or  " +
                    "         A.DATEFINISH between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "') " +
                    "             and A.RETINBK = 0 and A.DEPNAME != 'НИО редкой книги'" +
                    "), " +
                    "FG as (select * from FG1 where CC = '" + filter + "' and FG1.DEPNAME != 'НИО редкой книги')," +

                    " FC as " +
                    "(select A.ID id, " +
                    "(case when A.INV = '-1' then D.[NAME]+'; '+D.[YEAR] +'; ' +D.NUMBER + '; '+ " +
                    "(case when D.ADDNUMBERS is null then '' else D.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                    "CC.PLAIN avt, " +
                    "(case when A.INV = '-1' then A.BAR else A.INV end) inv, " +
                    "(A.DEPNAME), " +
                    "FF.PLAIN mhr, " +
                     F1.BASENAME + ".dbo.GetSHIFRBJVVV(A.BAR) as shi, " +
                    "A.DATESTART, " +
                        "A.BAR bar " +
                        " from FG A " +
                        "  left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                        "  left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                        "  left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                        "  left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$p' and E.SORT collate Cyrillic_General_CS_AI = A.INV and A.IDMAIN = E.IDMAIN " +
                        "  left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                        "  left join BJVVV..DATAEXT BAR on BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and E.IDDATA = BAR.IDDATA " +
                        "  left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                        "), FE as " +
                        " (select FC.*, (row_number() over ( partition by FC.bar order by FC.bar)) as bnum " +
                        " from FC ) select * from FE A where A.bnum in (select max(B.bnum) from FE B group by B.bar ) ";
                }
                else
                {
                    if (filter == "Всего")
                    {
                        Conn.SQLDA.SelectCommand.CommandText =
                        "with " +
                        "FG1 as (select A.ID,(case when CC.PLAIN is null then 'Не в базе' else CC.PLAIN end) CC,A.INV,A.DEPNAME,A.IDMAIN,A.BAR,A.DATESTART " +
                        "from " + F1.BASENAME + "..RecievedBooks A " +
                        " left join BJVVV..DATAEXT B on A.INV collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$p' and A.IDMAIN = B.IDMAIN " +
                        " left join BJVVV..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                        " left join BJVVV..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                        " left join " + F1.BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RECDEPNAME is not null  " +
                        " where  " +
                        "         (A.DATESTART between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'  or  " +
                        "         A.DATEFINISH between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "') " +
                        "             and A.RETINBK = 0 " +
                        "), " +
                        "FG as (select * from FG1 where FG1.DEPNAME != 'НИО редкой книги')," +

                        " FC as " +
                        "(select A.ID id, " +
                        "(case when A.INV = '-1' then D.[NAME]+'; '+D.[YEAR] +'; ' +D.NUMBER + '; '+ " +
                        "(case when D.ADDNUMBERS is null then '' else D.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                        "CC.PLAIN avt, " +
                        "(case when A.INV = '-1' then D.BARCODE else A.INV end) inv, " +
                        "(A.DEPNAME), " +
                        "FF.PLAIN mhr, " +
                        F1.BASENAME + ".dbo.GetSHIFRBJVVV(A.BAR) as shi, " +
                        "A.DATESTART, " +
                        "A.BAR bar " +
                        " from FG A " +
                        "  left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                        "  left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                        "  left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                        "  left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$p' and E.SORT collate Cyrillic_General_CS_AI = A.INV and A.IDMAIN = E.IDMAIN " +
                        "  left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                        "  left join BJVVV..DATAEXT BAR on BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and E.IDDATA = BAR.IDDATA " +
                        "  left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                        "), FE as " +
                        " (select FC.*, (row_number() over ( partition by FC.bar order by FC.bar)) as bnum " +
                        " from FC ) select * from FE A where A.bnum in (select max(B.bnum) from FE B group by B.bar ) ";

                    }
                    else
                    {
                        Conn.SQLDA.SelectCommand.CommandText =
                        "with " +
                        "FG1 as (select A.ID,(case when CC.PLAIN is null then 'Не в базе' else CC.PLAIN end) CC,A.INV,A.DEPNAME,A.IDMAIN,A.BAR,A.DATESTART " +
                        "from " + F1.BASENAME + "..RecievedBooks A " +
                        " left join BJVVV..DATAEXT B on A.INV collate Cyrillic_General_CS_AI = B.SORT and B.MNFIELD = 899 and B.MSFIELD = '$p' and A.IDMAIN = B.IDMAIN " +
                        " left join BJVVV..DATAEXT C on B.IDDATA = C.IDDATA and C.MNFIELD = 899 and C.MSFIELD = '$a' " +
                        " left join BJVVV..DATAEXTPLAIN CC on CC.IDDATAEXT = C.ID " +
                        " left join " + F1.BASENAME + "..RecievedBooks D on A.ID = D.ID and D.RECDEPNAME is not null  " +
                        " where  " +
                        "         (A.DATESTART between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'  or  " +
                        "         A.DATEFINISH between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "') " +
                        "             and A.RETINBK = 0 and A.STARTMHR = '" + filter + "'" +
                        "), " +
                        "FG as (select * from FG1 where FG1.DEPNAME != 'НИО редкой книги')," +

                        " FC as " +
                        "(select A.ID id, " +
                        "(case when A.INV = '-1' then D.[NAME]+'; '+D.[YEAR] +'; ' +D.NUMBER + '; '+ " +
                        "(case when D.ADDNUMBERS is null then '' else D.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                        "CC.PLAIN avt, " +
                        "(case when A.INV = '-1' then D.BARCODE else A.INV end) as inv, " +
                        "(A.DEPNAME), " +
                        "FF.PLAIN mhr, " +
                         F1.BASENAME + ".dbo.GetSHIFRBJVVV(A.BAR) as shi, " +
                        "A.DATESTART, " +
                        "A.BAR bar " +
                        " from FG A " +
                        "  left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                        "  left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                        "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                        "  left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                        "  left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$p' and E.SORT collate Cyrillic_General_CS_AI = A.INV and A.IDMAIN = E.IDMAIN " +
                        "  left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                        "  left join BJVVV..DATAEXT BAR on BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and E.IDDATA = BAR.IDDATA " +
                        "  left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                        "), FE as " +
                        " (select FC.*, (row_number() over ( partition by FC.bar order by FC.bar)) as bnum " +
                        " from FC ) select * from FE A where A.bnum in (select max(B.bnum) from FE B group by B.bar ) ";
                    }
                }
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void InsertStatisticsIssuedBooksAnotherReader(string readerid, dbBook StatBook)
        {
            if (readerid == null)
                readerid = "0";
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + 
                    "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                    "(" + F1.EmpID + ",getdate(),13," + F1.DepID + "," + readerid + ",'" + StatBook.barcode + 
                    "'," + StatBook.NumbersCount.ToString() + ")";//выдана с чужой бронеполки
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (выдана книга)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER, BAR) values" +
                "(" + F1.EmpID + ",getdate(),3," + F1.DepID + "," + readerid + ",'" + StatBook.barcode + "')";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (кол-во читателей)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal int getCountIss(dbBook BookRecordWork)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..[Statistics] where ID = " +
                " (select max(ID) from "+F1.BASENAME+"..[Statistics] where BAR = '" + BookRecordWork.barcode + "' " +
                " and (ACTIONTYPE = 2 or ACTIONTYPE = 6 or ACTIONTYPE = 8 or ACTIONTYPE = 13)  )";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 0)
            {
                //MessageBox.Show("1.Не сработала статистика обратитесь к разработчику.");
                return 0;
            }
            object ret = DS.Tables["t"].Rows[0]["COUNTISS"];
            string str = DS.Tables["t"].Rows[0]["COUNTISS"].ToString();
            if (ret == null)
            {
                MessageBox.Show("2.Не сработала статистика обратитесь к разработчику.");
                return 1;
            }
            Int16 r = (Int16)ret;
            return (int)r;
        }

        internal object GetBooksForCurrentFloor()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (F1.DepName == "…Хран… КНИО Группа хранения редкой книги")
            {
                Conn.SQLDA.SelectCommand.CommandText =
                  "with FC as (select A.ID id, " +
                  "(case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                  "CC.PLAIN avt, " +
                  "(case when A.INV = '-1' then '' else A.INV end) inv, " +
                  "(A.BAR), " +
                  "FF.PLAIN mhr, " +
                   F1.BASENAME + ".dbo.GetSHIFRREDKOSTJ(A.BAR) as shi " +
                  "from " + F1.BASENAME + "..PREPBK A " +
                  "left join REDKOSTJ..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                  "left join REDKOSTJ..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                  "left join REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                  "left join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                  "left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                  "left join REDKOSTJ..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and E.SORT collate Cyrillic_General_CS_AI = A.BAR " +
                  "left join REDKOSTJ..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                  "left join REDKOSTJ..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                  "left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR     " +
                  "where A.LOCATION = '" + F1.DepName + "'" +
                  "  ) " +
                  "  select * from FC ";

            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText =
                  "with FC as (select A.ID id, " +
                  "(case when A.INV = '-1' then PRE.[NAME]+'; '+PRE.[YEAR] +'; ' +PRE.NUMBER + '; '+(case when PRE.ADDNUMBERS is null then '' else PRE.ADDNUMBERS end) else BB.PLAIN end) zag, " +
                  "CC.PLAIN avt, " +
                  "(case when A.INV = '-1' then '' else A.INV end) inv, " +
                  "(A.BAR), " +
                  "FF.PLAIN mhr, " +
                  F1.BASENAME + ".dbo.GetSHIFRBJVVV(A.BAR) as shi " +
                  "from " + F1.BASENAME + "..PREPBK A " +
                  "left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                  "left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                  "left join BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a' " +
                  "left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                  "left join " + F1.BASENAME + "..PreDescr D on A.BAR = D.BARCODE " +
                  "left join BJVVV..DATAEXT E on E.MNFIELD = 899 and E.MSFIELD = '$w' and E.SORT collate Cyrillic_General_CS_AI = A.BAR " +
                  "left join BJVVV..DATAEXT F on F.MNFIELD = 899 and F.MSFIELD = '$a' and E.IDDATA = F.IDDATA " +
                  "left join BJVVV..DATAEXTPLAIN FF on FF.IDDATAEXT = F.ID " +
                  "left join " + F1.BASENAME + "..PreDescr PRE on PRE.BARCODE = A.BAR     " +
                  "where A.LOCATION = '" + F1.DepName + "'" +
                  "  ) " +
                  "  select * from FC ";
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataSet CheckForTwo(string inv)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                "select A.IDMAIN,EE.PLAIN,B.SORT bar,C.SORT,A.ID from BJVVV..DATAEXT A " +
                "left join BJVVV..DATAEXT E on E.IDMAIN = A.IDMAIN and E.MNFIELD = 200 and E.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN EE on EE.IDDATAEXT = E.ID  " +
                "left join BJVVV..DATAEXT B on B.IDDATA = A.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                "left join BJVVV..DATAEXT C on C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = A.ID  " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$p' and D.PLAIN = '" + inv + "'"; 
                //" and not exists (select * from BJVVV..DATAEXT EX where EX.MNFIELD = 482 and EX.MSFIELD = '$a' and A.IDDATA = EX.IDDATA)";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 0)
            {
                Conn.SQLDA.SelectCommand.CommandText =
                    "select A.IDMAIN,EE.PLAIN,B.SORT,C.SORT,A.ID from REDKOSTJ..DATAEXT A " +
                    "left join REDKOSTJ..DATAEXT E on E.IDMAIN = A.IDMAIN and E.MNFIELD = 200 and E.MSFIELD = '$a' " +
                    "left join REDKOSTJ..DATAEXTPLAIN EE on EE.IDDATAEXT = E.ID  " +
                    "left join REDKOSTJ..DATAEXT B on B.IDDATA = A.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$w' " +
                    "left join REDKOSTJ..DATAEXT C on C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$a' " +
                    "left join REDKOSTJ..DATAEXTPLAIN D on D.IDDATAEXT = A.ID  " +
                    "where A.MNFIELD = 899 and A.MSFIELD = '$p' and D.PLAIN = '" + inv + "'";
                t = Conn.SQLDA.Fill(DS, "t");
            }
            return DS;
        }

        internal DataSet GetCurrentPlace(string p)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.DEPNAME,B.[NAME],A.DATESTART from " + F1.BASENAME + "..RecievedBooks A" +
                " left join BJVVV..USERS B on A.IDEMP = B.ID" +
                " where A.INV = '" + p + "' and (A.RETINBK = 'false' or A.RETINBK is null) and (A.PFORBK = 'false' or A.PFORBK is null) ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS;
        }

        internal DataTable getFreeServices()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.FSNAME,case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then '0' else sum(B.AMOUNT) end from " + F1.BASENAME + "..FREESER_LIST A " +
                " left join " + F1.BASENAME + "..FREESERVICE B on A.ID = B.IDFREELIST and cast(cast(B.DATESERVICE as varchar(11)) as datetime) = '" + DateTime.Today.ToString("yyyyMMdd") + "' and B.IDDEP = " + F1.DepID+
                " group by A.ID,A.FSNAME";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable getInputFreeServices()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.FSNAME,0,A.ID from " + F1.BASENAME + "..FREESER_LIST A";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void InputFreeService(string idfree, string amount)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME +
                "..[FREESERVICE] (IDFREELIST,AMOUNT,DATESERVICE,IDEMP,IDDEP) values" +
                "(" + idfree +
                      ", " + amount +
                      ", '" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") +
                      "'," + F1.EmpID +
                      ", " + F1.DepID +
                      ")";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработало добавление услуги");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void AddFreeService(string p)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME +
                "..[FREESER_LIST] (FSNAME) values" +
                "('" + p +  "')";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработало добавление услуги");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void InputPaidService(string id, string amount)
        {
            int price;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select PRICE from " + F1.BASENAME + "..PAIDSER_LIST A where ID = "+id;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            price = int.Parse(DS.Tables["t"].Rows[0]["PRICE"].ToString());
            

            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME +
                "..[PAIDSERVICE] (IDPAIDLIST,AMOUNT,DATESERVICE,IDEMP,IDDEP,PRICE) values" +
                "(" + id +
                      ", " + amount +
                      ", '" + DateTime.Today.ToString("yyyyMMdd") +
                      "'," + F1.EmpID +
                      ", " + F1.DepID +
                      ", " + price.ToString()  +
                      ")";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не сработало добавление услуги"+ex.Message);
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal void AddPaidService(string name, string price)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME +
                "..[PAIDSER_LIST] (PSNAME,PRICE) values" +
                "('" + name + "',"+price+")";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработало добавление услуги");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal DataTable getPaidServices()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " with FA as( select A.ID id,"+
                " A.PSNAME nam,"+
                " (case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then 0 else sum(B.AMOUNT) end) cnt, "+
                " A.PRICE pri, " +
                " (case when B.PRICE*sum(B.AMOUNT) = 0 or B.PRICE*sum(B.AMOUNT) is null then 0 else B.PRICE*sum(B.AMOUNT) end) cst," +
                " A.ID idt " +
                " from " + F1.BASENAME + "..PAIDSER_LIST A " +
                " left join " + F1.BASENAME + "..PAIDSERVICE B on A.ID = B.IDPAIDLIST and B.DATESERVICE = '" + DateTime.Today.ToString("yyyyMMdd") + "' and B.IDDEP = " + F1.DepID +
                " group by A.ID,A.PSNAME,A.PRICE,B.PRICE"+
                " ) "+
                " select id,nam,sum(cnt),pri,sum(cst),idt from FA "+
                " group by id,nam,pri,idt "+
                //" having sum(cnt) != 0" +
                " order by id";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object getInputPaidServices(string id)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.PSNAME,0,A.ID from " + F1.BASENAME + "..PAIDSER_LIST A where A.ID = "+id;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable getFreeServicesEdit()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.FSNAME,A.ID from " + F1.BASENAME + "..FREESER_LIST A ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object getPaidServicesEdit()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.PSNAME,A.PRICE,A.ID from " + F1.BASENAME + "..PAIDSER_LIST A ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void EditFreeService(string p, string idservice)
        {
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..FREESER_LIST set FSNAME = '" + p + "' where ID = " + idservice;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            Conn.SQLDA.UpdateCommand.Connection.Close();
        }

        internal void EditPaidService(string p, string idservice,string price)
        {
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.BJVVVConn;
            Conn.SQLDA.UpdateCommand.CommandText = "update " + F1.BASENAME + "..PAIDSER_LIST " +
                                                   " set PSNAME = '" + p + "',PRICE = " + price +
                                                   " where ID = " + idservice;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            int rcc = Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            Conn.SQLDA.UpdateCommand.Connection.Close();
        }

        internal void DelFreeService(string p)
        {
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..FREESER_LIST where ID =  " + p;
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }
        }

        internal void DelPaidService(string p)
        {
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..PAIDSER_LIST where ID =  " + p;
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }
        }

        internal object GetFreeIndividual(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.FSNAME,case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then '0' else sum(B.AMOUNT) end from " + F1.BASENAME + "..FREESER_LIST A " +
                " left join " + F1.BASENAME + "..FREESERVICE B on A.ID = B.IDFREELIST and cast(cast(B.DATESERVICE as varchar(11)) as datetime) between cast(cast('" + start.ToString("yyyyMMdd") +
                "' as varchar(11)) as datetime) and cast(cast('" + end.ToString("yyyyMMdd") + "'  as varchar(11)) as datetime) and B.IDEMP = " + F1.EmpID +
                " group by A.ID,A.FSNAME ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetPaidIndividual(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " with FA as( select A.ID id," +
                " A.PSNAME nam," +
                " (case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then 0 else sum(B.AMOUNT) end) cnt, " +
                " A.PRICE pri,"+
                " (case when B.PRICE*sum(B.AMOUNT) = 0 or B.PRICE*sum(B.AMOUNT) is null then 0 else B.PRICE*sum(B.AMOUNT) end) cst" +
                " from " + F1.BASENAME + "..PAIDSER_LIST A " +
                " left join " + F1.BASENAME + "..PAIDSERVICE B on A.ID = B.IDPAIDLIST and cast(cast(B.DATESERVICE As VarChar(11))As DateTime) between '" +
                 start.ToString("yyyyMMdd") + "' and '" +end.ToString("yyyyMMdd")+"' and B.IDEMP = "+F1.EmpID+
                " group by A.ID,A.PSNAME,A.PRICE,B.PRICE" +
                " ) " +
                " select id,nam,sum(cnt),pri,sum(cst) from FA" +
                " group by id,nam,pri" +
                " having sum(cnt) != 0"+
                " order by id";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetFreeDep(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select A.ID,A.FSNAME,case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then '0' else sum(B.AMOUNT) end from " + F1.BASENAME + "..FREESER_LIST A " +
                " left join " + F1.BASENAME + "..FREESERVICE B on A.ID = B.IDFREELIST and cast(cast(B.DATESERVICE As VarChar(11))As DateTime) between '" + start.ToString("yyyyMMdd") +
                "' and '" + end.ToString("yyyyMMdd") + "' and B.IDDEP = " + F1.DepID +
                " group by A.ID,A.FSNAME ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetPaidDep(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " with FA as( select A.ID id," +
                " A.PSNAME nam," +
                " (case when sum(B.AMOUNT) = 0 or sum(B.AMOUNT) is null then 0 else sum(B.AMOUNT) end) cnt, " +
                " A.PRICE pri,"+
                " (case when B.PRICE*sum(B.AMOUNT) = 0 or B.PRICE*sum(B.AMOUNT) is null then 0 else B.PRICE*sum(B.AMOUNT) end) cst" +
                " from " + F1.BASENAME + "..PAIDSER_LIST A " +
                " left join " + F1.BASENAME + "..PAIDSERVICE B on A.ID = B.IDPAIDLIST and cast(cast(B.DATESERVICE As VarChar(11))As DateTime) between '" +
                 start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "' and B.IDDEP = " + F1.DepID +
                " group by A.ID,A.PSNAME,A.PRICE,B.PRICE" +
                " ) " +
                " select id,nam,sum(cnt),pri,sum(cst) from FA" +
                " group by id,nam,pri " +
                " having sum(cnt) != 0"+
                " order by id";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal string GetAttendance()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select * from " + F1.BASENAME + "..ATTENDANCE where Cast(Cast(DATEATT As VarChar(11)) As DateTime) = '" + DateTime.Today.ToString("yyyyMMdd") +
                "' and IDDEP = " + F1.DepID;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return t.ToString();
        }

        internal void AddAttendance(string p,string barg)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.Parameters.Add("DATEA", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["DATEA"].Value = DateTime.Now;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME +
                "..[ATTENDANCE] (IDDEP,BAR,DATEATT,BARG) values" +
                "(" + F1.DepID + ",'" + p + "',@DATEA,'"+barg+"')";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработало добавление посещаемости");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }

        internal string GetAttendance(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText =
                " select count(*) cnt from " + F1.BASENAME + "..ATTENDANCE where Cast(Cast(DATEATT As VarChar(11)) As DateTime) between '" + start.ToString("yyyyMMdd") +
                "' and '" + end.ToString("yyyyMMdd") + "' and IDDEP = " + F1.DepID;
            if (Conn.SQLDA.SelectCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.SelectCommand.Connection.Open();
            }
            string s = Conn.SQLDA.SelectCommand.ExecuteScalar().ToString();
            if (Conn.SQLDA.SelectCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.SelectCommand.Connection.Close();
            }

            DataSet DS = new DataSet();
            return s;
        }

        internal DataTable GetNegotiabilityINV_InHallDP()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                     "   group by A.INV" +
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                      "  group by B.INV" +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                     //"   union" +
                     //"   select * from FCC)" +
                     " ) " +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                     "   where D.IDINLIST = " + F1.DepID+
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityPIN_INHALLDP()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt,A.INV from " + F1.BASENAME + 
                   "..ISSUED_OF_HST A where A.IDMAIN != -1 and A.ZALISS = '" + F1.DepName + "'"+
                   " group by A.IDMAIN,A.INV" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt,B.INV from " + F1.BASENAME + 
                   "..ISSUED_OF B where B.IDMAIN_CONST != -1 and B.ZALISS ='" + F1.DepName + "'" +
                   " group by B.IDMAIN_CONST,B.INV" +
                   "  )," +
                   "  FG as(" +
                   "  select distinct FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt,FC.INV from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  " +
                   "  )" +
                   "  select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = FG.INV" +
                   "  left join BJVVV..DATAEXT D on INV.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                   "  where D.IDINLIST = "+F1.DepID+
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityINV_BK()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME +
                     "..ISSUED_OF_HST A where A.INV != '-1' and A.ZALISS ='" + F1.DepName +
                     "'   group by A.INV " + 
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                     "   and B.ZALISS = '"+F1.DepName+"' group by B.INV " +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                     "   " +
                     "   )" +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                     "   where (D.IDINLIST = 6 or D.IDINLIST = 7 or D.IDINLIST = 8 or D.IDINLIST = 9 or D.IDINLIST = 10" +
                     "   or D.IDINLIST = 11 or D.IDINLIST = 12 or D.IDINLIST = 13 or D.IDINLIST = 14 or D.IDINLIST = 15" +
                     "   or D.IDINLIST = 46) " +
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityPIN_BK()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt,A.INV from " + F1.BASENAME + 
                   "..ISSUED_OF_HST A where A.IDMAIN != -1 and A.ZALISS ='" + F1.DepName +
                   "' group by A.IDMAIN,A.INV" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt,B.INV from " + F1.BASENAME +
                   "..ISSUED_OF B where B.IDMAIN_CONST != -1 and B.ZALISS = '" + F1.DepName + "'" +
                   " group by B.IDMAIN_CONST,B.INV" +
                   "  )," +
                   "  FG as(" +
                   "  select FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt,FC.INV from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  " +
                   "  )" +
                   "  select distinct B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = FG.INV" +
                   "  left join BJVVV..DATAEXT D on INV.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                   "  where (D.IDINLIST = 6 or D.IDINLIST = 7 or D.IDINLIST = 8 or D.IDINLIST = 9 or D.IDINLIST = 10" +
                   "  or D.IDINLIST = 11 or D.IDINLIST = 12 or D.IDINLIST = 13 or D.IDINLIST = 14 or D.IDINLIST = 15" +
                   "  or D.IDINLIST = 46) " + 
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetAttendace(DateTime start, DateTime end)
        {
            
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.Parameters.Add("start", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["start"].Value = start;
            Conn.SQLDA.SelectCommand.Parameters.Add("end", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["end"].Value = end;
            Conn.SQLDA.SelectCommand.Parameters.Add("iddep", SqlDbType.Int);
            Conn.SQLDA.SelectCommand.Parameters["iddep"].Value = F1.DepID;
            Conn.SQLDA.SelectCommand.CommandText = "SELECT * FROM " + F1.BASENAME + ".dbo.GetAttendance(@start, @end, @iddep)";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetAttendaceUnique(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.Parameters.Add("start", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["start"].Value = start;
            Conn.SQLDA.SelectCommand.Parameters.Add("end", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["end"].Value = end;
            Conn.SQLDA.SelectCommand.Parameters.Add("iddep", SqlDbType.Int);
            Conn.SQLDA.SelectCommand.Parameters["iddep"].Value = F1.DepID;
            Conn.SQLDA.SelectCommand.CommandText = "SELECT id,fam,nam,fnam,idr,case when idr = '<нет>' then 1 else count(idr) end cnt FROM " + F1.BASENAME + ".dbo.[GetAttendance](@start, @end, @iddep) " +
                                       " group by id,fam,nam,fnam,idr";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal bool WasTodayInCurrentDep(string bar)
        {
            if (bar.Substring(0, 1) == "V") return false;

            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.Parameters.Add("datt", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["datt"].Value = DateTime.Now.Date;
            Conn.SQLDA.SelectCommand.CommandText = "SELECT * FROM " + F1.BASENAME + "..ATTENDANCE " +
                                                   " where Cast(Cast(DATEATT As VarChar(11)) As DateTime) = @datt " +
                                                   " and IDDEP = " +F1.DepID+
                                                   " and BARG = '"+bar+"'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if ((((bar.Substring(0, 1) != "G") && (t == 0))) || ((bar.Substring(0, 1) == "G") && (t == 0)))
            {
                return false;
            }
            if ((bar.Substring(0, 1) != "G") && (t >= 1))
            {
                return true;
            }
            string idgcurrent = this.GetIDGCurr(bar);
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                if (idgcurrent == r["BAR"].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        internal string GetIDGCurr(string bar)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //Conn.SQLDA.SelectCommand.Parameters.Add("datt", SqlDbType.DateTime);
            //Conn.SQLDA.SelectCommand.Parameters["datt"].Value = datt.Date;
            Conn.SQLDA.SelectCommand.CommandText = " SELECT top 1 A.IDReaderInput id FROM Readers..Input A "+
                                 " where A.BarCodeInput = '"+bar+"'"+
                                 " order by A.DateInInput desc";
            DataSet DS = new DataSet();
            int tt = Conn.SQLDA.Fill(DS, "tt");
            if (tt == 0) return "-1";
            return (DS.Tables["tt"].Rows[0]["id"].ToString() == "0") ? "-1" : DS.Tables["tt"].Rows[0]["id"].ToString();
        }

        internal object GetReaders(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.Parameters.Add("start", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["start"].Value = start.Date;
            Conn.SQLDA.SelectCommand.Parameters.Add("end", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["end"].Value = end.Date;
            Conn.SQLDA.SelectCommand.Parameters.Add("iddep", SqlDbType.Int);
            Conn.SQLDA.SelectCommand.Parameters["iddep"].Value = F1.DepID;
            Conn.SQLDA.SelectCommand.CommandText = "select distinct A.IDREADER,B.FamilyName,B.[Name],B.FatherName,A.IDREADER,Cast(Cast(A.DATEACTION As VarChar(11)) As DateTime) from " + F1.BASENAME + "..[Statistics] A " +
                                                   " left join Readers..Main B on A.IDREADER = B.NumberReader " +
                                                   " where  Cast(Cast(A.DATEACTION As VarChar(11)) As DateTime) between @start and @end " +
                                                   " and A.ACTIONTYPE = 3 and A.DEPID = @iddep and A.IDREADER is not null ";

            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetReadersUnique(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.Parameters.Add("start", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["start"].Value = start.Date;
            Conn.SQLDA.SelectCommand.Parameters.Add("end", SqlDbType.DateTime);
            Conn.SQLDA.SelectCommand.Parameters["end"].Value = end.Date;
            Conn.SQLDA.SelectCommand.Parameters.Add("iddep", SqlDbType.Int);
            Conn.SQLDA.SelectCommand.Parameters["iddep"].Value = F1.DepID;
            Conn.SQLDA.SelectCommand.CommandText = "select distinct A.IDREADER idr,B.FamilyName fam,B.[Name] nam,B.FatherName fnam,A.IDREADER idr,count(A.IDREADER) cnt  from " + F1.BASENAME + "..[Statistics] A " +
                                                   " left join Readers..Main B on A.IDREADER = B.NumberReader " +
                                                   " where  Cast(Cast(A.DATEACTION As VarChar(11)) As DateTime) between @start and @end " +
                                                   " and A.ACTIONTYPE = 3 and A.DEPID = @iddep and A.IDREADER is not null " +
                                                   " group by A.IDREADER,B.FamilyName,B.[Name],B.FatherName";

            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal bool IsReaderEntered(dbReader R)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = " SELECT * FROM Readers..ReaderRight A " +
                                 " where A.IDReader = " + R.id +
                                 " and A.IDReaderRight = 3";
            DataSet DS = new DataSet();
            int tt = Conn.SQLDA.Fill(DS, "tt");
            if (tt == 0) 
            {
                Conn.SQLDA.SelectCommand = new SqlCommand();
                Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                Conn.SQLDA.SelectCommand.CommandText = " SELECT * FROM Readers..Input A " +
                                     " where A.IDReaderInput = " + R.id +
                                     " and Cast(Cast(A.DateInInput As VarChar(11)) As DateTime) = '" + DateTime.Now.ToString("yyyyMMdd") +
                                     "' and A.OutInput = 0 and (A.TapeInput =1 or A.TapeInput = 2 or A.TapeInput = 3)";
                DS = new DataSet();
                tt = Conn.SQLDA.Fill(DS, "tt");
                if (tt == 0)
                    return false; 
                else
                    return true;
            }
            else 
                return true;
        }

        internal bool IsOrderedByCurrentReader(dbBook BookRecord, dbReader ReaderRecord)
        {
            if (BookRecord.inv == "-1")
                return false;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = " select * from Reservation_O..Orders where ID_Reader = " +ReaderRecord.id + 
                                                   " and IDDATA = "+BookRecord.iddata;
            DataSet DS = new DataSet();
            int tt = Conn.SQLDA.Fill(DS, "tt");
            if (tt == 0)
                return false;
            else
                return true;
        }

        internal bool OrderedAtAll(dbBook BookRecord)
        {
            if (BookRecord.inv == "-1")
                return false;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = " select * from Reservation_O..Orders where Status != 8 and IDDATA = " + BookRecord.iddata;
            DataSet DS = new DataSet();
            int tt = Conn.SQLDA.Fill(DS, "tt");
            if (tt == 0)
                return false;
            else
                return true;
        }



        internal DataTable GetNegotiabilityPIN_INHALLDP_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt,A.INV from " + F1.BASENAME +
                   "..ISSUED_OF_HST A where A.IDMAIN != -1 and A.ZALISS = '" + F1.DepName + "'" + 
                   " and DATE_ISSUE between '" +start.ToString("yyyyMMdd")+ "' and '" +end.ToString("yyyyMMdd") +"'" +
                   " group by A.IDMAIN,A.INV" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt,B.INV from " + F1.BASENAME +
                   "..ISSUED_OF B where B.IDMAIN_CONST != -1 and B.ZALISS ='" + F1.DepName + "'" +
                   " and DATE_ISSUE between '" +start.ToString("yyyyMMdd")+ "' and '" +end.ToString("yyyyMMdd") +"'" +
                   " group by B.IDMAIN_CONST,B.INV" +
                   "  )," +
                   "  FG as(" +
                   "  select distinct FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt,FC.INV from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  )" +
                   "  select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = FG.INV" +
                   "  left join BJVVV..DATAEXT D on INV.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                   "  where D.IDINLIST = " + F1.DepID +
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }


        internal DataTable GetNegotiabilityINV_InHallDP_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1' " +
                     " and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                     "   group by A.INV" +
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                     " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                     "  group by B.INV" +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                //"   union" +
                //"   select * from FCC)" +
                     " ) " +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                     "   where D.IDINLIST = " + F1.DepID +
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityPIN_BK_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = F1.DepName;
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt,A.INV from " + F1.BASENAME +
                   "..ISSUED_OF_HST A where A.IDMAIN != -1 and A.ZALISS ='" + F1.DepName +
                   "' and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +

                   " group by A.IDMAIN,A.INV" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt,B.INV from " + F1.BASENAME +
                   "..ISSUED_OF B where B.IDMAIN_CONST != -1 and B.ZALISS = '" + F1.DepName + "'" +
                   " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                   " group by B.IDMAIN_CONST,B.INV" +
                   "  )," +
                   "  FG as(" +
                   "  select FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt,FC.INV from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  " +
                   "  )" +
                   "  select distinct B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT INV on INV.SORT collate Cyrillic_General_CS_AI = FG.INV" +
                   "  left join BJVVV..DATAEXT D on INV.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                   "  where (D.IDINLIST = 6 or D.IDINLIST = 7 or D.IDINLIST = 8 or D.IDINLIST = 9 or D.IDINLIST = 10" +
                   "  or D.IDINLIST = 11 or D.IDINLIST = 12 or D.IDINLIST = 13 or D.IDINLIST = 14 or D.IDINLIST = 15" +
                   "  or D.IDINLIST = 46) " +
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityINV_BK_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME +
                     "..ISSUED_OF_HST A where A.INV != '-1' and A.ZALISS ='" + F1.DepName +
                     "' and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                     "   group by A.INV " +
                     "   )," +
                     "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                     "   and B.ZALISS = '" + F1.DepName + "'"+
                     " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                     " group by B.INV " +
                     "   )," +
                     "   FG as(" +
                     "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                     "    left join FCC on FC.INV=FCC.INV" +
                     "   " +
                     "   )" +
                     "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt, EE.PLAIN tema from FG" +
                     "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                     "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                     "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT E on FIND.IDMAIN = E.IDMAIN and E.MNFIELD = 922 and E.MSFIELD = '$e'" +
                     "   left join BJVVV..DATAEXTPLAIN EE on E.ID = EE.IDDATAEXT " +
                     "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a' " +
                     "   where (D.IDINLIST = 6 or D.IDINLIST = 7 or D.IDINLIST = 8 or D.IDINLIST = 9 or D.IDINLIST = 10" +
                     "   or D.IDINLIST = 11 or D.IDINLIST = 12 or D.IDINLIST = 13 or D.IDINLIST = 14 or D.IDINLIST = 15" +
                     "   or D.IDINLIST = 46) " +
                     "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityPIN_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.IDMAIN,count(A.IDMAIN) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.IDMAIN != -1" +
                   " and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                   " group by A.IDMAIN" +
                   " )," +
                   " FCC as (select B.IDMAIN_CONST,count(B.IDMAIN_CONST) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.IDMAIN_CONST != -1" +
                   " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                   " group by B.IDMAIN_CONST" +
                   "  )," +
                   "  FG as(" +
                   "  select FC.IDMAIN, (case when FCC.IDMAIN_CONST is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                   "   left join FCC on FC.IDMAIN=FCC.IDMAIN_CONST" +
                   "  " +
                   "  )" +
                   "  select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.IDMAIN inv,FG.cnt cnt from FG" +
                   "  left join BJVVV..DATAEXT B on FG.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                   "  left join BJVVV..DATAEXT C on FG.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                   "  left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                   "  order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal DataTable GetNegotiabilityINV_PERIOD(DateTime start, DateTime end)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
                         " and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                         "   group by A.INV" +
                         "   )," +
                         "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
                           " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
                          "  group by B.INV" +
                         "   )," +
                         "   FG as(" +
                         "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
                         "    left join FCC on FC.INV=FCC.INV" +
                         "   " +
                         "   )" +
                         "   , T as ("+
                         "   select B.IDMAIN,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt, DD.PLAIN dep, TEMAP.PLAIN tema from FG" +
                         "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
                         "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                         "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
                         "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
                         "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT " +
                         "   left join BJVVV..DATAEXT D on FIND.IDDATA = D.IDDATA and D.MNFIELD = 899 and D.MSFIELD = '$a'" +
                         "   left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                         "   left join BJVVV..DATAEXT TEMA on FIND.IDMAIN = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'  " +
                         "   left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID  " +

                         "  ) ,"+          
                         " prexml as( " +
                                                    "select A.IDMAIN,B.VALUE " +
                                                    "from BJVVV..DATAEXT A " +
                                                    "left join BJVVV..TPR_TES B on A.SORT = B.IDCHAIN " +
                                                    "where MNFIELD = 606 and MSFIELD = '$a' and IDMAIN in (select IDMAIN from T) " +
                                                    "), " +
                                                    "xml606a as " +
                                                    "( " +
                                                    "select  A1.IDMAIN, " +
                                                    "        (select A2.VALUE+ '; '  " +
                                                    "        from prexml A2  " +
                                                    "        where A1.IDMAIN = A2.IDMAIN  " +
                                                    "        for XML path('') " +
                                                    "        ) vaj " +
                                                    "from prexml A1  " +
                                                    "group by A1.IDMAIN " +
                                                    ") " +
                                                    //"prelang as( " +
                                                    //"select A.IDMAIN,B.PLAIN  " +
                                                    //"from BJVVV..DATAEXT A " +
                                                    //"left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                                    //"where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDMAIN from T) " +
                                                    //"), " +
                                                    //"lang as " +
                                                    //"( " +
                                                    //"select  A1.IDMAIN, " +
                                                    //"        (select A2.PLAIN+ '; '  " +
                                                    //"        from prelang A2  " +
                                                    //"        where A1.IDMAIN = A2.IDMAIN  " +
                                                    //"        for XML path('') " +
                                                    //"        ) lng " +
                                                    //"from prelang A1  " +
                                                    //"group by A1.IDMAIN " +
                                                    //") " +
                                                    "select null,T.zag, T.avt, T.inv,count(T.inv) spr,T.dep ,A.vaj, T.tema " +
                                                    "from T   " +
                                                    "left join xml606a A on T.IDMAIN = A.IDMAIN " +
                                                    //"left join lang L on T.IDMAIN = L.IDMAIN " +
                                                    " group by T.zag , T.avt, T.inv, T.dep, A.vaj, T.tema " +
                                                    " order by spr desc";


            //Conn.SQLDA.SelectCommand.CommandText = "with FC as (select A.INV,count(A.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF_HST A where A.INV != '-1'" +
            //         " and A.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
            //         "   group by A.INV" +
            //         "   )," +
            //         "   FCC as (select B.INV,count(B.INV) as cnt from " + F1.BASENAME + "..ISSUED_OF B where B.INV != '-1'" +
            //           " and B.DATE_ISSUE between '" + start.ToString("yyyyMMdd") + "' and '" + end.ToString("yyyyMMdd") + "'" +
            //          "  group by B.INV" +
            //         "   )," +
            //         "   FG as(" +
            //         "   select FC.INV, (case when FCC.INV is not null then FC.cnt+FCC.cnt else FC.cnt end) cnt from FC" +
            //         "    left join FCC on FC.INV=FCC.INV" +
            //         "   " +
            //         "   )" +
            //         "   select B.ID,BB.PLAIN zag,CC.PLAIN avt,FG.INV inv,FG.cnt cnt from FG" +
            //         "   left join BJVVV..DATAEXT FIND on FIND.SORT collate Cyrillic_General_CS_AI = FG.INV and FIND.MNFIELD = 899 and FIND.MSFIELD = '$p' " +
            //         "   left join BJVVV..DATAEXT B on FIND.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
            //         "   left join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT " +
            //         "   left join BJVVV..DATAEXT C on FIND.IDMAIN = C.IDMAIN and C.MNFIELD = 700 and C.MSFIELD = '$a'" +
            //         "   left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
            //         "   order by cnt desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void DeleteRefusual()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_O..Orders where DATEDIFF(day,Start_Date,getdate()) >3 and Status = 10";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            foreach (DataRow r in B.Tables["t"].Rows)
            {
                Conn.SQLDA.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$a' and IDDATA = " + r["IDDATA"].ToString();
                t = Conn.SQLDA.Fill(B, "ab");
                /*if (t == 0)//не должно быть
                {
                    
                    continue;
                }
                if (t > 1)//не должно быть
                    continue;*/
                //if (!B.Tables["ab"].Rows[0]["SORT"].ToString().Contains("Абонемент"))
                {
                    Conn.SQLDA.InsertCommand = new SqlCommand();
                    Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                    Conn.SQLDA.InsertCommand.Connection.Open();
                    Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_O..OrdHis " +
                                    " select ID_Reader,ID_Book_EC,ID_Book_CC, Status,Start_Date, " +
                                    " Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA,REFUSUAL " +
                                    " from Reservation_O..Orders where ID = " + r["ID"].ToString();
                    Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                    Conn.SQLDA.InsertCommand.Connection.Close();

                    Conn.SQLDA.DeleteCommand = new SqlCommand();
                    Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Open();
                    }
                    Conn.SQLDA.DeleteCommand.CommandText = "delete from Reservation_O..Orders where ID = " + r["ID"].ToString();
                    Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Close();
                    }
                }
            }
            this.DeleteRefusualEmployee();
        }
        internal void DeleteRefusualEmployee()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_E..Orders where DATEDIFF(day,Start_Date,getdate()) >3 and Status = 10";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            foreach (DataRow r in B.Tables["t"].Rows)
            {
                Conn.SQLDA.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$a' and IDDATA = " + r["IDDATA"].ToString();
                t = Conn.SQLDA.Fill(B, "ab");
                /*if (t == 0)//не должно быть
                    continue;
                if (t > 1)//не должно быть
                    continue;*/
                //if (!B.Tables["ab"].Rows[0]["SORT"].ToString().Contains("Абонемент"))
                {
                    Conn.SQLDA.InsertCommand = new SqlCommand();
                    Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                    Conn.SQLDA.InsertCommand.Connection.Open();
                    Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_E..OrdHis " +
                                    " select ID_Reader,ID_Book_EC,ID_Book_CC, Status,Start_Date, " +
                                    " Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA,REFUSUAL " +
                                    " from Reservation_E..Orders where ID = " + r["ID"].ToString();
                    Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                    Conn.SQLDA.InsertCommand.Connection.Close();

                    Conn.SQLDA.DeleteCommand = new SqlCommand();
                    Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Open();
                    }
                    Conn.SQLDA.DeleteCommand.CommandText = "delete from Reservation_E..Orders where ID = " + r["ID"].ToString();
                    Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
                    if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
                    {
                        Conn.SQLDA.DeleteCommand.Connection.Close();
                    }
                }
            }
        }

        internal string GetOrders0(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a"+
                " from Reservation_O..OrdHis A "+
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 15 "+
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all "+
                "select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 15 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' ) "+
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders5(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 10 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' "+
                " union all "+
                " select count(*) a " +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 10 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' ) " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders1(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 6 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' "+
                " union all "+
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 6 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') "+
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders2(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 7 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 7 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders3(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 8 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 8 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders4(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 9 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 9 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }

        internal string GetOrders6(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 11 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = 11 " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
            throw new NotImplementedException();
        }

        internal string GetOrders8(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (6,7,8,9,10,11,15) " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (6,7,8,9,10,11,15) " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }
        internal string GetOrders11(DateTime dateTime, DateTime dateTime_2)
        {
            Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a" +
                " from Reservation_O..OrdHis A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (47) " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' " +
                " union all " +
                " select count(*) a" +
                " from Reservation_O..Orders A " +
                " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (47) " +
                " where A.Status != 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "') " +
                " select sum(a) from A";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }
        internal string GetResusual(DateTime dateTime, DateTime dateTime_2, string p)
        {
            if (p == "all")
            {
                Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a " +
                    " from Reservation_O..OrdHis A " +
                    " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (6,7,8,9,10,11,15)"+
                    " where A.Status = 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' "+
                    " union all "+
                    " select count(*) a " +
                    " from Reservation_O..Orders A " +
                    " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST in (6,7,8,9,10,11,15)"+
                    " where A.Status = 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' )"+
                    " select sum(a) from A";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText = "with A as (select count(*) a " +
                    " from Reservation_O..OrdHis A " +
                    " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = " + p +
                    " where A.Status = 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' "+
                    " union all " +
                    " select count(*) a" +
                    " from Reservation_O..Orders A " +
                    " join BJVVV..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.IDINLIST = " + p +
                    " where A.Status = 10 and A.Start_Date between '" + dateTime.ToString("yyyyMMdd") + "' and '" + dateTime_2.ToString("yyyyMMdd") + "' )"+
                    " select sum(a) from A";
            }
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet B = new DataSet();
            int t = Conn.SQLDA.Fill(B, "t");
            return B.Tables["t"].Rows[0][0].ToString();
        }


        internal List<string> GetNotInBaseTitles()
        {
            List<string> result = new List<string>();
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select distinct NAME from Reservation_R..PreDescr";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");

            foreach (DataRow str in DS.Tables["t"].Rows)
            {
                result.Add(str[0].ToString());
            }
            return result;
        }


        internal string GetCountIssuedHomeBooks()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = F1.DepName;
            Conn.SQLDA.SelectCommand.CommandText = " select count(A.ID)" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " where A.ZALISS = '" + F1.DepName + "' and A.IDMAIN != 0 and A.ATHOME = 1";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"].Rows[0][0].ToString();
        }

        internal void setBookForReaderHome(dbBook book, dbReader reader)
        {
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + "..ISSUED_OF " +
                " where BAR = '" + book.barcode +"'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.DeleteCommand.Connection.Close();
            }

            this.NEED_REQUIRMENT = false;
            SqlCommand cmd = new SqlCommand();
            Conn.SQLDA.InsertCommand = new SqlCommand();
            cmd.Connection = Conn.ZakazCon;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            SqlTransaction tran;
            tran = cmd.Connection.BeginTransaction("forreader");
            cmd.Transaction = tran;
            try
            {
                cmd.CommandText = "select * from Reservation_O..Orders where IDDATA = " + book.iddata;
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    r.Read();
                    if ((r["Status"].ToString() == "8") && (r["ID_Reader"].ToString() != reader.id))
                    {
                        r.Close();

                        cmd.CommandText = "insert into Reservation_O..OrdHis (ID_Reader,ID_Book_EC,ID_Book_CC,Status,Start_Date,Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA)" +
                            " select ID_Reader,ID_Book_EC,ID_Book_CC,11,Start_Date,Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA " +
                            " from Reservation_O..Orders where IDDATA = " + book.iddata;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete from Reservation_O..Orders where IDDATA = " + book.iddata;
                        cmd.ExecuteNonQuery();

                        this.NEED_REQUIRMENT = true;
                    }
                    else
                    {
                        r.Close();

                        if (book.inv != "-1")
                        {
                            cmd.CommandText = "update Reservation_O..Orders set Status = 9, REFUSUAL = '' where IDDATA = " + book.iddata;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                r.Close();
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF (IDMAIN,BAR,DATE_RET,IDREADER,IDEMP,DATE_ISSUE,DATE_CHG_KLASS, IDMAIN_CONST, " +
                                                        " INV, ZALISS, RESPAN, KLASS, RLOCATION,IDDATA,ATHOME) values (@IDMAIN,@BAR,@DATE_RET,@IDREADER,@IDEMP,@DATE_ISSUE,@DATE_CHG_KLASS,@IDMAIN_CONST, " +
                                                        " @INV,  @ZALISS, @RESPAN, @KLASS, @RLOCATION,@IDDATA,1)";
                cmd.Parameters.Add("IDMAIN", SqlDbType.Int);
                cmd.Parameters.Add("BAR", SqlDbType.NVarChar);
                cmd.Parameters.Add("DATE_RET", SqlDbType.DateTime);
                cmd.Parameters.Add("IDREADER", SqlDbType.Int);
                cmd.Parameters.Add("IDEMP", SqlDbType.Int);
                cmd.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime);
                cmd.Parameters.Add("DATE_CHG_KLASS", SqlDbType.DateTime);
                cmd.Parameters.Add("IDMAIN_CONST", SqlDbType.NVarChar);
                cmd.Parameters.Add("INV", SqlDbType.NVarChar);
                cmd.Parameters.Add("ZALISS", SqlDbType.NVarChar);
                cmd.Parameters.Add("RESPAN", SqlDbType.NVarChar);
                cmd.Parameters.Add("KLASS", SqlDbType.NVarChar);
                cmd.Parameters.Add("RLOCATION", SqlDbType.NVarChar);
                cmd.Parameters.Add("IDDATA", SqlDbType.Int);

                cmd.Parameters["IDMAIN"].Value = book.id;
                cmd.Parameters["BAR"].Value = book.barcode;
                if (book.klass == "ДП")
                {
                    cmd.Parameters["DATE_RET"].Value = DateTime.Now.Date.AddDays(1);
                }
                else
                {
                    if (book.getFloor().Contains("Абонемент"))
                    {
                        cmd.Parameters["DATE_RET"].Value = DateTime.Today.AddMonths(1);
                    }
                    else
                    {
                        if ((reader.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL)
                            cmd.Parameters["DATE_RET"].Value = DateTime.Today.AddMonths(3);
                        else
                            cmd.Parameters["DATE_RET"].Value = F1.dateTimePicker2.Value;
                    }
                }
                cmd.Parameters["IDREADER"].Value = reader.id;
                cmd.Parameters["IDEMP"].Value = F1.EmpID;
                cmd.Parameters["DATE_CHG_KLASS"].Value = book.ChgKlass;
                cmd.Parameters["DATE_ISSUE"].Value = DateTime.Now;
                cmd.Parameters["IDMAIN_CONST"].Value = book.id;
                cmd.Parameters["INV"].Value = book.inv;
                cmd.Parameters["ZALISS"].Value = F1.DepName;
                if (book.klass == "ДП")
                {
                    cmd.Parameters["RESPAN"].Value = "ДП";
                }
                else
                {
                    if (book.RESPAN == null)
                        book.RESPAN = "";
                    if (this.NEED_REQUIRMENT)
                    {
                        cmd.Parameters["RESPAN"].Value = book.RESPAN;
                    }
                    else
                    {
                        if (book.ord_rid == "-1")
                            cmd.Parameters["RESPAN"].Value = reader.id;
                        else
                            cmd.Parameters["RESPAN"].Value = book.ord_rid;
                    }
                }
                cmd.Parameters["KLASS"].Value = book.klass;
                cmd.Parameters["IDDATA"].Value = ((book.iddata == null) || (book.iddata == "")) ? 0 : int.Parse(book.iddata);
                cmd.Parameters["RLOCATION"].Value = (reader.rlocation == null) ? "" : reader.rlocation;
                cmd.CommandText += ";select cast(scope_identity() as int)";
                object p = cmd.ExecuteScalar();
                int ident = (int)p;
                cmd.CommandText = "update " + F1.BASENAME + "..RecievedBooks " +
                                                       " set IDISSUED_OF = " + ident.ToString() +
                                                       " where BAR = '" + book.barcode + "' and RETINBK=0";
                int rcc = cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from " + F1.BASENAME + "..PREPBK " +
                    " where BAR = '" + book.barcode + "'";
                cmd.ExecuteNonQuery();
                tran.Commit();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
        }
        internal void MoveToHistoryAlienRespan(dbBook book)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn.BJVVVConn;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            string zalretbk = "";
            if (F1.DepName.Contains("Книгохра"))
            {
                zalretbk = book.GetZalIss(F1.DepName);
            }
            else
            {
                zalretbk = F1.DepName;
            }
            
            cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF_HST " +
                                                   " (IDREADER,IDEMP,DATE_ISSUE,DATE_RET,IDMAIN,INV,BAR,ZALISS,ZALRET,RESPAN) values (" +
                                                   " " + book.rid +
                                                   " ," + ((book.idemp == null) ? F1.EmpID : book.idemp) +
                                                   " , '" + book.FirstIssue.ToString("yyyyMMdd") + "'" +
                                                   " , '" + book.vzv.ToString("yyyyMMdd") + "'" +
                                                   " ," + book.id +
                                                   " ,'" + book.inv +
                                                   "' ,'" + book.barcode +
                                                   "' ,'" + zalretbk +
                                                   "' ,'" + zalretbk + "','"+book.RESPAN+"');select cast(scope_identity() as int)";
            
            object p = cmd.ExecuteScalar();
            cmd.Connection.Close();
            int ident = (int)p;
            //this.MoveToPREPBK(book, ident);

            /*Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.ISSUED_OF where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();

            Conn.SQLDA.DeleteCommand.Connection.Close();
            
            Conn.SQLDA.UpdateCommand = new SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.CommandText = "Update " + F1.BASENAME + ".dbo.RecievedBooks set IDISSUED_OF = null where BAR = '" + book.barcode + "' and RETINBK = 0 and PFORBK is null";
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();

            Conn.SQLDA.UpdateCommand.Connection.Close();
            //this.ReturnOn_RESPAN_NOT_EVER_ISSUED(book);*/
        }



        internal bool ReaderHaveDedts(dbReader ReaderRecord)
        {
            if ( //если читатель имеет право брать на дом
                        ((ReaderRecord.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL) ||
                        ((ReaderRecord.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS) ||
                        ((ReaderRecord.ReaderRights & dbReader.Rights.COLL) == dbReader.Rights.COLL)
               )
            {

                Conn.SQLDA.SelectCommand.CommandText = "select * from "+F1.BASENAME+"..ISSUED_OF where IDREADER = "+ReaderRecord.id
                    + " and ATHOME = 1 and cast(cast(DATE_RET as varchar(11)) as datetime) < cast(cast(getdate() as varchar(11)) as datetime)";

                Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
                DataSet B = new DataSet();
                int t = Conn.SQLDA.Fill(B, "t");
                if (t > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        internal object GetDebtBooks()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select A.ID, " +
                " (case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
                " (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN end), " +
                " DD.PLAIN, (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end), A.IDREADER, A.RESPAN, A.DATE_RET, " +
                "  "+F1.BASENAME+".dbo.GetReaderRightString(A.IDREADER) rights, l8.SHORTNAME dep" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                   " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                   " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                   " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                   " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                   " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                   " where A.ZALISS = '" + dep + "' and A.IDMAIN != 0";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetIssBooksAtHome()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select A.ID, " +
                " (case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
                " (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN +'; '+ isnull(DD.PLAIN,'') end) zag, " +
                " (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end) inv, A.IDREADER,Reservation_R.dbo.GetReaderRights(A.IDREADER), A.ZALISS, cast(cast(A.DATE_ISSUE as varchar(11)) as datetime) iss, A.DATE_RET ret, " +
                " case when datediff(d,A.DATE_RET,getdate()) < 0 then 0 else datediff(d,A.DATE_RET,getdate()) end cdays" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                   " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                   " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                   " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                   " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                   " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                   " where A.IDMAIN != 0 and A.ATHOME = 1";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetIssBooksAtHomeOverDue()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select A.ID, " +
                " (case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
                " (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN +'; '+ isnull(DD.PLAIN,'') end) zag, " +
                " (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end) inv, A.IDREADER, "+
                " isnull(rm.FamilyName,'') +' '+isnull(rm.Name,'')+' '+isnull(rm.FatherName,'') fio , " +
                " isnull('факт.: '+rm.LiveTelephone+',','') + ' ' + isnull('дом.:'+rm.RegistrationTelephone+',','') ph, " +
                " Reservation_R.dbo.GetReaderRights(A.IDREADER), A.ZALISS, cast(cast(A.DATE_ISSUE as varchar(11)) as datetime) iss, A.DATE_RET ret, " +
                " case when datediff(d,A.DATE_RET,getdate()) < 0 then 0 else datediff(d,A.DATE_RET,getdate()) end cdays" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                   " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                   " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                   " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                   " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                   " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                   " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                   " where A.IDMAIN != 0 and A.ATHOME = 1 and A.DATE_RET < cast(cast(getdate() as varchar(11)) as datetime)";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetReadersAndIssBooksAtHomeOverDue()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            //ФИО
            //Номер билета
            //Права читателя
            //Инвентарный номер
            //телефон
            //Эл.почта
            //Адрес
            //Дата выдачи
            //Срок сдачи
            //Дней просрочено

            Conn.SQLDA.SelectCommand.CommandText = " select distinct rm.NumberReader, " +
                 "isnull(rm.FamilyName,'') +' '+isnull(rm.Name,'')+' '+isnull(rm.FatherName,'') fio , " +
                 "rm.NumberReader numr, " +
                 " Reservation_R.dbo.GetReaderRights(A.IDREADER) rgt, " +
                 "(case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end) inv," +
                 "isnull('факт.: '+rm.LiveTelephone+',','') + ' ' + isnull('дом.:'+rm.RegistrationTelephone+',','') ph, " +
                 "isnull('факт.: '+rm.Email+',','') em ," +
                 " 'Зарегистрирован: '+ isnull(rm.RegistrationProvince,'') + ', '+isnull(rm.RegistrationCity,'')+', '+isnull(rm.RegistrationStreet,'')+ '; ' + " +
                 " 'Проживает: ' +isnull(rm.LiveProvince,'') + ', '+isnull(rm.LiveCity,'')+ ', '+isnull(rm.LiveStreet,'') address ," +
                 " A.DATE_ISSUE , "+
                 " A.DATE_RET ret," +
                 " case when datediff(day, A.DATE_RET, getdate() ) < 0 then 0 else datediff(day, A.DATE_RET, getdate()) end ovrd " +
                 //" case when rm.Email is not null and rm.Email != '' then 'Email существует' else '' end note" +
                //" (case when A.IDMAIN_CONST = -1 then E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
                //" (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) else CC.PLAIN +'; '+ isnull(DD.PLAIN,'') end) zag " +
                //" (case when A.IDMAIN_CONST = -1 then A.BAR else A.INV end) inv, A.IDREADER,Reservation_R.dbo.GetReaderRights(A.IDREADER), A.ZALISS, cast(cast(A.DATE_ISSUE as varchar(11)) as datetime) iss, A.DATE_RET ret, " +
                //" case when datediff(d,A.DATE_RET,getdate()) < 0 then 0 else datediff(d,A.DATE_RET,getdate()) end cdays" +
                                                   " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                   " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                   //" left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                   //" left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                   //" left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                   //" left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                   //" left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                   " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                   " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                   " where A.IDMAIN != 0 and A.ATHOME = 1 and A.DATE_RET < cast(cast(getdate() as varchar(11)) as datetime)";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }
        internal object GetIssBooksAtHomeByFloor()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select * from "+F1.BASENAME+"..GetBooksIssuedAtHomeByFloor()";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetFloorAtHome(string p)
        {
            int idinlist = 0;
            switch (p)
            {
                case "…Хран… Сектор книгохранения - 0 этаж":
                    idinlist = 15;
                    break;
                case "…Хран… Сектор книгохранения - 2 этаж":
                    idinlist = 6;
                    break;
                case "…Хран… Сектор книгохранения - 3 этаж":
                    idinlist = 7;
                    break;
                case "…Хран… Сектор книгохранения - 4 этаж":
                    idinlist = 8;
                    break;
                case "…Хран… Сектор книгохранения - 5 этаж":
                    idinlist = 9;
                    break;
                case "…Хран… Сектор книгохранения - 6 этаж":
                    idinlist = 10;
                    break;
                case "…Хран… Сектор книгохранения - 7 этаж":
                    idinlist = 11;
                    break;
                case "Не в базе":
                    idinlist = -1;
                    break;
            }
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            if (idinlist == -1)
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID, " +
                    "  E.[NAME]+'; '+E.[YEAR] +'; ' +E.NUMBER + '; '+ " +
                    " (case when E.ADDNUMBERS is null then '' else E.ADDNUMBERS end) zag, " +
                    " DD.PLAIN avt, A.BAR inv, A.IDREADER, A.ZALISS, cast(cast(A.DATE_ISSUE as varchar(11)) as datetime) iss, A.DATE_RET ret, " +
                    " case when datediff(d,A.DATE_RET,getdate()) < 0 then 0 else datediff(d,A.DATE_RET,getdate()) end cdays" +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                       " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                       " where A.IDMAIN != 0 and A.IDMAIN = -1 and A.ATHOME = 1";
            }
            else
            {
                Conn.SQLDA.SelectCommand.CommandText = " select A.ID, " +
                    " CC.PLAIN zag, " +
                    " DD.PLAIN avt,  A.INV inv, A.IDREADER, A.ZALISS, cast(cast(A.DATE_ISSUE as varchar(11)) as datetime) iss, A.DATE_RET ret, " +
                    " case when datediff(d,A.DATE_RET,getdate()) < 0 then 0 else datediff(d,A.DATE_RET,getdate()) end cdays" +
                                                       " from " + F1.BASENAME + "..ISSUED_OF A" +
                                                       " inner join BJVVV..LIST_8 DEP on A.ZALISS = DEP.SHORTNAME" +
                                                       " left join BJVVV..DATAEXT C on A.IDMAIN_CONST = C.IDMAIN and C.MNFIELD = 200 and C. MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT" +
                                                       " left join BJVVV..DATAEXT D on A.IDMAIN_CONST = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a'" +
                                                       " left join BJVVV..DATAEXTPLAIN DD on D.ID = DD.IDDATAEXT " +
                                                       " left join BJVVV..DATAEXT flor0 " +
                                                       " on A.IDDATA = flor0.IDDATA and flor0.MNFIELD = 899 and flor0.MSFIELD = '$a' " +
                                                       " left join " + F1.BASENAME + "..PreDescr E on E.BARCODE = A.BAR " +
                                                       " left join Readers..Main rm on A.IDREADER = rm.NumberReader " +
                                                       " left join BJVVV..LIST_8 l8 on l8.ID = rm.WorkDepartment " +
                                                       " where A.IDMAIN != 0 and A.IDMAIN != -1 and A.ATHOME = 1 and flor0.IDINLIST =" + idinlist.ToString();
            }
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal void InsertActionEMAIL(dbReader reader)
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into " + F1.BASENAME + "..[Statistics] (IDEMP,DATEACTION,ACTIONTYPE,DEPID,IDREADER,BAR,COUNTISS) values" +
                "(" + F1.EmpID + ",getdate(),18," + F1.DepID + "," + reader.id + ",'',0)";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (отправлен email)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }
        internal void InsertTEST()
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into ReservationTEST..[Statistics] (DATEACTION) values" +
                "('" + DateTime.Now.ToString("yyyyMMdd hh:m:ss.fff tt") + "')";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Не сработала статистика (тест)");
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }
        internal string GetLastEmailDate(dbReader reader)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " select top 1 DATEACTION from " + F1.BASENAME + "..[Statistics] where IDREADER = "+reader.id +
                " and ACTIONTYPE = 18 order by ID desc";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            if (t == 0)
                return "noemail";
            else
                return ((DateTime)DS.Tables["t"].Rows[0]["DATEACTION"]).ToString("dd.MM.yyyy");
        }



        internal void InsertTEST1()
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.InsertCommand.CommandText = "insert into ReservationTEST..test (TXT) output inserted.ID values ('12')";
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.InsertCommand.Connection.Open();
            }
            try
            {
                int t = (int)Conn.SQLDA.InsertCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Open)
            {
                Conn.SQLDA.InsertCommand.Connection.Close();
            }
        }







        internal DataTable GetAllBooksInAbonement(DateTime dt)
        {
            
            
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "                with " +
                "T as (  " +
                "select POLP.PLAIN pol,ZAGP.PLAIN zag,AVTP.PLAIN avt,GODP.PLAIN god,INVP.PLAIN inv, (SPR.INV) spr,  " +
                "VYD.ID,  " +
                "case when VYD.ID is not null then 'Выдано' else 'Свободно' end vyd ,A.IDMAIN, TEMAP.PLAIN tema, ClassP.PLAIN class" +
                " from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXTPLAIN ZAGP on ZAGP.IDDATAEXT = A.ID  " +
                "left join BJVVV..DATAEXT AVT on A.IDMAIN = AVT.IDMAIN and AVT.MNFIELD = 700 and AVT.MSFIELD = '$a'  " +
                "left join BJVVV..DATAEXTPLAIN AVTP on AVTP.IDDATAEXT = AVT.ID  " +
                "left join BJVVV..DATAEXT GOD on A.IDMAIN = GOD.IDMAIN and GOD.MNFIELD = 2100 and GOD.MSFIELD = '$d'  " +
                "left join BJVVV..DATAEXTPLAIN GODP on GODP.IDDATAEXT = GOD.ID  " +
                "left join BJVVV..DATAEXT TEMA on A.IDMAIN = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'  " +
                "left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID  " +
                "left join BJVVV..DATAEXT POL on A.IDMAIN = POL.IDMAIN and POL.MNFIELD = 899 and POL.MSFIELD = '$c'  " +
                "left join BJVVV..DATAEXTPLAIN POLP on POLP.IDDATAEXT = POL.ID  " +
                "left join BJVVV..DATAEXT INV on A.IDMAIN = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p'  " +
                "left join BJVVV..DATAEXTPLAIN INVP on INVP.IDDATAEXT = INV.ID  " +
                "left join BJVVV..DATAEXT Class on INV.IDDATA = Class.IDDATA and Class.MNFIELD = 921 and Class.MSFIELD = '$c'  " +
                "left join BJVVV..DATAEXTPLAIN ClassP on ClassP.IDDATAEXT = Class.ID  " +
                "left join BJVVV..DATAEXT MST on INV.IDDATA = MST.IDDATA and MST.MNFIELD = 899 and MST.MSFIELD = '$a'  " +
                "left join Reservation_R..ISSUED_OF_HST SPR on SPR.INV collate cyrillic_general_ci_ai= INV.SORT  " +
                "left join Reservation_R..ISSUED_OF VYD on VYD.INV collate cyrillic_general_ci_ai= INV.SORT  " +
                "where A.MNFIELD = 200 and A.MSFIELD = '$a' and MST.IDINLIST = 29 " +
                " and INV.IDDATA not in (select A.IDDATA from BJVVV..INOUT_BASE A "+
						                " left join BJVVV..INOUT B on A.IDINOUT = B.ID "+
						                " where B.DEPTARGET = 29 and B.DateSent >= '"+dt.ToString("yyyyMMdd")+"') " +
                ")  , " +
                "prexml as( " +
                "select A.IDMAIN,B.VALUE " +
                "from BJVVV..DATAEXT A " +
                "left join BJVVV..TPR_TES B on A.SORT = B.IDCHAIN " +
                "where MNFIELD = 606 and MSFIELD = '$a' and IDMAIN in (select IDMAIN from T) " +
                "), " +
                "xml606a as " +
                "( " +
                "select  A1.IDMAIN, " +
                "        (select A2.VALUE+ '; '  " +
                "        from prexml A2  " +
                "        where A1.IDMAIN = A2.IDMAIN  " +
                "        for XML path('') " +
                "        ) vaj " +
                "from prexml A1  " +
                "group by A1.IDMAIN " +
                "), " + 
                "prelang as( " +
                "select A.IDMAIN,B.PLAIN  " +
                "from BJVVV..DATAEXT A " +
                "left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                "where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDMAIN from T) " +
                "), " +
                "lang as " +
                "( " +
                "select  A1.IDMAIN, " +
                "        (select A2.PLAIN+ '; '  " +
                "        from prelang A2  " +
                "        where A1.IDMAIN = A2.IDMAIN  " +
                "        for XML path('') " +
                "        ) lng " +
                "from prelang A1  " +
                "group by A1.IDMAIN " +
                ") " +
                "select null,T.pol,T.zag, T.avt, T.god,T.inv,count(T.spr) spr,T.vyd ,A.vaj,L.lng, T.tema, T.class " +
                "from T   " +
                "left join xml606a A on T.IDMAIN = A.IDMAIN " +
                "left join lang L on T.IDMAIN = L.IDMAIN " +
                " group by T.vyd,T.avt,T.god,T.inv,T.pol,T.zag ,A.vaj,L.lng, T.tema, T.class ";
            //Conn.SQLDA.SelectCommand.CommandText = "with T as ( " +
            //  "select POLP.PLAIN pol,ZAGP.PLAIN zag,AVTP.PLAIN avt,GODP.PLAIN god,INVP.PLAIN inv, (SPR.INV) spr, " +
            //  "VYD.ID, " +
            //  "case when VYD.ID is not null then 'Выдано' else 'Свободно' end vyd " +
            //  " from BJVVV..DATAEXT A " +
            //  "left join BJVVV..DATAEXTPLAIN ZAGP on ZAGP.IDDATAEXT = A.ID " +
            //  "left join BJVVV..DATAEXT AVT on A.IDMAIN = AVT.IDMAIN and AVT.MNFIELD = 700 and AVT.MSFIELD = '$a' " +
            //  "left join BJVVV..DATAEXTPLAIN AVTP on AVTP.IDDATAEXT = AVT.ID " +
            //  "left join BJVVV..DATAEXT GOD on A.IDMAIN = GOD.IDMAIN and GOD.MNFIELD = 2100 and GOD.MSFIELD = '$d' " +
            //  "left join BJVVV..DATAEXTPLAIN GODP on GODP.IDDATAEXT = GOD.ID " +
            //  "left join BJVVV..DATAEXT POL on A.IDMAIN = POL.IDMAIN and POL.MNFIELD = 899 and POL.MSFIELD = '$c' " +
            //  "left join BJVVV..DATAEXTPLAIN POLP on POLP.IDDATAEXT = POL.ID " +
            //  "left join BJVVV..DATAEXT INV on A.IDMAIN = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p' " +
            //  "left join BJVVV..DATAEXTPLAIN INVP on INVP.IDDATAEXT = INV.ID " +
            //  "left join BJVVV..DATAEXT MST on INV.IDDATA = MST.IDDATA and MST.MNFIELD = 899 and MST.MSFIELD = '$a' " +
            //  "left join Reservation_R..ISSUED_OF_HST SPR on SPR.INV collate cyrillic_general_ci_ai= INV.SORT " +
            //  "left join Reservation_R..ISSUED_OF VYD on VYD.INV collate cyrillic_general_ci_ai= INV.SORT " +
            //  "where A.MNFIELD = 200 and A.MSFIELD = '$a' and MST.IDINLIST = 29 " +
            //  ")  " +
            //  "select null,T.pol,T.zag, T.avt, T.god,T.inv,count(T.spr) spr,T.vyd  " +
            //   "from T  " +
            //   " group by T.vyd,T.avt,T.god,T.inv,T.pol,T.zag ";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];
            
        }

        internal void RemoveResponsibility(dbBook book)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = Conn.BJVVVConn;
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            string zalretbk = "";
            if (F1.DepName.Contains("Книгохра"))
            {
                zalretbk = book.GetZalIss(F1.DepName);
            }
            else
            {
                zalretbk = F1.DepName;
            }
            if (book.ISIssuedAtHome)
            {
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF_HST " +
                                                       " (IDREADER,IDEMP,DATE_ISSUE,DATE_RET,IDMAIN,INV,BAR,ZALISS,ZALRET,ATHOME,RESPAN) values (" +
                                                       " " + book.rid +
                                                       " ," + ((book.idemp == null) ? F1.EmpID : book.idemp) +
                                                       " , '" + book.FirstIssue.ToString("yyyyMMdd") + "'" +
                                                       " , '" + book.vzv.ToString("yyyyMMdd") + "'" +
                                                       " ," + book.id +
                                                       " ,'" + book.inv +
                                                       "' ,'" + book.barcode +
                                                       "' ,'" + zalretbk +
                                                       "' ,'" + zalretbk +
                                                       "' ,1,'" + book.RESPAN + "');select cast(scope_identity() as int)";
            }
            else
            {
                cmd.CommandText = "insert into " + F1.BASENAME + "..ISSUED_OF_HST " +
                                                       " (IDREADER,IDEMP,DATE_ISSUE,DATE_RET,IDMAIN,INV,BAR,ZALISS,ZALRET,RESPAN) values (" +
                                                       " " + book.rid +
                                                       " ," + ((book.idemp == null) ? F1.EmpID : book.idemp) +
                                                       " , '" + book.FirstIssue.ToString("yyyyMMdd") + "'" +
                                                       " , '" + book.vzv.ToString("yyyyMMdd") + "'" +
                                                       " ," + book.id +
                                                       " ,'" + book.inv +
                                                       "' ,'" + book.barcode +
                                                       "' ,'" + zalretbk +
                                                       "' ,'" + zalretbk + "','" + book.RESPAN + "');select cast(scope_identity() as int)";
            }
            try
            {
                cmd.ExecuteScalar();
            }
            catch { }

            cmd.Connection.Close();
            

            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.DeleteCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.DeleteCommand.Connection.Open();
            }
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.ISSUED_OF where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();

            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.PREPBK where BAR = '" + book.barcode + "'";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();

            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + F1.BASENAME + ".dbo.RecievedBooks where BAR = '" + book.barcode + "' and RETINBK = 0";
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();

            Conn.SQLDA.DeleteCommand.Connection.Close();

            this.MoveToHistoryOrders(book);
        }

        internal void MoveToHistoryOrders(string idOrder)
        {
            if (idOrder == "") return;
            if (idOrder == null) return;
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            SqlConnection con = Conn.ZakazCon;
            con.Open();
            SqlTransaction tr;
            Conn.SQLDA.SelectCommand.Connection = con;
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_O..Orders where ID = " + idOrder;
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "possb");
            con.Close();
            if (i > 0)
            {
                foreach (DataRow row in DS.Tables[0].Rows)
                {
                    con.Open();

                    SqlCommand comm = new SqlCommand();
                    comm.Connection = con;
                    tr = con.BeginTransaction("movhis");
                    comm.Transaction = tr;
                    try
                    {
                        //DA.Update(DS.Tables["ordhis"]);//так тоже транзакция работает!
                        //DA.DeleteCommand.ExecuteNonQuery();
                        string idstatus = row["Status"].ToString();
                        if (idstatus != "10")//если не отказ, значит завершено
                        {
                            idstatus = "11";
                        }
                        comm.CommandText = "insert into Reservation_O..OrdHis (ID_Reader, ID_Book_EC, ID_Book_CC, Status, Start_Date, Change_Date, InvNumber, Form_Date, Duration, Who, IDDATA,REFUSUAL) " +
                                           "values (" + row["ID_Reader"].ToString() + "," + row["ID_Book_EC"].ToString() + "," +
                                           row["ID_Book_CC"].ToString() + "," + idstatus + ",'" + ((DateTime)row["Start_Date"]).ToString("yyyyMMdd") + "','" +
                                           DateTime.Now.ToString("yyyyMMdd") + "','" + row["InvNumber"].ToString() + "','" +
                                           ((DateTime)row["Form_Date"]).ToString("yyyyMMdd hh:m:ss.fff tt") + "'," + row["Duration"].ToString() + "," +
                                           row["Who"].ToString() + ", " + row["IDDATA"].ToString() + ",'" + row["REFUSUAL"].ToString() + "')";
                        comm.ExecuteNonQuery();
                        comm.CommandText = "delete from Reservation_O..Orders where ID = " + row["ID"].ToString();
                        comm.ExecuteNonQuery();

                        tr.Commit();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        //System.Windows.Forms.MessageBox.Show(ex.Message);
                        tr.Rollback();
                    }


                }
            }
        }

        internal bool IsAtHome(string zi)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + F1.BASENAME + "..ISSUED_OF where ID = " + zi;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                return false;
            }
            if ((bool)DS.Tables["t"].Rows[0]["ATHOME"])
            {
                return true;
            }
            else
                return false;
        }

        internal string GetReaderRights(string NumberReader)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select Convert(varchar, DataEndReaderRight, 104) r " +
                                                   " from Readers..ReaderRight A where IDReaderRight = 4 and IDReader = " + NumberReader;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            string ret = "";
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                ret = "Бесплатный: нет \n";
            }
            else
            {
                ret = "Бесплатный до "+DS.Tables["t"].Rows[0]["r"].ToString()+"\n";
            }
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select Convert(varchar, DataEndReaderRight, 104) r " +
                                                   " from Readers..ReaderRight A where IDReaderRight = 5 and IDReader = " + NumberReader;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                ret += "Платный: нет" + "\n";
            }
            else
            {
                ret += "Платный до " + DS.Tables["t"].Rows[0]["r"].ToString() + "\n";
            }
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.CommandText = "select Convert(varchar, DataEndReaderRight, 104) r " +
                                                   " from Readers..ReaderRight A where IDReaderRight = 3 and IDReader = " + NumberReader;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                ret += "Сотрудник: нет";
            }
            else
            {
                ret += "Сотрудник: да";
            }
            return ret;
        }

        internal object GetReadersAbonementRights()
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandTimeout = 1200;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            string dep = this.GetDepName(F1.DepID);
            Conn.SQLDA.SelectCommand.CommandText = " SELECT  'Бесплатный абонемент',count([IDReader]) "+
                                                   "   FROM [Readers].[dbo].[ReaderRight] "+
                                                   "   where DataEndReaderRight >= GETDATE() "+
                                                   "   and IDReaderRight = 4 "+

                                                   " union all "+

                                                   " SELECT  'Платный абонемент',count([IDReader]) "+
                                                   "   FROM [Readers].[dbo].[ReaderRight] "+
                                                   "   where DataEndReaderRight >= GETDATE() "+
                                                   "   and IDReaderRight = 5";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            return DS.Tables["t"];


            
  
        }
    }
}