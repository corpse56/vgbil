using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Security;
using System.Xml.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExportBJ_XML.classes;

namespace ExportBJ_XML
{
    public partial class Form1 : Form
    {
        BJVuFindConverter bjvvv = new BJVuFindConverter("BJVVV");
        BJVuFindConverter redkostj = new BJVuFindConverter("REDKOSTJ");
        BJVuFindConverter bjacc = new BJVuFindConverter("BJACC");
        BJVuFindConverter bjfcc = new BJVuFindConverter("BJFCC");
        BJVuFindConverter bjscc = new BJVuFindConverter("BJSCC");
        //BJVuFindConverter brit_sovet = new BJVuFindConverter("BRIT_SOVET");
        LitresVuFindConverter litres = new LitresVuFindConverter();
        PeriodVuFindConverter period = new PeriodVuFindConverter();
        PearsonVuFindConverter pearson = new PearsonVuFindConverter();
        JBHVuFindConverter jbh = new JBHVuFindConverter();

        Stopwatch sw;

        public Form1()
        {
            InitializeComponent();
            
            bjvvv.RecordExported += new EventHandler(RecordExported);
            bjacc.RecordExported += new EventHandler(RecordExported);
            //brit_sovet.RecordExported += new EventHandler(RecordExported);
            bjfcc.RecordExported += new EventHandler(RecordExported);
            bjscc.RecordExported += new EventHandler(RecordExported);
            redkostj.RecordExported += new EventHandler(RecordExported);
            litres.RecordExported += new EventHandler(RecordExported);
            pearson.RecordExported += new EventHandler(RecordExported);
            period.RecordExported += new EventHandler(RecordExported);
            
        }

    

        void RecordExported(object sender, EventArgs e)
        {
            label2.Text = ((VuFindConverterEventArgs)e).RecordId;
            Application.DoEvents();
        }
      

       

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath, Encoding.Default))
            {
                
                string[] headers = sr.ReadLine().Split(';');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header.Trim('"'));
                }
                int g = 1;
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(';');
                    for (int i = 0; i < rows.Count(); i++ )
                    {
                        rows[i] = rows[i].Trim('"');
                    }

                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                    if (g++ > 1000) break;
                }

            }


            return dt;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //pearson.GetPearsonSourceData();
            
            //XDocument xdoc = XDocument.Load(@"f:\import\bjvvv.xml");
            //var books = xdoc.Descendants("doc");
            //var dublicates = xdoc
            //            .Descendants("doc")
            //            .GroupBy(g => (string)g.Attribute("id"))
            //            .Where(g => g.Count() > 1)
            //            .Select(g => g.Key);
            //textBox1.Text = "";



            //string text = "Tendances dans l'economie mondiale; &#x0; 23/ Conseil scientifique hongrois d'economie mondaile";
            //XElement x = new XElement("bb",text);
            //string xmlText = SecurityElement.Escape(text);

            //Exporter exr = new Exporter(this);
            //exr.ExportSingleRecord("BJVVV", 1449315);
            //test.GetBookStatusRequest sdds = new ExportBJ_XML.test.GetBookStatusRequest();
            //test.ServiceSoapClient cli = new ExportBJ_XML.test.ServiceSoapClient();
            //string[] nnn = new string[13];
            //nnn[0] = "REDKOSTJ_1397";
            //nnn[1] = "BJVVV_1299980";
            //nnn[2] = "BJVVV_1301198";
            //nnn[3] = "BJVVV_1299121";
            //nnn[4] = "BJVVV_1304618";
            //nnn[5] = "BJVVV_1375197";
            //nnn[6] = "BJVVV_1186227";
            //nnn[7] = "BJVVV_1130210";
            //nnn[8] = "BJVVV_128956";
            //nnn[9] = "BJVVV_1375197";
            //nnn[10] = "BJVVV_137519788";
            //nnn[11] = "BJVVV_1375199";
            //nnn[12] = "REDKOSTJ_1397";

            






            //test.ArrayOfString arr = new ExportBJ_XML.test.ArrayOfString();
            //arr.Add(nnn[0]);
            //arr.Add(nnn[1]);
            //arr.Add(nnn[2]);
            //arr.Add(nnn[3]);
            //arr.Add(nnn[4]);
            //arr.Add(nnn[5]);
            //arr.Add(nnn[6]);
            //arr.Add(nnn[7]);
            //arr.Add(nnn[8]);
            //arr.Add(nnn[9]);
            //arr.Add(nnn[10]);
            //arr.Add(nnn[11]);
            //arr.Add(nnn[12]);
            //string res = cli.GetBookStatus(arr);

            //SqlDataAdapter da = new SqlDataAdapter();

            //da = new SqlDataAdapter();
            //da.InsertCommand = new SqlCommand();
            //da.InsertCommand.Connection = new SqlConnection();
            //da.InsertCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            //da.InsertCommand.Connection.Open();
            //StreamReader sr = new StreamReader(@"f:\Lib_100S30708.txt");
            //string account;
            //while (sr.Peek() >= 0)
            //{
            //    account = sr.ReadLine();
            //    da.InsertCommand.Parameters.Clear();
            //    da.InsertCommand.Parameters.AddWithValue("login", account.Split(',')[0]);
            //    da.InsertCommand.Parameters.AddWithValue("pwd", account.Split(',')[1]);
            //    da.InsertCommand.CommandText = "insert into LITRES..ACCOUNTS (LRLOGIN,LRPWD,CREATED) values (@login, @pwd, getdate())";
            //    da.InsertCommand.ExecuteNonQuery();
            //}
            //da.InsertCommand.Connection.Close();
            
        }

       
        #region вставить новые пароли литрес

        //SqlDataAdapter da = new SqlDataAdapter();
        ////da.SelectCommand = new SqlCommand();
        ////da.SelectCommand.Connection = new SqlConnection();
        ////da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536";
        ////da.SelectCommand.CommandText = "select * from BJVVV..DATAEXT where MNFIELD = 230";
        ////DataSet ds = new DataSet();
        ////int i = da.Fill(ds, "t");
        ////da.SelectCommand.CommandText = "select * from BJVVV..DATAEXTPLAIN where ID = 3";
        ////i = da.Fill(ds, "t");

        //da = new SqlDataAdapter();
        //da.InsertCommand = new SqlCommand();
        //da.InsertCommand.Connection = new SqlConnection();
        //da.InsertCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
        //da.InsertCommand.Connection.Open();
        //StreamReader sr = new StreamReader(@"f:\Lib_100S25642.txt");
        //string account;
        //while (sr.Peek() >= 0)
        //{
        //    account = sr.ReadLine();
        //    da.InsertCommand.Parameters.Clear();
        //    da.InsertCommand.Parameters.AddWithValue("login",account.Split(',')[0]);
        //    da.InsertCommand.Parameters.AddWithValue("pwd",account.Split(',')[1]);
        //    da.InsertCommand.CommandText = "insert into LITRES..ACCOUNTS (LRLOGIN,LRPWD,CREATED) values (@login, @pwd, getdate())";
        //    da.InsertCommand.ExecuteNonQuery();
        //}
        //da.InsertCommand.Connection.Close();


        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sw == null) return;
            Application.DoEvents();
            label1.Text = "Потрачено: " + sw.Elapsed.Days.ToString() + " дней " + sw.Elapsed.Hours.ToString() + " часов " + sw.Elapsed.Minutes.ToString() + " минут " + sw.Elapsed.Seconds.ToString() + " секунд ";
        }
        
        private void StartTimer()
        {
            sw = new Stopwatch();
            sw.Start();
            label3.Text = "Начато в " + DateTime.Now.ToLongTimeString();//.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }
        private void StopTimer()
        {
            sw.Stop();
            label1.Text = "Закончено. Потрачено: " + sw.Elapsed.Days.ToString() + " дней " + sw.Elapsed.Hours.ToString() + " часов " + sw.Elapsed.Minutes.ToString() + " минут " + sw.Elapsed.Seconds.ToString() + " секунд ";
            label4.Text = "Закочено в " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        }
        private void bjvvv_Click(object sender, EventArgs e)
        {
            //StartTimer();
            //Exporter exr = new Exporter(this);
            //exr.BJVVV();
            //StopTimer();

            StartTimer();
            bjvvv.Export();
            StopTimer();


        }

        private void redkostj_Click(object sender, EventArgs e)
        {
            StartTimer();
            redkostj.Export();
            StopTimer();
        }

        private void all_Click(object sender, EventArgs e)
        {
            StartTimer();

            bjacc.Export();
            bjfcc.Export();
            bjscc.Export();
            bjvvv.Export();
            redkostj.Export();
            //brit_sovet.Export();
            //pearson.GetPearsonSourceData();
            //pearson.Export();
            //litres.GetLitresSourceData();
            //litres.Export();
            //period.Export();

            StopTimer();

        }

        private void brit_sovet_Click(object sender, EventArgs e)
        {
            StartTimer();
            //brit_sovet.Export();
            StopTimer();
        }

        private void bjacc_Click(object sender, EventArgs e)
        {
            StartTimer();
            bjacc.Export();
            StopTimer();
        }

        private void bjfcc_Click(object sender, EventArgs e)
        {
            StartTimer();
            bjfcc.Export();
            StopTimer();
        }

        private void bjscc_Click(object sender, EventArgs e)
        {
            StartTimer();
            bjscc.Export();
            StopTimer();
        }

        private void period_Click(object sender, EventArgs e)
        {
            StartTimer();
            period.Export();
            StopTimer();
        }

        private void pearson_Click(object sender, EventArgs e)
        {
            StartTimer();
            pearson.Export();
            StopTimer();
        }

        private void litres_Click(object sender, EventArgs e)
        {
            StartTimer();
            litres.Export();
            StopTimer();
        }

        private void btnJBH_Click(object sender, EventArgs e)
        {
            StartTimer();
            jbh.Export();
            StopTimer();
        }

        private void bjvvvCovers_Click(object sender, EventArgs e)
        {
            StartTimer();
            bjvvv.ExportCovers();
            StopTimer();

            

        }

        private void litresCovers_Click(object sender, EventArgs e)
        {
            StartTimer();
            litres.ExportCovers();
            StopTimer();
        }

        private void allCovers_Click(object sender, EventArgs e)
        {
            StartTimer();

            bjvvv.ExportCovers();
            redkostj.ExportCovers();
            //brit_sovet.ExportCovers();
            bjacc.ExportCovers();
            bjfcc.ExportCovers();
            bjscc.ExportCovers();
            //litres.ExportCovers();
            //pearson.ExportCovers();
            //period.ExportCovers();


            StopTimer();
        }

        private void getLitresSource_Click(object sender, EventArgs e)
        {
            litres.GetLitresSourceData();
        }

        private void getPearsonSource_Click(object sender, EventArgs e)
        {
            pearson.GetPearsonSourceData();
        }

        private void pearsonCovers_Click(object sender, EventArgs e)
        {
            StartTimer();
            pearson.ExportCovers();
            StopTimer();
        }

        private void exportSingleRecord_Click(object sender, EventArgs e)
        {
            if (txtSingleRecordId.Text == "")
            {
                MessageBox.Show("Введите Id записи");
                return;
            }

            string fund = txtSingleRecordId.Text.Substring(0,txtSingleRecordId.Text.LastIndexOf("_"));
            int id = int.Parse(txtSingleRecordId.Text.Substring(txtSingleRecordId.Text.LastIndexOf("_")+1));

            switch (fund)
            {
                case "BJVVV":
                    bjvvv.ExportSingleRecord(id);
                    break;
                case "REDKOSTJ":
                    redkostj.ExportSingleRecord(id);
                    break;
                //case "BRIT_SOVET":
                //    brit_sovet.ExportSingleRecord(id);
                //    break;
                case "BJACC":
                    bjacc.ExportSingleRecord(id);
                    break;
                case "BJFCC":
                    bjfcc.ExportSingleRecord(id);
                    break;
                case "BJSCC":
                    bjscc.ExportSingleRecord(id);
                    break;
                case "LITRES":
                    litres.ExportSingleRecord(id);
                    break;
                case "PEARSON":
                    pearson.ExportSingleRecord(id);
                    break;
                case "PERIOD":
                    period.ExportSingleRecord(id);
                    break;
            }
        }

        private void btnGetJBHSource_Click(object sender, EventArgs e)
        {
            //просто конвертируем РТФ в обычный текст
            jbh.GetSource();

        }


      

      
    }
}
