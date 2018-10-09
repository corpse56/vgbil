using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.ExportToVufind.Vufind;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BookJSONViewers
{
    public class BookJSONShortViewer
    {
        public string GetView(BJBookInfo book)
        {
            VufindDoc vfDoc = book.GetVufindDocument();

            string json = JsonConvert.SerializeObject(book, Formatting.Indented);

            return json;
        }

        
    }
}
