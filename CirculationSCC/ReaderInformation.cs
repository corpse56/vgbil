using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;
namespace Circulation
{
    public partial class ReaderInformation : Form
    {
        ReaderVO reader;
        Form1 f1;
        public ReaderInformation(ReaderVO reader_, Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
            reader = reader_;
            label2.Text = reader.FIO;
            pictureBox1.Image = reader.Photo;
            MethodsForCurBase.FormTable(reader,dataGridView1);
            //DisplayCommNote();
            //RegInMos();
            label6.Text = "Дата последней отправки письма: " +reader.GetLastDateEmail();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //EmailSending f24 = new EmailSending(f1, this.reader);
            //if (f24.canshow)
            //    f24.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ViewFullSizePhoto fullsize = new ViewFullSizePhoto(pictureBox1.Image);
            fullsize.ShowDialog();

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
            SqlDataAdapter SQLDA = new SqlDataAdapter();
            SQLDA.SelectCommand = new SqlCommand();
            SQLDA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationSCC"));

            DataSet DS = new DataSet();
            int cnt;
            switch (colname)
            {
                case "Document":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..Document where IDDocument = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameDocument"].ToString();
                    }
                case "Education":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..Education where IDEducation = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducation"].ToString();
                    }
                case "AcademicDegree":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..AcademicDegree where IDAcademicDegree = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameAcademicDegree"].ToString();
                    }
                case "WorkDepartment":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from BJSCC..LIST_8 where ID = " + value_;
                        int c = SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NAME"].ToString();
                    }
                case "EducationalInstitution":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..EducationalInstitution where IDEducationalInstitution = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameEducationalInstitution"].ToString();
                    }
                case "ClassInfringer":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..ClassInfringer where IDClassInfringer = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameClassInfringer"].ToString();
                    }
                case "InfringerEditor":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "PenaltyID":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..Penalty where IDPenalty = " + value_;
                        int c = SQLDA.Fill(DS, "tmp");
                        if (c == 0)
                        {
                            return "(нет)";
                        }
                        return DS.Tables["tmp"].Rows[0]["NamePenalty"].ToString();
                    }
                case "EditorCreate":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorEnd":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
                case "EditorNow":
                    {
                        SQLDA.SelectCommand.CommandText = "select * from Readers..[User] where IDUser = " + value_;
                        cnt = SQLDA.Fill(DS, "tmp");
                        return (cnt == 0) ? "" : DS.Tables["tmp"].Rows[0]["NameUser"].ToString();
                    }
            }
            return value_;
        }
        public static void FormTable(ReaderVO reader, DataGridView dataGridView1)
        {
            SqlDataAdapter SQLDA = new SqlDataAdapter();
            SQLDA.SelectCommand = new SqlCommand();
            SQLDA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationSCC"));
            SQLDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + reader.ID;
            DataSet DS = new DataSet();
            SQLDA.Fill(DS, "lll");
            dataGridView1.Columns.Add("value", "");
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 296;
            dataGridView1.Columns[0].Width = 436;
            int i = 0;
            Dictionary<string, string> FieldsCaptions = new Dictionary<string, string>();
            SQLDA.SelectCommand.CommandText = "      USE Readers;  " +
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
            SQLDA.Fill(DS, "fldcap");
            foreach (DataRow r in DS.Tables["fldcap"].Rows)
            {
                FieldsCaptions.Add(r["Column Name"].ToString(), r["Description"].ToString());
            }
            foreach (DataColumn col in DS.Tables["lll"].Columns)
            {
                if ((col.ColumnName == "Document") || (col.ColumnName == "DocumentNumber") || (col.ColumnName == "Photo") || (col.ColumnName == "Photo") || (col.ColumnName == "AbonementType") || (col.ColumnName == "SheetWithoutCard") || (col.ColumnName == "Password") || (col.ColumnName == "FamilyNameFind") || (col.ColumnName == "NameFind") || (col.ColumnName == "FatherNameFind") || (col.ColumnName == "Interest"))
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
                dataGridView1.Rows[i].Cells[0].Value = value;
                i++;
            }
            /*SQLDA.SelectCommand.CommandText = "select B.NameInterest intr from Readers..Interest A inner join Readers..InterestList B on A.IDInterest = B.IDInterest where IDReader = " + reader.ID;
            SQLDA.Fill(DS, "itrs");
            foreach (DataRow r in DS.Tables["itrs"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Интерес";
                dataGridView1.Rows[i].Cells[0].Value = r["intr"].ToString();
                i++;
            }*/
            /*SQLDA.SelectCommand.CommandText = "select B.NameLanguage lng from Readers..Language A inner join Readers..LanguageList B on A.IDLanguage = B.IDLanguage where IDReader = " + reader.ID;
            SQLDA.Fill(DS, "lng");
            foreach (DataRow r in DS.Tables["lng"].Rows)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = "Язык";
                dataGridView1.Rows[i].Cells[0].Value = r["lng"].ToString();
                i++;
            }*/
        }
    }
}
