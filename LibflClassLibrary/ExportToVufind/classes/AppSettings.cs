using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportBJ_XML.classes
{
    
    class AppSettings
    {
        static public string ConnectionString
        {
            get
            {
                string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";

                return connectionString;
            }
        }
    }
    
}
