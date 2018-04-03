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
    public partial class History : Form
    {
        public History(ReaderVO reader)
        {
            InitializeComponent();
            label2.Text = reader.FIO;
            DBReference dbref = new DBReference();


            dataGridView1.DataSource = dbref.GetReaderHistory(reader);

            dataGridView1.Columns["DATE_ISSUE"].HeaderText = "Дата выдачи";
            dataGridView1.Columns["DATE_RETURN"].HeaderText = "Дата возврата";
            dataGridView1.Columns["inv"].HeaderText = "Инвентарный номер";
            dataGridView1.Columns["tit"].HeaderText = "Заглавие";
            dataGridView1.Columns["avt"].HeaderText = "Автор";
            dataGridView1.Columns["ID"].HeaderText = "№№";
            dataGridView1.Columns["ID"].Width = 40;
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ID"].Value = (++i).ToString();
            }
            dataGridView1.Columns["tit"].Width = 300;
            dataGridView1.Columns["tit"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            

        }
    }
}
