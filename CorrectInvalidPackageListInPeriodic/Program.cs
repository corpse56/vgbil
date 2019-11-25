using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UpDgtPeriodic;

namespace CorrectInvalidPackageListInPeriodic
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlConnection connection = new SqlConnection("Data Source=127.0.0.1,1433;Initial Catalog=PERIOD;Integrated Security=true;");
            SqlConnection connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=MKO;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable ds = new DataTable();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = connection;
            da.SelectCommand.CommandText = "select distinct E.POLE zagl, G.POLE vid, F.POLE lang, D.POLE god, B.POLE sost, B.IDZ idzSost," +
                                           " H.POLE pin " +
                                           " from PERIOD..PI A  " +
                                           " left join PERIOD..PI B on A.VVERH = B.VVERH and B.IDF = 240 " + 
                                           " left join PERIOD..PI C on A.VVERH = C.IDZ and C.IDF = 211 " +
                                           " left join PERIOD..PI D on C.VVERH = D.IDZ and D.IDF = 131 " +
                                           " left join PERIOD..PI E on D.VVERH = E.VVERH and E.IDF = 121 " +
                                           " left join PERIOD..PI F on D.VVERH = F.VVERH and F.IDF = 128 " +
                                           " left join PERIOD..PI G on D.VVERH = G.VVERH and G.IDF = 124 " +
                                           " left join PERIOD..PI H on D.VVERH = H.IDZ and H.IDF = 120 " +
                                           " where A.IDF = 363  and A.POLE = 'e-book'  and B.POLE like '%JPEG%'";
            da.Fill(ds);

            foreach(DataRow row in ds.Rows)
            {
                SetRightKitComposition(row["pin"].ToString(), row["god"].ToString(), Convert.ToInt32(row["idzSost"]));
            }

            Console.Write("end");
            Console.ReadKey();
        }

        static void SetRightKitComposition(string pin, string year, int kitIdz)
        {
            string sTargetConnect = @"\\192.168.4.30\BookAddInf\PERIOD\";
            string exactPath = GetPath(pin, year);
            string kitString = string.Empty ;
            using (new NetworkConnection(sTargetConnect, new NetworkCredential("bj\\DigitCentreWork01", "DigCW_01")))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(sTargetConnect + exactPath);
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    kitString += $"{dir.Name}; ";
                }
                //SqlConnection connection = new SqlConnection("Data Source=127.0.0.1,1433;Initial Catalog=PERIOD;Integrated Security=true;");
                SqlConnection connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=MKO;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
                SqlDataAdapter da = new SqlDataAdapter();
                da.UpdateCommand = new SqlCommand();
                da.UpdateCommand.Connection = connection;
                connection.Open();
                da.UpdateCommand.Parameters.Add("pole", SqlDbType.NVarChar).Value = kitString;
                da.UpdateCommand.Parameters.Add("idz", SqlDbType.Int).Value = kitIdz;
                da.UpdateCommand.CommandText = "update PERIOD..[PI] set POLE = @pole where IDZ = @idz";
                da.UpdateCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        static string GetPath(string pin, string year)
        {
            string path = PINFormat(pin);
            path = path.Substring(0, 3) + @"\" + path.Substring(3, 3) + @"\" + path.Substring(6, 3) + @"\" + year + @"\";
            return path;
        }

        static string PINFormat(string pin)
        {
            switch (pin.Length)
            {
                case 1:
                    pin = "00000000" + pin;
                    break;
                case 2:
                    pin = "0000000" + pin;
                    break;
                case 3:
                    pin = "000000" + pin;
                    break;
                case 4:
                    pin = "00000" + pin;
                    break;
                case 5:
                    pin = "0000" + pin;
                    break;
                case 6:
                    pin = "000" + pin;
                    break;
                case 7:
                    pin = "00" + pin;
                    break;
                case 8:
                    pin = "0" + pin;
                    break;
            }
            return pin;
        }





    }
}
