using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Litres
{
    class LitresLoader
    {
        LitresDBWrapper dbWrapper = new LitresDBWrapper();
        internal LitresInfo GetLitresAccount(int readerId)
        {
            DataTable table = dbWrapper.GetLitresAccount(readerId);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            LitresInfo result = new LitresInfo();
            result.Login = table.Rows[0]["LRLOGIN"].ToString();
            result.Password = table.Rows[0]["LRPWD"].ToString();
            return result;
        }

        internal void AssignLitresAccount(int readerId)
        {
            dbWrapper.AssignLitresAccount(readerId);

        }

    }
}
