using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace Circulation
{
    public partial class Auth : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        //DBWork db;
        Form1 F1;
        public bool Canceled = false;
        public Auth(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
            Canceled = false;
            //db = new DBWork(F1);
            //textBox2.Text = "";
            //textBox3.Text = "";
            //this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            //this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }
        //bool Authorization = false;
        private void button5_Click(object sender, EventArgs e)
        {
            DBGeneral dbG = new DBGeneral();

            if (dbG.Login(textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Авторизация прошла успешно!", "Добро пожаловать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                F1.EmpID = dbG.EmpID;
                F1.textBox1.Text = dbG.UserName;
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователя с таким именем или паролем не существует!", "Неверное имя или пароль!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Canceled = true;
            this.Close();
            
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


    }
}