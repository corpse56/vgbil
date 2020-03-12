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
        public static ICOrderInfo GetICOrderById(int id, bool loadImages)
        {
            ICOrderLoader loader = new ICOrderLoader();
            return loader.GetICOrderById(id, loadImages);
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

        public string GetBarString()
        {
            string result = this.Id.ToString();
            switch (result.Length)
            {
                case 1:
                    result = $"*M00000{result}*";
                    break;
                case 2:
                    result = $"*M0000{result}*";
                    break;
                case 3:
                    result = $"*M000{result}*";
                    break;
                case 4:
                    result = $"*M00{result}*";
                    break;
                case 5:
                    result = $"*M0{result}*";
                    break;
                case 6:
                    result = $"*M{result}*";
                    break;
            }
            return result;
        }
    }

}
