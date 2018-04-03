using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Circulation
{
    public partial class Form7 : Form
    {
        public Form7(dbReader reader, string BASENAME)
        {
            InitializeComponent();
            label2.Text = reader.FIO;
            Conn.SQLDA.SelectCommand.CommandText = "select A.ID,(case when A.INV = '-1' then EE.[NAME] else C.PLAIN end) as zag, " +
                                                   " E.PLAIN avt,A.DATE_ISSUE,A.DATE_RET, A.ZALISS,(case when A.INV = '-1' then EE.BARCODE else A.INV end) as INV " +
                                                   " from " + BASENAME + "..ISSUED_OF_HST A" +
                                                   " left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                                   " left join BJVVV..DATAEXTPLAIN C on B.ID = C.IDDATAEXT " +
                                                   " left join BJVVV..DATAEXT D on A.IDMAIN = D.IDMAIN and D.MNFIELD = 700 and D.MSFIELD = '$a' " +
                                                   " left join BJVVV..DATAEXTPLAIN E on D.ID = E.IDDATAEXT " +
                                                   " left join " + BASENAME + "..PreDescr EE on EE.BARCODE = A.BAR " +
                                                   " where A.IDREADER = " + reader.id ;
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS,"lll");
            dataGridView1.DataSource = DS.Tables["lll"];

            dataGridView1.Columns["DATE_ISSUE"].HeaderText = "Дата выдачи";
            dataGridView1.Columns["DATE_ISSUE"].Width = 70;
            dataGridView1.Columns["DATE_RET"].HeaderText = "Дата возврата";
            dataGridView1.Columns["DATE_RET"].Width = 70;
            dataGridView1.Columns["ZALISS"].HeaderText = "Выдано в зале";
            dataGridView1.Columns["INV"].HeaderText = "Инв номер/ шкод";
            dataGridView1.Columns["ZAG"].HeaderText = "Заглавие";
            dataGridView1.Columns["AVT"].HeaderText = "Автор";
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
