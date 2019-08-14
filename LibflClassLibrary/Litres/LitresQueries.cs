using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Litres
{
    class LitresQueries
    {
        public string GET_LITRES_ACCOUNT
        {
            get
            {
                return " select * from LITRES..ACCOUNTS where IDREADER = @ReaderId";
            }
        }

        public string ASSIGN_LITRES_ACCOUNT
        {
            get
            {
                return " update LITRES..ACCOUNTS set IDREADER = @ReaderId, ASSIGNED=getdate() where ID = @AccountId ";
            }
        }

        public string GET_FIRST_FREE_LITRES_ACCOUNT
        {
            get
            {
                return " select top 1 ID from LITRES..ACCOUNTS where IDREADER is null order by ID ";
            }
        }

    }
}
