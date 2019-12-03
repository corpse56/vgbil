using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodicBooks;

namespace LibflClassLibrary.Books.PeriodBooks
{

    public class PeriodicLoader
    {
        private PeriodicDatabaseWrapper dbWrapper_;

        public PeriodicLoader()
        {
            dbWrapper_ = new PeriodicDatabaseWrapper();
        }

        internal PeriodicBookInfo GetBookByBar(string bar)
        {
            DataTable table = dbWrapper_.GetBookByBar(bar);
            if (table.Rows.Count == 0) return null;
            PeriodicBookInfo result = new PeriodicBookInfo();
            DataRow row = table.Rows[0];
            result.Id = row["pin"].ToString();
            result.Title = row["title"].ToString();
            result.PublishYear = row["pubYear"].ToString();
            result.Number = row["number"].ToString();
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo exemplar = loader.GetExemplarByBar(bar);
            if (exemplar != null)
            {
                result.Exemplars.Add(exemplar);
            }
            return result;
        }

        internal PeriodicExemplarInfo GetPeriodicExemplarInfoByExemplarId(int exemplarId)
        {
            throw new NotImplementedException();
        }

        internal string GetBookBarByInventoryNumber(string inventoryNumber)
        {
            DataTable table = dbWrapper_.GetBookBarByInventoryNumber(inventoryNumber);
            if (table.Rows.Count == 0) return null;
            return table.Rows[0]["bar"].ToString();
        }

        internal PeriodicExemplarInfo GetExemplarByBar(string bar)
        {
            PeriodicExemplarInfo result = new PeriodicExemplarInfo();
            DataTable table = dbWrapper_.GetBookByBar(bar);
            if (table.Rows.Count == 0) return null;
            DataRow row = table.Rows[0];
            result.Bar = bar;
            result.Fund = "PERIOD";
            result.Number = row["number"].ToString();
            result.Id = row["exemplarId"].ToString();//idz of bar
            result.PublishYear = row["pubYear"].ToString();
            result.BookId = $"PERIOD_{row["pin"].ToString()}";
            result.Author = string.Empty ;
            result.Title = row["title"].ToString();
            result.AccessInfo = new ExemplarAccessInfo();
            result.AccessInfo.Access = 1005;
            result.AccessInfo.MethodOfAccess = 4000;
            return result;
        }
    }
}
