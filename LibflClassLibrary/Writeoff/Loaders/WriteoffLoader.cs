using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Writeoff.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Writeoff.Loaders
{
    public class WriteoffLoader
    {
        private string Fund;
        private WriteoffDatabaseWrapper db;
        public WriteoffLoader(string Fund)
        {
            this.Fund = Fund;
            db = new WriteoffDatabaseWrapper(Fund);
        }

        public Dictionary<int, string> GetDepartments()
        {
            DataTable table = db.GetDepartments();
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (DataRow row in table.Rows)
            {
                result.Add((int)row["ID"], row["NAME"].ToString());
            }
            return result;
        }

        internal List<BJBookInfo> GetBooksByAct(string ActNumber)
        {
            DataTable table = db.GetBooksByAct(ActNumber);
            List<BJBookInfo> result = new List<BJBookInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJBookInfo b = BJBookInfo.GetBookInfoByPIN((int)row["IDMAIN"], Fund);
                result.Add(b);
            }
            return result;
        }


        internal List<BJExemplarInfo> GetBooksPerYear(int year, string prefix)
        {
            DataTable table = db.GetBooksPerYear(year, prefix);
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }
        internal List<BJExemplarInfo> GetBooksPerYearAnotherFundholder(int year)
        {
            DataTable table = db.GetBooksPerYearAnotherFundholder(year);
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }

        internal List<BJExemplarInfo> GetBooksOnSpecifiedActNumbers(List<string> acts)
        {
            StringBuilder filter = new StringBuilder("(");
            foreach (var act in acts)
            {
                filter.Append($"'{act}',");
            }
            filter.Remove(filter.Length - 1, 1);
            filter.Append(")");
            DataTable table = db.GetBooksOnSpecifiedActNumbers(filter.ToString());
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }

        internal List<BJExemplarInfo> GetBooksPerYearInActNameAB(int year)
        {
            DataTable table = db.GetBooksPerYearInActNameAB(year);
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }
        internal List<BJExemplarInfo> GetBooksPerYearInActNameOF(int year)
        {
            DataTable table = db.GetBooksPerYearInActNameOF(year);
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }
        internal List<BJExemplarInfo> GetBooksPerYearInActNameAnotherFundholder(int year)
        {
            DataTable table = db.GetBooksPerYearInActNameAnotherFundholder(year);
            List<BJExemplarInfo> result = new List<BJExemplarInfo>();
            foreach (DataRow row in table.Rows)
            {
                BJExemplarInfo b = BJExemplarInfo.GetExemplarByIdData((int)row["IDDATA"], Fund);
                result.Add(b);
            }
            return result;
        }

        public Dictionary<string, string> GetWriteoffActs(int year)
        {
            DataTable table = db.GetWriteoffActs(year);
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(DataRow row in table.Rows)
            {
                result.Add(row["SORT"].ToString(), row["PLAIN"].ToString());
            }
            return result;
        }
    }
}
