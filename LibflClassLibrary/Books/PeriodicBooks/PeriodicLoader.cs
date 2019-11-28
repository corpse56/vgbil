using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return result;
        }
    }
}
