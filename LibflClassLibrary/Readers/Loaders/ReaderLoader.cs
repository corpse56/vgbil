using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.Readers.DB;
using LibflClassLibrary.Readers.ReadersRight;
using Utilities;

namespace LibflClassLibrary.Readers.Loaders
{
    class ReaderLoader
    {
        public ReaderLoader() { }
        public ReaderInfo LoadReader(string Email)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.GetReaderByEmail(Email);
            if (table.Rows.Count != 0)
            {
                return this.LoadReader(Convert.ToInt32(table.Rows[0][0]));
            }
            else
            {
                return null;
            }
        }

        public ReaderInfo LoadReader(int Id)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.GetReader(Id);
            ReaderInfo reader = new ReaderInfo();
            if (table.Rows.Count == 0)
            {
                throw new Exception("Читатель не найден");
            }
            DataRow row = table.Rows[0];
            foreach (DataColumn col in table.Columns)
            {
                switch (col.ColumnName)
                {
                    case "NumberReader":
                        reader.NumberReader = (int)row[col];
                        break;
                    case "BarCode":
                        reader.BarCode = "R" + row[col].ToString();
                        break;
                    case "DateBirth":
                        reader.DateBirth = (DateTime)row[col];
                        break;
                    case "DateRegistration":
                        reader.DateRegistration = (DateTime)row[col];
                        break;
                    case "DateReRegistration":
                        reader.DateReRegistration = (DateTime)row[col];
                        break;
                    case "Email":
                        reader.Email = row[col].ToString();
                        break;
                    case "FamilyName":
                        reader.FamilyName = row[col].ToString();
                        break;
                    case "Name":
                        reader.Name = row[col].ToString();
                        break;
                    case "FatherName":
                        reader.FatherName = row[col].ToString();
                        break;
                    case "MobileTelephone":
                        reader.MobileTelephone = row[col].ToString();
                        break;
                    case "TypeReader":
                        reader.TypeReader = ((bool)row[col] == true) ? TypeReader.Remote : TypeReader.Local;
                        reader.IsRemoteReader = (reader.TypeReader == TypeReader.Remote) ? true : false;//для апи
                        break;
                    case "WordReg":
                        reader.Salt = row[col].ToString();
                        break;
                    case "Password":
                        reader.HashedPassword = row[col].ToString();
                        break;
                    case "RegistrationCountry":
                        reader.RegistrationCountry = (int)row[col];
                        break;
                    case "RegistrationRegion":
                        reader.RegistrationRegion = row[col].ToString();
                        break;
                    case "RegistrationProvince":
                        reader.RegistrationProvince = row[col].ToString();
                        break;
                    case "RegistrationDistrict":
                        reader.RegistrationDistrict = row[col].ToString();
                        break;
                    case "RegistrationCity":
                        reader.RegistrationCity = row[col].ToString();
                        break;
                    case "RegistrationStreet":
                        reader.RegistrationStreet = row[col].ToString();
                        break;
                    case "RegistrationHouse":
                        reader.RegistrationHouse = row[col].ToString();
                        break;
                    case "RegistrationFlat":
                        reader.RegistrationFlat = row[col].ToString();
                        break;
                    case "LiveRegion":
                        reader.LiveRegion = row[col].ToString();
                        break;
                    case "LiveProvince":
                        reader.LiveProvince = row[col].ToString();
                        break;
                    case "LiveDistrict":
                        reader.LiveDistrict = row[col].ToString();
                        break;
                    case "LiveCity":
                        reader.LiveCity = row[col].ToString();
                        break;
                    case "LiveStreet":
                        reader.LiveStreet = row[col].ToString();
                        break;
                    case "LiveHouse":
                        reader.LiveHouse = row[col].ToString();
                        break;
                    case "LiveFlat":
                        reader.LiveFlat = row[col].ToString();
                        break;
                }
            }
            reader.Rights = ReaderRightsInfo.GetReaderRights(reader.NumberReader);
            return reader;
        }

        internal void GiveFreeAbonementRight(int numberReader)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            dbw.GiveFreeAbonementRight(numberReader);
        }


        internal int GetReaderIdByOAuthToken(string token)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.GetReaderIdByOAuthToken(token);
            if (table.Rows.Count == 0)
            {
                throw new Exception("R002");
            }
            return Convert.ToInt32(table.Rows[0][0]);
        }

        public bool IsFiveElBooksIssued(int Id)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.IsFiveElBooksIssued(Id);
            return (table.Rows.Count >= 5) ? true : false;
        }

        internal void UpdateRegistrationFields(ReaderInfo readerInfo)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            dbw.UpdateRegistrationFields(readerInfo);
        }
        internal void UpdateLiveFields(ReaderInfo readerInfo)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            dbw.UpdateLiveFields(readerInfo);
        }

        internal ReaderInfo Authorize(int numberReader, string password)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.AuthorizeReaderWithNumberReader(numberReader, password);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            ReaderInfo result = this.LoadReader(Convert.ToInt32(table.Rows[0][0]));
            return result;
        }
        internal ReaderInfo Authorize(string Email, string password)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.AuthorizeReaderWithEmail(Email, password);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            ReaderInfo result = this.LoadReader(Convert.ToInt32(table.Rows[0][0]));
            return result;
        }
        internal ReaderRightsInfo GetReaderRights(int NumberReader)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.GetReaderRights(NumberReader);
            ReaderRightsInfo result = new ReaderRightsInfo();
            if (table.Rows.Count == 0)
            {
                return result;
            }

            
            foreach (DataRow row in table.Rows)
            {
                ReaderRight right = new ReaderRight();
                object o = row["DataEndReaderRight"];
                Type t = o.GetType();
                right.DateEndReaderRight = (row["DataEndReaderRight"].GetType() != typeof(DBNull)) ? (DateTime)row["DataEndReaderRight"] : DateTime.Now.AddYears(1);
                right.IDOrganization = (row["IDOrganization"].GetType() == typeof(DBNull)) ? 0 : (int)row["IDOrganization"];
                switch((int)row["IDReaderRight"])
                {
                    case 1:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.BritSovet;
                        break;
                    case 2:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.ReadingRoomUser;
                        break;
                    case 3:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.Employee;
                        break;
                    case 4:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.FreeAbonement;
                        break;
                    case 5:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.PaidAbonement;
                        break;
                    case 6:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.CollectiveAbonement;
                        break;
                    case 7:
                        right.ReaderRightValue = ReadersRights.ReaderRightsEnum.Partner;
                        break;

                }
                result.RightsList.Add(right);
            }
            return result;
        }

        public static DataTable GetReaderCountries()
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.GetReaderCountries();
            return table;
        }

    }
}
