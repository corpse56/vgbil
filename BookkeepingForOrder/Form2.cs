using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System.Data.SqlClient;

namespace BookkeepingForOrder
{
    public partial class Form2 : Form
    {
        DataGridView dg;
        public int i = 0;
        private Font printFont;
        Form1 F1;
        public Form2(DataGridView dg_, Form1 F1)
        {
            dg = dg_;
            this.F1 = F1;
            InitializeComponent();
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            this.printFont = new Font("Arial Unicode MS", 10f);
            Rectangle rectangle;
            StringFormat format;
            format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;
            DataSet DS = new DataSet();
            int t = 0;
            F1.SqlDA.SelectCommand = new SqlCommand();
            F1.SqlDA.SelectCommand.Connection = F1.SqlCon;
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReaderRight = 3 and IDReader = " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            F1.SqlDA.Fill(DS, "t");
            #region читатель-сотрудник 
            string str = "Билет № " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            //string inv = DS.Tables["t"].Rows[0][1].ToString();
            string dep = GetDepartment(DS.Tables["t"].Rows[0]["IDOrganization"].ToString());
            string abonement = GetAbonement(dg.SelectedRows[0].Cells["fio"].Value.ToString());
            int CurrentY = 0;
            rectangle = new Rectangle(0, CurrentY, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "НА ДОМ\n до:";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            //rectangle = new Rectangle(0, 0, 315, 800);
            //e.Graphics.DrawRectangle(Pens.Aqua, rectangle);



            rectangle = new Rectangle(70, CurrentY, 245, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.AddDays(30).ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 10f);
            str = F1.Floor.Substring(F1.Floor.IndexOf("-") + 2);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 13f);
            str = "Билет № " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;
            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            F1.SqlDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring(ISNULL(FatherName,' '),1,1)+case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            DS = new DataSet();
            t = F1.SqlDA.Fill(DS, "t");
            printFont = new Font("Arial Unicode MS", 10f);
            str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString("Сотрудник отдела: " + dep, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Шифр: " + dg.SelectedRows[0].Cells["shifr"].Value.ToString(); ;
            printFont = new Font("Arial Unicode MS", 13f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            if (dg.SelectedRows[0].Cells["note"].Value.ToString() == string.Empty)
            {
                str = "Инв. № " + dg.SelectedRows[0].Cells["inv"].Value.ToString();
            }
            else
            {
                str = "Инв. № " + dg.SelectedRows[0].Cells["inv"].Value.ToString() + "; метка: " + dg.SelectedRows[0].Cells["note"].Value.ToString();
            }
            printFont = new Font("Arial Unicode MS", 13f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            /*rectangle = new Rectangle(158, 175, 315, 25);
            str = dg.SelectedRows[0].Cells["note"].Value.ToString();
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);*/
            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Автор: " + dg.SelectedRows[0].Cells["avt"].Value.ToString();
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 315, 75);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Заглавие: " + dg.SelectedRows[0].Cells["zag"].Value.ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            CurrentY += 75;
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            F1.SqlDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 101 and lng.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "where A.IDMAIN = " + dg.SelectedRows[0].Cells["idm"].Value.ToString();
            DS = new DataSet();
            t = F1.SqlDA.Fill(DS, "t");
            str = "Язык: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            F1.SqlDA.SelectCommand.CommandText = "select (case when Plng.PLAIN is null then '<нет>' else Plng.PLAIN end) as first, (case when Ptom.PLAIN is null then '<нет>' else Ptom.PLAIN end) as second " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 2100 and lng.MSFIELD = '$d' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "left join BJVVV..DATAEXT tom on A.IDMAIN = tom.IDMAIN and tom.MNFIELD = 225 and tom.MSFIELD = '$h' " +
                "left join BJVVV..DATAEXTPLAIN Ptom on Ptom.IDDATAEXT = tom.ID " +
                "where A.IDMAIN = " + dg.SelectedRows[0].Cells["idm"].Value.ToString();
            DS = new DataSet();
            t = F1.SqlDA.Fill(DS, "t");
            str = "Год: " + DS.Tables["t"].Rows[0][0].ToString() + "   Том: " + DS.Tables["t"].Rows[0][1].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Место издания: " + dg.SelectedRows[0].Cells["gizd"].Value.ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            //rectangle = new Rectangle(0, 325, 315, 25);
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            //str = "Подпись читателя";
            //e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.Date.ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 75);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);


            //========вторая часть требования
            DS = new DataSet();
            t = 0;// Conn.SQLDA.Fill(DS, "t");
            str = "Билет № " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            CurrentY += 75;
            rectangle = new Rectangle(0, CurrentY, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);

            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 13f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;
            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            F1.SqlDA.SelectCommand.CommandText = "select ISNULL(FamilyName+' ' +substring([Name],1,1)+'. ',' ') + substring(ISNULL(FatherName,' '),1,1)+case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            DS = new DataSet();
            t = F1.SqlDA.Fill(DS, "t");
            printFont = new Font("Arial Unicode MS", 10f);
            str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString("Сотрудник отдела: " + dep, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;


            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "НА ДОМ";
            printFont = new Font("Arial Unicode MS", 11f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Шифр: " + dg.SelectedRows[0].Cells["shifr"].Value.ToString(); ;
            printFont = new Font("Arial Unicode MS", 13f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            if (dg.SelectedRows[0].Cells["note"].Value.ToString() == string.Empty)
            {
                str = "Инв. № " + dg.SelectedRows[0].Cells["inv"].Value.ToString();
            }
            else
            {
                str = "Инв. № " + dg.SelectedRows[0].Cells["inv"].Value.ToString() + "; метка: " + dg.SelectedRows[0].Cells["note"].Value.ToString();
            }
            printFont = new Font("Arial Unicode MS", 13f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            CurrentY += 25;
            printFont = new Font("Arial Unicode MS", 10f);
            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.Date.ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;
            rectangle = new Rectangle(0, CurrentY, 315, 10);
            e.Graphics.DrawRectangle(Pens.White, rectangle);
            #endregion



        }
        private string GetAbonement(string p)
        {
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..ReaderRight A " +
                                                " left join Readers..ReaderRightList B on A.IDReaderRight = B.IDReaderRight " +
                                                "where A.IDReader = " + p;
            DataSet DS = new DataSet();
            F1.SqlDA.Fill(DS, "t");
            string retval = string.Empty;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                if (((int)r["IDReaderRight"] == 4) || ((int)r["IDReaderRight"] == 5) || ((int)r["IDReaderRight"] == 6))
                {
                    retval += r["NameReaderRight"].ToString() + "; ";
                }
            }
            return retval.TrimEnd();
        }
        private string GetDepartment(string p)
        {
            F1.SqlDA.SelectCommand.CommandText = "select SHORTNAME from BJVVV..LIST_8 where ID = " + p;
            DataSet DS = new DataSet();
            F1.SqlDA.Fill(DS, "t");
            return DS.Tables["t"].Rows[0]["SHORTNAME"].ToString();
        }

    }
}
