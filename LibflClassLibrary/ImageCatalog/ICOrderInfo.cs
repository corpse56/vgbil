using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public class ICOrderInfo
    {
        public int Id { get; set; }
        public string CardId { get; set; }
        public int CardSide { get; set; }
        public string Comment { get; set; }
        public int ReaderId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
