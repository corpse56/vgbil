using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace BookkeepingForOrder
{
    public class DbForEmployee
    {
        private SqlDataAdapter SqlDA;
        private SqlConnection SqlCon;
        private OleDbDataAdapter OleDA;
        private OleDbConnection OleCon;
        private string OrdTableType;
        private string BASE;
        private Form1 F1;
        public DbForEmployee(string OrdTableType_,string BASE_, Form1 f1)
        {
            SqlCon = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDA = new SqlDataAdapter();
            SqlDA.SelectCommand = new SqlCommand();
            SqlDA.SelectCommand.Connection = SqlCon;
            OleCon = new OleDbConnection(XmlConnections.GetConnection("/Connections/Reader"));
            OleDA = new OleDbDataAdapter();
            OleDA.SelectCommand = new OleDbCommand();
            OleDA.SelectCommand.Connection = OleCon;
            this.BASE = BASE_;
            this.OrdTableType = OrdTableType_;
            this.F1 = f1;
        }

        public DataTable GetTable(string name)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SqlDA.SelectCommand.CommandText = "select O.ID oid, O.ID_Book_EC as idm,  avt.PLAIN avt, " +
              " zag.PLAIN zag, O.InvNumber as inv, Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr" +
              " , O.ID_Reader as idr,  dep.NAME as dp,fio.NAME fio, yaz.PLAIN yaz, ntp.PLAIN note,izd.PLAIN izd, gizd.PLAIN gizd " +  
              " from Reservation_E.." + this.OrdTableType + " O " +
              " left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
              " left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
              " left join BJVVV..DATAEXT dyaz on dyaz.ID = (select top 1 ID from BJVVV..DATAEXT B where B.IDMAIN = O.ID_Book_EC and B.MNFIELD = 101 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN yaz on dyaz.ID = yaz.IDDATAEXT" +
              " left join BJVVV..DATAEXT dzag on O.ID_Book_EC = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'" +
              " left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT" +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
              " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT" +
              " left join BJVVV..DATAEXT dmhran on O.ID_Book_EC = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'" +
              " left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT" +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
              " left join BJVVV..USERS fio on O.ID_Reader = fio.ID " +
              " left join BJVVV..LIST_8 dep on fio.DEPT = dep.ID " +
                //" where O.Start_Date >= '" + DateTime.Now.ToString("yyyyMMdd") +"'"
               " where O.Status != 10 " + name + " order by idm ";
            SqlDA.SelectCommand.CommandTimeout = 1200;
            DataSet ds = new DataSet();
            int count = SqlDA.Fill(ds, "orders");
            sw.Stop();

            //string s = ds.Tables["orders"].Rows[0]["dp"].ToString();
            //return this.GetCorrectTable(ds.Tables["orders"]);
            return ds.Tables["orders"];
        }
        internal DataTable GetReaders(string name)
        {
            SqlDA.SelectCommand.CommandText = "select O.ID oid, ISNULL(O.ALGIDM,O.ID_Book_EC) as idm,  avt.PLAIN avt,  " +
                "(case when O.ALGIDM is null then zag.PLAIN else zag.PLAIN + ' [Сплетено]' end) zag, O.InvNumber as inv,  " +
                "Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr , " +
                " O.ID_Reader as idr,  " +
                " cast(fio.NumberReader as nvarchar)+'; ' + fio.FamilyName +' '+ fio.[Name]+' ' + ISNULL(fio.FatherName,'')  dp, " +
                " cast(fio.NumberReader as nvarchar) fio, O.Start_Date startd,ntp.PLAIN note,izd.PLAIN izd, gizd.PLAIN gizd  " +
                "from Reservation_O.." + this.OrdTableType + " O  " +
                "left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
                "left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dzag on ISNULL(O.ALGIDM,O.ID_Book_EC) = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'  " +
                "left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT  " +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dmhran on ISNULL(O.ALGIDM,O.ID_Book_EC) = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'  " +
                "left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT  " +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
                " left join Readers..Main fio on O.ID_Reader = fio.NumberReader " +
                "where O.Start_Date <= '" + DateTime.Now.AddDays(7).ToString("yyyyMMdd") +
                "'" + name + " and O.Status = 0 order by idm ";
            SqlDA.SelectCommand.CommandTimeout = 1200;
            DataSet ds = new DataSet();
            int count = SqlDA.Fill(ds, "orders");
            //string s = ds.Tables["orders"].Rows[0]["dp"].ToString();
            //return this.GetCorrectTable(ds.Tables["orders"]);
            return (ds.Tables["orders"]);
        }
        public DataTable GetHistory(string name)
        {
            
            SqlDA.SelectCommand.CommandText = "select  O.ID oid, O.ID_Book_EC as idm,  avt.PLAIN avt, "+
              " zag.PLAIN zag, O.InvNumber as inv, Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr" +
              " , O.ID_Reader as idr, dep.[NAME] as dp,fio.NAME fio,  "+
              " O.Start_Date startd,O.REFUSUAL refusual, yaz.PLAIN yaz, ntp.PLAIN note,izd.PLAIN izd , gizd.PLAIN gizd " +    
              " from Reservation_E..OrdHis O  " +
                " left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
                " left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
              " left join BJVVV..DATAEXT dyaz on dyaz.ID = (select top 1 ID from BJVVV..DATAEXT B where B.IDMAIN = O.ID_Book_EC and B.MNFIELD = 101 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN yaz on dyaz.ID = yaz.IDDATAEXT" +
              " left join BJVVV..DATAEXT dzag on O.ID_Book_EC = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'" +
              " left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT" +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
              " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT" +
              " left join BJVVV..DATAEXT dmhran on O.ID_Book_EC = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'" +
              " left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT" +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
              " left join BJVVV..USERS fio on O.ID_Reader = fio.ID " +
              " left join BJVVV..LIST_8 dep on fio.DEPT = dep.ID " +//and dep.ID  = " + F1.FloorID+
              " left join BJVVV..USERS who on O.Who = who.ID " +
              " left join BJVVV..LIST_8 whod on who.DEPT = whod.ID " +
              " where O.Start_Date <= '" + DateTime.Now.ToString("yyyyMMdd HH:mm") + "' and O.Start_Date >=  '" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd HH:mm") +                                                         
              "' and O.Who = "+F1.EmpID+
              " union all " +
              "select O.ID oid, O.ID_Book_EC as idm,  avt.PLAIN avt, "+
              " zag.PLAIN zag, O.InvNumber as inv, Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr" +
              " ,O.ID_Reader as idr, dep.[NAME] as dp,fio.NAME fio,  "+
              " O.Start_Date startd,O.REFUSUAL refusual, yaz.PLAIN yaz, ntp.PLAIN note,izd.PLAIN izd, gizd.PLAIN gizd " +    
              " from Reservation_E..Orders O  " +
                " left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
                " left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
              " left join BJVVV..DATAEXT dyaz on dyaz.ID = (select top 1 ID from BJVVV..DATAEXT B where B.IDMAIN = O.ID_Book_EC and B.MNFIELD = 101 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN yaz on dyaz.ID = yaz.IDDATAEXT" +
              " left join BJVVV..DATAEXT dzag on O.ID_Book_EC = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'" +
              " left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT" +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
              " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
              " left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT" +
              " left join BJVVV..DATAEXT dmhran on O.ID_Book_EC = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'" +
              " left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT" +
                "  left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
              " left join BJVVV..USERS fio on O.ID_Reader = fio.ID " +
              " left join BJVVV..LIST_8 dep on fio.DEPT = dep.ID "+//and dep.ID  = " + F1.FloorID+
              " left join BJVVV..USERS who on O.Who = who.ID " +
              " left join BJVVV..LIST_8 whod on who.DEPT = whod.ID " +
              " where O.Start_Date <= '" + DateTime.Now.ToString("yyyyMMdd HH:mm") + "' and O.Start_Date >=  '" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd HH:mm") +                                                         
              //"'" + name + " order by idm ";
              //"' and dmhran.IDINLIST = "+F1.FloorID
                            "' and O.Who = "+F1.EmpID
              ;
            SqlDA.SelectCommand.CommandTimeout = 1200;
            DataSet ds = new DataSet();
            int count = SqlDA.Fill(ds,"ordhis");
            //string s = ds.Tables["orders"].Rows[0]["dp"].ToString();
            return ds.Tables["ordhis"];
        }
        internal DataTable GetReadersHistory(string name)
        {
            SqlDA.SelectCommand.CommandText = "select  O.ID oid, ISNULL(O.ALGIDM,O.ID_Book_EC) as idm,  avt.PLAIN avt,   " +
                "zag.PLAIN zag, O.InvNumber as inv,  " +
                "Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr , " +
                " O.ID_Reader as idr,   " +
                " cast(fio.NumberReader as nvarchar)+'; ' + fio.FamilyName +' '+ fio.[Name]+' ' + ISNULL(fio.FatherName,'')  dp, " +
                " cast(fio.NumberReader as nvarchar) fio,O.Start_Date startd,O.REFUSUAL refusual,stus.Name sts, ntp.PLAIN note,izd.PLAIN izd, gizd.PLAIN gizd  " +
                " from Reservation_O..OrdHis O  " +
                " left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
                " left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dzag on ISNULL(O.ALGIDM,O.ID_Book_EC) = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'  " +
                "left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT  " +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dmhran on ISNULL(O.ALGIDM,O.ID_Book_EC) = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'  " +
                "left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT  " +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
                " left join Readers..Main fio on O.ID_Reader = fio.NumberReader " +
                "left join Reservation_O..Status stus on O.Status = stus.ID " +
                " where O.Start_Date <= '" + DateTime.Now.ToString("yyyyMMdd HH:mm") + "' and O.Start_Date >=  '" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd HH:mm") +
                "'" + name + " and O.Status != 0 and dmhran.IDINLIST = " + F1.FloorID +

                " union all " +

                "select  O.ID oid, ISNULL(O.ALGIDM,O.ID_Book_EC) as idm,  avt.PLAIN avt,   " +
                "zag.PLAIN zag, O.InvNumber as inv,  " +
                "Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr , " +
                " O.ID_Reader as idr,  " +
                " cast(fio.NumberReader as nvarchar)+'; ' + fio.FamilyName +' '+ fio.[Name]+' ' + ISNULL(fio.FatherName,'')  dp, " +
                " cast(fio.NumberReader as nvarchar) fio, O.Start_Date startd,O.REFUSUAL refusual,stus.Name sts, ntp.PLAIN note,izd.PLAIN izd, gizd.PLAIN gizd  " +
                "from Reservation_O..Orders O  " +
                "left join BJVVV..DATAEXT invd on O.IDDATA = invd.IDDATA and invd.MNFIELD = 899 and invd.MSFIELD = '$p' " +
                "left join BJVVV..DATAEXTPLAIN inv on inv.IDDATAEXT = invd.ID " +
                "left join BJVVV..DATAEXT nt on ISNULL(O.ALGIDM,O.ID_Book_EC) = nt.IDMAIN and nt.MNFIELD = 899 and nt.MSFIELD = '$x' and nt.IDDATA = O.IDDATA " +
                "left join BJVVV..DATAEXTPLAIN ntp on nt.ID = ntp.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dzag on ISNULL(O.ALGIDM,O.ID_Book_EC) = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'  " +
                "left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT  " +
                " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT  " +
                " left join BJVVV..DATAEXT dizd on dizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 2100 and B.MSFIELD = '$d')" +
                "left join BJVVV..DATAEXTPLAIN izd on dizd.ID = izd.IDDATAEXT  " +
                " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                "left join BJVVV..DATAEXT dmhran on ISNULL(O.ALGIDM,O.ID_Book_EC) = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'  " +
                "left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT  " +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
                " left join Readers..Main fio on O.ID_Reader = fio.NumberReader " +
                "left join Reservation_O..Status stus on O.Status = stus.ID "+
                //" where O.Start_Date <= '" + DateTime.Now.AddDays(8).ToString("yyyyMMdd HH:mm") + "' and O.Start_Date >=  '" + DateTime.Now.AddDays(-10).ToString("yyyyMMdd HH:mm") +
                //"'" + name + " and O.Status in (1, 10,8,11) and dmhran.IDINLIST = " + F1.FloorID;
                " where dmhran.IDINLIST = " + F1.FloorID + " " + name ;
                
               
            SqlDA.SelectCommand.CommandTimeout = 1200;
            DataSet ds = new DataSet();
            int count = SqlDA.Fill(ds, "orders");
            //string s = ds.Tables["orders"].Rows[0]["dp"].ToString();
            return ds.Tables["orders"];
            //return (ds.Tables["orders"]);
        }
        public void delFromOrders(string oid)
        {
            SqlDataAdapter sdvig = new SqlDataAdapter();
            SqlCon.Open();
            sdvig.DeleteCommand = new SqlCommand("delete from Reservation_E..Orders where ID = " + oid, SqlCon);
            sdvig.DeleteCommand.ExecuteNonQuery();
            SqlCon.Close();
        }
        public void OrdHis(string oid, string empl) //перенос из таблицы корзина в таблицу читатели
        {
            DataSet DS = new DataSet();
            //SqlConnection con = new SqlConnection("Data Source=192.168.3.63;Initial Catalog=Reservation;Persist Security Info=True;User ID=Sasha;Password=Corpse536");
            SqlConnection con = new SqlConnection(XmlConnections.GetConnection("/Connections/Zakaz"));
            SqlDataAdapter sdvig = new SqlDataAdapter("select * from Reservation_E..Orders where ID = " + oid, con);
            int u  = sdvig.Fill(DS, "ord");
            con.Close();
            

            sdvig = new SqlDataAdapter("select * from Reservation_E..OrdHis where ID_Book_EC = 9999999" , con);
            sdvig.Fill(DS, "ordhis");

            DataRow r = DS.Tables["ordhis"].NewRow();
            r["ID_Reader"] = DS.Tables["ord"].Rows[0]["ID_Reader"].ToString();
            r["ID_Book_EC"] = DS.Tables["ord"].Rows[0]["ID_Book_EC"].ToString(); 
            r["ID_Book_CC"] = DS.Tables["ord"].Rows[0]["ID_Book_CC"].ToString(); //че сюда загонять?????пока ноль. это номер книги карточного каталога
            r["Status"] = 1; //требование распечатано
            r["Start_Date"] = DS.Tables["ord"].Rows[0]["Start_Date"].ToString(); 
            r["Change_Date"] =  DateTime.Now;
            r["InvNumber"] = DS.Tables["ord"].Rows[0]["InvNumber"].ToString(); 
            r["Form_Date"] = DS.Tables["ord"].Rows[0]["Form_Date"].ToString(); 
            r["Duration"] = DS.Tables["ord"].Rows[0]["Duration"].ToString();
            r["Who"] = empl;//этаж
            r["OID"] = DS.Tables["ord"].Rows[0]["ID"];//кто сменил статус
            r["REFUSUAL"] = DS.Tables["ord"].Rows[0]["REFUSUAL"];//отказ
            r["IDDATA"] = DS.Tables["ord"].Rows[0]["IDDATA"];
            r["ALGIDM"] = DS.Tables["ord"].Rows[0]["ALGIDM"];
            DS.Tables["ordhis"].Rows.Add(r);

            //SqlTransaction tract = new SqlTransaction();
            SqlCommandBuilder cb = new SqlCommandBuilder(sdvig);

            sdvig.InsertCommand = cb.GetInsertCommand();

            sdvig.Update(DS.Tables["ordhis"]);
        }




        internal void ChangeStatus(string oid, string idemp,string Floor)
        {
            SqlDataAdapter sdvig = new SqlDataAdapter();
            SqlCon.Open();
            /*if (Floor.Contains("Абонемент"))//потому что у абонемента нет подразделения книгохранения.
            {
                sdvig.UpdateCommand = new SqlCommand("update Reservation_O..Orders  set Status = 2,Who = " + idemp + " where ID = " + oid, SqlCon);
            }
            else*/
            {
                sdvig.UpdateCommand = new SqlCommand("update Reservation_O..Orders  set Status = 1,Who = " + idemp + " where ID = " + oid, SqlCon);
            }
            sdvig.UpdateCommand.ExecuteNonQuery();
            SqlCon.Close();
        }

        internal void RefusualEmployee(string cause, string oid)//перемещаем заказ из истории обратно
        {
            SqlDataAdapter sdvig = new SqlDataAdapter();
            SqlCon.Open();
            sdvig.UpdateCommand = new SqlCommand("update Reservation_E..OrdHis  set REFUSUAL = '" + cause + "',Status = 10 where ID = " + oid, SqlCon);
            sdvig.UpdateCommand.ExecuteNonQuery();
            SqlCon.Close();

            /*sdvig.InsertCommand = new SqlCommand();
            sdvig.InsertCommand.Connection = SqlCon;
            sdvig.InsertCommand.Connection.Open();
            sdvig.InsertCommand.CommandText = "insert into Reservation_E..Orders " +
                            " select ID_Reader,ID_Book_EC,ID_Book_CC, 10,Start_Date, " +
                            " Change_Date,InvNumber,Form_Date,Duration,Who,null,null,REFUSUAL " +
                            " from Reservation_E..OrdHis where ID = " + oid;
            sdvig.InsertCommand.ExecuteNonQuery();
            sdvig.InsertCommand.Connection.Close();

            sdvig.DeleteCommand = new SqlCommand();
            sdvig.DeleteCommand.Connection = SqlCon;
            sdvig.DeleteCommand.Connection.Open();
            sdvig.DeleteCommand.CommandText = "delete from Reservation_E..OrdHis where ID = " + oid;
            sdvig.DeleteCommand.ExecuteNonQuery();
            sdvig.DeleteCommand.Connection.Close();*/
        }

        internal void RefusualReader(string cause, string oid)//здесь перемещать заказ из истории обратно не нужно.
        {
            SqlDataAdapter sdvig = new SqlDataAdapter();
            SqlCon.Open();
            sdvig.UpdateCommand = new SqlCommand("update Reservation_O..Orders  set REFUSUAL = '" + cause + "', Status = 10 where ID = " + oid, SqlCon);
            sdvig.UpdateCommand.ExecuteNonQuery();
            SqlCon.Close();

            /*sdvig.InsertCommand = new SqlCommand();
            sdvig.InsertCommand.Connection = SqlCon;
            sdvig.InsertCommand.Connection.Open();
            sdvig.InsertCommand.CommandText = "insert into Reservation_O..Orders " +
                            " select ID_Reader,ID_Book_EC,ID_Book_CC, 10,Start_Date, " +
                            " Change_Date,InvNumber,Form_Date,Duration,Who,ALGIDM,IDDATA,REFUSUAL " +
                            " from Reservation_O..OrdHis where ID = " + oid;
            sdvig.InsertCommand.ExecuteNonQuery();
            sdvig.InsertCommand.Connection.Close();

            sdvig.DeleteCommand = new SqlCommand();
            sdvig.DeleteCommand.Connection = SqlCon;
            sdvig.DeleteCommand.Connection.Open();
            sdvig.DeleteCommand.CommandText = "delete from Reservation_O..OrdHis where ID = " + oid;
            sdvig.DeleteCommand.ExecuteNonQuery();
            sdvig.DeleteCommand.Connection.Close();*/
            
        }


        internal DataTable GetReadersChoosing(string name)
        {
            SqlDA.SelectCommand.CommandText = "select O.ID oid, ISNULL(O.ALGIDM,O.ID_Book_EC) as idm,  avt.PLAIN avt,   " +
                " (case when O.ALGIDM is null then zag.PLAIN else zag.PLAIN + ' [Сплетено]' end) zag, O.InvNumber as inv,  " +
                " Reservation_R.dbo.GetSHIFRBJVVVINVIDDATA(O.InvNumber,inv.IDDATA) shifr , " +
                "  O.ID_Reader as idr,   " +
                " cast(fio.NumberReader as nvarchar)+'; ' + fio.FamilyName +' '+ fio.[Name]+' ' + ISNULL(fio.FatherName,'')  dp, " +
                " cast(fio.NumberReader as nvarchar) fio, O.Start_Date startd, O.INOTE note, yaz.PLAIN yaz, gizd.PLAIN gizd " +
                " from Reservation_O.." + this.OrdTableType + " O  " +
                " join BJVVV..DATAEXTPLAIN inv on O.InvNumber COLLATE Cyrillic_General_CI_AI = inv.PLAIN and inv.IDMAIN = ISNULL(O.ALGIDM,O.ID_Book_EC) " +
                " left join BJVVV..DATAEXT dzag on ISNULL(O.ALGIDM,O.ID_Book_EC) = dzag.IDMAIN and dzag.MNFIELD = 200 and dzag.MSFIELD = '$a'  " +
                " left join BJVVV..DATAEXTPLAIN zag on dzag.ID = zag.IDDATAEXT  " +
              " left join BJVVV..DATAEXT dyaz on dyaz.ID = (select top 1 ID from BJVVV..DATAEXT B where B.IDMAIN = O.ID_Book_EC and B.MNFIELD = 101 and B.MSFIELD = '$a')" +
                " left join BJVVV..DATAEXTPLAIN yaz on dyaz.ID = yaz.IDDATAEXT" +
              " left join BJVVV..DATAEXT davt on davt.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 700 and B.MSFIELD = '$a')" +
                " left join BJVVV..DATAEXTPLAIN avt on davt.ID = avt.IDDATAEXT  " +
                " left join BJVVV..DATAEXT dmhran on ISNULL(O.ALGIDM,O.ID_Book_EC) = dmhran.IDMAIN and dmhran.IDDATA = inv.IDDATA and dmhran.MNFIELD = 899 and dmhran.MSFIELD = '$a'  " +
                " left join BJVVV..DATAEXT mizd on mizd.ID = (select top 1 ID from BJVVV..DATAEXT B where O.ID_Book_EC = B.IDMAIN and B.MNFIELD = 210 and B.MSFIELD = '$a')" +
                "left join BJVVV..DATAEXTPLAIN gizd on mizd.ID = gizd.IDDATAEXT  " +
                " left JOIN BJVVV..DATAEXTPLAIN MHRANshort on dmhran.ID = MHRANshort.IDDATAEXT  " +
                " left join BJVVV..LIST_8 mhran on dmhran.IDINLIST = mhran.ID   " +
                " left join Readers..Main fio on O.ID_Reader = fio.NumberReader " +
                " where O.Status = 1 " + name + " order by idm ";
            SqlDA.SelectCommand.CommandTimeout = 1200;
            DataSet ds = new DataSet();
            int count = SqlDA.Fill(ds, "orders");
            //string s = ds.Tables["orders"].Rows[0]["dp"].ToString();
            return ds.Tables["orders"];
            //return (ds.Tables["orders"]);       
        }
    }
}