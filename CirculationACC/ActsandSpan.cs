using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Circulation
{
    public partial class Form3Act : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]

        private static extern int GetMenuItemCount(IntPtr hWnd);
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        private int userid;
        public int UserID
        {
            get { return userid; }
            set { userid = value; }
        }
        private Form1 f1;
        public Form3Act(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            DataTable users = f1.dbw.getOperators();
            comboBox1.DataSource = users;
            comboBox1.DisplayMember = "NAME";
            comboBox1.ValueMember = "ID";
            //comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.StartDate = dateTimePicker1.Value;
            this.EndDate = dateTimePicker2.Value;
            this.UserID = (int)comboBox1.SelectedValue;

            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
    }
}