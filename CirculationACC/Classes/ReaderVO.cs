using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;

namespace Circulation
{
    public class ReaderVO
    {
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



        public int ID;
        public string Family;
        public string Name;
        public string Father;
        public Image Photo;
        public string FIO;
        public string BAR;
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
