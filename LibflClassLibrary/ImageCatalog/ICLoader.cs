using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.ImageCatalog
{
    public class ICLoader
    {
        public const int MAX_ALLOWED_ORDERS_PER_READER = 30;
        ICDBWrapper dbWrapper = new ICDBWrapper();
        internal ImageCardInfo GetCard(string cardFileName,  bool isNeedToLoadImages)
        {
            if (string.IsNullOrWhiteSpace(cardFileName))
            {
                throw new Exception("M001");
            }
            int cardFileNameNumeric;
            bool tryNumber = int.TryParse(cardFileName, out cardFileNameNumeric);
            if (!tryNumber)
            {
                throw new Exception("M002");
            }
            ImageCardInfo result = new ImageCardInfo();
            DataTable cardTable = dbWrapper.GetCard(cardFileName);
            foreach (DataRow row in cardTable.Rows)
            {
                result.Box = Convert.ToInt32(row["Box"]);
                result.CardFileName = row["FilesName"].ToString();
                result.CardId = (int)row["ID"];
                result.CardType = (CardType)(int)row["CardType"];
                result.Closet = (int)row["Closet"];
                result.CountSide = (int)row["CountSide"];
                result.SeparatorId = (int)row["IDSeparator"];
                result.MainSideFullFileName = $@"\\{AppSettings.IPAddressFileServer}\ImageCatalog\{GetPath(result.SeparatorId)}\HQ\{result.CardFileName}_01.jpg";
                result.MainSideUrl = $@"https://cdn.libfl.ru/imcat/{GetPath(result.SeparatorId)}/HQ/{result.CardFileName}_01.jpg";
            }
            if (isNeedToLoadImages)
            {
                LoadImages(result);
            }

            return result;
        }


        private void LoadImages(ImageCardInfo card)
        {
            string login = AppSettings.LoginFileServerRead;
            string pwd = AppSettings.PasswordFileServerRead;
            string ip = AppSettings.IPAddressFileServer;
            string directoryPath = $@"\\{ip}\ImageCatalog\{ICLoader.GetPath(card.SeparatorId)}\HQ\";

            using (new NetworkConnection(directoryPath, new NetworkCredential(login, pwd)))
            {
                DirectoryInfo di = new DirectoryInfo(directoryPath);
                FileInfo[] files = di.GetFiles($"{card.CardFileName}_01.jpg");
                if (files.Length != 0)
                {
                    card.MainSideImage = Image.FromFile(files[0].FullName);
                }
            }
        }

        public static string GetPath(int separatorId)
        {
            string path = separatorId.ToString();
            switch(path.Length)
            {
                case 1:
                    path = $@"000\00{path}";
                    break;
                case 2:
                    path = $@"000\0{path}";
                    break;
                case 3:
                    path = $@"000\{path}";
                    break;
                case 4:
                    path = $@"00{path.Substring(0,1)}\{path.Substring(1,3)}";
                    break;
                case 5:
                    path = $@"0{path.Substring(0, 2)}\{path.Substring(2, 3)}";
                    break;
                case 6:
                    path = $@"{path.Substring(0, 3)}\{path.Substring(3, 3)}";
                    break;
            }
            return path;
        }
    }
}
