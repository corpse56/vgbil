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

            using (Log log = new Log())
            {
                log.WriteLog("Начало инкрементной загрузки Litres...");
                Console.WriteLine("Начало инкрементной загрузки Litres...");
                LitresVufindIndexUpdater litres = new LitresVufindIndexUpdater();

                //получаем инкремент
                XDocument IncrementXML = new XDocument();
                try
                {
                    IncrementXML = litres.GetCurrentIncrement();
                }
                catch (Exception ex)
                {
                    log.WriteLog("Загрузка инкремента завершилось неудачей. " + ex.Message);
                    Console.WriteLine("Error...");
                    Console.ReadKey();
                    return;
                }
                log.WriteLog("Загрузка инкремента Litres успешно завершена...");
                Console.WriteLine("Загрузка инкремента Litres успешно завершена...");

                //IncrementXML = XDocument.Load(@"f:\projects\LIBFL\VufindIncrementUpdate\bin\Debug\increment31.05.2018 01.10.xml");//для отладки 

                //вычленияем удалённые записи и удаляем их из индекса
                var removedBooks = IncrementXML.Descendants("removed-book");
                List<string> removedBookIDs = new List<string>();
                foreach (XElement elt in removedBooks)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                    removedBookIDs.Add(sb.ToString());
                }

                //вычленияем изменённые записи и тоже удаляем их из индекса. хотя по моему это необязятельно. если послать запись, которая уже есть, то она автоматически обновится. надо проверить
                //так и есть - это необязательно. экономим время и пропускаем этот шаг
                //removedBooks = IncrementXML.Descendants("updated-book");
                //foreach (XElement elt in removedBooks)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                //    removedBookIDs.Add(sb.ToString());
                //}

                try
                {
                    litres.DeleteFromIndex(removedBookIDs);//удаляем сразу все одним запросом. 
                }
                catch (Exception ex)
                {
                    log.WriteLog("Удаление завершилось неудачей. \n" + ex.Message);
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

                //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
                IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
                LitresVuFindConverter converter = new LitresVuFindConverter();
                foreach (XElement elt in UpdatedBooks)
                {
                    VufindDoc doc = converter.CreateVufindDoc(elt);
                    try
                    {
                        litres.AddToIndex(doc);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendFormat("Запись {0} обновлена. ", doc.id);
                        log.WriteLog(sb.ToString());

                    }
                    catch (Exception ex)
                    {
                        log.WriteLog("Добавление в индекс завершилось неудачей.  \n" + ex.Message + " Запись: " + doc.id);
                        Console.WriteLine("Error...");
                        Console.ReadKey();
                        return;
                    }
                }

                litres.SetLastIncrementDate("Litres");

                log.WriteLog("Завершено.");
                Console.WriteLine("Done.");
                Console.ReadKey();
            }

        }
    }
}
