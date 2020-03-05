using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LibflClassLibrary.ALISAPI.ResponseObjects.Books;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Circulation.CirculationService.RecieveExemplarFromReader;

namespace LibflClassLibrary.Books.BJBooks.Loaders
{
    public class BJExemplarLoader
    {
        BJDatabaseWrapper dbWrapper;
        public BJExemplarLoader(string Fund)
        {
            this.Fund = Fund;
            dbWrapper = new BJDatabaseWrapper(Fund);
        }
        string Fund { get; set; }

        internal List<BJElectronicExemplarAvailabilityStatus> LoadAvailabilityStatuses(int IDMAIN, string Fund)
        {
            DataTable table = dbWrapper.LoadAvailabilityStatuses(IDMAIN, Fund);
            var listResult = new List<BJElectronicExemplarAvailabilityStatus>();
            BJElectronicExemplarAvailabilityStatus result;
            foreach (DataRow row in table.Rows)
            {
                result = new BJElectronicExemplarAvailabilityStatus();
                switch (row["CodeTypeProject"].ToString())
                {
                    case "v-stop":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vstop;
                        break;
                    case "v-free-view":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vfreeview;
                        break;
                    case "v-login-view":
                        result.Code = BJElectronicExemplarAvailabilityCodes.vloginview;
                        break;
                    case "dlstop":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlstop;
                        break;
                    case "dlopen":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlopen;
                        break;
                    case "dlview":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dlview;
                        break;
                    case "dllimit":
                        result.Code = BJElectronicExemplarAvailabilityCodes.dllimit;
                        break;

                }
                switch ((int)row["IDProject"])
                {
                    case 1:
                        result.Project = BJElectronicAvailabilityProjects.VGBIL;
                        break;
                    case 2:
                        result.Project = BJElectronicAvailabilityProjects.NEB;
                        break;
                }
                listResult.Add(result);
            }
            return listResult;
        }

        internal BJExemplarInfo GetExemplarByIdData(int exemplarId, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(exemplarId);
            if (table.Rows.Count == 0) return null;
            BJExemplarInfo exemplar = new BJExemplarInfo((int)table.Rows[0]["IDDATA"]);
            exemplar.IDMAIN = (int)table.Rows[0]["IDMAIN"];
            exemplar.Fund = fund;
            exemplar.BookId = $"{exemplar.Fund}_{exemplar.IDMAIN.ToString()}";
            foreach (DataRow row in table.Rows)//записываем все поля в объект
            {
                if (fund == "BJACC")
                {
                    if (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "899$w")//в американской базе нет инвентарных номеров. берем штрихкод
                    {
                        exemplar.Created = (DateTime)row["Created"];//за дату создания берем дату присвоения штрихкода
                    }
                }
                else
                {
                    if (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "899$p")//в остальных есть и берём дату создания поля инвентарный номер
                    {
                        exemplar.Created = (DateTime)row["Created"];//за дату создания берем дату присвоения инвентаря
                    }
                }
                if (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "899$a")
                {
                    exemplar.Fields.AddField(row["NAME"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString()); //местонахождение берём из LIST_8, а не из DATAEXTPLAIN, потому что в поле PLAIN меняются некоторые символы
                    continue;
                }
                if (row["MNFIELD"].ToString() + row["MSFIELD"].ToString() == "899$w")
                {
                    exemplar.Fields.AddField(row["SORT"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString()); //местонахождение берём из LIST_8, а не из DATAEXTPLAIN, потому что в поле PLAIN меняются некоторые символы
                    continue;
                }
                exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());//добавляем все поля блока 260 к объекту экземпляра
                if ((int)row["MNFIELD"] == 929 && row["MSFIELD"].ToString() == "$b")
                {
                    exemplar.Fields["929$b"].AFLINKID = (int)row["AFLINKID"];
                }
            }
            try
            {
                exemplar.AccessInfo = BJExemplarInfo.GetExemplarAccess(exemplar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (exemplar.Fields["482$a"].HasValue)//это приплётышь
            {
                BJConvoluteInfo convolute = BJExemplarInfo.GetConvoluteInfo(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
                //BJExemplarInfo Convolute = BJExemplarInfo.GetExemplarByInventoryNumber(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
                if (convolute != null)//нашёлся конволют
                {
                    exemplar.ConvolutePin = $"{convolute.Fund}_{convolute.IDMAIN}";
                    exemplar.ConvoluteIdData = convolute.IDDATA;
                }
                else//не нашёлся конволют
                {
                    exemplar.ConvolutePin = null;
                }
            }
            else
            {
                //это не приплётышь ConvolutePin 
                exemplar.ConvolutePin = null;
            }


            exemplar.Cipher = string.IsNullOrEmpty(exemplar.Fields["899$j"].ToString()) ? dbw.GetCipher(exemplar.Fields["899$b"].ToString(), exemplar.IDMAIN) : exemplar.Fields["899$j"].ToString();
            exemplar.Bar = exemplar.Fields["899$w"].ToString();
            exemplar.InventoryNumber = string.IsNullOrWhiteSpace(exemplar.Fields["899$p"].ToString()) ?
                                        exemplar.Fields["899$w"].ToString() : exemplar.Fields["899$p"].ToString();
            exemplar.Author = BJBookInfo.GetFieldValue(exemplar.Fund, BookBase.GetPIN(exemplar.BookId), 700, "$a");
            exemplar.Title = BJBookInfo.GetFieldValue(exemplar.Fund, BookBase.GetPIN(exemplar.BookId), 200, "$a");
            exemplar.Rack = exemplar.Fields["899$c"].ToString();
            exemplar.Location = exemplar.Fields["899$a"].ToString();
            exemplar.Language = BJBookInfo.GetFieldValue(exemplar.Fund, BookBase.GetPIN(exemplar.BookId), 101, "$a");
            exemplar.PublicationClass = exemplar.Fields["921$c"].ToString();
            exemplar.circulation = new BJCirculationManager();
            exemplar.simpleViewer = new BJExemplarSimpleViewer();
            return exemplar;
        }

        internal BJExemplarInfo GetExemplarByBar(string bar)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper("");
            DataTable table = dbw.GetExemplarIdByBar(bar);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            DataRow r = table.Rows[0];
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData((int)r["exemplarId"], r["fund"].ToString());
            return exemplar;
        }

        internal BJExemplarInfo GetExemplarByInventoryNumber(string inv)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper("");
            DataTable table = dbw.GetExemplarIdByInventoryNumber(inv);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            DataRow r = table.Rows[0];
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData((int)r["exemplarId"], r["fund"].ToString());
            return exemplar;
        }

        internal bool IsExistsDigitalCopy(string BookId)
        {
            DataTable table = dbWrapper.IsExistsDigitalCopy(BookId);
            return (table.Rows.Count == 0)? false : true;
        }

        internal string GetAuthor(int idMain)
        {
            DataTable table = dbWrapper.GetAuthor(idMain);
            string result = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                result += $"{row[0].ToString()};";
            }
            return (result.Length == 0)? result : result.Remove(result.Length - 1);
        }
        internal string GetTitle(int idMain)
        {
            DataTable table = dbWrapper.GetTitle(idMain);
            string result = $"{table.Rows[0]["PLAIN"].ToString()}";
            return result;
        }
    }
}
