using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportBJ_XML.classes.BJ.Vufind
{
    public class VufindField
    {
         private List<string> _valueList;

         public VufindField()
        {
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
    }
}
