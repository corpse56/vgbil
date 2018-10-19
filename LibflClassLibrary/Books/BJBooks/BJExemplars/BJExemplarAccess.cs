using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BJBooks.BJExemplars
{
    public class BJExemplarAccessInfo
    {
        public int Access { get; set; }
        public int MethodOfAccess { get; set; }
        public int AccessGroup
        {
            get
            {
                return KeyValueMapping.AccessCodeToGroup[Access];
            }
        }
    }
}
