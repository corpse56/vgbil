using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace UpDgtPeriodic
{
    public partial class fAddJPGChoice : Form
    {

        DataTable dtTitleSource;
        DataTable dtFilterTitleSource;
        bool can_load_years = false;
        string Login;
        bool ISPDF;
        public fAddJPGChoice(string Login_, bool ISPDF_)
        {
            this.Login = Login_;
            this.ISPDF = ISPDF_;
            InitializeComponent();
            dtTitleSource = GetTitles();
            dtFilterTitleSource = GetTitles(); 
            
            listBox1.DataSource = dtTitleSource;
            listBox1.DisplayMember = "POLE";
            listBox1.ValueMember = "VVERH";
            
            comboBox1.DataSource = GetEbookYears(listBox1.SelectedValue.ToString());
            comboBox1.DisplayMember = "POLE";
            comboBox1.ValueMember = "nomcop";

            can_load_years = true;
            if (ISPDF)
            {
                this.Text = "Выберите заглавие и год периодического издания, к которому вы собираетесь привязать PDF";
            }
            else
            {
                this.Text = "Выберите заглавие и год периодического издания, к которому вы собираетесь привязать JPG";
            }
        }
        public DataTable GetTitles()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(XMLConnections.XmlConnections.GetConnection("/Connections/SCANINFO"));
            da.SelectCommand.CommandText = "select * from PERIOD..[PI] where IDF = 121 and POLE != '' order by POLE ";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                r["POLE"] = RemoveDiacritics(r["POLE"].ToString());
            }
            return ds.Tables["t"];

        }
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public DataTable GetEbookYears(string IDZPIN)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(XMLConnections.XmlConnections.GetConnection("/Connections/SCANINFO"));
            da.SelectCommand.CommandText = "select A.*, B.IDZ nomcop from PERIOD..[PI] A " +
                                           " left join PERIOD..[PI] B on A.IDZ = B.VVERH  " +
                                           " left join PERIOD..[PI] C on B.IDZ = C.VVERH  " +
                                           "  where A.VVERH = " + IDZPIN + " and A.IDF = 131 and B.IDF = 211 and C.IDF = 363 " +
                                           "  and C.POLE = 'e-book' " +
                                           "  order by A.POLE  ";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            return ds.Tables["t"];

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            can_load_years = false;

            if (textBox1.Text == "")
            {
                listBox1.DataSource = dtTitleSource;
                return;
            }
            //string f = dtTitleSource.Rows[0]["POLE"].ToString();
            //bool t = f.StartsWith(textBox1.Text);
            string tmp = "XXÈ";
            string tmp1 = "XXE";
            //CompareInfo ci = 
            //int res = String.Compare(tmp,tmp1, StringComparison.CurrentCulture);
            var query = from DataRow x in dtTitleSource.AsEnumerable()
                        where x["POLE"].ToString().ToLower().Contains(textBox1.Text.ToLower().Trim())
                        //orderby x["title"]
                        select x;

            dtFilterTitleSource.Rows.Clear();
            query.CopyToDataTable(dtFilterTitleSource, LoadOption.OverwriteChanges);
            listBox1.DataSource = dtFilterTitleSource;
            can_load_years = true;
            listBox1_SelectedIndexChanged(sender, e);
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (can_load_years)
            {
                if (listBox1.SelectedValue == null)
                {
                    if (comboBox1.Items.Count != 0)
                    {
                        comboBox1.DataSource = null;
                        return;
                    }
                }
                if (listBox1.SelectedValue != null)
                {
                    string value = listBox1.SelectedValue.ToString();
                    comboBox1.DataSource = GetEbookYears(value);
                    comboBox1.DisplayMember = "POLE";
                    comboBox1.ValueMember = "nomcop";

                }
                //can_load_years = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ISPDF)
            {
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Выберите год!");
                    return;
                }
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = "pdf";
                ofd.Filter = "PDF files (*.pdf)|*.pdf";

                ofd.Multiselect = false;
                ofd.Title = "Выберите PDF-файл";
                ofd.ShowDialog();
                if (ofd.FileName == "") return;
                string PIN = GetPinByIDZ(listBox1.SelectedValue.ToString());
                fPreviewPDF fpdf = new fPreviewPDF((int)listBox1.SelectedValue, comboBox1.Text, ofd.FileName, PIN, this.Login);
                fpdf.ShowDialog();

            }
            else
            {
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Выберите год!");
                    return;
                }

                FolderBrowserDialog fb = new FolderBrowserDialog();
                fb.ShowNewFolderButton = false;
                fb.Description = @"Выберите папку с годом, внутри которой находятся папки с номерами изданий! (Пример: ""C:\Правда\1991"")";
                fb.SelectedPath = @"f:\1983";
                fb.ShowDialog();
                string path = fb.SelectedPath;
                //string YEAR_IDZ = comboBox1.SelectedValue.ToString();
                DirectoryInfo di = new DirectoryInfo(fb.SelectedPath);

                string FolderYear = di.Name;
                string selectedYear = comboBox1.SelectedText;
                selectedYear = comboBox1.Text;
                if (FolderYear != selectedYear)
                {
                    MessageBox.Show("Выбранный год и имя папки с годом не совпадают!");
                    return;
                }

                fPreview fp;
                string PIN = GetPinByIDZ(listBox1.SelectedValue.ToString());
                try
                {
                    fp = new fPreview((int)listBox1.SelectedValue, FolderYear, fb.SelectedPath, comboBox1.Text, PIN, this.Login);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                fp.ShowDialog();
            }
        }
        private string GetPinByIDZ(string IDZ)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(XMLConnections.XmlConnections.GetConnection("/Connections/SCANINFO"));
            da.SelectCommand.CommandText = "select POLE from PERIOD..[PI] where IDZ = "+IDZ;
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            return ds.Tables["t"].Rows[0][0].ToString();

        }
    }
}
