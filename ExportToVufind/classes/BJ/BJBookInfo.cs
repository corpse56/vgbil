using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using ExportBJ_XML.classes.BJ;
using ExportBJ_XML.classes.DB;
using System.Data;

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
        
        //public string Title { get; set; }
        //public BJField Author = new BJField();//700a,701a
        //public BJField Annotation = new BJField();//

        public string ID { get; set; }


        #region Экземпляры книги

        public List<ExemplarInfo> Exemplars = new List<ExemplarInfo>();

        #endregion

        public static BJBookInfo GetBookInfoByPIN(int pin, string fund)
        {
            DatabaseWrapper dbw = new DatabaseWrapper(fund);
            DataTable table = dbw.GetBJRecord(pin);
            BJBookInfo result = new BJBookInfo();
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
            return result;
        }

        public static BJBookInfo GetBookInfoByInventoryNumber(string inv, string fund)
        {
            DatabaseWrapper dbw = new DatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(inv);
            BJBookInfo result = BJBookInfo.GetBookInfoByPIN((int)table.Rows[0]["IDMAIN"], fund);
            return result;
        }
    }
}
