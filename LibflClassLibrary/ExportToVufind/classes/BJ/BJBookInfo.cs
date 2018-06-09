using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using ExportBJ_XML.classes.BJ;
using ExportBJ_XML.classes.DB;
using System.Data;
using System.Windows.Forms;
using DataProviderAPI.Loaders;

/// <summary>
/// Сводное описание для BookInfo
/// </summary>
namespace ExportBJ_XML.ValueObjects
{
    public class BJBookInfo
    {
        public BJBookInfo()
        {
        }

        public BJFields Fields = new BJFields();

        public string RTF;

        public int ID { get; set; }

        public string Fund {get;set;}


        #region Экземпляры книги

        public List<ExemplarInfo> Exemplars = new List<ExemplarInfo>();

        #endregion

        public static BJBookInfo GetBookInfoByPIN(int pin, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetBJRecord(pin);
            BJBookInfo result = new BJBookInfo();
            result.ID = pin;
            result.Fund = fund;
            ExemplarInfo exemplar = new ExemplarInfo(0);
            int CurrentIdData = 0;
            foreach (DataRow row in table.Rows)
            {
                if ((int)row["IDBLOCK"] != 260)
                {
                    result.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());
                }
                else
                {
                    if (CurrentIdData != (int)row["IDDATA"])
                    {
                        CurrentIdData = (int)row["IDDATA"];
                        result.Exemplars.Add(ExemplarInfo.GetExemplarByIdData(CurrentIdData, fund));
                        exemplar = new ExemplarInfo((int)row["IDDATA"]);
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

        //Если выданы все бумажные, то нельзя.
        //Если электронных плюс бумажных выдач столько же сколько всего бумажных то нельзя.
        //Если электронных выдач столько же сколько бумажных, то
        public bool IsElBookAvailableForIssue()
        {
            if (this.Exemplars.Count - this.GetBusyExemplarCount() <= 0)
            {
                return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право." +
                    " Ближайшая свободная дата " + b.GetNearestFreeDate().ToString("dd.MM.yyyy") + ". Попробуйте заказать в указанную дату.";

            }
        }
        public int GetBusyExemplarCount()
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.GetBusyExemplarCount(this.ID);
            return table.Rows.Count;
        }

        public DateTime GetNearestFreeDateForElectronicIssue()
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
            DataTable table = dbw.GetNearestFreeDateForElectronicIssue(this.ID);
            if (table.Rows.Count == 0) return DateTime.Today;
            return (DateTime)table.Rows[0][0];
        }


    }
}
