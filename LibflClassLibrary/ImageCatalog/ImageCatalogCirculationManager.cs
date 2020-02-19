using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public class ImageCatalogCirculationManager
    {
        public List<ICOrderInfo> GetActiveOrdersByReader(int readerId)
        {
            ICOrderLoader loader = new ICOrderLoader();
            return loader.GetActiveOrdersByReader(readerId);
        }
    }
}
