using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ExportBJ_XML.classes.DB;
using System.Data;
using System.Xml.Linq;
using System.IO;

namespace LibflClassLibrary.ExportToVufind.classes.Vufind
{
    public abstract class VufindIndexUpdater
    {
        public VufindIndexUpdater() { }

        public abstract XDocument GetCurrentIncrement();


//        curl http://dev.libfl.ru:8080/solr/biblio/update --data "<delete><query>id:BJVVV_1463041</query></delete>" -H "Content-type:text/xml; charset=utf-8"
//        curl http://dev.libfl.ru:8080/solr/biblio/update --data "<commit/>" -H "Content-type:text/xml; charset=utf-8"
//        curl http://dev.libfl.ru:8080/solr/biblio/update?commit=true -H "Content-Type: text/xml" --data-binary @f:\import\singleRecords\BJVVV_1463041.xml

        //curl http://192.168.56.31:8080/solr/biblio/update --data "<delete><query>id:BJVVV_1463041</query><query>id:BJVVV_1463040</query></delete>" -H "Content-type:text/xml; charset=utf-8"
        //curl http://192.168.56.31:8080/solr/biblio/update --data "<commit/>" -H "Content-type:text/xml; charset=utf-8"

            
        public void DeleteFromIndex(List<string> IdList)
        {
            if (IdList.Count == 0) return;
            StringBuilder sb = new StringBuilder();
            sb.Append("http://192.168.56.31:8080/solr/biblio/update --data \"<delete>\"");
            foreach (string id in IdList)
            {
                sb.AppendFormat("<query>id:{0}</query>", id);
            }
            sb.Append("</delete>\" -H \"Content-type:text/xml; charset=utf-8\"");
            Uri url = new Uri(sb.ToString());
            
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;//посылаем запрос на удаление


            url = new Uri("http://192.168.56.31:8080/solr/biblio/update --data \"<commit/>\" -H \"Content-type:text/xml; charset=utf-8\"");
            request = HttpWebRequest.Create(url) as HttpWebRequest;
            response = request.GetResponse() as HttpWebResponse;//подтверждаем удаление
            XDocument ResponseXML = XDocument.Load(new StreamReader(response.GetResponseStream()));


            //здесь добавить анализ ответа

        }
        public DateTime GetLastIncrementDate(string fund)
        {
            DatabaseWrapper db = new DatabaseWrapper(fund);
            DataTable table = db.GetLastIncrementDate();
            return (DateTime)table.Rows[0][0];
        }

        public void SetLastIncrementDate(string fund)
        {
            DatabaseWrapper db = new DatabaseWrapper(fund);
            db.SetLastIncrementDate(DateTime.Now.AddHours(-1));
        }
    }
}
