using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace BookkeepingForOrder
{
    public class PrintBlank
    {
        private static PrintDocument pd;
        private Font printFont;
        private DbForEmployee db;
        private System.Windows.Forms.DataGridView dg;
        public PrintBlank(DbForEmployee db_,System.Windows.Forms.DataGridView dg_, string Dept)
        {
            this.db = db_;
            this.dg = dg_;
            pd = new PrintDocument();
            switch (Dept)
            {
                case "����� ������ ������������� - 2 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 2nd floor";
                        break;
                    }
                case "����� ������ ������������� - 3 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 3rd floor";
                        break;
                    }
                case "����� ������ ������������� - 4 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "����� ������ ������������� - ����� ���������":
                    {
                        //pd.PrinterSettings.PrinterName = "Zebra TLP2844 4th floor";
                        pd.PrinterSettings.PrinterName = XmlConnections.GetConnection("/Connections/FourthFloorNewPeriodica");
                        break;
                    }
                case "����� ������ ������������� - 5 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 5th floor";
                        break;
                    }
                case "����� ������ ������������� - 6 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 6th floor";
                        break;
                    }
                case "����� ������ ������������� - 7 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 7th floor";
                        break;
                    }
                case "����� ������ ������������� - 0 ����":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 zero floor";
                        break;
                    }
                case "����� ������ ������������� - ���������":
                    {
                        pd.PrinterSettings.PrinterName = "Zebra TLP2844 CDD";
                        break;
                    }
            }
            //pd.PrinterSettings.PrinterName = "Zebra TLP2844";
            //pd.PrinterSettings.PrinterName = XmlConnections.GetConnection("/Connections/Printer");//"Zebra TLP2844";
            this.printFont = new Font("Arial Unicode MS", 10f);
            //num = this.printFont.Height;
            //pd.PrinterSettings.PrinterName = "Zebra  TLP2844";
            //pd.PrinterSettings.PrinterName = "HP LaserJet M1522 MFP Series PCL 6";
            pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, 490);

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        }
        public void Print()
        {
            pd.Print();
        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            PaperSize size;
            Rectangle rectangle;
            StringFormat format;
            //string str = this.printString;
            string str = "���� ������������ ������: " + DateTime.Now.ToString("dd.MM.yyyy HH:MM");
            size = new PaperSize("bar", 314, 492);
            Size rectsize = new Size(314, 492);
            Font printFont = new Font("Arial Unicode MS", 10f);

            rectangle = new Rectangle(new Point(0, 0), new Size(314, 490));
            format = new StringFormat(StringFormatFlags.NoClip);// (0x4000);
            format.LineAlignment = StringAlignment.Far;
            format.Alignment = StringAlignment.Far;
            format.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;

            //format.LineAlignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.DirectionVertical | StringFormatFlags.DirectionRightToLeft;
            //format.Alignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.NoClip;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawLine(Pens.Black, new Point(280, 0), new Point(280, 490));
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("��������") ||
                dg.SelectedRows[0].Cells["dp"].Value.ToString().ToLower().Contains("����"))
            {
                str = "�����-����� ��� ������ ���������� ��\r\n ��������";
            }
            else
                if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("���������"))
                {
                    str = "�����-����� ��� ������ ���������� �\r\n ���-���������";
                }
                else
                {
                    str = "�����-����� ��� ������ ���������� ��\r\n ���������� ����������� � ������";
                }
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Center;
            //e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            //g.DrawString(this.printString, this.printFont, Brushes.Black, rectangle, format);
            //this.printString = "";

            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawLine(Pens.Black, new Point(280, 0), new Point(280, 490));
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            format.Alignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;
            rectangle = new Rectangle(new Point(260, 0), new Size(20, 490));
            str = "�����: " + dg.SelectedRows[0].Cells["dp"].Value.ToString();



            //e.Graphics.DrawRectangle(Pens.Black, rectangle);

            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("��������") ||
               dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("���������"))
            {
                printFont = new Font(printFont, FontStyle.Bold);
            }
            else

            {
                printFont = new Font("Arial Unicode MS", 10f);
            }
            str = "(" + dg.SelectedRows[0].Cells["fio"].Value.ToString() + ")";//db.GetReader(dg.SelectedRows[0].Cells["idr"].Value.ToString());            rectangle = new Rectangle(new Point(200, 0), new Size(30, 240));
            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("��������") ||
               dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("���������"))
            {
                printFont = new Font(printFont, FontStyle.Bold);
            }
            else
            {
                printFont = new Font("Arial Unicode MS", 10f);
            }
            rectangle = new Rectangle(new Point(240, 0), new Size(20, 490));
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            printFont = new Font("Arial Unicode MS", 10f);

            rectangle = new Rectangle(new Point(200, 0), new Size(30, 240));
            str = "����: " + dg.SelectedRows[0].Cells["shifr"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(200, 240), new Size(30, 250));
            str = "���. : " + dg.SelectedRows[0].Cells["inv"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(170, 0), new Size(30, 490));
            str = "����� : " + dg.SelectedRows[0].Cells["avt"].Value.ToString();
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(90, 0), new Size(80, 490));
            str = "�������� : " + dg.SelectedRows[0].Cells["zag"].Value.ToString();
            format.LineAlignment = StringAlignment.Near;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(60, 0), new Size(30, 240));
            str = "��� �������: " + dg.SelectedRows[0].Cells["izd"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(60, 240), new Size(30, 250));
            //str = "����: " + ((DataTable)dg.DataSource).Rows[0]["yaz"].ToString();// dg.SelectedRows[0].Cells["yaz"].Value.ToString();
            str = "����: " + dg.SelectedRows[0].Cells["yaz"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(20, 0), new Size(40, 490));
            str = "�������______________________________ ";
            format.LineAlignment = StringAlignment.Far;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(0, 0), new Size(40, 490));
            str = "*123459854*";//�������� ���
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.
            //e.Graphics.DrawRectangle(Pens.Red, rectangle);
            //e.Graphics.DrawString(str, new Font("C39HrP24DhTt", 36f), Brushes.Black, rectangle, format);

            //throw new Exception("The method or operation is not implemented.");
        }
    }
}
