using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RastShifrRestored
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f1;
            try
            {
                f1 = new Form1();
            }
            catch
            {
                return;
            }
                Application.Run(f1);
        }
    }
}