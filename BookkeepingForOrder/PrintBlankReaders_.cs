using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;

namespace BookkeepingForOrder
{
    public class PrintBlankReaders
    {
        private static PrintDocument pd;
        private Font printFont;
        private DbForEmployee db;
        private Form1 F1;
        private System.Windows.Forms.DataGridView dg;
        private int PaperSizeForReaders = 742;
        private int PaperSizeForEmployee = 837;
        public PrintBlankReaders(DbForEmployee db_, System.Windows.Forms.DataGridView dg_, string Dept, Form1 f1)
        {
            this.F1 = f1;
            this.db = db_;
            this.dg = dg_;
            pd = new PrintDocument();
            switch (Dept)
            {
                case "Книгохранение - 2 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 2nd floor";
                        break;
                    }
                case "Книгохранение - 3 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 3rd floor";
                        break;
                    }
                case "Книгохранение - 4 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "Книгохранение - 5 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 5th floor";
                        break;
                    }
                case "Книгохранение - 6 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 6th floor";
                        break;
                    }
                case "Книгохранение - 7 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 7th floor";
                        break;
                    }
                case "Книгохранение - цоколь":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "ЦДД - Абонемент":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 CDD";
                        break;
                    }
            }
            //pd.PrinterSettings.PrinterName = "Zebra TLP2844";
            //pd.PrinterSettings.PrinterName = XmlConnections.GetConnection("/Connections/Printer");//"Zebra TLP2844";
            this.printFont = new Font("Arial Unicode MS", 10f);
            //num = this.printFont.Height;
            //pd.PrinterSettings.PrinterName = "Zebra TLP2844";
            //pd.PrinterSettings.PrinterName = "Zebra  TLP2844";
            F1.SqlDA.SelectCommand = new SqlCommand();
            F1.SqlDA.SelectCommand.Connection = F1.SqlCon;
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            DataSet DS = new DataSet();
            F1.SqlDA.Fill(DS, "t");
            if ((int)DS.Tables["t"].Rows[0]["WorkDepartment"] == 1)
                pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, PaperSizeForReaders);
            else
                pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, PaperSizeForEmployee);

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        }
        public void Print()
        {
            pd.Print();
        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle rectangle;
            StringFormat format;
            Font printFont = new Font("Arial Unicode MS", 11f, FontStyle.Bold);
            format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;

            F1.SqlDA.SelectCommand = new SqlCommand();
            F1.SqlDA.SelectCommand.Connection = F1.SqlCon;
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..Main where NumberReader = " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
            DataSet DS = new DataSet();
            int t = 0;
            F1.SqlDA.Fill(DS, "t");
            if ((int)DS.Tables["t"].Rows[0]["WorkDepartment"] != 1)
            {
                #region читатель-сотрудник 
                string str = "Билет № " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
                //string inv = DS.Tables["t"].Rows[0][1].ToString();
                string dep = GetDepartment(DS.Tables["t"].Rows[0]["WorkDepartment"].ToString());
                string abonement = GetAbonement(dg.SelectedRows[0].Cells["fio"].Value.ToString());
                int CurrentY = 0;
                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                CurrentY += 25;
                if ((((DateTime)dg.SelectedRows[0].Cells["startd"].Value).Date > DateTime.Now.Date) && (dg.Name == "dgwReaders"))
                {

                    rectangle = new Rectangle(0, CurrentY, 315, 25);
                    e.Graphics.DrawRectangle(Pens.Black, rectangle);
                    printFont = new Font("Arial Unicode MS", 14f, FontStyle.Bold);
                    str = "Заказ на " + ((DateTime)dg.SelectedRows[0].Cells["startd"].Value).Date.ToString("dd MMMM yyyy");
                    e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                    CurrentY += 25;
                }
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

                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
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
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
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
                e.Graphics.DrawString("Сотрудник отдела: "+dep, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;


                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
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
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 315, 10);
                e.Graphics.DrawRectangle(Pens.White, rectangle);
            #endregion
            }
            else
            {
                #region обычный читатель
                string str = "Билет № " + dg.SelectedRows[0].Cells["fio"].Value.ToString();
                //string inv = DS.Tables["t"].Rows[0][1].ToString();
                int CurrentY = 0;
                    rectangle = new Rectangle(0, CurrentY, 315, 25);
                    e.Graphics.DrawRectangle(Pens.Black, rectangle);
                    CurrentY += 25;

                if ((((DateTime)dg.SelectedRows[0].Cells["startd"].Value).Date > DateTime.Now.Date) && (dg.Name == "dgwReaders"))
                {
                    
                    rectangle = new Rectangle(0, CurrentY, 315, 25);
                    e.Graphics.DrawRectangle(Pens.Black, rectangle);
                    printFont = new Font("Arial Unicode MS", 14f, FontStyle.Bold);
                    str = "Заказ на " + ((DateTime)dg.SelectedRows[0].Cells["startd"].Value).Date.ToString("dd MMMM yyyy");
                    e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                    CurrentY += 25;
                }
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
                F1.SqlDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring(ISNULL(FatherName, ' '),1,1)+case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["fio"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
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
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
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
                F1.SqlDA.SelectCommand.CommandText = "select ISNULL(FamilyName+' ' +substring([Name],1,1)+'. ',' ') + substring(ISNULL(FatherName, ' '),1,1)+case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["fio"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
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
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 315, 10);
                e.Graphics.DrawRectangle(Pens.White, rectangle);

            #endregion
            }
        }

        private string GetAbonement(string p)
        {
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReader = " + p;
            DataSet DS = new DataSet();
            F1.SqlDA.Fill(DS, "t");
            string retval = string.Empty;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                if ((int)r["IDReaderRight"] == 4)
                {
                    retval += "Индивидуальный персональный абонемент; ";
                }
                if ((int)r["IDReaderRight"] == 5)
                {
                    retval += "Персональный абонемент; ";
                }
                if ((int)r["IDReaderRight"] == 6)
                {
                    retval += "Коллективный абонемент; ";
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
