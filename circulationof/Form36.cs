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
    public partial class Form36 : Form
    {
        Form1 F1;
        string idservice;
        bool Edit;
        public Form36(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
            textBox1.Text = "";
            Edit = false;
        }
        public Form36(Form1 f1,string s,string id)
        {
            F1 = f1;
            InitializeComponent();
            textBox1.Text = s;
            idservice = id;
            Edit = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите название услуги!");
                return;
            }
            if (Edit)
            {
                F1.dbw.EditFreeService(textBox1.Text,idservice);
            }
            else
            {
                F1.dbw.AddFreeService(textBox1.Text);
                MessageBox.Show("Услуга успешно добавлена!");
            }
            Close();
        }
    }
}
