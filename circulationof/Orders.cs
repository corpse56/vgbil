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
    public partial class Orders : Form
    {
        string IDR = "";
        public Orders(string idr)
        {
            InitializeComponent();
            this.IDR = idr;
        }

        private void Orders_Load(object sender, EventArgs e)
        {
            Conn.SQLDA.SelectCommand.CommandText = "select 1,sta.[Name] sta,O.InvNumber inv, RTF.RTF rtf,O.Start_Date startd, "+
                                         " case when Status = 10 then 'Отказ: ' + O.REFUSUAL else '-' end refu, mhre.PLAIN mh, O.Form_Date fd, O.ID  "+
                                         " from Reservation_O..Orders O " +
                                         " left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                                         " left join BJVVV..DATAEXT mhr on mhr.IDDATA = O.IDDATA and mhr.MNFIELD = 899 and mhr.MSFIELD = '$a' " +
                                         " left join BJVVV..DATAEXTPLAIN mhre on mhr.ID = mhre.IDDATAEXT " +
                                         " left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                                         " left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                                         " left join Reservation_O..Status sta on sta.ID = O.Status " +
                                         " where  DT.MSFIELD='$a' and DT.MNFIELD=200 and ID_Reader = " + this.IDR + " order by O.Start_Date desc";
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "r");
            foreach (DataRow r in DS.Tables["r"].Rows)
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Rtf = r["rtf"].ToString();
                r["rtf"] = rtb.Text;
            }
            dgO.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgO.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgO.DataSource = DS.Tables["r"];
            autoinc(dgO);
            dgO.Columns[0].Width = 30;
            dgO.Columns[0].HeaderText = "№№";
            dgO.Columns[1].Width = 90;
            dgO.Columns[1].HeaderText = "Статус";
            dgO.Columns[2].Width = 90;
            dgO.Columns[2].HeaderText = "Инв. н.";
            dgO.Columns[3].Width = 400;
            dgO.Columns[3].HeaderText = "Краткое библиографическое описание";
            dgO.Columns[4].Width = 80;
            dgO.Columns[4].HeaderText = "Дата заказа";
            dgO.Columns[4].DefaultCellStyle.Format = "dd.MM.yyyy";
            dgO.Columns[5].Width = 100;
            dgO.Columns[5].HeaderText = "Отказ";
            dgO.Columns[6].Width = 100;
            dgO.Columns[6].HeaderText = "Место хранения";
            dgO.Columns[7].Width = 80;
            dgO.Columns[7].HeaderText = "Дата формирования заказа";
            dgO.Columns[7].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            dgO.Columns[8].Visible = false;

            Conn.SQLDA.SelectCommand.CommandText = "select 1,case when sta.[ID] != 10 then 'Завершено' else sta.[Name] end sta,O.InvNumber inv, RTF.RTF rtf,O.Start_Date startd, " +
                             " case when Status = 10 then 'Отказ: ' + O.REFUSUAL else '-' end refu " +
                             " from Reservation_O..OrdHis O " +
                             " left join BJVVV..DATAEXT DT on DT.IDMAIN = O.ID_Book_EC " +
                             " left join BJVVV..RTF RTF on RTF.IDMAIN = O.ID_Book_EC " +
                             " left join BJVVV..DATAEXTPLAIN DTP on DTP.IDDATAEXT = DT.ID " +
                             " left join Reservation_O..Status sta on sta.ID = O.Status " +
                             " where  DT.MSFIELD='$a' and DT.MNFIELD=200 and ID_Reader = " + this.IDR + " order by O.Start_Date desc";
            DS = new DataSet();
            Conn.SQLDA.Fill(DS, "r");
            foreach (DataRow r in DS.Tables["r"].Rows)
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Rtf = r["rtf"].ToString();
                r["rtf"] = rtb.Text;
            }
            dgOH.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgOH.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgOH.DataSource = DS.Tables["r"];
            autoinc(dgOH);
            dgOH.Columns[0].Width = 30;
            dgOH.Columns[0].HeaderText = "№№";
            dgOH.Columns[1].Width = 100;
            dgOH.Columns[1].HeaderText = "Статус";
            dgOH.Columns[2].Width = 150;
            dgOH.Columns[2].HeaderText = "Инв. н.";
            dgOH.Columns[3].Width = 450;
            dgOH.Columns[3].HeaderText = "Краткое библиографическое описание";
            dgOH.Columns[4].Width = 90;
            dgOH.Columns[4].HeaderText = "Дата заказа";
            dgOH.Columns[5].Width = 150;
            dgOH.Columns[5].HeaderText = "Отказ";




        }
        public void autoinc(DataGridView dgv)
        {
            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Value = (++i).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgO.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку в таблице заказов!");
                return;
            }
            DBWork dbw = new DBWork();
            string idOrder = dgO.SelectedRows[0].Cells["ID"].Value.ToString();
            dbw.MoveToHistoryOrders(idOrder);
            Orders_Load(sender, e);
        }
    }
}
