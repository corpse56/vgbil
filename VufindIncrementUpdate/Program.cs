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
                Console.WriteLine("Начинаю удаление изъятых записей из индекса...");
                log.WriteLog("Начинаю удаление изъятых записей из индекса...");

                try
                {
                    litres.DeleteFromIndex(removedBookIDs);//удаляем сразу все одним запросом. 
                }
                catch (Exception ex)
                {
                    log.WriteLog("Удаление завершилось неудачей. \n" + ex.Message);
                    Console.WriteLine("Удаление завершилось неудачей. \n" + ex.Message);
                    Console.ReadKey();
                    return;
                }
                foreach (string elt in removedBookIDs)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Запись {0} удалена из индекса", elt);
                    Console.WriteLine(sb.ToString());
                    log.WriteLog(sb.ToString());
                }
                Console.WriteLine("Начинаю обновление записей...");
                log.WriteLog("Начинаю обновление записей...");

                //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
                IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
                LitresVuFindConverter converter = new LitresVuFindConverter();
                List<VufindDoc> UpdatedBooksList = new List<VufindDoc>();
                VufindDoc doc;
                foreach (XElement elt in UpdatedBooks)
                {
                    doc = converter.CreateVufindDoc(elt);
                    UpdatedBooksList.Add(doc);
                }

                    
                try
                {
                    litres.AddToIndex(UpdatedBooksList);
                }
                catch (Exception ex)
                {
                    log.WriteLog("Добавление в индекс завершилось неудачей.  \n" + ex.Message);
                    Console.WriteLine("Error...");
                    Console.ReadKey();
                    return;
                }
                foreach (VufindDoc vfdoc in UpdatedBooksList)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Запись {0} обновлена. ", vfdoc.id);
                    Console.WriteLine(sb.ToString());
                    log.WriteLog(sb.ToString());
                }

                litres.SetLastIncrementDate("Litres");

                log.WriteLog("Завершено.");
                Console.WriteLine("Завершено.");
            }
            Console.ReadKey();
        }
    }
}
