using LibflClassLibrary.Books.BJBooks.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace LibflClassLibrary.Books.BJBooks.BJExemplars
{
    
    
    /// <summary>
    /// Сводное описание для ElectronicExemplarInfo
    /// </summary>

    public class BJElectronicExemplarInfo : BJExemplarInfo  //наследуем этот класс от  ExemplarInfo. нахуя?
    {

        public BJElectronicExemplarInfo(int IDMAIN, string Fund)
            //: base(idData)
        {
            this.IDMAIN = IDMAIN;
            this.Fund = Fund;
            base.IDMAIN = IDMAIN;
            base.Fund = Fund;
            loader = new BJExemplarLoader(Fund);
            Statuses = loader.LoadAvailabilityStatuses(IDMAIN, Fund);
            var Status = Statuses.Find(x => x.Project == BJElectronicAvailabilityProjects.VGBIL);
            this.ExemplarAccess = new BJExemplarAccessInfo();
            if (Status.Code == BJElectronicExemplarAvailabilityCodes.vfreeview)
            {
                this.ExemplarAccess.Access = 1001;
                this.ExemplarAccess.MethodOfAccess = 4002;
            }
            else if (Status.Code == BJElectronicExemplarAvailabilityCodes.vloginview)
            {
                this.ExemplarAccess.Access = 1002;
                this.ExemplarAccess.MethodOfAccess = 4002;

            }
            else
            {
                this.ExemplarAccess.Access = 1999;
                this.ExemplarAccess.MethodOfAccess = 4005;
            }
            
        }

        private BJExemplarLoader loader;
        public int IDMAIN { get; set; }
        public string Fund { get; set; }
        public List<BJElectronicExemplarAvailabilityStatus> Statuses;
        //public BJElectronicExemplarAvailabilityStatus this[BJElectronicAvailabilityProjects key]
        //{
        //    get
        //    {
        //        return Statuses.Find(x => x.Project == key);
        //    }
        //}




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
                    result = "00000000" + idmain;
                    break;
                case 2:
                    result = "0000000" + idmain;
                    break;
                case 3:
                    result = "000000" + idmain;
                    break;
                case 4:
                    result = "00000" + idmain;
                    break;
                case 5:
                    result = "0000" + idmain;
                    break;
                case 6:
                    result = "000" + idmain;
                    break;
                case 7:
                    result = "00" + idmain;
                    break;
                case 8:
                    result = "0" + idmain;
                    break;
                case 9:
                    result = idmain;
                    break;
            }
            return @baseName + @"\" + @result[0] + @result[1] + @result[2] + @"\" + result[3] + result[4] + result[5] + @"\" + result[6] + result[7] + result[8] + @"\";
        }
    }
}