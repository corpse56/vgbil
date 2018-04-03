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
    public partial class Form14 : Form
    {
        public Form14()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_R..PENY order by DATEOFSETUP desc";
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "tmp");
            if ((DateTime)DS.Tables["tmp"].Rows[0]["DATEOFSETUP"] == DateTime.Today)
            {
                MessageBox.Show("Сегодня уже устанавливалась сумма штрафа. Программа изменит сегодняшнюю сумму штрафа.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Conn.SQLDA.UpdateCommand = new System.Data.SqlClient.SqlCommand();
                Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
                Conn.ZakazCon.Open();
                Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_R..PENY set PENYPERDAY = " + textBox1.Text + "where DATEOFSETUP ='" + DateTime.Today.ToString("yyyyMMdd")+ "'";
                Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
                Conn.ZakazCon.Close();
            }
            else
            {
                Conn.SQLDA.InsertCommand = new System.Data.SqlClient.SqlCommand();
                Conn.SQLDA.InsertCommand.Connection = Conn.ZakazCon;
                Conn.ZakazCon.Open();
                Conn.SQLDA.InsertCommand.CommandText = "insert into Reservation_R..PENY (PENYPERDAY,DATEOFSETUP) values (" + textBox1.Text + ",'" + DateTime.Today.ToString("yyyyMMdd") + "')";
                Conn.SQLDA.InsertCommand.ExecuteNonQuery();
                Conn.ZakazCon.Close();
                MessageBox.Show("Сумма штрафа изменена.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
