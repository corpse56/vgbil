using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes;
using System.Xml.Linq;
using LibflClassLibrary.ExportToVufind.classes.Vufind;

namespace VufindIncrementUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            LitresVufindIndexUpdater litres = new LitresVufindIndexUpdater();
            XDocument IncrementXML = litres.GetCurrentIncrement();
            var removedBooks = IncrementXML.Descendants("removed-book");
            List<string> removedBookIDs = new List<string>();
            foreach (XElement elt in removedBooks)
            {
                removedBookIDs.Add(elt.Attribute("id").Value);
            }
            ...
        }
    }
}
