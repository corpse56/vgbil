using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using ExportBJ_XML.classes.DB;
using System.Data;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Security.Policy;
using System.Web;


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
            StringBuilder address = new StringBuilder();
            StringBuilder query = new StringBuilder();
            address.Append(@"http://192.168.56.31:8080/solr/biblio/update?stream.body=");
            query.Append("%3Cdelete%3E");
            foreach (string id in IdList)
            {
                query.AppendFormat("%3Cquery%3Eid:{0}%3C/query%3E", id);
            }
            query.Append("%3C/delete%3E");

            address.Append(query.ToString());
            Uri url = new Uri(address.ToString());
            
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;


            XDocument ResponseXML = new XDocument();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)//посылаем запрос на удаление
                {
                    
                }
            }
            catch (WebException ex)
            {
                ResponseXML = XDocument.Load(new StreamReader(ex.Response.GetResponseStream()));
                throw new Exception(ResponseXML.ToString());
            }


            url = new Uri("http://192.168.56.31:8080/solr/biblio/update?stream.body=%3Ccommit/%3E");
            request = HttpWebRequest.Create(url) as HttpWebRequest;

            
            
            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;//подтверждаем удаление
                ResponseXML = XDocument.Load(new StreamReader(response.GetResponseStream()));
            }
            catch (WebException ex)
            {
                ResponseXML = XDocument.Load(new StreamReader(ex.Response.GetResponseStream()));
                throw new Exception(ResponseXML.ToString());
            }


            //здесь добавить анализ ответа
            //Анализ ответа добавлять не нужно, поскольку если солару что-то не нравится, то в response будет ответ не 200, а другой и вывалится исключение. его ловим и передаём ответ солара для анализа наверх и записываем в лог
            //var status = ResponseXML.Descendants("int");
            //foreach (XElement elt in status)
            //{
            //    if (elt.Attribute("name") != null)
            //    {
            //        string sss = elt.Attribute("name").Value;
            //        if (elt.Attribute("name").Value == "status")
            //        {
            //            string s = elt.Value;
            //            if (elt.Value != "0")
            //            {
            //                throw new Exception("Ошибка Solr. " + ResponseXML.ToString());
            //            }
            //        }
            //    }
            //}

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
