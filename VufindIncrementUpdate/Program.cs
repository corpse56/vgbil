using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes;
using System.Xml.Linq;
using LibflClassLibrary.ExportToVufind.classes.Vufind;
using Utilities;
using System.Xml;
using System.Net;
using LibflClassLibrary.ExportToVufind.classes.BJ;

namespace VufindIncrementUpdate
{
    class Program
    {
        static void Main(string[] args)
        {

            Log log = new Log();
            log.WriteLog("Начало инкрементной загрузки Litres...");
            LitresVufindIndexUpdater litres = new LitresVufindIndexUpdater();

            //получаем инкремент
            XDocument IncrementXML = new XDocument();
            try
            {
                //IncrementXML = litres.GetCurrentIncrement();
            }
            catch (Exception ex)
            {
                log.WriteLog("Загрузка инкремента завершилось неудачей. " + ex.Message);
                log.Dispose();
                Console.WriteLine("Error...");
                Console.ReadKey();
            }


            IncrementXML = XDocument.Load(@"F:\increment_litres.xml");
            //XmlWriter writ = XmlTextWriter.Create(@"F:\increment_litres.xml");
            //IncrementXML.WriteTo(writ);
            //writ.Flush();
            //writ.Close();

            //вычленияем удалённые записи и удаляем их из индекса
            var removedBooks = IncrementXML.Descendants("removed-book");
            List<string> removedBookIDs = new List<string>();
            foreach (XElement elt in removedBooks)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                removedBookIDs.Add(sb.ToString());
            }

            //вычленияем изменённые записи и тоже удаляем их из индекса
            removedBooks = IncrementXML.Descendants("updated-book");
            foreach (XElement elt in removedBooks)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                removedBookIDs.Add(sb.ToString());
            }

            removedBookIDs.Clear();
            removedBookIDs.Add("132123213213213213213213123");
            try
            {
                litres.DeleteFromIndex(removedBookIDs);
            }
            catch (Exception ex)
            {
                log.WriteLog("Удаление завершилось неудачей. \n" + ex.Message);
                log.Dispose();
                Console.WriteLine("Error...");
                Console.ReadKey();
                return;
            }
            foreach (string elt in removedBookIDs)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Запись {0} удалена из индекса", elt);
                log.WriteLog(sb.ToString());
            }

            //теперь добавляем изменённые

            IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
            LitresVuFindConverter converter = new LitresVuFindConverter();
            foreach (XElement elt in UpdatedBooks)
            {
                VufindDoc doc = converter.CreateVufindDoc(elt);
                try
                {
                    litres.AddToIndex(doc);
                }
                catch (Exception ex)
                {
                    log.WriteLog("Добавление в индекс завершилось неудачей.  \n" + ex.Message+ " Запись: "+doc.id);
                    log.Dispose();
                    Console.WriteLine("Error...");
                    Console.ReadKey();
                }
            }
            //writer.FinishWriting();




            log.WriteLog("Завершено...");
            log.Dispose();
            Console.WriteLine("Done...");
            Console.ReadKey();

        }
    }
}
