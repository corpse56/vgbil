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
    public class ICOrderLoader
    {
        internal ICOrderInfo GetICOrderById(int id)
        {
            ICDBWrapper dbWrapper = new ICDBWrapper();
            DataTable table = dbWrapper.GetICOrderById(id);
            if (table.Rows.Count == 0) return null;
            ICOrderInfo result = new ICOrderInfo();
            result.Id = id;
            DataRow row = table.Rows[0];
            result.ReaderId = (int)row["ReaderId"];
            result.StartDate = (DateTime)row["StartDate"];
            result.Comment = row["Comment"].ToString();
            result.SelectedCardSide = (int)row["CardSide"];
            result.CardFileName = row["CardFileName"].ToString();
            result.Card = ImageCardInfo.GetCard(result.CardFileName, true);
            LoadImages(result, result.Card, result.SelectedCardSide.ToString());
            return result;
        }
        internal ICOrderInfo CreateOrder(string cardFileName, string selectedCardSide, int readerId, string comment)
        {
            ICOrderInfo order = new ICOrderInfo();
            order.Card = ImageCardInfo.GetCard(cardFileName, true);
            order.CardFileName = cardFileName;
            order.Comment = comment;
            order.ReaderId = readerId;
            int numericSelectedCardSide;
            order.SelectedCardSide = (int.TryParse(selectedCardSide, out numericSelectedCardSide)) ? numericSelectedCardSide : 1;
            string selectedCard = order.SelectedCardSide.ToString();
            selectedCard = (selectedCard.Length == 1) ? $"0{selectedCard}" : selectedCard;
            order.SelectedSideUrl = $@"https://cdn.libfl.ru/imcat/{ICLoader.GetPath(order.Card.SeparatorId)}/HQ/{order.CardFileName}_{selectedCard}.jpg";
            LoadImages(order, order.Card, order.SelectedCardSide.ToString());
            order.StartDate = DateTime.Now;
            return order;
        }

        private void LoadImages(ICOrderInfo order, ImageCardInfo card, string selectedCard)
        {
            string login = AppSettings.LoginFileServerRead;
            string pwd = AppSettings.PasswordFileServerRead;
            string ip = AppSettings.IPAddressFileServer;
            string directoryPath = $@"\\{ip}\ImageCatalog\{ICLoader.GetPath(card.SeparatorId)}\HQ\";

            using (new NetworkConnection(directoryPath, new NetworkCredential(login, pwd)))
            {
                DirectoryInfo di = new DirectoryInfo(directoryPath);
                selectedCard = (selectedCard.Length == 1) ? $"0{selectedCard}" : selectedCard;
                FileInfo[] files = di.GetFiles($"{order.CardFileName}_{selectedCard}.jpg");
                if (files.Length != 0)
                {
                    FileInfo selectedImageFile = files.FirstOrDefault(x => x.Name == $"{order.CardFileName}_{selectedCard}.jpg");
                    order.SelectedSideImage = (selectedImageFile == null) ? null : Image.FromFile(selectedImageFile.FullName);
                }
            }
        }

    }
}
