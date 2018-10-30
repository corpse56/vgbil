using LibflClassLibrary.Circulation.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation
{
    public class CirculationInfo
    {
        CirculationLoader loader;
        public CirculationInfo()
        {
            loader = new CirculationLoader();
        }
        public List<BasketInfo> GetBasket(int ReaderId)
        {
            return loader.GetBasket(ReaderId);
        }


    }
}
