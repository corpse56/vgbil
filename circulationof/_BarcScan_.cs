using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OposScanner_CCO;
using Test1;
using System.Windows.Forms;
using OPOSCONSTANTSLib;

namespace Circulation
{
    public class _BarcScan
    {
        public OPOSScannerClass Scanner;
        //public event EventHandler Scanned;
        public Form1 F1;
        public _BarcScan(Form1 _F1)
        {

            F1 = _F1;
            try
            {
                this.Scanner = new OPOSScannerClass();
                this.Scanner.ErrorEvent += new _IOPOSScannerEvents_ErrorEventEventHandler(Scanner_ErrorEvent);
                this.Scanner.DataEvent += new _IOPOSScannerEvents_DataEventEventHandler(Scanner_DataEvent);
                this.Scanner.Open("STI_SCANNER");
                //MessageBox.Show("1");
                ResultCodeH.Check(this.Scanner.ClaimDevice(7000));
                //MessageBox.Show("2");
                this.Scanner.DeviceEnabled = true;
                ResultCodeH.Check(this.Scanner.ResultCode);
                //MessageBox.Show("3");
                this.Scanner.AutoDisable = true;

                ResultCodeH.Check(this.Scanner.ResultCode);
                //MessageBox.Show("4");
                this.Scanner.DataEventEnabled = true;
                ResultCodeH.Check(this.Scanner.ResultCode);
                //MessageBox.Show("5");
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message.ToString());
            }
        }
        void Scanner_DataEvent(int Status)
        {
            this.Scanner.DeviceEnabled = true;
            this.Scanner.DataEventEnabled = true;
            F1.FireScan(this.Scanner, EventArgs.Empty);
        }

        void Scanner_ErrorEvent(int ResultCode, int ResultCodeExtended, int ErrorLocus, ref int pErrorResponse)
        {
            pErrorResponse = (int)OPOS_Constants.OPOS_ER_CLEAR;
            MessageBox.Show(ResultCodeH.Message(ResultCode));
            this.Scanner.DeviceEnabled = true;
            this.Scanner.DataEventEnabled = true;
        }
    }
}
