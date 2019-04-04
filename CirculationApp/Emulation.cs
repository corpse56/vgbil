using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CirculationApp
{
    public partial class Emulation : Form
    {
        MainForm f1;
        public string emul;
        public Emulation(MainForm f1_)
        {
            InitializeComponent();
            f1 = f1_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.emul = textBox1.Text;
            this.emul = textBox1.Text;
            Close();
        }
    }
}
