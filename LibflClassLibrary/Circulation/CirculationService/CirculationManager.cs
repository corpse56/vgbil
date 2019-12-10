using LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Circulation.CirculationService
{
    public abstract class CirculationManager
    {
        public IExemplarRecieverFromReader exemplarRecieverFromReader;

    }
}
