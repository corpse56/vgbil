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
    public partial class Form38 : Form
    {
        Form1 F1;
        int ISFREE;
        public Form38(Form1 f1,int isfree)
        {
            InitializeComponent();
            F1 = f1;
            ISFREE = isfree;
        }

        private void Form38_Load(object sender, EventArgs e)
        {
            if (ISFREE == 0)//бесплатные
            {
                ShowFreeServices();
            }
            else//платные
            {
                ShowPaidServices();
            }

        }

        private void ShowFreeServices()
        {
            ServiceGrid.AutoGenerateColumns = true;
            ServiceGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            ServiceGrid.DataSource = F1.dbw.getFreeServicesEdit();
            ServiceGrid.Columns[0].Width = 50;
            ServiceGrid.Columns[0].HeaderText = "№ п/п";
            ServiceGrid.Columns[1].Width = 450;
            ServiceGrid.Columns[1].HeaderText = "Наименование услуги";
            ServiceGrid.Columns[2].Visible = false;
            ServiceGrid.ReadOnly = true;
            F1.autoinc(ServiceGrid);
        }

        private void ShowPaidServices()
        {
            ServiceGrid.AutoGenerateColumns = true;

            ServiceGrid.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            ServiceGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            
            ServiceGrid.DataSource = F1.dbw.getPaidServicesEdit();
            ServiceGrid.Columns[0].Width = 50;
            ServiceGrid.Columns[0].HeaderText = "№ п/п";
            ServiceGrid.Columns[1].Width = 420;
            ServiceGrid.Columns[1].HeaderText = "Наименование услуги";
            ServiceGrid.Columns[2].Width = 80;
            ServiceGrid.Columns[2].HeaderText = "Цена";
            ServiceGrid.Columns[3].Visible = false;
            ServiceGrid.ReadOnly = true;
            F1.autoinc(ServiceGrid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ISFREE == 0)
            {
                Form36 f36 = new Form36(F1);
                f36.ShowDialog();
                ShowFreeServices();
            }
            else
            {
                Form37 f37 = new Form37(F1);
                f37.ShowDialog();
                ShowPaidServices();
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ISFREE == 0)
            {
                Form36 f36 = new Form36(F1, ServiceGrid.SelectedRows[0].Cells[1].Value.ToString(), ServiceGrid.SelectedRows[0].Cells[2].Value.ToString());
                f36.ShowDialog();
                ShowFreeServices();
            }
            else
            {
                Form37 f37 = new Form37(F1, ServiceGrid.SelectedRows[0].Cells[1].Value.ToString(), ServiceGrid.SelectedRows[0].Cells[3].Value.ToString(), ServiceGrid.SelectedRows[0].Cells[2].Value.ToString());
                f37.ShowDialog();
                ShowPaidServices();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы уверены что хотите удалить выбранную услугу?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (ISFREE == 0)//бесплатные
                {
                    F1.dbw.DelFreeService(ServiceGrid.SelectedRows[0].Cells[2].Value.ToString());
                    ShowFreeServices();
                    MessageBox.Show("Услуга успешно удалена!");
                }
                else
                {
                    F1.dbw.DelPaidService(ServiceGrid.SelectedRows[0].Cells[3].Value.ToString());
                    ShowPaidServices();
                    MessageBox.Show("Услуга успешно удалена!");
                }
            }
            else
            {
                return;
            }
        }
    }
}
