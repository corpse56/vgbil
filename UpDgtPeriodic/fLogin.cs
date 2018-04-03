using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using UpDgtPeriodic;
namespace UpDgtPeriodic
{
    public partial class fLogin : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

      
        public fLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.Login(textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Авторизация прошла успешно!", "Добро пожаловать", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Visible = false;
                fMain fM = new fMain(IDUser);
                fM.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователя с таким именем или паролем не существует!", "Неверное имя или пароль!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = "";
            }
            
        }
        private string IDUser;
        private bool Login(string login, string pass)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XMLConnections.XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("name", SqlDbType.VarChar);
            da.SelectCommand.Parameters.Add("pass", SqlDbType.VarChar);
            da.SelectCommand.Parameters["name"].Value = login;
            da.SelectCommand.Parameters["pass"].Value = pass;
            da.SelectCommand.CommandText = "select * from BJVVV..USERS where lower(LOGIN) = lower(@name) and lower(PASSWORD) = lower(@pass)";
            DataSet ds = new DataSet();
            int res = da.Fill(ds,"t");
            if (res == 0)
            {
                return false;
            }
            else
            {
                this.IDUser = ds.Tables["t"].Rows[0]["ID"].ToString();
                return true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }


    }
}