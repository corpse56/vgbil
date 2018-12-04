using LibflClassLibrary.Books.BJBooks;
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
    }
}
