using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes.BJ;

namespace ExportBJ_XML.classes
{
    public class ExemplarAccessInfo
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
