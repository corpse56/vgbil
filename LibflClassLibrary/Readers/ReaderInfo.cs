using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.Loaders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibflClassLibrary.Readers
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
        [JsonIgnore]
        public string HashedPassword { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public int NumberReader { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TypeReader TypeReader { get; set; }
        

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

        public static ReaderInfo GetReaderByOAuthToken(string token)
        {
            ReaderLoader loader = new ReaderLoader();
            int Id = loader.GetReaderIdByOAuthToken(token);
            ReaderInfo result = loader.LoadReader(Id);
            return result;
        }

        public bool IsFiveElBooksIssued()
        {
            ReaderLoader loader = new ReaderLoader();
            bool result = loader.IsFiveElBooksIssued(this.NumberReader);
            return result;
        }

        public static string GetLoginType(string login)
        {
            int NumberReader = 0;
            bool Check = false;
            if (login.Length <= 9 && int.TryParse(login, out NumberReader))
            {
                return "NumberReader";
            }
            else
                if (login.Length == 19)
            {
                for (int i = 0; i < 19; i++)
                {
                    if (!int.TryParse(login[i].ToString(), out NumberReader))
                    {
                        Check = true;
                        break;
                    }
                }
                if (!Check)//значит 18 цифр. типа номер социалки вбил
                {
                    return "SocialCardNumber";
                }
            }
            else
                    if (Regex.IsMatch(login,
                       @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))//не номер, и не социалка, значит email. проверяем формат
            {
                return "Email";
            }
            else
            {
                return "NotDefined";
            }
            return "NotDefined";
        }
    }
}
