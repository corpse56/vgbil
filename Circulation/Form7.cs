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
    public partial class Form7 : Form
    {
        public string str = "";
        public Form7()
        {
            InitializeComponent();
            this.ActiveControl = this.textBox1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            str = textBox1.Text;
            Close();
        }
    }
}
