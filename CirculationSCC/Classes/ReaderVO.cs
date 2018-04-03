using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.ComponentModel;

namespace Circulation
{
    [Flags]
    public enum Rights
    {
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
    public class ReaderVO
    {
        public int ID;
        public string Family;
        public string Name;
        public string Father;
        public Image Photo;
        public string FIO;
        public string BAR;
        public bool IsEmployee = false;
        public Rights ReaderRights;
        public string IDDepartment = "";

        public ReaderVO() { }

        public static bool IsReader(string bar)
        {
            if (bar.Length > 0)
                bar = bar.Remove(0, 1);
            return ((bar.Length > 18) || (bar.Length == 7)) ? true : false;
        }

        
        public ReaderVO(int ID)
        {
            DBReader dbr = new DBReader();
            DataRow reader = dbr.GetReaderByID(ID);
            DataTable rights = dbr.GetReaderRightsById(ID);
            if (reader == null) return;
            this.ID = (int)reader["NumberReader"];
            this.Family = reader["FamilyName"].ToString();
            this.Father = reader["FatherName"].ToString();
            this.Name = reader["Name"].ToString();
            this.FIO = this.Family + " " + this.Name + " " + this.Father;
            if (reader["fotka"].GetType() != typeof(System.DBNull))
            {
                byte[] data = (byte[])reader["fotka"];

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
            InitReaderRights();
        }
        public ReaderVO(string BAR)
        {
            this.BAR = BAR;
            if (BAR[0] == 'G') return;
            var dbr = new DBReader();
            DataRow reader = dbr.GetReaderByBAR(BAR);
            if (reader == null) return;
            this.ID = (int)reader["NumberReader"];
            this.Family = reader["FamilyName"].ToString();
            this.Father = reader["FatherName"].ToString();
            this.Name = reader["Name"].ToString();
            this.FIO = this.Family + " " + this.Name + " " + this.Father;
            if (reader["fotka"].GetType() != typeof(System.DBNull))
            {
                object o = reader["fotka"];
                byte[] data = (byte[])reader["fotka"];

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
            InitReaderRights();
        }
        private void InitReaderRights()
        {
            var dbr = new DBReader();
            DataTable rights = dbr.GetReaderRightsById(this.ID);
            if (rights.Rows.Count != 0)
            {
                foreach (DataRow r in rights.Rows)
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
                            this.IDDepartment = r["IDOrganization"].ToString();
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
        }

        public bool IsAlreadyIssuedMoreThanFourBooks()
        {
            DBReader dbr = new DBReader();
            return dbr.IsAlreadyIssuedMoreThanFourBooks(this);
        }
        public DataTable GetFormular()
        {
            DBReader dbr = new DBReader();
            return dbr.GetFormular(this.ID);
        }

        internal string GetEmail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetEmail(this);
        }

        internal string GetLastDateEmail()
        {
            DBReader dbr = new DBReader();
            return dbr.GetLastDateEmail(this);

        }

        public bool IsAlreadyMarked()
        {
            DBReader dbr = new DBReader();
            return dbr.IsAlreadyMarked(this.BAR);


            //кароче тут такая фигня неоднозначная:
            //Если читатель забыл билет, то ему выдают временный с буквой G, который привязан к реальному. При этом в таблице Input поле TapeInput = 3.
            //и типа надо проверять, что за читатель на самом деле. Но сейчас ему выдают не временный, а ещё один реальный. Полноценный, но с другим штрихкодом
            //поэтому можно забить на такую проверку. Всё равно нужно только количество. А когда правила изменятся, тогда и будем думать
            //в основном фонде это типа реализовано, хотя и как-то подозрительно.

            //string idgcurrent = this.GetRealIDByGuestBar(bar);
            //foreach (DataRow r in DS.Tables["t"].Rows)
            //{
            //    if (idgcurrent == r["BAR"].ToString())
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        internal string GetRealIDByGuestBar(string bar)
        {
            DBReader dbr = new DBReader();
            return dbr.GetRealIDByGuestBar(bar);

        }


        internal string GetComment()
        {
            DBReader dbr = new DBReader();
            return dbr.GetComment(this.ID);
        }

        internal void ChangeComment(string comment)
        {
            DBReader dbr = new DBReader();
            dbr.ChangeComment(this.ID, comment);
        }
    }

}
