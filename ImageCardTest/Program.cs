using LibflClassLibrary.BJUsers;
using LibflClassLibrary.ImageCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCardTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //ImageCardInfo card = ImageCardInfo.GetCard("000000001");
            //card.LoadImages();

            ImageCatalogCirculationManager manager = new ImageCatalogCirculationManager();
            BJUserInfo user = BJUserInfo.GetUserByLogin("bk", "BJVVV");
            user.SelectedUserStatus = user.UserStatus[0];
            ICOrderInfo order = ICOrderInfo.GetICOrderById(10, false);
            manager.RefuseOrder(order, user, "refuse111");
            order = ICOrderInfo.GetICOrderById(11, false);
            manager.RefuseOrder(order, user, "refuse222");
            order = ICOrderInfo.GetICOrderById(12, false);
            manager.RefuseOrder(order, user, "refuse333");


        }
    }
}
