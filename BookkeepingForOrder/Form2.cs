using LibflClassLibrary.BJUsers;
using LibflClassLibrary.ImageCatalog;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
using Zen.Barcode;

namespace BookkeepingForOrder
{
    public partial class Form2 : Form
    {
        private Form1 F1;

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Form1 f1)
        {
            this.F1 = f1;
            InitializeComponent();            
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            this.Invalidate();
            //this.Update();
            //this.Refresh();
        }


        void paintOrder(object sender, PaintEventArgs e, ReaderInfo reader, ICOrderInfo order)
        {

            Rectangle rectangle;
            StringFormat format;
            Font printFont = new Font("Arial Unicode MS", 11f, FontStyle.Bold);
            format = new StringFormat(StringFormatFlags.NoClip)
            {
                LineAlignment = StringAlignment.Near,
                Alignment = StringAlignment.Near
            };

            ImageCatalogCirculationManager ci = new ImageCatalogCirculationManager();

            string str = "Билет № " + reader.NumberReader;
            int CurrentY = 0;

            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 70);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            //BarcodeDraw bdraw = BarcodeDrawFactory.GetSymbology(BarcodeSymbology.Code39C);
            //Image barcodeImage = bdraw.Draw(order.GetBarString(), 40);
            //e.Graphics.DrawImage(barcodeImage, 20, CurrentY+5);
            str = "Билет № " + reader.NumberReader;
            Font barFont = new Font("C39HrP24DhTt", 40f, FontStyle.Regular);
            rectangle = new Rectangle(50, CurrentY+10, 315, 50);
            e.Graphics.DrawString(order.GetBarString(), barFont, Brushes.Black, rectangle, format);
            CurrentY += 70;

            rectangle = new Rectangle(0, CurrentY, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);

            str = "ЧЗ\nдо:";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(70, CurrentY, 245, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.AddDays(4).ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            rectangle = new Rectangle(0, CurrentY, 70, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 10f);
            str = BJUserInfo.GetAdmin().SelectedUserStatus.DepName.Substring(BJUserInfo.GetAdmin().SelectedUserStatus.DepName.IndexOf("-") + 2);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);

            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 13f);
            str = "Билет № " + reader.NumberReader;
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(70, CurrentY, 245, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 10f);
            str = "Фамилия: " + reader.FIOShort;
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 100);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            printFont = new Font("Arial Unicode MS", 11f);
            e.Graphics.DrawString(/*abonement*/ reader.Rights.ToString(), printFont, Brushes.Black, rectangle, format);
            CurrentY += 100;


            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = order.Card.CardType.In(CardType.AV, CardType.PERIODICAL) ? "Страна: " + order.Card.LanguageName
                                                                            : "Язык: " + order.Card.LanguageName;
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.Date.ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 75);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = $"Комментарий читателя: {order.Comment}";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 75;

            rectangle = new Rectangle(0, CurrentY, 315, 75);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);


            //========вторая часть требования
            str = "Билет № " + reader.NumberReader;
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
            printFont = new Font("Arial Unicode MS", 10f);
            str = "Фамилия: " + reader.FIOShort;
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "ЧЗ";
            printFont = new Font("Arial Unicode MS", 11f);
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            printFont = new Font("Arial Unicode MS", 10f);
            rectangle = new Rectangle(0, CurrentY, 315, 50);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = DateTime.Now.Date.ToString("dd.MM.yyyy");
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 50;

            printFont = new Font("Arial Unicode MS", 10f);
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Главная карточка:";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 500);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            Image img = order.Card.MainSideImage;
            img.RotateFlip(RotateFlipType.Rotate90FlipX);
            e.Graphics.DrawImage(img, new PointF(15, CurrentY + 15));
            //e.Graphics.DrawString($"{img.Width},{img.Height}", printFont, Brushes.Black, rectangle, format);
            CurrentY += 500;

            printFont = new Font("Arial Unicode MS", 10f);
            rectangle = new Rectangle(0, CurrentY, 315, 25);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            str = "Выбранная карточка:";
            e.Graphics.DrawString(str, printFont, Brushes.Black, rectangle, format);
            CurrentY += 25;

            rectangle = new Rectangle(0, CurrentY, 315, 500);
            e.Graphics.DrawRectangle(Pens.Black, rectangle);
            img = order.SelectedSideImage;
            img.RotateFlip(RotateFlipType.Rotate90FlipX);
            e.Graphics.DrawImage(img, new PointF(15, CurrentY + 15));
            CurrentY += 500;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            paintOrder(sender, e, ReaderInfo.GetReader(189245), ICOrderInfo.GetICOrderById(1, true));
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pictureBox1.BackgroundImage.Save(@"e:\order.png", ImageFormat.Png);
            Bitmap bmp = new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
            pictureBox1.DrawToBitmap(bmp, pictureBox1.ClientRectangle);
            bmp.Save(@"e:\order.png", ImageFormat.Png);

        }
    }
}
