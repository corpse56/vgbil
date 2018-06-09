using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes.DB;
using System.Data;

namespace LibflClassLibrary.Readers
{
    class ReaderLoader
    {
        public ReaderLoader() { }

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
                        reader.TypeReader = (TypeReader)row[col];
                        break;
                }
            }
            return reader;
        }

        public bool IsFiveElBooksIssued(int Id)
        {
            ReaderDatabaseWrapper dbw = new ReaderDatabaseWrapper();
            DataTable table = dbw.IsFiveElBooksIssued(Id);
            return (table.Rows.Count >= 5) ? true : false;
        }

        
    }
}
