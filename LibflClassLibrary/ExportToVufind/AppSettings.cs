using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace LibflClassLibrary.ExportToVufind
{
    
    public class AppSettings
    {
        static public string ConnectionString
        {
            get
            {
                //Initial Catolog всегда (!!!!) должен быть равен Readers, потому что внешняя дллка регистрации использует именно это имя для обращений к базе.
                //зачем это было сделано, известно только тому, кто сделал.
                string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Readers;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
                //string connectionString = @"Data Source=127.0.0.1;Initial Catalog=Readers;Integrated Security=True;";
                // string connectionString = @"Data Source=192.168.1.165;Initial Catalog=Readers;Integrated Security=True;";

                return connectionString;
            }
        }
        static public string StatisticsConnectionString
        {
            get
            {
                string connectionString = "Data Source=192.168.4.24,1442;Initial Catalog=Readers;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
                return connectionString;
            }
        }
        static public string IPAddressFileServer
        {
            get
            {
                string connectionString = "192.168.4.30";
                return connectionString;
            }
        }
        static public string LoginFileServerRead
        {
            get
            {
                string connectionString = "BJStor01\\imgview";
                return connectionString;
            }
        }
        static public string PasswordFileServerRead
        {
            get
            {
                string connectionString = "Image_123Viewer";
                return connectionString;
            }
        }
        static public string LoginFileServerReadWrite
        {
            get
            {
                string connectionString = "bj\\DigitCentreWork01";
                return connectionString;
            }
        }
        static public string PasswordFileServerReadWrite
        {
            get
            {
                string connectionString = "DigCW_01";
                return connectionString;
            }
        }
        static public string IPAddressBackupFileServer
        {
            get
            {
                string connectionString = "192.168.6.65";
                return connectionString;
            }
        }
        static public string LoginBackupFileServerRead
        {
            get
            {
                string connectionString = "DgtPeriodic";
                return connectionString;
            }
        }
        static public string PasswordBackupFileServerRead
        {
            get
            {
                string connectionString = "Periodic_DGT";
                return connectionString;
            }
        }
        static public string BookStatusConnection
        {
            get
            {
                string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=OpacBJ;Password=BJ_Opac;Connection Timeout = 1200";
                return connectionString;
            }
        }
        static public string ReadersConnection_Basket
        {
            get
            {
                string connectionString = "Data Source=192.168.4.7;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=EmplOrd;Password=Employee_Order;Connection Timeout = 1200";
                return connectionString;
            }
        }
        static public string ReadersConnection_OnlyRead
        {
            get
            {
                string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Readers;Persist Security Info=True;User ID=Readers_Read;Password=Read_Only;Connection Timeout = 1200";
                return connectionString;
            }
        }
        static public string ReadersConnection_OnlyReadWrite
        {
            get
            {
                string connectionString = "Data Source=192.168.4.25,1443;Initial Catalog=Readers;Persist Security Info=True;User ID=Read_RW;Password=Read_onlyReadWrite;Connect Timeout=1200";
                return connectionString;
            }
        }
        static public string ExternalElectronicBookViewer
        {
            get
            {
                string connectionString = "http://opac.libfl.ru/elcir/viewer.aspx";
                return connectionString;
            }
        }
        static public string IndoorElectronicBookViewer
        {
            get
            {
                //string connectionString = "http://192.168.3.191/elcir/viewer.aspx";
                string connectionString = "http://192.168.4.28/elcir/viewer.aspx";
                return connectionString;
            }
        }

        public static string OauthMySqlConnectionString
        {
            get
            {
                //старый пароль до аварии
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=oauth;Pwd=oauthpwd;Database=oauth";
                //новый пароль и пользователь. Доступ только с 80.250.173.142 и с моих локальных айпишников
                return "Server=oauth.libfl.ru;Port=3306;Character Set=utf8;Uid=alisapi;Pwd=api_alis_pwd;Database=libfl_oauth;SslMode=None";
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=alisapi;Pwd=api_alis_pwd;Database=libfl_oauth;SslMode=None";
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=alisapi;Pwd=api_alis_pwd;Database=libfl_oauth;SslMode=None";
            }
        }
        public static string DevOauthMySqlConnectionString
        {
            get
            {
                //старый пароль до аварии
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=oauth;Pwd=oauthpwd;Database=oauth";
                //новый пароль и пользователь. Доступ только с 80.250.173.142 и с моих локальных айпишников
                return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=alis;Pwd=dibagura23s;Database=libfl_oauth;SslMode=None";
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=alisapi;Pwd=api_alis_pwd;Database=libfl_oauth;SslMode=None";
                //return "Server=192.168.6.216;Port=3306;Character Set=utf8;Uid=alisapi;Pwd=api_alis_pwd;Database=libfl_oauth;SslMode=None";
            }
        }



        //<add key="ExternalElectronicBookViewer" value="http://opac.libfl.ru/elcir/viewer.aspx"/>
        //<add key="IndoorElectronicBookViewer" value="http://192.168.3.191/elcir/viewer.aspx"/>



        //<add name="ReadersConnection_OnlyRead" providerName="System.Data.SqlClient" connectionString="Data Source=192.168.4.25,1443;Initial Catalog=Readers;Persist Security Info=True;User ID=Readers_Read;Password=Read_Only;Connection Timeout = 1200"/>
        //<add name="ReadersConnection_OnlyReadWrite" providerName="System.Data.SqlClient" connectionString="Data Source=192.168.4.25,1443;Initial Catalog=Readers;Persist Security Info=True;User ID=Read_RW;Password=Read_onlyReadWrite;Connect Timeout=1200"/>
        //<add name="ReadersConnection_Basket" providerName="System.Data.SqlClient" connectionString="Data Source=192.168.4.7;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=EmplOrd;Password=Employee_Order;Connection Timeout = 1200"/>
        //<add name="BookStatusConnection" providerName="System.Data.SqlClient" connectionString="Data Source=192.168.4.25,1443;Initial Catalog=Reservation_R;Persist Security Info=True;User ID=OpacBJ;Password=BJ_Opac;Connection Timeout = 1200"/>
        //<add name="IPAddressFileServer" providerName="System.Data.SqlClient" connectionString="192.168.4.30"/>
        //<add name="LoginFileServer" providerName="System.Data.SqlClient" connectionString="BJStor01\\imgview"/>
        //<add name="PasswordFileServer" providerName="System.Data.SqlClient" connectionString="Image_123Viewer"/>

    }

}
