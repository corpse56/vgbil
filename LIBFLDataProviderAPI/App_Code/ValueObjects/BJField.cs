using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace DataProviderAPI.ValueObjects
{
    /// <summary>
    /// Сводное описание для BJField
    /// </summary>


    [JsonConverter(typeof(ToStringJsonConverter))]
    public class BJField
    {
        private List<string> _valueList;

        public BJField()
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

        public int MNFIELD {get; set;}
        public string MSFIELD { get; set; }

        public string AllSynonymsFromAuthorityFile { get; set; }

    }
}