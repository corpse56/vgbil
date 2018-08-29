using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using iTextSharp.text.pdf;
using ImageMagick;
using XMLConnections;
using System.Net;

namespace UpDgtPeriodic
{
    public partial class fPreviewPDF : Form
    {
        private int IDZ;
        private List<DirectoryInfo> NumberFolderList = new List<DirectoryInfo>();
        private string mTitle;
        private int Year;
        private List<Bitmap> lb;
        private int max_img = 0;
        bool PDF = true;
        private bool done = false;
        private string Login;
        private FileInfo fPDF;
        private string PIN = "";

        public fPreviewPDF(int IDZ_, string selectedYear, string fPDF_, string PIN_, string login_)
        {

            InitializeComponent();
            this.IDZ = IDZ_;
            this.fPDF = new FileInfo(fPDF_);
            this.PIN = PIN_;
            this.Login = login_;
            DBScanInfo db = new DBScanInfo();

            mTitle = db.GetTitleByIDZ(this.IDZ);
            label3.Text += mTitle;
            label4.Text += selectedYear + ". Выбранный PDF-файл: " +fPDF.Name;


            this.Year = int.Parse(selectedYear);


            ThreadPool.QueueUserWorkItem(new WaitCallback(EndlessProc));


            //получаем список номеров выбранного издания и года
            //NumberFolderList = GetNumberFolderList();

            //comboBox1.Items.AddRange(NumberFolderList.ToArray());
            //comboBox1.SelectedIndex = 0;
            /// <summary>
            /// Метод настраивающий просмотр первых 10 страниц выбранного номера
            /// </summary>
            /// <param name="msg"></param>            /// <exception cref="System.FileNotFoundException">Thrown when files in Temp Storage are not found</exception>
            //SetupViewing();

            SetupPreview();
            //this.Controls.Remove(rp);
            //fWait.Close();
            panel1.Focus();
            done = true;

        }

        private void SetupPreview()
        {
            lb = new List<Bitmap>();
            PdfReader reader = new PdfReader(fPDF.FullName);
            max_img = reader.NumberOfPages;
            max_img = (max_img < 10) ? max_img : 10;
            MagickReadSettings settings = new MagickReadSettings();
            settings.Density = new MagickGeometry(300, 300);//качество изображения
            settings.FrameIndex = 0; // Первая страница
            settings.FrameCount = 10; // Количество страниц. (10 значит 10, а не 11)


            using (MagickImageCollection images = new MagickImageCollection())
            {
                images.Read(fPDF.FullName, settings);
                for (int i = 0; i < max_img; i++)
                {
                    lb.Add(images[i].ToBitmap());
                }
            }
            pictureBox1.Image = lb[0];
            label1.Text = "Страница " + (current + 1).ToString() + " из " + max_img.ToString();

        }


        
        public void EndlessProc(Object objState)
        {
            fProgress fp = new fProgress("Загрузка программы");
            fp.Show();
            while (!done)
                Application.DoEvents();
            fp.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //List<Bitmap> lb = new List<Bitmap>();
            //PdfReader reader = new PdfReader(@"c:\kuzin.pdf");
            //int y = reader.NumberOfPages;
            //MagickReadSettings settings = new MagickReadSettings();
            //settings.Density = new MagickGeometry(100, 100);//качество изображения
            //settings.FrameIndex = 50; // Первая страница
            //settings.FrameCount = 10; // Количество страниц. (10 значит 10, а не 11)


            //using (MagickImageCollection images = new MagickImageCollection())
            //{
            //    images.Read(@"c:\kuzin.pdf", settings);

            //    lb.Add(images[1].ToBitmap());
            //    lb.Add(images[2].ToBitmap());
            //    lb.Add(images[3].ToBitmap());
            //    lb.Add(images[4].ToBitmap());
            //    lb.Add(images[5].ToBitmap());
            //    lb.Add(images[6].ToBitmap());
            //    lb.Add(images[7].ToBitmap());
            //    lb.Add(images[8].ToBitmap());
            //    lb.Add(images[9].ToBitmap());
            //    lb.Add(images[0].ToBitmap());
            //}
            //pictureBox1.Image = lb[6];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            done = true;
            this.Close();
        }
        int current = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            if (current == max_img - 1)
            {
                return;
            }
            current++;
            pictureBox1.Image = lb[current];
            label1.Text = "Страница " + (current + 1).ToString() + " из " + max_img.ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (current == 0)
            {
                return;
            }
            current--;
            pictureBox1.Image = lb[current];
            label1.Text = "Страница " + (current + 1).ToString() + " из " + max_img.ToString();
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }
        Point _mousePt = new Point();
        bool _tracking = false;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mousePt = e.Location;
                _tracking = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_tracking &&
               (pictureBox1.Image.Width > panel1.ClientSize.Width ||
               pictureBox1.Image.Height > panel1.ClientSize.Height))
            {
                panel1.AutoScrollPosition = new Point(-panel1.AutoScrollPosition.X + (_mousePt.X - e.X),
                 -panel1.AutoScrollPosition.Y + (_mousePt.Y - e.Y));
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _tracking = false;
            panel1.Focus();
        }
       
        fProgress fp;
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Продолжить?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            string store_ip = XmlConnections.GetConnection("/Connections/store_ip");
            string sTargetConnect = @"\\" + store_ip + @"\BookAddInf\PERIOD\";
            string sTarget = @"\\" + store_ip + @"\BookAddInf\PERIOD\";
            string sTargetConnectBookAddInf = @"\\" + store_ip + @"\BookAddInf";
            DBScanInfo db = new DBScanInfo();

            Package = "";

            int Number = 1;
            int Total = new PdfReader(fPDF.FullName).NumberOfPages;
            fp = new fProgress(Total);
            this.Enabled = false;
            fp.Show();
            Application.DoEvents();

            //преобразовать и скопиировать все изображения номера
            //int Total = new PdfReader(fPDF.FullName).NumberOfPages;
            //fp.ResetProgress(Total, Number, Total);
            try
            {
                CopyPDFToTarget(store_ip, sTarget, fPDF.DirectoryName, fPDF, Number, Total, fp, sTargetConnectBookAddInf);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message + ". Попробуйте еще раз!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fp.Close();
                this.Enabled = true;
                return;
            }



        }
        private void AddInfo()
        {
            DBScanInfo db = new DBScanInfo();
            PackageList.Add(Package);
            Package = "";
            string AllPackage = "";
            foreach (string s in PackageList)
            {
                AllPackage += s;
            }
            db.InsertUploadInfo( this.Year.ToString(), this.Login, AllPackage, this.IDZ, PDF,PIN);
            try
            {
                db.InsertPackage(PackageList, this.PIN,this.Year);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.BuildAndInsertHyperLink(this.PIN,this.Year.ToString());

            fp.Close();
            this.Enabled = true;
            MessageBox.Show("Привязка успешно завершена!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private string GetPath(string pin, string year)
        {
            //string idz = this.IDZ.ToString();
            //return @"PERIOD\" + ValidPath(GetFolderByIDZ(idz)) + @"\";
            string path = PINFormat(pin);
            path = path.Substring(0, 3) + @"\" + path.Substring(3, 3) + @"\" + path.Substring(6, 3) + @"\" + year + @"\";

            return path;

        }
        private string PINFormat(string pin)
        {
            switch (pin.Length)
            {
                case 1:
                    pin = "00000000" + pin;
                    break;
                case 2:
                    pin = "0000000" + pin;
                    break;
                case 3:
                    pin = "000000" + pin;
                    break;
                case 4:
                    pin = "00000" + pin;
                    break;
                case 5:
                    pin = "0000" + pin;
                    break;
                case 6:
                    pin = "000" + pin;
                    break;
                case 7:
                    pin = "00" + pin;
                    break;
                case 8:
                    pin = "0" + pin;
                    break;
            }
            return pin;
        }

        private string Package = "";
        private List<string> PackageList = new List<string>();
        private void CopyPDFToTarget(string ip, string sTarget, string sSource, FileInfo fPDF, int number, int total, fProgress fp,string sTargetConnectBookAddInf)
        {
            sTarget += GetPath(this.PIN, Year.ToString());
            sTarget += fPDF.Name.Remove(fPDF.Name.LastIndexOf(".")) + "\\JPEG_HQ";
            DirectoryInfo diTarget = new DirectoryInfo(sTarget);
            DirectoryInfo diSource = new DirectoryInfo(sSource);
            DirectoryInfo TargetFolder = new DirectoryInfo(sTarget);
            string outside_ip = @"\\" + XmlConnections.GetConnection("/Connections/outside_ip") + @"\Backup\BookAddInf\PERIOD\";
            string PIN = PINFormat(this.PIN);

            outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";
            DirectoryInfo TargetFolderOutside = new DirectoryInfo(outside_ip + @"\" + this.Year.ToString() + @"\" + fPDF.Name.Remove(fPDF.Name.LastIndexOf(".")));



            PdfReader reader = new PdfReader(fPDF.FullName);
            max_img = reader.NumberOfPages;
            if ((Package + fPDF.Name + ";").Length >= 3000)
            {
                PackageList.Add(Package);
                Package = "";
            }
            Package += fPDF.Name.Remove(fPDF.Name.LastIndexOf(".")) + ";";
            MagickReadSettings settings = new MagickReadSettings();
            using (new NetworkConnection(sTargetConnectBookAddInf, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
            {
                TargetFolder.Refresh();
            }

            if (!TargetFolder.Exists)
            {
                try
                {
                    TargetFolder.Create();
                }
                catch
                {
                    using (new NetworkConnection(sTargetConnectBookAddInf, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                    {
                        TargetFolder.Create();
                    }
                }
            }
            else
            {
                MessageBox.Show("Такой файл уже добавлялся! Выберите другой файл!");
                fp.Close();
                this.Enabled = true;
                return;
            }
            using (MagickImageCollection images = new MagickImageCollection())
            {

                for (int i = 0; i < max_img; i++)
                {
                    settings.Density = new MagickGeometry(300, 300);//качество изображения
                    settings.FrameIndex = i; // Первая страница
                    settings.FrameCount = 1; // Количество страниц. (10 значит 10, а не 11)
                    images.Read(fPDF.FullName, settings);
                    DirectoryInfo To = new DirectoryInfo(TargetFolder.FullName);
                    DirectoryInfo ToOutside = new DirectoryInfo(TargetFolderOutside.FullName);

                    if (!To.Exists)
                    {
                        To.Create();
                    }
                    string fileTo = To.FullName + "\\" + i.ToString() + ".jpg";
                    //string fileToOutside = ToOutside.FullName + "\\" + i.ToString() + ".jpg";
                    try
                    {
                        images[0].ToBitmap().Save(fileTo);
                    }
                    catch
                    {
                        using (new NetworkConnection(sTargetConnectBookAddInf, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                        {
                            images[0].ToBitmap().Save(fileTo);
                        }
                    }
                    /*try
                    {
                        images[0].ToBitmap().Save(fileToOutside);
                    }
                    catch
                    {
                        using (new NetworkConnection(outside_ip, new NetworkCredential(@"bj\CopyPeriodAddInf", "Period_Copy")))
                        {
                            images[0].ToBitmap().Save(fileToOutside);
                        }
                    }*/
                    //fp.IncProgress();
                    fp.IncProgress(total, i + 1, total);
                    Application.DoEvents();
                    GC.Collect();
                }
            }
            AddInfo();


                
        }
    }
}
