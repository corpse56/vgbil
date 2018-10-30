using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Linq;
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
            }
            return result;
        }

        private static BookSimpleView GetBJ(int IDMAIN, string fund)
        {
            BookSimpleView result = new BookSimpleView();
            BJVuFindConverter converter = new BJVuFindConverter(fund);
            VufindDoc vfDoc = converter.CreateVufindDocument(IDMAIN);
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
            result.CoverURL = $"http://cdn.libfl.ru/covers/{fund.ToUpper()}/{result.CoverURL.Replace("\\","/")}cover.jpg";
            BJBookInfo bjBook = BJBookInfo.GetBookInfoByPIN(IDMAIN, fund);

            result.Exemplars = GetBJExemplars(bjBook);
            result.IsExistsDigitalCopy = (bjBook.DigitalCopy == null) ? false : true;
            result.DigitalCopy = new DigitalCopySimpleView();
            if (result.IsExistsDigitalCopy)
            {
                BJElectronicExemplarInfo ElExemplar = new BJElectronicExemplarInfo(IDMAIN, fund);
                var Status = ElExemplar.Statuses.Find(x => x.Project == BJElectronicAvailabilityProjects.VGBIL);
                if (Status.Code == BJElectronicExemplarAvailabilityCodes.vfreeview)
                {
                    result.DigitalCopy.DigitalAccess = "Свободный доступ";
                }
                else if (Status.Code == BJElectronicExemplarAvailabilityCodes.vloginview)
                {
                    result.DigitalCopy.DigitalAccess = "Доступ через личный кабинет";
                }
                else
                {
                    result.DigitalCopy.DigitalAccess = "Доступ запрещён";
                }
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
                ExemplarView.Barcode = exemplar.Fields["899$w"].ToString();
                ExemplarView.Carrier = exemplar.Fields["921$a"].ToString();
                ExemplarView.CarrierCode = KeyValueMapping.CarrierNameToCode[ExemplarView.Carrier];
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
                ExemplarView.Status = "Свободно";
                result.Add(ExemplarView);
            }
            return result;
        }
    }
}
