using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security;

namespace ExportBJ_XML.classes
{

    public abstract class VuFindConverter
    {
        public string Fund
        { 
            get 
            {
                return this._fund; 
            }
            protected set
            {
                this._fund = value;
            }
        }
        private string _fund = "unknown";

        public event EventHandler RecordExported;
        //public event EventHandler<VuFindConverterEventArgs> OnDatabaseTimeout;
        //event EventHandler<VuFindConverterEventArgs> OnConvertError;

        public VuFindConverter() 
        {

            


        }

        public abstract void Export();
        public abstract void ExportSingleRecord(int idRecord);
        public abstract void ExportCovers();
        public abstract void ExportSingleCover(object idRecord);


        protected virtual void OnRecordExported(EventArgs e)
        {
            EventHandler handler = RecordExported;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public static string GetCoverExportPath(string id)
        {
            string ID = id.Substring(id.LastIndexOf("_")+1);
            string result = "";

            switch (ID.Length)//настроено на семизначный, но в будущем будет 9-значный айдишник
            {
                case 1:
                    result = "00000000" + ID;
                    break;
                case 2:
                    result = "0000000" + ID;
                    break;
                case 3:
                    result = "000000" + ID;
                    break;
                case 4:
                    result = "00000" + ID;
                    break;
                case 5:
                    result = "0000" + ID;
                    break;
                case 6:
                    result = "000" + ID;
                    break;
                case 7:
                    result = "00" + ID;
                    break;
                case 8:
                    result = "0" + ID;
                    break;
                case 9:
                    result = ID;
                    break;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(result[0]).Append(result[1]).Append(result[2]).Append(@"\").Append(result[3]).Append(result[4]).Append(result[5]).Append(@"\").Append(result[6]).Append(result[7]).Append(result[8]).Append(@"\JPEG_AB\");
            //string returnValue = result[0].ToString() + result[1].ToString() + result[2].ToString() + @"\" + result[3] + result[4] + result[5] + @"\" + result[6] + result[7] + result[8] + @"\JPEG_AB\";
            return sb.ToString();
        }

        public static string GetFundId(string fund)
        {

            switch (fund)
            {
                case "BJVVV":
                    return "5000";
                case "REDKOSTJ":
                    return "5001";
                case "BJACC":
                    return "5003";
                case "BJFCC":
                    return "5004";
                case "BJSCC":
                    return "5005";
                case "BRIT_SOVET":
                    return "5002";
                case "litres":
                    return "5007";
                case "period":
                    return "5006";
                case "HJB":
                    return "5009";
                case "pearson":
                    return "5008";
            }
            return "<неизвестный фонд>";
        }


    }
}
