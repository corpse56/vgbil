using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CirculationACC
{
    public class DBGeneral:DB
    {
//=====================================================================LOGIN==============================================================================
        public int EmpID;
        public string UserName;

        public bool Login(string name,string pass)
        {
            DA.SelectCommand.CommandText = "select * from BJACC..USERS where lower(LOGIN) = '" + name.ToLower() + "' and lower(PASSWORD) = '" + pass.ToLower() + "'";
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
            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJACC..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + data + "'";
            DS = new DataSet();
            int i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.BookACC;

            DA.SelectCommand.CommandText = "select top 1 IDMAIN from BJVVV..DATAEXT where MNFIELD = 899 and MSFIELD = '$w' and SORT = '" + data + "'";
            DS = new DataSet();
            i = DA.Fill(DS, "t");
            if (i > 0) return BARTYPE.BookBJVVV;

            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where BarCode = '" + data.Substring(1) + "'";
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
            DA.SelectCommand.CommandText = "select top 1 NumberReader from Readers..Main where NumberSC = '" + data.Substring(0, data.IndexOf(" ")) +
                                                                                       "' and SerialSC = '" + data.Substring(data.IndexOf(" ") + 1) + "'";
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
        internal int ISSUE(BookVO ScannedBook, ReaderVO ScannedReader, int IDEMP)
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDMAIN", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDDATA", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDREADER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATE_ISSUE", SqlDbType.DateTime);
            DA.InsertCommand.Parameters.Add("DATE_RETURN", SqlDbType.DateTime);
            DA.InsertCommand.Parameters.Add("IDSTATUS", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("BaseId", SqlDbType.Int);

            DA.InsertCommand.Parameters["IDMAIN"].Value = ScannedBook.IDMAIN;
            DA.InsertCommand.Parameters["IDDATA"].Value = ScannedBook.IDDATA;
            DA.InsertCommand.Parameters["IDREADER"].Value = ScannedReader.ID;
            DA.InsertCommand.Parameters["DATE_ISSUE"].Value = DateTime.Now;
            DA.InsertCommand.Parameters["DATE_RETURN"].Value = DateTime.Now.AddDays(21);
            DA.InsertCommand.Parameters["IDSTATUS"].Value = (ScannedBook.FUND == Bases.BJACC) ? 1 : 6;//1 - выдано из Центр Американской Культуры, 6 - выдано из основного фонда
            DA.InsertCommand.Parameters["BaseId"].Value = (ScannedBook.FUND == Bases.BJACC) ? 1 : 2;
            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_ACC (IDMAIN,IDDATA,IDREADER,DATE_ISSUE,DATE_RETURN,IDSTATUS,BaseId) values " +
                                            " (@IDMAIN,@IDDATA,@IDREADER,@DATE_ISSUE,@DATE_RETURN,@IDSTATUS,@BaseId);select scope_identity();";
            DA.InsertCommand.Connection.Open();
            object scope_id = DA.InsertCommand.ExecuteScalar();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_ACC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = (ScannedBook.FUND == Bases.BJACC) ? 1 : 6;//1 - выдано из Центр Американской Культуры, 6 - выдано из основного фонда
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_ACC"].Value = scope_id;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;



            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_ACC_ACTIONS (IDACTION,IDEMP,IDISSUED_ACC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_ACC,@DATEACTION)";
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();

            return 0;


        }
        internal void Recieve(BookVO ScannedBook, ReaderVO ScannedReader, int IDEMP)
        {
            DA.UpdateCommand.Parameters.Clear();
            DA.UpdateCommand.Parameters.Add("IDISSUED", SqlDbType.Int);

            DA.UpdateCommand.Parameters["IDISSUED"].Value = ScannedBook.IDISSUED;
            DA.UpdateCommand.CommandText = "update Reservation_R..ISSUED_ACC set IDSTATUS = 2 where ID = @IDISSUED";
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_ACC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 2;
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_ACC"].Value = ScannedBook.IDISSUED;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_ACC_ACTIONS (IDACTION,IDEMP,IDISSUED_ACC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_ACC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }
        public void InsertSendEmailAction(int IDEMP, int IDISSUED_ACC)//здесь IDISSUED_ACC - это номер читателя, а не номер выдачи. потому что так! 
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_ACC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 4;
            DA.InsertCommand.Parameters["IDUSER"].Value = IDEMP;
            DA.InsertCommand.Parameters["IDISSUED_ACC"].Value = IDISSUED_ACC;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_ACC_ACTIONS (IDACTION,IDEMP,IDISSUED_ACC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_ACC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }

        public DataTable GetLog()
        {
            DA.SelectCommand.CommandText = "with acc as ( " +
                            "select convert(VARCHAR(8),B.DATEACTION,108) [time],   " +
                            " bar.SORT collate Cyrillic_general_ci_ai bar,  " +
                            " case when avtp.PLAIN IS null then '' else avtp.PLAIN + '; ' end + " +
                            " case when titp.PLAIN IS null then '' else titp.PLAIN end collate Cyrillic_general_ci_ai tit,  " +
                            " A.IDREADER idr,  " +
                            " C.STATUSNAME st, 'ЦАК' fund  " +
                            "from Reservation_R..ISSUED_ACC_ACTIONS B  " +
                            " left join Reservation_R..ISSUED_ACC A on A.ID = B.IDISSUED_ACC  " +
                            " left join Reservation_R..STATUS_ISSUED_ACC C on B.IDACTION = C.ID  " +
                            " left join BJACC..DATAEXT tit on A.IDMAIN = tit.IDMAIN and tit.MNFIELD = 200 and tit.MSFIELD = '$a' and A.BaseId = 1 " +
                            " left join BJACC..DATAEXTPLAIN titp on tit.ID = titp.IDDATAEXT  " +
                            " left join BJACC..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' and A.BaseId = 1 " +
                            " left join BJACC..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT  " +
                            " left join BJACC..DATAEXT bar on A.IDDATA = bar.IDDATA and bar.MNFIELD = 899 and bar.MSFIELD = '$w'  and A.BaseId = 1 " +
                            " where cast(cast(B.DATEACTION as varchar(11)) as datetime)  " +
                            " = cast(cast(GETDATE() as varchar(11)) as datetime) and A.BaseId = 1 " +
                            " ), " +
                            " vvv as ( " +
                            "select convert(VARCHAR(8),B.DATEACTION,108) [time],   " +
                            "bar.SORT bar,  " +
                            " case when avtp.PLAIN IS null then '' else avtp.PLAIN + '; ' end + " +
                            " case when titp.PLAIN IS null then '' else titp.PLAIN end tit,  " +
                            " A.IDREADER idr,  " +
                            " C.STATUSNAME st, 'ОФ' fund  " +
                            "from Reservation_R..ISSUED_ACC_ACTIONS B  " +
                            " left join Reservation_R..ISSUED_ACC A on A.ID = B.IDISSUED_ACC  " +
                            " left join Reservation_R..STATUS_ISSUED_ACC C on B.IDACTION = C.ID  " +
                            " left join BJVVV..DATAEXT tit on A.IDMAIN = tit.IDMAIN and tit.MNFIELD = 200 and tit.MSFIELD = '$a' and A.BaseId = 2 " +
                            " left join BJVVV..DATAEXTPLAIN titp on tit.ID = titp.IDDATAEXT  " +
                            " left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' and A.BaseId = 2 " +
                            " left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT  " +
                            " left join BJVVV..DATAEXT bar on A.IDDATA = bar.IDDATA and bar.MNFIELD = 899 and bar.MSFIELD = '$w'  and A.BaseId = 2 " +
                            " where cast(cast(B.DATEACTION as varchar(11)) as datetime)  " +
                            " = cast(cast(GETDATE() as varchar(11)) as datetime) and A.BaseId = 2 " +
                            " ) " +
                            " select * from acc " +
                            " union all " +
                            " select * from vvv " +
                            " order by time desc";
            DS = new DataSet();
            int i = DA.Fill(DS, "log");
            return DS.Tables["log"];

        }



        public object GetOperatorActions(DateTime dateTime, DateTime dateTime_2, int EmpID)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select 1 ID,B.ACTION act,A.DATEACTION from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID "+
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and IDEMP = " + EmpID;
            DS = new DataSet();
            int i = DA.Fill(DS, "act");
            
            return DS.Tables["act"];
        }

        public object GetDepReport(DateTime dateTime, DateTime dateTime_2)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 1";
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
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 2";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep2");
            DS.Tables["result"].Rows.Add(new string[] { "2", "Количество принятых книг", DS.Tables["rep2"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(distinct C.IDREADER) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " left join Reservation_R..ISSUED_ACC C on A.IDISSUED_ACC = C.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 1";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep3");
            DS.Tables["result"].Rows.Add(new string[] { "3", "Количество читателей, получивших книги", DS.Tables["rep3"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.SORT) from BJACC..DATAEXT A " +
                                           " left join BJACC..DATAEXT B on A.IDDATA = B.IDDATA and B.MNFIELD = 921 and B.MSFIELD = '$c' "+
                                           " where A.MNFIELD = 899 and A.MSFIELD = '$w' and B.SORT = 'Disponible' ";
            //DS = new DataSet();
            i = DA.Fill(DS, "rep4");
            DS.Tables["result"].Rows.Add(new string[] { "4", "Количество всех книг в фонде (в базе ЦАК)", DS.Tables["rep4"].Rows[0][0].ToString() });



            return DS.Tables["result"];
        }

        public object GetOprReport(DateTime dateTime, DateTime dateTime_2, int p)
        {
            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 1 and A.IDEMP = "+p;
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
            DA.SelectCommand.CommandText = " select count(A.DATEACTION) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 2 and A.IDEMP = "+p;
            //DS = new DataSet();
            i = DA.Fill(DS, "rep2");
            DS.Tables["result"].Rows.Add(new string[] { "2", "Количество принятых книг", DS.Tables["rep2"].Rows[0][0].ToString() });

            DA.SelectCommand.Parameters.Clear();
            DA.SelectCommand.Parameters.AddWithValue("start", dateTime.Date);
            DA.SelectCommand.Parameters.AddWithValue("end", dateTime_2.Date);
            DA.SelectCommand.CommandText = " select count(distinct C.IDREADER) from Reservation_R..ISSUED_ACC_ACTIONS A " +
                                           " left join Reservation_R..ACTIONSTYPE B on A.IDACTION = B.ID " +
                                           " left join Reservation_R..ISSUED_ACC C on A.IDISSUED_ACC = C.ID " +
                                           " where cast(cast(A.DATEACTION as varchar(11)) as datetime) between @start and @end and A.IDACTION = 1 and A.IDEMP = "+p;
            //DS = new DataSet();
            i = DA.Fill(DS, "rep3");
            DS.Tables["result"].Rows.Add(new string[] { "3", "Количество читателей, получивших книги", DS.Tables["rep3"].Rows[0][0].ToString() });



            return DS.Tables["result"];
        }

        public void RemoveResposibility(int idiss, int EmpID)
        {
            DA.UpdateCommand.Parameters.Clear();
            DA.UpdateCommand.Parameters.Add("IDISSUED", SqlDbType.Int);

            DA.UpdateCommand.Parameters["IDISSUED"].Value = idiss;
            DA.UpdateCommand.CommandText = "update Reservation_R..ISSUED_ACC set IDSTATUS = 2 where ID = @IDISSUED";
            DA.UpdateCommand.Connection.Open();
            DA.UpdateCommand.ExecuteNonQuery();
            DA.UpdateCommand.Connection.Close();

            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.Add("IDACTION", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDUSER", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("IDISSUED_ACC", SqlDbType.Int);
            DA.InsertCommand.Parameters.Add("DATEACTION", SqlDbType.DateTime);

            DA.InsertCommand.Parameters["IDACTION"].Value = 5;
            DA.InsertCommand.Parameters["IDUSER"].Value = EmpID;
            DA.InsertCommand.Parameters["IDISSUED_ACC"].Value = idiss;
            DA.InsertCommand.Parameters["DATEACTION"].Value = DateTime.Now;

            DA.InsertCommand.CommandText = "insert into Reservation_R..ISSUED_ACC_ACTIONS (IDACTION,IDEMP,IDISSUED_ACC,DATEACTION) values " +
                                            "(@IDACTION,@IDUSER,@IDISSUED_ACC,@DATEACTION)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }

        public void AddAttendance(ReaderVO reader)
        {
            DA.InsertCommand.Parameters.Clear();
            DA.InsertCommand.Parameters.AddWithValue("BAR", reader.BAR);
            if (reader.BAR[0] == 'G')
                DA.InsertCommand.Parameters.AddWithValue("IDReader", -1);
            else
                DA.InsertCommand.Parameters.AddWithValue("IDReader", reader.ID);

            DA.InsertCommand.CommandText = "insert into Reservation_R..ATTENDANCE_ACC (IDReader, DATEATT, BAR) values " +
                                           " (@IDReader, getdate(), @BAR)";
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();
        }

        public int GetAttendance()
        {
            DA.SelectCommand.CommandText = " select (ID) from Reservation_R..ATTENDANCE_ACC A " +
                                           " where cast(cast(A.DATEATT as varchar(11)) as datetime) between " +
                                           " cast(cast(getdate() as varchar(11)) as datetime) and cast(cast(getdate() as varchar(11)) as datetime) ";
            return DA.Fill(DS);
        }
    }
}
