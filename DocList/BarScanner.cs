using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test1;
using System.Windows.Forms;

namespace DocList
{
    public class BarScanner
    {
        //private OPOSScannerClass Scanner;
        private Form1 F1;
        public BarScanner(Form1 _F1)
        {

            F1 = _F1;
            try
            {
               
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message.ToString());
            }
        }
        void Scanner_DataEvent(int Status)
        {
            
        }

        public static bool CheckScanData(string p)
        {
            if (p[0] != 'U')
                return false;
            if (p.Length != 10)
                return false;
            int i;
            if (!int.TryParse(p.Remove(0, 1), out i))
                return false;
            return true;

        }

        void Scanner_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
        {
           
        }
    }
}
