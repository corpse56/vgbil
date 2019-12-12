using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.PeriodicBooks;
using LibflClassLibrary.Circulation.CirculationService;

namespace LibflClassLibrary.Books.PeriodBooks
{

    public class PeriodicLoader
    {
        private PeriodicDatabaseWrapper dbWrapper_;

        public PeriodicLoader()
        {
            dbWrapper_ = new PeriodicDatabaseWrapper();
        }

        internal PeriodicBookInfo GetBookByBar(string bar)
        {
            DataTable table = dbWrapper_.GetBookByBar(bar);
            if (table.Rows.Count == 0) return null;
            PeriodicBookInfo result = new PeriodicBookInfo();
            DataRow row = table.Rows[0];
            result.Id = row["pin"].ToString();
            result.Title = row["title"].ToString();
            result.PublishYear = row["pubYear"].ToString();
            result.Number = row["number"].ToString();
            PeriodicLoader loader = new PeriodicLoader();
            PeriodicExemplarInfo exemplar = loader.GetExemplarByBar(bar);
            if (exemplar != null)
            {
                result.Exemplars.Add(exemplar);
            }
            return result;
        }
        //пином считаем IDZ Штрихкода!!!!!!
        //internal BookBase GetBookInfoByPIN(int pin)
        //{
        //    DataTable table = dbWrapper_.GetBookInfoByPIN(pin);
        //    if (table.Rows.Count == 0) return null;
        //    DataRow row = table.Rows[0];
        //    PeriodicBookInfo result = new PeriodicBookInfo();
        //    result.Id = $"PERIOD_{pin.ToString()}";
        //    result.Fund = "PERIOD";
        //    result.Exemplars = new List<ExemplarBase>();
        //    //result.Exemplars.Add(null);
        //    result.Pin = pin.ToString();
        //    result.Title = row["title"].ToString();
        //    result.Language = row["language"].ToString();
        //    return result;
        //}

        //IDZBar - это пин периодики
        internal PeriodicBookInfo GetBookInfoByIDZBar(int IDZBar)
        {
            DataTable table = dbWrapper_.GetBookInfoByIDZBar(IDZBar);
            if (table.Rows.Count == 0) return null;
            DataRow row = table.Rows[0];
            PeriodicBookInfo result = new PeriodicBookInfo();
            result.Id = $"PERIOD_{IDZBar.ToString()}";
            result.Fund = "PERIOD";
            //экземпляр периодики всегда один. экземпляр == книга
            result.Exemplars = new List<ExemplarBase>();
            result.Exemplars.Add(ExemplarFactory.CreateExemplar(IDZBar, "PERIOD"));
            result.OriginalPin = row["pin"].ToString();
            result.Title = $"{row["title"].ToString()}-->{row["pubYear"].ToString()}-->{row["number"].ToString()}";
            result.Language = row["language"].ToString();
            result.PublishYear = row["pubYear"].ToString();
            return result;
        }

        internal PeriodicExemplarInfo GetExemplarByInventoryNumber(string inventoryNumber)
        {
            string bar = this.GetExemplarBarByInventoryNumber(inventoryNumber);
            PeriodicExemplarInfo result = this.GetExemplarByBar(bar);
            return result;
        }

        internal PeriodicExemplarInfo GetPeriodicExemplarInfoByExemplarId(int exemplarId)
        {
            DataTable table = dbWrapper_.GetPeriodicExemplarBarByExemplarId(exemplarId);
            if (table.Rows.Count == 0) return null;
            string bar = table.Rows[0]["bar"].ToString();
            return PeriodicExemplarInfo.GetPeriodicExemplarInfoByBar(bar);
        }

        internal string GetExemplarBarByInventoryNumber(string inventoryNumber)
        {
            DataTable table = dbWrapper_.GetExemplarBarByInventoryNumber(inventoryNumber);
            if (table.Rows.Count == 0) return null;
            return table.Rows[0]["bar"].ToString();
        }

        internal PeriodicExemplarInfo GetExemplarByBar(string bar)
        {
            PeriodicExemplarInfo result = new PeriodicExemplarInfo();
            DataTable table = dbWrapper_.GetBookByBar(bar);
            if (table.Rows.Count == 0) return null;
            DataRow row = table.Rows[0];
            result.Bar = bar;
            result.Fund = "PERIOD";
            result.Number = row["number"].ToString();
            result.Id = row["exemplarId"].ToString();//idz of bar
            result.PublishYear = row["pubYear"].ToString();
            result.BookId = $"PERIOD_{row["exemplarId"].ToString()}";//для периодики пин и id экземпляра совпадают
            result.Author = string.Empty ;
            result.Title = $"{row["title"].ToString()}-->{row["pubYear"].ToString()}-->{row["number"].ToString()}";
            result.AccessInfo = GetPeriodicAccessInfo(row["location"].ToString(), row["access"].ToString(), row["pubClass"].ToString());
            result.Rack = string.Empty;
            result.Location = row["location"].ToString();
            result.Language = row["language"].ToString();
            result.Cipher = row["cipher"].ToString();
            result.PublicationClass = row["pubClass"].ToString();
            result.circulation = new PeriodicCirculationManager();
            result.simpleViewer = new PeriodicExemplarSimpleViewer();
            return result;
        }
        private ExemplarAccessInfo GetPeriodicAccessInfo(string location, string access, string pubClass)
        {
            ExemplarAccessInfo accessInfo = new ExemplarAccessInfo();
            if (pubClass == "Для выставки")
            {
                accessInfo.Access = 1011;
                accessInfo.MethodOfAccess = 4000;
                return accessInfo;
            }
            if (location.ToLower().Contains("книгохране") && access != "Недоступно для заказа")
            {
                accessInfo.Access = 1005;
                accessInfo.MethodOfAccess = 4000;
                return accessInfo;
            }
            if (!location.ToLower().Contains("книгохране") && pubClass == "Для длительного пользования")
            {
                accessInfo.Access = 1007;
                accessInfo.MethodOfAccess = 4000;
                return accessInfo;
            }
            accessInfo.Access = 1017;
            accessInfo.MethodOfAccess = 4005;
            return accessInfo;


        }

    }
}
