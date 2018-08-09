using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BJBooks
{
    public class AuthoritativeFile
    {
        private List<string> _valueList;


        public List<string> AFValues
        {
            get
            {
                return _valueList;
            }
        }


        public AuthoritativeFile()
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
