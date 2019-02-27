using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportBJUserRolesAndRights
{
    class Program
    {

        static void Main(string[] args)
        {
            PreparingBJVVV vvv = new PreparingBJVVV();
            vvv.Execute();
        //    PreparingBJACC acc = new PreparingBJACC();
        //    acc.Execute();
        }

    }
}
