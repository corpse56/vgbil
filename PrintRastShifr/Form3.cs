using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RastShifrRestored
{
    public partial class Form3 : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        //ref int i;
        Form1 F1;
        public Form3(DataTable T,Form1 f1)
        {
            InitializeComponent();
            F1 = f1;
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = T;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].Width = 170;
            dataGridView1.Columns[0].HeaderText = "Инв. номер";
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].HeaderText = "Расст. шифр";
            dataGridView1.Columns[3].Name = "shifr";
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            F1.IDDATAEXT = dataGridView1.SelectedRows[0].Cells["shifid"].Value.ToString();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F1.IDDATAEXT = "Cancel";
            Close();
        }
    }
}