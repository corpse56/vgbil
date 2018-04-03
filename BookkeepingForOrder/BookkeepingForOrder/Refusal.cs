using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BookkeepingForOrder
{
    public partial class Refusal : Form
    {
        string OrderID;
        public string Cause = "";
        public Refusal(string OrderID_)
        {
            InitializeComponent();
            this.OrderID = OrderID_;
            this.Cause = "";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cause = "";
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.Cause = radioButton1.Text;
            }
            if (radioButton2.Checked)
            {
                this.Cause = radioButton2.Text;
            }
            if (radioButton3.Checked)
            {
                this.Cause = radioButton3.Text;
            }
            if (radioButton4.Checked)
            {
                this.Cause = radioButton4.Text;
            }
            if (radioButton5.Checked)
            {
                this.Cause = radioButton5.Text;
            }
            Close();
        }
    }
}
