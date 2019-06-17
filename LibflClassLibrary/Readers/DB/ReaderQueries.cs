using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.DB
{
    public class ReaderQueries
    {
        internal string PROLONG_RIGHT
        {
            get
            {
                return "update Readers..ReaderRight set DataEndReaderRight = DATEADD (year , 1 , getdate() ) where IDReader = @NumberReader and IDReaderRight = @Id";
            }

        }

        public string GET_READER
        {
            get
            {
                return "select *, B.Photo fotka from Readers..Main A  " +
                       " left join Readers..Photo B on A.NumberReader = B.IDReader " +
                       " where A.NumberReader = @Id";
            }
        }
        public string IS_FIVE_ELBOOKS_ISSUED
        {
            get
            {
                return "select * from Reservation_R..ELISSUED where IDREADER = @Id";
            }
        }

        //mysql
        public string GET_READER_ID_BY_OAUTH_TOKEN
        {
            get
            {
                return "select user_id from oauth_access_tokens where access_token = @token and expires >= CURDATE()";
            }
        }

        public string AUTHORIZE_READER_WITH_NUMBERREADER
        {
            get
            {
                return "select NumberReader from Readers..Main where NumberReader = @Id and Password = @Password";
            }
        }
        public string AUTHORIZE_READER_WITH_EMAIL
        {
            get
            {
                return "select NumberReader from Readers..Main where Email = @Email and Password = @Password";
            }
        }
        public string GET_READER_BY_EMAIL
        {
            get
            {
                return "select NumberReader from Readers..Main where Email = @Email";
            }
        }

        public string GET_READER_RIGHTS
        {
            get
            {
                return "select * from Readers..ReaderRight where IDReader = @NumberReader";
            }
        }

        public string GET_COUNTRIES_READERS
        {
            get
            {
                return "select IDCountry, NameCountry from Readers..Country";
            }
        }
        public string GIVE_FREE_ABONEMENT_RIGHTS
        {
            get
            {
                return "insert into Readers..[ReaderRight] (IDReader, IDReaderRight, DataEndReaderRight, IDOrganization) values " +
                                                            "(@NumberReader, 4, dateadd(year, 1, getdate()), null)";
            }
        }

        public string UPDATE_REGISTRATION_FIELDS
        {
            get
            {
                return "update Readers..Main "+
                        "set RegistrationCountry = @RegistrationCountry, " +
                        " RegistrationRegion = @RegistrationRegion, " +
                        " RegistrationProvince = @RegistrationProvince, " +
                        " RegistrationDistrict = @RegistrationDistrict, " +
                        " RegistrationCity = @RegistrationCity, " +
                        " RegistrationStreet = @RegistrationStreet, " +
                        " RegistrationHouse = @RegistrationHouse, " +
                        " RegistrationFlat = @RegistrationFlat, " +
                        " Email = @Email, " +
                        " MobileTelephone = @MobileTelephone " +
                        " where NumberReader = @NumberReader";
            }
        }
        public string UPDATE_LIVE_FIELDS
        {
            get
            {
                return "update Readers..Main " +
                        "set " +
                        " LiveRegion = @LiveRegion, " +
                        " LiveProvince = @LiveProvince, " +
                        " LiveDistrict = @LiveDistrict, " +
                        " LiveCity = @LiveCity, " +
                        " LiveStreet = @LiveStreet, " +
                        " LiveHouse = @LiveHouse, " +
                        " LiveFlat = @LiveFlat, " +
                        " Email = @Email, " +
                        " MobileTelephone = @MobileTelephone " +
                        " where NumberReader = @NumberReader";

            }
        }

        public string CHANGE_PASSWORD_LOCAL_READER
        {
            get
            {
                return "update Readers..Main set Password = @Password where NumberReader = @NumberReader";
            }
        }

        public string GET_READER_BY_BAR
        {
            get
            {
                return "select NumberReader from Readers..Main where BarCode = @bar";
            }
        }

        public string GET_READER_BY_UID
        {
            get
            {
                return "select BarCodeBarCodeToUID from Readers..BarCodeToUID where UIDBarCodeToUID = @uid";
            }
        }
    }

}
