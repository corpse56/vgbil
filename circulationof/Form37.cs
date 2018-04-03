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
    public partial class Form37 : Form
    {
        Form1 F1;
        bool Edit;
        string idservice;
        string price;
        public Form37(Form1 f1)
        {
            F1 = f1;
            InitializeComponent();
            textBox1.Text = "";
            Edit = false;
        }
        public Form37(Form1 f1,string s,string id,string pr)
        {
            F1 = f1;
            InitializeComponent();
            textBox1.Text = s;
            numericUpDown1.Value = int.Parse(pr);
            idservice = id;
            Edit = true;
            price = pr;
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
                F1.dbw.EditPaidService(textBox1.Text,idservice,numericUpDown1.Value.ToString());
                MessageBox.Show("Услуга успешно изменена!");
            }
            else
            {
                F1.dbw.AddPaidService(textBox1.Text, numericUpDown1.Value.ToString());
                MessageBox.Show("Услуга успешно добавлена!");
            }
            Close();
        }
    }
}
