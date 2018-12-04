using LibflClassLibrary.Books.BJBooks;
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
