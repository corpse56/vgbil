using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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
    public class ImageCardInfo
    {
        public int CardId;
        public string CardFileName;
        public CardType CardType;
        public Image MainSideImage;
        public string MainSideFullFileName;
        public Image SeparatorImage;
        public int SeparatorId;
        public int Closet;
        public int Box;
        public int CountSide;
        public string MainSideUrl;
        public static ImageCardInfo GetCard(string cardFileName, bool isNeedToLoadImages)
        {
            ICLoader loader = new ICLoader();
            return loader.GetCard(cardFileName, isNeedToLoadImages);
        }

    }
}
