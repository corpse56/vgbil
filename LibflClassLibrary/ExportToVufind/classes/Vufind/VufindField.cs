using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportBJ_XML.classes.BJ.Vufind
{
    public class VufindField
    {
        public VufindField()
        {
            _valueList = new List<string>();
        }
        public VufindField(string InitValue)
        {
            _valueList = new List<string>();
            this.Add(InitValue);
        }


        private List<string> _valueList;
        public List<string> ValueList
        {
            get
            {
                return _valueList;
            }
        }
        public string FieldName;

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

    }
}
