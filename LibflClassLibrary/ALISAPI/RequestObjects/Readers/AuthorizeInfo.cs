using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.ALISAPI.RequestObjects.Readers
{
    public class AuthorizeInfo
    {
        /// <summary>Логин может быть как номер читательского билета, так и имэйл</summary>
        public string login { get; set; }
        public string password { get; set; }
    }
}
