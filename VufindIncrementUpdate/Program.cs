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
using LibflClassLibrary.Books.BJBooks.DB;
using NLog;

namespace VufindIncrementUpdate
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {

            Program.BJIncrementUpdate("BJVVV");
            Program.BJIncrementUpdate("REDKOSTJ");
            Program.BJIncrementUpdate("BJACC");
            Program.BJIncrementUpdate("BJFCC");
            Program.BJIncrementUpdate("BJSCC");
            Program.LitresIncrementUpdate();


            //для отладки
            //Console.ReadKey();
        }

        static void BJIncrementUpdate(string Fund)
        {
            ///////////////////////////////////////////////////////////////////////
            //BJVuFindConverter converter1 = new BJVuFindConverter(Fund);

            //BJDatabaseWrapper wrapper1 = new BJDatabaseWrapper(Fund);
            //int IDMAIN1 = 909236;
            //DataTable BJRecord1 = wrapper1.GetBJRecord(IDMAIN1);
            //if (BJRecord1.Rows.Count == 0)
            //{
            //    return;
            //}
            //VufindDoc doc1 = converter1.CreateVufindDocument(IDMAIN1);
            ///////////////////////////////////////////////////////////////////////



            logger.Info("Начало инкрементной загрузки "+Fund+"...");
            Console.WriteLine("Начало инкрементной загрузки " + Fund + "...");
            BJVufindIndexUpdater bj = new BJVufindIndexUpdater(@"catalog.libfl.ru", Fund);

            //получаем инкремент
            List<IncrementStruct> Increment = new List<IncrementStruct>();
            List<IncrementStruct> IncrementCovers = new List<IncrementStruct>();
            try
            {
                Increment = (List<IncrementStruct>)bj.GetCurrentIncrement();
            }
            catch (Exception ex)
            {
                logger.Error("Загрузка инкремента завершилось неудачей. " + ex.Message);
                Console.WriteLine("Error...");
                //Console.ReadKey();
                return;
            }
            logger.Info("Загрузка инкремента " + Fund + " успешно завершена...");
            Console.WriteLine("Загрузка инкремента " + Fund + " успешно завершена...");

            //вычленияем удалённые записи и удаляем их из индекса
            List<IncrementStruct> RemovedBooks = Increment.FindAll(x => x.Flag == "deleted");
            List<IncrementStruct> UpdatedBooks = Increment.FindAll(x => x.Flag == "updated");
            List<IncrementStruct> CoverUpdatedBooks = Increment.FindAll(x => x.Flag == "cover");

            List<string> BooksToDeleteIds = new List<string>();
            foreach (IncrementStruct elt in Increment)
            {
                StringBuilder sb = new StringBuilder();
                if (elt.Flag != "cover")
                {
                    sb.AppendFormat("{0}", elt.Id);
                    BooksToDeleteIds.Add(sb.ToString());
                }
            }

            Console.WriteLine("Начинаю удаление изменённых/удалённых " + Fund + " записей из индекса...");
            logger.Info("Начинаю удаление изменённых/удалённых " + Fund + " записей из индекса...");

            try
            {
                bj.DeleteFromIndex(BooksToDeleteIds);//удаляем сразу все одним запросом. 
            }
            catch (Exception ex)
            {
                logger.Error("Удаление завершилось неудачей. \n" + ex.Message);
                Console.WriteLine("Удаление завершилось неудачей. \n" + ex.Message);
                //Console.ReadKey();
                return;
            }
            foreach (string elt in BooksToDeleteIds)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Запись {0} удалена из индекса", elt);
                Console.WriteLine(sb.ToString());
                logger.Info(sb.ToString());
            }

            //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
            Console.WriteLine("Начинаю обновление " + Fund + " изменённых записей...");
            logger.Info("Начинаю обновление " + Fund + " изменённых записей...");


            BJVuFindConverter converter = new BJVuFindConverter(Fund);
            List<VufindDoc> UpdatedBooksList = new List<VufindDoc>();
            VufindDoc doc;
            string tmp = "";
            //для отладки
            //UpdatedBooks = UpdatedBooks.Take(5).ToList();
            //doc = converter.CreateVufindDocument(1497354);

            try
            {
                foreach (IncrementStruct elt in UpdatedBooks)
                {
                    tmp = elt.Id;
                    BJDatabaseWrapper wrapper = new BJDatabaseWrapper(Fund);
                    int IDMAIN = int.Parse(elt.Id.Substring(elt.Id.IndexOf("_") + 1));
                    DataTable BJRecord = wrapper.GetBJRecord(IDMAIN);
                    if (IDMAIN == 1497354)//это битая запись
                    {
                        continue;
                    }
                    if (BJRecord.Rows.Count == 0)
                    {
                        continue;
                    }
                    doc = converter.CreateVufindDocument(IDMAIN);
                    if (doc == null)
                    {
                        continue;
                    }
                    UpdatedBooksList.Add(doc);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Формирование списка обновляемых записей завершилось неудачей.  \n" + ex.Message);
                Console.WriteLine("Error...");
            }


            try
            {
                bj.AddToIndex(UpdatedBooksList);
            }
            catch (Exception ex)
            {
                logger.Error("Добавление в индекс завершилось неудачей.  \n" + ex.Message);
                Console.WriteLine("Error...");
                //Console.ReadKey();
                return;
            }
            foreach (VufindDoc vfdoc in UpdatedBooksList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Запись {0} обновлена. ", vfdoc.id);
                Console.WriteLine(sb.ToString());
                logger.Info(sb.ToString());
            }


            Console.WriteLine("Начинаю обновление обложек " + Fund + "...");
            logger.Info("Начинаю обновление обложек " + Fund + "...");
            foreach (IncrementStruct elt in CoverUpdatedBooks)
            {
                //скачиваем обложечку
                try
                {
                    converter.ExportSingleCover(elt.Id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Скачивание  обложки " + elt.Id + " завершилось неудачей. " + ex.Message);
                    logger.Error("Скачивание  обложки " + elt.Id + " завершилось неудачей. " + ex.Message);
                    continue;
                }
                Console.WriteLine("Обложка  " + elt.Id + " скачана успешно. ");
                logger.Info("Обложка  " + elt.Id + " скачана успешно. ");
            }


            bj.SetLastIncrementDate(Fund);

            logger.Info("Завершено.");
            Console.WriteLine("Завершено.");
        }
        static void LitresIncrementUpdate()
        {
            logger.Info("Начало инкрементной загрузки Litres...");
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
                logger.Error("Загрузка инкремента завершилось неудачей. " + ex.Message);
                Console.WriteLine("Error...");
                //Console.ReadKey();
                return;
            }
            logger.Info("Загрузка инкремента Litres успешно завершена...");
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
            logger.Info("Начинаю удаление изъятых Litres записей из индекса...");

            try
            {
                litres.DeleteFromIndex(removedBookIDs);//удаляем сразу все одним запросом. 
            }
            catch (Exception ex)
            {
                logger.Error("Удаление завершилось неудачей. \n" + ex.Message);
                Console.WriteLine("Удаление завершилось неудачей. \n" + ex.Message);
                //Console.ReadKey();
                return;
            }
            foreach (string elt in removedBookIDs)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Запись Litres {0} удалена из индекса", elt);
                Console.WriteLine(sb.ToString());
                logger.Info(sb.ToString());
            }
            Console.WriteLine("Начинаю обновление обложек Litres...");
            logger.Info("Начинаю обновление обложек Litres...");

            //теперь добавляем новые и изменяем изменённые. Изменённые заменяться автоматически
            IEnumerable<XElement> UpdatedBooks = IncrementXML.Descendants("updated-book");
            LitresVuFindConverter converter = new LitresVuFindConverter("litres");
            List<VufindDoc> UpdatedBooksList = new List<VufindDoc>();
            VufindDoc doc;
            foreach (XElement elt in UpdatedBooks)
            {
                doc = converter.CreateVufindDocument(elt);
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
                    logger.Error("Скачивание Litres обложки " + elt.Attribute("id").Value + " завершилось неудачей. " + ex.Message);
                    continue;
                }
                Console.WriteLine("Обложка Litres " + elt.Attribute("id").Value + " скачана успешно. ");
                logger.Info("Обложка Litres " + elt.Attribute("id").Value + " скачана успешно. ");
            }

            Console.WriteLine("Начинаю обновление Litres записей...");
            logger.Info("Начинаю обновление Litres записей...");


            try
            {
                litres.AddToIndex(UpdatedBooksList);
            }
            catch (Exception ex)
            {
                logger.Info("Добавление в индекс завершилось неудачей.  \n" + ex.Message);
                Console.WriteLine("Error...");
                //Console.ReadKey();
                return;
            }
            foreach (VufindDoc vfdoc in UpdatedBooksList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Запись {0} обновлена. ", vfdoc.id);
                Console.WriteLine(sb.ToString());
                logger.Info(sb.ToString());
            }

            litres.SetLastIncrementDate("Litres");

            logger.Info("Завершено.");
            Console.WriteLine("Завершено.");
        }
    }
}
