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
    public partial class FindReaderBySurname : Form
    {
        Form1 f1;
        public FindReaderBySurname(Form1 f1_)
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
            DBReader dbr = new DBReader();

            //DataSet DS = new DataSet();

            DataTable t = dbr.GetReaderByFamily(textBox1.Text);
            if (t.Rows.Count == 0)
            {
                MessageBox.Show("Читатель не найден!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dataGridView1.DataSource = t;
            dataGridView1.Columns[0].HeaderText = "Номер читателя";
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2].HeaderText = "Имя";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Дата рождения";
            dataGridView1.Columns[5].HeaderText = "Город";
            dataGridView1.Columns[6].HeaderText = "Улица";
            dataGridView1.Columns[7].HeaderText = "Email";
            dataGridView1.Columns[6].Width = 200;
            dataGridView1.Columns[7].Width = 200;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //f1.FrmlrFam(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Читатель не выбран!");
                return;
            }
            ReaderVO reader = new ReaderVO((int)dataGridView1.SelectedRows[0].Cells[0].Value);
            f1.FillFormular(reader);
            Close();
        }

        private void FindReaderBySurname_Load(object sender, EventArgs e)
        {

        }
    }
}
