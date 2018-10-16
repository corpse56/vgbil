using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using Itenso.Rtf.Converter.Html;
using System.Net.Mail;
using System.Net;
using System.Drawing.Printing;
using System.IO;

namespace Circulation
{
    public partial class OverdueEmail : Form
    {
        Form1 f1;
        dbReader reader;
        List<int> bold;
        string mailtext;
        public bool canshow = false;
        string rn = System.Environment.NewLine;
        string Email = "";
        string htmltext;
        DataGridView formular;
        DBWork dbw;
        public OverdueEmail(Form1 f1_, dbReader reader_,DataGridView form_,DBWork dbw_)
        {
            InitializeComponent();
            formular = form_;
            f1 = f1_;
            reader = reader_;
            label1.Text = reader.Surname + " " + reader.Name + " " + reader.SecondName;
            int rownum = 0;
            bold = new List<int>();
            dbw = dbw_;



            Email = reader.GetEmail();

            if (Email == "")
            {
                MessageBox.Show("Email не существует или имеет неверный формат!");
                this.Close();
                return;
            }
            richTextBox1.Text = "Уважаемый(ая) " + reader.Name + " " + reader.SecondName + "!" + rn +
                "Вы задерживаете книги:" + rn + rn;
            foreach (DataGridViewRow r in formular.Rows)
            {
                if (r.DefaultCellStyle.ForeColor == Color.Red)
                {
                    rownum++;
                    string zag = r.Cells["zag"].Value.ToString();
                    if (zag.Length > 21)
                        zag.Remove(20);
                    TimeSpan ts = DateTime.Now.Date - (DateTime)r.Cells["dend"].Value;
                    richTextBox1.Text += rownum.ToString() + ". " + r.Cells["avt"].Value.ToString() +
                        ", " + zag +
                        ", выдано: " + ((DateTime)r.Cells["diss"].Value).ToString("dd.MM.yyyy") +
                        ", вернуть: ";
                    richTextBox1.Text += ((DateTime)r.Cells["dend"].Value).ToString("dd.MM.yyyy");
                    bold.Add(richTextBox1.TextLength - 10);
                    richTextBox1.Text += ". Задержано на " + ts.Days.ToString() + " дней." + rn;
                }
            }
            if (rownum == 0)
            {
                MessageBox.Show("За читателем нет задоженностей!");
                this.Close();
                return;
            }
            richTextBox1.Text += rn + "   Убедительно просим Вас в ближайшее время вернуть задерживаемую литературу." + rn +
                //"   Если через 90 дней со дня нарушения срока возврата документов книги не будут возвращены в фонд, " + rn +
                //"мы будем вынуждены исключить Вас из числа пользователей Библиотеки на 6 месяцев." + rn +
                "Заранее благодарим Вас." + rn +
                "Вход в библиотеку открыт:" + rn +
                "                                   понедельник - пятница с 11-00 до 20-30 час." + rn +
                "                                   суббота с 11-00 до 18-30 час." + rn +
                "                                   (в летние месяцы воскресенье - выходной)" + rn +
                "Контактный телефон: 8 (495) 915-37-02" + rn +
                "Внутренний телефон: 2-42" + rn +
                //((f1.DepName.ToLower().Contains("бонемент")) ?
                "Email: abonement@libfl.ru" + rn +//:
               // "Email: kafedra@libfl.ru" + rn) + 

                "Сайт: www.libfl.ru" + rn +
                "                                          Зал абонементного обслуживания" + rn +
                //"                                          абонементного обслуживания" + rn +
                //"                                          Акмаева И. А." + rn +
                "                                          " + DateTime.Now.ToString("dd.MM.yyyy");
            /*                "Вход в библиотеку открыт:" + rn +
                           "Вход в библиотеку открыт:" + rn +
                            "С уважением, " + rn +
                            "Кафедра единой выдачи - ВГБИЛ," + rn +
                            "(495) 915-36-69" + rn +
                            "пн-пт - 10.00-19.30" + rn +
                            "субб,вс - 10.00-17.30";*/

            foreach (int i in bold)
            {
                richTextBox1.Select(i, 10);
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            }
            htmltext = ConvertRtfToHtml(richTextBox1.Rtf);
            label2.Text += (dbw.GetLastEmailDate(reader) == "noemail") ?  " (нет)": " " +dbw.GetLastEmailDate(reader);
            this.canshow = true;
        }
        private static string ConvertRtfToHtml(string rtfText)
        {
            IRtfDocument rtfDocument = RtfInterpreterTool.BuildDoc(rtfText);
            RtfHtmlConvertSettings settings = new RtfHtmlConvertSettings();
            settings.ConvertScope = RtfHtmlConvertScope.Content;

            RtfHtmlConverter htmlConverter = new RtfHtmlConverter(rtfDocument, settings);
            return htmlConverter.Convert();
        } // ConvertRtfToHtml
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("no-reply@libfl.ru", "noreplayLIBFL");
            //client.Credentials = new NetworkCredential("lingua_automail@libfl.ru", "automail");
            MailAddress from = new MailAddress("no-reply@libfl.ru", "Библиотека Иностранной Литературы - Зал абонементного обслуживания", Encoding.UTF8);
            MailAddress to;
            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            if (Email != "")
            {
                to = new MailAddress(Email);
                message.To.Add(to);
            }

            message.Body = htmltext;
            
            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "ВГБИЛ";
            message.SubjectEncoding = Encoding.UTF8;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            message.Dispose();
            MessageBox.Show("Отправлено успешно!");
            dbw.InsertActionEMAIL(reader);
            Close();


            ////smtp сервер
            //string strSmtpHost = "smtp.gmail.com";
            ////smtp порт
            //int nSmtpPort = 587;
            ////логин
            //string strLogin = "#####@libfl.ru";
            ////пароль
            //string strPass = "#####";
            ////создаем подключение
            //SmtpClient smtpClient = new SmtpClient(strSmtpHost, nSmtpPort);
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.EnableSsl = true;
            //smtpClient.Credentials = new NetworkCredential(strLogin, strPass);
            ////От кого письмо
            //string strFrom = "#####@libfl.ru";
            ////Кому письмо
            //string strTo = "#####@###.##";
            ////Тема письма
            //string strSubject = "#####";
            ////Текст письма
            //string strBody = "#####";
            ////Создаем сообщение
            //MailMessage mailMessage = new MailMessage(strFrom, strTo, strSubject, strBody);
            ////Формат письма (если надо)
            //mailMessage.IsBodyHtml = true;
            ////Отправляем письмо
            //try
            //{
            //    smtpClient.Send(mailMessage);
            //}
            //catch (SmtpFailedRecipientsException ex)
            //{
            //    //throw new ApplicationException(ex.ToString());
            //}



        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument documentToPrint = new PrintDocument();
            printDialog1.Document = documentToPrint;
            //printDialog1.UseEXDialog = true;
            //DialogResult dr = printDialog.ShowDialog();
           // if (printDialog.ShowDialog() == DialogResult.OK)
            //{
                //StringReader reader = new StringReader(richTextBox1.Text);
                documentToPrint.PrintPage += new PrintPageEventHandler(documentToPrint_PrintPage);
                documentToPrint.Print();
            //}
            MessageBox.Show("Успешно отправлено на печать!");
        }

        void documentToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringReader reader = new StringReader(richTextBox1.Text);
            float LinesPerPage = 0;
            float YPosition = 0;
            int Count = 0;
            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;
            string Line = null;
            Font PrintFont = this.richTextBox1.Font;
            SolidBrush PrintBrush = new SolidBrush(Color.Black);

            LinesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

            while (Count < LinesPerPage && ((Line = reader.ReadLine()) != null))
            {
                YPosition = TopMargin + (Count * PrintFont.GetHeight(e.Graphics));
                e.Graphics.DrawString(Line, PrintFont, PrintBrush, LeftMargin, YPosition, new StringFormat());
                Count++;
            }

            if (Line != null)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
            PrintBrush.Dispose();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject dto = new DataObject();
            dto.SetText(richTextBox1.Rtf, TextDataFormat.Rtf);
            dto.SetText(richTextBox1.Text, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }

        private void bCopyToClipboard_Click(object sender, EventArgs e)
        {
            копироватьToolStripMenuItem_Click(sender, e);
            MessageBox.Show("Текст сообщения скопирован в буфер обмена!");
        }
    }
}
