using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationQueries
    {
        public string GET_BASKET
        {
            get
            {
                return "select * from Circulation..Basket where ReaderId = @ReaderId";
            }
        }

    }
}
