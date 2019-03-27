using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportBJUserRolesAndRights
{
    class UserStatus
    {
        public int RoleId;
        public int DepId;
        public string DepName;
    }
    class BJUserInfo
    {
        public string login;
        public string password;
        public string HashPwd;
        public List<UserStatus> UserStatus = new List<UserStatus>();


        public static string HashPassword(string password)
        {
            int x, y;
            char[] HashedPasswordChars;
            HashedPasswordChars = password.ToCharArray();
            for (x = 0, y = 0; x < HashedPasswordChars.Length; x++)
            {
                int i1, i2, i3;
                i1 = HashedPasswordChars[x];
                i2 = x;
                i3 = GetSecretChar(x);
                HashedPasswordChars[y++] = (char)(i1 + i2 + i3);
            }
            string HashedPassword = new string(HashedPasswordChars);

            byte[] bytes = new byte[HashedPasswordChars.Length];
            for (int i = 0; i < HashedPasswordChars.Length; i++)
            {
                bytes[i] = Convert.ToByte(HashedPassword[i]);
            }

            var cp1251 = Encoding.GetEncoding(1251);
            HashedPassword = cp1251.GetString(bytes);
            return (HashedPassword);
        }
        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
        private static int GetSecretChar(int X)
        {
                int SEC = 21;

                if (X > 2) SEC = 27;
                if (X > 4) SEC = 12;
                if (X > 6) SEC = 7;
                if (X > 8) SEC = 33;
                if (X > 10) SEC = 43;
                if (X > 14) SEC = 13;

                return (SEC);
        }
    }
}
