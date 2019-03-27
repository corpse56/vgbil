using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Circulation
{
    public class BasketDelete
    {
        public int ReaderId { get; set; }
        public List<string> BooksToDelete { get; set; } = new List<string>();
    }
}
