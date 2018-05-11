using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CirculationACC
{
    public class DBReference:DB
    {
        public DBReference()
        { }
        public DataTable GetAllIssuedBook()
        {
            DA.SelectCommand.CommandText = "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ЦАК' fund" +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 1 " +
                " union all " +
                "select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when B.Email is null then 'false' else 'true' end) email, E.PLAIN collate Cyrillic_general_ci_ai shifr, 'ОФ' fund" +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 6 "; DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];

        }



        public object GetAllOverdueBook()
        {
            DA.SelectCommand.CommandText = "select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ЦАК' fund " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS EM on EM.ID = (select top 1 ID from Reservation_R..ISSUED_ACC_ACTIONS Z "+
                                                                            " where Z.IDISSUED_ACC = A.IDREADER and Z.IDACTION = 4 " + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                                                                            " order by ID desc) "+
                                                                            //" and Z.ID = (select max(ID) from Reservation_R..ISSUED_ACC_ACTIONS ZZ where ZZ.IDISSUED_ACC = A.IDREADER and ZZ.IDACTION = 4))" +
                           " and EM.ID = (select max(z.ID) from Reservation_R..ISSUED_ACC_ACTIONS z where z.IDISSUED_ACC = A.IDREADER and z.IDACTION = 4)" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate() " +
                " union all " +
                " select distinct 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,A.DATE_RETURN," +
                " (case when (B.Email is null or B.Email = '')  then 'false' else 'true' end) isemail," +
                " case when EM.DATEACTION is null then 'email не отправлялся' else CONVERT (NVARCHAR, EM.DATEACTION, 104) end emailsent, E.PLAIN collate Cyrillic_general_ci_ai shifr,'ОФ' fund " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT EE on A.IDDATA = EE.IDDATA and EE.MNFIELD = 899 and EE.MSFIELD = '$j'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXTPLAIN E on E.IDDATAEXT = EE.ID" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS EM on EM.ID = (select top 1 ID from Reservation_R..ISSUED_ACC_ACTIONS Z " +
                                                                            " where Z.IDISSUED_ACC = A.IDREADER and Z.IDACTION = 4 " + // 4 - это ACTIONTYPE = сотрудник отослал емаил
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
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS ret on ret.IDISSUED_ACC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId = 1 and A.IDREADER = " + reader.ID +
                " union all " +
                "select 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,ret.DATEACTION DATE_RETURN" +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJVVV..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS ret on ret.IDISSUED_ACC = A.ID and ret.IDACTION = 2 " +
                " where A.IDSTATUS = 2 and A.BaseId =2 and A.IDREADER = " + reader.ID + ") select * from hist order by DATE_ISSUE desc";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetAllBooks()
        {
            DA.SelectCommand.CommandText =
                                "select 1 ID, C.PLAIN collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
                " INV.SORT  collate cyrillic_general_ci_ai inv, 'Центр Американской Культуры' fund, TEMAP.PLAIN tema, POLKAP.PLAIN polka " +
                " from BJACC..MAIN A" +
                " left join BJACC..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on DD.ID = (select top 1 Z.ID from BJACC..DATAEXT Z where A.ID = Z.IDMAIN and Z.MNFIELD = 700 and Z.MSFIELD = '$a')" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join BJACC..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
                " left join BJACC..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
                " left join BJACC..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
                " left join BJACC..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
                " left join BJACC..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
                " where INV.SORT is not null "+//and klass.SORT='Длявыдачи'" +
                
                " union all " +

                "select 1 ID,C.PLAIN  collate cyrillic_general_ci_ai tit,D.PLAIN  collate cyrillic_general_ci_ai avt," +
                " INV.SORT  collate cyrillic_general_ci_ai inv, 'Основной фонд' fund , TEMAP.PLAIN tema, POLKAP.PLAIN polka " +
                " from BJVVV..MAIN A " +
                " left join BJVVV..DATAEXT CC on A.ID = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJVVV..DATAEXT DD on DD.ID = (select top 1 Z.ID from BJVVV..DATAEXT Z where A.ID = Z.IDMAIN and Z.MNFIELD = 700 and Z.MSFIELD = '$a')" +
                " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJVVV..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJVVV..DATAEXT INV on A.ID = INV.IDMAIN and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " left join BJVVV..DATAEXT klass on INV.IDDATA = klass.IDDATA and klass.MNFIELD = 921 and klass.MSFIELD = '$c' " +
                " left join BJVVV..DATAEXT polka on INV.IDDATA = polka.IDDATA and polka.MNFIELD = 899 and polka.MSFIELD = '$c' " +
                " left join BJVVV..DATAEXTPLAIN POLKAP on POLKAP.IDDATAEXT = polka.ID" +
                " left join BJVVV..DATAEXT TEMA on A.ID = TEMA.IDMAIN and TEMA.MNFIELD = 922 and TEMA.MSFIELD = '$e'" +
                " left join BJVVV..DATAEXTPLAIN TEMAP on TEMAP.IDDATAEXT = TEMA.ID" +
                " left join BJVVV..DATAEXT FF on INV.IDDATA = FF.IDDATA and FF.MNFIELD = 899 and FF.MSFIELD = '$a'" +
                " where INV.SORT is not null  and FF.IDINLIST = 52 ";//and klass.SORT='Длявыдачи'";
            //спросить какой класс издания для них считается нормальным

            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }

        public object GetBookNegotiability()
        {
            DA.SelectCommand.CommandText = "with F1 as  " +
                                           " ( " +
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ISSUED_ACC B on B.ID = A.IDISSUED_ACC " +
                                           " where A.IDACTION = 2 and B.BaseId = 1 " +
                                           " group by B.IDDATA " +
                                           " ), acc as ( " +
                                           " select distinct 1 ID,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt, " +
                                           " INV.SORT collate Cyrillic_general_ci_ai inv,A.cnt, 'ЦАК' fund" +
                                           "  from F1 A " +
                                           " left join BJACC..DATAEXT idm on A.IDDATA = idm.IDDATA " +
                                           " left join BJACC..DATAEXT CC on idm.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a' " +
                                           "  left join BJACC..DATAEXT DD on idm.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a' " +
                                           " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID " +
                                           "  left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID " +
                                           "  left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                                           "), " +
                                           " F2 as  " +
                                           " ( " +
                                           " select B.IDDATA,COUNT(B.IDDATA) cnt " +
                                           " from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ISSUED_ACC B on B.ID = A.IDISSUED_ACC " +
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
                                           " select * from acc " +
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
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ЦАК' fund " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS AA on A.ID = AA.IDISSUED_ACC " +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join BJACC..DATAEXT CC on A.IDMAIN = CC.IDMAIN and CC.MNFIELD = 200 and CC.MSFIELD = '$a'" +
                " left join BJACC..DATAEXT DD on A.IDMAIN = DD.IDMAIN and DD.MNFIELD = 700 and DD.MSFIELD = '$a'" +
                " left join BJACC..DATAEXTPLAIN C on C.IDDATAEXT = CC.ID" +
                " left join BJACC..DATAEXTPLAIN D on D.IDDATAEXT = DD.ID" +
                " left join BJACC..DATAEXT INV on A.IDDATA = INV.IDDATA and INV.MNFIELD = 899 and INV.MSFIELD = '$w'" +
                " where AA.IDACTION = 5 and A.BaseId = 1" +

                " union all " +

                " select 1,C.PLAIN collate Cyrillic_general_ci_ai tit,D.PLAIN collate Cyrillic_general_ci_ai avt,A.IDREADER,B.FamilyName,B.[Name],B.FatherName," +
                " INV.SORT collate Cyrillic_general_ci_ai inv,A.DATE_ISSUE,AA.DATEACTION,'ОФ' fund " +
                " from Reservation_R..ISSUED_ACC A" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS AA on A.ID = AA.IDISSUED_ACC " +
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
                " from Reservation_R..ISSUED_ACC A" +
                " left join Readers..Main B on A.IDREADER = B.NumberReader" +
                " left join Reservation_R..ISSUED_ACC_ACTIONS EM on EM.IDISSUED_ACC = A.IDREADER and EM.IDACTION = 4" + // 4 - это ACTIONTYPE = сотрудник отослал емаил
                " where A.IDSTATUS = 1 and A.DATE_RETURN < getdate() and "+
                " (EM.DATEACTION = (select max(DATEACTION) from Reservation_R..ISSUED_ACC_ACTIONS where IDISSUED_ACC = A.IDREADER and IDACTION = 4) " +
                " or EM.DATEACTION is null) )" +
                " select * from vio";
                //" select * from vio where emailsent = (select max)";
            DS = new DataSet();
            DA.Fill(DS, "t");
            return DS.Tables["t"];
        }
    }
}
