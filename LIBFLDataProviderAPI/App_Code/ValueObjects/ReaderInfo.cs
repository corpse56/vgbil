using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataProviderAPI.ValueObjects
{
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
        public string NumberReader { get; set; }

        public string GetRights()
        {
            return "Бесплатный абонемент";
        }
    }
}
