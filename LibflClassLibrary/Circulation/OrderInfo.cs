using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation
{
    public class OrderInfo
    {
        public BookSimpleView Book;
        public string BookId { get; set; }
        public string ExemplarId { get; set; }
        public int ReaderId { get; set; }
        public string StatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? FactReturnDate { get; set; }
        public int AnotherReaderId { get; set; }
        public string IssueDep { get; set; }
        public string ReturnDep { get; set; }
        

    }
}
