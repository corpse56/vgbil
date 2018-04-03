using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace DocList
{
    public partial class ViewRep : Form
    {
        public ViewRep(DocList dl,string num, string total)
        {
            InitializeComponent();
            Viewer.ReportSource = dl;
            ((TextObject)dl.ReportDefinition.ReportObjects["Text12"]).Text = "Список документов к Договору № " + num;
            ((TextObject)dl.ReportDefinition.ReportObjects["Text9"]).Text = "Всего: "+total+" ед.хр.";

        }
    }
}
