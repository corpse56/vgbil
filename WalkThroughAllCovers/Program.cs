using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

namespace WalkThroughAllCovers
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = AppSettings.IPAddressFileServer;
            string login = AppSettings.LoginFileServerRead;
            string pwd = AppSettings.PasswordFileServerRead;
            string _directoryPath = @"\\" + ip + @"\BookAddInf\BJVVV\";
            List<string> covers = new List<string>();
            using (new NetworkConnection(_directoryPath, new NetworkCredential(login, pwd)))
            {
                foreach (string file in Directory.EnumerateFiles(_directoryPath, "*.*", SearchOption.AllDirectories))
                {
                    if (file.ToLower().Contains("cover.jpg"))
                    {
                        Image img = Image.FromFile(file);
                        if (img.Width > img.Height)
                        {
                            //\\192.168.4.30\BookAddInf\BJVVV\000\008\668\JPEG_AB\cover.jpg
                            string pin = file.Substring(file.IndexOf("AddInf")+7);
                            pin = pin.Substring(pin.IndexOf("\\"));
                            pin = $"{pin[1]}{pin[2]}{pin[3]}{pin[5]}{pin[6]}{pin[7]}{pin[9]}{pin[10]}{pin[11]}";
                            pin = pin.TrimStart('0');
                            covers.Add(pin);
                            Console.WriteLine(pin);
                        }
                    }
                }
            }
            File.WriteAllLines("BJVVV.txt", covers);
            Console.WriteLine("finish");
            Console.ReadKey();
        }
    }
}
