using LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation.CirculationService
{
    class PeriodicCirculationManager : CirculationManager
    {
        public PeriodicCirculationManager()
        {
            exemplarRecieverFromReader = new PeriodicExemplarRecieverFromReader();
        }
    }
}
