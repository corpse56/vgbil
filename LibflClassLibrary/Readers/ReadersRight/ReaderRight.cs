using LibflClassLibrary.Readers.ReadersRights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.ReadersRight
{
    public class ReaderRight
    {
        //public string ReaderRightName { get; set; }
        public DateTime DateEndReaderRight { get; set; }
        public int IDOrganization { get; set; } // если права сотрудника, то  в этом поле ID отдела, в котором он работает

        public ReaderRightsEnum ReaderRightValue;

        public override string ToString()
        {
            string result = null;
            switch (this.ReaderRightValue)
            {
                case ReaderRightsEnum.None:
                    result = "Права не указаны";
                    break;
                case ReaderRightsEnum.BritSovet:
                    result = "Пользователь британского совета до " +DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.ReadingRoomUser:
                    result = "Пользователь читальных залов ВГБИЛ до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.Employee:
                    result = "Сотрудник ВГБИЛ до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.FreeAbonement:
                    result = "Бесплатный абонемент до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.PaidAbonement:
                    result = "Платный абонемент до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.CollectiveAbonement:
                    result = "Коллективный абонемент до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
                case ReaderRightsEnum.Partner:
                    result = "Сотрудник-партнёр до " + DateEndReaderRight.ToString("dd.MM.yyyy");
                    break;
            }
            return result;
        }
    }
}
