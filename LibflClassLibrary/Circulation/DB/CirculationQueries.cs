using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{
    class CirculationQueries
    {

        public string INSERT_INTO_USER_BASKET
        {
            get
            {
                return "insert into Circulation..Basket (BookId,ReaderId,PutDate) values (@BookID,@IDReader, getdate())";
            }
        }

        public string GET_BASKET
        {
            get
            {
                return "select * from Circulation..Basket where ReaderId = @ReaderId";
            }
        }

        public string IS_EXISTS_IN_BASKET
        {
            get
            {
                return "select 1 from Circulation..Basket where ReaderId = @IDReader and BookId = @BookID";
            }
        }
        public string GET_ORDERS
        {
            get
            {
                return "select ID OrderId ,from Circulation..Orders ";
            }
        }

    }
}
