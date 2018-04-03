using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace BookkeepingForOrder
{
    public partial class authorization : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        
        Form1 F1;
        public authorization(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
            //textBox2.Text = "";
            //textBox3.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (F1.Login(textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Авторизация прошла успешно!", "Добро пожаловать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователя с таким именем или паролем не существует!", "Неверное имя или пароль!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = "";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}