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
    public partial class Form26 : Form
    {
        private string ID;
        private string DP;
        public Form26(string id)
        {
            InitializeComponent();
            this.ID = id;
            Conn.SQLDA.SelectCommand = new System.Data.SqlClient.SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "Select * from Reservation_R..ISSUED_OF where ID = " + id;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "t");
            label1.Text = "Текущий номер бронеполки: " + DS.Tables[0].Rows[0]["RESPAN"].ToString();
            if (DS.Tables[0].Rows[0]["RESPAN"].ToString() == "ДП")
            {
                this.DP = "DP";
            }
            else
            {
                numericUpDown1.Value = int.Parse(DS.Tables[0].Rows[0]["RESPAN"].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.DP == "DP")
            {
                MessageBox.Show("Книга на длительном пользовании! Изменить бронеполку невоможно!");
                return;
            }
            Conn.SQLDA.UpdateCommand = new System.Data.SqlClient.SqlCommand();
            Conn.SQLDA.UpdateCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.UpdateCommand.CommandText = "update Reservation_R..ISSUED_OF set RESPAN = " + numericUpDown1.Value.ToString() + " where ID = " + this.ID;
            if (Conn.SQLDA.UpdateCommand.Connection.State == ConnectionState.Closed)
            {
                Conn.SQLDA.UpdateCommand.Connection.Open();
            }
            Conn.SQLDA.UpdateCommand.ExecuteNonQuery();
            MessageBox.Show("Бронеполка успешно изменена!");
            this.Close();
        }

    }
}
