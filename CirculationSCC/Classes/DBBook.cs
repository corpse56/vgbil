using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Circulation
{
    class DBBook : DB
    {
        //BJSCCRecord rec;// = new BJSCCRecord();
        public DBBook()
        {
            
        }
        public List<BJRecord> GetBookByIDMAIN(int IDMAIN, Bases fund)
        {
            if (fund == Bases.BJSCC)
                DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJSCC..DATAEXT A "+
                                           " left join BJSCC..DATAEXTPLAIN B on A.ID = B.IDDATAEXT where A.IDMAIN = "+IDMAIN;
            else
                DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJSCC..DATAEXT A " +
                                           " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT where A.IDMAIN = " + IDMAIN;
            DS = new DataSet();
            DA.Fill(DS, "t");
            List<BJRecord> Book = new List<BJRecord>();
            BJRecord rec;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                rec = new BJRecord();
                rec.ID = (int)r["ID"];
                rec.IDDATA = (int)r["IDDATA"];
                rec.IDINLIST = (int)r["IDINLIST"];
                rec.IDMAIN = IDMAIN;
                rec.MNFIELD = (int)r["MNFIELD"];
                rec.MSFIELD = r["MSFIELD"].ToString();
                rec.PLAIN = r["PLAIN"].ToString();
                rec.SORT = r["SORT"].ToString();
                rec.Fund = fund;
                Book.Add(rec);
            }
            return Book;
        }

        public List<BJRecord> GetBookByBAR(string BAR)
        {
            DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJSCC..DATAEXT A " +
                                           " left join BJSCC..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                           " where A.IDMAIN = (select top 1 IDMAIN from BJSCC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + BAR + "')";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            Bases fund = Bases.BJSCC;
            if (i == 0)
            {
                DA.SelectCommand.CommandText = "select A.*,B.PLAIN from BJVVV..DATAEXT A " +
                                               " left join BJVVV..DATAEXTPLAIN B on A.ID = B.IDDATAEXT " +
                                               " where A.IDMAIN = (select top 1 IDMAIN from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + BAR + "')";
                fund = Bases.BJVVV;
            }
            DS = new DataSet();
            i = DA.Fill(DS, "t");

            List<BJRecord> Book = new List<BJRecord>();
            BJRecord rec;
            foreach (DataRow r in DS.Tables["t"].Rows)
            {
                rec = new BJRecord();
                rec.ID = (int)r["ID"];
                rec.IDDATA = (int)r["IDDATA"];
                rec.IDINLIST = (int)r["IDINLIST"];
                rec.IDMAIN = (int)r["IDMAIN"]; 
                rec.MNFIELD = (int)r["MNFIELD"];
                rec.MSFIELD = r["MSFIELD"].ToString();
                rec.PLAIN = r["PLAIN"].ToString();
                rec.SORT = r["SORT"].ToString();
                rec.Fund = fund;
                Book.Add(rec);
            }
            return Book;
        }
        //public bool Exists(string BAR)
        //{
        //    DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJSCC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + BAR + "'";
        //    DS = new DataSet();
        //    int i = DA.Fill(DS, "t");
        //    if (i > 0) return true; else return false;

        //}

        internal bool IsIssued(int IDDATA, Bases fund)
        {
            if (fund == Bases.BJSCC)
                DA.SelectCommand.CommandText = "select IDMAIN from Reservation_R..ISSUED_SCC where IDDATA = " + IDDATA + " and IDSTATUS in (1,6) and BaseId = 1";
            else
                DA.SelectCommand.CommandText = "select IDMAIN from Reservation_R..ISSUED_SCC where IDDATA = " + IDDATA + " and IDSTATUS in (1,6) and BaseId = 2";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return true; else return false;
        }

        internal int GetIDISSUED(int IDDATA, Bases fund)
        {
            if (fund == Bases.BJSCC)
                DA.SelectCommand.CommandText = "select ID from Reservation_R..ISSUED_SCC where IDDATA = " + IDDATA + " and IDSTATUS in (1,6) and  BaseId = 1";
            else
                DA.SelectCommand.CommandText = "select ID from Reservation_R..ISSUED_SCC where IDDATA = " + IDDATA + " and IDSTATUS in (1,6) and  BaseId = 2";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return (int)DS.Tables["t"].Rows[0]["ID"]; else return 0;
        }
    }
}
