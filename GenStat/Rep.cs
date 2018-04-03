using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace GenStat
{
    public partial class Rep : Form
    {
        public Rep(DataTable source,string title,int reptype)
        {
            InitializeComponent();
            ReportMainFirst r;
            ReportMainFirstNEW rn;
            switch (reptype)
            {
                case 1://Спавка по обслуживанию
                    r = new ReportMainFirst();
                    ((TextObject)r.Section2.ReportObjects["Text5"]).Text = title;
                    ((TextObject)r.Section2.ReportObjects["Text2"]).Text = "Наименование справки";
                    r.Database.Tables["RDT"].SetDataSource(source);
                    RViewer.ReportSource = r;
                    break;
                case 2://Пополнение фон=да новыми поступлениями
                    r = new ReportMainFirst();
                    ((TextObject)r.Section2.ReportObjects["Text5"]).Text = title;
                    ((TextObject)r.Section2.ReportObjects["Text2"]).Text = "Наименование раздела";
                    r.Database.Tables["RDT"].SetDataSource(source);
                    RViewer.ReportSource = r;
                    break;
                case 3://количество посетителей библиотеки
                    r = new ReportMainFirst();
                    ((TextObject)r.Section2.ReportObjects["Text5"]).Text = title;
                    ((TextObject)r.Section2.ReportObjects["Text2"]).Text = "Наименование пункта посещения";
                    r.Database.Tables["RDT"].SetDataSource(source);
                    RViewer.ReportSource = r;
                    break;
                case 0://среднее время обслуживания поэтажно
                    Report51 r51 = new Report51();
                    r51 = new Report51();
                    ((TextObject)r51.Section2.ReportObjects["Text5"]).Text = title;
                    //((TextObject)rn.Section2.ReportObjects["Text6"]).Text = "Этаж";
                    //((TextObject)rn.Section2.ReportObjects["Text4"]).Text = "Время (мин)";
                    //((TextObject)rn.Section2.ReportObjects["Text7"]).Text = "Кол-во";
                    r51.Database.Tables["RDT51"].SetDataSource(source);
                    RViewer.ReportSource = r51;
                    break;
                case 4://количество посетителей зада без услуг книговыдачи
                    rn = new ReportMainFirstNEW();
                    ((TextObject)rn.Section2.ReportObjects["Text5"]).Text = title;
                    //((TextObject)r.Section2.ReportObjects["Text2"]).Text = "Наименование подразделения библиотеки";
                    rn.Database.Tables["RDT4"].SetDataSource(source);
                    RViewer.ReportSource = rn;
                    break;
                case 5://количество читателей получивших литературу
                    rn = new ReportMainFirstNEW();
                    ((TextObject)rn.Section2.ReportObjects["Text5"]).Text = title;
                    rn.Database.Tables["RDT4"].SetDataSource(source);
                    //((TextObject)rn.Section2.ReportObjects["Text2"]).Text = "Наименование подразделения библиотеки";
                    RViewer.ReportSource = rn;
                    break;
                case 6:
                    Report6 r6 = new Report6();
                    ((TextObject)r6.Section2.ReportObjects["Text7"]).Text = title;
                    r6.Database.Tables["RDT6"].SetDataSource(source);
                    RViewer.ReportSource = r6;
                    break;
                case 7://ShowFreeService();
                    Report7 r7 = new Report7();
                    ((TextObject)r7.Section2.ReportObjects["Text8"]).Text = title;
                    r7.Database.Tables["RDT8"].SetDataSource(source);
                    RViewer.ReportSource = r7;
                    break;
                case 8:
                    Report5 r5 = new Report5();
                    ((TextObject)r5.Section2.ReportObjects["Text6"]).Text = title;
                    r5.Database.Tables["RDT5"].SetDataSource(source);
                    RViewer.ReportSource = r5;
                    break;
                case 9:
                    ReportA rr6 = new ReportA();
                    ((TextObject)rr6.Section2.ReportObjects["Text7"]).Text = title;
                    rr6.Database.Tables["RDTA"].SetDataSource(source);
                    RViewer.ReportSource = rr6;
                    break;
                case 10:
                    ReportA rr = new ReportA();
                    ((TextObject)rr.Section2.ReportObjects["Text7"]).Text = title;
                    rr.Database.Tables["RDTA"].SetDataSource(source);
                    RViewer.ReportSource = rr;
                    break;
            }
        }
    }

    class DBCrystalReportViewer : CrystalDecisions.Windows.Forms.CrystalReportViewer
    {
        public DBCrystalReportViewer()
        {
            DoubleBuffered = true;
        }
    }

}
