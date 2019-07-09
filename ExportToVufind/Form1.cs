﻿using System;
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
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Litres;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.ExportToVufind.Vufind;

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
        LitresVuFindConverter litres = new LitresVuFindConverter("litres");
        PeriodVuFindConverter period = new PeriodVuFindConverter("period");
        PearsonVuFindConverter pearson = new PearsonVuFindConverter("pearson");
        JBHVuFindConverter jbh = new JBHVuFindConverter();
        CERFVufindConverter cerf = new CERFVufindConverter("CERF");
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
            //литрес обрезать большой файл для теста. первые 50 записей
            //XDocument xdoc = XDocument.Load(@"f:\litres_source.xml");
            //XmlWriter writ = XmlTextWriter.Create(@"F:\litres_example.xml");
            //var books = xdoc.Descendants("updated-book").Take(50);
            //writ.WriteStartElement("litresBooks");
            //foreach (XElement elt in books)
            //{
            //    elt.WriteTo(writ);
            //}

            //writ.Flush();
            //writ.WriteEndElement();
            //writ.Close();

            JBHVuFindConverter jbh = new JBHVuFindConverter();
            jbh.GetSource();


        }

       

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

            redkostj.Export();
            bjvvv.Export();
            bjacc.Export();
            bjfcc.Export();
            bjscc.Export();
            //brit_sovet.Export();
            //litres.GetLitresSourceData();
            //litres.Export();
            //pearson.GetPearsonSourceData();
            //pearson.Export();
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
        private void bCERFExportAll_Click(object sender, EventArgs e)
        {
            StartTimer();
            cerf.Export();
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

            switch (fund.ToUpper())
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

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void bExportSingleCover_Click(object sender, EventArgs e)
        {
            string fnd = txtSingleRecordId.Text.Substring(0, txtSingleRecordId.Text.IndexOf("_"));
            int id = int.Parse(txtSingleRecordId.Text.Substring(txtSingleRecordId.Text.IndexOf("_")+1));

            BJVuFindConverter converter = new BJVuFindConverter(fnd);
            converter.ExportSingleCover(id);
        }

    }
}
