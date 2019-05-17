using SipLibrary.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIPServer
{
    class SipClientInfo
    {
        public Session session { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string locationCode { get; set; }
    }
}
