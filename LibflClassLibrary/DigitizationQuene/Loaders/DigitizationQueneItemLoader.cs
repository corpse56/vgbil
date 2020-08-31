using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.DigitizationQuene.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.DigitizationQuene.Loaders
{
    class DigitizationQueneItemLoader
    {
        DigitizatioQueneDBWrapper dbWrapper = new DigitizatioQueneDBWrapper();
        public List<DigitizationQueneItemInfo> GetQuene()
        {
            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            DataTable table = dbWrapper.GetQuene();
            foreach (DataRow row in table.Rows)
            {
                DigitizationQueneItemInfo item = ParseRow(row);
                result.Add(item);
            }
            return result;
        }
        public List<DigitizationQueneItemInfo> GetCompleted()
        {
            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            DataTable table = dbWrapper.GetCompleted();
            foreach (DataRow row in table.Rows)
            {
                DigitizationQueneItemInfo item = ParseRow(row);
                result.Add(item);
            }
            return result;
        }
        public List<DigitizationQueneItemInfo> GetDeleted()
        {
            List<DigitizationQueneItemInfo> result = new List<DigitizationQueneItemInfo>();
            DataTable table = dbWrapper.GetDeleted();
            foreach (DataRow row in table.Rows)
            {
                DigitizationQueneItemInfo item = ParseRow(row);
                result.Add(item);
            }
            return result;
        }
        public void AddToQuene(string bookId, int readerId)
        {
            int idBase = GetIdBase(bookId);
            int idMain = BJBookInfo.GetPIN(bookId);
            DataTable table = dbWrapper.IsAlreadyInQuene(idBase,idMain);
            if (table.Rows.Count > 0)
            {
                throw new Exception("Q001");
            }
            table = dbWrapper.IsMoreThanTwoBooksReaderWantsToDigitizePer24Hour(readerId);
            if (table.Rows.Count >= 2)
            {
                throw new Exception("Q002");
            }
            table = dbWrapper.IsQueneOverflowed();
            if (table.Rows.Count >= 80)
            {
                throw new Exception("Q003");
            }
            if ((BAZA)idBase != BAZA.BJVVV)
            {
                throw new Exception("Q004");
            }
            dbWrapper.AddToQuene(idBase, idMain, readerId);
        }
        private int GetIdBase(string bookId)
        {
            string fund = BJBookInfo.GetFund(bookId);
            switch (fund)
            {
                case "BJVVV":
                    return 1;
                case "REDKOSTJ":
                    return 2;
                case "BJACC":
                    return 3;
                case "BJFCC":
                    return 4;
                case "BJSCC":
                    return 5;
            }
            return 0;
        }
        
        private DigitizationQueneItemInfo ParseRow(DataRow row)
        {
            DigitizationQueneItemInfo item = new DigitizationQueneItemInfo();
            item.baza = (BAZA)Convert.ToInt32(row["BAZA"]);
            item.delCause = row["DELCAUSE"].ToString();
            item.delDate = (row["DELDATE"] == DBNull.Value) ? (DateTime?)null : (DateTime)row["DELDATE"];
            item.deleted = (row["DELETED"] == DBNull.Value)? false : (bool)row["DELETED"];
            item.done = (row["DateEBook"] == DBNull.Value) ? (DateTime?)null : (DateTime)row["DateEBook"];
            item.id = (int)row["ID"];
            item.idMain = (int)row["IDMAIN"];
            item.IsRemotereader = false;
            item.mark = (row["MARK"] == DBNull.Value)? false : (bool)row["MARK"];
            item.ReaderId = (int)row["IDREADER"];
            return item;
        }

    }
}
