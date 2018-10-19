using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BJBooks.BJExemplars
{
    public enum BJElectronicExemplarAvailabilityCodes
    {
        vstop = 0,
        vfreeview = 1,
        vloginview = 2,
        dlstop = 3,
        dlview = 4,
        dllimit = 5,
        dlopen = 6
    }
    public enum BJElectronicAvailabilityProjects
    {
        NEB = 1,
        VGBIL = 2
    }
    public class BJElectronicExemplarAvailabilityStatus
    {
        public BJElectronicExemplarAvailabilityCodes Code { get; set; }
        public BJElectronicAvailabilityProjects Project { get; set; }
    }
}
