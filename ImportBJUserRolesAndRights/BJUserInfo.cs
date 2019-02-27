using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            int Y, X, LEN;
            char[] STR;
            STR = password.ToCharArray();
            LEN = STR.Length;
            for (X = 0, Y = 0; X < LEN; X++)
            {
                int i1, i2, i3;
                i1 = STR[X];
                i2 = X;
                i3 = GetSecretChar(X);
                STR[Y++] = (char)(i1 + i2 + i3);
            }
            //STR[Y-1] = (char)0;
            string NEWPASS = new string(STR);

            // Get an array of bytes representing the unicode string, two for each character.
            byte[] source = Encoding.Unicode.GetBytes(NEWPASS);

            // Convert the Unicode bytes to UTF-8 representation.
            byte[] converted = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, source);

            // Now that we have converted the bytes, save them to a new string.
            string utf8 = Encoding.UTF8.GetString(converted);

            return (NEWPASS);
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
