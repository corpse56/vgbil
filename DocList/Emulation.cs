using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocList
{
    public partial class Emulation : Form
    {
        public string emul = "";
        public Emulation( )
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.emul = textBox1.Text;
            Close();
        }
    }
}
