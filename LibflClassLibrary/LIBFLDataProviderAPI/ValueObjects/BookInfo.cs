using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Сводное описание для BookInfo
/// </summary>
namespace DataProviderAPI.ValueObjects
{
    public class BookInfo
    {
        public BookInfo()
        {
        }

        public string Title { get; set; }

        //[DataMember]
        public BJField Author = new BJField();//700a,701a


        public BJField Annotation = new BJField();//




        #region Экземпляры книги

        public List<ExemplarInfoAPI> Exemplars = new List<ExemplarInfoAPI>();

        #endregion


    }
}
