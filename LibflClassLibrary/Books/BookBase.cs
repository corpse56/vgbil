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
        public string Fund { get; set; }
        public List<ExemplarBase> Exemplars { get; set; } = new List<ExemplarBase>();


        //виртуальные свойства
        public virtual string Language { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        internal string ToJsonString()
        {
            throw new NotImplementedException();
        }
        public static string GetFund(string bookId)
        {
            return bookId.Substring(0, bookId.IndexOf("_"));
        }
        public static int GetPIN(string bookId)
        {
            return int.Parse(bookId.Substring(bookId.LastIndexOf("_") + 1));
        }
        public static string GetRusFundName(string fund)
        {
            switch (fund)
            {
                case "BJVVV":
                    return "ОФ";
                case "REDKOSTJ":
                    return "Редкая книга";
                case "BJACC":
                    return "АКЦ";
                case "BJFCC":
                    return "ФКЦ";
                case "BJSCC":
                    return "СКЦ";
                case "PERIOD":
                    return "Периодика";
            }
            return "<неизвестно>";
        }

    }
}
