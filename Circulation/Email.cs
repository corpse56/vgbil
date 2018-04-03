using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf;
using Itenso.Rtf.Support;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Drawing.Printing;

namespace Circulation
{
    public partial class SendEmail : Form
    {
        Form1 f1;
        public bool canshow = false;
        string Email;
        DBWork.dbReader reader;
        string rn = System.Environment.NewLine;
        List<int> bold;
        string htmltext;
        public SendEmail(Form1 f1,DataGridView d,string num)
        {
            InitializeComponent();
            this.f1 = f1;
            reader = new DBWork.dbReader(int.Parse(num));
            label1.Text = reader.Surname + " " + reader.Name + " " +reader.SecondName;
            label2.Text = "Дата последней отправки письма: " + f1.dbw.GetLastEmailDate(num);
            if (d.Rows.Count == 0)
            {
                MessageBox.Show("За читателем не числится долгов!");
                canshow = false;
                return;
            }
            rtb.Text = "";
            Email = reader.GetEmail();

            if (Email == "")
            {
                MessageBox.Show("Email не существует или имеет неверный формат!");
                canshow = false;
                return;
            }
            canshow = true;
            GetTextBoxText(reader,d);
            //richTextBox1.Select(10, 10);
            //richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            foreach (int i in bold)
            {
                rtb.Select(i, 10);
                rtb.SelectionFont = new Font(rtb.Font, FontStyle.Bold);
            }
            htmltext = SendEmail.ConvertRtfToHtml(rtb.Rtf);

        }

        private void GetTextBoxText(DBWork.dbReader reader, DataGridView d)
        {
            rtb.Text = "Уважаемый(ая) " + reader.Name + " " + reader.SecondName + "!" + rn +
                "Вы задерживаете книги:" + rn + rn;
            getEmailText(d);
            rtb.Text += rn + "   Убедительно просим Вас в ближайшее время вернуть задерживаемую литературу." + rn +
                            //"   Если через 90 дней со дня нарушения срока возврата документов книги не будут возвращены в фонд, " + rn +
                            //"мы будем вынуждены исключить Вас из числа пользователей Библиотеки на 6 месяцев." + rn +
                            "Заранее благодарим Вас." + rn +
                            "Вход в библиотеку открыт:" + rn +
                            "                                   понедельник - пятница с 11-00 до 20-30 час." + rn +
                            "                                   суббота с 11-00 до 18-30 час." + rn +
                            "                                   воскресенье - выходной" + rn +
                            "Контактный телефон: 8 (495) 915-36-69" + rn +
                            "Внутренний телефон: 2-42" + rn +
                            "Email: lingua_automail@libfl.ru" + rn +
                            "Сайт: www.libfl.ru" + rn +
                            //"                                                  Заведующая залом" + rn +
                            "                                                  Зал абонементного обслуживания," + rn +
                            //"                                                  Акмаева И. А." + rn +
                            "                                                  " + DateTime.Now.ToString("dd.MM.yyyy");
            /*rtb.Text += rn + "Просим Вас в ближайшее время вернуть литературу в зал языкознания Библиотеки иностранной литературы." + rn + rn+
                "С уважением, " + rn +
                "Зал языкознания - Библиотека иностранной литературы," + rn +
                "(495) 915-36-69" + rn +
                "пн-пт - 10.00-19.30" + rn +
                "субб - 10.00-17.30"+ rn +
                "Письмо составлено автоматически. На него не нужно отвечать.";*/
        }

        private void getEmailText(DataGridView d)
        {
            int rownum = 0;
            bold = new List<int>();
            foreach (DataGridViewRow r in d.Rows)
            {
                if (((bool)r.Cells[12].Value == true) && (r.Cells[11].Value.ToString() == ""))
                {
                    rownum++;
                    string zag = r.Cells[1].Value.ToString();
                    if (zag.Length > 21)
                        zag.Remove(20);
                    TimeSpan ts = DateTime.Now.AddDays(1) - (DateTime)r.Cells[10].Value;
                    rtb.Text += rownum.ToString() + ". " + r.Cells[3].Value.ToString() +
                        ", " + zag +
                        ", ";
                    rtb.Text += ((DateTime)r.Cells[10].Value).ToString("dd.MM.yyyy");
                    bold.Add(rtb.TextLength - 10);
                    rtb.Text += ". Задержано на ";
                    rtb.Text += ts.Days.ToString();
                    rtb.Text += " дней." + rn;
                }
            }
            if (rownum == 0)
            {
                MessageBox.Show("За читателем нет задоженностей!");
                rtb.Text = "";
                canshow = false;
            }
        }

        // ----------------------------------------------------------------------
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
            button1.Enabled = false;
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
            f1.dbw.InsertActionEmail(reader);
            label2.Text = "Дата последней отправки письма: " + f1.dbw.GetLastEmailDate(reader.id);
        }

        private void bCopy_Click(object sender, EventArgs e)
        {
            DataObject dto = new DataObject();
            dto.SetText(rtb.Rtf, TextDataFormat.Rtf);
            dto.SetText(rtb.Text, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }

        private void bPrint_Click(object sender, EventArgs e)
        {
            //PrintDialog printDialog = new PrintDialog();
            PrintDocument documentToPrint = new PrintDocument();
            //printDialog.Document = documentToPrint;
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
            StringReader reader = new StringReader(rtb.Text);
            float LinesPerPage = 0;
            float YPosition = 0;
            int Count = 0;
            float LeftMargin = e.MarginBounds.Left;
            float TopMargin = e.MarginBounds.Top;
            string Line = null;
            Font PrintFont = this.rtb.Font;
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

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bCopy_Click(sender, e);
        }
    }
}
