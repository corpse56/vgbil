using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public enum CardType { 
        MAIN = 0, 
        PERIODICAL = 1, 
        SUBSCRIPT = 2,
        AV = 3
    }
    public class ImageCard
    {
        public CardType CardType;
        public Image MainSideImage;
        public string MainSideFileName;
        public Image SelectedSideImage;
        public int SelectedSideFileName;
        public Image SeparatorImage;
        public int SeparatorId;
        public int Closet;
        public int Box;
        public int CountSide;
        public string MainSideUrl;
        public string SelectedSideUrl;

        public static ImageCard GetCard(string cardId)
        {
            ICLoader loader = new ICLoader();
            return loader.GetCard(cardId);

        }
    }
}
