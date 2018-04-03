using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace DataProviderAPI.ValueObjects
{
    /// <summary>
    /// Сводное описание для ElectronicExemplarInfo
    /// </summary>

    public class ElectronicExemplarInfo : ExemplarInfo  //наследуем этот класс от  ExemplarInfo
    {

        public ElectronicExemplarInfo(int idData)
            : base(idData)
        {

        }

        public string Path;
        public List<string> JPGFiles = new List<string>();
        public int CountJPG
        {
            get
            {
                return JPGFiles.Count;
            }
        }
        public int WidthFirstFile;
        public int HeightFirstFile;
        public bool IsExistsLQ;
        public bool IsExistsHQ;




        public static string GetPathToElectronicCopy(string id)//принимает ID из вуфайнда
        {
            string baseName = id.Substring(0, id.LastIndexOf("_")).ToUpper();
            string idmain = id.Substring(id.LastIndexOf("_") + 1);
            string result = "";

            switch (idmain.Length)//настроено на семизначный, но в будущем будет 9-значный айдишник
            {
                case 1:
                    result = "000000" + idmain;
                    break;
                case 2:
                    result = "00000" + idmain;
                    break;
                case 3:
                    result = "0000" + idmain;
                    break;
                case 4:
                    result = "000" + idmain;
                    break;
                case 5:
                    result = "00" + idmain;
                    break;
                case 6:
                    result = "0" + idmain;
                    break;
                case 7:
                    result = idmain;
                    break;
            }
            return @baseName + @"\" + @result[0] + @"\" + result[1] + result[2] + result[3] + @"\" + result[4] + result[5] + result[6] + @"\";
        }
    }
}