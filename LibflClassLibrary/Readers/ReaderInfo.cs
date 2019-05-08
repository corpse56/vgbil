﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LibflClassLibrary.ALISAPI.RequestObjects.Readers;
using LibflClassLibrary.Readers;
using LibflClassLibrary.Readers.Loaders;
using LibflClassLibrary.Readers.ReadersRight;
using LibflClassLibrary.Readers.ReadersRights;
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
        private ReaderLoader loader = new ReaderLoader();

        public string FamilyName { get; set; }

        public string Name { get; set; }
        public string FatherName { get; set; }
        public DateTime DateBirth { get; set; }
        public bool IsRemoteReader { get; set; }
        public string BarCode { get; set; }
        public DateTime DateRegistration { get { return new DateTime(DateRegistration.Ticks, DateTimeKind.Utc); } set { } }
        public DateTime DateReRegistration { get; set; }
        public string MobileTelephone { get; set; }
        public string Email { get; set; }
        public int WorkDepartment { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public int NumberReader { get; set; }
        public int RegistrationCountry { get; set; }
        public string RegistrationRegion { get; set; }
        public string RegistrationProvince { get; set; }
        public string RegistrationDistrict { get; set; }
        public string RegistrationCity { get; set; }
        public string RegistrationStreet { get; set; }
        public string RegistrationHouse { get; set; }
        public string RegistrationFlat { get; set; }
        public string RegistrationTelephone { get; set; }

        public string LiveRegion { get; set; }
        public string LiveProvince { get; set; }
        public string LiveDistrict { get; set; }
        public string LiveCity { get; set; }
        public string LiveStreet { get; set; }
        public string LiveHouse { get; set; }
        public string LiveFlat { get; set; }
        public Image Photo;


        public ReaderRightsInfo Rights = null;



        [JsonConverter(typeof(StringEnumConverter))]
        public TypeReader TypeReader { get; set; }

        public static Dictionary<int, string> GetCountriesList()
        {
            ReaderLoader loader = new ReaderLoader();
            return loader.GetReaderCountries();
        }

        public static ReaderInfo GetReader(int Id)
        {
            ReaderLoader loader = new ReaderLoader();
            ReaderInfo result = loader.LoadReader(Id);
            return result;
        }
        public static ReaderInfo GetReader(string Email)
        {
            ReaderLoader loader = new ReaderLoader();
            ReaderInfo result = loader.LoadReader(Email);
            return result;
        }

        public static string HashPass(string strPassword, string strSol)
        {
            String strHashPass = string.Empty;
            byte[] bytes = Encoding.Unicode.GetBytes(strSol + strPassword);
            //создаем объект для получения средст шифрования 
            SHA256CryptoServiceProvider CSP = new SHA256CryptoServiceProvider();
            //вычисляем хеш-представление в байтах 
            byte[] byteHash = CSP.ComputeHash(bytes);
            //формируем одну цельную строку из массива 
            foreach (byte b in byteHash)
            {
                strHashPass += string.Format("{0:x2}", b);
            }
            return strHashPass;
        }

        public static ReaderInfo GetReaderByOAuthToken(AccessToken token)
        {
            ReaderLoader loader = new ReaderLoader();
            int Id = loader.GetReaderIdByOAuthToken(token.TokenValue);
            ReaderInfo result = loader.LoadReader(Id);
            return result;
        }

        public bool IsFiveElBooksIssued()
        {
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
            //if (Regex.IsMatch(login,
            //   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
            //   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$"))//не номер, и не социалка, значит email. проверяем формат
            if (Regex.IsMatch(login, @"^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$"))
            {
                return "Email";
            }
            else
            {
                return "NotDefined";
            }
        }

        internal static bool IsRightReaderBarcode(string barcode)
        {
            if (barcode.Length > 0)
            {
                if (barcode[0] != 'R' && barcode[0] != 'G')
                {
                    return false;
                }
                else
                {
                    if (barcode.Length == 8)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }                
            }
            else
            {
                return false;
            }

        }

        public static ReaderInfo GetReaderByBar(string data)
        {
            ReaderLoader loader = new ReaderLoader();
            ReaderInfo result = loader.LoadReaderByBar(data);
            return result;
        }

        public void ChangePasswordLocalReader(ChangePasswordLocalReader request)
        {
            DateTime DateBirth;
            if (!DateTime.TryParseExact(request.DateBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth))
            {
                throw new Exception("G001");
            }

            if (this.DateBirth.Date != DateBirth.Date)
            {
                throw new Exception("R005");
            }
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                throw new Exception("R006");
            }
            loader.ChangePasswordLocalReader(this, request);
        }

        public bool IsBirthDateMatchReaderId(BirthDateMatchReaderId request)
        {
            DateTime DateBirth;
            if (!DateTime.TryParseExact(request.DateBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateBirth))
            {
                throw new Exception("G001");
            }

            if (this.DateBirth.Date != DateBirth.Date)
            {
                return false;//throw new Exception("R005");
            }
            return true;
        }

        internal void UpdateRegistrationFields()
        {
            loader.UpdateRegistrationFields(this);
        }
        internal void UpdateLiveFields()
        {
            loader.UpdateLiveFields(this);
        }
        internal void ProlongRights(ReaderRightsEnum right)
        {
            int id = this.Rights[right].ToIdDataBase();
            loader.ProlongRights(id, this.NumberReader);
        }
        public static ReaderInfo Authorize(AuthorizeInfo request)
        {
            int NumberReader = 0;
            ReaderInfo result = null;
            ReaderLoader loader = new ReaderLoader();
            string LoginType = ReaderInfo.GetLoginType(request.login);
            if (LoginType == "NumberReader")
            {
                NumberReader = int.Parse(request.login);
                ReaderInfo reader = ReaderInfo.GetReader(NumberReader);
                if (reader == null) throw new Exception("R001");
                request.password = ReaderInfo.HashPass(request.password, reader.Salt);
                result = loader.Authorize(NumberReader, request.password);
            }
            else if (LoginType == "Email")
            {
                ReaderInfo reader = ReaderInfo.GetReader(request.login);
                if (reader == null) throw new Exception("R001");
                request.password = ReaderInfo.HashPass(request.password, reader.Salt);
                result = loader.Authorize(request.login, request.password);
            }
            if (result == null)
            {
                throw new Exception("R001");
            }
            return result;
        }

        public void SetPasswordLocalReader(SetPasswordLocalReader request, ReaderInfo reader)
        {
            loader.SetPasswordLocalReader(request, reader);
        }
    }
}
