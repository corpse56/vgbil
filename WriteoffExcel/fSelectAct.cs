using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using LibflClassLibrary.Writeoff;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using WriteoffExcel.Classes;

namespace WriteOff
{
    public partial class FSelectAct : Form
    {
        public string Base;
        fSelectFund f2;
        WriteoffInfo wi;
        public FSelectAct()
        {
            f2 = new fSelectFund(this);
            f2.ShowDialog();
            InitializeComponent();
            this.Text = "Списание книг";
            wi = new WriteoffInfo(this.Base);

            BindingSource bs = new BindingSource(wi.GetDepartments(), null);
            comboBox1.DataSource = bs;
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                comboBox2.Items.Add(i.ToString());
            }
        }

        private void bCreateAct_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Выберите отдел фондодержателя!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.Text == string.Empty)
            {
                MessageBox.Show("Выберите год!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox3.Text == string.Empty)
            {
                MessageBox.Show("Выберите акт!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            KeyValuePair<string, string> item = (KeyValuePair<string, string>)comboBox3.SelectedItem;
            string ActNumberSort    = item.Key.ToString();
            string ActNumber        = item.Value.ToString();
            using (ExcelWork excel = new ExcelWork(ActNumber))
            {
                try
                {
                    excel.Init();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                List<BJBookInfo> books = wi.GetBooksByAct(ActNumberSort);
                int RowIndex = 0;
                int Cost = 0;
                foreach (BJBookInfo b in books)
                {
                    foreach (BJExemplarInfo exemplar in b.Exemplars)
                    {
                        if (exemplar.Fields["929$b"].ToString() != string.Empty)
                        {
                            if (exemplar.Fields["929$b"].ToString() == ActNumber)
                            {
                                RowIndex++;
                                excel.InsertExemplar(exemplar, b, RowIndex);
                            }
                        }
                    }
                }
                excel.InsertDocumentHeader(RowIndex, comboBox1.Text, Cost);
                MessageBox.Show("Формирование акта успешно завершено!");
            }
            
        }
        private void bMakeActForYear_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYear(Convert.ToInt32(comboBox2.SelectedItem), "АБ");
            string ActNumber = "Абонемент";
            GenerateAct(Exemplars, ActNumber);
        }
        private void bMakeActPerYearOF_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYear(Convert.ToInt32(comboBox2.SelectedItem), "ОФ");
            string ActNumber = "Основной фонд";
            GenerateAct(Exemplars, ActNumber);
        }
        private void bAnotherFundholder_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYearAnotherFundholder(Convert.ToInt32(comboBox2.SelectedItem));
            string ActNumber = "Другие фондодержатели";
            GenerateAct(Exemplars, ActNumber);
        }
        private void bByYearInActNameOF_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYearInActNameOF(Convert.ToInt32(comboBox2.SelectedItem)-2000);
            string ActNumber = "Основной фонд";
            GenerateAct(Exemplars, ActNumber);

        }
        private void bByYearInActNameAB_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYearInActNameAB(Convert.ToInt32(comboBox2.SelectedItem) - 2000);
            string ActNumber = "Абонемент";
            GenerateAct(Exemplars, ActNumber);
        }
        private void bByYearInActNameAnotherFundholder_Click(object sender, EventArgs e)
        {
            List<BJExemplarInfo> Exemplars = wi.GetBooksPerYearInActNameAnotherFundholder(Convert.ToInt32(comboBox2.SelectedItem) - 2000);
            string ActNumber = "Другие фондодержатели";
            GenerateAct(Exemplars, ActNumber);
        }



        private void GenerateAct(List<BJExemplarInfo> Exemplars, string ActNumber)
        {
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Выберите отдел фондодержателя!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (comboBox2.Text == string.Empty)
            {
                MessageBox.Show("Выберите год!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            KeyValuePair<string, string> item = (KeyValuePair<string, string>)comboBox3.SelectedItem;
            string ActNumberSort = item.Key.ToString();
            using (ExcelWork excel = new ExcelWork(ActNumber))
            {
                try
                {
                    excel.Init();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                
                int RowIndex = 0;
                int Cost = 0;
                //foreach (BJBookInfo b in books)
                {
                    foreach (BJExemplarInfo exemplar in Exemplars)
                    {
                        if (exemplar.Fields["929$b"] != null)
                        {
                            //if (exemplar.Fields["929$b"].ToString() == ActNumber)
                            {
                                BJBookInfo b = BJBookInfo.GetBookInfoByPIN(exemplar.IDMAIN, exemplar.Fund);
                                RowIndex++;
                                excel.InsertExemplar(exemplar, b, RowIndex);
                            }
                        }
                    }
                }
                excel.InsertDocumentHeader(RowIndex, comboBox1.Text, Cost);
                MessageBox.Show("Формирование акта успешно завершено!");

            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (comboBox2.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Выберите год!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            BindingSource bs = new BindingSource(wi.GetWriteoffActs(Convert.ToInt32(comboBox2.SelectedItem)), null);
            comboBox3.DataSource = bs;
            comboBox3.ValueMember = "Key";
            comboBox3.DisplayMember = "Value";
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
            //MSWordInserter ms = new MSWordInserter("BJVVV");
            //ms.CreateAct("123", "department", 100, 12, 14, "makulature", "old");
        }

    }
}