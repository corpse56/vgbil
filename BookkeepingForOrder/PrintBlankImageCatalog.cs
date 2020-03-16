using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Readers.ReadersRights;
using System.Windows.Forms;
using LibflClassLibrary.Books;
using LibflClassLibrary.ImageCatalog;
using Utilities;
using LibflClassLibrary.BJUsers;

namespace BookkeepingForOrder
{
    public class PrintBlankImageCatalog
    {
        private static PrintDocument pd;
        private Font printFont;
        private int PaperSize = 1600;
        private ReaderInfo reader;
        private ICOrderInfo order;
        private BJUserInfo bjUser;
        public PrintBlankImageCatalog( BJUserInfo bjUser, Form1 f1, ReaderInfo reader, ICOrderInfo order)
        {
            this.order = order;
            this.reader = reader;
            this.bjUser = bjUser;
            pd = new PrintDocument();

            #region PrinterNaming
            switch (bjUser.SelectedUserStatus.DepName)
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
            string[] ddd = new string[PrinterSettings.InstalledPrinters.Count];
            PrinterSettings.InstalledPrinters.CopyTo(ddd, 0);
            //pd.PrinterSettings.PrinterName = @"Zebra TLP2844";
            
            pd.DefaultPageSettings.PaperSize = new PaperSize("rdr", 315, PaperSize);
            
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
        }
        public void Print()
        {
            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            paintOrder(sender, e, reader, order);
        }

        void paintOrder(object sender, PrintPageEventArgs e, ReaderInfo reader, ICOrderInfo order)
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
            rectangle = new Rectangle(50, CurrentY + 10, 315, 50);
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
            str = bjUser.SelectedUserStatus.DepName.Substring(bjUser.SelectedUserStatus.DepName.IndexOf("-") + 2);
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
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
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
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            e.Graphics.DrawImage(img, new PointF(15, CurrentY + 15));
            CurrentY += 500;
        }



    }
}
