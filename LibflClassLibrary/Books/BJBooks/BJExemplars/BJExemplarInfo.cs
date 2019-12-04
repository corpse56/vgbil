using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.Books.BJBooks.Loaders;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Data;
using System.Diagnostics;

namespace LibflClassLibrary.Books.BJBooks.BJExemplars
{
    /// <summary>
    /// Сводное описание для ExemplarInfo
    /// </summary>
    public class BJExemplarInfo : ExemplarBase
    {
        public BJExemplarInfo() { }
        public BJExemplarInfo(int idData)
        {
            this._iddata = idData;
            this.Id = idData.ToString();
        }

        private int _iddata;
        public int IdData
        {
            get
            {
                return _iddata;
            }
        }

        public int IDMAIN { get; set; }
        public bool IsAlligat { get; set; }
        public string ConvolutePin { get; set; }
        public int ConvoluteIdData { get; set; }

        public DateTime Created; //для новых поступлений. Дата присвоения инвентарного номера.
        public BJFields Fields = new BJFields();
        public ExemplarAccessInfo ExemplarAccess = new ExemplarAccessInfo();

        //virtual properties
        public override string Cipher { get; set; }
        public override string InventoryNumber { get; set; }
        public override string Title { get; set; }
        public override string Rack { get; set; }
        public override string Bar { get; set; }
        public override string Location { get; set; }
        public override string PublicationClass { get; set; }

        public static BJExemplarInfo GetExemplarByInventoryNumber(string inv)
        {
            BJExemplarLoader loader = new BJExemplarLoader("");
            BJExemplarInfo result = loader.GetExemplarByInventoryNumber(inv);
            return result;
        }
        public static BJExemplarInfo GetExemplarByBar(string bar)
        {
            BJExemplarLoader loader = new BJExemplarLoader("");
            BJExemplarInfo result = loader.GetExemplarByBar(bar);
            return result;
        }

        public static BJExemplarInfo GetExemplarByIdData(int exemplarId, string fund)
        {
            BJExemplarLoader loader = new BJExemplarLoader(fund);
            BJExemplarInfo result = loader.GetExemplarByIdData(exemplarId, fund);
            return result;
        }

        public static BJConvoluteInfo GetConvoluteInfo(string InventoryNumber, string fund)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            //DataTable table = dbw.GetExemplar(InventoryNumber);
            DataTable table = dbw.GetConvolute(InventoryNumber);
            sw.Stop();
            if (table.Rows.Count == 0)
            {
                return null;
            }
            BJConvoluteInfo convolute = new BJConvoluteInfo();
            convolute.Fund = fund;
            convolute.IDDATA = (int)table.Rows[0]["IDDATA"];
            convolute.IDMAIN = (int)table.Rows[0]["IDMAIN"];
            convolute.BookId = $"{convolute.Fund}_{convolute.IDMAIN.ToString()}";

            return convolute;

        }

        public static ExemplarAccessInfo GetExemplarAccess(BJExemplarInfo exemplar)
        {

            ExemplarAccessInfo access = new ExemplarAccessInfo();
            //сначала суперусловия
            if (exemplar.Fields["899$x"].ToString().ToLower().Contains("э"))
            {
                access.Access = 1020;//такого в таблице нет. это только здесь. означает экстремистскую литературу.
                access.MethodOfAccess = 4005;
                return access;
            }

            switch (exemplar.Fund)
            {
                case "BJVVV":
                    if (exemplar.Fields["482$a"].ToLower() != "")
                    {
                        BJExemplarInfo Convolute = BJExemplarInfo.GetExemplarByInventoryNumber(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
                        if (Convolute != null)
                        {
                            access.MethodOfAccess = Convolute.ExemplarAccess.MethodOfAccess;
                            access.Access = Convolute.ExemplarAccess.Access;
                        }
                        else
                        {
                            access.Access = 1017;
                            access.MethodOfAccess = 4005;
                        }
                    } else
                    if ((exemplar.Fields["899$b"].ToLower() == "абонемент") && (!exemplar.Fields["899$a"].ToLower().Contains("книгохране")) && (exemplar.Fields["899$a"].ToLower().Contains("абонем")))
                    {
                        access.Access = 1006;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    if ((exemplar.Fields["899$b"].ToLower() == "вх") && (!exemplar.Fields["899$a"].ToLower().Contains("книгохране")) && (exemplar.Fields["899$a"].ToLower().Contains("абонем")))
                    {
                        access.Access = 1006;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if ((exemplar.Fields["899$b"].ToLower() == "абонемент") && (exemplar.Fields["899$a"].ToLower().Contains("книгохране")) && (exemplar.Fields["899$a"].ToLower().Contains("абонем")))
                    {
                        access.Access = 1000;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if ((exemplar.Fields["899$a"].ToLower().Contains("книгохране")) && (exemplar.Fields["899$a"].ToLower().Contains("абонем")))
                    {
                        access.Access = 1000;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if (((exemplar.Fields["899$a"].ToLower().Contains("славянс")) || (exemplar.Fields["899$a"].ToLower().Contains("франко"))
                            || (exemplar.Fields["899$a"].ToLower().Contains("детск")) || (exemplar.Fields["899$a"].ToLower().Contains("америка")))
                            && (exemplar.Fields["899$b"].ToLower() != "вх"))
                    {
                        access.Access = 1007;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else if (((exemplar.Fields["899$a"].ToLower().Contains("славянс")) || (exemplar.Fields["899$a"].ToLower().Contains("франко")) 
                            || (exemplar.Fields["899$a"].ToLower().Contains("детск")) || (exemplar.Fields["899$a"].ToLower().Contains("америка")))
                            && (exemplar.Fields["899$b"].ToLower() == "вх"))
                    {
                        access.Access = 1006;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() == "Для выдачи") && (exemplar.Fields["899$a"].ToLower().Contains("книгохране")))
                    {
                        access.Access = 1005;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() == "Для выдачи") && (exemplar.Fields["899$a"].ToLower().Contains("франко")))
                    {
                        access.Access = 1006;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() == "Для выдачи") && (exemplar.Fields["899$a"].ToLower().Contains("абонемен")) && (!exemplar.Fields["899$a"].ToLower().Contains("книгохр")))
                    {
                        access.Access = 1006;
                        access.MethodOfAccess = 4001;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() == "ДП")
                            && (KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()] != "Служебные подразделения"))
                    {
                        access.Access = 1007;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() == "ДП")
                            && (KeyValueMapping.UnifiedLocationAccess[exemplar.Fields["899$a"].ToString()] == "Служебные подразделения"))
                    {
                        access.Access = 1013;
                        access.MethodOfAccess = 4005;
                        return access;
                    }
                    else if (exemplar.Fields["921$c"].ToString() == "Выставка")
                    {
                        access.Access = 1011;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else if ((exemplar.Fields["921$c"].ToString() != "Для выдачи") && 
                             (exemplar.Fields["921$c"].ToString() != "Выставка") &&
                             (exemplar.Fields["921$c"].ToString() != "Перевод в другой фонд") &&
                             (exemplar.Fields["921$c"].ToString() != "")
                        )
                    {
                        access.Access = 1013;
                        access.MethodOfAccess = 4005;
                        return access;
                    }
                    else if (
                                    (exemplar.Fields["899$b"].ToLower() == "спв") || (!exemplar.Fields["921$a"].ToLower().Contains("бумага"))        
                                &&
                                    (exemplar.Fields["899$a"].ToLower().Contains("книгохране"))
                            )
                    {
                        access.Access = 1012;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else if (exemplar.Fields["921$d"].ToLower() == "на усмотрение сотрудника")
                    {
                        access.Access = 1010;
                        access.MethodOfAccess = 4005;
                    }
                    else
                    {
                        access.Access = 1999;
                        access.MethodOfAccess = 4005;
                    }
                    break;
                case "REDKOSTJ":
                    if (exemplar.Fields["482$a"].ToLower() != "")
                    {
                        BJExemplarInfo Convolute = BJExemplarInfo.GetExemplarByInventoryNumber(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
                        if (Convolute == null)
                        {
                            access.Access = 1016;
                            access.MethodOfAccess = 4005;
                        }
                        else
                        {
                            access.Access = Convolute.ExemplarAccess.Access;
                            access.MethodOfAccess = Convolute.ExemplarAccess.MethodOfAccess;
                        }

                    } 
                    else if (exemplar.Fields["899$a"].ToLower().Contains("зал"))
                    {
                        access.Access = 1007;
                        access.MethodOfAccess = 4000;
                    }
                    else if (exemplar.Fields["899$a"].ToLower().Contains("хранения"))
                    {
                        access.Access = 1014;
                        access.MethodOfAccess = 4000;
                    }
                    else if (exemplar.Fields["899$a"].ToLower().Contains("обраб"))
                    {
                        access.Access = 1013;
                        access.MethodOfAccess = 4005;
                    }
                    else if (exemplar.Fields["921$c"].ToString() == "Выставка")
                    {
                        access.Access = 1011;
                        access.MethodOfAccess = 4000;
                        return access;
                    }
                    else
                    {
                        access.Access = 1999;
                        access.MethodOfAccess = 4005;
                    }
                    break;
                case "BJACC":
                    access.Access = 1006;
                    access.MethodOfAccess = 4001;
                    break;
                case "BJFCC":
                    access.Access = 1006;
                    access.MethodOfAccess = 4001;
                    break;
                case "BJSCC":
                    //костыль временно:
                    access.Access = 1010;
                    access.MethodOfAccess = 4005;

                    //то, что должно быть
                    //access.Access = 1007;
                    //access.MethodOfAccess = 4000;
                    break;
                default:
                    access.Access = 1999;
                    access.MethodOfAccess = 4005;
                    break;
            }
            return access;
        }

        public override string Author { get; set; }
        public override string Language { get; set; }

        private string GetAuthor()
        {
            BJExemplarLoader loader = new BJExemplarLoader(this.Fund);
            return loader.GetAuthor(this.IDMAIN);
        }

        private string GetTitle()
        {
            BJExemplarLoader loader = new BJExemplarLoader(this.Fund);
            return loader.GetTitle(this.IDMAIN);
        }

        //public string GetWriteoffReason()
        //{
        //    //BJExemplarLoader loader = new BJExemplarLoader(this.Fund);
        //    //string result = this.Fields["929$b"].ToString();
        //    ////loader.GetWriteoffreason(this.Fields["929$b"].);
        //    //return "dsfsdfsdf";
        //}

        #region эти методы надо выносить в другой класс. Они относятся к книговыдаче
        public bool IsIssuedOrOrderedEmployee()
        {
            switch (this.Fund)
            {
                case "BJVVV":
                    BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
                    DataTable table = dbw.IsIssuedOrOrderedEmployee(this.IDMAIN, this.IdData);
                    return (table.Rows.Count == 0) ? false : true;
                default:
                    return false;
            }
        }

        public bool IsSelfIssuedOrOrderedEmployee(int IdReader)
        {
            switch (this.Fund)
            {
                case "BJVVV":
                    BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
                    DataTable table = dbw.IsSelfIssuedOrOrderedEmployee(this.IdData, this.IDMAIN, IdReader);
                    return (table.Rows.Count == 0) ? false : true;
                default:
                    return false;
            }
        }


        //этот метод закрыть с потрохами
        public bool IsIssuedToReader()
        {
            switch (this.Fund)
            {
                case "BJVVV":
                    BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
                    DataTable table = dbw.IsIssuedToReader(this.IdData);
                    return (table.Rows.Count == 0) ? false : true;
                default:
                    return false;
            }
        }

        public string GetEmployeeStatus()
        {
            switch (this.Fund)
            {
                case "BJVVV":
                    BJDatabaseWrapper dbw = new BJDatabaseWrapper(this.Fund);
                    DataTable table = dbw.GetEmployeeStatus(this.IDMAIN);
                    return (table.Rows.Count == 0) ? "" : table.Rows[0][0].ToString();
                default:
                    return "";
            }
        }
        #endregion
    }
}