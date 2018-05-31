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
using ExportBJ_XML.classes;
using LibflClassLibrary.ExportToVufind.classes.BJ;
using Newtonsoft.Json;


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
            address.Append(@"http://192.168.56.31:8080/solr/biblio/update");

            //IdList = IdList.Take(5).ToList();
            //IdList.Clear();
            //IdList.Add("Litres_164927");
            //IdList.Add("Litres_164928");
            //IdList.Add("Litres_164929");


            //query.Append("%3Cdelete%3E");
            //foreach (string id in IdList)
            //{
            //    query.AppendFormat("%3Cquery%3Eid:{0}%3C/query%3E", id);
            //}
            //query.Append("%3C/delete%3E");

            query.Append("<delete>");
            foreach (string id in IdList)
            {
                query.AppendFormat("<query>id:{0}</query>", id);
            }
            query.Append("</delete>");

            //address.Append(query.ToString());
            Uri url = new Uri(address.ToString());
            
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 120000000;
            request.Method = "POST";
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] postByteArray = System.Text.Encoding.UTF8.GetBytes("stream.body=" + query);
            request.ContentLength = postByteArray.Length;
            Stream postStream = request.GetRequestStream();
            postStream.Write(postByteArray, 0, postByteArray.Length);
            postStream.Close();

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
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;//подтверждаем удаление. подтверждение нужно посылать отдельным запросом.
                ResponseXML = XDocument.Load(new StreamReader(response.GetResponseStream()));
            }
            catch (WebException ex)
            {
                ResponseXML = XDocument.Load(new StreamReader(ex.Response.GetResponseStream()));
                throw new Exception(ResponseXML.ToString());
            }
        }

        public void AddToIndex(VufindDoc vdoc)
        {
            
            // To convert an XML node contained in string xml into a JSON string   
            XmlDocument doc = new XmlDocument();

            XmlNode RootNode = doc.CreateElement("add");
            doc.AppendChild(RootNode);
            XmlNode node = vdoc.CreateExportXmlNode();
            node = doc.ImportNode(node, true);
            RootNode.AppendChild(node);
            doc.AppendChild(RootNode);

            //doc.Load(AppDomain.CurrentDomain.BaseDirectory + @"increment31.05.2018 01.54.xml");//для отладки

            Uri url = new Uri("http://192.168.56.31:8080/solr/biblio/update?commit=true");
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 120000000;
            request.Method = "POST";
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/xml";
            
            Stream postStream = request.GetRequestStream();
            doc.Save(postStream);
            postStream.Flush();
            postStream.Close();
            
            XDocument ResponseXML = new XDocument();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)//посылаем запрос на удаление
                {
                    using (Stream streamResponse = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(streamResponse, Encoding.UTF8))
                        {
                            ResponseXML = XDocument.Load(reader);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                ResponseXML = XDocument.Load(new StreamReader(ex.Response.GetResponseStream()));
                throw new Exception(ResponseXML.ToString());
            }
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
