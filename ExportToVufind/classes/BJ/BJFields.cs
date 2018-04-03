using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.ValueObjects;

namespace ExportBJ_XML.classes.BJ
{
    public class BJFields
    {

        private List<BJField> _fields = new List<BJField>();

        public BJField this[string key]
        {
            get
            {
                return FindField(key);
            }
        }

        public BJField this[int key]
        {
            get
            {
                return _fields[key];
            }
        }

        private BJField FindField(string key)
        {
            BJField result = _fields.FirstOrDefault(x => key == x.MNFIELD.ToString() + x.MSFIELD);
            return (result == null) ? new BJField(0, "$0") : result;
        }

        internal BJField AddField(string fieldValue, int mNFIELD, string mSFIELD)
        {
            BJField search = _fields.FirstOrDefault(code => code.MNFIELD == mNFIELD && code.MSFIELD == mSFIELD);
            if (search == null)
            {
                search = new BJField(mNFIELD, mSFIELD);
                search.Add(fieldValue);
                _fields.Add(search);
            }
            else
            {
                search.Add(fieldValue);
            }
            return search;
        }
        internal BJField AddField(string fieldValue, int mNFIELD, string mSFIELD, AuthoritativeFile af)
        {
            BJField search = AddField(fieldValue, mNFIELD, mSFIELD);
            search.AFData = af;
            _fields.Add(search);
            return search;
        }

    }
}
