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
    public partial class fPreview : Form
    {
        private int IDZ;
        private List<DirectoryInfo> NumberFolderList = new List<DirectoryInfo>();
        private string mTitle;
        private string FolderYear;
        private int Year;
        private List<Bitmap> lb;
        private int max_img = 0;
        bool PDF = false;
        FileInfo fPDF;
        private bool done = false;
        private string Login;
        private string YearPath;
        private string PIN;
        public fPreview(int IDZ_, string FolderYear_,string path_,string selectedYear,string PIN_,string login_)
        {

            InitializeComponent();
            this.IDZ = IDZ_;
            //this.Login = Login_;
            this.FolderYear = FolderYear_;
            this.YearPath = path_;
            this.PIN = PIN_;
            this.Login = login_;
            DBScanInfo db = new DBScanInfo();

            mTitle = db.GetTitleByIDZ(this.IDZ);
            if (!db.IfExistsYearByIDZandFolderYear(this.IDZ, FolderYear))
            {
                throw new Exception("В базе не найден год, соответствующий названию выбранной папки!");
            }
            label3.Text += mTitle;
            label4.Text += FolderYear;


            //проверяем  есть ли такой год для выбранного издания.
            int year = 0;
            if (FolderYear.Length != 4)
            {
                MessageBox.Show("Неверный формат года: " + FolderYear);
                Environment.Exit(0);
            }
            if (!int.TryParse(FolderYear, out year))
            {
                MessageBox.Show("Неверный формат года: " + FolderYear);
                Environment.Exit(0);
            }
            this.Year = year;


            ThreadPool.QueueUserWorkItem(new WaitCallback(EndlessProc));


            //получаем список номеров выбранного издания и года
            NumberFolderList = GetNumberFolderList();

            if (NumberFolderList.Count == 0)
            {
                throw new Exception("Внутри папки с годом отсутствуют папки с номерами!");
            }


            comboBox1.Items.AddRange(NumberFolderList.ToArray());
            comboBox1.SelectedIndex = 0;
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
            FileInfo[] fi = ((DirectoryInfo)comboBox1.SelectedItem).GetFiles();
            if (fi.Length == 0)
            {
                throw new FileNotFoundException("В папке временного хранения для данного года и заглавия отсутствуют файлы!");
            }
            lb = new List<Bitmap>();

            if (fi.Length >= 10)
            {
                max_img = 10;
            }
            else
            {
                max_img = fi.Length;
            }
            for (int i = 0; i < max_img; i++)
            {
                lb.Add((Bitmap)Bitmap.FromFile(fi[i].FullName));
            }
            pictureBox1.Image = lb[0];
            label1.Text = "Страница " + (current + 1).ToString() + " из " + max_img.ToString();

        }

        private List<DirectoryInfo> GetNumberFolderList()
        {
            DirectoryInfo tmp;

            tmp = new DirectoryInfo(YearPath);
            try
            {
                tmp = new DirectoryInfo(YearPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tmp.GetDirectories().ToList();

        }

        //    try
        //    {
        //        NumberFolderList = GetNumberFolderList(this.IDZ);
        //    }
        //    catch (Exception ex)
        //    {
        //        done = true;
        //        DialogResult dr = MessageBox.Show("Произошла ошибка: " + ex.Message + "\". Попробовать еше раз?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
        //        if (dr == DialogResult.Yes)
        //        {
        //            done = false;
        //            ThreadPool.QueueUserWorkItem(new WaitCallback(EndlessProc));
        //            GetNumberFolderList();
        //        }
        //        else
        //        {
        //            Environment.Exit(0);
        //        }
        //    }
        //}

        //private fSelectExistsTitle ChooseFromTmpStg()
        //{
        //    fSelectExistsTitle fSET = new fSelectExistsTitle();
        //    //выбираем существующее издание во временном хранилище
        //    try
        //    {
        //        fSET = new fSelectExistsTitle(mTitle, mYear);
        //        fSET.StartPosition = FormStartPosition.CenterScreen;
        //        fSET.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        DialogResult dr = MessageBox.Show("Произошла ошибка: " + ex.Message + "\". Попробовать еше раз?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
        //        if (dr == DialogResult.Yes)
        //        {
        //            fSET = ChooseFromTmpStg();
        //        }
        //        else
        //        {
        //            Environment.Exit(0);
        //        }
        //    }
        //    return fSET;
        //}
        public void EndlessProc(Object objState)
        {
            fProgress fp = new fProgress("Загрузка программы");
            fp.Show();
            while (!done)
                Application.DoEvents();
            fp.Close();
        }
        public void SetupViewing()
        {
            FileInfo[] fi = ((DirectoryInfo)comboBox1.SelectedItem).GetFiles();
            if (fi.Length == 0)
            {
                throw new FileNotFoundException("В папке временного хранения для данного года и заглавия отсутствуют файлы!");
            }
            PDF = false;
            fPDF = fi[0];
            int PDFFileCount = 0;
            foreach (FileInfo f in fi)
            {
                if (f.Extension.Contains("pdf"))
                {
                    PDF = true;
                    fPDF = f;
                    PDFFileCount++;
                }
            }
            if (PDFFileCount > 1)
            {
                throw new Exception("Во временном хранении в указанном заглавии и годе более одного ПДФ-файла!");
            }
            if (PDF)//ЕСЛИ В ПАПКЕ ПДФ
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
            else // ЕСЛИ В ПАПКЕ КАРТИНКИ
            {
                lb = new List<Bitmap>();

                if (fi.Length >= 10)
                {
                    max_img = 10;
                }
                else
                {
                    max_img = fi.Length;
                }
                for (int i = 0; i < max_img; i++)
                {
                    lb.Add((Bitmap)Bitmap.FromFile(fi[i].FullName));
                }
                pictureBox1.Image = lb[0];
                label1.Text = "Страница " + (current + 1).ToString() + " из " + max_img.ToString();
            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //здесь обработать исключения
            //SetupViewing();
            SetupPreview();
            panel1.Focus();
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
        private string Set = "";
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
            DBScanInfo db = new DBScanInfo();
            string PIN = this.PIN;
            PIN = PINFormat(PIN);
            string ExactPath = GetPath(PIN, FolderYear);
            sTarget += ExactPath;//PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";

            DirectoryInfo diSource = new DirectoryInfo(YearPath);
            Package = "";
            DirectoryInfo[] diNumbers = diSource.GetDirectories();
            if (diNumbers.Length == 0)
            {
                throw new Exception("В источнике нет папок: " + diSource.ToString());
            }
            bool FirstPass = false;
            int Number = 1;
            int Total = diNumbers.Length;
            fp = new fProgress("Подождите...");
            this.Enabled = false;
            fp.Show();
            Application.DoEvents();

            foreach (DirectoryInfo d in diNumbers)
            {
                Set = d.Name + "; ";
                FileInfo[] fi = d.GetFiles();
                foreach (FileInfo f in fi)
                {
                    //скопиировать все изображения номера
                    //fp.ResetProgress(d.GetFiles("*.jpg").Length, Number, Total);
                    fp.IncProgress(Total, Number, Total);
                    Application.DoEvents();
                    //try
                    //{
                        bool result = CopyJPGToTarget(store_ip, sTarget, YearPath, f.Directory, FirstPass, Number, Total, fp,sTargetConnect);
                    //}
                    //catch (Exception ex)
                    if (!result)
                    {
                        //MessageBox.Show("Произошла ошибка: " + ex.Message + ". Попробуйте еще раз!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        fp.Close();
                        this.Enabled = true;
                        return;
                    }
                    FirstPass = true;
                    Number++;
                    break;
                }
            }

            #region _info.txt
            FileInfo[] _infotxtsearch = diSource.GetFiles();
            FileInfo _infotxt = null;
            foreach (FileInfo f in _infotxtsearch)
            {
                if (f.Name == "_info.txt")
                {
                    _infotxt = f;
                }
            }
            if (_infotxt != null)
            {
                DirectoryInfo d = new DirectoryInfo(sTarget + this.Year + @"\");
                try
                {
                    _infotxt.CopyTo(sTarget + this.Year + @"\_info.txt",true);
                }
                catch
                {
                    using (new NetworkConnection(sTargetConnect, new NetworkCredential("libfl\\DigitCentreWork01", "DigCW_01")))
                    {
                        _infotxt.CopyTo(sTarget + this.Year + @"\_info.txt", true);
                    }
                }

                string outside_ip = @"\\" + XmlConnections.GetConnection("/Connections/outside_ip") + @"\Backup\BookAddInf\PERIOD\";
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";
                try
                {
                    _infotxt.CopyTo(outside_ip + this.Year.ToString() + @"\_info.txt", true);
                }
                catch
                {
                    using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                    {
                        _infotxt.CopyTo(outside_ip + this.Year.ToString() + @"\_info.txt", true);
                    }
                }
            }
            #endregion
            AddInfo();

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
            db.InsertUploadInfo( FolderYear, this.Login, AllPackage, this.IDZ, PDF,PIN);
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


        private string Package = "";
        private List<string> PackageList = new List<string>();
        private bool CopyJPGToTarget(string ip, string sTarget, string sSource, DirectoryInfo SF, bool FirstPass, int number, int total, fProgress fp,string sTargetConnect)
        {
            DirectoryInfo diTarget = new DirectoryInfo(sTarget);
            DirectoryInfo diSource = new DirectoryInfo(sSource);
            char[] invalid_chars = System.IO.Path.GetInvalidFileNameChars();

            //string outside_ip = @"\\" + XmlConnections.GetConnection("/Connections/outside_ip") + @"\Backup\BookAddInf\PERIOD\";
            //string outsideIPConnect = @"\\" + XmlConnections.GetConnection("/Connections/outside_ip") + @"\Backup";
            string PIN = PINFormat(this.PIN);

            //outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";
            sTarget += SF.Name;//имя папки = название номер
            sTarget += "\\JPEG_HQ";
            DirectoryInfo TargetFolderOutside = new DirectoryInfo(sTarget);
            DirectoryInfo TargetFolder = new DirectoryInfo(sTarget);

            //string sTargetConnect = @"\\192.168.4.30\BookAddInf\";
            //MessageBox.Show(TargetFolder.FullName);
            using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
            {
                TargetFolder.Refresh();
            }
            //MessageBox.Show("11");
            if (!TargetFolder.Exists)
            {
                try
                {
                    TargetFolder.Create();
                }
                catch
                {
                    using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                    {
                        TargetFolder.Create();
                    }
                }
            }
            else
            {
                if (!FirstPass)
                {
                    try
                    {
                        TargetFolder.Delete(true);
                        TargetFolder.Create();
                    }
                    catch
                    {

                        using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                        {
                            TargetFolder.Delete(true);
                            TargetFolder.Create();
                        }
                    }
                }
            }

            
            DirectoryInfo[] di = diSource.GetDirectories();
            max_img = di.Length;



            FileInfo[] fi = SF.GetFiles();
            DirectoryInfo To = new DirectoryInfo(TargetFolder.FullName);
            using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
            {
                To.Refresh();
            }
           /* using (new NetworkConnection(outsideIPConnect, new NetworkCredential(@"bj\CopyPeriodAddInf", "Period_Copy")))
            {
                ToOutside.Refresh();
            }*/
            

            if (!To.Exists)
            {
                try
                {
                    To.Create();
                }
                catch
                {
                    using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                    {
                        To.Create();
                    }
                }
            }
            /*if (!ToOutside.Exists)
            {
                try
                {
                    ToOutside.Create();
                }
                catch
                {
                    using (new NetworkConnection(outsideIPConnect, new NetworkCredential(@"bj\CopyPeriodAddInf", "Period_Copy")))
                    {
                        ToOutside.Create();
                    }
                }
            }*/
            if ((Package + To.Name + ";").Length >= 3000)
            {
                PackageList.Add(Package);
                Package = "";
            }
            Package += To.Name + ";";

            foreach (FileInfo f in fi)
            {
                try
                {
                    f.CopyTo(To + "\\" + f.Name);
                }
                catch
                {
                    using (new NetworkConnection(sTargetConnect, new NetworkCredential(@"bj\DigitCentreWork01", "DigCW_01")))
                    {
                        f.CopyTo(To + "\\" + f.Name);
                    }
                }
                //fp.IncProgress();
                Application.DoEvents();
            }
            /*foreach (FileInfo f in fi)
            {
                try
                {
                    f.CopyTo(ToOutside + "\\" + f.Name);
                }
                catch
                {
                    using (new NetworkConnection(outsideIPConnect, new NetworkCredential(@"bj\CopyPeriodAddInf", "Period_Copy")))
                    {
                        f.CopyTo(ToOutside + "\\" + f.Name);
                    }
                }
                //fp.IncProgress();
                Application.DoEvents();
            }*/
            return true;
        }
    }
}
