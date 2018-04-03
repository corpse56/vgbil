using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Circulation
{
    public struct BJACCRecord
    {
        public int ID;
        public int IDMAIN;
        public int IDDATA;
        public int MNFIELD;
        public string MSFIELD;
        public int IDINLIST;
        public string SORT;
        public string PLAIN;
        public DateTime Created;
        public int Creaator;
        public DateTime Changed;
        public int Changer;
        //public BJACCRecord() { }
    }
}
