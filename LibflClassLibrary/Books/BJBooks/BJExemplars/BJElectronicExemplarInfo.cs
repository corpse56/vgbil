using LibflClassLibrary.Books.BJBooks.Loaders;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

            //BJBookInfo book = BJBookInfo.GetBookInfoByPIN(IDMAIN, Fund);

            if (!loader.IsExistsDigitalCopy($"{Fund}_{IDMAIN}"))
            {
                throw new Exception("B003");
            }
            Statuses = loader.LoadAvailabilityStatuses(IDMAIN, Fund);
            var Status = Statuses.Find(x => x.Project == BJElectronicAvailabilityProjects.VGBIL);
            this.ExemplarAccess = new BJExemplarAccessInfo();
            if (Status == null)//это значит книги нет в базе BookAddInf, но она там должна быть! либо надо гиперссылку из BJVVV убирать
            {
                this.ExemplarAccess.Access = 1999;
                this.ExemplarAccess.MethodOfAccess = 4005;
            }
            else
            {
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
        public string Path_HQ;
        public string Path_LQ;
        public bool FilesFound = false;
        public List<string> JPGFiles = new List<string>();
        public string Path_Cover;
        public void FillFileFields()
        {
            string ip = AppSettings.IPAddressFileServer;
            string login = AppSettings.LoginFileServerRead;
            string pwd = AppSettings.PasswordFileServerRead;
            string _directoryPath = @"\\" + ip + @"\BookAddInf\";

            //когда появится инвентаризация электронных копий, то сюда надо вставить получение инфы об электронной копии
            FileInfo[] fi;
            using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
            {
                _directoryPath = @"\\" + ip + @"\BookAddInf\" + GetPathToElectronicCopy(this.BookId);

                DirectoryInfo di = new DirectoryInfo(_directoryPath);
                FilesFound = di.Exists;//каталога с картинками страниц не существует или существует
                DirectoryInfo hq = new DirectoryInfo(_directoryPath + @"\JPEG_HQ\");
                this.IsExistsHQ = (hq.Exists) ? true : false;
                this.Path_HQ = (hq.Exists) ? hq.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;

                DirectoryInfo lq = new DirectoryInfo(_directoryPath + @"\JPEG_LQ\");
                this.IsExistsLQ = (lq.Exists) ? true : false;
                this.Path_LQ = (lq.Exists) ? lq.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;

                fi = hq.GetFiles("*.jpg").OrderBy(f => f.Name).ToArray(); //сортируем по дате изменения. именно в таком порядке они сканировались. а вообще вопрос непростой, поскольку попадаются файлы, выпадающие из этого условия

                foreach (FileInfo f in fi)
                {
                    this.JPGFiles.Add(f.Name);
                }

                FileInfo coverPath = new FileInfo(_directoryPath + @"\JPEG_AB\cover.jpg");
                this.Path_Cover = (coverPath.Exists) ? coverPath.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;

            }


            Image img = Image.FromFile(fi[1].FullName);//берём вторую страницу, потому что первая бывает с пустым разворотом
            this.WidthFirstFile = img.Width;
            this.HeightFirstFile = img.Height;

        }

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