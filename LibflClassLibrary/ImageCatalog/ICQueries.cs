using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    class ICQueries
    {
        string tableName_;
        public ICQueries(string tableName)
        {
            this.tableName_ = tableName;
        }
        public string GET_CARD
        {
            get
            {
                return $" select * from ImageCatalog..{tableName_} A " +
                        " where A.MNFIELD = 10 and A.MSFIELD = '$b' and A.IDDATA = @iddata";
            }
        }

    }
}
