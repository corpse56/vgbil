using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks.DB;

namespace LibflClassLibrary.Books.BJBooks.Loaders
{
    public class BJExemplarLoader
    {
        BJDatabaseWrapper dbWrapper;
        public BJExemplarLoader(string Fund)
        {
            this.Fund = Fund;
            dbWrapper = new BJDatabaseWrapper(Fund);
        }
        string Fund { get; set; }

        internal List<BJElectronicExemplarAvailabilityStatus> LoadAvailabilityStatuses(int IDMAIN, string Fund)
        {
            DataTable table = dbWrapper.LoadAvailabilityStatuses(IDMAIN, Fund);
            var listResult = new List<BJElectronicExemplarAvailabilityStatus>();
            var result = new BJElectronicExemplarAvailabilityStatus();
            foreach (DataRow row in table.Rows)
            {
                switch (row["CodeTypeProject"].ToString())
                {
                    case "v-stop":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vstop;
                        break;
                    case "v-free-view":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vfreeview;
                        break;
                    case "v-login-view":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vloginview;
                        break;
                    case "dlstop":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlstop;
                        break;
                    case "dlopen":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlopen;
                        break;
                    case "dlview":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlview;
                        break;
                    case "dllimit":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dllimit;
                        break;

                }
                switch ((int)row["IDProject"])
                {
                    case 1:
                        result.Project = BJElectronicAvailabilityProjects.VGBIL;
                        break;
                    case 2:
                        result.Project = BJElectronicAvailabilityProjects.NEB;
                        break;
                }
                listResult.Add(result);
            }
            return listResult;
        }
    }
}
