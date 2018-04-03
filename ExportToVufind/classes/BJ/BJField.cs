using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ExportBJ_XML.classes.BJ;



namespace ExportBJ_XML.ValueObjects
{
    /// <summary>
    /// Сводное описание для BJField
    /// </summary>

    public class BJField
    {
        private List<string> _valueList;

        public BJField(int mNFIELD, string mSFIELD)
        {
            this.MNFIELD = mNFIELD;
            this.MSFIELD = mSFIELD;
            _valueList = new List<string>();
        }

        public void Add(string value)
        {
            _valueList.Add(value);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (string value in _valueList)
            {
                result.Append(value);
                result.Append("; ");
            }
            return (result.Length == 0) ? result.ToString() : result.ToString().Remove(result.Length - 2);

        }

        public string ToLower()
        {
            return this.ToString().ToLower();
        }


        public int MNFIELD {get; set;}
        public string MSFIELD { get; set; }

        public string FieldCode
        {
            get
            {
                StringBuilder sb = new StringBuilder(MNFIELD.ToString());
                return sb.Append(MSFIELD).ToString();
            }
        }

        public AuthoritativeFile AFData = new AuthoritativeFile();

    }
}