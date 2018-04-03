using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Circulation
{
    class DBGeneral:DB
    {
//=====================================================================LOGIN==============================================================================
        public int EmpID;
        public string UserName;

        public bool Login(string name,string pass)
        {
            DA.SelectCommand.CommandText = "select * from BJSCC..USERS where lower(LOGIN) = '" +name.ToLower()+"' and lower(PASSWORD) = '"+pass.ToLower()+"'";
            DS = new DataSet();
            int i = DA.Fill(DS, "login");
            if (i == 0) return false;
            EmpID = (int)DS.Tables["login"].Rows[0]["ID"];
            UserName = DS.Tables["login"].Rows[0]["NAME"].ToString();
            return true;
        }
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^LOGIN^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        internal BARTYPE BookOrReader(string data)
        {
            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJSCC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + data + "'";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.BookSCC;

            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + data + "'";
            DS = new DataSet();
            i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.BookBJVVV;

            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where BarCode = '" + data.Substring(1)+"'";
            DS = new DataSet();
            try
            {
                i = DA.Fill(DS, "t");
            }
            catch
            {
                i = 0;
            }
            if (i > 0) return BARTYPE.Reader;
            if (data.IndexOf(" ") == -1) return BARTYPE.NotExist;
            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where NumberSC = '" + data.Substring(0,data.IndexOf(" "))+
                                                                                       "' and SerialSC = '" + data.Substring(data.IndexOf(" ")+1)+"'";
            DS = new DataSet();
            i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.Reader;

            return BARTYPE.NotExist;
            //R.Tables["t"].Rows[0]["NumberSC"].ToString().Trim().Replace("\0", "") + R.Tables["t"].Rows[0]["SerialSC"].ToString().Trim().Replace("\0", "");
        }


        /// <summary>
        /// Возвращаемые значения:
        /// 0 - успех
        /// 
        /// 
        /// </summary>
        /// <param name="ScannedBook"></param>
        /// <param name="ScannedReader"></param>
        /// <returns></returns>
        internal void ISSUE(BookVO ScannedBook, ReaderVO ScannedReader, int IDEMP) //выдача на дом
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = ScannedBook.IDMAIN;
            DA.InsertCommand.Parameters.Add("IDDATA", SqlDbType.Int).Value = ScannedBook.IDDATA;
            DA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int).Value = ScannedReader.ID;
            DA.InsertCommand.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime).Value = DateTime.Now;
            DA.InsertCommand.Parameters.Add("DATE_RETURN", SqlDbType.DateTime).Value = DateTime.Now.AddDays(21);
            DA.InsertCommand.Parameters.Add("IDSTATUS", SqlDbType.Int).Value =  1 ;//1 - выдано на дом, 6 - выдано в зал
            DA.InsertCommand.Parameters.Add("BaseId", SqlDbType.Int).Value = (ScannedBook.FUND == Bases.BJSCC) ? 1 : 2;
            DA.InsertCommand.Parameters.Add("IsAtHome", SqlDbType.Bit).Value = true;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC (IDMAIN,IDDATA,IDREADER,DATE_ISSUE,DATE_RETURN,IDSTATUS,BaseId) values " +
                                            " (@IDMAIN,@IDDATA,@IDREADER,@DATE_ISSUE,@DATE_RETURN,@IDSTATUS,@BaseId);select scope_identity();";
            DA.InsertCommand.Connection.Open();
            object scope_id = DA.InsertCommand.ExecuteScalar();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_SCC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value =  1;//1 - выдано на дом, 6 - выдано в зал
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_SCC"].Value = scope_id;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC_ACTIONS (IDACTION,IDEMP,IDISSUED_SCC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_SCC,@DATEACTION)";
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }
        internal void IssueInHall(BookVO ScannedBook, ReaderVO ScannedReader, int IDEMP)//выдача в зал
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = ScannedBook.IDMAIN;
            DA.InsertCommand.Parameters.Add("IDDATA", SqlDbType.Int).Value = ScannedBook.IDDATA;
            DA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int).Value = ScannedReader.ID;
            DA.InsertCommand.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime).Value = DateTime.Now;
            DA.InsertCommand.Parameters.Add("DATE_RETURN", SqlDbType.DateTime).Value = DateTime.Now;
            DA.InsertCommand.Parameters.Add("IDSTATUS", SqlDbType.Int).Value = 6;//1 - выдано на дом, 6 - выдано в зал
            DA.InsertCommand.Parameters.Add("BaseId", SqlDbType.Int).Value = (ScannedBook.FUND == Bases.BJSCC) ? 1 : 2;
            DA.InsertCommand.Parameters.Add("IsAtHome", SqlDbType.Bit).Value = false;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC (IDMAIN,IDDATA,IDREADER,DATE_ISSUE,DATE_RETURN,IDSTATUS,BaseId,IsAtHome) values " +
                                            " (@IDMAIN,@IDDATA,@IDREADER,@DATE_ISSUE,@DATE_RETURN,@IDSTATUS,@BaseId,@IsAtHome);select scope_identity();";
            DA.InsertCommand.Connection.Open();
            object scope_id = DA.InsertCommand.ExecuteScalar();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_SCC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 6;//1 - выдано из центра славянских культур, 6 - выдано из основного фонда
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_SCC"].Value = scope_id;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC_ACTIONS (IDACTION,IDEMP,IDISSUED_SCC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_SCC,@DATEACTION)";
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
            //return 0;
        }
        internal void Recieve(BookVO ScannedBook, ReaderVO ScannedReader, int IDEMP)
        {
            DA.UpdateCommand.Parameters.Clear();
            DA.UpdateCommand.Parameters.Add("IDISSUED", SqlDbType.Int);

            DA.UpdateCommand.Parameters["IDISSUED"].Value = ScannedBook.IDISSUED;
            DA.UpdateCommand.CommandText = "update Reservation_R..ISSUED_SCC set IDSTATUS = 2 where ID = @IDISSUED";
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_SCC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 2;
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_SCC"].Value = ScannedBook.IDISSUED;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC_ACTIONS (IDACTION,IDEMP,IDISSUED_SCC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_SCC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }
        internal void InsertSendEmailAction(int IDEMP, int IDISSUED_SCC)//здесь IDISSUED_SCC - это номер читателя, а не номер выдачи. потому что так! // ПОВЕРИТЬ ЧТО ЭТО ТАК!
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_SCC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 4;
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_SCC"].Value = IDISSUED_SCC;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC_ACTIONS (IDACTION,IDEMP,IDISSUED_SCC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_SCC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }

        internal DataTable GetLog()
        {
            DA.SelectCommand.CommandText = "with scc as ( " +
                            "select convert(VARCHAR(8),B.DATEACTION,108) [time],   " +
                            " bar.SORT collate Cyrillic_general_ci_ai bar,  " +
                            " case when avtp.PLAIN IS null then '' else avtp.PLAIN + '; ' end + " +
                            " case when titp.PLAIN IS null then '' else titp.PLAIN end collate Cyrillic_general_ci_ai tit,  " +
                            " cast(A.IDREADER as nvarchar) idr,  " +
                            " C.STATUSNAME st, 'ЦСК' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome " +
                            "from Reservation_R..ISSUED_SCC_ACTIONS B  " +
                            " left join Reservation_R..ISSUED_SCC A on A.ID = B.IDISSUED_SCC  " +
                            " left join Reservation_R..STATUS_ISSUED_SCC C on B.IDACTION = C.ID  " +
                            " left join BJSCC..DATAEXT tit on A.IDMAIN = tit.IDMAIN and tit.MNFIELD = 200 and tit.MSFIELD = '$a' and A.BaseId = 1 " +
                            " left join BJSCC..DATAEXTPLAIN titp on tit.ID = titp.IDDATAEXT  " +
                            " left join BJSCC..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' and A.BaseId = 1 " +
                            " left join BJSCC..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT  " +
                            " left join BJSCC..DATAEXT bar on A.IDDATA = bar.IDDATA and bar.MNFIELD = 899 and bar.MSFIELD = '$w'  and A.BaseId = 1 " +
                            " where cast(cast(B.DATEACTION as varchar(11)) as datetime)  " +
                            " = cast(cast(GETDATE() as varchar(11)) as datetime) and A.BaseId = 1 " +
                            " ), " +
                            " vvv as ( " +
                            "select convert(VARCHAR(8),B.DATEACTION,108) [time],   " +
                            "bar.SORT bar,  " +
                            " case when avtp.PLAIN IS null then '' else avtp.PLAIN + '; ' end + " +
                            " case when titp.PLAIN IS null then '' else titp.PLAIN end tit,  " +
                            " cast(A.IDREADER as nvarchar) idr,  " +
                            " C.STATUSNAME st, 'ОФ' fund, case when A.IsAtHome = 1 then 'на дом' else 'в зал' end IsAtHome " +
                            "from Reservation_R..ISSUED_SCC_ACTIONS B  " +
                            " left join Reservation_R..ISSUED_SCC A on A.ID = B.IDISSUED_SCC  " +
                            " left join Reservation_R..STATUS_ISSUED_SCC C on B.IDACTION = C.ID  " +
                            " left join BJVVV..DATAEXT tit on A.IDMAIN = tit.IDMAIN and tit.MNFIELD = 200 and tit.MSFIELD = '$a' and A.BaseId = 2 " +
                            " left join BJVVV..DATAEXTPLAIN titp on tit.ID = titp.IDDATAEXT  " +
                            " left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' and A.BaseId = 2 " +
                            " left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT  " +
                            " left join BJVVV..DATAEXT bar on A.IDDATA = bar.IDDATA and bar.MNFIELD = 899 and bar.MSFIELD = '$w'  and A.BaseId = 2 " +
                            " where cast(cast(B.DATEACTION as varchar(11)) as datetime)  " +
                            " = cast(cast(GETDATE() as varchar(11)) as datetime) and A.BaseId = 2 " +
                            " ) " +
                            " select * from scc " +
                            " union all " +
                            " select * from vvv " +
                            " order by time desc";
            DS = new DataSet();
            int i = DA.Fill(DS, "log");
            return DS.Tables["log"];
        }

        internal object GetOperatorActions(DateTime dateTime, DateTime dateTime_2, int EmpID)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select 1 ID,B.ACTION act,A.DATEACTION from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID "+
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and IDEMP = " + EmpID;
            DS = new DataSet();
            int i = DA.Fill(DS, "act");
            
            return DS.Tables["act"];
        }

        internal object GetDepReport(DateTime dateTime, DateTime dateTime_2)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION in (1,6)";
            DS = new DataSet();
            int i = DA.Fill(DS, "rep1");
            DS.Tables.Add("result");
            DS.Tables["result"].Columns.Add("num");
            DS.Tables["result"].Columns.Add("name");
            DS.Tables["result"].Columns.Add("kolvo");
            DS.Tables["result"].Rows.Add(new string[] { "1","Количество выданных книг",DS.Tables["rep1"].Rows[0][0].ToString()});

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 2";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep2");
            DS.Tables["result"].Rows.Add(new string[] { "2", "Количество принятых книг", DS.Tables["rep2"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(distinct C.IDREADER) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " left join Reservation_R..ISSUED_SCC C on A.IDISSUED_SCC = C.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION in (1,6)";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep3");
            DS.Tables["result"].Rows.Add(new string[] { "3", "Количество читателей, получивших книги", DS.Tables["rep3"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.SORT) from BJSCC..DATAEXT A " +
                                           " left join BJSCC..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$c' "+
                                           " where A.MNFIELD = 899 and A.MSFIELD = '$w' and B.SORT = 'Длявыдачи' ";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep4");
            DS.Tables["result"].Rows.Add(new string[] { "4", "Количество всех книг в фонде", DS.Tables["rep4"].Rows[0][0].ToString() });



            return DS.Tables["result"];
        }

        internal object GetOprReport(DateTime dateTime, DateTime dateTime_2, int p)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION in (1,6) and A.IDEMP = "+p;
            DS = new DataSet();
            int i = DA.Fill(DS, "rep1");
            DS.Tables.Add("result");
            DS.Tables["result"].Columns.Add("num");
            DS.Tables["result"].Columns.Add("name");
            DS.Tables["result"].Columns.Add("kolvo");
            DS.Tables["result"].Rows.Add(new string[] { "1", "Количество выданных книг", DS.Tables["rep1"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 2 and A.IDEMP = "+p;
            //DS = new DataSet();
            i = DA.Fill(DS, "rep2");
            DS.Tables["result"].Rows.Add(new string[] { "2", "Количество принятых книг", DS.Tables["rep2"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(distinct C.IDREADER) from Reservation_R..ISSUED_SCC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " left join Reservation_R..ISSUED_SCC C on A.IDISSUED_SCC = C.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION in (1,6) and A.IDEMP = "+p;
            //DS = new DataSet();
            i = DA.Fill(DS, "rep3");
            DS.Tables["result"].Rows.Add(new string[] { "3", "Количество читателей, получивших книги", DS.Tables["rep3"].Rows[0][0].ToString() });

            return DS.Tables["result"];
        }

        internal void RemoveResposibility(int idiss, int EmpID)
        {
            DA.UpdateCommand.Parameters.Clear();
            DA.UpdateCommand.Parameters.Add("IDISSUED", SqlDbType.Int);

            DA.UpdateCommand.Parameters["IDISSUED"].Value = idiss;
            DA.UpdateCommand.CommandText = "update Reservation_R..ISSUED_SCC set IDSTATUS = 2 where ID = @IDISSUED";
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_SCC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 5;
            DA.InsertCommand.Parameters["IDUSER"].Value = EmpID;
            DA.InsertCommand.Parameters["IDISSUED_SCC"].Value = idiss;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_SCC_ACTIONS (IDACTION,IDEMP,IDISSUED_SCC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_SCC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }
        internal void AddAttendance(ReaderVO reader)
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.AddWithValue("BAR", reader.BAR);
            if (reader.BAR[0] == 'G')
                DA.InsertCommand.Parameters.AddWithValue("IDReader", -1);
            else
                DA.InsertCommand.Parameters.AddWithValue("IDReader", reader.ID);

            DA.InsertCommand.CommandText = "insert into Reservation_R..ATTENDANCE_SCC (IDReader, DATEATT, BAR) values " +
                                           " (@IDReader, getdate(), @BAR)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }

        internal int GetAttendance()
        {
            DA.SelectCommand.CommandText = " select (ID) from Reservation_R..ATTENDANCE_SCC A " +
                                           " where cast(cast(A.DATEATT as varchar(11)) as datetime) between " +
                                           " cast(cast(getdate() as varchar(11)) as datetime) and cast(cast(getdate() as varchar(11)) as datetime) ";
            return DA.Fill(DS);
        }

    }
}
