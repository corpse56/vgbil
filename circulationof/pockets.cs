using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Circulation
{
    public class pockets
    {
        SqlConnection con;
        SqlDataAdapter DA;
        DataSet DS;
        List<pocket> PockList;
        int Hall;
        public struct pocket 
        {
            public int ID;
            public int IDReader;
            public string INV;
            public pocket(int id_, int IDReader_, string INV_)
            {
                this.ID = id_;
                this.IDReader = IDReader_;
                this.INV = INV_;
            }
        }
        public pockets(int hall, string BASENAME)
        {
            this.Hall = hall;
            Circulation.DBWork.XmlConnections xml = new Circulation.DBWork.XmlConnections();
            con = new SqlConnection(xml.GetBJVVVCon());
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            PockList = new List<pocket>();
            DA.SelectCommand.CommandText = "select * from " + BASENAME + "..POCKETS where IDHALL = " + hall.ToString();
            DA.Fill(DS, "p");
            foreach (DataRow r in DS.Tables["p"].Rows)
            {
                pocket p = new pocket(int.Parse(r["IDPOCKET"].ToString()), int.Parse(r["IDREADER"].ToString()), r["INV"].ToString());
                PockList.Add(p);
            }
            PockList.Sort(comprasion);
        }
        private static int comprasion(pocket x,pocket y)
        {
            if (x.ID == y.ID)
            {
                return 0;
            }
            if (x.ID > y.ID)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public class pockEqualityComparer : IEqualityComparer<pocket>
        {
            public bool Equals(pocket x, pocket y)
            {
                return (x.ID == y.ID) ? true : false;
            }
            public int GetHashCode(pocket pock)
            {
                return pock.ToString().GetHashCode();
            }
        }
        private void ReadDB(string BASENAME)
        {
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            PockList = new List<pocket>();
            DA.SelectCommand.CommandText = "select * from " + BASENAME + "..POCKETS where IDHALL = " + this.Hall.ToString();
            DS = new DataSet();
            DA.Fill(DS, "p");
            foreach (DataRow r in DS.Tables["p"].Rows)
            {
                pocket p = new pocket(int.Parse(r["IDPOCKET"].ToString()), int.Parse(r["IDREADER"].ToString()), r["INV"].ToString());
                PockList.Add(p);
            }
            PockList.Sort(comprasion);
        }
        public int AddFirstFree(int idr, string inv, string BASENAME)
        {
            ReadDB("");
            List<pocket> dst = PockList.Distinct(new pockEqualityComparer()).ToList();
            if (dst.Count != PockList.Count)
            {
                throw new Exception("Номера карманов не могут совпадать! Обратитесь к разработчику!");
            }
            DataSet DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select * from " + BASENAME + "..POCKETS where IDREADER = " + idr.ToString() + " and INV = '" + inv + "' and IDHALL = " + this.Hall.ToString();
            int dublid = DA.Fill(DS, "p");
            if (dublid != 0) return -1;
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = con;
            DA.SelectCommand.CommandText = "select * from " + BASENAME + "..POCKETS where INV = '" + inv + "' and IDHALL = " + this.Hall.ToString();
            int dublinv = DA.Fill(DS, "p");
            if (dublinv != 0)
            {
                MessageBox.Show("Повторный инвентарь!");
            }
            int MaxID = GetMaxID();
            PockList.Add(new pocket(MaxID , idr, inv));
            WriteDB("");
            return MaxID;
        }
        private int GetMaxID()
        {
            PockList.Sort(comprasion);
            int i;
            for (i = 0; i < PockList.Count; i++)
            {
                if (PockList[i].ID != i + 1)
                {
                    return i + 1;
                }
            }
            return i + 1;
        }
        public int FreePocketByINV(string inv)
        {
            this.finv = inv;
            int idx = PockList.FindIndex(findinv);
            if (idx == -1) return -1;
            int retval = PockList[idx].ID;
            PockList.RemoveAt(idx);
            WriteDB("");
            return retval;
        }
        public int GetPocketByInv(string inv, string hl, string BASENAME)
        {
            this.finv = inv;
            int idx = PockList.FindIndex(findinv);
            return PockList[idx].ID;
        }
        public static void DelPocketByInvAndHall(string inv, string hl, string BASENAME)
        {
            string hall = pockets.GetZalIssID(hl);
            Conn.SQLDA.DeleteCommand = new SqlCommand();
            Conn.SQLDA.DeleteCommand.CommandText = "delete from " + BASENAME + "..POCKETS where INV = '" + inv + "' and IDHALL =" + hall;
            Conn.SQLDA.DeleteCommand.Connection = Conn.ZakazCon;
            if (Conn.ZakazCon.State != ConnectionState.Open)
            {
                Conn.ZakazCon.Open();
            }
            Conn.SQLDA.DeleteCommand.ExecuteNonQuery();
            Conn.SQLDA.DeleteCommand.Connection.Close();
        }
        public static string GetZalIssID(string sname)
        {
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select ID from BJVVV..LIST_8 where SHORTNAME = '" + sname + "'";
            Conn.SQLDA.SelectCommand.Connection = Conn.BJVVVConn;
            DataSet DS = new DataSet();
            Conn.SQLDA.Fill(DS, "t");
            return DS.Tables[0].Rows[0]["ID"].ToString();
        }
        public static string FindPocketByInvAndHall(string inv, string hl, string BASENAME)
        {
            string hall = pockets.GetZalIssID(hl);
            Conn.SQLDA.SelectCommand = new SqlCommand();
            Conn.SQLDA.SelectCommand.CommandText = "select IDPOCKET from " + BASENAME + "..POCKETS where INV = '" + inv + "' and IDHALL =" + hall;
            Conn.SQLDA.SelectCommand.Connection = Conn.BJVVVConn;
            DataSet DS = new DataSet();
            int i = Conn.SQLDA.Fill(DS, "t");
            if (i == 0)
            {
                MessageBox.Show("Карман не найден. Обратитесь к разработчику!");
                return "";
            }
            return DS.Tables[0].Rows[0]["IDPOCKET"].ToString();
        }
        private string finv;
        private bool findinv(pocket p)
        {
            if (p.INV == this.finv)
                return true;
            else
                return false;
        }
        public void DeleteByInv(string inv, string BASENAME)
        {
            DA.DeleteCommand = new SqlCommand();
            DA.DeleteCommand.Connection = this.con;
            if (DA.DeleteCommand.Connection.State != ConnectionState.Open)
            {
                DA.DeleteCommand.Connection.Open();
            }
            DA.DeleteCommand.CommandText = "delete from " + BASENAME + "..POCKETS where INV = " + inv;
            DA.DeleteCommand.ExecuteNonQuery();
            DA.DeleteCommand.Connection.Close();
        }
        private void WriteDB(string BASENAME)
        {
            DA.DeleteCommand = new SqlCommand();
            DA.DeleteCommand.Connection = this.con;
            if (DA.DeleteCommand.Connection.State != ConnectionState.Open)
            {
                DA.DeleteCommand.Connection.Open();
            }
            DA.DeleteCommand.CommandText = "delete from " + BASENAME + "..POCKETS where IDHALL = " + this.Hall.ToString();
            DA.DeleteCommand.ExecuteNonQuery();
            DA.DeleteCommand.Connection.Close();
            DA.SelectCommand.CommandText = "select top 1 * from " + BASENAME + "..POCKETS";
            DataTable POCKETS = new DataTable("POCKETS");
            DA.FillSchema(POCKETS, SchemaType.Mapped);
            foreach (pocket p in PockList)
            {
                DataRow r = POCKETS.NewRow();
                r["IDHALL"] = Hall;
                r["IDPOCKET"] = p.ID;
                r["INV"] = p.INV;
                r["IDREADER"] = p.IDReader;
                POCKETS.Rows.Add(r);
            }
            SqlBulkCopy bc = new SqlBulkCopy(con);
            con.Open();
            bc.DestinationTableName = "" + BASENAME + "..POCKETS";
            bc.WriteToServer(POCKETS);
            con.Close();
        }
    }
}
