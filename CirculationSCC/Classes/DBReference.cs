using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class DBReference:DB
    {
        public DBReference()
        { }
        internal DataTable GetAllIssuedBook()
        {
            DA.SelectCommand.CommandText = "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ЦСК' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome" +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJSCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJSCC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJSCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS in (1,6) and A.BaseId = 1 "+
                " union all "+
                "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ОФ' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome" +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS in (1,6) and A.BaseId = 2 ";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];

        }



        internal object GetAllOverdueBook()
        {
            DA.SelectCommand.CommandText = "select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ЦСК' fund " +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJSCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJSCC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS EM on EM.IDISSUED_SCC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                           " and EM.ID = (select max(z.ID) from Reservation_R..ISSUED_SCC_ACTIONS z where z.IDISSUED_SCC = A.IDREADER and z.IDACTION = 4)" +
                " left join BJSCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS in (1,6) and A.BaseId = 1 and A.DATE_RETURN < getdate() " +
                " union all "+
                " select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ОФ' fund " +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS EM on EM.IDISSUED_SCC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS in (1,6) and A.BaseId = 2 and A.DATE_RETURN < getdate()";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetReaderHistory(ReaderVO reader)
        {
            DA.SelectCommand.CommandText = "with hist as (select 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJSCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJSCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS ret on ret.IDISSUED_SCC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId = 1 and A.IDREADER = "+reader.ID+
                " union all "+
                "select 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS ret on ret.IDISSUED_SCC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId =2 and A.IDREADER = " + reader.ID + ") select * from hist order by DATE_ISSUE desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetAllBooks()
        {
            DA.SelectCommand.CommandTimeout = 1200;
            DA.SelectCommand.CommandText =
                "with vvv as ( " +
                "select A.ID IDMAIN,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt, " +
                " BAR.SORT  collate cyrillic_general_ci_ai bar, 'Основной фонд' fund, INV.SORT collate cyrillic_general_ci_ai inv, " +
                " TEMAP.PLAIN collate cyrillic_general_ci_ai tema, CLCP.PLAIN collate cyrillic_general_ci_ai clc" +
                " from BJVVV..MAIN A " +
                " left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                " left join BJVVV..DATAEXT DD on DD.ID = " +
                " ( select top 1 ID from BJVVV..DATAEXT where A.ID = IDMAIN and MNFIELD = 700 and MSFIELD = '$a' ) " +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                " left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p' " +
                " left join BJVVV..DATAEXT BAR on A.ID = BAR.IDMAIN and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and BAR.IDDATA = INV.IDDATA  " +
                " left join BJVVV..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e' " +
                " left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID " +
                " left join BJVVV..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c'  " +
                " left join BJVVV..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a' " +
                " left join BJVVV..DATAEXT CLC on INV.IDDATA = CLC.IDDATA and CLC.MNFIELD = 899 and CLC.MSFIELD = '$b' " +
                " left join BJVVV..DATAEXTPLAIN CLCP on CLCP.IDDATAEXT = CLC.ID " +
                " where FF.IDINLIST = 61  " +
                " ), " +
                " scc as ( " +
                "select A.ID IDMAIN,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt, " +
                " BAR.SORT  collate cyrillic_general_ci_ai bar, 'Фонд ЦСК' fund, INV.SORT collate cyrillic_general_ci_ai inv, " +
                " TEMAP.PLAIN collate cyrillic_general_ci_ai tema, CLCP.PLAIN clc   " +
                " from BJSCC..MAIN A " +
                " left join BJSCC..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                " left join BJSCC..DATAEXT DD on DD.ID =  " +
                " ( select top 1 ID from BJSCC..DATAEXT where A.ID = IDMAIN and MNFIELD = 700 and MSFIELD = '$a' ) " +
                " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
                " left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                " left join BJSCC..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p' " +
                " left join BJSCC..DATAEXT BAR on A.ID = BAR.IDMAIN and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and BAR.IDDATA = INV.IDDATA  " +
                " left join BJSCC..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e' " +
                " left join BJSCC..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID " +
                " left join BJSCC..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c'  " +
                " left join BJSCC..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a' " +
                " left join BJSCC..DATAEXT CLC on INV.IDDATA = CLC.IDDATA and CLC.MNFIELD = 899 and CLC.MSFIELD = '$b' " +
                " left join BJSCC..DATAEXTPLAIN CLCP on CLCP.IDDATAEXT = CLC.ID " +
                " where FF.IDINLIST = 61  " +
                " ), " +
                "prexmlVVV as( " +
                "select A.IDMAIN,B.NAME " +
                "from BJVVV..DATAEXT A " +
                "left join BJVVV..LIST_1 B on A.IDINLIST = B.ID " +
                "where MNFIELD = 101 and MSFIELD = '$a' and IDMAIN in (select IDMAIN from vvv) " +
                "), " +
                "xml101aVVV as " +
                "( " +
                "select  A1.IDMAIN, " +
                "        (select A2.NAME+ '; '  " +
                "        from prexmlVVV A2  " +
                "        where A1.IDMAIN = A2.IDMAIN  " +
                "        for XML path('') " +
                "        ) vaj " +
                "from prexmlVVV A1  " +
                "group by A1.IDMAIN " +
                "), " +
                "prexmlSCC as( " +
                "select A.IDMAIN,B.NAME " +
                "from BJVVV..DATAEXT A " +
                "left join BJVVV..LIST_1 B on A.IDINLIST = B.ID " +
                "where MNFIELD = 101 and MSFIELD = '$a' and IDMAIN in (select IDMAIN from vvv) " +
                "), " +
                "xml101aSCC as " +
                "( " +
                "select  A1.IDMAIN, " +
                "        (select A2.NAME+ '; '  " +
                "        from prexmlSCC A2  " +
                "        where A1.IDMAIN = A2.IDMAIN  " +
                "        for XML path('') " +
                "        ) vaj " +
                "from prexmlSCC A1  " +
                "group by A1.IDMAIN " +
                ") " +
                "select vvv.IDMAIN, tit, avt, bar,  fund, inv, tema, vaj, clc  from vvv " +
                "left join xml101aVVV on vvv.IDMAIN = xml101aVVV.IDMAIN "+
                "union all " +
                "select scc.IDMAIN, tit, avt, bar,  fund, inv, tema, vaj, clc  from scc " +
                "left join xml101aSCC on scc.IDMAIN = xml101aSCC.IDMAIN ";


                //"select 1 ID, C.PLAIN collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
                //" BAR.SORT  collate cyrillic_general_ci_ai bar, 'Основной фонд' fund, INV.SORT inv, TEMAP.PLAIN tema, LANGP.PLAIN lang " +
                //" from BJSCC..MAIN A" +
                //" left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXT DD on A.ID = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                //" left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                //" left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p'" +
                //" left join BJVVV..DATAEXT BAR on A.ID = BAR.IDMAIN and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w'" +
                //" left join BJVVV..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
                //" left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
                //" left join BJVVV..DATAEXT LANG on A.ID = LANG.IDMAIN and LANG.MNFIELD = 101 and LANG.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXTPLAIN LANGP on LANGP.IDDATAEXT = LANG.ID" +
                //" left join BJSCC..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
                ////" where INV.SORT is not null and klass.SORT='Длявыдачи'"+
                //" union all "+
                //"select 1 ID,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
                //" BAR.SORT  collate cyrillic_general_ci_ai bar, 'Основной фонд' fund, INV.SORT inv, TEMAP.PLAIN tema, LANGP.PLAIN lang " +
                //" from BJVVV..MAIN A" +
                //" left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXT DD on A.ID = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                //" left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                //" left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p'" +
                //" left join BJVVV..DATAEXT BAR on A.ID = BAR.IDMAIN and BAR.MNFIELD = 899 and BAR.MSFIELD = '$w' and BAR.IDDATA = INV.IDDATA " +
                //" left join BJVVV..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
                //" left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
                //" left join BJVVV..DATAEXT LANG on A.ID = LANG.IDMAIN and LANG.MNFIELD = 101 and LANG.MSFIELD = '$a'" +
                //" left join BJVVV..DATAEXTPLAIN LANGP on LANGP.IDDATAEXT = LANG.ID" +
                //" left join BJVVV..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
                //" left join BJVVV..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a'" +
                //" where FF.IDINLIST = 61 "; //and klass.SORT='Длявыдачи'
            //спросить какой класс издания для них считается нормальным
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetBookNegotiability()
        {
            DA.SelectCommand.CommandText = "with F1 as  "+
                                           " ( "+
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_SCC_ACTIONS A "+
                                           " left join Reservation_R..ISSUED_SCC B on B.ID = A.IDISSUED_SCC "+
                                           " where A.IDACTION = 2 and B.BaseId = 1 "+
                                           " group by B.IDDATA " +
                                           " ), scc as ( "+
                                           " select distinct 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt, " +
                                           " INV.SORT collate Cyrillic_general_ci_ai inv,A.cnt, 'ЦСК' fund" +
                                           "  from F1 A "+
                                           " left join BJSCC..DATAEXT idm on A.IDDATA = idm.IDDATA " +
                                           " left join BJSCC..DATAEXT CC on idm.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                                           "  left join BJSCC..DATAEXT DD on idm.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
                                           " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID "+
                                           "  left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJSCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'"+
                                           "), "+
                                           " F2 as  "+
                                           " ( "+
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_SCC_ACTIONS A "+
                                           " left join Reservation_R..ISSUED_SCC B on B.ID = A.IDISSUED_SCC "+
                                           " where A.IDACTION = 2 and B.BaseId = 2 "+
                                           " group by B.IDDATA " +
                                           " ), vvv as ( "+
                                           " select distinct 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt, " +
                                           " INV.SORT collate Cyrillic_general_ci_ai inv,A.cnt , 'ОФ' fund" +
                                           "  from F2 A "+
                                           " left join BJVVV..DATAEXT idm on A.IDDATA = idm.IDDATA " +
                                           " left join BJVVV..DATAEXT CC on idm.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                                           "  left join BJVVV..DATAEXT DD on idm.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID "+
                                           "  left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'"+
                                           ") "+
                                           " select * from scc "+
                                           " union all "+
                                           " select * from vvv "+ 
                                           " order by cnt desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        internal object GetBooksWithRemovedResponsibility()
        {
            DA.SelectCommand.CommandText = " select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ЦСК' fund " +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS AA on A.ID = AA.IDISSUED_SCC " +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJSCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJSCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJSCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJSCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where AA.IDACTION = 5 and A.BaseId = 1"+

                " union all "+

                " select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ОФ' fund " +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS AA on A.ID = AA.IDISSUED_SCC " +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where AA.IDACTION = 5 and A.BaseId = 2 ";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];

        }

        internal object GetViolators()
        {
            DA.SelectCommand.CommandText = "select distinct 1,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " (case when (B.Email is null or B.Email = '') then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent " +
                " from Reservation_R..ISSUED_SCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join Reservation_R..ISSUED_SCC_ACTIONS EM on EM.IDISSUED_SCC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate()";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }
    }
}
