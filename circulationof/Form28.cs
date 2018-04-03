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
    public partial class Form28 : Form
    {
        public dbBook book;
        public bool Cancel = false;
        private dbBook previous = null;
        public Form28(dbBook b,bool IsIssue,dbBook previous_)
        {
            this.book = b;
            this.previous = previous_;
            InitializeComponent();
            textBox1.Text = book.name;
            textBox2.Text = book.year;
            textBox3.Text = book.number;
            this.numericUpDown1.Value = 1;
            if (IsIssue)
            {
                this.numericUpDown1.Visible = true;
                label6.Visible = true;
                this.button3.Visible = false;
            }
            else
            {
                this.numericUpDown1.Visible = false;
                this.label6.Visible = false;
                this.button3.Visible = true;
            }

            
            if (book.id == "-1")
            {
                if (book.mainfund)
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
            }
            label5.Text = book.barcode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            book.name = textBox1.Text;
            book.year = textBox2.Text;
            book.number = textBox3.Text;
            book.additionalNumbers = textBox4.Text;
            book.NumbersCount = (int)this.numericUpDown1.Value;
            if (radioButton1.Checked)
            {
                book.mainfund = true;
                book.klass = "Для выдачи";
            }
            if (radioButton2.Checked)
            {
                book.mainfund = false;
                book.klass = "ДП";
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form28_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((!radioButton1.Checked) && (!radioButton2.Checked))
            {
                MessageBox.Show("Нужно обязательно выбрать фонд!");
                e.Cancel = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Cancel = true;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (previous == null) previous = new dbBook();
            textBox1.Text = previous.name;
            textBox2.Text = previous.year;
            textBox3.Text = previous.number;
            textBox4.Text = previous.additionalNumbers;
        }
        AutoCompleteStringCollection TitleACSC = new AutoCompleteStringCollection();
        List<string> TitleAutoCompleteSource = new List<string>();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FillAutoCompleteTitle(textBox1.Text);
            textBox1.AutoCompleteCustomSource = TitleACSC;

        }

        private void FillAutoCompleteTitle(string exp)
        {
            List<string> filteredList = TitleAutoCompleteSource.FindAll(x => x.ToLower().Contains(exp.ToLower()));
            TitleACSC = new AutoCompleteStringCollection();
            foreach (string title in filteredList)
            {
                TitleACSC.Add(title.Trim());
            }
        }

        private void Form28_Load(object sender, EventArgs e)
        {
            DBWork dbw = new DBWork();
            TitleAutoCompleteSource = dbw.GetNotInBaseTitles();

        }
    }
}
