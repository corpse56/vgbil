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
    public partial class Form27 : Form
    {
        public string rlocation;
        public string respan;
        public Form27(Form1 f1)
        {
            InitializeComponent();
            textBox1.Text = "";
            foreach (object dep in comboBox1.Items)
            {
                if (f1.DepName == dep.ToString())
                {
                    comboBox1.SelectedItem = dep;
                    break;
                }
            }

            //comboBox1.Text = f1.DepName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if ((textBox1.Text == "") || (comboBox1.Text == ""))
            {
                MessageBox.Show("Заполните все данные!");
                return;
            }
            this.respan = this.textBox1.Text;
            this.rlocation = this.comboBox1.Text;
            */
            int res = 0;
            if (!int.TryParse(textBox1.Text, out res))
            {
                MessageBox.Show("Укажите правильный номер бронеполки!");
                return;
            }
            Close();
        }

        private void Form27_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((textBox1.Text == "") || (comboBox1.Text == ""))
            {
                MessageBox.Show("Заполните все данные!");
                e.Cancel = true;
                return;
            }
            int res = 0;
            if (!int.TryParse(textBox1.Text, out res))
            {
                MessageBox.Show("Укажите правильный номер бронеполки!");
                e.Cancel = true;
                return;
            } this.respan = this.textBox1.Text;
            this.rlocation = this.comboBox1.Text;
            e.Cancel = false;
        }
    }
}
