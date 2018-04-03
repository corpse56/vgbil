using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public partial class Form34 : Form
    {
        DataSet DS;
        public string selectedid;
        public string bar;
        public Form34(DataSet _ds)
        {
            this.DS = _ds;
            InitializeComponent();
        }

        private void Form34_Load(object sender, EventArgs e)
        {
            Statistics.DataSource = DS.Tables["t"];
            Statistics.Columns[0].HeaderText = "ПИН";
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 400;
            Statistics.Columns[2].HeaderText = "Штрихкод";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Носитель";
            Statistics.Columns[3].Width = 200;
            Statistics.Columns[4].Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Statistics.SelectedRows.Count != 0)
            {
                selectedid = Statistics.SelectedRows[0].Cells[4].Value.ToString();
                bar = Statistics.SelectedRows[0].Cells[2].Value.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не выбрано ни одной строки!");
            }
        }

        private void Form34_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Statistics.SelectedRows.Count != 0)
            {
                selectedid = Statistics.SelectedRows[0].Cells[4].Value.ToString();
                bar = Statistics.SelectedRows[0].Cells[2].Value.ToString();
            }
            else
            {
                selectedid = "0";
            }
        }

    }
}
