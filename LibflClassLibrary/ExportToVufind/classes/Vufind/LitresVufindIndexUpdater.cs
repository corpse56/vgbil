using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using ExportBJ_XML.classes;
using System.Xml.Linq;
using Utilities;

namespace LibflClassLibrary.ExportToVufind.classes.Vufind
{
    public class LitresVufindIndexUpdater : VufindIndexUpdater
    {
        public override XDocument GetCurrentIncrement()
        {
            //внимание! Timestamp нужен от текущего времени, а не от чекпоинта! SHA генерируется тоже от текущего времени, а не от чекпоинта
            DateTime CurrentDate = DateTime.Now;
            string stamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();
            string key = "geqop45m))AZvb23zerhgjj76cvc##PFbbfqorptqskj";
            DateTime LastIncrementDate = this.GetLastIncrementDate("litres");
            DateTime checkpointDate = LastIncrementDate;
            string checkpoint = checkpointDate.ToString("yyyy-MM-dd HH:mm:ss");//"2017-01-01 00:00:00";
            //string endpoint = checkpointDate.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");

            string inputString = stamp + ":" + key + ":" + checkpoint;
            string sha256 = Utilities.Extensions.sha256(inputString);


            Uri apiUrl =
            new Uri("http://partnersdnld.litres.ru/get_fresh_book/?checkpoint=" + checkpoint +
                                                                 "&place=GTCTL" +
                                                                 "&timestamp=" + stamp +
                                                                 "&sha=" + sha256);
            //"&enpoint="+endpoint);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(apiUrl) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //response.Close();
            XDocument xdoc = XDocument.Load(new StreamReader(response.GetResponseStream()));

            return xdoc;
        }

    }
}
