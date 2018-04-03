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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_R..PENY";
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "peny");
            dataGridView1.DataSource = DS.Tables["peny"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Сумма штрафа, руб.";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].HeaderText = "Дата назначения штрафа";
            dataGridView1.Columns[2].Width = 310;
            label3.Text = dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells[1].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form14 f14 = new Form14();
            f14.ShowDialog();
            Conn.SQLDA.SelectCommand.CommandText = "select * from Reservation_R..PENY";
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "peny");
            dataGridView1.DataSource = DS.Tables["peny"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Сумма штрафа, руб.";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].HeaderText = "Дата назначения штрафа";
            dataGridView1.Columns[2].Width = 310;

        }
    }
}
