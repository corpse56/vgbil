using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Readers
{
    public class ChangePasswordLocalReader
    {
        public int NumberReader { get; set; }
        public DateTime DateBirth { get; set; }
        public string NewPassword { get; set; }
    }
}
