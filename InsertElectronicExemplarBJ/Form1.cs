using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InsertElectronicExemplarBJ;
using LibflClassLibrary.Books.BJBooks;
using Utilities;

namespace InsertElectronicExemplarBJ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int PIN = 0;
            try
            {
                PIN = int.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("ПИН должен быть числом!");
                return;
            }

            ElectronicExemplarType AccessType = ElectronicExemplarType.Free;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    AccessType = ElectronicExemplarType.Free;
                    break;
                case 1:
                    AccessType = ElectronicExemplarType.Indoor;
                    break;
                case 2:
                    AccessType = ElectronicExemplarType.Order;
                    break;
            }


            ElectronicExemplarInserter ec = new ElectronicExemplarInserter(PIN, comboBox1.Text, comboBox1.Text);
            Utilities.Log log = new Log();
            try
            {
                ec.InsertElectronicExemplar(AccessType);
            }
            catch (Exception ex)
            {
                log.WriteLog(DateTime.Now + ". Программа не смогла выполнить вставку электронной копии. Текст ошибки: " + ex.Message);
                log.Dispose();
                ec.Dispose();
                MessageBox.Show("Произошла ошибка. Проверьте лог-файл _log.txt в папке с программой. " + ex.Message);
                return;
            }

            log.WriteLog(DateTime.Now + " база:" + comboBox1.Text + " пин:" + PIN+ " Тип доступа: "+comboBox2.Text);
            MessageBox.Show("Готово!");
            log.Dispose();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=BJVVV_Test;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = connection;
            da.SelectCommand.CommandText = "select * from BookAddInf..ScanInfo where IDBase = 1";
            DataTable table = new DataTable();
            da.Fill(table);
            foreach(DataRow row in table.Rows)
            {
                BJBookInfo book = BJBookInfo.GetBookInfoByPIN((int)row["IDBook"], "BJVVV");
                ElectronicExemplarInserter ec = new ElectronicExemplarInserter((int)row["IDBook"], "BJVVV_Test", "BJVVV");
                ElectronicExemplarType AccessType = ElectronicExemplarType.Order;
                if (book.DigitalCopy.ExemplarAccess.Access == 1001)
                {
                    AccessType = ElectronicExemplarType.Free;
                }
                if (book.DigitalCopy.ExemplarAccess.Access == 1002)
                {
                    AccessType = ElectronicExemplarType.Order;
                }
                if (book.DigitalCopy.ExemplarAccess.Access == 1003)
                {
                    AccessType = ElectronicExemplarType.Indoor;
                }
                ec.InsertElectronicExemplar(AccessType);
            }

        }
    }
}
