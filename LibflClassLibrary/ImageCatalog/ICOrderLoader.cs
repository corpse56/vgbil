using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Circulation;
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
        ICDBWrapper dbWrapper = new ICDBWrapper();

        internal List<ICOrderInfo> GetActiveOrdersByReader(int readerId)
        {
            DataTable table = new DataTable();
            table = dbWrapper.GetActiveOrdersByReader(readerId);
            List<ICOrderInfo> result = new List<ICOrderInfo>();
            foreach (DataRow row in table.Rows)
            {
                ICOrderInfo item = this.GetICOrderById((int)row["Id"], false);
                result.Add(item);
            }
            return result;
        }

        internal void DeleteOrder(int readerId, int orderId)
        {
            dbWrapper.DeleteOrder(orderId);
        }


        internal ICOrderInfo GetICOrderById(int id, bool loadImages)
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
            result.StatusName = row["StatusName"].ToString();
            result.Card = ImageCardInfo.GetCard(result.CardFileName, true);
            string selectedCard = result.SelectedCardSide.ToString();
            selectedCard = (selectedCard.Length == 1) ? $"0{selectedCard}" : selectedCard;
            result.SelectedSideUrl = $@"https://cdn.libfl.ru/imcat/{ICLoader.GetPath(result.Card.SeparatorId)}/HQ/{result.CardFileName}_{selectedCard}.jpg";
            result.RefusualReason = row["Refusual"].ToString();
            if (loadImages)
            {
                LoadImages(result, result.Card, result.SelectedCardSide.ToString());
            }
            return result;
        }

        internal List<ICOrderInfo> GetActiveOrdersForBookkeeping()
        {
            DataTable table = new DataTable();
            table = dbWrapper.GetActiveOrdersForBookkeeping();
            List<ICOrderInfo> result = new List<ICOrderInfo>();
            foreach (DataRow row in table.Rows)
            {
                ICOrderInfo item = this.GetICOrderById((int)row["Id"], true);
                result.Add(item);
            }
            return result;
        }

        internal void RefuseOrder(ICOrderInfo order, BJUserInfo bjUser, string refusualReason)
        {
            dbWrapper.RefuseOrder( order,  bjUser,  refusualReason);
        }

        internal List<ICOrderInfo> GetHistoryOrdersByReader(int readerId)
        {
            DataTable table = new DataTable();
            table = dbWrapper.GetHistoryOrdersByReader(readerId);
            List<ICOrderInfo> result = new List<ICOrderInfo>();
            foreach (DataRow row in table.Rows)
            {
                ICOrderInfo item = this.GetICOrderById((int)row["Id"], false);
                item.RefusualReason = row["Refusual"].ToString();
                result.Add(item);
            }
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
            order.StatusName = CirculationStatuses.OrderIsFormed.Value;
            if (this.IsOrderAlreadyExists(order))
            {
                throw new Exception("M003");
            }
            if (this.GetOrdersCountForReader(order.ReaderId) >= ICLoader.MAX_ALLOWED_ORDERS_PER_READER)
            {
                throw new Exception("M004");
            }
            this.InsertOrderInDb(order);
            return order;
        }

        private int GetOrdersCountForReader(int readerId)
        {
            DataTable table = dbWrapper.GetOrdersCountForReader(readerId);
            return (table.Rows.Count);
        }

        private bool IsOrderAlreadyExists(ICOrderInfo order)
        {
            DataTable table = dbWrapper.IsOrderAlreadyExists(order);
            return (table.Rows.Count == 0) ? false : true;
        }

        private void InsertOrderInDb(ICOrderInfo order)
        {
            dbWrapper.InsertOrderInDb(order);
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
