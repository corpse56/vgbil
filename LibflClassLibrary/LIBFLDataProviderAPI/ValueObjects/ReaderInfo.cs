using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibflClassLibrary.Readers;


namespace DataProviderAPI.ValueObjects
{

    public enum TypeReader { Local = 0, Remote = 1 };


    /// <summary>
    /// Сводное описание для ReaderInfo
    /// </summary>
    public class ReaderInfo
    {
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public DateTime DateBirth { get; set; }
        public bool IsRemoteReader { get; set; }
        public string BarCode { get; set; }
        public DateTime DateRegistration { get; set; }
        public DateTime DateReRegistration { get; set; }
        public string MobileTelephone { get; set; }
        public string Email { get; set; }
        public int WorkDepartment { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public int NumberReader { get; set; }
        public TypeReader TypeReader;
        

        public string GetRights()
        {
            return "Бесплатный абонемент";
        }

        public static ReaderInfo GetReader(int Id)
        {
            ReaderLoader loader = new ReaderLoader();
            ReaderInfo result = loader.LoadReader(Id);
            return result;
        }


        public bool IsFiveElBooksIssued()
        {
            ReaderLoader loader = new ReaderLoader();
            bool result = loader.IsFiveElBooksIssued(this.NumberReader);
            return result;
        }
    }
}
