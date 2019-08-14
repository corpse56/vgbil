using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.Litres;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GetLitresAccountTest
{
    class Program
    {

        static void Main(string[] args)
        {
            LitresInfo account = new LitresInfo();

            LitresAccountManager manager = new LitresAccountManager();
            LitresApiHandler.get_sid_from_response("");
            manager.GetLitresNewAccount();




            //manager.GetNewLitresAccount();

            Console.ReadKey();
            //{ "success" : true, "my_sid" : { "currency" : "RUB", "success" : true, "sid" : "587bd14m5q4p4m3s2vcc4q0tf5a5b927", "country" : "RUS", "region" : "Moscow", "city" : "Moscow"}, "time" : "2019-08-14T18:12:59+03:00"}
        }
    }
}
