using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Readers.ReadersRights
{

    //[Flags]
    public enum ReaderRightsEnum
    {
        None                = 0,
        BritSovet           = 1 << 0,               //Пользователь британского совета
        ReadingRoomUser     = 1 << 1,               //Пользователь читальных залов ВГБИЛ
        Employee            = 1 << 2,               //Сотрудник ВГБИЛ
        FreeAbonement       = 1 << 3,               //Бесплатный абонемент
        PaidAbonement       = 1 << 4,               //Платный абонемент
        CollectiveAbonement = 1 << 5,               //Коллективный абонемент
        Partner             = 1 << 6                //Сотрудник-партнёр
    }

}
