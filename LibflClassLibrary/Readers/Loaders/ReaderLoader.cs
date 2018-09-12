using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataProviderAPI.ValueObjects;
using LibflClassLibrary.Readers.DB;

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

                }
            }
            return reader;
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
    }
}
