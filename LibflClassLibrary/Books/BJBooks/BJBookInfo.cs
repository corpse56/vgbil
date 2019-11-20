using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Security.Cryptography;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks.DB;
using Newtonsoft.Json;
using LibflClassLibrary.Readers;
using LibflClassLibrary.ExportToVufind.Vufind;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind;
using Utilities;
using System.Diagnostics;
using LibflClassLibrary.Books.BJBooks.Loaders;

/// <summary>
/// Сводное описание для BookInfo
/// </summary>
namespace LibflClassLibrary.Books.BJBooks
{
    [JsonConverter(typeof(BJBookJsonConverter))]
    public class BJBookInfo : BookBase
    {
        public BJBookInfo()
        {
        }

        public BJFields Fields = new BJFields();

        public string RTF;

        public int ID { get; set; }

        public string Fund {get;set;}


        #region Экземпляры книги

        //public List<ExemplarInfo> Exemplars = new List<ExemplarInfo>();

        #endregion

        public BJElectronicExemplarInfo DigitalCopy = null;
        public bool IsExistsDigitalCopy => this.DigitalCopy != null;

        public static BJBookInfo GetBookInfoByPIN(int pin, string fund)
        {
            BJBookLoader loader = new BJBookLoader(fund);
            return loader.GetBookInfoByPIN(pin);

        }
        public static BJBookInfo GetBookInfoByPIN(string FullPin)
        {
            string fund = FullPin.Substring(0, FullPin.IndexOf("_"));
            int IDRecord = int.Parse(FullPin.Substring(FullPin.LastIndexOf("_") + 1));
            return BJBookInfo.GetBookInfoByPIN(IDRecord, fund);
        }
        public static BJBookInfo GetBookInfoByInventoryNumber(string inv, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(inv);
            if (table.Rows.Count == 0) return null;
            BJBookInfo result = BJBookInfo.GetBookInfoByPIN((int)table.Rows[0]["IDMAIN"], fund);
            return result;
        }

        public string AuthorTitle()
        {
            return (this.Fields["700$a"].HasValue) ? 
                                                    $"{this.Fields["700$a"].ToString()}; {this.Fields["200$a"].ToString()}" 
                                                    : this.Fields["200$a"].ToString();
        }

        internal static BJBookInfo GetBookInfoByBAR(string data)
        {
            BJBookInfo book;
            BJBookLoader loader = new BJBookLoader("BJVVV");
            book = loader.GetBJBookByBar(data);
            return book;
            
        }







        public int GetBusyElectronicExemplarCount()
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.GetBusyElectronicExemplarCount(this.ID);
            return table.Rows.Count;
        }

        public DateTime GetNearestFreeDateForElectronicIssue()
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.GetNearestFreeDateForElectronicIssue(this.ID);
            if (table.Rows.Count == 0) return DateTime.Today;
            object o = table.Rows[0][0];
            if (o == DBNull.Value) return DateTime.Today;
            return (DateTime)table.Rows[0][0];
        }

        public bool IsOneDayPastAfterReturn(int IDREADER)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.IsOneDayPastAfterReturn(this.ID,IDREADER);

            foreach (DataRow r in table.Rows)
            {
                TimeSpan ts = (DateTime.Now.Date - (DateTime)r["DATERETURN"]);
                if (ts.Days >= 1) return true; else return false;
            }
            return true;
        }


        public bool IsElectronicCopyIsuuedToReader(int IDREADER)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.IsElectronicCopyIsuuedToReader(this.ID, IDREADER);
            return (table.Rows.Count != 0)? true : false;
        }

        public string GetElectronicViewKeyForReader(int IDREADER)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.GetElectronicViewKeyForReader(this.ID, IDREADER);
            return table.Rows[0]["VIEWKEY"].ToString();
        }

      

        public bool IsElectronicCopyIssued()
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.IsElectronicCopyIssued(this.ID);
            return (table.Rows.Count != 0) ? true : false;
        }

       

        public void IssueElectronicCopyToReader(int IDREADER)
        {
            ReaderInfo reader = ReaderInfo.GetReader(IDREADER);
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            byte[] random = new byte[20];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.
            string ViewKey = Convert.ToBase64String(random);
            int IssuePeriodDays = 30;
            dbw.IssueElectronicCopyToReader(this.ID, IssuePeriodDays, ViewKey, IDREADER, reader.TypeReader);
        }

      

        internal string ToJsonString()
        {
            throw new NotImplementedException();
        }
    }
}
