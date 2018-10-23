using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Windows.Forms;
using DataProviderAPI.Loaders;
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
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetBJRecord(pin);
            BJBookInfo result = new BJBookInfo();
            result.ID = pin;
            result.Fund = fund;
            BJExemplarInfo exemplar = new BJExemplarInfo(0);
            int CurrentIdData = 0;
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["IDBLOCK"] != 260)
                {
                    if ((int)row["IDBLOCK"] == 270)//если есть гиперссылка
                    {
                        result.DigitalCopy = new BJElectronicExemplarInfo(-1, fund);
                    }
                    else
                    {
                        result.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                    }
                }
                else
                {
                    if (CurrentIdData != (int)row["IDDATA"])
                    {
                        CurrentIdData = (int)row["IDDATA"];
                        result.Exemplars.Add(BJExemplarInfo.GetExemplarByIdData(CurrentIdData, fund));
                        exemplar = new BJExemplarInfo((int)row["IDDATA"]);
                        exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                    }
                    else
                    {
                        exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                    }
                }
            }
            
            table = dbw.GetRTF(pin);
            if (table.Rows.Count != 0)
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Rtf = table.Rows[0][0].ToString();
                result.RTF = rtb.Text;
                rtb.Dispose();
            }
            return result;
        }

        public static BJBookInfo GetBookInfoByInventoryNumber(string inv, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(inv);
            BJBookInfo result = BJBookInfo.GetBookInfoByPIN((int)table.Rows[0]["IDMAIN"], fund);
            return result;
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
