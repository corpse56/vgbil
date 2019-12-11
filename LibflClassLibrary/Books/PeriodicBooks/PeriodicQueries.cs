using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibflClassLibrary.Books.PeriodBooks
{
    //коды полей:
    //243 - штрихкод
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
                return "  select 'PERIOD_'+pin.POLE pin,title.POLE title, inv.KLASS pubClass, " +
                       "  god.POLE pubYear, nomer.POLE number, bar.POLE bar, bar.IDZ exemplarId, " +
                       "  inv.MESTO location, inv.INV_NOM invNumber, inv.DOCTUP access," +
                       "  lang.POLE language, (select top 1 cipher.POLE " +
                       "                       from PERIOD..PI cipher " +
                       "                       where cipher.VVERH = pin.IDZ and cipher.IDF = 239) cipher" +
                       "  from PERIOD..PI bar " +
                       "  left join PERIOD..PI nomer on bar.VVERH = nomer.IDZ " +
                       "  left join PERIOD..PI tom on nomer.VVERH = tom.IDZ " +
                       "  left join PERIOD..PI god on tom.VVERH = god.IDZ " +
                       "  left join PERIOD..PI pin on pin.IDZ = god.VVERH and pin.IDF = 120 " +
                       "  left join PERIOD..PI title on title.VVERH = pin.IDZ and title.IDF = 121 " +
                       "  left join PERIOD..PI lang on pin.IDZ = lang.VVERH and lang.IDF = 128 " +
                       "  left join PERIOD..PEREPLET inv on inv.SHTRIHKOD collate cyrillic_general_ci_ai = bar.POLE " +
                       "  where bar.POLE = @bar and bar.IDF = 243 ";
            }
        }

        public string GET_EXEMPLAR_BAR_BY_INVENTORYNUMBER
        {
            get
            {
                return " select B.POLE bar " +
                       " from PERIOD..PEREPLET A" +
                       " left join PERIOD..PI B on A.INV_NOM collate cyrillic_general_ci_ai = @inventoryNumber " +
                       " where B.IDF = 243";
            }
        }

        public string GET_BAR_BY_EXEMPLARID
        {
            get
            {
                return " select A.POLE bar " +
                       " from PERIOD..PI A" +
                       " where A.IDF = 243 and A.IDZ = @exemplarId";
            }
        }

        //public string GET_BOOK_BY_PIN
        //{
        //    get
        //    {
        //        return "  select 'PERIOD_'+pin.POLE pin,title.POLE title, " +
        //               "  lang.POLE language " +
        //               "  from PERIOD..PI pin " +
        //               "  left join PERIOD..PI title on title.VVERH = pin.IDZ and title.IDF = 121 " +
        //               "  left join PERIOD..PI lang on pin.IDZ = lang.VVERH and lang.IDF = 128 " +
        //               "  where pin.POLE = '@pin' and pin.IDF = 120 ";
        //    }
        //}

        public string GET_BOOK_BY_IDZBar
        {
            get
            {
                return "  select 'PERIOD_'+pin.POLE pin,title.POLE title, inv.KLASS pubClass, " +
                       "  god.POLE pubYear, nomer.POLE number, bar.POLE bar, bar.IDZ exemplarId, " +
                       "  inv.MESTO location, inv.INV_NOM invNumber, inv.DOCTUP access," +
                       "  lang.POLE language, (select top 1 cipher.POLE " +
                       "                       from PERIOD..PI cipher " +
                       "                       where cipher.VVERH = pin.IDZ and cipher.IDF = 239) cipher" +
                       "  from PERIOD..PI bar " +
                       "  left join PERIOD..PI nomer on bar.VVERH = nomer.IDZ " +
                       "  left join PERIOD..PI tom on nomer.VVERH = tom.IDZ " +
                       "  left join PERIOD..PI god on tom.VVERH = god.IDZ " +
                       "  left join PERIOD..PI pin on pin.IDZ = god.VVERH and pin.IDF = 120 " +
                       "  left join PERIOD..PI title on title.VVERH = pin.IDZ and title.IDF = 121 " +
                       "  left join PERIOD..PI lang on pin.IDZ = lang.VVERH and lang.IDF = 128 " +
                       "  left join PERIOD..PEREPLET inv on inv.SHTRIHKOD collate cyrillic_general_ci_ai = bar.POLE " +
                       "  where bar.IDZ = @idzBar and bar.IDF = 243 ";
            }
        }

    }
}
