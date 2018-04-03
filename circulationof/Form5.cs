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
    public partial class Form5 : Form
    {
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]

        private static extern int GetMenuItemCount(IntPtr hWnd);
        public Form5()
        {
            InitializeComponent();
            
        }
        private string idr;
        public string abon;
        public Form5(string idr)
        {
            InitializeComponent();
            this.idr = idr;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show();
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Выберите тип абонемента или нажмите \"Отмена\"", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DBWork db = new DBWork();
            db.SetReaderAbonement(this.idr,comboBox1.SelectedValue.ToString());
            MessageBox.Show("Тип абонемента успешно изменён!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            abon = comboBox1.Text;
            Form1.FireAbon(this, EventArgs.Empty);
            Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);

            Conn.ReaderDA.SelectCommand.CommandText = "select * from AbonementType";
            Conn.ReaderDA.SelectCommand.Connection = Conn.ReadersCon;
            DataSet Abon = new DataSet();
            int i = Conn.ReaderDA.Fill(Abon);
            //comboBox1.Text = "Нет значения";
            comboBox1.DataSource = Abon.Tables[0];
            comboBox1.DisplayMember = "NameAbonType";
            comboBox1.ValueMember = "IDAbonemetType";
            //comboBox1.Text = "Нет значения";
            //MessageBox.Show(comboBox1.SelectedValue + comboBox1.SelectedText.ToString());


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}