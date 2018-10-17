using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Readers;

namespace LibflClassLibrary.Books.BJBooks.DB
{
    public class BJDatabaseWrapper : DatabaseWrapper
    {

        public string Fund { get; set; }
        public string AFTable { get; set; }
        public Bibliojet BJQueries;

        public BJDatabaseWrapper(string fund)
        {
            this.Fund = fund;
            BJQueries = new Bibliojet(fund);
        }
       

        public DataTable GetBJRecord(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.SELECT_RECORD_QUERY, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int ).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetIncrementUpdate()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_INCREMENT_UPDATE_QUERY, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetIncrementCovers()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_INCREMENT_COVERS_QUERY, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetIncrementDeleted()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_INCREMENT_DELETED_QUERY, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_10a(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_10a, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_517a(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_517a, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_1(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_205a_1, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_2(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_205a_2, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_205a_3(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_205a_3, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetBookAuthorRights(int IDMAIN,int IDProject)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.BOOK_AUTHOR_RIGHTS, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("IDProject", SqlDbType.Int).Value = IDProject;
                return this.ExecuteSelectQuery(dataAdapter);
            }

        }

        internal DataTable Clarify_606a(int idchain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_606a, connection);
                dataAdapter.SelectCommand.Parameters.Add("idchain", SqlDbType.Int).Value = idchain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetIdDataOfAllExemplars(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_IDDATA_OF_ALL_EXEMPLARS, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetExemplar(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_EXEMPLAR, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
        
        internal DataTable GetExemplar(string InventoryNumber)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_EXEMPLAR_BY_INVENTORY_NUMBER, connection);
                dataAdapter.SelectCommand.Parameters.Add("inv", SqlDbType.NVarChar).Value = InventoryNumber;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetHyperLink(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_HYPERLINK, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetBookScanInfo(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_BOOK_SCAN_INFO, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("idbase", SqlDbType.Int);
                if (this.Fund == "BJVVV")
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 1;
                }
                else if (this.Fund == "REDKOSTJ")
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 2;
                }
                else
                {
                    dataAdapter.SelectCommand.Parameters["idbase"].Value = 0;
                }
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetAllIdmainWithImages()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_ALL_IDMAIN_WITH_IMAGES, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetImage(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_IMAGE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetParentIDMAIN(int ParentPIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_PARENT_PIN, connection);
                dataAdapter.SelectCommand.Parameters.Add("ParentPIN", SqlDbType.Int).Value = ParentPIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetMaxIDMAIN()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_MAX_IDMAIN, connection);
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetTitle(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_TITLE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetAFAllValues(string AFTable, int AFLinkId)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                BJQueries.AFTable = AFTable;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_AF_ALL_VALUES, connection);
                dataAdapter.SelectCommand.Parameters.Add("AFLinkId", SqlDbType.Int).Value = AFLinkId;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetRTF(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_RTF, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }



        internal DataTable IsAlligat(int IDDATA)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ALLIGAT, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = IDDATA;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable Clarify_101a(int IDINLIST)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IMPORT_CLARIFY_101a, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDINLIST", SqlDbType.Int).Value = IDINLIST;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
        internal DataTable IsIssuedOrOrderedEmployee(int IDMAIN, int IDDATA)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ISSUED_OR_ORDERED_EMPLOYEE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = IDDATA;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }
        internal DataTable IsSelfIssuedOrOrderedEmployee(int IDDATA, int IDMAIN, int IdReader)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_SELF_ISSUED_OR_ORDERED_EMPLOYEE, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = IDDATA;
                dataAdapter.SelectCommand.Parameters.Add("idreader", SqlDbType.Int).Value = IdReader;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable IsIssuedToReader(int iddata)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ISSUED_TO_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("iddata", SqlDbType.Int).Value = iddata;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetEmployeeStatus(int idmain)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_EMPLOYEE_STATUS, connection);
                dataAdapter.SelectCommand.Parameters.Add("idmain", SqlDbType.Int).Value = idmain;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetLastIncrementDate()
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_LAST_INCREMENT_DATE, connection);
                dataAdapter.SelectCommand.Parameters.Add("base", SqlDbType.NVarChar).Value = this.Fund;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal void SetLastIncrementDate(DateTime LastIncrement)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(BJQueries.SET_LAST_INCREMENT_DATE, connection);
                command.Parameters.Add("LastIncrement", SqlDbType.DateTime).Value = LastIncrement;
                command.Parameters.Add("base", SqlDbType.NVarChar).Value = this.Fund;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        internal DataTable GetBusyElectronicExemplarCount(int Id)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_BUSY_ELECTRONIC_EXEMPLAR_COUNT, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = Id;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetNearestFreeDateForElectronicIssue(int Id)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_NEAREST_FREE_DATE_FOR_ELECTRONIC_ISSUE, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = Id;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable IsOneDayPastAfterReturn(int IDMAIN, int IDREADER)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ONE_DAY_PAST_AFTER_RETURN, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                dataAdapter.SelectCommand.Parameters.Add("IDREADER", SqlDbType.Int).Value = IDREADER;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable IsElectronicCopyIsuuedToReader(int IDMAIN, int IDREADER)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ELECTRONIC_COPY_ISSUED_TO_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDREADER", SqlDbType.Int).Value = IDREADER;
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable GetElectronicViewKeyForReader(int IDMAIN, int IDREADER)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.GET_ELECTRONIC_VIEWKEY_FOR_READER, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDREADER", SqlDbType.Int).Value = IDREADER;
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal DataTable IsElectronicCopyIssued(int IDMAIN)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(BJQueries.IS_ELECTRONIC_COPY_ISSUED, connection);
                dataAdapter.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                return this.ExecuteSelectQuery(dataAdapter);
            }
        }

        internal void IssueElectronicCopyToReader(int IDMAIN, int IssuePeriodDays, string ViewKey, int IDREADER, TypeReader ReaderType)
        {
            string connectionString = AppSettings.ConnectionString;
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(BJQueries.ISSUE_ELECTRONIC_COPY_TO_READER, connection);
                command.Parameters.Add("IDMAIN", SqlDbType.Int).Value = IDMAIN;
                command.Parameters.Add("IssuePeriodDays", SqlDbType.Int).Value = IssuePeriodDays;
                command.Parameters.Add("ViewKey", SqlDbType.NVarChar).Value = ViewKey;
                command.Parameters.Add("IDREADER", SqlDbType.Int).Value = IDREADER;
                command.Parameters.Add("DateReturn", SqlDbType.DateTime).Value = DateTime.Now.AddDays(IssuePeriodDays);
                command.Parameters.Add("ReaderType", SqlDbType.Int).Value = (int)ReaderType;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }        
    }
}
