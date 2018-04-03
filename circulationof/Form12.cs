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
    public partial class Form12 : Form
    {
        dbReader reader;
        public Form12(dbReader reader_)
        {
            InitializeComponent();
            reader = reader_;
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AbonementMemo where IDReader = " + reader.id;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "tmp");
            if (c == 0) return;
            textBox1.Text = DS.Tables["tmp"].Rows[0]["Note"].ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("[Readers]..[changenote]", Conn.ReadersCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IDR", SqlDbType.Int);
            cmd.Parameters.Add("@NOTE", SqlDbType.NVarChar);
            cmd.Parameters["@IDR"].Value = reader.id;
            cmd.Parameters["@NOTE"].Value = textBox1.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Примечание успешно сохранено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
