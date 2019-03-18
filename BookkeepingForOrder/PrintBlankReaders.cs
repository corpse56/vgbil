using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Readers.ReadersRights;

namespace BookkeepingForOrder
{
    public class PrintBlankReaders
    {
        private static PrintDocument pd;
        private Font printFont;
        private DbForEmployee db;
        private Form1 F1;
        private System.Windows.Forms.DataGridView dg;
        private int PaperSizeForReaders = 800;
        private int PaperSizeForEmployee = 837;
        private ReaderInfo Reader;
        public PrintBlankReaders(DbForEmployee db_, System.Windows.Forms.DataGridView dg_, string Dept, Form1 f1)
        {
            this.F1 = f1;
            this.db = db_;
            this.dg = dg_;
            pd = new PrintDocument();

            #region PrinterNaming
            switch (Dept)
            {
                case "…Хран… Сектор книгохранения - 2 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 2nd floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - 3 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 3rd floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - 4 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - Новая периодика":
                    {
                        //pd.PrinterSettings.PrinterName = "Zebra TLP2844 4th floor";
                        pd.PrinterSettings.PrinterName = XmlConnections.GetConnection("/Connections/FourthFloorNewPeriodica");
                        break;
                    }
                case "…Хран… Сектор книгохранения - 5 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 5th floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - 6 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 6th floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - 7 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 7th floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - 0 этаж":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "…Хран… Сектор книгохранения - Абонемент":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 CDD";
                        
                        break;
                    }
            }
            #endregion

            this.printFont = new Font("Arial Unicode MS", 10f);
            //pd.PrinterSettings.PrinterName = "Zebra TLP2844";
            //pd.PrinterSettings.PrinterName = "HP LaserJet 5000 Series PCL 5";
            pd.PrinterSettings.PrinterName = "HP LaserJet M1522 MFP Series PCL 6";


            //Reader = ReaderInfo.GetReader(Convert.ToInt32(dg.SelectedRows[0].Cells["readerid"].Value));
            //ReaderRight EmployeeRight = new ReaderRight();
            
            //if (Reader.Rights.RightsList.Exists( x => x.ReaderRightValue == ReaderRightsEnum.Employee))
            //{
            //    pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, PaperSizeForReaders);
            //}
            //else
            //{
            //    pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, PaperSizeForEmployee);
            //}

            F1.SqlDA.SelectCommand = new SqlCommand();
            F1.SqlDA.SelectCommand.Connection = F1.SqlCon;
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReaderRight = 3 and IDReader = " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
            DataSet DS = new DataSet();
            int cc = F1.SqlDA.Fill(DS, "t");
            if (cc != 0)
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
            F1.SqlDA.SelectCommand.CommandText = "select * from Readers..ReaderRight where IDReaderRight = 3 and IDReader = " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
            DataSet DS = new DataSet();
            int t = 0;
            int cc = F1.SqlDA.Fill(DS, "t");
            if (cc != 0)
            {
                #region читатель-сотрудник 
                string str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                //string inv = DS.Tables["t"].Rows[0][1].ToString();
                string dep = GetDepartment(DS.Tables["t"].Rows[0]["IDOrganization"].ToString());
                string abonement = GetAbonement(dg.SelectedRows[0].Cells["readerid"].Value.ToString());
                int CurrentY = 0;

                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                CurrentY += 25;

                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "НА ДОМ\n до:";
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = DateTime.Now.AddDays(30).ToString("dd.MM.yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 10f);
                str = F1.user.SelectedUserStatus.DepName.Substring(F1.user.SelectedUserStatus.DepName.IndexOf("-") + 2);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, CurrentY, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 13f);
                str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                CurrentY += 25;
                rectangle = new Rectangle(70, CurrentY, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring(ISNULL(FatherName,' '),1,1)+case when FatherName is null then '' else '.' end " +
                                                    " from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
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
                str = "Шифр: " + dg.SelectedRows[0].Cells["cipher"].Value.ToString(); ;
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
                str = "Автор: " + dg.SelectedRows[0].Cells["author"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 10f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 315, 75);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Заглавие: " + dg.SelectedRows[0].Cells["title"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                CurrentY += 75;
                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 101 and lng.MSFIELD = '$a' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "where A.IDMAIN = " + dg.SelectedRows[0].Cells["pin"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                str = "Язык: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select (case when Plng.PLAIN is null then '<нет>' else Plng.PLAIN end) as first," +
                    " (case when Ptom.PLAIN is null then '<нет>' else Ptom.PLAIN end) as second " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 2100 and lng.MSFIELD = '$d' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "left join BJVVV..DATAEXT tom on A.IDMAIN = tom.IDMAIN and tom.MNFIELD = 225 and tom.MSFIELD = '$h' " +
                    "left join BJVVV..DATAEXTPLAIN Ptom on Ptom.IDDATAEXT = tom.ID " +
                    "where A.IDMAIN = " + dg.SelectedRows[0].Cells["pin"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                str = "Год: " + DS.Tables["t"].Rows[0][0].ToString() + "   Том: " + DS.Tables["t"].Rows[0][1].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Место издания: " + dg.SelectedRows[0].Cells["pubdate"].Value.ToString();
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
                str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
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
                F1.SqlDA.SelectCommand.CommandText = "select ISNULL(FamilyName+' ' +substring([Name],1,1)+'. ',' ') + substring(ISNULL(FatherName,' '),1,1)+ " +
                    " case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
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
                str = "Шифр: " + dg.SelectedRows[0].Cells["cipher"].Value.ToString(); ;
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
                #endregion
            }
            else
            {
                #region обычный читатель
                BJBookInfo Book = BJBookInfo.GetBookInfoByInventoryNumber(dg.SelectedRows[0].Cells["inv"].Value.ToString(), "BJVVV");
                BJExemplarInfo Exemplar = BJExemplarInfo.GetExemplarByInventoryNumber(dg.SelectedRows[0].Cells["inv"].Value.ToString(), "BJVVV");

                string abonement = GetAbonement(dg.SelectedRows[0].Cells["readerid"].Value.ToString());
                string str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                //string inv = DS.Tables["t"].Rows[0][1].ToString();
                int CurrentY = 0;

                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                CurrentY += 25;

                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                str = (Exemplar.ExemplarAccess.Access == 1000) ? "НА ДОМ\n до:" : "ЧЗ\nдо:"; //1000 - на дом
                if (abonement.Contains("Платный"))
                {
                    str = "НА ДОМ\n до:";
                }
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, CurrentY, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = (Exemplar.ExemplarAccess.Access == 1000) ? DateTime.Now.AddDays(30).ToString("dd.MM.yyyy") : DateTime.Now.AddDays(3).ToString("dd.MM.yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 10f);
                str = F1.user.SelectedUserStatus.DepName.Substring(F1.user.SelectedUserStatus.DepName.IndexOf("-") + 2);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, CurrentY, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 13f);
                str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;

                rectangle = new Rectangle(70, CurrentY, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring(ISNULL(FatherName, ' '),1,1)+ " +
                    " case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;

                rectangle = new Rectangle(0, CurrentY, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 11f);
                e.Graphics.DrawString(abonement, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Шифр: " + dg.SelectedRows[0].Cells["cipher"].Value.ToString(); ;
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
                str = "Автор: " + dg.SelectedRows[0].Cells["author"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 10f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 315, 75);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Заглавие: " + dg.SelectedRows[0].Cells["title"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 75;

                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 101 and lng.MSFIELD = '$a' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "where A.IDMAIN = " + dg.SelectedRows[0].Cells["pin"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                str = "Язык: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;

                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                F1.SqlDA.SelectCommand.CommandText = "select (case when Plng.PLAIN is null then '<нет>' else Plng.PLAIN end) as first, " +
                    "(case when Ptom.PLAIN is null then '<нет>' else Ptom.PLAIN end) as second " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 2100 and lng.MSFIELD = '$d' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "left join BJVVV..DATAEXT tom on A.IDMAIN = tom.IDMAIN and tom.MNFIELD = 225 and tom.MSFIELD = '$h' " +
                    "left join BJVVV..DATAEXTPLAIN Ptom on Ptom.IDDATAEXT = tom.ID " +
                    "where A.IDMAIN = " + dg.SelectedRows[0].Cells["pin"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                str = "Год: " + DS.Tables["t"].Rows[0][0].ToString() + "   Том: " + DS.Tables["t"].Rows[0][1].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;

                rectangle = new Rectangle(0, CurrentY, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Место издания: " + dg.SelectedRows[0].Cells["pubdate"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
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
                str = "Билет № " + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
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
                F1.SqlDA.SelectCommand.CommandText = "select ISNULL(FamilyName+' ' +substring([Name],1,1)+'. ',' ') + substring(ISNULL(FatherName, ' '),1,1)+ " +
                    " case when FatherName is null then '' else '.' end  from  Readers..Main where NumberReader =" + dg.SelectedRows[0].Cells["readerid"].Value.ToString();
                DS = new DataSet();
                t = F1.SqlDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 25;
                rectangle = new Rectangle(0, CurrentY, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = (Exemplar.ExemplarAccess.Access == 1000) ? "НА ДОМ:" : "ЧЗ"; //1000 - на дом
                if (abonement.Contains("Платный"))
                {
                    str = "НА ДОМ\n до:";
                }
                printFont = new Font("Arial Unicode MS", 11f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                CurrentY += 50;

                rectangle = new Rectangle(0, CurrentY, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Шифр: " + dg.SelectedRows[0].Cells["cipher"].Value.ToString(); ;
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

                #endregion
            }
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
