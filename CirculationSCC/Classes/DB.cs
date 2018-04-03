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
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationSCC"));
            DA.UpdateCommand = new SqlCommand();
            DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationSCC"));
            DA.InsertCommand = new SqlCommand();
            DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationSCC"));
            
        }

    }
}
