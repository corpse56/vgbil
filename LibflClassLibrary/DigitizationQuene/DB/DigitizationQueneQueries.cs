using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibflClassLibrary.DigitizationQuene.DB
{
    class DigitizationQueneQueries
    {
        internal string GET_QUENE_ITEM_BY_ID
        {
            get
            {
                return  " ";
            }
        }
        internal string GET_QUENE
        {
            get
            {
                return "select * from Circulation..DigitizationQuene A" +
                       " left join BookAddInf..ScanInfo CC on A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase" +
                       " where (A.DELETED is null or DELETED = 0) " +
                       "and not exists (select IDBook,IDBase " +
                       "                from BookAddInf..ScanInfo CC " +
                       "                where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase)";
            }
        }
        internal string GET_DELETED
        {
            get
            {
                return " select * from Circulation..DigitizationQuene A " +
                       " where A.DELETED = 1 ";
            }
        }
        internal string GET_COMPLETED
        {
            get
            {
                return " select * from Circulation..DigitizationQuene A " +
                       " left join BookAddInf..ScanInfo CCC on A.IDMAIN = CCC.IDBook and A.BAZA = CCC.IDBase" +
                       " where exists (select IDBook,IDBase from BookAddInf..ScanInfo CC where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase))";
            }
        }
        internal string ADD_TO_QUENE
        {
            get
            {
                return
                    "insert into Circulation..DigitizationQuene " +
                    "(IDMAIN,CREATED,IDREADER,ISREMOTEUSER,BAZA) values " +
                    "(@idMain,getdate(),@readerId,@isRemoteReader,@idBase)";
            }
        }
        internal string IS_ALREADY_IN_QUENE
        {
            get
            {
                return "select * from Circulation..DigitizationQuene where IDMAIN = @idMain and BAZA = @idBase and(DELETED = 0 or DELETED is null)";
            }
        }
        internal string IS_MORE_THAN_TWO_BOOKS_READER_WANTS_TO_DIGITIZE
        {
            get
            {
                return "select * from Circulation..DigitizationQuene where IDREADER = @readerId and cast(cast(CREATED as varchar(11)) as datetime) = cast(cast(getdate() as varchar(11)) as datetime)";
            }
        }
        internal string QUENE_OVERFLOW
        {
            get
            {
                return "select * from Circulation..DigitizationQuene A where (DELETED = 0 or DELETED is null)  and not exists (select IDBook,IDBase from BookAddInf..ScanInfo CC where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase)";
            }
        }



    }
}
