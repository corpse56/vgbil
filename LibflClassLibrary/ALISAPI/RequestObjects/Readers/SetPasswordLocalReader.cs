using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Readers
{
    public class SetPasswordLocalReader
    {
        public int ReaderId { get; set; }
        public string NewPassword { get; set; }
    }
}
