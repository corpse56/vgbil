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
    public partial class Form30 : Form
    {
        public string rlocation;
        public Form30(Form1 f1)
        {
            InitializeComponent();
            foreach (object dep in comboBox2.Items)
            {
                if (f1.DepName == dep.ToString())
                {
                    comboBox2.SelectedItem = dep;
                    break;
                }
            } 
            rlocation = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form30_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Заполните все данные!");
                e.Cancel = true;
                return;
            }
            this.rlocation = this.comboBox2.Text;
            e.Cancel = false;
        }
    }
}
