using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation
{
    public class OrderHistoryInfo
    {
        public BookSimpleView Book;
        public string BookId { get; set; }
        public int ExemplarId { get; set; }
        public int ReaderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime FactReturnDate { get; set; }
        public int OrderId { get; set; }
    }
}
