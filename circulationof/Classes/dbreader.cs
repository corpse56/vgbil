using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Drawing;

namespace Circulation
{
    public class dbReader
    {
        public dbReader()
        {
            
        }
        public dbReader(int numberReader): this(dbReader.GetBarByID(numberReader))
        {

        }
        public dbReader(int numberReader, string formular)
        {
            if (formular != "formular")
            {
                return;
            }
            Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType, WorkDepartment, " +
                                                        " IDOldAbonement " +
                                                        " from Readers..Main " +
                                                        " inner join AbonementType on main.AbonementType = AbonementType.IDAbonemetType " +
                                                        " where NumberReader = "+ numberReader;
            DataSet DS = new DataSet();
            int ct = Conn.ReaderDA.Fill(DS);
            if (ct == 0)
            {
                this.barcode = "notfoundbynumber";
                return;
            }
            this.barcode = DS.Tables[0].Rows[0]["BarCode"].ToString();
            this.id = DS.Tables[0].Rows[0]["NumberReader"].ToString();
            string name = "";
            string secondName = "";
            try
            {
                name = DS.Tables[0].Rows[0]["Name"].ToString().Remove(1, DS.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
            }
            catch
            {
                name = "";
            }
            try
            {
                secondName = DS.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, DS.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
            }
            catch
            {
                secondName = "";
            }
            this.FIO = DS.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
            this.AbonType = DS.Tables[0].Rows[0]["NameAbonType"].ToString();
            this.Name = DS.Tables[0].Rows[0]["Name"].ToString();
            this.Surname = DS.Tables[0].Rows[0]["FamilyName"].ToString();
            this.SecondName = DS.Tables[0].Rows[0]["FatherName"].ToString();
            this.IsWasInOldBase = (bool)DS.Tables[0].Rows[0]["IDOldAbonement"];
            this.Department = "1";
            
            if (this.barcode != "notfoundbynumber")
            {
                if (this.id == "") return;
                Conn.ReaderDA.SelectCommand.CommandText = "select * from Readers..AbonementAdd where IDReader = " + this.id;
                DS = new DataSet();
                int rr = Conn.ReaderDA.Fill(DS, "t");
                if (rr == 0)
                {
                    this.RegInMos = DateTime.MinValue;
                }
                else
                {
                    if (DS.Tables["t"].Rows[0]["RegInMoscow"] == DBNull.Value)
                    {
                        this.RegInMos = DateTime.MinValue;
                    }
                    else
                    {
                        this.RegInMos = (DateTime)DS.Tables["t"].Rows[0]["RegInMoscow"];
                    }
                }
                Conn.ReaderDA.SelectCommand.CommandText =
                    //"select * from Readers..ReaderRight where (DataEndReaderRight is null or DataEndReaderRight >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "') and IDReader = " + this.id;
                    "select * from Readers..ReaderRight where IDReader = " + this.id;
                DS = new DataSet();
                rr = Conn.ReaderDA.Fill(DS, "t");
                if (rr != 0)
                {
                    foreach (DataRow r in DS.Tables["t"].Rows)
                    {
                        switch (r["IDReaderRight"].ToString())
                        {
                            case "1":
                                this.ReaderRights |= Rights.BRIT;
                                break;
                            case "2":
                                this.ReaderRights |= Rights.HALL;
                                break;
                            case "3":
                                this.ReaderRights |= Rights.EMPL;
                                this.Department = r["IDOrganization"].ToString();
                                break;
                            case "4":
                                this.ReaderRights |= Rights.ABON;
                                break;
                            case "5":
                                this.ReaderRights |= Rights.PERS;
                                break;
                            case "6":
                                this.ReaderRights |= Rights.COLL;
                                break;
                            default:
                                this.ReaderRights |= Rights.HALL;
                                break;
                        }
                    }
                }
                if ( //если читатель имеет право брать на дом
                        ((this.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL) ||
                        ((this.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS) ||
                        ((this.ReaderRights & dbReader.Rights.COLL) == dbReader.Rights.COLL)
                    )
                {
                    this.CanGetAtHome = true;
                }
                else
                {
                    this.CanGetAtHome = false;
                }
                //this.Department = "1";
                if (this.id != "")
                {
                    Conn.SQLDA.SelectCommand.CommandText = "select A.*,B.Photo fotka from Readers..Main A left join Readers..Photo B on A.NumberReader = B.IDReader where NumberReader = " + this.id;
                    DS = new DataSet();
                    int i = Conn.SQLDA.Fill(DS, "reader");
                    if (i != 0)
                    {
                        if (DS.Tables["reader"].Rows[0]["fotka"] != DBNull.Value)
                        {

                            byte[] data = (byte[])DS.Tables["reader"].Rows[0]["fotka"];

                            if (data != null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    ms.Write(data, 0, data.Length);
                                    ms.Position = 0L;

                                    this.Photo = new Bitmap(ms);
                                }
                            }
                        }
                        else
                        {
                            this.Photo = Properties.Resources.nofoto;
                        }
                    }
                }

            }
        }
        public static string GetBarByID(int numberReader)
        {
            Conn.ReaderDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + numberReader.ToString();
            Conn.ReaderDA.SelectCommand.Connection = Conn.ReadersCon;
            DataSet R = new DataSet();
            int i;
            try
            {
                i = Conn.ReaderDA.Fill(R, "t");
            }
            catch
            {
                return "";
            }
            if (i == 0)
            {
                return "";
            }
            string r = R.Tables["t"].Rows[0]["BarCode"].ToString();
            if (r == "0")
            {
                string re = R.Tables["t"].Rows[0]["NumberSC"].ToString().Trim().Replace("\0", "") + R.Tables["t"].Rows[0]["SerialSC"].ToString().Trim().Replace("\0", "");
                if (re == "")
                {
                    return "Читателю не присвоен штрихкод и нет социальной карты";
                }
                return re;
            }
            else
            {
                return "R" + R.Tables["t"].Rows[0]["BarCode"].ToString();
            }
        }
        public dbReader(dbReader Reader)
        {
            this.barcode = Reader.barcode;
            this.FIO = Reader.FIO;
            this.id = Reader.id;
            this.Surname = Reader.Surname;
            this.Name = Reader.Name;
            this.SecondName = Reader.SecondName;
            this.AbonType = Reader.AbonType;
            this.IsWasInOldBase = Reader.IsWasInOldBase;
            this.RegInMos = Reader.RegInMos;
            this.rlocation = Reader.rlocation;
            this.ReaderRights = Reader.ReaderRights;
            this.CanGetAtHome = Reader.CanGetAtHome;
            this.Department = Reader.Department;
            this.Photo = Reader.Photo;
        }
        public dbReader Clone()
        {
            return new dbReader(this);
        }
        public dbReader(string Bar)
        {
            this.Department = "";
            if (Bar == "Читателю не присвоен штрихкод и нет социальной карты")
            {
                this.barcode = "Читателю не присвоен штрихкод и нет социальной карты";
                return;
            }
            if (Bar == "")
            {
                this.barcode = "notfoundbynumber";
                return;
            }
            bool SocCard = false;
            bool NumSocCard = false;
            bool SerSocCard = false;
            bool FoundByNumber = false;
            DataSet DS = new DataSet();
            if (Bar.Length > 18)
            {
                SocCard = true;
                if (Bar.Contains(" "))
                {
                    Bar = Bar.Remove(19, 1);
                }
                string Ser = Bar.Substring(19, 8);
                Bar = Bar.Substring(0, 19);
                //Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where NumberSC = '" + Bar + "' and SerialSC = '" + Ser + "'";
                Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType,IDOldAbonement,WorkDepartment from main inner join AbonementType on main.AbonementType= AbonementType.IDAbonemetType where NumberSC = '" + Bar + "'";
                DS = new DataSet();
                int c = Conn.ReaderDA.Fill(DS);
                if (c == 0)
                    NumSocCard = true;
                else
                {
                    NumSocCard = false;
                    Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType,IDOldAbonement,WorkDepartment from main inner join AbonementType on main.AbonementType= AbonementType.IDAbonemetType where NumberSC = '" + Bar + "' and SerialSC = '" + Ser + "'";
                    DS = new DataSet();
                    int cnt = Conn.ReaderDA.Fill(DS);
                    if (cnt == 0)
                        SerSocCard = true;
                    else
                        SerSocCard = false;
                }
            }
            else
            {
                SocCard = false;
                int parse = 0;
                if (!int.TryParse(Bar.Remove(0, 1), out parse))
                {
                    this.barcode = "notfoundbynumber";
                    return;
                }
                //Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName from main where BarCode = " + Bar;
                if (Bar[0].ToString() == "R")
                {
                    Conn.ReaderDA.SelectCommand.CommandText = "select NumberReader, BarCode, FamilyName, Name, FatherName,AbonementType,NameAbonType, WorkDepartment, " +
                        " IDOldAbonement " +
                        " from Readers..Main " +
                        " inner join AbonementType on main.AbonementType = AbonementType.IDAbonemetType " +
                        " where BarCode = '" + Bar.Remove(0,1) + "'";
                }
                else
                {
                    Conn.ReaderDA.SelectCommand.CommandText =
                         " select top 1 A.NumberReader, A.BarCode, A.FamilyName, A.[Name], A.FatherName, A.WorkDepartment, " +
                         " A.AbonementType,B.NameAbonType, A.IDOldAbonement  " +
                         " from Readers..Main A " +
                         " inner join Readers..AbonementType B on A.AbonementType = B.IDAbonemetType  " +
                         " left join Readers..Input C on C.IDReaderInput = A.NumberReader " +
                         " where C.BarCodeInput = '" + Bar + "' and DateOutInput is null and TapeInput = 3 " + //тип 3 для тех кто забыл билет, а тип 5 берётся из таблицы OnePass, но нам это не надо
                         " and cast(cast(DateInInput as varchar(11)) as datetime) = cast(cast(getdate() as varchar(11)) as datetime)" +
                         "   order by C.IDInput desc";

                }
                DS = new DataSet();
                int ct = Conn.ReaderDA.Fill(DS);
                if (ct == 0)
                    FoundByNumber = true;
                else
                    FoundByNumber = false;
            }

            if (SocCard)
            {
                if (!NumSocCard)
                {
                    if (!SerSocCard)
                    {
                        //т яюЁ фх. эшўх эх фхырхь
                    }
                    else
                    {
                        this.barcode = "sersoc";
                    }
                }
                else
                {
                    this.barcode = "numsoc";
                }
            }
            else
            {
                if (!FoundByNumber)
                {
                    //т яюЁ фх
                }
                else
                {
                    this.barcode = "notfoundbynumber";
                }
            }
            if ((this.barcode == "sersoc") || (this.barcode == "numsoc") || (this.barcode == "notfoundbynumber"))
            {
                this.FIO = "";
                this.AbonType = "";
                this.Name = "";
                this.Surname = "";
                this.SecondName = "";
                this.IsWasInOldBase = false;
                this.id = "";
            }
            else
            {
                this.barcode = DS.Tables[0].Rows[0]["BarCode"].ToString();
                this.id = DS.Tables[0].Rows[0]["NumberReader"].ToString();
                string name = "";
                string secondName = "";
                try
                {
                    name = DS.Tables[0].Rows[0]["Name"].ToString().Remove(1, DS.Tables[0].Rows[0]["Name"].ToString().Length - 1) + ". ";
                }
                catch
                {
                    name = "";
                }
                try
                {
                    secondName = DS.Tables[0].Rows[0]["FatherName"].ToString().Remove(1, DS.Tables[0].Rows[0]["FatherName"].ToString().Length - 1) + ".";
                }
                catch
                {
                    secondName = "";
                }
                this.FIO = DS.Tables[0].Rows[0]["FamilyName"].ToString() + " " + name + secondName;
                this.AbonType = DS.Tables[0].Rows[0]["NameAbonType"].ToString();
                this.Name = DS.Tables[0].Rows[0]["Name"].ToString();
                this.Surname = DS.Tables[0].Rows[0]["FamilyName"].ToString();
                this.SecondName = DS.Tables[0].Rows[0]["FatherName"].ToString();
                this.IsWasInOldBase = (bool)DS.Tables[0].Rows[0]["IDOldAbonement"];
                this.Department = "1";//DS.Tables[0].Rows[0]["WorkDepartment"].ToString();
            }
            if (this.barcode != "notfoundbynumber")
            {
                if (this.id == "") return;
                Conn.ReaderDA.SelectCommand.CommandText = "select * from Readers..AbonementAdd where IDReader = " + this.id;
                DS = new DataSet();
                int rr = Conn.ReaderDA.Fill(DS, "t");
                if (rr == 0)
                {
                    this.RegInMos = DateTime.MinValue;
                }
                else
                {
                    if (DS.Tables["t"].Rows[0]["RegInMoscow"] == DBNull.Value)
                    {
                        this.RegInMos = DateTime.MinValue;
                    }
                    else
                    {
                        this.RegInMos = (DateTime)DS.Tables["t"].Rows[0]["RegInMoscow"];
                    }
                }
                Conn.ReaderDA.SelectCommand.CommandText =
                    //"select * from Readers..ReaderRight where (DataEndReaderRight is null or DataEndReaderRight >= '" + DateTime.Now.Date.ToString("yyyyMMdd") + "') and IDReader = " + this.id;
                    "select * from Readers..ReaderRight where IDReader = " + this.id;
                DS = new DataSet();
                rr = Conn.ReaderDA.Fill(DS, "t");
                if (rr != 0)
                {
                    foreach (DataRow r in DS.Tables["t"].Rows)
                    {
                        switch (r["IDReaderRight"].ToString())
                        {
                            case "1":
                                this.ReaderRights |= Rights.BRIT;
                                break;
                            case "2":
                                this.ReaderRights |= Rights.HALL;
                                break;
                            case "3":
                                this.ReaderRights |= Rights.EMPL;
                                this.Department = r["IDOrganization"].ToString();
                                break;
                            case "4":
                                this.ReaderRights |= Rights.ABON;
                                break;
                            case "5":
                                this.ReaderRights |= Rights.PERS;
                                break;
                            case "6":
                                this.ReaderRights |= Rights.COLL;
                                break;
                            default:
                                this.ReaderRights |= Rights.HALL;
                                break;
                        }
                    }
                }
                if ( //если читатель имеет право брать на дом
                        ((this.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL) ||
                        ((this.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS) ||
                        ((this.ReaderRights & dbReader.Rights.COLL) == dbReader.Rights.COLL)
                    )
                {
                    this.CanGetAtHome = true;
                }
                else
                {
                    this.CanGetAtHome = false;
                }
                if (this.id != "")
                {
                    Conn.SQLDA.SelectCommand.CommandText = "select A.*,B.Photo fotka from Readers..Main A left join Readers..Photo B on A.NumberReader = B.IDReader where NumberReader = " + this.id;
                    DS = new DataSet();
                    int i = Conn.SQLDA.Fill(DS, "reader");
                    if (i != 0)
                    {
                        if (DS.Tables["reader"].Rows[0]["fotka"] != DBNull.Value)
                        {

                            byte[] data = (byte[])DS.Tables["reader"].Rows[0]["fotka"];

                            if (data != null)
                            {
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                {
                                    ms.Write(data, 0, data.Length);
                                    ms.Position = 0L;

                                    this.Photo = new Bitmap(ms);
                                }
                            }
                        }
                        else
                        {
                            this.Photo = Properties.Resources.nofoto;
                        }
                    }
                }

            }

        }

        internal string GetEmail()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select Email from Readers..Main where NumberReader = " + this.id;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet D = new DataSet();
            int i = Conn.SQLDA.Fill(D);
            if (i == 0) return "";
            if (dbReader.IsValidEmail(D.Tables[0].Rows[0][0].ToString()))
            {
                return D.Tables[0].Rows[0][0].ToString();
            }
            else
            {
                return "";
            }

        }
        public static bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

       
        [Flags]
        public enum Rights { 
            [Description("N/A")]                                
            NA = 0, 
            [Description("Пользователь британского совета")]    
            BRIT = 1,
            [Description("Пользователь читальных залов ВГБИЛ")]
            HALL = 2,
            [Description("Сотрудник ВГБИЛ")]
            EMPL = 4,
            [Description("Индивидуальный абонемент")]
            ABON = 8,
            [Description("Персональный абонемент")]
            PERS = 16,
            [Description("Коллективный абонемент")]
            COLL = 32,
        };
        public bool isRightsExpired()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + this.id + " and IDReaderRight = 4";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.SQLDA);
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "right");
            bool retval = false;
            DateTime RightsDate = (DateTime)DS.Tables["right"].Rows[0]["DataEndReaderRight"];
            if ((DateTime.Now.Month == 12) && (RightsDate.Year == DateTime.Now.Year))
            {
                retval = true;
            }
            return retval;
        }
        public void ProlongRights()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from [Readers].[dbo].ReaderRight where IDReader = " + this.id + " and IDReaderRight = 4";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.SQLDA);
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "right");
            DS.Tables[0].Rows[0]["DataEndReaderRight"] = ((DateTime)DS.Tables[0].Rows[0]["DataEndReaderRight"]).AddYears(1);
            Conn.SQLDA.Update(DS.Tables[0]);
        }
        public void setReaderRight()
        {
            Conn.SQLDA.InsertCommand = new SqlCommand();
            Conn.SQLDA.InsertCommand.CommandText = "insert into [Readers].[dbo].ReaderRight "+
                " (IDReader,IDReaderRight,DataEndReaderRight) values (@id,@idr,@date)";
            Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
            if (Conn.SQLDA.InsertCommand.Connection.State == ConnectionState.Closed)
                Conn.SQLDA.InsertCommand.Connection.Open();
            Conn.SQLDA.InsertCommand.Parameters.Add("id", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("idr", SqlDbType.Int);
            Conn.SQLDA.InsertCommand.Parameters.Add("date", SqlDbType.DateTime);
            Conn.SQLDA.InsertCommand.Parameters["id"].Value = this.IntID;
            Conn.SQLDA.InsertCommand.Parameters["idr"].Value = 4;
            Conn.SQLDA.InsertCommand.Parameters["date"].Value = new DateTime(DateTime.Now.AddYears(1).Year, 12, 31);
            Conn.SQLDA.InsertCommand.ExecuteNonQuery();
            Conn.SQLDA.InsertCommand.Connection.Close();
            Conn.SQLDA.InsertCommand.Dispose();
        }
        public string barcode;
        public string id;
        public string FIO;
        public string Surname;
        public string Name;
        public string SecondName;
        public string AbonType;
        public bool IsWasInOldBase;
        public DateTime RegInMos;
        public string rlocation;
        public Rights ReaderRights;
        public bool CanGetAtHome;
        public string Department;
        public Image Photo;

        public int IntID
        {
            get
            {
                return int.Parse(this.id);
            }
        }

        internal string getReaderRightsString()
        {
            string ret = "";
            if ((this.ReaderRights & dbReader.Rights.ABON) == dbReader.Rights.ABON)
            {
                ret += "Индивидуальный абонемент; ";
            }
            if ((this.ReaderRights & dbReader.Rights.BRIT) == dbReader.Rights.BRIT)
            {
                ret += "Пользователь британского совета; ";
            }
            if ((this.ReaderRights & dbReader.Rights.COLL) == dbReader.Rights.COLL)
            {
                ret += "Коллективный абонемент; ";
            }
            if ((this.ReaderRights & dbReader.Rights.EMPL) == dbReader.Rights.EMPL)
            {
                ret += "Сотрудник ВГБИЛ (" + this.GetDepartment()+ "); " ;
                
            }
            if ((this.ReaderRights & dbReader.Rights.HALL) == dbReader.Rights.HALL)
            {
                //ret += "Пользователь читальных залов ВГБИЛ; ";
            }
            if ((this.ReaderRights & dbReader.Rights.PERS) == dbReader.Rights.PERS)
            {
                ret += "Персональный абонемент";
            }
            /*if ((this.ReaderRights & dbReader.Rights.NA) == dbReader.Rights.NA)
            {
                ret += "<нет>";
            }*/
            return ret;
        }

        private string GetDepartment()
        {
            Conn.ReaderDA.SelectCommand.CommandText = "select [SHORTNAME] from BJVVV..LIST_8 where  ID = " + this.Department;
            DataSet DS = new DataSet();

            if (Conn.ReaderDA.Fill(DS, "t") != 0)
            {
                return DS.Tables["t"].Rows[0]["SHORTNAME"].ToString();
            }
            else
            {
                return "";
            }
        }

        internal DateTime? GetDateEndPersAbonement()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + this.id + " and IDReaderRight = 5";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.SQLDA);
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "right");
            if (i != 0)
            {
                DateTime RightsDate = (DateTime)DS.Tables["right"].Rows[0]["DataEndReaderRight"];
                return RightsDate;
            }
            else
            {
                return null;
            }
        }

        internal DateTime? GetDateEndIndividualPersAbonement()
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + this.id + " and IDReaderRight = 4";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            SqlCommandBuilder cmd = new SqlCommandBuilder(Conn.SQLDA);
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "right");
            if (i != 0)
            {
                DateTime RightsDate = (DateTime)DS.Tables["right"].Rows[0]["DataEndReaderRight"];
                return RightsDate;
            }
            else
            {
                return null;
            }
        }
    }

}

