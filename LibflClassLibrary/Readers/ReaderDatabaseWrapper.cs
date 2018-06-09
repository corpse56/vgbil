using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.QueriesText;
using ExportBJ_XML.classes;
using System.Data;
using System.Data.SqlClient;
using LibflClassLibrary.ExportToVufind.classes.DB;

namespace LibflClassLibrary.Readers
{
    class ReaderDatabaseWrapper : DatabaseWrapper
    {

        public Reader ReaderQueries;
        public ReaderDatabaseWrapper()
        {
            ReaderQueries = new Reader();
        }

        internal DataTable GetReader(int Id)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.GET_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("Id", SqlDbType.Int).Value = Id;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
        internal DataTable IsFiveElBooksIssued(int Id)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.IS_FIVE_ELBOOKS_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("Id", SqlDbType.Int).Value = Id;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
    }
}
