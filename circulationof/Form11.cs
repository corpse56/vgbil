using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Circulation
{
    public partial class Form11 : Form
    {
        dbReader reader;
        public Form11(dbReader reader_)
        {
            InitializeComponent();
            reader  =reader_;
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AbonementMemo where IDReader = " + reader.id;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "tmp");
            if (c == 0) return;
            textBox1.Text = DS.Tables["tmp"].Rows[0]["Comment"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("[Readers]..[changecomment]", Conn.ReadersCon);
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IDR", SqlDbType.Int);
            cmd.Parameters.Add("@COMMENT", SqlDbType.NVarChar);
            cmd.Parameters["@IDR"].Value = reader.id;
            cmd.Parameters["@COMMENT"].Value = textBox1.Text;
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            MessageBox.Show("Комментарий успешно сохранён!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
