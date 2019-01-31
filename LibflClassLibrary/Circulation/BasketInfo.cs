using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation
{
    public class BasketInfo
    {
        public int ID { get; set; }
        public string BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime PutDate { get; set; }
        public List<string> AcceptableOrderType { get; set; } = new List<string>();
        public BookSimpleView Book
        {
            get
            {
                return ViewFactory.GetBookSimpleView(this.BookId);
            }
        }
    }
}
