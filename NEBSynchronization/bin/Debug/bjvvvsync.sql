with 
pinsToGo as
(
select IDBook from BookAddInf..ScanInfo where PDF = 1 and IDBase = 1 and ForAllReader = 1
),
-------------------------------------------------------Коллекция---899b------Collection---------------------
FCollection899b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 899 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FCollection899b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FCollection899b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FCollection899b1 B
inner join FCollection899b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FCollection899b as
(
select IDMAIN,MAX(SORT) [Collection] from FCollection899b2 group by IDMAIN
),
-------------------------------------------------------Примечание о содержании---327a------ContentRemark---------------------
FContentRemark327a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 327 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FContentRemark327a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FContentRemark327a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FContentRemark327a1 B
inner join FContentRemark327a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FContentRemark327a as
(
select IDMAIN,MAX(SORT) ContentRemark from FContentRemark327a2 group by IDMAIN
),
-----------------------------------------------------------------Заглавие сводного уровня---225a------Series---------------------
FSeries225a1 as
(
select row_number() over(partition by A.IDMAIN order by A.SORT) rna,
A.IDMAIN IDMAIN,C.PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXT B on B.IDMAIN = A.SORT and A.MNFIELD = 225 and A.MSFIELD = '$a'
left join BJVVV..DATAEXTPLAIN C on B.ID = C.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$a' and B.MNFIELD = 200 and B.MSFIELD = '$a' and
 A.IDMAIN in (select IDBook from pinsToGo)
), 
FSeries225a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FSeries225a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FSeries225a1 B
inner join FSeries225a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FSeries225a as
(
select IDMAIN,MAX(SORT) Series from FSeries225a2 group by IDMAIN
),
-------------------------------------------------------------Заглавие тома---200i------VolumeName---------------------
FVolumeName200i1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 200 and MSFIELD = '$i'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FVolumeName200i2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FVolumeName200i1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FVolumeName200i1 B
inner join FVolumeName200i2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FVolumeName200i as
(
select IDMAIN,MAX(SORT) VolumeName from FVolumeName200i2 group by IDMAIN
),
-------------------------------------------------------------Номер тома---225h------VolumeNumber---------------------
FVolumeNumber225h1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 225 and MSFIELD = '$h'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FVolumeNumber225h2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FVolumeNumber225h1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FVolumeNumber225h1 B
inner join FVolumeNumber225h2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FVolumeNumber225h as
(
select IDMAIN,MAX(SORT) VolumeNumber from FVolumeNumber225h2 group by IDMAIN
),
-----------------------------------------------------------------Тематика---606a------UDKText---------------------
FUDKText606a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,UDC SORT 
from BJVVV..DATAEXT A 
left join BJVVV..TPR_UDC B on A.SORT = B.IDCHAIN
where MNFIELD = 606 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
and UDC is not null
),
FUDKText606a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FUDKText606a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FUDKText606a1 B
inner join FUDKText606a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FUDKText606a as
(
select IDMAIN,MAX(SORT) UDKText from FUDKText606a2 group by IDMAIN
),

-----------------------------------------------------------------Формат произведения---921b------Format---------------------
FFormat921b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 921 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FFormat921b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FFormat921b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FFormat921b1 B
inner join FFormat921b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FFormat921b as
(
select IDMAIN,MAX(SORT) Format from FFormat921b2 group by IDMAIN
),
-----------------------------------------------------------------Объём(кол-во страниц)---215a------CountPages---------------------
FCountPages215a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 215 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FCountPages215a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FCountPages215a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FCountPages215a1 B
inner join FCountPages215a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FCountPages215a as
(
select IDMAIN,MAX(SORT) CountPages from FCountPages215a2 group by IDMAIN
),
-----------------------------------------------------------------Место издания---210a------PublishPlace---------------------
FMesto210a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 210 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FMesto210a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FMesto210a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FMesto210a1 B
inner join FMesto210a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FMesto210a as
(
select IDMAIN,MAX(SORT) PublishPlace from FMesto210a2 group by IDMAIN
),

-----------------------------------------------------------------доп. Сведения об издании---205b------PublicationInformation---------------------
FPubInfo205b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 205 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FPubInfo205b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FPubInfo205b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FPubInfo205b1 B
inner join FPubInfo205b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FPubInfo205b as
(
select IDMAIN,MAX(SORT) PublicationInformation from FPubInfo205b2 group by IDMAIN
),
-----------------------------------------------------------------Автор 710а------Author---------------------
FAuthor710a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 710 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor710a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FAuthor710a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FAuthor710a1 B
inner join FAuthor710a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FAuthor710a as
(
select IDMAIN,MAX(SORT) Author from FAuthor710a2 group by IDMAIN
),
-----------------------------------------------------------------Автор 701а------Author---------------------
FAuthor701a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 701 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor701a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FAuthor701a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FAuthor701a1 B
inner join FAuthor701a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FAuthor701a as
(
select IDMAIN,MAX(SORT) Author from FAuthor701a2 group by IDMAIN
),
-------------------------------------------------------------Автор 700а------Author---------------------
FAuthor700a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 700 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor700a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FAuthor700a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FAuthor700a1 B
inner join FAuthor700a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FAuthor700a as
(
select IDMAIN,MAX(SORT) Author from FAuthor700a2 group by IDMAIN
),
------------------------------------------------------------------ИЗДАТЕЛЬСТВО----Publisher---------------
FIZD1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from BJVVV..DATAEXT A 
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 210 and MSFIELD = '$c'
and A.IDMAIN in (select IDBook from pinsToGo)
),
FIZD2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FIZD1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FIZD1 B
inner join FIZD2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FIZD as
(
select IDMAIN,MAX(SORT) Publisher from FIZD2 group by IDMAIN
),
FYAZ1 as
(
select row_number() over(partition by IDMAIN order by SORT) rna,
		IDMAIN IDMAIN,SORT SORT from BJVVV..DATAEXT 
where MNFIELD = 101 and MSFIELD = '$a'
and IDMAIN in (select IDBook from pinsToGo)
),
FYAZ2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from FYAZ1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from FYAZ1 B
inner join FYAZ2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
FYAZ as
(
select IDMAIN,MAX(SORT) yaz from FYAZ2 group by IDMAIN
),
F1 as (
select 

A.IDMAIN idm,
--BB.PLAIN avt,
CC.PLAIN zag,
EE.PLAIN god,
FF.PLAIN respon,
GG.PLAIN sved,
HH.PLAIN isbn,
II.PLAIN tiraz,
--JJ.PLAIN nomtom,
--KK.PLAIN [Collection],
MM.PLAIN CommonRemark,
NN.PLAIN Annotation,
case when L.ForAllReader = 0 then 1 else 0 end AccessType
 from BJVVV..DATAEXT A
--left join BJVVV..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a'
--LEFT join BJVVV..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT
left join  BJVVV..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C.MSFIELD = '$a'
LEFT join BJVVV..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT
left join  BJVVV..DATAEXT E on A.IDMAIN = E.IDMAIN and E.MNFIELD = 2100 and E.MSFIELD = '$d'
LEFT join BJVVV..DATAEXTPLAIN EE on E.ID = EE.IDDATAEXT
left join  BJVVV..DATAEXT F on A.IDMAIN = F.IDMAIN and F.MNFIELD = 200 and F.MSFIELD = '$f'
LEFT join BJVVV..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT
left join  BJVVV..DATAEXT G on A.IDMAIN = G.IDMAIN and G.MNFIELD = 205 and G.MSFIELD = '$a'
LEFT join BJVVV..DATAEXTPLAIN GG on G.ID = GG.IDDATAEXT
left join  BJVVV..DATAEXT H on A.IDMAIN = H.IDMAIN and H.MNFIELD = 10 and H.MSFIELD = '$a'
LEFT join BJVVV..DATAEXTPLAIN HH on H.ID = HH.IDDATAEXT
left join  BJVVV..DATAEXT I on A.IDMAIN = I.IDMAIN and I.MNFIELD = 11 and I.MSFIELD = '$9'
LEFT join BJVVV..DATAEXTPLAIN II on I.ID = II.IDDATAEXT
--left join  BJVVV..DATAEXT J on A.IDMAIN = J.IDMAIN and J.MNFIELD = 225 and J.MSFIELD = '$h'
--LEFT join BJVVV..DATAEXTPLAIN JJ on J.ID = JJ.IDDATAEXT
--left join  BJVVV..DATAEXT K on A.IDMAIN = K.IDMAIN and K.MNFIELD = 899 and K.MSFIELD = '$b'
--LEFT join BJVVV..DATAEXTPLAIN KK on K.ID = KK.IDDATAEXT
left join BookAddInf..ScanInfo L on A.IDMAIN = L.IDBook and L.IDBase = 1
left join  BJVVV..DATAEXT M on A.IDMAIN = M.IDMAIN and M.MNFIELD = 300 and M.MSFIELD = '$a'
LEFT join BJVVV..DATAEXTPLAIN MM on M.ID = MM.IDDATAEXT
left join  BJVVV..DATAEXT N on A.IDMAIN = N.IDMAIN and N.MNFIELD = 330 and N.MSFIELD = '$a'
LEFT join BJVVV..DATAEXTPLAIN NN on N.ID = NN.IDDATAEXT

--не повторяются: 200f,205a,2100d,10a,11$9,225h,899b,300а
where A.IDMAIN in 
(select IDBook from pinsToGo)
) ,
auth as
 (
 select case when Author.Author IS null then '' else Author.Author +'#' end + 
	 case when Author1.Author IS null then '' else Author1.Author +'#' end+
	case when Author2.Author IS null then '' else Author2.Author end avtr,idm
 from F1
 left join FAuthor700a Author on F1.idm = Author.IDMAIN
 left join FAuthor701a Author1 on F1.idm = Author1.IDMAIN
 left join FAuthor710a Author2 on F1.idm = Author2.IDMAIN
where F1.idm in (select IDBook from pinsToGo)
),
final as (
select 
distinct
 10 ALIS,
 'BJVVV'+cast(F1.idm as nvarchar) IdFromALIS,
 --case when Author.Author IS null then '' else Author.Author +'#' end + 
 --case when Author1.Author IS null then '' else Author1.Author +'#' end+
 --case when Author2.Author IS null then '' else Author2.Author end avt,
Case auth.avtr when null then null else (case LEN(auth.avtr) when 0 then auth.avtr else LEFT(auth.avtr, LEN(auth.avtr) - 1) end ) end Author,
 zag Title,
 respon Responsibility,
 case when sved is null then '' else sved+'#' end+
 case when PublicationInformation.PublicationInformation is null then '' else 
				PublicationInformation.PublicationInformation end PublicationInformation,
 god PublishYear,
 Publisher.Publisher ,
 PublishPlace.PublishPlace,
 isbn ISBN,
 yaz LanguageText,
 CountPages.CountPages CountPages,
 tiraz tiraz,
 Format.Format Format,
 UDCText.UDKText,
 FSeries225a.Series Series,
 FVolumeNumber225h.VolumeNumber VolumeNumber,
 VolumeName.VolumeName VolumeName,
 [Collection].[Collection] [Collection],
 AccessType AccessType,
 1 isLicenseAgreement,
 ContentRemark.ContentRemark ContentRemark,
 CommonRemark CommonRemark,
 Annotation,
 getdate() CreationDateTime,
 GETDATE() UpdatingDateTime,
 F1.idm 
 
 
 from F1
 left join FYAZ A on F1.idm = A.IDMAIN
 left join FIZD B on F1.idm = B.IDMAIN
 left join FAuthor700a Author on F1.idm = Author.IDMAIN
 left join FAuthor701a Author1 on F1.idm = Author1.IDMAIN
 left join FAuthor710a Author2 on F1.idm = Author2.IDMAIN
 left join FPubInfo205b PublicationInformation on F1.idm=PublicationInformation.IDMAIN
 left join FIZD Publisher on F1.idm=Publisher.IDMAIN
 left join FMesto210a PublishPlace on F1.idm=PublishPlace.IDMAIN
 left join FCountPages215a CountPages on F1.idm = CountPages.IDMAIN
 left join FFormat921b Format on F1.idm = Format.IDMAIN
 left join FUDKText606a UDCText on F1.idm = UDCText.IDMAIN
 left join FSeries225a Series on F1.idm = Series.IDMAIN
 left join FVolumeName200i VolumeName on F1.idm = VolumeName.IDMAIN
 left join FContentRemark327a ContentRemark on F1.idm = ContentRemark.IDMAIN
 left join FCollection899b [Collection] on F1.idm = [Collection].IDMAIN
 left join auth auth on F1.idm = auth.idm 
 left join FSeries225a FSeries225a on F1.idm = FSeries225a.IDMAIN
  left join FVolumeNumber225h FVolumeNumber225h on F1.idm = FVolumeNumber225h.IDMAIN

),
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--============================REDKOSTJ================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================
--====================================================================================

RpinsToGo as
(
select IDBook from BookAddInf..ScanInfo where PDF = 1 and IDBase = 2 and ForAllReader = 1
),
-------------------------------------------------------Коллекция---899b------Collection---------------------
RCollection899b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 899 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RCollection899b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RCollection899b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RCollection899b1 B
inner join RCollection899b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RCollection899b as
(
select IDMAIN,MAX(SORT) [Collection] from RCollection899b2 group by IDMAIN
),
-------------------------------------------------------Примечание о содержании---327a------ContentRemark---------------------
RContentRemark327a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 327 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RContentRemark327a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RContentRemark327a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RContentRemark327a1 B
inner join RContentRemark327a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RContentRemark327a as
(
select IDMAIN,MAX(SORT) ContentRemark from RContentRemark327a2 group by IDMAIN
),
-------------------------------------------------------------Номер тома---225h------VolumeNumber---------------------
RVolumeNumber225h1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 225 and MSFIELD = '$h'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RVolumeNumber225h2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RVolumeNumber225h1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RVolumeNumber225h1 B
inner join RVolumeNumber225h2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RVolumeNumber225h as
(
select IDMAIN,MAX(SORT) VolumeNumber from RVolumeNumber225h2 group by IDMAIN
),
-------------------------------------------------------------Заглавие тома---200i------VolumeName---------------------
RVolumeName200i1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 200 and MSFIELD = '$i'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RVolumeName200i2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RVolumeName200i1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RVolumeName200i1 B
inner join RVolumeName200i2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RVolumeName200i as
(
select IDMAIN,MAX(SORT) VolumeName from RVolumeName200i2 group by IDMAIN
),
 -----------------------------------------------------------------Заглавие сводного уровня---225a------Series---------------------
RSeries225a1 as
(
select row_number() over(partition by A.IDMAIN order by A.SORT) rna,
A.IDMAIN IDMAIN,C.PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXT B on B.IDMAIN = A.SORT and A.MNFIELD = 225 and A.MSFIELD = '$a'
left join REDKOSTJ..DATAEXTPLAIN C on B.ID = C.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$a' and B.MNFIELD = 200 and B.MSFIELD = '$a' and
 A.IDMAIN in (select IDBook from RpinsToGo)
), 
RSeries225a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RSeries225a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RSeries225a1 B
inner join RSeries225a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RSeries225a as
(
select IDMAIN,MAX(SORT) Series from RSeries225a2 group by IDMAIN
),
-----------------------------------------------------------------Тематика---606a------UDKText---------------------
RUDKText606a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,UDC SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..TPR_UDC B on A.SORT = B.IDCHAIN
where MNFIELD = 606 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
and UDC is not null
),
RUDKText606a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RUDKText606a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RUDKText606a1 B
inner join RUDKText606a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RUDKText606a as
(
select IDMAIN,MAX(SORT) UDKText from RUDKText606a2 group by IDMAIN
),

-----------------------------------------------------------------Формат произведения---921b------Format---------------------
RFormat921b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 921 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RFormat921b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RFormat921b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RFormat921b1 B
inner join RFormat921b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RFormat921b as
(
select IDMAIN,MAX(SORT) Format from RFormat921b2 group by IDMAIN
),
-----------------------------------------------------------------Объём(кол-во страниц)---215a------CountPages---------------------
RCountPages215a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 215 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RCountPages215a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RCountPages215a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RCountPages215a1 B
inner join RCountPages215a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RCountPages215a as
(
select IDMAIN,MAX(SORT) CountPages from RCountPages215a2 group by IDMAIN
),
-----------------------------------------------------------------Место издания---210a------PublishPlace---------------------
RMesto210a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 210 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RMesto210a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RMesto210a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RMesto210a1 B
inner join RMesto210a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RMesto210a as
(
select IDMAIN,MAX(SORT) PublishPlace from RMesto210a2 group by IDMAIN
),

-----------------------------------------------------------------доп. Сведения об издании---205b------PublicationInformation---------------------
RPubInfo205b1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 205 and MSFIELD = '$b'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RPubInfo205b2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RPubInfo205b1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RPubInfo205b1 B
inner join RPubInfo205b2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RPubInfo205b as
(
select IDMAIN,MAX(SORT) PublicationInformation from RPubInfo205b2 group by IDMAIN
),
-----------------------------------------------------------------Автор 710а------Author---------------------
RAuthor710a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 710 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RAuthor710a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RAuthor710a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RAuthor710a1 B
inner join RAuthor710a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RAuthor710a as
(
select IDMAIN,MAX(SORT) Author from RAuthor710a2 group by IDMAIN
),
-----------------------------------------------------------------Автор 701а------Author---------------------
RAuthor701a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 701 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RAuthor701a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RAuthor701a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RAuthor701a1 B
inner join RAuthor701a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RAuthor701a as
(
select IDMAIN,MAX(SORT) Author from RAuthor701a2 group by IDMAIN
),
-------------------------------------------------------------Автор 700а------Author---------------------
RAuthor700a1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 700 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RAuthor700a2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RAuthor700a1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RAuthor700a1 B
inner join RAuthor700a2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RAuthor700a as
(
select IDMAIN,MAX(SORT) Author from RAuthor700a2 group by IDMAIN
),
------------------------------------------------------------------ИЗДАТЕЛЬСТВО----Publisher---------------
RIZD1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
A.IDMAIN IDMAIN,PLAIN SORT 
from REDKOSTJ..DATAEXT A 
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where MNFIELD = 210 and MSFIELD = '$c'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RIZD2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RIZD1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RIZD1 B
inner join RIZD2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RIZD as
(
select IDMAIN,MAX(SORT) Publisher from RIZD2 group by IDMAIN
),
------------------------------------------------------------------ЯЗЫК-------------------

RYAZ1 as
(
select row_number() over(partition by IDMAIN order by SORT) rna,
		IDMAIN IDMAIN,SORT SORT from REDKOSTJ..DATAEXT 
where MNFIELD = 101 and MSFIELD = '$a'
and IDMAIN in (select IDBook from RpinsToGo)
),
RYAZ2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RYAZ1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RYAZ1 B
inner join RYAZ2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RYAZ as
(
select IDMAIN,MAX(SORT) yaz from RYAZ2 group by IDMAIN
),
------------------------------------------------------------------ОБЩЕЕ ПРИМЕЧАНИЕ-------------------
-- в редкости это поле повторяемое, а в основной базе нет
RCommonRemark1 as
(
select row_number() over(partition by A.IDMAIN order by SORT) rna,
		A.IDMAIN IDMAIN,PLAIN SORT from REDKOSTJ..DATAEXT A
		left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
 
where MNFIELD = 300 and MSFIELD = '$a'
and A.IDMAIN in (select IDBook from RpinsToGo)
),
RCommonRemark2  as
(
select distinct A.IDMAIN IDMAIN,cast(A.SORT as varchar(max)) SORT, 2 LEV
from RCommonRemark1 A where A.rna=1
union all
select  B.IDMAIN IDMAIN,cast(B.SORT + '#'+C.SORT as varchar(max)) SORT, LEV+1
from RCommonRemark1 B
inner join RCommonRemark2 C on B.IDMAIN = C.IDMAIN and B.rna = C.LEV --and B.SORT != C.SORT
),
RCommonRemark as 
(
select IDMAIN,MAX(SORT) RCommonRemark from RCommonRemark2 group by IDMAIN
),
R1 as (
select 

A.IDMAIN idm,
--BB.PLAIN avt,
CC.PLAIN zag,
EE.PLAIN god,
FF.PLAIN respon,
GG.PLAIN sved,
HH.PLAIN isbn,
II.PLAIN tiraz,
--JJ.PLAIN nomtom,
--KK.PLAIN [Collection],
--MM.PLAIN CommonRemark,
NN.PLAIN Annotation,
case when L.ForAllReader = 0 then 1 else 0 end AccessType
 from REDKOSTJ..DATAEXT A
--left join REDKOSTJ..DATAEXT B on A.IDMAIN = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a'
--LEFT join REDKOSTJ..DATAEXTPLAIN BB on B.ID = BB.IDDATAEXT
left join  REDKOSTJ..DATAEXT C on A.IDMAIN = C.IDMAIN and C.MNFIELD = 200 and C.MSFIELD = '$a'
LEFT join REDKOSTJ..DATAEXTPLAIN CC on C.ID = CC.IDDATAEXT
left join  REDKOSTJ..DATAEXT E on A.IDMAIN = E.IDMAIN and E.MNFIELD = 2100 and E.MSFIELD = '$d'
LEFT join REDKOSTJ..DATAEXTPLAIN EE on E.ID = EE.IDDATAEXT
left join  REDKOSTJ..DATAEXT F on A.IDMAIN = F.IDMAIN and F.MNFIELD = 200 and F.MSFIELD = '$f'
LEFT join REDKOSTJ..DATAEXTPLAIN FF on F.ID = FF.IDDATAEXT
left join  REDKOSTJ..DATAEXT G on A.IDMAIN = G.IDMAIN and G.MNFIELD = 205 and G.MSFIELD = '$a'
LEFT join REDKOSTJ..DATAEXTPLAIN GG on G.ID = GG.IDDATAEXT
left join  REDKOSTJ..DATAEXT H on A.IDMAIN = H.IDMAIN and H.MNFIELD = 10 and H.MSFIELD = '$a'
LEFT join REDKOSTJ..DATAEXTPLAIN HH on H.ID = HH.IDDATAEXT
left join  REDKOSTJ..DATAEXT I on A.IDMAIN = I.IDMAIN and I.MNFIELD = 11 and I.MSFIELD = '$9'
LEFT join REDKOSTJ..DATAEXTPLAIN II on I.ID = II.IDDATAEXT
--left join  REDKOSTJ..DATAEXT J on A.IDMAIN = J.IDMAIN and J.MNFIELD = 225 and J.MSFIELD = '$h'
--LEFT join REDKOSTJ..DATAEXTPLAIN JJ on J.ID = JJ.IDDATAEXT
--left join  REDKOSTJ..DATAEXT K on A.IDMAIN = K.IDMAIN and K.MNFIELD = 899 and K.MSFIELD = '$b'
--LEFT join REDKOSTJ..DATAEXTPLAIN KK on K.ID = KK.IDDATAEXT
left join BookAddInf..ScanInfo L on A.IDMAIN = L.IDBook and L.IDBase = 1
--left join  REDKOSTJ..DATAEXT M on A.IDMAIN = M.IDMAIN and M.MNFIELD = 300 and M.MSFIELD = '$a'
--LEFT join REDKOSTJ..DATAEXTPLAIN MM on M.ID = MM.IDDATAEXT
left join  REDKOSTJ..DATAEXT N on A.IDMAIN = N.IDMAIN and N.MNFIELD = 330 and N.MSFIELD = '$a'
LEFT join REDKOSTJ..DATAEXTPLAIN NN on N.ID = NN.IDDATAEXT

--не повторяются: 200f,205a,2100d,10a,11$9,225h,899b
where A.IDMAIN in 
(select IDBook from RpinsToGo)
) ,
Rauth as
 (
 select case when Author.Author IS null then '' else Author.Author +'#' end + 
	 case when Author1.Author IS null then '' else Author1.Author +'#' end+
	case when Author2.Author IS null then '' else Author2.Author end avtr,idm
 from R1
 left join RAuthor700a Author on R1.idm = Author.IDMAIN
 left join RAuthor701a Author1 on R1.idm = Author1.IDMAIN
 left join RAuthor710a Author2 on R1.idm = Author2.IDMAIN
where R1.idm in (select IDBook from RpinsToGo)
),
Rfinal as (
select 
distinct
 10 ALIS,
 'REDKOSTJ'+cast(R1.idm as nvarchar) IdFromALIS,
Case Rauth.avtr when null then null else (case LEN(Rauth.avtr) when 0 then Rauth.avtr else LEFT(Rauth.avtr, LEN(Rauth.avtr) - 1) end ) end Author,
 zag Title,
 respon Responsibility,
 case when sved is null then '' else sved+'#' end+
 case when PublicationInformation.PublicationInformation is null then '' else 
				PublicationInformation.PublicationInformation end PublicationInformation,
 god PublishYear,
 Publisher.Publisher ,
 PublishPlace.PublishPlace,
 isbn ISBN,
 yaz LanguageText,
 CountPages.CountPages CountPages,
 tiraz tiraz,
 Format.Format Format,
 UDCText.UDKText,
 Series.Series Series,
 RVolumeNumber225h.VolumeNumber VolumeNumber,
 VolumeName.VolumeName VolumeName,
 [Collection].[Collection] [Collection],
 AccessType AccessType,
 1 isLicenseAgreement,
 ContentRemark.ContentRemark ContentRemark,
 RCommonRemark.RCommonRemark  RCommonRemark,
 Annotation,
 getdate() CreationDateTime,
 GETDATE() UpdatingDateTime,
 R1.idm idm
 
 
 from R1
 left join RYAZ A on R1.idm = A.IDMAIN
 left join RIZD B on R1.idm = B.IDMAIN
 left join RAuthor700a Author on R1.idm = Author.IDMAIN
 left join RAuthor701a Author1 on R1.idm = Author1.IDMAIN
 left join RAuthor710a Author2 on R1.idm = Author2.IDMAIN
 left join RPubInfo205b PublicationInformation on R1.idm=PublicationInformation.IDMAIN
 left join RIZD Publisher on R1.idm=Publisher.IDMAIN
 left join RMesto210a PublishPlace on R1.idm=PublishPlace.IDMAIN
 left join RCountPages215a CountPages on R1.idm = CountPages.IDMAIN
 left join RFormat921b Format on R1.idm = Format.IDMAIN
 left join RUDKText606a UDCText on R1.idm = UDCText.IDMAIN
 left join RSeries225a Series on R1.idm = Series.IDMAIN
 left join RVolumeName200i VolumeName on R1.idm = VolumeName.IDMAIN
 left join RContentRemark327a ContentRemark on R1.idm = ContentRemark.IDMAIN
 left join RCollection899b [Collection] on R1.idm = [Collection].IDMAIN
 left join Rauth Rauth on R1.idm = Rauth.idm 
 left join RCommonRemark RCommonRemark on R1.idm = RCommonRemark.IDMAIN
 left join RVolumeNumber225h RVolumeNumber225h on R1.idm = RVolumeNumber225h.IDMAIN
),
prefinal as (


select  ALIS,IdFromALIS,Author,Title,Responsibility,PublicationInformation,PublishYear,
Publisher ,PublishPlace,ISBN,LanguageText,CountPages,tiraz,Format,UDKText,Series,
VolumeNumber,VolumeName,[Collection],AccessType,isLicenseAgreement,ContentRemark,CommonRemark COLLATE database_default CommonRemark,
Annotation,CreationDateTime,UpdatingDateTime,idm 
from final
union all
select  ALIS,IdFromALIS,Author,Title,Responsibility,PublicationInformation,PublishYear,
Publisher ,PublishPlace,ISBN,LanguageText,CountPages,tiraz,Format,UDKText,Series,
VolumeNumber,VolumeName,[Collection],AccessType,isLicenseAgreement,ContentRemark,RCommonRemark COLLATE database_default CommonRemark,
Annotation,CreationDateTime,UpdatingDateTime,idm 
from Rfinal
)
--test as
--(
select ROW_NUMBER() over(order by Author asc) num, * from prefinal
--)
  
  --select * from REDKOSTJ..DATAEXT where IDMAIN  = 32458
  
--select IDBook,IDBase from BookAddInf..ScanInfo where PDF = 1 and ForAllReader = 1
--except 
--select case when substring(IdFromAlis,1,1)='B' then substring(IdFromALIS,6,10000) else substring(IdFromALIS,9,10000) end IDBook ,
--case  when substring(IdFromAlis,1,1)='B' then 1 else 2 end IDBase
--from test


--17630	2
--32458	2
--,test as (
--select ROW_NUMBER() over(order by Author asc) num, * from prefinal
--)
--select IdFromAlis from test
--group by IdFromAlis
--having COUNT(IdFromAlis)>1



