using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Circulation
{
    public class DB
    {
        public SqlDataAdapter DA;
        public DataSet DS;
        public static string BASENAME = "Reservation_R";
        public DB()
        {
            Circulation.DBWork.XmlConnections xml = new Circulation.DBWork.XmlConnections();
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(xml.GetBJVVVCon());
            DA.UpdateCommand = new SqlCommand();
            DA.UpdateCommand.Connection = new SqlConnection(xml.GetBJVVVCon());
            DA.InsertCommand = new SqlCommand();
            DA.InsertCommand.Connection = new SqlConnection(xml.GetBJVVVCon());
            
        }

    }
}
