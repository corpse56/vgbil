using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Circulation
{
    public partial class Form9 : Form
    {
        dbReader reader;
        public Form9(dbReader reader_)
        {
            InitializeComponent();
            reader = reader_;
            label2.Text = reader.FIO;
            MethodsForCurBase.FormTable(reader,dataGridView1);
        }

    }
    public static class MethodsForCurBase
    {
        public static string GetRightBoolValue(string value_)
        {
            string tmp = value_.ToString();
            if (value_.ToString() == "True")
            {
                return "да";
            }
            if (value_.ToString() == "False")
            {
                return "нет";
            }
            return value_;
        }
        public static string GetValueFromList(string colname, string value_)
        {
            DataSet DS = new DataSet();
            int cnt;
            switch (colname)
            {
                case "Document":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Document where IDDocument = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameDocument"].ToString();
                    }
                case "Education":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Education where IDEducation = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducation"].ToString();
                    }
                case "AcademicDegree":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AcademicDegree where IDAcademicDegree = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameAcademicDegree"].ToString();
                    }
                case "WorkDepartment":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from BJVVV..LIST_8 where ID = " + value_;
                        int c = Conn.SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NAME"].ToString();
                    }
                case "EducationalInstitution":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..EducationalInstitution where IDEducationalInstitution = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducationalInstitution"].ToString();
                    }
                case "ClassInfringer":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..ClassInfringer where IDClassInfringer = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameClassInfringer"].ToString();
                    }
                case "InfringerEditor":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "PenaltyID":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Penalty where IDPenalty = " + value_;
                        int c = Conn.SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NamePenalty"].ToString();
                    }
                case "EditorCreate":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorEnd":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorNow":
                    {
                        Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = Conn.SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
            }
            return value_;
        }
        public static void FormTable(dbReader reader, DataGridView dataGridView1)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + reader.id;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "lll");
            dataGridView1.Columns.Add("value", "");
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 296;
            dataGridView1.Columns[0].Width = 436;
            int i = 0;
            Dictionary<string, string> FieldsCaptions = new Dictionary<string, string>();
            Conn.SQLDA.SelectCommand.CommandText = "      USE Readers;  " +
                                                   "SELECT " +
                                                   "             [Table Name] = OBJECT_NAME(c.object_id),  " +
                                                   "             [Column Name] = c.name,  " +
                                                   "             [Description] = ex.value   " +
                                                   "       FROM   " +
                                                   "             sys.columns c   " +
                                                   "       LEFT OUTER JOIN   " +
                                                   "             sys.extended_properties ex   " +
                                                   "       ON   " +
                                                   "             ex.major_id = c.object_id  " +
                                                   "             AND ex.minor_id = c.column_id   " +
                                                   "             AND ex.name = 'MS_Description'   " +
                                                   "       WHERE   " +
                                                   "             OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0   " +
                                                   "             AND OBJECT_NAME(c.object_id) = 'Main' " +
                                                   "       ORDER  " +
                                                   "             BY OBJECT_NAME(c.object_id), c.column_id;";
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            //DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "fldcap");
            foreach (DataRow r in DS.Tables["fldcap"].Rows)
            {
                FieldsCaptions.Add(r["Column Name"].ToString(), r["Description"].ToString());
            }
            foreach (DataColumn col in DS.Tables["lll"].Columns)
            {
                if ((col.ColumnName == "AbonementType") ||
                    (col.ColumnName == "SheetWithoutCard") || 
                    (col.ColumnName == "Password") || 
                    (col.ColumnName == "FamilyNameFind") || 
                    (col.ColumnName == "NameFind") || 
                    (col.ColumnName == "FatherNameFind") || 
                    (col.ColumnName == "Interest") ||
                    (col.ColumnName == "DocumentNumber") ||
                    (col.ColumnName == "DateRegistration") ||
                    (col.ColumnName == "DateReRegistration") ||
                    (col.ColumnName == "MobileTelephone") ||
                    (col.ColumnName == "WorkCity") ||
                    (col.ColumnName == "WorkName") ||
                    (col.ColumnName == "WorkPosition") ||
                    (col.ColumnName == "WorkTelephone") ||
                    (col.ColumnName == "WorkTelephoneAdd") ||
                    (col.ColumnName == "WorkDepartment") ||
                    (col.ColumnName == "RegistrationPostOffice") ||
                    (col.ColumnName == "RegistrationCountry") ||
                    (col.ColumnName == "RegistrationRegion") ||
                    (col.ColumnName == "RegistrationProvince") ||
                    (col.ColumnName == "RegistrationDistrict") ||
                    (col.ColumnName == "RegistrationCity") ||
                    (col.ColumnName == "RegistrationStreet") ||
                    (col.ColumnName == "RegistrationHouse") ||
                    (col.ColumnName == "RegistrationFlat") ||
                    (col.ColumnName == "RegistrationTelephone") ||
                    (col.ColumnName == "LivePostOffice") ||
                    (col.ColumnName == "LiveCountry") ||
                    (col.ColumnName == "LiveRegion") ||
                    (col.ColumnName == "LiveProvince") ||
                    (col.ColumnName == "LiveDistrict") ||
                    (col.ColumnName == "LiveCity") ||
                    (col.ColumnName == "LiveStreet") ||
                    (col.ColumnName == "LiveHouse") ||
                    (col.ColumnName == "LiveFlat") ||
                    (col.ColumnName == "LiveTelephone") ||
                    (col.ColumnName == "WordReg") ||
                    (col.ColumnName == "Email") ||
                    (col.ColumnName == "NumberSC") ||
                    (col.ColumnName == "SerialSC") ||
                    (col.ColumnName == "Document") ||
                    (col.ColumnName == "ClassInfringer") ||
                    (col.ColumnName == "SheetWithoutCardData") ||
                    (col.ColumnName == "SpecialNote") ||
                    (col.ColumnName == "EditorCreate") ||
                    (col.ColumnName == "EditorEnd") ||
                    (col.ColumnName == "EditEndDate") ||
                    (col.ColumnName == "EditorNow") ||
                    (col.ColumnName == "SelfRecord") ||
                    (col.ColumnName == "ReRegistration") ||
                    (col.ColumnName == "AbonementType") ||
                    (col.ColumnName == "InBlackList") ||
                    (col.ColumnName == "Photo") ||
                    (col.ColumnName == "InputAlwaysDate") 
                   )
                {
                    continue;
                }
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = FieldsCaptions[col.ColumnName];
                string value = DS.Tables["lll"].Rows[0][col].ToString();
                value = MethodsForCurBase.GetValueFromList(col.ColumnName, value);
                value = MethodsForCurBase.GetRightBoolValue(value);
                if (DS.Tables["lll"].Rows[0][col].GetType() == typeof(DateTime))
                {
                    value = ((DateTime)DS.Tables["lll"].Rows[0][col]).ToShortDateString();
                }
                if (dataGridView1.Rows[i].HeaderCell.Value.ToString() == "Инвалидность")
                {
                    if (value == "1")
                    {
                        value = "нет";
                    }
                    else
                    {
                        value = "да";
                    }
                }
                dataGridView1.Rows[i].Cells[0].Value = value;
                i++;
            }
            /*Conn.SQLDA.SelectCommand.CommandText = "select B.NameInterest intr from Readers..Interest A inner join Readers..InterestList B on A.IDInterest = B.IDInterest where IDReader = " + reader.id;
            Conn.SQLDA.Fill(DS, "itrs");
            foreach (DataRow r in DS.Tables["itrs"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Интерес";
                dataGridView1.Rows[i].Cells[0].Value = r["intr"].ToString();
                i++;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select B.NameLanguage lng from Readers..Language A inner join Readers..LanguageList B on A.IDLanguage = B.IDLanguage where IDReader = " + reader.id;
            Conn.SQLDA.Fill(DS, "lng");
            foreach (DataRow r in DS.Tables["lng"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Язык";
                dataGridView1.Rows[i].Cells[0].Value = r["lng"].ToString();
                i++;
            }*/
            Conn.SQLDA.SelectCommand.CommandText = "select B.SHORTNAME dep from Readers..ReaderRight A inner join BJVVV..LIST_8 B on A.IDOrganization = B.ID where A.IDReader = " + reader.id;
            Conn.SQLDA.Fill(DS, "d");
            foreach (DataRow r in DS.Tables["d"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Отдел (если сотрудник)";
                dataGridView1.Rows[i].Cells[0].Value = r["dep"].ToString();
                i++;
            }
        }
    }
}
