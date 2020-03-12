using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookkeepingForOrder
{
    public partial class fICRefuseReason : Form
    {
        public bool IsCanceled = false;
        public string Cause = "";
        public fICRefuseReason()
        {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            IsCanceled = true;
            Close();
        }

        private void fICRefuseReason_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.IsCanceled = true;
            }
        }

        private void bRefuse_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Введите причину отказа!");
                return;
            }
            this.Cause = textBox1.Text;
            this.FormClosing -= fICRefuseReason_FormClosing;
            Close();
        }
    }
}
