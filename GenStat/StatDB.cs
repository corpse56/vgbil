using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using System.IO;

namespace GenStat
{
    
    public class StatDB
    {
        public DateTime Start;
        public DateTime End;
        SqlConnection con;
        SqlDataAdapter da;
        public StatDB(DateTime s, DateTime e)
        {
            this.Start = s;
            this.End = e;
            con = new SqlConnection(new XmlConnections().GetBJVVVCon());
            con.Open();
            da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection = this.con;
            da.SelectCommand.CommandTimeout = 1200;
            da.SelectCommand.Parameters.Add("start", System.Data.SqlDbType.DateTime);
            da.SelectCommand.Parameters.Add("end", System.Data.SqlDbType.DateTime);
            da.SelectCommand.Parameters["start"].Value = this.Start.Date;
            da.SelectCommand.Parameters["end"].Value = this.End.Date;
        }


        internal int GetReadersRegCount()//количество зарегистрировавшихся читателей
        {
            da.SelectCommand.CommandText = "select NumberReader from Readers..Main where Cast(Cast(DateRegistration As VarChar(11)) As DateTime) between @start and @end";
            DataSet DS = new DataSet();
            return da.Fill(DS);
        }

        internal int GetReadersReRegCount()//количество перерегистрировавшихся читателей
        {
            da.SelectCommand.CommandText = "select NumberReader from Readers..Main where Cast(Cast(DateReRegistration As VarChar(11)) As DateTime) between DATEADD (yyyy , 5 , @start ) and DATEADD (yyyy , 5 , @end ) and [ReRegistration] = 1";
            DataSet DS = new DataSet();
            return da.Fill(DS);
        }

        internal int GetVisitorsCount()//количество посетителей библиотеки
        {
            //da.SelectCommand.CommandText = "select * from Readers..HistoryInput where Cast(Cast(DateInputHistoryInput As VarChar(11)) As DateTime) between @start and @end";
            //DataSet DS = new DataSet();
            //int tmp = da.Fill(DS);
            //da.SelectCommand.CommandText = "with A as ( " +
                                //"select IDREADER,DATE_ISSUE  " +
                                //"from Reservation_R..ISSUED   " +
                                //"where DATE_ISSUE between @start and @end " +
                                //"group by DATE_ISSUE,IDREADER " +
                                //"), " +
                                //"B as ( " +
                                //"select IDREADER,DATE_FACT_VOZV  " +
                                //"from Reservation_R..ISSUED   " +
                                //"where DATE_FACT_VOZV between @start and @end   " +
                                //"group by DATE_FACT_VOZV,IDREADER " +
                                //"), " +
                                //"C as ( " +
                                //"select IDREADER,DATE_PROLONG  " +
                                //"from Reservation_R..ISSUED   " +
                                //"where DATE_PROLONG between @start and @end   " +
                                //"group by DATE_PROLONG,IDREADER " +
                                //") " +
                                //"select * from A " +
                                //"union " +
                                //"select * from B " +
                                //"union " +
                                //"select * from C";
            //DS = new DataSet();
            //tmp += da.Fill(DS);
//            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAttendance(@start, @end,40)";
  //          DataSet DS = new DataSet();
    //        int tmp = da.Fill(DS);
            //da.SelectCommand.CommandText = "select IDREADER from Reservation_R..[Statistics] " +
             //                              " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between @start and @end " +
                  //                         " and ACTIONTYPE = 3 and IDREADER is not null and DEPID = 40 " +
                  //                         " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
            //DS = new DataSet();
            //tmp += da.Fill(DS);
            //tmp += this.GetCenterEntranceFKCCount();
            //tmp += this.GetCenterEntranceOnePassCount();
            //tmp += this.GetCenterEntranceGuestCount();
            da.SelectCommand.CommandText = "SELECT sum(CardStatisticInput)+ sum(SCStatisticInput)+sum(WithoutCardStatisticInput)+sum(FrnStatisticInput)+sum(GuestStatisticInput) +sum(VisitStatisticInput) from Readers..StatisticInput where DataStatisticInput between @start and @end";
            DataSet DS = new DataSet();
            int tmp = da.Fill(DS);
            return (int)DS.Tables[0].Rows[0][0];
        }


        internal int GetIssuedBooksCount()//колво выданной литературы
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.GetAllIssuedBooksCountAllCount(@start,@end)";
            //DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();

        }


        internal int GetReaderRecievedBookCount()//количество читателей, получивших литературу
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.GetAllReadersRecievedBooksCount(@start, @end)";
            //DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();

        }


        internal int GetAttendance()//количество посетителей зала без услуг книговыдачи
        {
            //da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAttendanceALL(@start, @end)";
            da.SelectCommand.CommandText = "select sss from Reservation_R.dbo.GetAttendanceAllByDep(@start,@end) " +
            "where dep = 'Итого' ";
            DataSet DS = new DataSet();
            int tmp = da.Fill(DS,"t");

            return (int)DS.Tables["t"].Rows[0][0];

        }

        internal int GetFreeServiceCount()//количество выданных бесплатных справок
        {
            da.SelectCommand.CommandText = "select (case when sum(AMOUNT) is null then 0 else sum(AMOUNT) end) from Reservation_R..[FREESERVICE] " +
                                           " where Cast(Cast(DATESERVICE As VarChar(11)) As DateTime) between @start and @end and IDDEP in (5,16,21,22,29,48,30,31,35,40,55,54)";
                                           
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetCenterEntranceCount()//центральный вход количество читателей
        {
            da.SelectCommand.CommandText = "select * from Readers..HistoryInput where Cast(Cast(DateInputHistoryInput As VarChar(11)) As DateTime) between @start and @end";
            DataSet DS = new DataSet();
            return da.Fill(DS);
        }

        internal int GetAbonementCount()//количество читателей абонемента
        {
            da.SelectCommand.CommandText = "with A as ( " +
                                "select IDREADER,DATE_ISSUE  " +
                                "from Reservation_R..ISSUED   " +
                                "where DATE_ISSUE between @start and @end " +
                                "group by DATE_ISSUE,IDREADER " +
                                "), " +
                                "B as ( " +
                                "select IDREADER,DATE_FACT_VOZV  " +
                                "from Reservation_R..ISSUED   " +
                                "where DATE_FACT_VOZV between @start and @end   " +
                                "group by DATE_FACT_VOZV,IDREADER " +
                                "), " +
                                "C as ( " +
                                "select IDREADER,DATE_PROLONG  " +
                                "from Reservation_R..ISSUED   " +
                                "where DATE_PROLONG between @start and @end   " +
                                "group by DATE_PROLONG,IDREADER " +
                                ") " +
                                "select * from A " +
                                "union " +
                                "select * from B " +
                                "union " +
                                "select * from C";
            DataSet DS = new DataSet();
            return da.Fill(DS);
        }

        internal int GetCMBCount()
        {
            da.SelectCommand.CommandText = "select IDREADER from Reservation_R..[Statistics] " +
                               " where  Cast(Cast(DATEACTION As VarChar(11)) As DateTime) between @start and @end " +
                               " and ACTIONTYPE = 3 and IDREADER is not null and DEPID = 40 " +
                               " group by Cast(Cast(DATEACTION As VarChar(11)) As DateTime),IDREADER ";
            DataSet DS = new DataSet();
            int tmp = da.Fill(DS);
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAttendance(@start, @end,40)";
            DS = new DataSet();
            tmp += da.Fill(DS);

            return tmp;

        }


        internal DataTable GetAllIssuedBook()
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAllIssuedBooksByDEP(@start, @end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }

        internal DataTable GetAllReadersRecievedBooks()
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAllReadersRecievedBooksByDEP(@start, @end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];

        }

        internal DataTable GetAttendanceAllByDep()
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAttendanceAllByDep(@start, @end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }

        internal DataTable GetFreeServiceListByDep()
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetFreeServiceListByDep(@start, @end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }

        internal int GetRecievedAndMadeShortTitle()
        {
            da.SelectCommand.CommandText = "SELECT count(*)  "+
                    "FROM BJVVV..DATAEXT  AS DE   "+
                    "LEFT JOIN BJVVV..DATAEXTPLAIN AS P  ON DE.ID = P.IDDATAEXT  "+
                    "WHERE   cast(cast(DE.Created as varchar(11)) as DateTime) between @start and @end " +
                    //"WHERE   DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@start)<=0   " +
                    //"AND DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@end)>=0   " +
                    "AND DE.MNFIELD=922 AND DE.MSFIELD='$c'   "+
                    "AND LEN(PLAIN)>3 and (len(PLAIN)-charindex('.',PLAIN))<3   "+
                    "AND CHARINDEX(' ',PLAIN)=0   "+
                    "AND NOT EXISTS (SELECT ID FROM BJVVV..DATAEXT WHERE  MNFIELD=482 AND IDDATA=DE.IDDATA) "+
                    "AND DE.Creator in (SELECT U.ID from BJVVV..USERS AS U where DEPT=26 OR DEPT=36)";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int InvAssignedBJVVV()
        {
            //old query
            /*da.SelectCommand.CommandText = "with main as ( "+
                "SELECT DE.IDMAIN,DE.IDDATA,P.PLAIN,DE.Creator "+
                "FROM BJVVV..DATAEXT  AS DE   "+
                "LEFT JOIN BJVVV..DATAEXTPLAIN AS P  ON DE.ID = P.IDDATAEXT  "+
                "WHERE   DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@start)<=0  " +
                "AND DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@end)>=0   " +
                "AND DE.MNFIELD=922 AND DE.MSFIELD='$c'   "+
                "AND LEN(PLAIN)>3 and (len(PLAIN)-charindex('.',PLAIN))<3   "+
                "AND CHARINDEX(' ',PLAIN)=0   "+
                "AND NOT EXISTS (SELECT ID FROM BJVVV..DATAEXT WHERE  MNFIELD=482 AND IDDATA=DE.IDDATA)  "+
                "AND DE.Creator in (SELECT U.ID from BJVVV..USERS AS U where DEPT=26 OR DEPT=36) "+
                ") "+
                "SELECT count(T.IDDATA) FROM main T "+
                " WHERE EXISTS (SELECT ID FROM BJVVV..DATAEXT  "+
                " WHERE MNFIELD=899 AND MSFIELD='$p' AND IDDATA=T.IDDATA)  "+
                " AND EXISTS (SELECT INVENTNO FROM BJVVV..INV_NO "+
                "   WHERE IDDATA=T.IDDATA)";*/
            da.SelectCommand.CommandText = "SELECT count(DE.IDDATA) " +
                            "FROM BJVVV..DATAEXT  AS DE    " +
                            "LEFT JOIN BJVVV..DATAEXTPLAIN AS P  ON DE.ID = P.IDDATAEXT   " +
                            "WHERE   cast(cast(DE.Created as varchar(11)) as DateTime) between @start and @end " +
                            "AND DE.MNFIELD=922 AND DE.MSFIELD='$c'    " +
                            "AND LEN(PLAIN)>3 and (len(PLAIN)-charindex('.',PLAIN))<3    " +
                            "AND CHARINDEX(' ',PLAIN)=0    " +
                            "AND NOT EXISTS (SELECT ID FROM BJVVV..DATAEXT WHERE  MNFIELD=482 AND IDDATA=DE.IDDATA)   " +
                            "AND DE.Creator in (SELECT U.ID from BJVVV..USERS AS U where DEPT=26 OR DEPT=36)  " +
                            "and EXISTS (SELECT ID FROM BJVVV..DATAEXT WHERE MNFIELD=899 AND MSFIELD='$p' AND IDDATA=DE.IDDATA)  " +
                            " AND EXISTS (SELECT INVENTNO FROM BJVVV..INV_NO WHERE IDDATA=DE.IDDATA)";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int InvAssignedREDKOSTJ()
        {
            da.SelectCommand.CommandText = "with main as ( " +
                "SELECT DE.IDMAIN,DE.IDDATA,P.PLAIN,DE.Creator " +
                "FROM REDKOSTJ..DATAEXT  AS DE   " +
                "LEFT JOIN REDKOSTJ..DATAEXTPLAIN AS P  ON DE.ID = P.IDDATAEXT  " +
                "WHERE   DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@start)<=0  " +
                "AND DATEDIFF(dy,cast(cast(DE.Created as varchar(11)) as DateTime),@end)>=0   " +
                "AND DE.MNFIELD=922 AND DE.MSFIELD='$c'   " +
                "AND LEN(PLAIN)>3 and (len(PLAIN)-charindex('.',PLAIN))<3   " +
                "AND CHARINDEX(' ',PLAIN)=0   " +
                "AND NOT EXISTS (SELECT ID FROM REDKOSTJ..DATAEXT WHERE  MNFIELD=482 AND IDDATA=DE.IDDATA)  " +
                "AND DE.Creator in (SELECT U.ID from REDKOSTJ..USERS AS U where DEPT=26 OR DEPT=36) " +
                ") " +
                "SELECT count(T.IDDATA) FROM main T " +
                " WHERE EXISTS (SELECT ID FROM REDKOSTJ..DATAEXT  " +
                " WHERE MNFIELD=899 AND MSFIELD='$p' AND IDDATA=T.IDDATA)  " +
                " AND EXISTS (SELECT INVENTNO FROM REDKOSTJ..INV_NO " +
                "   WHERE IDDATA=T.IDDATA)";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int ProccessedAndTransferedToMainFund()
        {
            da.SelectCommand.CommandText = "SELECT count(DISTINCT B.ID ) " +
                 "FROM BJVVV..INOUT  AS I " +
                 "LEFT JOIN BJVVV..INOUT_BASE AS B  ON B.IDINOUT=I.ID " +
                 "WHERE " +
                 "DEPTSOURCE=43 AND DEPTARGET in (11,47,14,15,12) " +
                 "and cast(cast(DateSent as varchar(11)) as DateTime) between @start and @end";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int ProccessedAndTransferedToAbonement()
        {
            da.SelectCommand.CommandText = "SELECT count(DISTINCT B.ID ) " +
                 "FROM BJVVV..INOUT  AS I " +
                 "LEFT JOIN BJVVV..INOUT_BASE AS B  ON B.IDINOUT=I.ID " +
                 "WHERE " +
                 "DEPTSOURCE=CAST(12 AS VARCHAR) AND DEPTARGET= CAST(47 AS VARCHAR) " +
                 "and cast(cast(DateSent as varchar(11)) as DateTime)>=@start " +
                 "and cast(cast(DateSent as varchar(11)) as DateTime)<=@end";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }
        internal int ProccessedAndTransferedToMainFundPeriodic()
        {
            da.SelectCommand.CommandText = "SELECT count(DISTINCT B.ID ) " +
                 "FROM BJVVV..INOUT  AS I " +
                 "LEFT JOIN BJVVV..INOUT_BASE AS B  ON B.IDINOUT=I.ID " +
                 "WHERE " +
                 "DEPTSOURCE=31 AND DEPTARGET in (11,47,14,15,12) " +
                 "and cast(cast(DateSent as varchar(11)) as DateTime)>=@start " +
                 "and cast(cast(DateSent as varchar(11)) as DateTime)<=@end";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }
        internal int IntroducedToREDKOSTJ()
        {
            da.SelectCommand.CommandText = "with INV(ot,sort) as ( " +
                 "SELECT distinct L.NAME,DE.SORT " +
                 "FROM REDKOSTJ..DATAEXT DE  " +
                 "left join REDKOSTJ..USERS u on u.ID=DE.Creator  " +
                 "left join REDKOSTJ..LIST_8 L on L.ID=u.DEPT  " +
                 "WHERE DATEDIFF(dy,cast(cast(Created as varchar(11)) as DateTime),@start)<=0  " +
                 "AND DATEDIFF(dy,cast(cast(Created as varchar(11)) as DateTime),@end)>=0  " +
                 "AND MNFIELD=899 AND MSFIELD='$p' ) " +
                 "select COUNT(*) AS Column2 FROM INV  " +
                 "where ot = 'ЦОД. Сектор учета' ";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();

        }

        internal int GetCenterEntranceFKCCount()
        {
            da.SelectCommand.CommandText = "select count(*) from Readers..Input "+
                "where cast(cast(DateInInput as varchar(11)) as datetime) "+
                "between @start and @end "+
                "and TapeInput = 4";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetCenterEntranceOnePassCount()
        {
            da.SelectCommand.CommandText = "select count(*) from Readers..Input " +
                "where cast(cast(DateInInput as varchar(11)) as datetime) " +
                "between @start and @end " +
                "and TapeInput = 5";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }
        internal int GetCenterEntranceGuestCount()
        {
            da.SelectCommand.CommandText = "select count(*) from Readers..Input " +
                "where cast(cast(DateInInput as varchar(11)) as datetime) " +
                "between @start and @end " +
                "and TapeInput = 6";
            DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetFreeServicesByUsers(string dep)
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetFreeServicesByUsers(@start, @end,'"+dep+"')";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }

        internal object GetAttendanceByVarcharDEP(string dep)
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetAttendanceByVarcharDEP(@start, @end,'" + dep + "')";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }

        internal object GetReadersByVarcharDEP(string dep)
        {
            da.SelectCommand.CommandText = "SELECT * FROM Reservation_R.dbo.GetReadersByVarcharDEP(@start, @end,'" + dep + "')";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }



        internal object GetAverageTimeOfService()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.GETAVERAGESERVICETIME(@start,@end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullCount()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNT]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetIssuedBooksCountPlusRefusual()
        {
            da.SelectCommand.CommandText = "SELECT  Reservation_R.dbo.GetAllIssuedBooksCountAllCount(@start,@end) " +
                " + Reservation_R.dbo.GETREFUSUALSCOUNT(@start,@end)";
            //DataSet DS = new DataSet();
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetAVGTimeByFloor()
        {
            da.SelectCommand.CommandText = "SELECT id,flr,avgtime,c_ord,case when c_h_ord is null then 0 else c_h_ord end c_h_ord FROM Reservation_R.dbo.GETAVGTIMEBYFLOOR(@start, @end)";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0];
        }
       
        internal int GetIssuedBooksUnder14Count()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.GetAllIssuedBooksUnder14(@start,@end) is null then 0 "+
                                            " else Reservation_R.dbo.GetAllIssuedBooksUnder14(@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetIssuedBooksFrom14To24Count()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.GetAllIssuedBooks14between24(@start,@end) is null then 0 "+
                                            "else Reservation_R.dbo.GetAllIssuedBooks14between24(@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetIssuedBooksIndAbCount()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.GetAllIssuedBooksIndAb(@start,@end) is null then 0"+
                                            " else Reservation_R.dbo.GetAllIssuedBooksIndAb(@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetIssuedBooksPersAbCount()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.GetAllIssuedBooksPersAb(@start,@end) is null then 0 " +
                                            "else Reservation_R.dbo.GetAllIssuedBooksPersAb(@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetIssuedBooksElIss()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.[GetAllIssuedBooksELISS](@start,@end) is null then 0"+
                                            " else Reservation_R.dbo.[GetAllIssuedBooksELISS](@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }
        internal object GetIssuedBooksElIssWITH()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.[GetAllIssuedBooksELISSWITH](@start,@end) is null then 0"+
                                            " else Reservation_R.dbo.[GetAllIssuedBooksELISSWITH](@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetIssuedBooksElIssWITHOUT()
        {
            da.SelectCommand.CommandText = "select case when Reservation_R.dbo.[GetAllIssuedBooksELISSWITHOUT](@start,@end) is null then 0"+
                                            " else Reservation_R.dbo.[GetAllIssuedBooksELISSWITHOUT](@start,@end) end";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int ConvertedIntoDigitalForm()
        {
            string ip = "192.168.4.30";
            string _directoryPath = @"\\" + ip + @"\BookAddInf";
            DirectoryInfo[] DIArrBJVVV;
            DirectoryInfo[] DIArrBJVVV_1;
            DirectoryInfo[] DIArrBJVVV_2;
            DirectoryInfo[] DIArrREDK;
            DirectoryInfo[] DIArrREDK_1;
            DirectoryInfo[] DIArrREDK_2;
            int bjvvv_res = 0;
            int redk_res = 0;
            using (new NetworkConnection(_directoryPath, new NetworkCredential(@"BJSTOR01\imgview", "Image_123Viewer")))
            {
                DirectoryInfo diBJVVV = new DirectoryInfo(_directoryPath+@"\BJVVV");
                DirectoryInfo diREDK = new DirectoryInfo(_directoryPath + @"\REDKOSTJ");
                //DIArr = di.GetDirectories(_directoryPath);
                DIArrBJVVV = diBJVVV.GetDirectories();
                foreach (DirectoryInfo di in DIArrBJVVV)
                {
                    DIArrBJVVV_1 = di.GetDirectories();
                    foreach (DirectoryInfo di1 in DIArrBJVVV_1)
                    {
                        DIArrBJVVV_2 = di1.GetDirectories();
                        foreach (DirectoryInfo di2 in DIArrBJVVV_2)
                        {
                            if ((di2.CreationTime >= this.Start) && (di2.CreationTime <= this.End))
                            {
                                bjvvv_res += DIArrBJVVV_2.Length;
                            }
                        }
                    }
                }
                DIArrREDK = diREDK.GetDirectories();
                foreach (DirectoryInfo di in DIArrREDK)
                {
                    DIArrREDK_1 = di.GetDirectories();
                    foreach (DirectoryInfo di1 in DIArrREDK_1)
                    {
                        DIArrREDK_2 = di1.GetDirectories();
                        foreach (DirectoryInfo di2 in DIArrREDK_2)
                        {
                            if ((di2.CreationTime >= this.Start) && (di2.CreationTime <= this.End))
                            {
                                redk_res += 1;
                            }
                        }
                    }
                }

            }
            return bjvvv_res+redk_res;
        }


        internal int GetReadersRegCountSimple()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountSimple](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetReadersRegCountBS()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountBS](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetReadersRegCountEmpl()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountEmpl](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetReadersRegCountInd()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountInd](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetReadersRegCountPers()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountPers](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal int GetReadersRegCountColl()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.[GetReadersRegCountColl](@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetIssuedBooksCollAbCount()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.GetIssuedBooksCollAbCount(@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetIssuedBooksBSAbCount()
        {
            da.SelectCommand.CommandText = "select Reservation_R.dbo.GetIssuedBooksBSAbCount(@start,@end)";
            return (int)da.SelectCommand.ExecuteScalar();
        }

        internal object GetReadersRegFullSimple()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTSIMPLE]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullBS()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTBS]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullEmpl()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTEmpl]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullInd()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTInd]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullPers()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTPers]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFullColl()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTColl]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegUnder14()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTU14]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

        internal object GetReadersRegFrom14To24()
        {
            da.SelectCommand.CommandText = "SELECT Reservation_R.dbo.[GETREGISTEREDREADERSCOUNTF14T24]()";
            DataSet DS = new DataSet();
            da.Fill(DS);
            return DS.Tables[0].Rows[0][0];
        }

    }
    public class NetworkConnection : IDisposable
    {
        #region Variables

        /// <summary>
        /// The full path of the directory.
        /// </summary>
        private readonly string _networkName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkConnection"/> class.
        /// </summary>
        /// <param name="networkName">
        /// The full path of the network share.
        /// </param>
        /// <param name="credentials">
        /// The credentials to use when connecting to the network share.
        /// </param>
        public NetworkConnection(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName.TrimEnd('\\')
            };

            var result = WNetAddConnection2(
                netResource, credentials.Password, credentials.UserName, 0);

            if (result != 0)
            {
                throw new Win32Exception(result);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when this instance has been disposed.
        /// </summary>
        public event EventHandler<EventArgs> Disposed;

        #endregion

        #region Public methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var handler = Disposed;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

            WNetCancelConnection2(_networkName, 0, true);
        }

        #endregion

        #region Private static methods

        /// <summary>
        ///The WNetAddConnection2 function makes a connection to a network resource. The function can redirect a local device to the network resource.
        /// </summary>
        /// <param name="netResource">A <see cref="NetResource"/> structure that specifies details of the proposed connection, such as information about the network resource, the local device, and the network resource provider.</param>
        /// <param name="password">The password to use when connecting to the network resource.</param>
        /// <param name="username">The username to use when connecting to the network resource.</param>
        /// <param name="flags">The flags. See http://msdn.microsoft.com/en-us/library/aa385413%28VS.85%29.aspx for more information.</param>
        /// <returns></returns>
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
                                                     string password,
                                                     string username,
                                                     int flags);

        /// <summary>
        /// The WNetCancelConnection2 function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.
        /// </summary>
        /// <param name="name">Specifies the name of either the redirected local device or the remote network resource to disconnect from.</param>
        /// <param name="flags">Connection type. The following values are defined:
        /// 0: The system does not update information about the connection. If the connection was marked as persistent in the registry, the system continues to restore the connection at the next logon. If the connection was not marked as persistent, the function ignores the setting of the CONNECT_UPDATE_PROFILE flag.
        /// CONNECT_UPDATE_PROFILE: The system updates the user profile with the information that the connection is no longer a persistent one. The system will not restore this connection during subsequent logon operations. (Disconnecting resources using remote names has no effect on persistent connections.)
        /// </param>
        /// <param name="force">Specifies whether the disconnection should occur if there are open files or jobs on the connection. If this parameter is FALSE, the function fails if there are open files or jobs.</param>
        /// <returns></returns>
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="NetworkConnection"/> class.
        /// Allows an <see cref="System.Object"></see> to attempt to free resources and perform other cleanup operations before the <see cref="System.Object"></see> is reclaimed by garbage collection.
        /// </summary>
        ~NetworkConnection()
        {
            Dispose(false);
        }
    }

    #region Objects needed for the Win32 functions
#pragma warning disable 1591

    /// <summary>
    /// The net resource.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    /// <summary>
    /// The resource scope.
    /// </summary>
    public enum ResourceScope
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    } ;

    /// <summary>
    /// The resource type.
    /// </summary>
    public enum ResourceType
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    /// <summary>
    /// The resource displaytype.
    /// </summary>
    public enum ResourceDisplaytype
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
#pragma warning restore 1591
    #endregion

}
