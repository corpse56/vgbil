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
    public partial class Form20 : Form
    {
        Form1 f1;
        public string pass="";
        public Form20(Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "aa")
            {
                f1.pass = textBox1.Text;
                this.pass = textBox1.Text;
                Close();
            }
        }
    }
}
