using LibflClassLibrary.Circulation.DB;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.DigitizationQuene.DB
{
    class DigitizatioQueneDBWrapper
    {
        private DigitizationQueneQueries Queries;
        private string connectionString;
        public DigitizatioQueneDBWrapper()
        {
            connectionString = AppSettings.DevConnectionString;
            Queries = new DigitizationQueneQueries();
        }
        internal DataTable GetQuene()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_QUENE, connection);
                //dataAdapter.SelectCommand.Parameters.Add("OrderId", SqlDbType.Int).Value = OrderId;
                //dataAdapter.SelectCommand.Parameters.Add("RefusualStatusName", SqlDbType.NVarChar).Value = CirculationStatuses.Refusual.Value;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }
        internal DataTable GetCompleted()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_COMPLETED, connection);
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }
        internal DataTable GetDeleted()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.GET_DELETED, connection);
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable IsMoreThanTwoBooksReaderWantsToDigitizePer24Hour(int readerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_MORE_THAN_TWO_BOOKS_READER_WANTS_TO_DIGITIZE, connection);
                dataAdapter.SelectCommand.Parameters.Add("readerId", SqlDbType.Int).Value = readerId;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal DataTable IsQueneOverflowed()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.QUENE_OVERFLOW, connection);
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }

        internal void AddToQuene(int idBase, int idMain, int readerId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ReaderInfo reader = ReaderInfo.GetReader(readerId);
                SqlCommand command = new SqlCommand(Queries.ADD_TO_QUENE, connection);
                command.Parameters.Add("idMain", SqlDbType.Int).Value = idMain;
                command.Parameters.Add("idBase", SqlDbType.Int).Value = idBase;
                command.Parameters.Add("readerId", SqlDbType.Int).Value = readerId;
                command.Parameters.Add("isRemoteReader", SqlDbType.Bit).Value = reader.IsRemoteReader;
                connection.Open();
                command.ExecuteNonQuery();
            }

        }

        internal DataTable IsAlreadyInQuene(int idBase, int idMain)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            { 
                SqlDataAdapter dataAdapter = new SqlDataAdapter(Queries.IS_ALREADY_IN_QUENE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idMain", SqlDbType.Int).Value = idMain;
                dataAdapter.SelectCommand.Parameters.Add("idBase", SqlDbType.Int).Value = idBase;
                DataTable table = new DataTable();
                int cnt = dataAdapter.Fill(table);
                return table;
            }

        }
        


    }
}
