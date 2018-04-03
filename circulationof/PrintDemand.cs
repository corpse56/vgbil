using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;

namespace Circulation
{
    class PrintDemand
    {
        private PrintDocument pd;
        private dbBook bb;
        public string result;
        private Form1 f1;
        private PrintPreviewDialog PrintPreviewDialog1;
        public PrintDemand(dbBook b,Form1 _f1)
        {
            this.bb = b;
            this.f1 = _f1;
            Conn.SQLDA.SelectCommand.CommandText = "select * from " + f1.BASENAME + "..ISSUED_OF where BAR = '" + bb.barcode + "'";
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "t");
            if (DS.Tables["t"].Rows.Count == 0)
            {
                result = "norespan";
            }
            else
            {
                result = "ok";
            }
            
        }
        public void ShowPreview()
        {
            pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, 670);

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            switch (f1.DepName)
            {
                case "…Зал… КОО Группа выдачи документов":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 CSI";
                        //pd.PrinterSettings.PrinterName = "Zebra TLP2844";
                        break;
                    }
                default:
                    {
                        MessageBox.Show("У вас не установлен принтер Zebra!");
                        return;
                    }
            }
            //pd.PrinterSettings.PrinterName = "Zebra TLP2844 CSI";
            try
            {
                InitializePrintPreviewDialog();
                PrintPreviewDialog1.Document = pd;
                PrintPreviewDialog1.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Принтер не найден!");
                
                return;
            }
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle rectangle;
            StringFormat format;
            Font printFont = new Font("Arial Unicode MS", 11f, FontStyle.Bold);
            format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;

            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.BJVVVConn;
            //F1.SqlDA.SelectCommand.CommandText = "select RESPAN,INV from " + F1.BASENAME + "..ISSUED_OF where BAR = '" + this.bar + "'";
            DataSet DS = new DataSet();
            int t = 0;// Conn.SQLDA.Fill(DS, "t");
            string str = "Билет № " + bb.rid;// dg.SelectedRows[0].Cells["fio"].Value.ToString();
            //string inv = DS.Tables["t"].Rows[0][1].ToString();


            {
                rectangle = new Rectangle(0, 0, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                rectangle = new Rectangle(0, 25, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 10f);
                str = bb.getFloor();
                str = str.Substring(str.IndexOf("-") + 2);// F1.Floor.Substring(F1.Floor.IndexOf("-") + 2);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, 25, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 13f);
                str = "Билет № " + bb.rid;// dg.SelectedRows[0].Cells["fio"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, 50, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                Conn.SQLDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + ISNULL(substring([FatherName],1,1)+'. ',' ') from  Readers..Main where NumberReader =" + bb.rid;// dg.SelectedRows[0].Cells["fio"].Value.ToString();
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 75, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, 75, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
                printFont = new Font("Arial Unicode MS", 11f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 125, 158, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Шифр: " + bb.GetShifr();// dg.SelectedRows[0].Cells["shifr"].Value.ToString(); ;
                printFont = new Font("Arial Unicode MS", 13f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(158, 125, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                str = "Инв. № " + bb.inv;// dg.SelectedRows[0].Cells["inv"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 13f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(158, 150, 315, 25);
                str = bb.getnote();// dg.SelectedRows[0].Cells["note"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 10f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 175, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Автор: " + bb.author;// dg.SelectedRows[0].Cells["avt"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 10f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 200, 315, 75);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Заглавие: " + bb.name;// dg.SelectedRows[0].Cells["zag"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 275, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                Conn.SQLDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 101 and lng.MSFIELD = '$a' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "where A.IDMAIN = " + bb.id;// dg.SelectedRows[0].Cells["idm"].Value.ToString();
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
                str = "Язык: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 300, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                Conn.SQLDA.SelectCommand.CommandText = "select (case when Plng.PLAIN is null then '<нет>' else Plng.PLAIN end) as first, (case when Ptom.PLAIN is null then '<нет>' else Ptom.PLAIN end) as second " +
                    "from BJVVV..DATAEXT A  " +
                    "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 2100 and lng.MSFIELD = '$d' " +
                    "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                    "left join BJVVV..DATAEXT tom on A.IDMAIN = tom.IDMAIN and tom.MNFIELD = 225 and tom.MSFIELD = '$h' " +
                    "left join BJVVV..DATAEXTPLAIN Ptom on Ptom.IDDATAEXT = tom.ID " +
                    "where A.IDMAIN = " + bb.id;// dg.SelectedRows[0].Cells["idm"].Value.ToString();
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
                str = "Год " + DS.Tables["t"].Rows[0][0].ToString() + "   Том " + DS.Tables["t"].Rows[0][1].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 325, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Место издания: " + bb.getMIZD();// dg.SelectedRows[0].Cells["gizd"].Value.ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                //rectangle = new Rectangle(0, 325, 315, 25);
                //e.Graphics.DrawRectangle(Pens.Black, rectangle);
                //str = "Подпись читателя";
                //e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 350, 315, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 375, 315, 75);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);


                //========вторая часть требования
                DS = new DataSet();
                t = 0;// Conn.SQLDA.Fill(DS, "t");
                str = "Билет № " + bb.rid;// dg.SelectedRows[0].Cells["fio"].Value.ToString();

                rectangle = new Rectangle(0, 450, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                rectangle = new Rectangle(70, 450, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                printFont = new Font("Arial Unicode MS", 13f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(70, 475, 245, 25);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                Conn.SQLDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring([FatherName],1,1)+'.' from  Readers..Main where NumberReader =" + bb.rid;//dg.SelectedRows[0].Cells["fio"].Value.ToString();
                DS = new DataSet();
                t = Conn.SQLDA.Fill(DS, "t");
                printFont = new Font("Arial Unicode MS", 10f);
                str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 500, 70, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                rectangle = new Rectangle(70, 500, 245, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
                printFont = new Font("Arial Unicode MS", 11f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 550, 158, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = "Шифр: " + bb.GetShifr();// dg.SelectedRows[0].Cells["shifr"].Value.ToString(); ;
                printFont = new Font("Arial Unicode MS", 13f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(158, 550, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                str = "Инв. № " + bb.inv;// dg.SelectedRows[0].Cells["inv"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 13f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(158, 575, 315, 25);
                str = bb.getnote();// dg.SelectedRows[0].Cells["note"].Value.ToString();
                printFont = new Font("Arial Unicode MS", 10f);
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

                rectangle = new Rectangle(0, 600, 315, 50);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);
                str = DateTime.Now.Date.ToString("dd MMMM yyyy");
                e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
                rectangle = new Rectangle(0, 625, 315, 10);
                e.Graphics.DrawRectangle(Pens.White, rectangle);
            }

            /*PaperSize size;
            Rectangle rectangle;
            StringFormat format;
            size = new PaperSize("bar", 315, 500);
            Size rectsize = new Size(315, 500);
            Font printFont = new Font("Arial Unicode MS", 11f, FontStyle.Bold);
            format = new StringFormat(StringFormatFlags.NoClip);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;

            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.Connection = Conn.ZakazCon;
            Conn.SQLDA.SelectCommand.CommandText = "select RESPAN,INV from " + f1.BASENAME + "..ISSUED_OF where BAR = '" + this.bar + "'";
            DataSet DS = new DataSet();
            int t = Conn.SQLDA.Fill(DS, "t");
            string str = "Билет № " + DS.Tables["t"].Rows[0][0].ToString();
            string inv = DS.Tables["t"].Rows[0][1].ToString();

            rectangle = new Rectangle(0, 0, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);

            rectangle = new Rectangle(70, 0, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);

            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(70, 25, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select FamilyName+' ' +substring([Name],1,1)+'. ' + substring([FatherName],1,1)+'.' from  Readers..Main where NumberReader =" + DS.Tables["t"].Rows[0][0].ToString();
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            printFont = new Font("Arial Unicode MS", 10f);
            str = "Фамилия: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 50, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            rectangle = new Rectangle(70, 50, 245, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "ЧИТАЛЬНЫЙ ЗАЛ ВГБИЛ";
            printFont = new Font("Arial Unicode MS", 11f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 100, 158, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select Pshi.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT fn on fn.IDDATA = A.IDDATA and fn.MNFIELD = 899 and fn.MSFIELD = '$b' " +
                "left join BJVVV..DATAEXT rfn on fn.SORT = rfn.SORT and rfn.IDMAIN = fn.IDMAIN " +
                "left join BJVVV..DATAEXT shi on shi.MNFIELD = 899 and shi.MSFIELD = '$j' and rfn.IDDATA = shi.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN Pshi on Pshi.IDDATAEXT = shi.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            str = "Шифр: " + DS.Tables["t"].Rows[0][0].ToString();
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(158, 100, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);

            str = "Инв. № " + inv;
            printFont = new Font("Arial Unicode MS", 10f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 150, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 700 and lng.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            str = "Автор: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 175, 315, 75);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 200 and lng.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            str = "Заглавие: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 250, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 101 and lng.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            str = "Язык: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 275, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select (case when Plng.PLAIN is null then '<нет>' else Plng.PLAIN end) as first, (case when Ptom.PLAIN is null then '<нет>' else Ptom.PLAIN end) as second " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 2100 and lng.MSFIELD = '$d' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "left join BJVVV..DATAEXT tom on A.IDMAIN = tom.IDMAIN and tom.MNFIELD = 225 and tom.MSFIELD = '$h' " +
                "left join BJVVV..DATAEXTPLAIN Ptom on Ptom.IDDATAEXT = tom.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            DS = new DataSet();
            t = Conn.SQLDA.Fill(DS, "t");
            str = "Год " + DS.Tables["t"].Rows[0][0].ToString() + "   Том " + DS.Tables["t"].Rows[0][1].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 300, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Conn.SQLDA.SelectCommand.CommandText = "select Plng.PLAIN " +
                "from BJVVV..DATAEXT A  " +
                "left join BJVVV..DATAEXT lng on A.IDMAIN = lng.IDMAIN and lng.MNFIELD = 210 and lng.MSFIELD = '$a' " +
                "left join BJVVV..DATAEXTPLAIN Plng on Plng.IDDATAEXT = lng.ID " +
                "where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = '" + this.bar + "'";
            str = "Место издания: " + DS.Tables["t"].Rows[0][0].ToString();
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 325, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Подпись читателя";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 350, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.Date.ToString("dd MMMM yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(0, 375, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);*/
        }

        private void InitializePrintPreviewDialog()
        {

            // Create a new PrintPreviewDialog using constructor.
            this.PrintPreviewDialog1 = new PrintPreviewDialog();

            //Set the size, location, and name.
            this.PrintPreviewDialog1.ClientSize =
                new System.Drawing.Size(800, 600);
            this.PrintPreviewDialog1.Location =
                new System.Drawing.Point(29, 29);
            this.PrintPreviewDialog1.Name = "PrintPreviewDialog1";
            // Associate the event-handling method with the 
            // document's PrintPage event.
            //this.pd.PrintPage +=
            //    new System.Drawing.Printing.PrintPageEventHandler
            //    (document_PrintPage);

            // Set the minimum size the dialog can be resized to.
            this.PrintPreviewDialog1.MinimumSize =
                new System.Drawing.Size(375, 250);

            // Set the UseAntiAlias property to true, which will allow the 
            // operating system to smooth fonts.
            this.PrintPreviewDialog1.UseAntiAlias = true;
        }
    }
}
