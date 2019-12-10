using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Books;

namespace LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader
{
    class BJCirculationManager : CirculationManager
    {
        
        public BJCirculationManager()
        {
            exemplarRecieverFromReader = new BJExemplarRecieverFromReader();
        }

    }
}