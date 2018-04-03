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
    public partial class Form23 : Form
    {
        dbReader reader;
        public Form23(dbReader reader_)
        {
            InitializeComponent();
            reader = reader_;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select * from Readers..AbonementAdd where IDReader = " + reader.id;
            DataSet DS = new DataSet();
            int c = Conn.SQLDA.Fill(DS, "t");
            if (c == 1)
            {
                SqlDataAdapter DA = new SqlDataAdapter();
                DA.UpdateCommand = new SqlCommand();
                DA.UpdateCommand.Connection = Conn.ReadersCon;
                if (DA.UpdateCommand.Connection.State == ConnectionState.Closed)
                {
                    DA.UpdateCommand.Connection.Open();
                }
                DA.UpdateCommand.CommandText = "update Readers..AbonementAdd set RegInMoscow = '"+dateTimePicker1.Value.ToString("yyyyMMdd")+"' where IDReader = " + this.reader.id;
                DA.UpdateCommand.ExecuteNonQuery();
                DA.UpdateCommand.Connection.Close();
            }
            else
            {
                SqlDataAdapter DA = new SqlDataAdapter();
                DA.InsertCommand = new SqlCommand();
                DA.InsertCommand.Connection = Conn.ReadersCon;
                if (DA.InsertCommand.Connection.State == ConnectionState.Closed)
                {
                    DA.InsertCommand.Connection.Open();
                }
                DA.InsertCommand.CommandText = "insert into Readers..AbonementAdd (IDReader, RegInMoscow) values(" + this.reader.id + ",'" + dateTimePicker1.Value.ToString("yyyyMMdd") + "')";
                DA.InsertCommand.ExecuteNonQuery();
                DA.InsertCommand.Connection.Close();

            }
            MessageBox.Show("Дата окончания регистрации в Москве успешно изменена!");
            Close();
        }
    }
}
