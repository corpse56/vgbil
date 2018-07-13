using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Circulation
{
    public partial class Form1 : Form
    {
        #region for virtual mode
        DataTable VirtualTable;
        Form3 f3;
        void Statistics_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Value = e.RowIndex + 1;
            }
            else
            {
                e.Value = VirtualTable.Rows[e.RowIndex][e.ColumnIndex].ToString();
            }
        }
        private void Statistics_Scroll(object sender, ScrollEventArgs e)
        {
            //Statistics.AutoResizeRows(DataGridViewAutoSizeRowsMode.DisplayedCells);
            Statistics.Columns[1].Width += 1;
            Statistics.Columns[1].Width -= 1;
        }
        #endregion

        private void выданныеКнигиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги выданные в залы";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooks();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Номер читателя";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Номер бронеполки";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Дата окончания брони";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Класс читателя";
            Statistics.Columns[7].Width = 90;
            Statistics.Columns[8].HeaderText = "Отдел";
            Statistics.Columns[8].Width = 90;
            Statistics.Columns[9].HeaderText = "Зал/Дом";
            Statistics.Columns[9].Width = 90;
        }
        #region обращаемость книг по пину
        private void обращаемостьКнигПоПинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Общая обращаемость книг по PIN";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker2.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;


        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN();
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true; 
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();

        }
        #endregion

        #region обращаемость книг по инвентарному номеру
        private void обращаемостьКнигПоИнвентарномуНомеруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Общая обращаемость книг по инвентарному номеру";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker1.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            //Statistics.Columns.Add("c6", "");
            
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll +=new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
            autoinc(Statistics);
        }


        #endregion

        #region все книги на ДП в текущем зале
        private void всеКнигиНаДПВТекущемЗалеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Все книги ДП текущего зала";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            GetAllBooksInHallDelegate getAllBooksInHallDelegate = dbw.GetAllBooksInHall;
            try
            {
                getAllBooksInHallDelegate.BeginInvoke(getallbooksISDONE, getAllBooksInHallDelegate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show(s, "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }
        private void getallbooksISDONE(IAsyncResult ar)
        {
            GetAllBooksInHallDelegate Done = (GetAllBooksInHallDelegate)ar.AsyncState;
            VirtualTable = (DataTable)Done.EndInvoke(ar);
            EndGetAllBooksInHallDelegate endGetAllBooksInHallDelegate = EndGetAllBooks;
            Statistics.Invoke(endGetAllBooksInHallDelegate, new object[] { VirtualTable });
        }
        private void EndGetAllBooks(DataTable t)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.Columns.Add("c6", "");
            Statistics.Columns.Add("c7", "");
            Statistics.Columns.Add("c8", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true; Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Место на полке";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Статус";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Номер читателя";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Обращае мость";
            Statistics.Columns[7].Width = 90;
            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion

        #region все книги "Для выдачи" в текущем зале
        private void всеКнигиДляВыдачиТекущегоЗалаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Все книги \"Для выдачи\" текущего зала";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            GetAllBooksInHallDelegate getAllBooksInHallDelegate = dbw.GetAllBooksInHall_forissue;
            try
            {
                getAllBooksInHallDelegate.BeginInvoke(getallbooksISDONE_forissue, getAllBooksInHallDelegate);
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show(s, "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }
        private void getallbooksISDONE_forissue(IAsyncResult ar)
        {
            GetAllBooksInHallDelegate Done = (GetAllBooksInHallDelegate)ar.AsyncState;
            VirtualTable = (DataTable)Done.EndInvoke(ar);
            EndGetAllBooksInHallDelegate endGetAllBooksInHallDelegate = EndGetAllBooks_forissue;
            Statistics.Invoke(endGetAllBooksInHallDelegate, new object[] { VirtualTable });
        }
        private void EndGetAllBooks_forissue(DataTable t)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.Columns.Add("c6", "");
            Statistics.Columns.Add("c7", "");
            Statistics.Columns.Add("c8", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true; Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 300;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инв номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Место на полке";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Статус";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Номер читателя";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Обращае мость";
            Statistics.Columns[7].Width = 90;
            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion


        #region обращаемость Книг Находящихся На ДП В Текущ По Инвентарному Номеру
        private void обращаемостьКнигНаходящихсяНаДПВТекущПоИнвентарномуНомеруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ДП текущего зала по инвентарному номеру";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker3.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_InHallDP();

        }
        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
            autoinc(Statistics);
        }

        #endregion


        #region обращаемость Книг Находящихся На ДП В Текущ По пину
        private void емЗалеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ДП текущего зала по PIN";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker4.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;

        }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN_INHALLDP();

        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
        }
        #endregion

        #region обращаемость книг бравшихся в текущем зале из книгохранения по инвентарному номеру
        private void обращаемостьКнигБравшихсяВТекущемЗалеИзКнигохраненияПоИнвентарномуНомеруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ОФ в текущем зале по инвентарному номеру";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker5.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_BK();

        }
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion
        #region обращаемость книг бравшихся в текущем зале из книгохранения по пину
        private void обращаемостьКнигБравшихсяВТекущемЗалеИзКнигохраненияПоПинуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ОФ текущего зала по PIN";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker6.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }
        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN_BK();

        }
        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
        }
        #endregion
        
        #region Обращаемость книг ДП текущего зала по PIN за период
        private void обращаемостьКнигДПТекущегоЗалаПоPINЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ДП текущего зала по PIN за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker7.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }
        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN_INHALLDP_PERIOD(f3.StartDate,f3.EndDate);
        }
        private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
        }
        #endregion
        #region Обращаемость книг ДП текущего зала по инвентарному номеру за период
        private void обращаемостьКнигДПТекущегоЗалаПоИнвентарномуНомеруЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ДП текущего зала по инв. номеру за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker8.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }
        private void backgroundWorker8_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_InHallDP_PERIOD(f3.StartDate,f3.EndDate);

        }

        private void backgroundWorker8_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion

        #region Обращаемость книг ОФ текущего зала по PIN за период
        private void обращаемостьКнигОФТекущегоЗалаПоPINЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ОФ текущего зала по PIN за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker9.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }

        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN_BK_PERIOD(f3.StartDate,f3.EndDate);

        }

        private void backgroundWorker9_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
        }
        #endregion
        #region Обращаемость книг ОФ в текущем зале по инвентарному номеру за период
        private void обращаемостьКнигОФВТекущемЗалеПоИнвентарномуНомеруЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Обращаемость книг ОФ в текущем зале по инв. номеру за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker10.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }

        private void backgroundWorker10_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_BK_PERIOD(f3.StartDate,f3.EndDate);

        }

        private void backgroundWorker10_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.Columns.Add("c6", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[2].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            Statistics.Columns[5].HeaderText = "Тематика";
            Statistics.Columns[5].Width = 80;
            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion

        #region список Всех Книг Находящихся В Открытом Доступе В Зале Абонементного Обслуживания
        private void спписокВсехКнигНаходящихсяВОткрытомДоступеВЗалеАбонементногоОбслуживанияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateSelect dsel = new DateSelect();
            dsel.ShowDialog();
            if (dsel == null) return;
            selectedDate = dsel._selDate;
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Список все книг находящихся в открытом доступе в зале абонементного обслуживания на "+selectedDate.ToString("yyyyMMdd");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker14.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            //Statistics.VirtualMode = true;
        }
        private DateTime selectedDate;
        private void backgroundWorker14_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetAllBooksInAbonement(selectedDate);

        }

        private void backgroundWorker14_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.Columns.Add("c6", "");
            Statistics.Columns.Add("c7", "");
            Statistics.Columns.Add("c8", "");
            Statistics.Columns.Add("c9", "");
            Statistics.Columns.Add("c10", "");
            Statistics.Columns.Add("c11", "");
            Statistics.Columns.Add("c12", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Полка";
            Statistics.Columns[1].Width = 80;
            Statistics.Columns[2].HeaderText = "Заглавие";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[4].HeaderText = "Год";
            Statistics.Columns[4].Width = 50;
            Statistics.Columns[5].HeaderText = "Инв. н.";
            Statistics.Columns[5].Width = 90;
            Statistics.Columns[6].HeaderText = "Обращаемость";
            Statistics.Columns[6].Width = 50;
            Statistics.Columns[7].HeaderText = "Выдача";
            Statistics.Columns[7].Width = 70;
            Statistics.Columns[8].HeaderText = "Тематическая предметная рубрика";
            Statistics.Columns[8].Width = 160;
            Statistics.Columns[9].HeaderText = "Язык";
            Statistics.Columns[9].Width = 50;
            Statistics.Columns[10].HeaderText = "Тематика";
            Statistics.Columns[10].Width = 70;
            Statistics.Columns[11].HeaderText = "Класс издания";
            Statistics.Columns[11].Width = 70;

            RndPrg.Dispose();
            autoinc(Statistics);
        }
        #endregion
        #region Общая обращаемость книг по пин за период
        private void общаяОбращаемостьКнигПоPINЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Общая обращаемость книг по PIN за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            backgroundWorker11.RunWorkerAsync();

            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
        }

        private void backgroundWorker11_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityPIN_PERIOD(f3.StartDate,f3.EndDate);

        }

        private void backgroundWorker11_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Пин";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
        }
        #endregion

        #region Общая обращаемость книг по инвентарному номеру за период
        private void общаяОбращаемостьКнигПоИнвентарномуНомеруЗаПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f3 = new Form3();
            f3.ShowDialog();
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Общая обращаемость книг по инвентарному номеру за период с " + f3.StartDate.ToString("dd.MM.yyyy") + " по " + f3.EndDate.ToString("dd.MM.yyyy");
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker12.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }

        private void backgroundWorker12_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_PERIOD(f3.StartDate,f3.EndDate);
        }

        private void backgroundWorker12_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            Statistics.Columns.Add("c6", "");
            Statistics.Columns.Add("c7", "");
            Statistics.Columns.Add("c8", "");

            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            Statistics.Columns[5].HeaderText = "Местонахождение";
            Statistics.Columns[5].Width = 110;
            Statistics.Columns[6].HeaderText = "Тематическая предметная рубрика";
            Statistics.Columns[6].Width = 150;
            Statistics.Columns[7].HeaderText = "Тематика";
            Statistics.Columns[7].Width = 100;
            RndPrg.Dispose();
            autoinc(Statistics);
        }

        #endregion
        #region обращаемость книг фонда абонемент по инвентарному номеру
        private void обращаемостьКнигФондаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Общая обращаемость книг по инвентарному номеру в фонде Абонемент";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            backgroundWorker13.RunWorkerAsync();
            RndPrg = new ExtGui.RoundProgress();
            RndPrg.Visible = true;
            RndPrg.Name = "progress";
            this.Controls.Add(RndPrg);
            RndPrg.BringToFront();
            RndPrg.Size = new Size(40, 60);
            RndPrg.Location = new Point(this.Width / 2, this.Height / 2);
            RndPrg.BackColor = SystemColors.AppWorkspace;
            Statistics.VirtualMode = true;
        }
        private void backgroundWorker13_DoWork(object sender, DoWorkEventArgs e)
        {
            VirtualTable = (DataTable)dbw.GetNegotiabilityINV_ABONEMENT();

        }

        private void backgroundWorker13_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Statistics.DataSource = null;
            Statistics.Columns.Add("c1", "");
            Statistics.Columns.Add("c2", "");
            Statistics.Columns.Add("c3", "");
            Statistics.Columns.Add("c4", "");
            Statistics.Columns.Add("c5", "");
            //Statistics.Columns.Add("c6", "");

            Statistics.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Statistics.CellValueNeeded += new DataGridViewCellValueEventHandler(Statistics_CellValueNeeded);
            Statistics.Scroll += new ScrollEventHandler(Statistics_Scroll);
            Statistics.RowCount = VirtualTable.Rows.Count;
            Statistics.VirtualMode = true;
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 40;
            Statistics.Columns[1].HeaderText = "Заглавие";
            Statistics.Columns[1].Width = 500;
            Statistics.Columns[2].HeaderText = "Автор";
            Statistics.Columns[3].Width = 150;
            Statistics.Columns[3].HeaderText = "Инвентарный номер";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Обращае мость";
            Statistics.Columns[4].Width = 110;
            RndPrg.Dispose();
            autoinc(Statistics);
        }

        #endregion
        private void книгиВыданныеНаДомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги на руках выданные на дом";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooksAtHome();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие; Автор";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[2].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[2].Width = 100;
            Statistics.Columns[3].HeaderText = "Номер читателя";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Права читателя";
            Statistics.Columns[4].Width = 120;
            Statistics.Columns[5].HeaderText = "Кафедра выдачи";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Дата выдачи";
            Statistics.Columns[6].Width = 90;
            Statistics.Columns[7].HeaderText = "Дата возврата";
            Statistics.Columns[7].Width = 90;
            Statistics.Columns[8].HeaderText = "Дней просро чено";
            Statistics.Columns[8].Width = 60;
        }
        private void книгиНаРукахУСотрудниковСПросроченнымСрокомСдачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги на руках, с просроченным сроком сдачи";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooksAtHomeOverDue();
                VirtualTable = (DataTable)Statistics.DataSource;

            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "Заглавие; Автор";
            Statistics.Columns[1].Width = 250;
            Statistics.Columns[2].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[2].Width = 100;
            Statistics.Columns[3].HeaderText = "Номер читателя";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "ФИО";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Телефон";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Права читателя";
            Statistics.Columns[6].Width = 120;
            Statistics.Columns[7].HeaderText = "Кафедра выдачи";
            Statistics.Columns[7].Width = 100;
            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].Width = 90;
            Statistics.Columns[9].HeaderText = "Дата возврата";
            Statistics.Columns[9].Width = 90;
            Statistics.Columns[10].HeaderText = "Дней просро чено";
            Statistics.Columns[10].Width = 60;
            if (Statistics.Rows.Count != 0)
                button18.Enabled = true;
        }
        private void книгиВыданныеНаДомПоэтажноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Книги на руках выданные на дом поэтажно";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetIssBooksAtHomeByFloor();
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            //autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "Этаж";
            Statistics.Columns[0].Width = 200;
            Statistics.Columns[1].HeaderText = "Кол-во сотрудники";
            Statistics.Columns[1].Width = 200;
            Statistics.Columns[2].HeaderText = "Кол-во читатели";
            Statistics.Columns[2].Width = 200;
            Statistics.Columns[3].HeaderText = "Кол-во всего";
            Statistics.Columns[3].Width = 200;
        }
        private void списокЧитателейИКнигВыданныхНаДомСПросроченнымСрокомСдачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            //label19.Text = "Книги на руках выданные на дом, с просроченным сроком сдачи";
            label19.Text = "Список нарушителей сроков пользования документов из основного фонда";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            try
            {
                Statistics.DataSource = dbw.GetReadersAndIssBooksAtHomeOverDue();
                VirtualTable = (DataTable)Statistics.DataSource;
            }
            catch (IndexOutOfRangeException ev)
            {
                string s = ev.Message;
                MessageBox.Show("Таких книг нет!", "Информация.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Statistics.Columns.Clear();
                return;
            }
            autoinc(Statistics);

            //ФИО
            //Номер билета
            //Права читателя
            //Инвентарный номер
            //телефон
            //Эл.почта
            //Адрес
            //Дата выдачи
            //Срок сдачи
            //Дней просрочено

            Statistics.Columns[0].HeaderText = "№№";
            Statistics.Columns[0].Width = 30;
            Statistics.Columns[1].HeaderText = "ФИО";
            Statistics.Columns[1].Width = 100;
            Statistics.Columns[2].HeaderText = "Номер читателя";
            Statistics.Columns[2].Width = 100;
            Statistics.Columns[3].HeaderText = "Права читателя";
            Statistics.Columns[3].Width = 100;
            Statistics.Columns[4].HeaderText = "Инв номер/ шкод";
            Statistics.Columns[4].Width = 100;
            Statistics.Columns[5].HeaderText = "Телефон";
            Statistics.Columns[5].Width = 100;
            Statistics.Columns[6].HeaderText = "Email";
            Statistics.Columns[6].Width = 100;
            Statistics.Columns[7].HeaderText = "Адрес";
            Statistics.Columns[7].Width = 100;

            Statistics.Columns[8].HeaderText = "Дата выдачи";
            Statistics.Columns[8].Width = 90;
            Statistics.Columns[9].HeaderText = "Дата возврата";
            Statistics.Columns[9].Width = 90;
            Statistics.Columns[10].HeaderText = "Дней просрочено";
            Statistics.Columns[10].Width = 90;
            //Statistics.Columns[4].Visible = false;

            //Statistics.Columns[11].HeaderText = "Примечание";
           // Statistics.Columns[11].Width = 120;
            if (Statistics.Rows.Count != 0)
                button18.Enabled = true;
        }



        private void количествоЧитателейСПравамиПлатногоИЕсплатногоАбонементаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistics.Columns.Clear();
            Statistics.AllowUserToAddRows = false;
            Statistics.AutoGenerateColumns = true;
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            label19.Text = "Количество читателей с действующими правами платного и бесплатного абонемента";
            Statistics.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Statistics.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            
            Statistics.DataSource = dbw.GetReadersAbonementRights();

            //autoinc(Statistics);
            Statistics.Columns[0].HeaderText = "Абонемент";
            Statistics.Columns[0].Width = 300;
            Statistics.Columns[1].HeaderText = "Кол-во читателей";
            Statistics.Columns[1].Width = 200;
            
        }
    }
}
