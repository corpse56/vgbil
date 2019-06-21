using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.SipServer
{
    public class SipServerHandler
    {
        public ReaderInfo GetPatron(string patronId)
        {
            ReaderInfo reader;
            try
            {
                if (patronId[0] == 'R')
                {
                    reader = ReaderInfo.GetReaderByBar(patronId);
                }
                else
                {
                    reader = ReaderInfo.GetReaderByUID(patronId);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //response.Ok = false;
                return null;
            }
            return reader;
        }

    }
}
