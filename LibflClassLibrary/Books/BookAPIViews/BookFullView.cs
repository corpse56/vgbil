using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BookAPIViews
{
    public class BookFullView
    {
        public List<BookViewField> Fields { get; set;} = new List<BookViewField>();
        public List<ExemplarFullView> Exemplars { get; set; } = new List<ExemplarFullView>();
    }
}
