using LibflClassLibrary.Books;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.Books
{
    public class ViewFactory
    {
        public static BookSimpleView GetBookSimpleView(string ID)
        {
            string fund = ID.Substring(0, ID.IndexOf("_"));
            int IDRecord = int.Parse(ID.Substring(ID.LastIndexOf("_")+1));
            BookSimpleView result = new BookSimpleView();
            switch (fund)
            {
                case "BJVVV":
                case "REDKOSTJ":
                case "BJACC":
                case "BJFCC":
                case "BJSCC":
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                case "PERIOD":
                    result = ViewFactory.GetPeriodic(IDRecord);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static BookSimpleView GetPeriodic(int iDRecord)
        {
            throw new NotImplementedException();
        }

        public static BookSimpleView GetBookSimpleViewWithAvailabilityStatus(string ID)
        {
            BookSimpleView result = GetBookSimpleView(ID);
            
            if (result == null) return null;
            CirculationInfo ci = new CirculationInfo();
            bool IsAvailable = false;
            foreach (ExemplarSimpleView e in result.Exemplars)
            {
                //здесь прямо если открытый доступ, то эвэлибл
                if (e.AccessCode == 1001)
                {
                    e.AvailabilityStatus = "Available";
                    IsAvailable = true;
                }
                else
                {
                    if (e.CarrierCode == 3011)
                    {
                        BJBookInfo book = BJBookInfo.GetBookInfoByPIN(ID);
                        int formula = (result.Exemplars.Count == 1) ? result.Exemplars.Count - ci.GetBusyExemplarsCount(book) : result.Exemplars.Count - ci.GetBusyExemplarsCount(book) - 1;

                        e.AvailabilityStatus = (formula <= 0) ? "Unavailable" : "Available";
                    }
                    else
                    {
                        e.AvailabilityStatus = ci.GetExemplarAvailabilityStatus(e.ID, BJBookInfo.GetFund(ID));
                    }
                }
                if (e.AvailabilityStatus == "Available")
                {
                    IsAvailable = true;
                }
            }

            result.AvailabilityStatus = IsAvailable ? "Available" : "Unavailable";

            return result;

        }

        private static BookSimpleView GetBJ(int IDMAIN, string fund)
        {
            BookSimpleView result = new BookSimpleView();
            BJBookInfo bjBook = BJBookInfo.GetBookInfoByPIN(IDMAIN, fund);
            result.ID = bjBook.Id;
            result.Annotation = bjBook.Fields["330$a"].ToString();//vfDoc.Annotation.ToString();
            result.Author = bjBook.Fields["700$a"].ToString(); //vfDoc.author.ToString();
            result.Fund = KeyValueMapping.FundCodeToRUSName[KeyValueMapping.FundENGToFundCode[fund]];
            result.Genre = bjBook.Fields["922$e"].ToString(); //vfDoc.genre.ToString();
            result.Language = bjBook.Fields["101$a"].ToString();//vfDoc.language.ToString();
            result.PlaceOfPublication = bjBook.Fields["210$a"].ToString();//vfDoc.PlaceOfPublication.ToString();
            result.PublishDate = bjBook.Fields["2100$d"].ToString();//vfDoc.publishDate.ToString();
            result.Publisher = bjBook.Fields["210$c"].ToString();//vfDoc.publisher.ToString();
            result.Title = bjBook.Fields["200$a"].ToString(); //vfDoc.title.ToString();
            result.CoverURL = VuFindConverter.GetCoverExportPath(result.ID);            /////http://cdn.libfl.ru/covers/BJVVV/000/025/169/JPEG_AB/cover.jpg

            //провекра существования картинки на сервере занимает много времени
            //поэтому просто передаём ссылку, но неизвестно есть ли там обложка или нет. надо сказать, что неизвестно есть ли обложка или нет...
            result.CoverURL = $"http://cdn.libfl.ru/{fund.ToUpper()}/{result.CoverURL.Replace("\\", "/")}cover.jpg";


            result.RTF = bjBook.RTF;

            result.Exemplars = GetBJExemplars(bjBook);
            result.IsExistsDigitalCopy = (bjBook.DigitalCopy == null) ? false : true;
            
            if (result.IsExistsDigitalCopy)
            {
                BJElectronicExemplarInfo ElExemplar = new BJElectronicExemplarInfo(IDMAIN, fund);
                ExemplarSimpleView ExemplarView = new ExemplarSimpleView();
                ExemplarView.MethodOfAccess = "Удалённый доступ";
                ExemplarView.MethodOfAccessCode = 4002;
                ExemplarView.AccessCode = ElExemplar.AccessInfo.Access;
                ExemplarView.Access = KeyValueMapping.AccessCodeToNameALISVersion[ElExemplar.AccessInfo.Access];
                ExemplarView.ID = 0;
                ExemplarView.Barcode = "E00000000";
                ExemplarView.Carrier = "Электронная копия";
                ExemplarView.CarrierCode = 3011;
                ExemplarView.Location = "Электронный доступ";
                ExemplarView.LocationCode = 2030;
                ExemplarView.BookUrl = (ExemplarView.AccessCode == 1001) ? @"http://catalog.libfl.ru/Bookreader/Viewer?bookID=" + ElExemplar.BookId + "&view_mode=HQ" : null;
                result.Exemplars.Add(ExemplarView);
            }
            return result;
        }

        public static ElectronicCopyFullView GetElectronicCopyFullView(string BookId)
        {
            ElectronicCopyFullView electronicCopyFullView = new ElectronicCopyFullView();
            BJElectronicExemplarInfo exemplar;
            exemplar = new BJElectronicExemplarInfo(BookBase.GetPIN(BookId),BookBase.GetFund(BookId));
            exemplar.FillFileFields();
            electronicCopyFullView.AccessCode = exemplar.AccessInfo.Access;
            electronicCopyFullView.HeightFirstFile = exemplar.HeightFirstFile;
            electronicCopyFullView.IsExistsHQ = exemplar.IsExistsHQ;
            electronicCopyFullView.IsExistsLQ = exemplar.IsExistsLQ;
            electronicCopyFullView.JPGFiles = exemplar.JPGFiles;
            electronicCopyFullView.MethodOfAccessCode = exemplar.AccessInfo.MethodOfAccess;
            electronicCopyFullView.Path_Cover = exemplar.Path_Cover;
            electronicCopyFullView.Path_HQ = exemplar.Path_HQ;
            electronicCopyFullView.Path_LQ = exemplar.Path_LQ;
            electronicCopyFullView.WidthFirstFile = exemplar.WidthFirstFile;

            return electronicCopyFullView;
        }

        private static List<ExemplarSimpleView> GetBJExemplars(BJBookInfo bjBook)
        {
            //string fund = ID.Substring(0, ID.IndexOf("_"));
            //int IDRecord = int.Parse(ID.Substring(ID.LastIndexOf("_") + 1));
            List<ExemplarSimpleView> result = new List<ExemplarSimpleView>();
            foreach(BJExemplarInfo exemplar in bjBook.Exemplars)
            {
                ExemplarSimpleView ExemplarView = GetExemplarSimpleView(exemplar);
                if (ExemplarView == null) continue;
                result.Add(ExemplarView);
            }
            return result;
        }
        public static ExemplarSimpleView GetExemplarSimpleView(BJExemplarInfo exemplar)
        {
            ExemplarSimpleView ExemplarView = new ExemplarSimpleView();
            try
            {
                if (exemplar.Fields["929$b"].HasValue)
                {
                    if (exemplar.Fields["921$c"].ToLower() == "списано")
                    {
                        return null;
                    }
                }
                ExemplarView.Barcode = exemplar.Fields["899$w"].ToString();
                ExemplarView.Carrier = exemplar.Fields["921$a"].ToString();
                //ExemplarView.CarrierCode = KeyValueMapping.CarrierNameToCode[ExemplarView.Carrier];
                ExemplarView.ID = Convert.ToInt32(exemplar.Id);
                ExemplarView.InventoryNote = exemplar.Fields["899$x"].ToString();
                ExemplarView.InventoryNumber = exemplar.Fields["899$p"].ToString();
                ExemplarView.Location = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];
                ExemplarView.LocationCode = KeyValueMapping.UnifiedLocationCode[ExemplarView.Location];
                ExemplarView.MethodOfAccessCode = exemplar.AccessInfo.MethodOfAccess;
                ExemplarView.MethodOfAccess = KeyValueMapping.MethodOfAccessCodeToName[ExemplarView.MethodOfAccessCode];
                ExemplarView.AccessCode = exemplar.AccessInfo.Access;
                ExemplarView.Access = KeyValueMapping.AccessCodeToName[ExemplarView.AccessCode];
                ExemplarView.RackLocation = exemplar.Fields["899$c"].ToString();
                //ExemplarView.Status = exemplar.IsIssuedToReader() ? "Занято" : "Свободно";
            }
            catch
            {
                //throw new Exception("B002");
                //continue;
                return null;
            }
            return ExemplarView;
        }
    }
}
