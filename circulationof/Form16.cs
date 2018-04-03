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
    public partial class Form16 : Form
    {
        Form1 f1;
        public Form16(Form1 f1_)
        {
            InitializeComponent();
            f1 = f1_;
            dataGridView1.Rows.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();

            if (this.textBox1.Text == "")
            {
                MessageBox.Show("Введите фамилию читателя!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select NumberReader, FamilyName, [Name], FatherName,DateBirth, Email from Readers..Main where lower(FamilyName) like lower('" + textBox1.Text + "')+'%' and lower(Name) like lower ('"+textBox2.Text+"')+'%'";
            DataSet DS = new DataSet();
            if (Conn.SQLDA.Fill(DS, "t") == 0)
            {
                MessageBox.Show("Читатель не найден!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dataGridView1.DataSource = DS.Tables["t"];
            dataGridView1.Columns[0].HeaderText = "Номер читателя";
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Дата рождения";
            //dataGridView1.Columns[5].HeaderText = "Город";
            //dataGridView1.Columns[6].HeaderText = "Улица";
            dataGridView1.Columns[5].HeaderText = "Email";



        }

        private void button2_Click(object sender, EventArgs e)
        
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите читателя!");
                return;
            }
            f1.FrmlrFam(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            Close();
        }
    }
}
