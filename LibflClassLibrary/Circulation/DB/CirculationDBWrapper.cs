using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Circulation.DB
{

    class CirculationDBWrapper
    {
        private CirculationQueries Queries;
        private string ConnectionString;
        public CirculationDBWrapper()
        {
            ConnectionString = "Data Source=80.250.173.142;Initial Catalog=Circulation;Persist Security Info=True;User ID=demo;Password=demo;Connect Timeout=1200";
            Queries = new CirculationQueries();
        }

        internal DataTable GetBasket(int readerId)
        {
            string connectionString = this.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_BASKET, connection);
                dataAdapter.SelectCommand.Parameters.Add("ReaderId", SqlDbType.Int).Value = readerId;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }
        }
    }
}
