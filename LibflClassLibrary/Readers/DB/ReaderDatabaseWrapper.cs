using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Readers.DB;
using LibflClassLibrary.ExportToVufind;

namespace LibflClassLibrary.Readers.DB
{
    class ReaderDatabaseWrapper : DatabaseWrapper
    {

        public Reader ReaderQueries;
        public ReaderDatabaseWrapper()
        {
            ReaderQueries = new Reader();
        }

        internal DataTable GetReaderByEmail(string Email)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.GET_READER_BY_EMAIL, connection);
                dataAdapter.SelectCommand.Parameters.Add("Email", SqlDbType.NVarChar).Value = Email;
                return this.ExecuteSelectQuery(dataAdapter);
            }
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

        internal DataTable GetReaderIdByOAuthToken(string token)
        {
            string connectionString = AppSettings.OauthMySqlConnectionString;
            DataTable table = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(ReaderQueries.GET_READER_ID_BY_OAUTH_TOKEN, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("token", token);
                dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable AuthorizeReaderWithNumberReader( int numberReader,  string password)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.AUTHORIZE_READER_WITH_NUMBERREADER, connection);
                dataAdapter.SelectCommand.Parameters.Add("Id", SqlDbType.Int).Value = numberReader;
                dataAdapter.SelectCommand.Parameters.Add("Password", SqlDbType.NVarChar).Value = password;
                return this.ExecuteSelectQuery(dataAdapter); 
            }
        }

        internal DataTable AuthorizeReaderWithEmail(string Email, string password)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.AUTHORIZE_READER_WITH_EMAIL, connection);
                dataAdapter.SelectCommand.Parameters.Add("Email", SqlDbType.NVarChar).Value = Email;
                dataAdapter.SelectCommand.Parameters.Add("Password", SqlDbType.NVarChar).Value = password;
                return this.ExecuteSelectQuery(dataAdapter); 
            }
        }

        internal DataTable GetReaderRights(int NumberReader)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(ReaderQueries.GET_READER_RIGHTS, connection);
                dataAdapter.SelectCommand.Parameters.Add("NumberReader", SqlDbType.Int).Value = NumberReader;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

    }
}
