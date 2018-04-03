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
    public partial class ChangeComment : Form
    {
        ReaderVO reader;
        public ChangeComment(ReaderVO reader_)
        {
            InitializeComponent();
            reader = reader_;
            textBox1.Text = reader.GetComment();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            reader.ChangeComment(textBox1.Text);



            MessageBox.Show("Комментарий успешно сохранён!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
