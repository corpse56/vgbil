using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenStat
{
    public enum RefT { Service = 0, BookSupply = 1 , QoS = 2}
    public partial class RefType : Form
    {
        
        public RefT rt;
        public bool CanShow = false;
        public RefType()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                rt = RefT.Service;
                this.CanShow = true;
                Close();
                return;
            }
            if (radioButton2.Checked)
            {
                rt = RefT.BookSupply;
                CanShow = true;
                Close();
                return;
            }
            if (radioButton3.Checked)
            {
                rt = RefT.QoS;
                CanShow = true;
                Close();
                return;
            }
            if ((!radioButton1.Checked) && (!radioButton2.Checked) && (!radioButton3.Checked))
            {
                MessageBox.Show("Выберите тип справки!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CanShow = false;
            Close();
            
        }

        private void RefType_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CanShow = false;
        }
    }
}
