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
    public partial class Form18 : Form
    {
        public Form18()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conn.SQLDA.DeleteCommand = new System.Data.SqlClient.SqlCommand();
            Conn.SQLDA.DeleteCommand.Connection = new System.Data.SqlClient.SqlConnection();
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.DeleteCommand.CommandText = "delete from Reservation_R..ADVORDER where INV = '" + textBox1.Text+"'";
            DataSet DS = new DataSet();
            Conn.SQLDA.DeleteCommand.Connection.Open();
            int cnt = Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            Conn.SQLDA.DeleteCommand.Connection.Close();
            if (cnt == 0)
            {
                MessageBox.Show("Среди предзаказанных документов не встречается экземпляра с номером " + textBox1.Text );
                return;
            }
            MessageBox.Show("Инвентарь " +textBox1.Text + " успешно удалён из предзаказанных!");
        }
    }
}
