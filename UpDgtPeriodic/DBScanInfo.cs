using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using XMLConnections;

namespace UpDgtPeriodic
{
    public class DBScanInfo
    {
        public DBScanInfo() { }
        public static bool IsEBook(int IDZ)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Value = IDZ;
            da.SelectCommand.CommandText = "select * from PERIOD..[PI] where VVERH = (select VVERH from PERIOD..[PI] where IDZ = @IDZ and IDF = 219) and IDF = 363";
            if (da.Fill(ds, "t") == 0)
            {
                return false;
                //throw new Exception("Не найдена гиперссылка");
            }
            if (ds.Tables["t"].Rows[0]["POLE"].ToString() == "e-book")
                return true;
            else
                return false;

        }
        public bool IfExistsYearByIDZandFolderYear(int idz,string FolderYear)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Value = idz;
            da.SelectCommand.Parameters.Add("fyear", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["fyear"].Value = FolderYear;
            //da.SelectCommand.CommandText = "with FA as" +
            //                               " (" +
            //                               " select A.* from PERIOD..[PI] A" +
            //                               " where A.IDZ = @IDZ" +
            //                               " union all " +
            //                               " select B.* from PERIOD..[PI] B" +
            //                               " inner join FA C on C.VVERH = B.IDZ" +
            //                               " )" +
            //                               " select * from FA where IDF = 131";
            da.SelectCommand.CommandText = " select A.* from PERIOD..[PI] A" +
                                           " where A.VVERH = @IDZ and IDF = 131 and POLE = @fyear";
            if (da.Fill(ds, "t") == 0)
            {
                //throw new Exception("В базе не найден год, соответствующий названию выбранной папки!");
                return false;
            }
            return true;
        }
        public string GetPINbyIDZ(int idz)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Value = idz;
            
            da.SelectCommand.CommandText = " select POLE from PERIOD..[PI]  " +
                                           " where IDZ = @IDZ and IDF = 120 ";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("В базе не найден ПИН!");
            }
            return ds.Tables["t"].Rows[0]["POLE"].ToString();
        }
        public string GetTitleByIDZ(int idz)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Value = idz;
            //da.SelectCommand.CommandText = "with FA as" +
            //                               " (" +
            //                               " select A.* from PERIOD..[PI] A" +
            //                               " where A.IDZ = @IDZ" +
            //                               " union all " +
            //                               " select B.* from PERIOD..[PI] B" +
            //                               " inner join FA C on C.VVERH = B.IDZ" +
            //                               " )" +
            //                               " ,FB as (" +
            //                               " select * from FA " +
            //                               " where IDF = 131 " +
            //                               " ) " +
            //                               " select * from PERIOD..[PI]  " +
            //                               " where VVERH = (select VVERH from FB) and IDF = 121 ";
            da.SelectCommand.CommandText = " select * from PERIOD..[PI]  " +
                                           " where VVERH = @IDZ and IDF = 121 ";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Не найдено заглавие");
            }
            return ds.Tables["t"].Rows[0]["POLE"].ToString();
        }

        public void InsertUploadInfo(string Year, string UUser, string Package, int IDZ, bool PDF, string PIN)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.CommandText = " select * from BookAddInf..DgtPeriodic  " +
                                           " where PIN ='"+PIN+"' and YEAR = '"+Year+"'";
            if (da.Fill(ds, "t") != 0)
            {
                da = new SqlDataAdapter();
                da.UpdateCommand = new SqlCommand();
                da.UpdateCommand.Connection = new SqlConnection();
                da.UpdateCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
                da.UpdateCommand.Parameters.Add("PIN", SqlDbType.NVarChar);
                da.UpdateCommand.Parameters["PIN"].Value = PIN;
                da.UpdateCommand.Parameters.Add("YEAR", SqlDbType.NVarChar);
                da.UpdateCommand.Parameters["YEAR"].Value = Year;
                da.UpdateCommand.Parameters.Add("CHANGED", SqlDbType.DateTime);
                da.UpdateCommand.Parameters["CHANGED"].Value = DateTime.Now;
                da.UpdateCommand.Parameters.Add("PACKAGE", SqlDbType.NVarChar);
                if (PDF)
                {
                    da.UpdateCommand.Parameters["PACKAGE"].Value = ds.Tables["t"].Rows[0]["PACKAGE"].ToString() + Package;
                }
                else
                {
                    da.UpdateCommand.Parameters["PACKAGE"].Value = Package;
                }
                da.UpdateCommand.Parameters.Add("CHANGER", SqlDbType.NVarChar);
                da.UpdateCommand.Parameters["CHANGER"].Value = UUser;
                da.UpdateCommand.CommandText = "update BookAddInf..DgtPeriodic set PIN = @PIN,CHANGED = @CHANGED,CHANGER = @CHANGER,PACKAGE=@PACKAGE where PIN = @PIN and YEAR = @YEAR";
                da.UpdateCommand.Connection.Open();
                da.UpdateCommand.ExecuteNonQuery();
                da.UpdateCommand.Connection.Close();

            }
            else
            {
                da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand();
                da.InsertCommand.Connection = new SqlConnection();
                da.InsertCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
                da.InsertCommand.Parameters.Add("PIN", SqlDbType.NVarChar);
                da.InsertCommand.Parameters["PIN"].Value = PIN;
                da.InsertCommand.Parameters.Add("YEAR", SqlDbType.NVarChar);
                da.InsertCommand.Parameters["YEAR"].Value = Year;
                da.InsertCommand.Parameters.Add("DATEADD", SqlDbType.DateTime);
                da.InsertCommand.Parameters["DATEADD"].Value = DateTime.Now;
                da.InsertCommand.Parameters.Add("PACKAGE", SqlDbType.NVarChar);
                da.InsertCommand.Parameters["PACKAGE"].Value = Package;
                da.InsertCommand.Parameters.Add("CREATOR", SqlDbType.NVarChar);
                da.InsertCommand.Parameters["CREATOR"].Value = UUser;
                da.InsertCommand.CommandText = "insert into BookAddInf..DgtPeriodic (PIN,YEAR,DATEADD,CREATOR,PACKAGE) values " +
                    "(@PIN,@YEAR,@DATEADD,@CREATOR,@PACKAGE)";
                da.InsertCommand.Connection.Open();
                da.InsertCommand.ExecuteNonQuery();
                da.InsertCommand.Connection.Close();
            }
        }

        public void InsertPackage(List<string> PackageList, string PIN,int YEAR)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = new SqlCommand();
            da.UpdateCommand.Connection = new SqlConnection();
            da.UpdateCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.UpdateCommand.Parameters.Add("PACKAGE", SqlDbType.NVarChar);
            da.UpdateCommand.Parameters["PACKAGE"].Value = PackageList[0];
            da.UpdateCommand.Parameters.Add("IDZ", SqlDbType.Int);
            //DataRowCollection rows = GetIDZofAllPackagesByHyperLinkIDZ(IDZ);
            DataRowCollection rows = GetIDZofAllPackagesByPINandYEAR(PIN,YEAR);

            foreach (DataRow r in rows)//обнуляем все составы комплектов
            {
                da.UpdateCommand.Parameters["IDZ"].Value = (int)r["IDZ"];
                da.UpdateCommand.CommandText = "update PERIOD..PI set POLE = '' where IDZ = @IDZ";
                da.UpdateCommand.Connection.Open();
                try
                {
                    da.UpdateCommand.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
                da.UpdateCommand.Connection.Close();
            }
            for (int i = 1; i < rows.Count; i++)//удаляем все составы комплектов кроме одного
            {
                DeleteNodeByIDZ((int)rows[i]["IDZ"]);
            }
            bool first = false;
            foreach (string s in PackageList)
            {
                if (!first)//если первый проход, то сразу присваиваем состав комплекта
                {
                    da.UpdateCommand.Parameters["PACKAGE"].Value = s;
                    da.UpdateCommand.Parameters["IDZ"].Value = (int)rows[0]["IDZ"];
                    da.UpdateCommand.CommandText = "update PERIOD..PI set POLE = @PACKAGE where IDZ = @IDZ";
                    da.UpdateCommand.Connection.Open();
                    da.UpdateCommand.ExecuteNonQuery();
                    da.UpdateCommand.Connection.Close();
                    first = true;
                }
                else //если второй и более проход цикла, то сначала размножаем поле состав комплекта, т.к. все, кроме одного мы удалили
                {
                    int idz_n = AddPackageNode((int)rows[0]["IDZ"]);//размножим состав комплекта
                    da.UpdateCommand.Parameters["PACKAGE"].Value = s;
                    da.UpdateCommand.Parameters["IDZ"].Value = idz_n;
                    da.UpdateCommand.CommandText = "update PERIOD..PI set POLE = @PACKAGE where IDZ = @IDZ";
                    da.UpdateCommand.Connection.Open();
                    da.UpdateCommand.ExecuteNonQuery();
                    da.UpdateCommand.Connection.Close();
                }

            }

            
        }

        private DataRowCollection GetIDZofAllPackagesByPINandYEAR(string PIN, int YEAR)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("PIN", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["PIN"].Value = PIN;
            da.SelectCommand.Parameters.Add("YEAR", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["YEAR"].Value = YEAR.ToString();
            da.SelectCommand.CommandText = " select E.IDZ " +
                                           "  from PERIOD..PI A "+
                                           "  left join PERIOD..PI B on A.IDZ = B.VVERH "+
                                           "  left join PERIOD..PI C on B.IDZ = C.VVERH "+
                                           "  left join PERIOD..PI D on C.IDZ = D.VVERH "+
                                           "  left join PERIOD..PI E on C.IDZ = E.VVERH "+
                                           "  where A.IDF = 120 and A.POLE = @PIN and B.POLE = @YEAR " +
                                           " and D.IDF = 363 and D.POLE = 'e-book' and E.IDF = 240";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Не найден состав комплекта.");
            }
            return ds.Tables["t"].Rows;
        }

        private int AddPackageNode(int IDZ)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Direction = ParameterDirection.Input;
            da.SelectCommand.Parameters["IDZ"].Value = IDZ;
            da.SelectCommand.Parameters.Add("IDZ_OUT", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ_OUT"].Value = IDZ;
            da.SelectCommand.Parameters["IDZ_OUT"].Direction = ParameterDirection.Output;
            da.SelectCommand.CommandText = "PERIOD.dbo.[PI_ADD_VETVJ_VPRAVO]";
            da.SelectCommand.Connection.Open();
            da.SelectCommand.ExecuteNonQuery();
            da.SelectCommand.Connection.Close();
            return (int)da.SelectCommand.Parameters["IDZ_OUT"].Value;
        }

        private void DeleteNodeByIDZ(int IDZ)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZS", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZS"].Value = IDZ;
            da.SelectCommand.CommandText = "PERIOD.dbo.[PI_DEL_VETVJ]";
            da.SelectCommand.Connection.Open();
            da.SelectCommand.ExecuteNonQuery();
            da.SelectCommand.Connection.Close();
        }

        private DataRowCollection GetIDZofAllPackagesByHyperLinkIDZ(int IDZ)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.SelectCommand.Parameters["IDZ"].Value = IDZ;
            da.SelectCommand.CommandText = "select IDZ from PERIOD..[PI] where IDF = 240 and VVERH = (select VVERH from PERIOD..[PI] where IDZ = @IDZ)";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Не найден состав комплекта.");
            }
            return ds.Tables["t"].Rows;
        }

        public void BuildAndInsertHyperLink(string PIN,string year)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.UpdateCommand = new SqlCommand();
            da.UpdateCommand.Connection = new SqlConnection();
            da.UpdateCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.UpdateCommand.Parameters.Add("HL", SqlDbType.NVarChar);
            da.UpdateCommand.Parameters["HL"].Value = @"http://opac.libfl.ru/elcirp/viewer.aspx?pin=" + PIN+"&year="+year;
            da.UpdateCommand.Parameters.Add("IDZ", SqlDbType.Int);
            da.UpdateCommand.Parameters["IDZ"].Value = GetIDZofHYPERLINKbyPINandYEAR(PIN,year);

            da.UpdateCommand.CommandText = "update PERIOD..PI set POLE = @HL where IDZ = @IDZ";
            da.UpdateCommand.Connection.Open();
            da.UpdateCommand.ExecuteNonQuery();
            da.UpdateCommand.Connection.Close();
        }

        private int GetIDZofHYPERLINKbyPINandYEAR(string PIN, string year)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = XmlConnections.GetConnection("/Connections/BJVVV");
            da.SelectCommand.Parameters.Add("PIN", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["PIN"].Value = PIN;
            da.SelectCommand.Parameters.Add("YEAR", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["YEAR"].Value = year.ToString();
            da.SelectCommand.CommandText = " select E.IDZ " +
                                           "  from PERIOD..PI A " +
                                           "  left join PERIOD..PI B on A.IDZ = B.VVERH " +
                                           "  left join PERIOD..PI C on B.IDZ = C.VVERH " +
                                           "  left join PERIOD..PI D on C.IDZ = D.VVERH " +
                                           "  left join PERIOD..PI E on C.IDZ = E.VVERH " +
                                           "  where A.IDF = 120 and A.POLE = @PIN and B.POLE = @YEAR " +
                                           " and D.IDF = 363 and D.POLE = 'e-book' and E.IDF = 219";
            if (da.Fill(ds, "t") == 0)
            {
                throw new Exception("Не найдена гиперссылка.");
            }
            return (int)ds.Tables["t"].Rows[0][0];
        }
    }

}
