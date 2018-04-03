using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace RastShifrRestored
{
    public partial class Form4 : Form
    {
        string printString;
        public Form4(string prints)
        {
            InitializeComponent();
            printString = prints;
        }

        private void Form4_Paint(object sender, PaintEventArgs e)
        {
            PaperSize size;
            Rectangle rectangle;
            int num;
            string str;
            StringFormat format;
            num = 0;
            str = this.printString;
            List<int> Semicolon = new List<int>();

            for (int i = 0; i < printString.Length; i++)
            {
                if (str.IndexOf(';', i, 1) != -1)
                {
                    Semicolon.Add(i);
                    num++;
                }
            }
            if (num > 2)
            {
                MessageBox.Show("Расстановочный шифр имеет неправильный формат!");
                return;
            }
            if (num == 2)
            {
                this.printString = this.printString.Remove(Semicolon[0], Semicolon[1] - Semicolon[0]);
            }
            if (num > 0)
            {
                this.printString = this.printString.Insert(Semicolon[0] + 1, Environment.NewLine);
            }
            /*if (this.printString.Length > 9)
            {
                this.printString = this.printString.Insert(8, " ");
            }*/
            /*if (this.printString.IndexOf(";") >= 0)
            {
                this.printString = this.printString.Remove(this.printString.IndexOf(";") + 1, 1);                
            }*/
            Font printFont;
 
            printFont = new Font("Arial", 19f);

            size = new PaperSize("bar", 0x9d, 0x2f);
            rectangle = new Rectangle(new Point(0,0), new Size(0xad, 0x37));
            format = new StringFormat();// (0x4000);
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;
            format.FormatFlags = StringFormatFlags.NoClip;
            e.Graphics.DrawRectangle(Pens.White, rectangle);
            e.Graphics.DrawString(this.printString, printFont, Brushes.Black, rectangle, format);
            //this.printString = "";
            //return;
        }
    }
}
