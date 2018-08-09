using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using LibflClassLibrary.ExportToVufind;

namespace DataProviderAPI.Loaders
{
    /// <summary>
    /// Сводное описание для BaseLoader
    /// </summary>
    public abstract class BaseLoader
    {


        public BaseLoader()
        {

        }
        private SqlDataAdapter _da;
        //private 

        public DataTable SelectQuery(string queryText)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string connectionString = AppSettings.ConnectionString;
            
            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            //da.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = 
            da.Fill(result);
            return result;
        }

        public int UpdateQuery(string queryText)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string connectionString = AppSettings.ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(queryText, connection);
            connection.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            connection.Close();
            return affectedRows;
        }


        //SqlDataAdapter da;
        //SqlConnection 


    }
}