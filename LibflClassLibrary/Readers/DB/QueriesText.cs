using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.DB
{
    public class Reader
    {
        public string GET_READER
        {
            get
            {
                return "select * from Readers..Main where NumberReader = @Id";
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
                return "select user_id from oauth_access_tokens where access_token = @token and expires>=CURDATE()";
            }
        }
    }

}
