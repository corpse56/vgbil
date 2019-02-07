using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
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
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                case "REDKOSTJ":
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                case "BJACC":
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                case "BJFCC":
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                case "BJSCC":
                    result = ViewFactory.GetBJ(IDRecord, fund);
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private static BookSimpleView GetBJ(int IDMAIN, string fund)
        {
            BookSimpleView result = new BookSimpleView();
            BJVuFindConverter converter = new BJVuFindConverter(fund);
            VufindDoc vfDoc = converter.CreateVufindDocument(IDMAIN);
            if (vfDoc == null) return null;
            result.ID = vfDoc.id;
            result.Annotation = vfDoc.Annotation.ToString();
            result.Author = vfDoc.author.ToString();
            result.Fund = KeyValueMapping.FundCodeToRUSName[KeyValueMapping.FundENGToFundCode[fund]];
            result.Genre = vfDoc.genre.ToString();
            result.Language = vfDoc.language.ToString();
            result.PlaceOfPublication = vfDoc.PlaceOfPublication.ToString();
            result.PublishDate = vfDoc.publishDate.ToString();
            result.Publisher = vfDoc.publisher.ToString();
            result.Title = vfDoc.title.ToString();
            result.CoverURL = VuFindConverter.GetCoverExportPath(result.ID);            /////http://cdn.libfl.ru/covers/BJVVV/000/025/169/JPEG_AB/cover.jpg
            string LoginPath = @"\\" + AppSettings.IPAddressFileServer + @"\BookAddInf";
            using (new NetworkConnection(LoginPath, new NetworkCredential(AppSettings.LoginFileServerReadWrite, AppSettings.PasswordFileServerReadWrite)))
            {
                LoginPath += @"\" + fund.ToUpper() + @"\" + result.CoverURL + @"\cover.jpg";
                if (File.Exists(LoginPath))
                {
                    result.CoverURL = $"http://cdn.libfl.ru/{fund.ToUpper()}/{result.CoverURL.Replace("\\", "/")}cover.jpg";
                }
                else
                {
                    result.CoverURL = null;
                }
            }
            BJBookInfo bjBook = BJBookInfo.GetBookInfoByPIN(IDMAIN, fund);
            result.RTF = bjBook.RTF;

            result.Exemplars = GetBJExemplars(bjBook);
            result.IsExistsDigitalCopy = (bjBook.DigitalCopy == null) ? false : true;
            result.DigitalCopy = new DigitalCopySimpleView();
            if (result.IsExistsDigitalCopy)
            {
                BJElectronicExemplarInfo ElExemplar = new BJElectronicExemplarInfo(IDMAIN, fund);
                var Status = ElExemplar.Statuses.Find(x => x.Project == BJElectronicAvailabilityProjects.VGBIL);
                ExemplarSimpleView ExemplarView = new ExemplarSimpleView();
                if (Status.Code == BJElectronicExemplarAvailabilityCodes.vfreeview)
                {
                    result.DigitalCopy.DigitalAccess = "Свободный доступ";
                    ElExemplar.ExemplarAccess.Access = 1001;
                }
                else if (Status.Code == BJElectronicExemplarAvailabilityCodes.vloginview)
                {
                    result.DigitalCopy.DigitalAccess = "Доступ через личный кабинет";
                    ElExemplar.ExemplarAccess.Access = 1002;
                }
                else
                {
                    result.DigitalCopy.DigitalAccess = "Доступ запрещён";
                    ElExemplar.ExemplarAccess.Access = 1999;
                }
                ExemplarView.MethodOfAccess = "Удалённый доступ";
                ExemplarView.MethodOfAccessCode = 4002;
                ExemplarView.AccessCode = ElExemplar.ExemplarAccess.Access;
                ExemplarView.Access = KeyValueMapping.AccessCodeToNameALISVersion[ElExemplar.ExemplarAccess.Access];
                ExemplarView.ID = 0;
                ExemplarView.Barcode = "E00000000";
                ExemplarView.Carrier = "Электронная копия";
                ExemplarView.CarrierCode = 3011;
                ExemplarView.Location = "Электронный доступ";
                ExemplarView.LocationCode = 2030;
                result.Exemplars.Add(ExemplarView);
            }
            return result;
        }

        private static List<ExemplarSimpleView> GetBJExemplars(BJBookInfo bjBook)
        {
            //string fund = ID.Substring(0, ID.IndexOf("_"));
            //int IDRecord = int.Parse(ID.Substring(ID.LastIndexOf("_") + 1));
            List<ExemplarSimpleView> result = new List<ExemplarSimpleView>();
            foreach(BJExemplarInfo exemplar in bjBook.Exemplars)
            {
                ExemplarSimpleView ExemplarView = new ExemplarSimpleView();
                try
                {
                    if (exemplar.Fields["929$b"].MNFIELD != 0) continue;
                    ExemplarView.Barcode = exemplar.Fields["899$w"].ToString();
                    ExemplarView.Carrier = exemplar.Fields["921$a"].ToString();
                    //ExemplarView.CarrierCode = KeyValueMapping.CarrierNameToCode[ExemplarView.Carrier];
                    ExemplarView.ID = exemplar.IdData;
                    ExemplarView.InventoryNote = exemplar.Fields["899$x"].ToString();
                    ExemplarView.InventoryNumber = exemplar.Fields["899$p"].ToString();
                    ExemplarView.Location = KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()];
                    ExemplarView.LocationCode = KeyValueMapping.UnifiedLocationCode[ExemplarView.Location];
                    ExemplarView.MethodOfAccessCode = exemplar.ExemplarAccess.MethodOfAccess;
                    ExemplarView.MethodOfAccess = KeyValueMapping.MethodOfAccessCodeToName[ExemplarView.MethodOfAccessCode];
                    ExemplarView.AccessCode = exemplar.ExemplarAccess.Access;
                    ExemplarView.Access = KeyValueMapping.AccessCodeToName[ExemplarView.AccessCode];
                    ExemplarView.RackLocation = exemplar.Fields["899$c"].ToString();
                    ExemplarView.Status = exemplar.IsIssuedToReader() ? "Занято" : "Свободно";
                }
                catch
                {
                    //throw new Exception("B002");
                    continue;
                }
                result.Add(ExemplarView);
            }
            return result;
        }
    }
}
