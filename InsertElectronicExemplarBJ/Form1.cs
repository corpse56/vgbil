using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InsertElectronicExemplarBJ.classes;

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


            ElectronicExemplarInserter ec = new ElectronicExemplarInserter(PIN, comboBox1.Text);
            Log log = new Log();
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



    }
}
