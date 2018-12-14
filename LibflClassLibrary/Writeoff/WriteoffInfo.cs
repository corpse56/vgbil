using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Writeoff.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Writeoff
{
    public class WriteoffInfo
    {
        WriteoffLoader loader;
        string Fund;
        public WriteoffInfo(string Fund)
        {
            this.Fund = Fund;
            loader = new WriteoffLoader(Fund);
        }

        public Dictionary<int,string> GetDepartments()
        {
            return loader.GetDepartments();
        }

        public Dictionary<string, string> GetWriteoffActs(int year)
        {
            return loader.GetWriteoffActs(year);
        }

        public List<BJBookInfo> GetBooksByAct(string ActNumber)
        {
            return loader.GetBooksByAct(ActNumber);
        }

        public List<BJExemplarInfo> GetBooksPerYear(int Year, string Prefix)
        {
            return loader.GetBooksPerYear(Year, Prefix);
        }

        public List<BJExemplarInfo> GetBooksPerYearAnotherFundholder(int Year)
        {
            return loader.GetBooksPerYearAnotherFundholder(Year);
        }

        public List<BJExemplarInfo> GetBooksPerYearInActNameAB(int Year)
        {
            return loader.GetBooksPerYearInActNameAB(Year);
        }
        public List<BJExemplarInfo> GetBooksPerYearInActNameOF(int Year)
        {
            return loader.GetBooksPerYearInActNameOF(Year);
        }
        public List<BJExemplarInfo> GetBooksPerYearInActNameAnotherFundholder(int Year)
        {
            return loader.GetBooksPerYearInActNameAnotherFundholder(Year);
        }

    }
}
