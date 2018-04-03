using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public partial class Form21 : Form
    {
        public Form21()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Today;//.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите инвентарный номер!");
                return;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select count(INV) from Reservation_R..ISSUED_OF where INV ='" + textBox1.Text + "' and DATE_ISSUE between '" + dateTimePicker1.Value.ToString("yyyyMMdd") + "' and '" + dateTimePicker2.Value.ToString("yyyyMMdd") + "'";
            Conn.SQLDA.SelectCommand.Connection.Open();
            int spr = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            MessageBox.Show("За указанный период этот инвентарный номер выдавался читателю "+spr.ToString()+" раз(а)");
        }
    }
}
