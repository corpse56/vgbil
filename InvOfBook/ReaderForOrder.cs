using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using XMLConnections;

namespace ReaderForOrder
{
    public class Reader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="sess"></param>
        /// <param name="type"> 0 - читатель, 1 - удаленный читатель, 2 - сотрудник</param>
        public Reader(string login, string sess, int type)
        {
            if (type == 0)
            {
                this.Type = 0;
                SqlDataAdapter DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
                DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberReader = " + login;
                DataSet DS = new DataSet();
                int recc = DA.Fill(DS, "Reader");
                this.ID = DS.Tables["Reader"].Rows[0]["NumberReader"].ToString();
                //this.Login = DS.Tables["Reader"].Rows[0]["login"].ToString();
                this.FIO = DS.Tables["Reader"].Rows[0]["FamilyName"].ToString() + " " + DS.Tables["Reader"].Rows[0]["Name"].ToString() + " " + DS.Tables["Reader"].Rows[0]["FatherName"].ToString();
                this.Session = sess;
                DA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + login;
                DS = new DataSet();
                DA.Fill(DS, "t");
                string retval = string.Empty;
                foreach (DataRow r in DS.Tables["t"].Rows)
                {
                    if ((int)r["IDReaderRight"] == 3)
                    {
                        retval += "Сотрудник ВГБИЛ; ";
                    }
                    if ((int)r["IDReaderRight"] == 4)
                    {
                        retval += "Индивидуальный абонемент; ";
                    }
                    if ((int)r["IDReaderRight"] == 5)
                    {
                        retval += "Персональный абонемент; ";
                    }
                    if ((int)r["IDReaderRight"] == 6)
                    {
                        retval += "Коллективный абонемент; ";
                    }
                }
                this.Abonement = retval.TrimEnd();
            }
            if (type == 1)
            {
                this.Type = 1;
                SqlDataAdapter DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
                DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberReader = " + login;
                DataSet DS = new DataSet();
                int recc = DA.Fill(DS, "Reader");
                this.ID = DS.Tables["Reader"].Rows[0]["NumberReader"].ToString();
                //this.Login = DS.Tables["Reader"].Rows[0]["login"].ToString();
                this.FIO = DS.Tables["Reader"].Rows[0]["FamilyName"].ToString() + " " + DS.Tables["Reader"].Rows[0]["Name"].ToString() + " " + DS.Tables["Reader"].Rows[0]["FatherName"].ToString();
                this.Session = sess;
            }
        }
        public Reader(string login, string sess)
        {
            this.Type = 0;
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
            DA.SelectCommand.CommandText = "select * from Readers.dbo.Main where NumberReader = " + login;
            DataSet DS = new DataSet();
            int recc = DA.Fill(DS, "Reader");
            this.ID = DS.Tables["Reader"].Rows[0]["NumberReader"].ToString();
            //this.Login = DS.Tables["Reader"].Rows[0]["login"].ToString();
            this.FIO = DS.Tables["Reader"].Rows[0]["FamilyName"].ToString() + " " + DS.Tables["Reader"].Rows[0]["Name"].ToString() + " " + DS.Tables["Reader"].Rows[0]["FatherName"].ToString();
            this.Session = sess;
            DA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + login;
            DS = new DataSet();
            DA.Fill(DS, "t");
            string retval = string.Empty;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                if ((int)r["IDReaderRight"] == 3)
                {
                    retval += "Сотрудник ВГБИЛ; ";
                }
                if ((int)r["IDReaderRight"] == 4)
                {
                    retval += "Индивидуальный абонемент; ";
                }
                if ((int)r["IDReaderRight"] == 5)
                {
                    retval += "Персональный абонемент; ";
                }
                if ((int)r["IDReaderRight"] == 6)
                {
                    retval += "Коллективный абонемент; ";
                }
                this.Abonement = retval.TrimEnd();
            }
        }
        public string FIO;
        public string ID;
        public string Session;
        public string Abonement;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="sess"></param>
        /// <param name="type"> 0 - читатель, 1 - удаленный читатель, 2 - сотрудник</param>
        public int Type;
    }

}
