using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.DigitizationQuene
{

    public enum BAZA { BJVVV = 1, REDKOSTJ = 2, BJACC = 3, BJFCC = 4, BJSCC = 5 }
    public class DigitizationQueneItemInfo
    {
        public int id { get; set; }
        public int idMain { get; set; }
        public int ReaderId { get; set; }
        public bool IsRemotereader { get; set; }
        public BAZA baza { get; set; }
        public DateTime? done { get; set; }
        public bool mark { get; set; }
        public bool deleted { get; set; }
        public string delCause { get; set; }
        public DateTime? delDate { get; set; }
    }
}
