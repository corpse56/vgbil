using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using Itenso.Rtf.Converter.Html;
using Itenso.Rtf.Support;
using Itenso.Rtf;
using CirculationACC;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.BJUsers;
using Utilities;

namespace CirculationApp
{
    public partial class EmailSending : Form
    {
        ReaderInfo reader;
        List<int> bold;
        string rn = System.Environment.NewLine;
        string Email = "";
        string htmltext;
        BJUserInfo bjUser;
        public EmailSending(ReaderInfo reader_, BJUserInfo bjUser_)
        {
            InitializeComponent();
            reader = reader_;
            bjUser = bjUser_;
            label1.Text = reader.FIO;
            int rownum = 0;
            bold = new List<int>();
            CirculationInfo ci = new CirculationInfo();
            DateTime? lastEmailDate = ci.GetLastEmailDate(reader);
            label2.Text = "Дата последней отправки письма: " + 
                          ((lastEmailDate == null) ? "Email не отправлялся" : lastEmailDate.Value.ToString("dd.MM.yyyy"));
            Email = reader.Email;
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email не существует или имеет неверный формат!");
                this.Load += (s, e) =>  this.Close();
            }
            richTextBox1.Text = "Уважаемый(ая) " + reader.Name + " " + reader.FatherName + "!" + rn +
                "Вы задерживаете книги:" + rn + rn;
            List<OrderInfo> orders = ci.GetOrders(reader.NumberReader);
            foreach (OrderInfo o in orders)
            {
                if (!o.StatusCode.In(CirculationStatuses.IssuedInHall.Id,
                                    CirculationStatuses.IssuedFromAnotherReserve.Id,
                                    CirculationStatuses.IssuedAtHome.Id))
                {
                    continue;
                }
                if (o.ReturnDate < DateTime.Today)
                {
                    rownum++;
                    BJBookInfo book = BJBookInfo.GetBookInfoByPIN(o.BookId);
                    string zag = book.AuthorTitle(); 
                    if (zag.Length > 51)
                        zag.Remove(50);
                    TimeSpan ts = DateTime.Now.AddDays(1) - o.ReturnDate;
                    richTextBox1.Text += rownum.ToString() + ". " + zag +
                        ", выдано: " + ((o.IssueDate.HasValue) ? o.IssueDate.Value.ToString("dd.MM.yyyy") : "не выдавалось") +
                        ", дата возврата: " + o.ReturnDate.ToString("dd.MM.yyyy");
                    bold.Add(richTextBox1.TextLength - 10);
                    richTextBox1.Text += ". Задержано на " + ts.Days.ToString() + " дней." + rn;
                }
            }
            if (rownum == 0)
            {
                MessageBox.Show("За читателем нет задоженностей!");
                this.Load += (s, e) => this.Close();
                return;
            }
            richTextBox1.Text += rn + "Просим Вас в ближайшее время вернуть литературу." + rn +
                "С уважением, " + rn +
                "Библиотека Иностранной Литературы," + rn +
                "тел. +7 (495) 915-36-41" + rn +
                "пн-пт - с 11:00 до 20:45." + rn +
                "субб - с 11:00 до 18:45";

            foreach (int i in bold)
            {
                richTextBox1.Select(i, 10);
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            }
            htmltext = EmailSending.ConvertRtfToHtml(richTextBox1.Rtf);

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
            button1.Enabled = false;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("no-reply@libfl.ru", "noreplayLIBFL");
            //client.Credentials = new NetworkCredential("lingua_automail@libfl.ru", "automail");
            MailAddress from = new MailAddress("noreply@libfl.ru", "Библиотека Иностранной Литературы", Encoding.UTF8);
            MailAddress to;
            MailMessage message = new MailMessage();
            message.From = from;
            message.IsBodyHtml = true;
            if (Email != "")
            {
                to = new MailAddress("debarkader@gmail.com");
                //to = new MailAddress(Email);
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
            CirculationInfo ci = new CirculationInfo();
            ci.InsertSentEmailAction(reader,bjUser);
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
