using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class ElectronicCopyFullView
    {
        public int MethodOfAccessCode { get; set; }
        public int AccessCode { get; set; }
        public string Path_Cover;
        public List<string> JPGFiles = new List<string>();
        public int CountJPG
        {
            get
            {
                return JPGFiles.Count;
            }
        }
        public int WidthFirstFile { get; set; }
        public int HeightFirstFile { get; set; }
        public bool IsExistsLQ { get; set; }
        public bool IsExistsHQ { get; set; }
        public string Path_HQ { get; set; }
        public string Path_LQ { get; set; }


    }
}
