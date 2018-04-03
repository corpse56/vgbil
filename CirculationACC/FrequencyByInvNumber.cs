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
    public partial class FrequencyByInvNumber : Form
    {
        public FrequencyByInvNumber()
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
            Conn.SQLDA.SelectCommand.CommandText = "select count(INV) from Reservation_R..ISSUED where INV ='" + textBox1.Text + "' and DATE_ISSUE between '" + dateTimePicker1.Value.ToString("yyyyMMdd") + "' and '" + dateTimePicker2.Value.ToString("yyyyMMdd") + "'";
            if (Conn.SQLDA.SelectCommand.Connection.State != ConnectionState.Open)
            {
                Conn.SQLDA.SelectCommand.Connection.Open();
            }
            int spr = (int)Conn.SQLDA.SelectCommand.ExecuteScalar();
            Conn.SQLDA.SelectCommand.Connection.Close();
            MessageBox.Show("За указанный период этот инвентарный номер выдавался читателю "+spr.ToString()+" раз(а)");
        }
    }
}
