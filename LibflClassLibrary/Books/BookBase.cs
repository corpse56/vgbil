using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.ExportToVufind.Vufind;
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

        internal string ToJsonString()
        {
            throw new NotImplementedException();
        }
        public static string GetFund(string BookId)
        {
            return BookId.Substring(0, BookId.IndexOf("_"));
        }
        public static int GetIDMAIN(string BookId)
        {
            return int.Parse(BookId.Substring(BookId.LastIndexOf("_") + 1));
        }

    }
}
