using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books
{
    public abstract class BookExemplarBase
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string Fund { get; set; }
        public virtual string Author() { return string.Empty; }
        public virtual string Title() { return string.Empty; }



    }
}
