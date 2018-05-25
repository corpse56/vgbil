using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace Utilities
{
    public class Log : IDisposable
    {
        TextWriter _tw;
        public Log()
        {
            _tw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"_log.txt", true);
        }

        public void WriteLog(string record)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}    {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), record);
            _tw.WriteLine(sb.ToString());
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
