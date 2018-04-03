using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.Data.EntityClient;

namespace DocList
{
    class dbConnect
    {
        private SqlDataAdapter sqlAdapter;
        private SqlConnection conn;
        private EntityConnection econn;

        /// <constructor>
        /// Initialise Connection
        /// </constructor>
        public dbConnect()
        {
            XmlConnections xcs;
            try
            {
                xcs = new XmlConnections();
            }
            catch
            {
                throw new Exception("Файл DBConnections.xml не найден!");
            }
            conn = new SqlConnection(xcs.GetBJVVVCon());
            econn = new EntityConnection(xcs.GetEntityCon());
            conn = this.openConnection();
            sqlAdapter = new SqlDataAdapter();

        }
        public string GetConnectionString()
        {
            return conn.ConnectionString;
        }
        public string GeteConnectionString()
        {
            return econn.ConnectionString;
        }
        private SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State ==
                        ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        public int ISBJVVV(string bar)
        {
            this.sqlAdapter.SelectCommand = new SqlCommand();
            this.sqlAdapter.SelectCommand.Connection = this.conn;
            this.sqlAdapter.SelectCommand.CommandText = "select Reservation_R.dbo.GetBaseByBar(@bar)";
            this.sqlAdapter.SelectCommand.Parameters.Add(new SqlParameter("bar", bar));
            object o = sqlAdapter.SelectCommand.ExecuteScalar();
            int i = Convert.ToInt32(o);
            return i;
        }
        

    }
    public class XmlConnections
    {
        private XmlTextReader reader;
        private String filename = Application.StartupPath + "\\DBConnections.xml";
        private XmlDocument doc;

        public XmlConnections()
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);// (reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        internal string GetBJVVVCon()
        {
            XmlNode node;
            try
            {
                node = this.doc.SelectSingleNode("/Connections/BJVVV");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            return node.InnerText;
        }
        internal string GetEntityCon()
        {
            XmlNode node;
            try
            {
                node = this.doc.SelectSingleNode("/Connections/Entity");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
            return node.InnerText;
        }


    }
}
