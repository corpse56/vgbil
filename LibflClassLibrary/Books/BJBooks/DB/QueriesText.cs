using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.Books.BJBooks.DB
{
    public class Bibliojet
    {
        private string Fund;
        public string AFTable;
        public Bibliojet(string fund)
        {
            this.Fund = fund;
        }


        public string SELECT_RECORD_QUERY {
            get
            {
                return "select A.*,cast(A.MNFIELD as nvarchar(10))+A.MSFIELD code,A.SORT,B.PLAIN, C.IDLEVEL level_id, " +
                    " case when C.IDLEVEL = 1 then 'Монография'  " +
                    "  when C.IDLEVEL = -100 then 'Коллекция'  " +
                    "  when C.IDLEVEL = -2 then 'Сводный уровень многотомного издания'  " +
                    "  when C.IDLEVEL = 2 then 'Том (выпуск) многотомного издания'  " +
                    "  when C.IDLEVEL = -3 then 'Сводный уровень сериального издания'  " +
                    "  when C.IDLEVEL = -33 then 'Сводный уровень подсерии, входящей в серию'  " +
                    "  when C.IDLEVEL = 3 then 'Выпуск сериального издания'  " +
                    "  when C.IDLEVEL = -4 then 'Сводный уровень сборника'  " +
                    "  when C.IDLEVEL = 4 then 'Часть сборника'  " +
                    "  when C.IDLEVEL = -5 then 'Сводный уровень продолжающегося издания'  " +
                    "  when C.IDLEVEL = 5 then 'Выпуск продолжающегося издания' else '' end level " +
                    "  ,A.MNFIELD, A.MSFIELD , F.NAME RusFieldName, F.IDBLOCK, A.IDDATA" +
                    " from " + this.Fund + "..DATAEXT A" +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                    " left join " + this.Fund + "..MAIN C on A.IDMAIN = C.ID " +
                    " left join " + this.Fund + "..FIELDS F on A.MNFIELD = F.MNFIELD and A.MSFIELD = F.MSFIELD " +
                    " where A.IDMAIN = @idmain " +
                    //" and not exists (select 1 from " + this.Fund + "..DATAEXT EXTR where EXTR.IDMAIN = @idmain and EXTR.MNFIELD = 899 and EXTR.MSFIELD = '$x' and lower(EXTR.SORT) = 'э') " +
                    //" and not exists (select 1 from " + this.Fund + "..DATAEXT FIN where FIN.IDMAIN = @idmain and FIN.MNFIELD = 338 and FIN.MSFIELD = '$b') " +
                    " order by A.IDMAIN, A.IDDATA";
            }
        }

        public string IMPORT_CLARIFY_10a
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A " +
                           " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                           " where A.MNFIELD = 10 and A.MSFIELD = '$b' and A.IDDATA = @iddata";
            }
        }
        public string IMPORT_CLARIFY_101a
        {
            get
            {
                return " select NAME from " + this.Fund + "..LIST_1 where ID = @IDINLIST"; 
            }
        }
        public string IMPORT_CLARIFY_517a
        {
            get
            {
                return " select B.PLAIN from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where MNFIELD = 517 and MSFIELD = '$b' and A.IDDATA = @iddata";
            }
        }

        public string IMPORT_CLARIFY_205a_1
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A " +
                                " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                " where A.MNFIELD = 205 and A.MSFIELD = '$b' and A.IDDATA = @iddata";
            }
        }

        public string IMPORT_CLARIFY_205a_2
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A " +
                        " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 205 and A.MSFIELD = '$f' and A.IDDATA = @iddata";
            }
        }

        public string IMPORT_CLARIFY_205a_3
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A " +
                        " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.MNFIELD = 205 and A.MSFIELD = '$g' and A.IDDATA = @iddata";
            }
        }

        public string IMPORT_CLARIFY_606a
        {
            get
            {
                return "select * " +
                                " from " + this.Fund + "..TPR_CHAIN A " +
                                " left join " + this.Fund + "..TPR_TES B on A.IDTES = B.ID " +
                                " where A.IDCHAIN = @idchain" +
                                " order by IDORDER";
            }
        }

        public string GET_IDDATA_OF_ALL_EXEMPLARS
        {
            get
            {
                return " select distinct A.IDMAIN, A.IDDATA from " + this.Fund + "..DATAEXT A" +
                            " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                            " where A.IDMAIN = @idmain and (A.MNFIELD = 899 and A.MSFIELD = '$p' or A.MNFIELD = 899 and A.MSFIELD = '$a' or A.MNFIELD = 899 and A.MSFIELD = '$w') " +
                            " and not exists (select 1 from "  +this.Fund + "..DATAEXT C where C.IDDATA = A.IDDATA and C.MNFIELD = 921 and C.MSFIELD = '$c' and C.SORT = 'Списано')";
            }
        }

        public string GET_EXEMPLAR
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A" +
                        " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                        " left join " + this.Fund + "..LIST_8 C on A.MNFIELD = 899 and A.MSFIELD = '$a' and A.IDINLIST = C.ID " +
                        " where A.IDDATA = @iddata";
            }
        }

        public string GET_EXEMPLAR_ID_BY_INVENTORY_NUMBER
        {
            get
            {
                return " select 'BJVVV' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJVVV..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv " +
                        " and not exists (select 1 from BJVVV..DATAEXT C " +
                        "                 where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')" +
                        " union all" +
                        " select 'REDKOSTJ' fund, A.IDDATA exemplarId, A.IDMAIN bookId from REDKOSTJ..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv " +
                        " and not exists (select 1 from REDKOSTJ..DATAEXT C " +
                        "                 where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')" +
                        " union all" +
                        " select 'BJACC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJACC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv " +
                        " and not exists (select 1 from BJACC..DATAEXT C " +
                        "                 where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')" +
                        " union all" +
                        " select 'BJFCC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJFCC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv " +
                        " and not exists (select 1 from BJFCC..DATAEXT C " +
                        "                 where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')" +
                        " union all" +
                        " select 'BJSCC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJSCC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv " +
                        " and not exists (select 1 from BJSCC..DATAEXT C " +
                        "                 where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')" ;
            }
        }
        public string GET_EXEMPLAR_ID_BY_BAR
        {
            get
            {
                return " select 'BJVVV' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJVVV..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = @bar " +
                        " union all" +
                        " select 'BJVVV' fund, A.IDDATA exemplarId, A.IDMAIN bookId from REDKOSTJ..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = @bar " +
                        " union all" +
                        " select 'BJACC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJACC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = @bar " +
                        " union all" +
                        " select 'BJFCC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJFCC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = @bar " +
                        " union all" +
                        " select 'BJSCC' fund, A.IDDATA exemplarId, A.IDMAIN bookId from BJSCC..DATAEXT A" +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$w' and A.SORT = @bar ";
            }
        }

        public string GET_CONVOLUTE_BY_INVENTORY_NUMBER
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A" +
                        //" left join " + this.Fund + "..DATAEXT B on A.IDDATA = B.IDDATA " +
                        //" left join " + this.Fund + "..DATAEXTPLAIN C on C.IDDATAEXT = A.ID " +
                        " where A.MNFIELD = 899 and A.MSFIELD = '$p' and A.SORT = @inv" +
                        " and not exists (select 1 from " + this.Fund + "..DATAEXT C where A.IDDATA = C.IDDATA and MNFIELD = 482 and MSFIELD = '$a')";
            }
        }

        public string GET_HYPERLINK
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A" +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.MNFIELD = 940 and A.MSFIELD = '$a' and A.IDMAIN = @idmain";
            }
        }
        public string GET_BOOK_SCAN_INFO
        {
            get
            {
                return " select * from BookAddInf..ScanInfo A" +
                    " where A.IDBase = @idbase and A.IDBook = @idmain";
            }
        }
        public string GET_ALL_IDMAIN_WITH_IMAGES
        {
            get
            {
                return "select IDMAIN from " + this.Fund + "..IMAGES";
            }
        }

        public string GET_IMAGE
        {
            get
            {
                return "select IDMAIN, PIC from " + this.Fund + "..IMAGES where IDMAIN = @idmain";
            }
        }

        public string GET_PARENT_PIN
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT " +
                           " where MNFIELD = 225 and MSFIELD = '$a' and IDMAIN = @ParentPIN";
            }
        }

        public string GET_MAX_IDMAIN
        {
            get
            {
                return "select max(ID) from " + this.Fund + "..MAIN";
            }
        }

        public string GET_TITLE
        {
            get
            {
                return " select * from " + this.Fund + "..DATAEXT A " +
                    " left join " + this.Fund + "..DATAEXTPLAIN B on B.IDDATAEXT = A.ID " +
                    " where A.IDMAIN = @idmain and MNFIELD = 200 and MSFIELD = '$a' ";
            }
        }
        public string GET_AF_ALL_VALUES  
        {
            get
            {
                return " select PLAIN from " + this.Fund + ".." + this.AFTable + " A " +
                               " where IDAF = @AFLinkId";
            }
        }
        public string GET_RTF
        {
            get
            {
                return " select RTF from " + this.Fund + "..RTF A " +
                               " where IDMAIN = @idmain";
            }
        }

        public string IS_ALLIGAT//запрос не дописан
        {
            get
            {
                return " select * from "+this.Fund+"..DATAEXT A " +
                        " where exists (select 1 from "+this.Fund+"..DATAEXT B on)"+
                        " and A.IDDATA = @iddata and A.MNFIELD = 899 and A.MSFIELD = '$p' "+
                        ""+
                        ""+
                               " where IDDATA = @iddata";
            }
        }
        public string IS_ISSUED_OR_ORDERED_EMPLOYEE
        {
            get
            {
                return " select * from Reservation_E..Orders A " +
                               " where IDDATA = @iddata";
            }
        }
        public string IS_SELF_ISSUED_OR_ORDERED_EMPLOYEE
        {
            get
            {
                return " select * from Reservation_E..Orders A " +
                               " where ID_Book_EC = @idmain and ID_Reader = @idreader and IDDATA = @iddata";
            }
        }


        public string GET_EMPLOYEE_STATUS
        {
            get
            {
                return  " select B.Name from Reservation_E..Orders A " +
                        " left join Reservation_E..Status B on A.Status = B.ID" +
                        " where ID_Book_EC = @idmain";
            }
        }

        public string GET_LAST_INCREMENT_DATE
        {
            get
            {
                return "select LastIncrement from EXPORTNEB..VufindIncrementUpdate where lower(BaseName) = lower(@base)";
            }
        }
        public string SET_LAST_INCREMENT_DATE
        {
            get
            {
                return "update EXPORTNEB..VufindIncrementUpdate set LastIncrement = @LastIncrement where lower(BaseName) = lower(@base)";
            }
        }
        public string GET_BUSY_ELECTRONIC_EXEMPLAR_COUNT//вся редкая книга без авторского права, но в будущем надо всё равно для всех фондов сделать BASE
        {
            get
            {
                return "select ID from Reservation_R..ELISSUED where BASE = "+((this.Fund == "BJVVV")? "1" : "2")+" and IDMAIN = @IDMAIN";
            }
        }
        public string GET_NEAREST_FREE_DATE_FOR_ELECTRONIC_ISSUE//вся редкая книга без авторского права, но в будущем надо всё равно для всех фондов сделать BASE
        {
            get
            {
                return "select min(DATERETURN) from Reservation_R..ELISSUED where BASE = " + ((this.Fund == "BJVVV") ? "1" : "2") + " and IDMAIN = @IDMAIN";
            }
        }
        public string IS_ONE_DAY_PAST_AFTER_RETURN
        {
            get
            {
                return "select top 1 * from Reservation_R..ELISSUED_HST where IDREADER = @IDREADER and IDMAIN = @IDMAIN and BASE = " + ((this.Fund == "BJVVV") ? "1" : "2") + " order by ID desc";
            }
        }
        public string IS_ELECTRONIC_COPY_ISSUED_TO_READER
        {
            get
            {
                return "select * from Reservation_R..ELISSUED where IDREADER = @IDReader and IDMAIN = @IDMAIN and BASE = " + ((this.Fund == "BJVVV") ? "1" : "2");
            }
        }
        public string GET_ELECTRONIC_VIEWKEY_FOR_READER
        {
            get
            {
                return "select * from Reservation_R..ELISSUED where IDREADER = @IDReader and IDMAIN = @IDMAIN and BASE = " + ((this.Fund == "BJVVV") ? "1" : "2") ;
            }
        }

        public string IS_ELECTRONIC_COPY_ISSUED
        {
            get
            {
                return "select * from Reservation_R..ELISSUED where IDMAIN = @IDMAIN and BASE = " + ((this.Fund == "BJVVV") ? "1" : "2");
            }
        }
        public string ISSUE_ELECTRONIC_COPY_TO_READER
        {
            get
            {
                
                return "insert into Reservation_R..ELISSUED (IDMAIN,IDREADER,DATEISSUE,DATERETURN,   VIEWKEY,FORMDATE,             BASE,                              R_TYPE) values " +
                                                         " (@IDMAIN,@IDREADER,getdate(),@DateReturn,@ViewKey,getdate(), " + ((this.Fund == "BJVVV") ? "1" : "2") + " ,@ReaderType)";
            }
        }

        public string GET_INCREMENT_UPDATE_QUERY
        {
            get
            {
                return "  select distinct IDMAIN from (" +
                        " select ID IDMAIN from "+ this.Fund + "..MAIN where DateChange >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" + 
                        " union all" +
                        " select IDMAIN from " + this.Fund + "..DATAEXT " +
                        "  where Changed >=  (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        //"     or Created >=  (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        " union all" +
                        " select IDMAIN from " + this.Fund + "..IMAGES where DateCreate >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" + 
                        " union all" +
                        "  select A.IDMAIN IDMAIN" +
                        "  from [BJVVV].[dbo].[TPR_TES] T " +
                        "  left join " + this.Fund + "..DATAEXT A on A.MNFIELD = 606 and A.MSFIELD = '$a' " +
                        "        and cast(T.IDCHAIN as varchar) = A.SORT "+
                        " where DateChanged >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        //"   or  DateCreate >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        " and A.IDMAIN is not null" +
                        " union all " +
                        " select IDBook IDMAIN from BookAddInf..ScanInfo" +
                        " where DateChang >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        " and IDBase = " + ((this.Fund == "BJVVV") ? "1" : "2") +
                        //" where DateChang >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')" +
                        //" where IDBase = " + ((this.Fund == "BJVVV") ? "1" : "2") +
                        ") tableAlias";
            }
        }
        public string GET_INCREMENT_DELETED_QUERY
        {
            get
            {
                return " select distinct IDMAIN from " + this.Fund + "..PROTOCOL " +
                       " where WHAT = 'Удалена запись' and Date >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')";
            }
        }
        public string GET_INCREMENT_COVERS_QUERY
        {
            get
            {
                return "select IDMAIN from " + this.Fund + "..IMAGES where DateCreate >= (select LastIncrement from EXPORTNEB..VufindIncrementUpdate where BaseName = '" + this.Fund + "')";
            }
        }

        public string GET_ELECTRONIC_EXEMPLAR_ACCESS_LEVEL
        {
            get
            {
                return "select * from BookAddInf..BookProject where IDBook = @IDMAIN and IDProject = @IDProject and IDBase = " + ((this.Fund == "BJVVV") ? "1" : "2");
            }
        }

        public string GET_ELECTRONIC_EXEMPLAR_AVAILABILITY_STATUSES
        {
            get
            {
                return "select * from BookAddInf..BookProject where IDBook = @IDMAIN and IDBase = " + ((this.Fund == "BJVVV") ? "1" : "2");
            }
        }

        public string IS_EXISTS_DIGITAL_COPY
        {
            get
            {
                return "select 1 from BookAddInf..ScanInfo where IDBook = @IDMAIN and IDBase = " + ((this.Fund == "BJVVV") ? "1" : "2");
                //return "select 1 from " + this.Fund + "..DATAEXT where MNFIELD = 940 and MSFIELD = '$a' and IDMAIN = @IDMAIN";
            }
        }

        public string GET_BJVVV_USER_BY_LOGIN
        {
            get
            {
                return  " select A.ID, A.LOGIN, A.HASH, B.IDROLE, B.IDDEPT DepId, C.ROLE, D.NAME DepName, A.NAME FIO " +
                        " from " + this.Fund + "..USERS A " +
                        " left join " + this.Fund + "..USERSTATUS B on A.ID = B.IDUSER " +
                        " left join " + this.Fund + "..USERSROLE C on B.IDROLE = C.ID " +
                        " left join " + this.Fund + "..LIST_8 D on B.IDDEPT = D.ID" +
                        " where A.LOGIN = @login ";
            }
        }

        public string GET_CIPHER
        {
            get
            {
                return "select cipher.PLAIN " +
                    " from " + this.Fund + "..DATAEXT A " +
                    " left join " + this.Fund + "..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 899 and B.MSFIELD = '$j' " +
                    " left join " + this.Fund + "..DATAEXTPLAIN cipher on cipher.IDDATAEXT = B.ID " +
                    " where A.IDMAIN = @idmain and A.MNFIELD = 899 and A.MSFIELD = '$b' and A.SORT = @fnd and cipher.PLAIN is not null";
            }
        }

        public string GET_BOOK_BY_BAR
        {
            get
            {
                return " select IDMAIN, 'BJVVV' fund from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = @bar " +
                        " union all " +
                        " select IDMAIN, 'REDKOSTJ' fund from REDKOSTJ..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = @bar " +
                        " union all " +
                        " select IDMAIN,'BJACC' fund from BJACC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = @bar " +
                        " union all " +
                        " select IDMAIN,'BJFCC' fund from BJFCC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = @bar " +
                        " union all " +
                        " select IDMAIN,'BJSCC' fund from BJSCC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = @bar ";
            }
        }

        public string GET_BJVVV_USER_BY_ID
        {
            get
            {
                return " select A.LOGIN from BJVVV..USERS A where ID = @id";
            }
        }

        public string GET_AUTHOR
        {
            get
            {
                return  " select B.PLAIN from " + this.Fund + "..DATAEXT A" +
                        " left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        " where A.IDMAIN = @IDMAIN and A.MNFIELD = 700 and A.MSFIELD = '$a' ";
            }
        }

        public string GET_FIELD_VALUE
        {
            get
            {
                return " select "+
                        "substring( "+
                        "    (select '; ' + B.PLAIN from " + this.Fund + "..DATAEXT A " +
                        "     left join " + this.Fund + "..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                        "    where A.IDMAIN = @idmain and MNFIELD = @mnfield and MSFIELD = @msfield " +
                        "    for XML path('')) " +
                        ",2,1000)";
            }
        }

    }


}
