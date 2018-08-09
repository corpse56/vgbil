using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ExportToVufind.Vufind
{
    public class VuFindConverterEventArgs : EventArgs
    {
        public string RecordId { get; set; }
    }
}
