using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
//using BoardNameMap = System.Collections.Generic.Dictionary<string /*boardName*/, int /*Number*/>;

namespace Circulation
{
    //using BoardThreadMap = System.Collections.Generic.Dictionary<string /*boardName*/, BoardNameMap>;

    public partial class Prolong : Form
    {
        //BoardThreadMap h_;
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]

        private static extern int GetMenuItemCount(IntPtr hWnd);
        private int days;
        public int Days
        {
            get { return days; }
            set { days = value; }
        }        
        public Prolong()
        {
            InitializeComponent();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Days = -99;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //decimal num;
            //bool flag;
            if (this.numericUpDown1.Value.ToString() == "")
            {
                MessageBox.Show("Введите число или нажмите \"Отмена\"", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int.Parse(this.numericUpDown1.Value.ToString());
            }
            catch
            {
                MessageBox.Show("Введенная информация не является числом!", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (((int)this.numericUpDown1.Value) < 0)
            {
                MessageBox.Show("Введите число больше нуля", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Days = (int)this.numericUpDown1.Value;
            Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);

        }
    }
}