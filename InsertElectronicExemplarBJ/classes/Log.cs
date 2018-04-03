using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InsertElectronicExemplarBJ.classes
{
    class Log : IDisposable
    {
        TextWriter _tw;
        public Log()
        {
            _tw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"_log.txt", true);
        }

        public void WriteLog(string record)
        {
            _tw.WriteLine(record);
        }
        

    
        #region Члены IDisposable

        public void  Dispose()
        {
            _tw.Flush();
            _tw.Close();
            _tw.Dispose();
        }

        #endregion
}
}
