using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BookClasses;
using System.Data.Sql;
using System.Data.SqlClient;

namespace WriteOff
{
    public partial class Form1 : Form
    {
        public XmlConnections xmlc;
        public ReadDelBooks reader_;
        public string Base;
        Form2 f2;
        public Form1()
        {
            f2 = new Form2(this);
            f2.ShowDialog();
            //this.Close();
            InitializeComponent();
            if (this.Base == "BJVVV")
            {
                this.Text = "�������� ���� �� ��������� �����";
            }
            else
            {
                this.Text = "�������� ���� �� ����� ������ �����";
            }
            try
            {
                reader_ = new ReadDelBooks(this.Base);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +"��������� �������� ��������� ������.");
                throw;
            }
            comboBox1.Items.AddRange(reader_.GetDepartments().ToArray());
            for (int i = 2008; i < 2099; i++)
            {
                comboBox2.Items.Add(i.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //MessageBox.Show("");
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("�������� ����� ��������������!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.Text == string.Empty)
            {
                MessageBox.Show("�������� ���!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox3.Text == string.Empty)
            {
                MessageBox.Show("�������� ���!", "��������", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            List<string> l = new List<string>();
            List<Book> ListOfBooks = new List<Book>();
            //try
            {
                ListOfBooks = reader_.GetBooksOnAct(comboBox3.SelectedItem.ToString());
            }
            //catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message + ex.Source + ex.InnerException + ex.Data + ex.HResult);
             //   Application.Exit();
            }
            if (ListOfBooks == null)
            {
                MessageBox.Show("��������� ��� �� �������� ����!");
                return;
            }
            if (Book.CountAllWOFFInvs(ListOfBooks) != reader_.GetInvsCountOnAct(comboBox3.Text))
            {
                if (MessageBox.Show("�� ��������� ���������� ���������� ����������� ����������(" + Book.CountAllWOFFInvs(ListOfBooks) + ") � ��������� � ����(" + reader_.GetInvsCountOnAct(comboBox3.Text) + "). ����������?", "��������", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            MSWordInserter wrd = new MSWordInserter(this.Base);
            KeyValuePair<int,int> counts = wrd.AddBooksToTableTemplate(ListOfBooks,comboBox3.SelectedItem.ToString(),comboBox1.SelectedItem.ToString());
            wrd.CreateAct(this.comboBox3.SelectedItem.ToString(),
                          comboBox1.SelectedItem.ToString(),
                          counts.Key, counts.Value,
                          (int)numericUpDown1.Value,
                          reader_.GetTransferDirection(comboBox3.SelectedItem.ToString()),
                          reader_.GetCause(comboBox3.SelectedItem.ToString())
                          );
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox2.SelectedItem.ToString() == "")
            {
                MessageBox.Show("�������� ���!", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox3.Items.Clear();
            comboBox3.Items.AddRange(reader_.GetDelAtcsByYear(new DateTime(int.Parse(comboBox2.SelectedItem.ToString()), 12, 12)).ToArray());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //comboBox1.SelectedIndex = 0;
            //comboBox2.SelectedIndex = 6;
            //comboBox2_SelectedIndexChanged(sender, e);
            //comboBox3.SelectedIndex = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MSWordInserter ms = new MSWordInserter("BJVVV");
            ms.CreateAct("123", "department", 100, 12, 14, "makulature", "old");
        }

    }
    /*public class Book
    {
        System.Collections.Generic.KeyValuePair f;
        Dictionary<int, string> f;
        void foo()
        {
         //   f.
        }

        int i;
        public int IK
        {
            get
            {
                return i;
            }
            private set
            {
                i = value;
            }
        }
    }*/
}