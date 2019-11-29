using SipLibrary;
using SipLibrary.Abstract;
using SipLibrary.TextEncodings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SIPServer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

            IDispatcher disp_ = null;
            Controller controller_ = null;

            disp_ = new Dispatcher();

            //IPAddress ip = IPAddress.Any;
            //IPAddress ip = IPAddress.Parse("80.250.173.142"); //IPAddress.Any;
            //IPAddress ip = IPAddress.Parse("192.168.1.68"); //IPAddress.Any;
            //IPAddress ip = IPAddress.Parse("192.168.1.165"); //IPAddress.Any;
            //var ip = IPAddress.Loopback;

            Utilities.IniFile iniFile = new Utilities.IniFile("SipServerSettings.ini");
            var ip = IPAddress.Parse(iniFile.Read("ip", "SIPServer"));


            var endpoint = new IPEndPoint(ip, 6001);
            var encoding =  new AsciiUTF8Encoding();

            controller_ = new Controller(endpoint, disp_, encoding);
            
            while (true)
            {
                if (Console.ReadLine() ==  "quit")
                {
                    controller_.StopServer();
                    break;
                }
            }

        }
    }
}
