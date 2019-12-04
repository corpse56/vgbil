using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books
{
    public abstract class ExemplarBase
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string Fund { get; set; }
        public abstract string Author { get; set; }
        public abstract string Title { get; set; }
        public abstract string InventoryNumber { get; set; }
        public abstract string Cipher { get; set; }
        public abstract string Rack { get; set; }
        public abstract string Bar { get; set; }
        public abstract string Location { get; set; }
        public abstract string Language { get; set; }
        public abstract string PublicationClass { get; set; }
        public string AuthorTitle
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Author) ? this.Title : $"{this.Author}; {this.Title}";
            }
        }

        public ExemplarAccessInfo AccessInfo { get; set; }

    }
}
