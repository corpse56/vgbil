using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    public class DBReference : DB
    {
        public DBReference()
        { }
        public DataTable GetAllIssuedBook()
        {
            DA.SelectCommand.CommandText = "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ФКЦ' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome" +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJFCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJFCC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJFCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS in (1,6) and A.BaseId = 1 " +
                " union all " +
                "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ОФ' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome" +
                " from Reservation_R..ISSUED_FCC A" +
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



        public object GetAllOverdueBook()
        {
            DA.SelectCommand.CommandText = "select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ФКЦ' fund, F.PLAIN rack " +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJFCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJFCC..DATAEXT FF on A.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$c'" +
                " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJFCC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJFCC..DATAEXTPLAIN F on F.IDDATAEXT = FF.ID" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS EM on EM.ID = (select top 1 ID from Reservation_R..ISSUED_FCC_ACTIONS Z " +
                                                                            " where Z.IDISSUED_FCC = A.IDREADER and Z.IDACTION = 4 " + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                                                                            " order by ID desc) " +
                //" and Z.ID = (select max(ID) from Reservation_R..ISSUED_FCC_ACTIONS ZZ where ZZ.IDISSUED_FCC = A.IDREADER and ZZ.IDACTION = 4))" +
                           " and EM.ID = (select max(z.ID) from Reservation_R..ISSUED_FCC_ACTIONS z where z.IDISSUED_FCC = A.IDREADER and z.IDACTION = 4)" +
                " left join BJFCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate() " +
                " union all " +
                " select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ОФ' fund, F.PLAIN rack " +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJVVV..DATAEXT FF on A.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$c'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJVVV..DATAEXTPLAIN F on F.IDDATAEXT = FF.ID" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS EM on EM.ID = (select top 1 ID from Reservation_R..ISSUED_FCC_ACTIONS Z " +
                                                                            " where Z.IDISSUED_FCC = A.IDREADER and Z.IDACTION = 4 " + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                                                                            " order by ID desc) " +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 6 and A.DATE_RETURN < getdate()";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetReaderHistory(ReaderVO reader)
        {
            DA.SelectCommand.CommandText = "with hist as (select 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJFCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJFCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS ret on ret.IDISSUED_FCC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId = 1 and A.IDREADER = " + reader.ID +
                " union all " +
                "select 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS ret on ret.IDISSUED_FCC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId =2 and A.IDREADER = " + reader.ID + ") select * from hist order by DATE_ISSUE desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetAllBooks()
        {
            DA.SelectCommand.CommandText =
                " with S0 as " +
                " ( " +
               " select 1 ID,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt, " +
               " INV.SORT  collate cyrillic_general_ci_ai inv, 'Основной фонд' fund , A.ID IDMAIN  " +
               " ,cipherP.PLAIN  collate cyrillic_general_ci_ai cipher, " +
               " case when iss.IDSTATUS in (1,6) then 'занято' else 'свободно' end sts " +
               " ,TEMAP.PLAIN tema, POLKAP.PLAIN rack " +
               "  from BJVVV..MAIN A " +
               "  left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
               "  left join BJVVV..DATAEXT DD on A.ID = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
               "  left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
               "  left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
               "  left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$p' " +
               "  left join BJVVV..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c'  " +
               "  left join BJVVV..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a' " +
               "  left join BJVVV..DATAEXT cipher on cipher.ID = (select top 1 ID from BJVVV..DATAEXT  " +
               "             where MNFIELD = 899 and MSFIELD = '$j' and IDDATA = INV.IDDATA)  " +
               "  left join BJVVV..DATAEXTPLAIN cipherP on cipherP.IDDATAEXT = cipher.ID " +
               "  left join Reservation_R..ISSUED_FCC iss on iss.ID = (select top 1 ID from Reservation_R..ISSUED_FCC iss   " +
               "              where IDDATA = INV.IDDATA order by ID desc)  " +
               "  left join BJVVV..DATAEXT TEMA on TEMA.ID = (select top 1 ID from BJVVV..DATAEXT  " +
               "             where MNFIELD = 922 and MSFIELD = '$e' and IDMAIN = INV.IDMAIN )  " +
               "  left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID  " +
               " left join BJVVV..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
               " left join BJVVV..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
               " where INV.SORT is not null  and FF.IDINLIST = 60  " +
            "  ), " +
            " prelang as(  " +
            " select A.IDMAIN,B.PLAIN   " +
            " from BJVVV..DATAEXT A  " +
            " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT  " +
            " where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDMAIN from S0)  " +
            " ),  " +
            " lang as  " +
            " (  " +
            " select  A1.IDMAIN,  " +
            "         (select A2.PLAIN+ '; '   " +
            "         from prelang A2   " +
            "         where A1.IDMAIN = A2.IDMAIN   " +
            "         for XML path('')  " +
            "         ) lng  " +
            " from prelang A1   " +
            " group by A1.IDMAIN  " +
            " ) , " +
            " S1 as " +
            " ( " +
            " select 1 ID,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt, " +
            " INV.SORT  collate cyrillic_general_ci_ai inv, 'Французский культурный центр' fund , A.ID IDMAIN,  " +
            " cipherP.PLAIN  collate cyrillic_general_ci_ai cipher, " +
            " case when iss.IDSTATUS in (1,6) then 'занято' else 'свободно' end sts, " +
            " TEMAP.PLAIN tema, POLKAP.PLAIN rack " +
            "  from BJFCC..MAIN A " +
            "  left join BJFCC..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
            "  left join BJFCC..DATAEXT DD on A.ID = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
            "  left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
            "  left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
            "  left join BJFCC..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w' " +
            "  left join BJFCC..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c'  " +
            "  left join BJFCC..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a' " +
            "  left join BJFCC..DATAEXT cipher on cipher.ID = (select top 1 ID from BJFCC..DATAEXT  " +
            "             where MNFIELD = 899 and MSFIELD = '$j' and IDDATA = INV.IDDATA)  " +
            "  left join BJFCC..DATAEXTPLAIN cipherP on cipherP.IDDATAEXT = cipher.ID " +
            "  left join Reservation_R..ISSUED_FCC iss on iss.ID = (select top 1 ID from Reservation_R..ISSUED_FCC iss   " +
            "              where IDDATA = INV.IDDATA order by ID desc)  " +
            "  left join BJFCC..DATAEXT TEMA on TEMA.ID = (select top 1 ID from BJFCC..DATAEXT  " +
            "             where MNFIELD = 922 and MSFIELD = '$e' and IDMAIN = INV.IDMAIN )  " +
            "  left join BJFCC..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID  " +
            " left join BJFCC..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
            " left join BJFCC..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
            " where INV.SORT is not null   " +
            "  ), " +
            " prelangF as(  " +
            " select A.IDMAIN,B.PLAIN   " +
            " from BJFCC..DATAEXT A  " +
            " left join BJFCC..DATAEXTPLAIN B on A.ID = B.IDDATAEXT  " +
            " where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDMAIN from S1) " +
            " ),  " +
            " langF as  " +
            " (  " +
            " select  A1.IDMAIN,  " +
            "         (select A2.PLAIN+ '; '   " +
            "         from prelangF A2   " +
            "         where A1.IDMAIN = A2.IDMAIN   " +
            "         for XML path('')  " +
            "         ) lng  " +
            " from prelangF A1   " +
            " group by A1.IDMAIN  " +
            " )  " +
            " , final as " +
            " ( " +
            " select 1 ID, A.tit, A.avt, A.inv, A.fund, B.lng, A.cipher, A.sts, A.tema, A.rack " +
            " from S0 A " +
            " left join lang B on A.IDMAIN = B.IDMAIN " +
            " union all " +
            " select 1 ID, A.tit, A.avt, A.inv, A.fund, B.lng, A.cipher, A.sts, A.tema, A.rack " +
            " from S1 A " +
            " left join langF B on A.IDMAIN = B.IDMAIN " +
            " ) " +
            " select * from final"; 
            
            //DA.SelectCommand.CommandText =
            //                    "select 1 ID, C.PLAIN collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
            //    " INV.SORT  collate cyrillic_general_ci_ai inv, 'Центр Американской Культуры' fund, TEMAP.PLAIN tema, POLKAP.PLAIN polka " +
            //    " from BJFCC..MAIN A" +
            //    " left join BJFCC..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
            //    " left join BJFCC..DATAEXT DD on DD.ID = (select top 1 Z.ID from BJFCC..DATAEXT Z where A.ID = Z.IDMAIN and Z.MNFIELD = 700 and Z.MSFIELD = '$a')" +
            //    " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
            //    " left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
            //    " left join BJFCC..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
            //    " left join BJFCC..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
            //    " left join BJFCC..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
            //    " left join BJFCC..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
            //    " left join BJFCC..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
            //    " left join BJFCC..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
            //    " where INV.SORT is not null " +//and klass.SORT='Длявыдачи'" +

            //    " union all " +

            //    "select 1 ID,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
            //    " INV.SORT  collate cyrillic_general_ci_ai inv, 'Основной фонд' fund , TEMAP.PLAIN tema, POLKAP.PLAIN polka " +
            //    " from BJVVV..MAIN A " +
            //    " left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
            //    " left join BJVVV..DATAEXT DD on DD.ID = (select top 1 Z.ID from BJVVV..DATAEXT Z where A.ID = Z.IDMAIN and Z.MNFIELD = 700 and Z.MSFIELD = '$a')" +
            //    " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
            //    " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
            //    " left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
            //    " left join BJVVV..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
            //    " left join BJVVV..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
            //    " left join BJVVV..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
            //    " left join BJVVV..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
            //    " left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
            //    " left join BJVVV..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a'" +
            //    " where INV.SORT is not null  and FF.IDINLIST = 52 ";//and klass.SORT='Длявыдачи'";
            ////спросить какой класс издания для них считается нормальным

            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetBookNegotiability()
        {
            DA.SelectCommand.CommandText = "with F1 as  " +
                                           " ( " +
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_FCC_ACTIONS A " +
                                           " left join Reservation_R..ISSUED_FCC B on B.ID = A.IDISSUED_FCC " +
                                           " where A.IDACTION = 2 and B.BaseId = 1 " +
                                           " group by B.IDDATA " +
                                           " ), FCC as ( " +
                                           " select distinct 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt, " +
                                           " INV.SORT collate Cyrillic_general_ci_ai inv,A.cnt, 'ФКЦ' fund" +
                                           "  from F1 A " +
                                           " left join BJFCC..DATAEXT idm on A.IDDATA = idm.IDDATA " +
                                           " left join BJFCC..DATAEXT CC on idm.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                                           "  left join BJFCC..DATAEXT DD on idm.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
                                           " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
                                           "  left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJFCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                                           "), " +
                                           " F2 as  " +
                                           " ( " +
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_FCC_ACTIONS A " +
                                           " left join Reservation_R..ISSUED_FCC B on B.ID = A.IDISSUED_FCC " +
                                           " where A.IDACTION = 2 and B.BaseId = 2 " +
                                           " group by B.IDDATA " +
                                           " ), vvv as ( " +
                                           " select distinct 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt, " +
                                           " INV.SORT collate Cyrillic_general_ci_ai inv,A.cnt , 'ОФ' fund" +
                                           "  from F2 A " +
                                           " left join BJVVV..DATAEXT idm on A.IDDATA = idm.IDDATA " +
                                           " left join BJVVV..DATAEXT CC on idm.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                                           "  left join BJVVV..DATAEXT DD on idm.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
                                           " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
                                           "  left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                                           ") " +
                                           " select * from FCC " +
                                           " union all " +
                                           " select * from vvv " +
                                           " order by cnt desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetBooksWithRemovedResponsibility()
        {
            DA.SelectCommand.CommandText = " select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ФКЦ' fund " +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS AA on A.ID = AA.IDISSUED_FCC " +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJFCC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJFCC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJFCC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJFCC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where AA.IDACTION = 5 and A.BaseId = 1" +

                " union all " +

                " select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ОФ' fund " +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS AA on A.ID = AA.IDISSUED_FCC " +
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

        public object GetViolators()
        {
            DA.SelectCommand.CommandText = "with vio as (select distinct 1 nn,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " (case when (B.Email is null or B.Email = '') then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent " +
                " from Reservation_R..ISSUED_FCC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join Reservation_R..ISSUED_FCC_ACTIONS EM on EM.IDISSUED_FCC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate() and " +
                " (EM.DATEACTION = (select max(DATEACTION) from Reservation_R..ISSUED_FCC_ACTIONS where IDISSUED_FCC = A.IDREADER and IDACTION = 4) " +
                " or EM.DATEACTION is null) )" +
                " select * from vio";
            //" select * from vio where emailsent = (select max)";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }
    }
}
