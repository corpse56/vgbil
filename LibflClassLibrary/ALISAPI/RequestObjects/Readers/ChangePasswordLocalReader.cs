using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Readers
{
    public class ChangePasswordLocalReader
    {
        public int NumberReader { get; set; }
        public string NewPassword { get; set; }
        public string DateBirth { get; set; }
    }
}
