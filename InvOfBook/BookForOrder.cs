using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XMLConnections;
using ReaderForOrder;
using InvOfBookForOrder;
using System.Security.Cryptography;
namespace BookForOrder
{
    public class Book //: IEnumerable
    {
        //private SqlConnection con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
        private SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
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
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DA.SelectCommand = new SqlCommand(" select B.NAME from BJVVV..DATAEXT A "+
                                              " left join BJVVV..LIST_1 B on A.IDINLIST = B.ID " +
                                              "  where MNFIELD = 101 and MSFIELD = '$a' and IDMAIN = " + this.ID , con);
            DS = new DataSet();
            if (DA.Fill(DS, "lang") > 0)
                this.Language = DS.Tables["lang"].Rows[0]["NAME"].ToString();
            else
                this.Language = "Не указан";
        }
        public Book(string idm)
        {
            this.ID = idm;
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
        public string Language;
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
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DA.SelectCommand = new SqlCommand("select * from Reservation_O..Orders where InvNumber = '" + p + "'",con);
            DA.Fill(DS, "Name");
            DA.SelectCommand.Connection.Close();
            DA.Dispose();
            con.Dispose();
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);
        }
        public bool IsAlreadyInOrder(string p)
        {
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DA.SelectCommand = new SqlCommand("select * from Reservation_E..Orders where InvNumber = '" + p + "'", con);
            DA.Fill(DS, "Name");
            DA.SelectCommand.Connection.Close();
            DA.Dispose();
            con.Dispose();
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);
        }

        public static void InsertIntoBasket(string session,string idr, string mname, int r_type)
        {
            DataSet DS = new DataSet();
            SqlConnection con;// = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if (mname == "VGBIL-OPAC")//внешний опак
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from TECHNOLOG_VVV..USERLIST where session = '" + session + "'", con);
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
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            sdvig = new SqlDataAdapter("select * from Reservation_O..Basket where ID = 0", con);
            //con.Open();
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            foreach (string rb in readbook)
            {
                DataRow r = DS.Tables["BasketBook"].NewRow();
                r["IDREADER"] = Convert.ToInt64(idr);
                r["IDMAIN"] = Convert.ToInt64(rb);
                r["R_TYPE"] = r_type;
                DS.Tables["BasketBook"].Rows.Add(r);
                /*sdvig.InsertCommand = new SqlCommand();
                sdvig.InsertCommand.Connection = con;
                sdvig.InsertCommand.CommandText = "insert into Reservation_";*/
            }
            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["BasketBook"]);
            con.Close();

            DS = new DataSet();
            sdvig = new SqlDataAdapter();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if (mname == "VGBIL-OPAC")
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from TECHNOLOG_VVV..USERLIST where session = '" + session + "'", con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            sdvig.Dispose();

        }
        public static void InsertIntoBasketE(string session, string idr, string mname)
        {
            DataSet DS = new DataSet();
            SqlConnection con;// = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if ((mname == "VGBIL-OPAC") || (mname == "ADMINPC"))//внешний опак
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from TECHNOLOG_VVV..USERLIST where session = '" + session + "'", con);
            sdvig.SelectCommand.CommandTimeout = 1200;
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
            sdvig.SelectCommand.CommandTimeout = 1200;
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            foreach (string rb in readbook)
            {
                DataRow r = DS.Tables["BasketBook"].NewRow();
                r["IDREADER"] = Convert.ToInt64(idr);
                r["IDMAIN"] = Convert.ToInt64(rb);
                DS.Tables["BasketBook"].Rows.Add(r);
                /*sdvig.InsertCommand = new SqlCommand();
                sdvig.InsertCommand.Connection = con;
                sdvig.InsertCommand.CommandText = "insert into Reservation_";*/
            }
            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["BasketBook"]);
            con.Close();

            DS = new DataSet();
            sdvig = new SqlDataAdapter();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if (mname == "VGBIL-OPAC")
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from TECHNOLOG_VVV..USERLIST where session = '" + session + "'", con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            sdvig.Dispose();

        }

        public static void InsertIntoBasket(Reader reader,string mname)
        {
            DataSet DS = new DataSet();
            SqlConnection con;// = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if ((mname == "VGBIL-OPAC") || (mname == "ADMINPC"))//внешний опак
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from TECHNOLOG_VVV..USERLIST where session = '" + reader.Session + "'", con);
            //исправлять и закрывать конекции
            //con.Open();
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            sdvig.SelectCommand.CommandTimeout = 1200;
            List<string> readbook = new List<string>();
            foreach (DataRow r in DS.Tables["BasketBook"].Rows)
            {
                readbook.Add(r["idbook"].ToString());
            }
            DS = new DataSet();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            sdvig = new SqlDataAdapter("select * from Reservation_O..Basket where ID = 0", con);
            //con.Open();
            sdvig.SelectCommand.CommandTimeout = 1200;
            sdvig.Fill(DS, "BasketBook");
            //con.Close();
            foreach (string rb in readbook)
            {
                DataRow r = DS.Tables["BasketBook"].NewRow();
                r["IDREADER"] = Convert.ToInt64(reader.ID);
                r["IDMAIN"] = Convert.ToInt64(rb);
                r["R_TYPE"] = reader.Type;
                DS.Tables["BasketBook"].Rows.Add(r);
            }
            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["BasketBook"]);
            con.Close();

            DS = new DataSet();
            sdvig = new SqlDataAdapter();
            //con = new SqlConnection("Data Source=192.168.3.241;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            if (mname == "VGBIL-OPAC")
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/TechOut"));
            }
            else
            {
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/Tech"));
            }
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from TECHNOLOG_VVV..USERLIST where session = '" + reader.Session + "'", con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            sdvig.Dispose();
               
        }
        public bool IsHundredBooksOrdered(InvOfBook inv, int idr)
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from Reservation_O..Orders where  ID_Reader =" + idr.ToString(), con);
            sdvig.Fill(DS, "Ten");
            int AbonCNT = 0;
            int BKCNT = 0;
            foreach (DataRow r in DS.Tables["Ten"].Rows)
            {
                InvOfBook tmp = new InvOfBook(r["InvNumber"].ToString(), r["ID_Book_EC"].ToString(), r["IDDATA"].ToString());
                if (tmp.mhr == null)
                    tmp.mhr = "";
                if (tmp.mhr.Contains("Абонемент"))
                {
                    AbonCNT++;
                }
                else
                {
                    BKCNT++;
                }
            }
            con.Dispose();
            sdvig.Dispose();
            if (inv.mhr.Contains("Абонемент"))
            {
                if (AbonCNT >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (BKCNT >= 100)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int GetExemplarCount()
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select ID from BJVVV..DATAEXT where IDMAIN =" + this.ID + " and MNFIELD = 899 and MSFIELD = '$p'", con);
            return da.Fill(DS);
        }
        public int GetExemplarCountRED()
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select ID from REDKOSTJ..DATAEXT where IDMAIN =" + this.ID + " and MNFIELD = 899 and MSFIELD = '$p'", con);
            return da.Fill(DS);
        }

        public int GetBusyExemplarCount()
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select ID from Reservation_R..ELISSUED where BASE = 1 and IDMAIN = " + this.ID, con);
            return da.Fill(DS);
        }
        public int GetBusyExemplarCountRED()
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select ID from Reservation_R..ELISSUED where BASE = 2 and IDMAIN = " + this.ID, con);
            return da.Fill(DS);
        }
        public DateTime GetNearestFreeDate()
        {
            DataSet DS = new DataSet();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            SqlDataAdapter da = new SqlDataAdapter("select min(DATERETURN) from Reservation_R..ELISSUED where BASE = 1 and IDMAIN = " + this.ID, con);
            da.Fill(DS, "t");
            return ((DateTime)DS.Tables[0].Rows[0][0]).AddDays(1);
        }
        public void Ord(InvOfBook _inv, int dur, DateTime date, int idr,int r_type) //перенос из таблицы корзина в таблицу читатели
        {
            if (!_inv.inv.Contains("Электронная"))
            {
                DataSet DS = new DataSet();
                //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
                SqlDataAdapter sdvig = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Book_EC =" + this.ID, con);
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
                r["INOTE"] = _inv.note;
                if (_inv.IsAllig)
                {
                    r["ALGIDM"] = _inv.IdmainOfMainAllig;
                }
                DS.Tables["Name"].Rows.Add(r);


                SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

                sdvig.InsertCommand = cb.GetInsertCommand();

                sdvig.Update(DS.Tables["Name"]);
                con.Dispose();
                sdvig.Dispose();
            }
            else
            {
                DataSet DS = new DataSet();
                con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
                SqlDataAdapter da = new SqlDataAdapter("select * from Reservation_O..Orders where ID_Book_EC =" + this.ID, con);
                da.InsertCommand = new SqlCommand();
                da.InsertCommand.Connection = con;
                if (da.InsertCommand.Connection.State == ConnectionState.Closed)
                {
                    da.InsertCommand.Connection.Open();
                }
                da.InsertCommand.CommandText = "insert into Reservation_R..ELISSUED (IDMAIN,IDREADER,DATEISSUE,DATERETURN,VIEWKEY,FORMDATE,BASE,R_TYPE) values "+
                    " (@idm,@idr,@di,@dr,@vk,@fd,@idb,@rtype)";
                da.InsertCommand.Parameters.Add("idm", SqlDbType.Int);
                da.InsertCommand.Parameters.Add("idr", SqlDbType.Int);
                da.InsertCommand.Parameters.Add("di", SqlDbType.DateTime);
                da.InsertCommand.Parameters.Add("dr", SqlDbType.DateTime);
                da.InsertCommand.Parameters.Add("vk", SqlDbType.NVarChar);
                da.InsertCommand.Parameters.Add("fd", SqlDbType.DateTime);
                da.InsertCommand.Parameters.Add("idb", SqlDbType.Int);
                da.InsertCommand.Parameters.Add("rtype", SqlDbType.Int);
                da.InsertCommand.Parameters["idm"].Value = _inv.IDMAIN;
                da.InsertCommand.Parameters["idr"].Value = idr;
                da.InsertCommand.Parameters["di"].Value = date;
                da.InsertCommand.Parameters["dr"].Value = date.AddDays(dur);
                da.InsertCommand.Parameters["fd"].Value = DateTime.Now;
                da.InsertCommand.Parameters["idb"].Value = 1;//исправить на базу редкая или основ фонд когда заказ редкой будет
                da.InsertCommand.Parameters["rtype"].Value = r_type;
                byte[] random = new byte[20];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.
                da.InsertCommand.Parameters["vk"].Value = Convert.ToBase64String(random);

                da.InsertCommand.ExecuteNonQuery();
                da.InsertCommand.Connection.Close();
            }
        }
        public void OrdE(InvOfBook _inv, int dur, DateTime date, int idr) //перенос из таблицы корзина в таблицу читатели
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
            //r["INOTE"] = _inv.note;
            if (_inv.IsAllig)
            {
                r["ALGIDM"] = _inv.IdmainOfMainAllig;
            }
            DS.Tables["Name"].Rows.Add(r);


            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["Name"]);
            con.Dispose();
            sdvig.Dispose();

        }

        public void delFromBasket(string idr, int r_type)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter sdvig = new SqlDataAdapter();
            //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_O..Basket where R_TYPE = "+r_type+" and IDMAIN = " + this.ID + " and IDREADER = " + idr, con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            sdvig.Dispose();
        }
        public void delFromBasketE(string idr)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter sdvig = new SqlDataAdapter();
            //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            con.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Basket where IDMAIN = " + this.ID + " and IDREADER = " + idr, con);
            sdvig.DeleteCommand.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            sdvig.Dispose();
        }
        internal bool IsForAbonement()
        {
            DA = new SqlDataAdapter();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/ZakazO"));
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select * from BJVVV..DATAEXT A where IDMAIN = " + this.ID +
                        " and exists (select 1 from BJVVV..DATAEXT B where A.IDMAIN = B.IDMAIN and B.MNFIELD = 899 and B.MSFIELD = '$a' and B.SORT = 'ЦДДАбонемент')";
            DA.Fill(DS, "Name");
            DA.SelectCommand.Connection.Close();
            con.Dispose();
            DA.Dispose();
            return (DS.Tables["Name"].Rows.Count > 0 ? true : false);

        }
        public string GetLimitationForWSEBookSender(string PIN)
        {
            Book b = new Book(PIN);
            if (b.GetExemplarCount() - b.GetBusyExemplarCount() <= 0)
            {
                return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных.";

            }
            return "";
        }
        public string GetLimitationForWSEBookSenderRED(string PIN)
        {
            Book b = new Book(PIN);
            if (b.GetExemplarCountRED() - b.GetBusyExemplarCountRED() <= 0)
            {
                return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных.";

            }
            return "";
        }
    }

}
