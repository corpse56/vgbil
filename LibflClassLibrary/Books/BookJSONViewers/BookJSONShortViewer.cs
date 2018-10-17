using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.ExportToVufind.BJ;
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

            BJVuFindConverter converter = new BJVuFindConverter(book.Fund);
            VufindDoc vfDoc = converter.CreateVufindDocument(book.ID);
            string json = JsonConvert.SerializeObject(book, Formatting.Indented);

            return json;
        }

        
    }
}
