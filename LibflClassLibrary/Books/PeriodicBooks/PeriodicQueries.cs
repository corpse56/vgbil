using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodBooks
{
    //коды полей:
    //128 - язык
    //121 - заглавие
    //120 - пин
    //219 - гиперссылк
    //240 - состав комплекта
    //363 - носитель
    //131 - год издания
    //211 - номер копии
    //124 - вид издания
    //352 - Инвентарный номер     AF
    //206	- Инвентарный номер комплекта PI


    class PeriodicQueries
    {
        public string GET_BOOK_BY_BAR
        {
            get
            {
                return "  select 'PERIOD_'+pin.POLE pin,title.POLE title, " +
                       "  god.POLE pubYear, nomer.POLE number, bar.POLE bar, bar.IDZ exemplarId  " +
                       "  from PERIOD..PI bar " +
                       "  left join PERIOD..PI nomer on bar.VVERH = nomer.IDZ " +
                       "   left join PERIOD..PI tom on nomer.VVERH = tom.IDZ " +
                       "   left join PERIOD..PI god on tom.VVERH = god.IDZ " +
                       "   left join PERIOD..PI pin on pin.IDZ = god.VVERH and pin.IDF = 120 " +
                       "   left join PERIOD..PI title on title.VVERH = pin.IDZ and title.IDF = 121 " +
                       "   where bar.POLE = '@bar' and bar.IDF = 243 ";

            }
        }

    }
}
