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
    public partial class Form22 : Form
    {
        public Form22()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Today;//.AddMonths(-1);
            dateTimePicker2.Value = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select IDREADER,DATE_ISSUE from Reservation_R..ISSUED_OF " +
                                                   " where DATE_ISSUE between '" + dateTimePicker1.Value.ToString("yyyyMMdd") + "' and '" + dateTimePicker2.Value.ToString("yyyyMMdd") + "' " +
                                                   " group by DATE_ISSUE,IDREADER";
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "t");
            MessageBox.Show("За указанный период было распечатано "+i.ToString()+" формуляра(ов)");
        }
    }
}
