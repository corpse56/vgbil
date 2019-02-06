using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Circulation
{
    public class ImpersonalBasket
    {
        public int ReaderId { get; set; }
        public List<string> BookIdArray { get; set; }
    }
}
