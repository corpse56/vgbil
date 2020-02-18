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
        public string SelectedSideUrl;
        public Image SelectedSideImage;
        public ImageCardInfo Card;
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
    }

}
