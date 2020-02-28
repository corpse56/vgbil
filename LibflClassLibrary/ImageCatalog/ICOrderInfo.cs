using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public class ICOrderInfo
    {
        public int Id { get; set; }
        public string CardFileName { get; set; }
        public int SelectedCardSide { get; set; }
        public string Comment { get; set; }
        public int ReaderId { get; set; }
        public DateTime StartDate { get; set; }
        public string SelectedSideUrl { get; set; }
        public Image SelectedSideImage { get; set; }
        public ImageCardInfo Card;
        public string StatusName { get; set; }
        public string RefusualReason { get; set; }
        //public Image SelectedSideImage;
        //public string SelectedSideUrl;
        public static ICOrderInfo GetICOrderById(int id)
        {
            ICOrderLoader loader = new ICOrderLoader();
            return loader.GetICOrderById(id);
        }
        public static ICOrderInfo CreateOrder(string cardFileName, string selectedCardSide, int readerId, string comment)
        {
            ICOrderLoader loader = new ICOrderLoader();
            return loader.CreateOrder(cardFileName, selectedCardSide, readerId, comment);
        }
        public static string GetCardTypeString(CardType ct)
        {
            string result = "book";
            switch(ct)
            {
                case CardType.AV:
                    result = "cardav";
                    break;
                case CardType.MAIN:
                    result = "book";
                    break;
                case CardType.PERIODICAL:
                    result = "period";
                    break;
                case CardType.SUBSCRIPT:
                    result = "subscript";
                    break;
            }
            return result;
        }
    }

}
