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
    public partial class LostBook : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        string read;
        DBWork db;
        public LostBook(string reader,Form1 f1)
        {
            this.read = reader;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            db = new DBWork();
            DataSet tmp = db.getBooksForReader(reader);
            if (tmp.Tables["booksonreader"].Rows.Count == 0)
            {
                MessageBox.Show("За читателем не числится книг");
                return;
            }
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = tmp.Tables["booksonreader"];
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].Width = 250;
            dataGridView1.Columns[0].HeaderText = "Заглавие";
            dataGridView1.Columns[1].Width = 145;
            dataGridView1.Columns[1].HeaderText = "Автор";
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            //dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count-1);
            dataGridView1.AllowUserToAddRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            db.setBookLost(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            MessageBox.Show("Книга больше не числится за читателем!");
            DataSet tmp = db.getBooksForReader(read);
            if (tmp.Tables["booksonreader"] == null)
            {
                MessageBox.Show("За читателем не числится книг");
            }
            dataGridView1.DataSource = tmp.Tables["booksonreader"];
        }

        private void LostBook_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
    }
}