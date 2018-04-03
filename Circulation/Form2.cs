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
    public partial class Form2 : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        DBWork db;
        Form1 F1;
        public Form2(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
            db = new DBWork(F1);
            textBox2.Text = "";
            textBox3.Text = "";
            //this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            //this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }
        //bool Authorization = false;
        private void button5_Click(object sender, EventArgs e)
        {
            if (db.ChangeEmployee(textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Авторизация прошла успешно!", "Добро пожаловать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Authorization = true;
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
            this.Close();
            
            /*if (F1.EmpID == null)
            {
                MessageBox.Show("Авторизуйтесь!");
                return;
            }
            Close();*/
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
           /* if (!Authorization)
                e.Cancel = true;
            Authorization = false;*/
            base.OnClosing(e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}