using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UpDgtPeriodic
{
    public partial class fProgress : Form
    {
        private int max = 0;
        public fProgress()
        {
            InitializeComponent();
            progressBar1.Maximum = 100;
            this.max = 100;
            label1.Text = "";
        }
        public fProgress(string mess)
        {
            InitializeComponent();
            progressBar1.Maximum = 100;
            this.max = 100;
            label1.Text = mess;
        }
        public fProgress(int max, int number, int total)
        {
            InitializeComponent();
            label1.Text = "Копирование в основное хранилище: " + number.ToString() + " из " + total.ToString() + ".";
            timer1.Enabled = false;
            progressBar1.Maximum = max;
            this.max = max;
        }
        public fProgress(int max)
        {
            InitializeComponent();
            label1.Text = "";
            timer1.Enabled = false;
            progressBar1.Maximum = max;
            this.max = max;
        }
        public fProgress(int max, string mess)
        {
            InitializeComponent();
            label1.Text = mess;
            timer1.Enabled = false;
            progressBar1.Maximum = max;
            this.max = max;
        }
        object lock_obj = new object();
        public void IncProgress()
        {
            lock (lock_obj)
            {
                if (progressBar1.Value == this.max)
                {
                    progressBar1.Value = 0;
                }
                progressBar1.Value++;
                Application.DoEvents();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (lock_obj)
            {

                if (progressBar1.Value == this.max)
                {
                    progressBar1.Value = 0;
                }
                progressBar1.Value++;
                Application.DoEvents();
            }
        }
        public void ResetProgress(int max, int number, int total)
        {
            label1.Text = "Копирование в основное хранилище: " + number.ToString() + " из " + total.ToString() + ".";
            Application.DoEvents();
            timer1.Enabled = false;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
            this.max = max;

        }
        public void IncProgress(int max, int number, int total)
        {
            label1.Text = "Копирование в основное хранилище: " + number.ToString() + " из " + total.ToString() + ".";
            timer1.Enabled = false;
            progressBar1.Maximum = max;
            progressBar1.Value = number;
            this.max = max;

        }
    }

}
