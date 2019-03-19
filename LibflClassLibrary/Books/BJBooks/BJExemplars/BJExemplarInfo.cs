using LibflClassLibrary.Books.BJBooks.DB;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Data;
using System.Diagnostics;

namespace LibflClassLibrary.Books.BJBooks.BJExemplars
{
    /// <summary>
    /// Сводное описание для ExemplarInfo
    /// </summary>
    public class BJExemplarInfo : BookExemplarBase
    {
        public BJExemplarInfo() { }
        public BJExemplarInfo(int idData)
        {
            this._iddata = idData;
        }

        private int _iddata;
        public int IdData
        {
            get
            {
                return _iddata;
            }
        }

        public string Fund { get; set; }
        public int IDMAIN { get; set; }
        public string BookId
        {
            get
            {
                return $"{Fund}_{IDMAIN.ToString()}";
            }
        }
        public bool IsAlligat { get; set; }
        public string ConvolutePin { get; set; }
        public int ConvoluteIdData { get; set; }
        public string Cipher { get; set; }
        public DateTime Created; //для новых поступлений. Дата присвоения инвентарного номера.

        public BJFields Fields = new BJFields();

        public BJExemplarAccessInfo ExemplarAccess = new BJExemplarAccessInfo(); 


        public static BJExemplarInfo GetExemplarByInventoryNumber(string inv, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(inv);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            BJExemplarInfo exemplar = BJExemplarInfo.GetExemplarByIdData((int)table.Rows[0]["IDDATA"], fund);
            return exemplar;
        }
        public static BJExemplarInfo GetExemplarByIdData(int iddata, string fund)
        {
            BJDatabaseWrapper dbw = new BJDatabaseWrapper(fund);
            DataTable table = dbw.GetExemplar(iddata);
            BJExemplarInfo exemplar = new BJExemplarInfo((int)table.Rows[0]["IDDATA"]);
            exemplar.IDMAIN = (int)table.Rows[0]["IDMAIN"];
            exemplar.Fund = fund;


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
                exemplar.Fields.AddField(row["PLAIN"].ToString(), (int)row["MNFIELD"], row["MSFIELD"].ToString());//добавляем все поля блока 260 к объекту экземпляра
            }
            exemplar.ExemplarAccess = BJExemplarInfo.GetExemplarAccess(exemplar);
            if (exemplar.Fields["482$a"].MNFIELD != 0)//это приплётышь
            {
                BJConvoluteInfo convolute = BJExemplarInfo.GetConvoluteInfo(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
                BJExemplarInfo Convolute = BJExemplarInfo.GetExemplarByInventoryNumber(exemplar.Fields["482$a"].ToString(), exemplar.Fund);
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

            return exemplar;
        }

        private static BJConvoluteInfo GetConvoluteInfo(string InventoryNumber, string fund)
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
            return convolute;

        }

        private static BJExemplarAccessInfo GetExemplarAccess(BJExemplarInfo exemplar)
        {

            BJExemplarAccessInfo access = new BJExemplarAccessInfo();
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