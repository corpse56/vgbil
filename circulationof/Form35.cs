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
    public partial class Form35 : Form
    {
        Form1 F1;
        int freepaid;
        string id;
        public Form35(Form1 f1,int f,string _id)
        {
            freepaid = f;
            F1 = f1;
            id = _id;
            InitializeComponent();
        }

        private void Form35_Load(object sender, EventArgs e)
        {
            if (freepaid == 0)
            {
                InputService.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                InputService.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                InputService.DataSource = F1.dbw.getInputFreeServices();
                InputService.Columns[0].Width = 50;
                InputService.Columns[0].HeaderText = "№ п/п";
                InputService.Columns[1].Width = 250;
                InputService.Columns[1].HeaderText = "Наименование услуги";
                InputService.Columns[2].Width = 110;
                InputService.Columns[2].HeaderText = "Кол-во";
                InputService.ReadOnly = false;
                InputService.Columns[0].ReadOnly = true;
                InputService.Columns[1].ReadOnly = true;
                InputService.Columns[2].ReadOnly = false;
                InputService.Columns[3].Visible = false;
                F1.autoinc(InputService);
            }
            else
            {
                InputService.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                InputService.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                InputService.DataSource = F1.dbw.getInputPaidServices(id);
                InputService.Columns[0].Width = 50;
                InputService.Columns[0].HeaderText = "№ п/п";
                InputService.Columns[1].Width = 250;
                InputService.Columns[1].HeaderText = "Наименование услуги";
                InputService.Columns[2].Width = 110;
                InputService.Columns[2].HeaderText = "Кол-во";
                InputService.ReadOnly = false;
                InputService.Columns[0].ReadOnly = true;
                InputService.Columns[1].ReadOnly = true;
                InputService.Columns[2].ReadOnly = false;
                InputService.Columns[3].Visible = false;
                F1.autoinc(InputService);
            }
        }

        private void InputService_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Введите число!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < InputService.Rows.Count;i++ )
            {
                if (InputService.Rows[i].Cells[2].Value.ToString() == "0")
                    continue;
                string id = InputService.Rows[i].Cells[3].Value.ToString();
                string amount = InputService.Rows[i].Cells[2].Value.ToString();
                if (this.freepaid == 0)
                {
                    F1.dbw.InputFreeService(id, amount);
                }
                else
                {
                    F1.dbw.InputPaidService(id, amount);
                }
            }
            MessageBox.Show("Услуги успешно добавлены!");
            Close();
        }


        private void InputService_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2)
                return;
            if ((int)InputService.Rows[e.RowIndex].Cells[e.ColumnIndex].Value < 0)
            {
                InputService.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
            }

        }

    }
}
