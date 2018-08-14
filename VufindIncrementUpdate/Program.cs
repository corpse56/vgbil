using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Utilities;
using System.Xml;
using System.Net;
using LibflClassLibrary.ExportToVufind.Litres;
using LibflClassLibrary.ExportToVufind.Vufind;
using LibflClassLibrary.ExportToVufind.BJ;
using System.Data;

namespace VufindIncrementUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program.LitresIncrementUpdate();
            Program.BJIncrementUpdate("BJVVV");


            //для отладки
            Console.ReadKey();
        }

        static void BJIncrementUpdate(string Fund)
        {
            using (Log log = new Log())
            {
                log.WriteLog("Начало инкрементной загрузки "+Fund+"...");
                Console.WriteLine("Начало инкрементной загрузки " + Fund + "...");
                BJVufindIndexUpdater bj = new BJVufindIndexUpdater(@"192.168.56.31", Fund);

                //получаем инкремент
                DataTable IncrementXML = new DataTable();
                try
                {
                    IncrementXML = (DataTable)bj.GetCurrentIncrement();
                }
                catch (Exception ex)
                {
                    log.WriteLog("Загрузка инкремента завершилось неудачей. " + ex.Message);
                    Console.WriteLine("Error...");
                    Console.ReadKey();
                    return;
                }
                log.WriteLog("Загрузка инкремента " + Fund + " успешно завершена...");
                Console.WriteLine("Загрузка инкремента " + Fund + " успешно завершена...");

                //для отладки 
                //IncrementXML = XDocument.Load(@"f:\litres_example.xml");//для отладки 

                //вычленияем удалённые записи и удаляем их из индекса
                //var removedBooks = IncrementXML.Descendants("removed-book");
                //List<string> removedBookIDs = new List<string>();
                //foreach (XElement elt in removedBooks)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                //    removedBookIDs.Add(sb.ToString());
                //}
                ////вычленияем удалённые записи по you_can_sell == 0
                //removedBooks = IncrementXML.Descendants("updated-book");
                //foreach (XElement elt in removedBooks)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    if (elt.Attribute("you_can_sell").Value == "0")
                //    {
                //        sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                //        removedBookIDs.Add(sb.ToString());
                //    }
                //}


                //вычленияем изменённые записи и тоже удаляем их из индекса. хотя по моему это необязятельно. если послать запись, которая уже есть, то она автоматически обновится. надо проверить
                //так и есть - это необязательно. экономим время и пропускаем этот шаг
                //removedBooks = IncrementXML.Descendants("updated-book");
                //foreach (XElement elt in removedBooks)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                //    removedBookIDs.Add(sb.ToString());
                //}
                Console.WriteLine("Начинаю удаление изъятых " + Fund + " записей из индекса...");
                log.WriteLog("Начинаю удаление изъятых " + Fund + " записей из индекса...");

                //try
                //{
                //    bj.DeleteFromIndex(removedBookIDs);//удаляем сразу все одним запросом. 
                //}
                //catch (Exception ex)
                //{
                //    log.WriteLog("Удаление завершилось неудачей. \n" + ex.Message);
                //    Console.WriteLine("Удаление завершилось неудачей. \n" + ex.Message);
                //    Console.ReadKey();
                //    return;
                //}
                //foreach (string elt in removedBookIDs)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Запись " + Fund + " {0} удалена из индекса", elt);
                //    Console.WriteLine(sb.ToString());
                //    log.WriteLog(sb.ToString());
                //}
                //Console.WriteLine("Начинаю обновление обложек " + Fund + "...");
                //log.WriteLog("Начинаю обновление обложек " + Fund + "...");

                //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
                //IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
                //LitresVuFindConverter converter = new LitresVuFindConverter("litres");
                //List<VufindDoc> UpdatedBooksList = new List<VufindDoc>();
                //VufindDoc doc;
                //foreach (XElement elt in UpdatedBooks)
                //{
                //    doc = converter.CreateVufindDoc(elt);
                //    if (doc == null)
                //    {
                //        continue;
                //    }
                //    UpdatedBooksList.Add(doc);
                //    //скачиваем обложечку
                //    try
                //    {
                //        converter.ExportSingleCover(elt);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("Скачивание " + Fund + " обложки " + elt.Attribute("id").Value + " завершилось неудачей. " + ex.Message);
                //        log.WriteLog("Скачивание " + Fund + " обложки " + elt.Attribute("id").Value + " завершилось неудачей. " + ex.Message);
                //        continue;
                //    }
                //    Console.WriteLine("Обложка " + Fund + " " + elt.Attribute("id").Value + " скачана успешно. ");
                //    log.WriteLog("Обложка " + Fund + " " + elt.Attribute("id").Value + " скачана успешно. ");
                //}

                //Console.WriteLine("Начинаю обновление " + Fund + " записей...");
                //log.WriteLog("Начинаю обновление " + Fund + " записей...");


                //try
                //{
                //    bj.AddToIndex(UpdatedBooksList);
                //}
                //catch (Exception ex)
                //{
                //    log.WriteLog("Добавление в индекс завершилось неудачей.  \n" + ex.Message);
                //    Console.WriteLine("Error...");
                //    Console.ReadKey();
                //    return;
                //}
                //foreach (VufindDoc vfdoc in UpdatedBooksList)
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.AppendFormat("Запись {0} обновлена. ", vfdoc.id);
                //    Console.WriteLine(sb.ToString());
                //    log.WriteLog(sb.ToString());
                //}

                //bj.SetLastIncrementDate(Fund);

                //log.WriteLog("Завершено.");
                //Console.WriteLine("Завершено.");
            }

        }
        static void LitresIncrementUpdate()
        {
            using (Log log = new Log())
            {
                log.WriteLog("Начало инкрементной загрузки Litres...");
                Console.WriteLine("Начало инкрементной загрузки Litres...");
                LitresVufindIndexUpdater litres = new LitresVufindIndexUpdater("catalog.libfl.ru");

                //получаем инкремент
                XDocument IncrementXML = new XDocument();
                try
                {
                    IncrementXML = (XDocument)litres.GetCurrentIncrement();
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

                //для отладки 
                //IncrementXML = XDocument.Load(@"f:\litres_example.xml");//для отладки 

                //вычленияем удалённые записи и удаляем их из индекса
                var removedBooks = IncrementXML.Descendants("removed-book");
                List<string> removedBookIDs = new List<string>();
                foreach (XElement elt in removedBooks)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                    removedBookIDs.Add(sb.ToString());
                }
                //вычленияем удалённые записи по you_can_sell == 0
                removedBooks = IncrementXML.Descendants("updated-book");
                foreach (XElement elt in removedBooks)
                {
                    StringBuilder sb = new StringBuilder();
                    if (elt.Attribute("you_can_sell").Value == "0")
                    {
                        sb.AppendFormat("Litres_{0}", elt.Attribute("id").Value);
                        removedBookIDs.Add(sb.ToString());
                    }
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
                Console.WriteLine("Начинаю удаление изъятых Litres записей из индекса...");
                log.WriteLog("Начинаю удаление изъятых Litres записей из индекса...");

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
                    sb.AppendFormat("Запись Litres {0} удалена из индекса", elt);
                    Console.WriteLine(sb.ToString());
                    log.WriteLog(sb.ToString());
                }
                Console.WriteLine("Начинаю обновление обложек Litres...");
                log.WriteLog("Начинаю обновление обложек Litres...");

                //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
                IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
                LitresVuFindConverter converter = new LitresVuFindConverter("litres");
                List<VufindDoc> UpdatedBooksList = new List<VufindDoc>();
                VufindDoc doc;
                foreach (XElement elt in UpdatedBooks)
                {
                    doc = converter.CreateVufindDoc(elt);
                    if (doc == null)
                    {
                        continue;
                    }
                    UpdatedBooksList.Add(doc);
                    //скачиваем обложечку
                    try
                    {
                        converter.ExportSingleCover(elt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Скачивание Litres обложки " + elt.Attribute("id").Value + " завершилось неудачей. " + ex.Message);
                        log.WriteLog("Скачивание Litres обложки " + elt.Attribute("id").Value + " завершилось неудачей. " + ex.Message);
                        continue;
                    }
                    Console.WriteLine("Обложка Litres " + elt.Attribute("id").Value + " скачана успешно. ");
                    log.WriteLog("Обложка Litres " + elt.Attribute("id").Value + " скачана успешно. ");
                }

                Console.WriteLine("Начинаю обновление Litres записей...");
                log.WriteLog("Начинаю обновление Litres записей...");


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

        }
    }
}
