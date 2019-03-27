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
        public fBJAuthorization(string fund)
        {
            this.DialogResult = DialogResult.Cancel;
            InitializeComponent();
            Fund = fund;
        }

        private string Fund;

        public BJUserInfo User { get; set; }
        

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            User = BJUserInfo.GetUserByLogin(tbLogin.Text, this.Fund);
            if (User == null)
            {
                cbRoles.Items.Clear();
                return;
            }
            cbRoles.Items.Clear();
            foreach(var userStatus in User.UserStatus)
            {
                cbRoles.Items.Add(userStatus);
            }
            cbRoles.SelectedIndex = cbRoles.Items.Count - 1;
            //cbRoles.DropDownWidth = cbRoles.Items.Cast<string>().Max(x => TextRenderer.MeasureText(x, cbRoles.Font).Width);//DropDownWidth(cbRoles);
            cbRoles.DropDownWidth = DropDownWidth(cbRoles);
        }
        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            foreach (var obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth;
        }
        

        private void bOk_Click(object sender, EventArgs e)
        {
            if (User == null || cbRoles.Items.Count == 0)
            {
                MessageBox.Show("Такое имя пользователя не найдено!");
                return;
            }
            string pwd = BJUserInfo.HashPassword(tbPassword.Text);
            if (User != null && User.HashedPwd == BJUserInfo.HashPassword(tbPassword.Text))
            {
                User.SelectedUserStatus = (UserStatus)cbRoles.SelectedItem;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Неверный пароль!");
                return;
            }
        }

        private void fBJAuthorization_Load(object sender, EventArgs e)
        {
            tbLogin_TextChanged(sender, e);
        }

        private void fBJAuthorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                this.User = null;
            }
        }
    }
}
