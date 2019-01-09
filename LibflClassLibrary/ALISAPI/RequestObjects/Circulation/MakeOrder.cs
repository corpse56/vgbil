using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Circulation
{
    public class MakeOrder
    {
        public string BookId { get; set; }
        public int ReaderId { get; set; }
        public string OrderType { get; set; }
    }
}
