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
    public partial class Form8 : Form
    {
        public Form8(dbReader reader)
        {
            InitializeComponent();
            label2.Text = reader.FIO;
            Conn.SQLDA.SelectCommand.CommandText = "select A.ID,C.PLAIN zag, E.PLAIN avt,A.PDATE, A.INV, A.SUM from Reservation_R..PENY_HIST A" +
                                                   " inner join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                                   " inner join BJVVV..DATAEXTPLAIN C on B.ID = C.IDDATAEXT " +
                                                   " inner join BJVVV..DATAEXT D on A.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a' " +
                                                   " inner join BJVVV..DATAEXTPLAIN E on D.ID = E.IDDATAEXT " +
                                                   " where A.IDREADER = " + reader.id ;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "lll");
            dataGridView1.DataSource = DS.Tables["lll"];

            dataGridView1.Columns["PDATE"].HeaderText = "Дата выплаты";
            dataGridView1.Columns["INV"].HeaderText = "Инвентарный номер";
            dataGridView1.Columns["ZAG"].HeaderText = "Заглавие";
            dataGridView1.Columns["AVT"].HeaderText = "Автор";
            dataGridView1.Columns["SUM"].HeaderText = "Сумма штрафа";
            dataGridView1.Columns["ID"].HeaderText = "№№";
            dataGridView1.Columns["ID"].Width = 40;
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ID"].Value = (++i).ToString();
            }
            dataGridView1.Columns["ZAG"].Width = 300;
            dataGridView1.Columns["ZAG"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

        }
    }
}
