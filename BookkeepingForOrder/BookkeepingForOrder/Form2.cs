using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace BookkeepingForOrder
{
    public partial class Form2 : Form
    {
        DataGridView dg;
        public int i = 0;
        public Form2(DataGridView dg_)
        {
            dg = dg_;
            InitializeComponent();
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            PaperSize size;
            Rectangle rectangle;
            StringFormat format;
            //string str = this.printString;
            string str = "Дата формирования заказа: " + DateTime.Now.ToString("dd.MM.yyyy HH:MM");
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
            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("Выставка"))
            {
                str = "БЛАНК-ЗАКАЗ ДЛЯ ВЫДАЧИ ЛИТЕРАТУРЫ НА\r\n ВЫСТАВКУ";
            }
            else
            {
                str = "БЛАНК-ЗАКАЗ ДЛЯ ВЫДАЧИ ЛИТЕРАТУРЫ НА\r\n ДЛИТЕЛЬНОЕ ПОЛЬЗОВАНИЕ В ОТДЕЛЫ";
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
            str = "Отдел: " + dg.SelectedRows[0].Cells["dp"].Value.ToString();



            //e.Graphics.DrawRectangle(Pens.Black, rectangle);

            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("Выставка"))
            {
                printFont = new Font(printFont, FontStyle.Bold);
            }
            else
            {
                printFont = new Font("Arial Unicode MS", 10f);
            }
            str = "(" + dg.SelectedRows[0].Cells["fio"].Value.ToString() + ")";//db.GetReader(dg.SelectedRows[0].Cells["idr"].Value.ToString());            rectangle = new Rectangle(new Point(200, 0), new Size(30, 240));
            if (dg.SelectedRows[0].Cells["fio"].Value.ToString().Contains("Выставка"))
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
            str = "Шифр: " + dg.SelectedRows[0].Cells["shifr"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(200, 240), new Size(30, 250));
            str = "Инв. : " + dg.SelectedRows[0].Cells["inv"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(170, 0), new Size(30, 490));
            str = "Автор : " + dg.SelectedRows[0].Cells["avt"].Value.ToString();
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(90, 0), new Size(80, 490));
            str = "Заглавие : " + dg.SelectedRows[0].Cells["zag"].Value.ToString();
            format.LineAlignment = StringAlignment.Near;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(60, 0), new Size(30, 240));
            str = "Год издания: " + dg.SelectedRows[0].Cells["izd"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(60, 240), new Size(30, 250));
            //str = "Язык: " + ((DataTable)dg.DataSource).Rows[0]["yaz"].ToString();// dg.SelectedRows[0].Cells["yaz"].Value.ToString();
            str = "Язык: " + dg.SelectedRows[0].Cells["yaz"].Value.ToString();
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(20, 0), new Size(40, 490));
            str = "Получил______________________________ ";
            format.LineAlignment = StringAlignment.Far;
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(new Point(0, 0), new Size(40, 490));
            str = "*123459854*";//вставить год
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            //format.FormatFlags = StringFormatFlags.
            //e.Graphics.DrawRectangle(Pens.Red, rectangle);
            //e.Graphics.DrawString(str, new Font("C39HrP24DhTt", 36f), Brushes.Black, rectangle, format);

            //throw new Exception("The method or operation is not implemented.");

        }
    }
}
