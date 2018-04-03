using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace RastShifrRestored
{
    public partial class Form2 : Form
    {
        // Fields
        private string _printerName;
        private Button button1;
        private Button button2;
        private ComboBox comboBox1;
        public string PrinterName
        {
            get
            {
                return this._printerName; ;
            }
            set
            {
                this._printerName = value;
                return;
            }
        }
        
        private Label label1;


        public Form2()
        {
            InitializeComponent();
        }
        void button2_Click(object sender, EventArgs e)
        {
            this.PrinterName = "close";
            this.Close();
        }

        void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.PrinterName == "")
            {
                this.PrinterName = "close";
                return;
            }
        }

        void Form2_Load(object sender, EventArgs e)
        {

            foreach (string str in PrinterSettings.InstalledPrinters)
            {
                this.comboBox1.Items.Add(str);
            }

        }

        void button1_Click(object sender, EventArgs e)
        {

            if ((string)this.comboBox1.SelectedItem == "")
            {
                MessageBox.Show("Выберите принтер!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.PrinterName = (string)this.comboBox1.SelectedItem;
            this.Close();
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}