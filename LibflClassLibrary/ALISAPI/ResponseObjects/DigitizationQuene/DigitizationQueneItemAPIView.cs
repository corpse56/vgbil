using LibflClassLibrary.ALISAPI.ResponseObjects.DigitizationQuene;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.DigitizationQuene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ALISAPI.ResponseObjects.DigitizationQuene
{
    public class DigitizationQueneItemAPIView
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime? Done { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Deleted { get; set; }
        public string ReasonForDeletion { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public static DigitizationQueneItemAPIView GetView(DigitizationQueneItemInfo item)
        {
            DigitizationQueneItemAPIView result = new DigitizationQueneItemAPIView();
            string fund = GetBaseNameByBaseId(item.baza);
            result.BookId = $"{fund}_{item.idMain.ToString()}";
            result.Author = BJBookInfo.GetFieldValue(fund, item.idMain, 700, "$a");
            result.Title = BJBookInfo.GetFieldValue(fund, item.idMain, 200, "$a");
            result.Deleted = item.delDate;
            result.ReasonForDeletion = item.delCause;
            result.Done = item.done;
            result.Id = item.id;
            result.IsDeleted = item.deleted;
            result.ReaderId = item.ReaderId;
            return result;

        }
        public static string GetBaseNameByBaseId(BAZA baseId)
        {
            switch (baseId)
            {
                case BAZA.BJVVV:
                    return "BJVVV";
                case BAZA.REDKOSTJ:
                    return "REDKOSTJ";
                case BAZA.BJACC:
                    return "BJACC";
                case BAZA.BJFCC:
                    return "BJFCC";
                case BAZA.BJSCC:
                    return "BJSCC";
                default:
                    return "Unknown";
            }
        }

    }

}
