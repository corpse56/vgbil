using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
namespace Circulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main()
        {
            /*if (IsTheOnlyInstance())
            {
                MessageBox.Show("Программа уже запущена!");
                //Application.Exit();
                return;
            }*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static bool IsTheOnlyInstance()
        {

            Process pThis = Process.GetCurrentProcess();

            Process[] ps = Process.GetProcessesByName(pThis.ProcessName);

            bool flag = false;

            foreach (Process p in ps)
            {
                if (p.StartInfo.FileName == pThis.StartInfo.FileName)
                {
                    if (p.Id == pThis.Id)
                    {
                        continue;
                    }
                    else
                    {
                        if ((p.StartInfo.FileName == pThis.StartInfo.FileName) && (p.Id != pThis.Id))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }
    }
}