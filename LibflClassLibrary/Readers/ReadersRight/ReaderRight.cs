using LibflClassLibrary.Readers.ReadersRights;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.ReadersRight
{
    public class ReaderRight
    {
        public DateTime DateEndReaderRight { get; set; }
        public int IDOrganization { get; set; } // если права сотрудника, то  в этом поле ID отдела, в котором он работает

        [JsonIgnore]
        public ReaderRightsEnum ReaderRightValue { get; set; }

        public string ReaderRightName
        {
            get
            {
                return this.ToString();
            }
        }

        public override string ToString()
        {
            string result = null;
            switch (this.ReaderRightValue)
            {
                case ReaderRightsEnum.None:
                    result = "Права не указаны";
                    break;
                case ReaderRightsEnum.BritSovet:
                    result = "Пользователь британского совета";
                    break;
                case ReaderRightsEnum.ReadingRoomUser:
                    result = "Пользователь читальных залов ВГБИЛ";
                    break;
                case ReaderRightsEnum.Employee:
                    result = "Сотрудник ВГБИЛ";
                    break;
                case ReaderRightsEnum.FreeAbonement:
                    result = "Бесплатный абонемент";
                    break;
                case ReaderRightsEnum.PaidAbonement:
                    result = "Платный абонемент";
                    break;
                case ReaderRightsEnum.CollectiveAbonement:
                    result = "Коллективный абонемент";
                    break;
                case ReaderRightsEnum.Partner:
                    result = "Сотрудник-партнёр";
                    break;
            }
            return result;
        }
    }
}
