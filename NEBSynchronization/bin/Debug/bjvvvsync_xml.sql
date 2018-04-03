with 
pinsToGo as
(
select IDBook from BookAddInf..ScanInfo where PDF = 1 and IDBase = 1 and ForAllReader = 1
--select IDBook from BookAddInf..ScanInfo where IDBook in (1364958,1365201,1056155)

),
-------------------------------------------------------Коллекция---899b------Collection---------------------
prexmlFCollection899b as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 899 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from pinsToGo)
),
FCollection899b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) [Collection]
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFCollection899b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFCollection899b A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------Примечание о содержании---327a------ContentRemark---------------------
prexmlFContentRemark327a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 327 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FContentRemark327a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) ContentRemark
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFContentRemark327a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFContentRemark327a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Заглавие сводного уровня---225a------Series---------------------
prexmlFSeries225a as(
select A.IDMAIN,C.PLAIN 
from BJVVV..DATAEXT A
left join BJVVV..DATAEXT B on B.IDMAIN = A.SORT and A.MNFIELD = 225 and A.MSFIELD = '$a'
left join BJVVV..DATAEXTPLAIN C on B.ID = C.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$a' and B.MNFIELD = 200 and B.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FSeries225a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Series
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFSeries225a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFSeries225a A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Заглавие тома---200i------VolumeName---------------------
prexmlFVolumeName200i as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 200 and A.MSFIELD = '$i' and A.IDMAIN in (select IDBook from pinsToGo)
),
FVolumeName200i as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) VolumeName
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFVolumeName200i A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFVolumeName200i A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Номер тома---225h------VolumeNumber---------------------
prexmlFVolumeNumber225h as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$h' and A.IDMAIN in (select IDBook from pinsToGo)
),
FVolumeNumber225h as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) VolumeNumber
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFVolumeNumber225h A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFVolumeNumber225h A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Тематика---606a------UDKText---------------------
prexmlFUDKText606a as(
select A.IDMAIN,B.UDC PLAIN
from BJVVV..DATAEXT A
join BJVVV..TPR_UDC B on A.SORT = B.IDCHAIN
where A.MNFIELD = 606 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FUDKText606a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) UDKText
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFUDKText606a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFUDKText606a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Формат произведения---921b------Format---------------------
prexmlFFormat921b as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 921 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from pinsToGo)
),
FFormat921b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Format
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFFormat921b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFFormat921b A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Объём(кол-во страниц)---215a------CountPages---------------------
prexmlFCountPages215a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 215 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FCountPages215a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) CountPages
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFCountPages215a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFCountPages215a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Место издания---210a------PublishPlace---------------------
prexmlFMesto210a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 210 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FMesto210a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) PublishPlace
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFMesto210a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFMesto210a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------доп. Сведения об издании---205b------PublicationInformation---------------------
prexmlFPubInfo205b as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
 join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from pinsToGo)
),
FPubInfo205b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) PublicationInformation
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFPubInfo205b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFPubInfo205b A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Автор 710а------Author---------------------
prexmlFAuthor710a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 710 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor710a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFAuthor710a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFAuthor710a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Автор 701а------Author---------------------
prexmlFAuthor701a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 701 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor701a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFAuthor701a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFAuthor701a A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Автор 700а------Author---------------------
prexmlFAuthor700a as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 700 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FAuthor700a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFAuthor700a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFAuthor700a A1 
	group by A1.IDMAIN
) A
),
------------------------------------------------------------------ИЗДАТЕЛЬСТВО---210c----Publisher---------------
prexmlFIZD as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 210 and A.MSFIELD = '$c' and A.IDMAIN in (select IDBook from pinsToGo)
),
FIZD as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Publisher
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFIZD A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFIZD A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------Language----101a------------------------------
prexmlFYAZ as(
select A.IDMAIN,B.PLAIN
from BJVVV..DATAEXT A
left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from pinsToGo)
),
FYAZ as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) yaz
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlFYAZ A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlFYAZ A1 
	group by A1.IDMAIN
) A
),
------------------------------------------------------------
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
prexmlRCollection899b as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 899 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RCollection899b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) [Collection]
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRCollection899b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRCollection899b A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------Примечание о содержании---327a------ContentRemark---------------------
prexmlRContentRemark327a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 327 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RContentRemark327a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) ContentRemark
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRContentRemark327a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRContentRemark327a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Заглавие сводного уровня---225a------Series---------------------
prexmlRSeries225a as(
select A.IDMAIN,C.PLAIN 
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXT B on B.IDMAIN = A.SORT and A.MNFIELD = 225 and A.MSFIELD = '$a'
left join REDKOSTJ..DATAEXTPLAIN C on B.ID = C.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$a' and B.MNFIELD = 200 and B.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RSeries225a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Series
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRSeries225a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRSeries225a A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Заглавие тома---200i------VolumeName---------------------
prexmlRVolumeName200i as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 200 and A.MSFIELD = '$i' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RVolumeName200i as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) VolumeName
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRVolumeName200i A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRVolumeName200i A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Номер тома---225h------VolumeNumber---------------------
prexmlRVolumeNumber225h as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 225 and A.MSFIELD = '$h' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RVolumeNumber225h as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) VolumeNumber
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRVolumeNumber225h A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRVolumeNumber225h A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Тематика---606a------UDKText---------------------
prexmlRUDKText606a as(
select A.IDMAIN,B.UDC PLAIN
from REDKOSTJ..DATAEXT A
join REDKOSTJ..TPR_UDC B on A.SORT = B.IDCHAIN
where A.MNFIELD = 606 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RUDKText606a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) UDKText
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRUDKText606a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRUDKText606a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Формат произведения---921b------Format---------------------
prexmlRFormat921b as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 921 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RFormat921b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Format
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRFormat921b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRFormat921b A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Объём(кол-во страниц)---215a------CountPages---------------------
prexmlRCountPages215a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 215 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RCountPages215a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) CountPages
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRCountPages215a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRCountPages215a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Место издания---210a------PublishPlace---------------------
prexmlRMesto210a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 210 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RMesto210a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) PublishPlace
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRMesto210a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRMesto210a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------доп. Сведения об издании---205b------PublicationInformation---------------------
prexmlRPubInfo205b as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RPubInfo205b as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) PublicationInformation
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRPubInfo205b A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRPubInfo205b A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Автор 710а------Author---------------------
prexmlRAuthor710a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 710 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RAuthor710a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRAuthor710a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRAuthor710a A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------------Автор 701а------Author---------------------
prexmlRAuthor701a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 701 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RAuthor701a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRAuthor701a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRAuthor701a A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------Автор 700а------Author---------------------
prexmlRAuthor700a as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 700 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RAuthor700a as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Author
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRAuthor700a A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRAuthor700a A1 
	group by A1.IDMAIN
) A
),
------------------------------------------------------------------ИЗДАТЕЛЬСТВО---210c----Publisher---------------
prexmlRIZD as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 210 and A.MSFIELD = '$c' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RIZD as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) Publisher
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRIZD A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRIZD A1 
	group by A1.IDMAIN
) A
),
-----------------------------------------------------------Language----101a------------------------------
prexmlRYAZ as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 101 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RYAZ as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) yaz
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRYAZ A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRYAZ A1 
	group by A1.IDMAIN
) A
),
------------------------------------------------------------------ОБЩЕЕ ПРИМЕЧАНИЕ-----300a--------------
--в редкости это поле повторяемое, а в основной базе нет
prexmlRCommonRemark as(
select A.IDMAIN,B.PLAIN
from REDKOSTJ..DATAEXT A
left join REDKOSTJ..DATAEXTPLAIN B on A.ID = B.IDDATAEXT
where A.MNFIELD = 300 and A.MSFIELD = '$a' and A.IDMAIN in (select IDBook from RpinsToGO)
),
RCommonRemark as
(
select A.IDMAIN,left(A.PLAIN,LEN(A.PLAIN) - 1) RCommonRemark
from 
(
	select  A1.IDMAIN IDMAIN,
						(select isnull(A2.PLAIN,'')+ '#' 
						from prexmlRCommonRemark A2 
						where A1.IDMAIN = A2.IDMAIN 
						for XML path('')
						) PLAIN

	from prexmlRCommonRemark A1 
	group by A1.IDMAIN
) A
),
-------------------------------------------------------------------------------------------------------------
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



