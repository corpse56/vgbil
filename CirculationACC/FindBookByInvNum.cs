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
    public partial class FindBookByInvNum : Form
    {
        public FindBookByInvNum()
        {
            InitializeComponent();
            textBox1.Focus();
            dataGridView1.Rows.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите инвентарный номер!");
                return;
            }
            Conn.SQLDA.SelectCommand.CommandText = "select A.IDMAIN, B.NumberReader, B.FamilyName, B.Name, B.FatherName," +
                                                   "A.DATE_VOZV, A.DATE_ISSUE, A.IDROLD, C.FullName" + 
                " from Reservation_R..ISSUED A " +
                " left join Readers..MAIN B on A.IDREADER = B.NumberReader " +
                " left join AbonOld..Main C on A.IDROLD collate Cyrillic_General_CS_AI = C.IDReader " +
                " where A.INV = '" + textBox1.Text + "' and A.IDMAIN <> 0";
            DataSet DS = new DataSet();
            dataGridView1.ColumnHeadersHeight = 30;
            dataGridView1.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            if (Conn.SQLDA.Fill(DS, "t") != 0)
            {
                dataGridView1.DataSource = DS.Tables["t"];
                dataGridView1.Columns[0].HeaderText = "ПИН";
                dataGridView1.Columns[1].HeaderText = "Номер читателя из новой базы";
                dataGridView1.Columns[2].HeaderText = "Фамилия читателя из новой базы";
                dataGridView1.Columns[3].HeaderText = "Имя читателя из новой базы";
                dataGridView1.Columns[4].HeaderText = "Отчество читателя из новой базы";
                dataGridView1.Columns[5].HeaderText = "Дата возврата";
                dataGridView1.Columns[6].HeaderText = "Дата выдачи";
                dataGridView1.Columns[7].HeaderText = "Номер читателя из старой базы";
                dataGridView1.Columns[8].HeaderText = "ФИО читателя из старой базы";
            }
            else
            {
                MessageBox.Show("Книга свободна!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
