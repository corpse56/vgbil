using LibflClassLibrary.BJUsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibflClassLibrary.Controls
{
    public partial class fBJAuthorization : Form
    {
        public fBJAuthorization()
        {
            InitializeComponent();
        }
        public BJUserInfo User { get; set; }
        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            User = BJUserInfo.GetUserByLogin(tbLogin.Text);

        }

        private void bOk_Click(object sender, EventArgs e)
        {

        }
    }
}
