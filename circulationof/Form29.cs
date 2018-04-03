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
    public partial class Form29 : Form
    {
        public int days;
        public Form29(int pdays)
        {
            InitializeComponent();
            if (pdays != -1)
            {
                days = pdays;
                numericUpDown1.Value = pdays;
            }
            else
            {
                days = -1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            days = -1;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            days = (int)numericUpDown1.Value;
            Close();
        }
    }
}
