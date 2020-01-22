using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public class ICOrderFlowInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Changer { get; set; }
        public int DepartmentId { get; set; }
        public DateTime Changed { get; set; }
        public string StatusName { get; set; }
        public string Refusual { get; set; }
    }
}
