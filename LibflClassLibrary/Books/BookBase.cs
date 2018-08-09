using LibflClassLibrary.Books.BJBooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books
{
    [JsonConverter(typeof(BookJsonConverter))]
    public abstract class BookBase
    {
        public string Id { get; set; }

        public List<BookExemplarBase> Exemplars { get; set; } = new List<BookExemplarBase>();

        public virtual string ToJsonString()
        {
            return "simpleBook";
        }

        //public static BookBase GetBook(string Id)
        //{

        //}
    }
}
