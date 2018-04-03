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
    public partial class ParolEmulation : Form
    {
        Form1 f1;
        public ParolEmulation(Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.pass = textBox1.Text;
            Close();
        }
    }
}
