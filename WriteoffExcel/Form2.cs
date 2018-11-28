using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WriteOff
{
    public partial class Form2 : Form
    {
        Form1 F1;
        public Form2(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                F1.Base = "BJVVV";
            }
            else
            {
                if (radioButton2.Checked == true)
                {
                    F1.Base = "REDKOSTJ";
                }
                else
                {
                    F1.Base = "BJDEZIDER";
                }
            }
            this.Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((F1.Base == "") || (F1.Base == null))
            {
                if (radioButton1.Checked == true)
                {
                    F1.Base = "BJVVV";
                }
                else
                {
                    if (radioButton2.Checked == true)
                    {
                        F1.Base = "REDKOSTJ";
                    }
                    else
                    {
                        F1.Base = "BRIT_SOVET";
                    }
                }
            }
        }
    }
}
