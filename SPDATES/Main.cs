using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Xml;
using System.IO;

namespace SPDATES
{

    public partial class Main : Form
    {
        SqlDataAdapter da;
        SqlConnection con;
        DataSet ds;
        public Main()
        {
            InitializeComponent();
            da = new SqlDataAdapter();
            con = new SqlConnection(XmlConnections.GetConnection("/Connections/sp"));
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = con;
            da.UpdateCommand = new SqlCommand();
            da.UpdateCommand.Connection = con;
            da.InsertCommand = new SqlCommand();
            da.InsertCommand.Connection = con;
            da.DeleteCommand = new SqlCommand();
            da.DeleteCommand.Connection = con;
            ds = new DataSet();
            con.Open();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!DateExist(dateTimePicker1.Value.Date))
            {
                da.InsertCommand.CommandText = "insert  into Reservation_R..SPECIALDATES (SPDATE) values ('" + dateTimePicker1.Value.ToString("yyyyMMdd") + "')";
                da.InsertCommand.ExecuteNonQuery();
                Main_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Такая дата уже есть!");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            da.SelectCommand.CommandText = "select * from Reservation_R..SPECIALDATES order by SPDATE";
            ds = new DataSet();
            da.Fill(ds, "sd");
            listBox1.DataSource = ds.Tables["sd"];
            listBox1.DisplayMember = "SPDATE";
            listBox1.ValueMember = "ID";


        }
        private bool DateExist(DateTime d)
        {
            da.SelectCommand.CommandText = "select * from Reservation_R..SPECIALDATES where SPDATE = '" + d.ToString("yyyyMMdd") + "'";
            ds = new DataSet();
            int i = da.Fill(ds);
            return (i == 0) ? false : true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count != 0)
            {
                int ind = listBox1.SelectedIndex;
                da.DeleteCommand.CommandText = "delete from Reservation_R..SPECIALDATES where ID = "+listBox1.SelectedValue.ToString();
                da.DeleteCommand.ExecuteNonQuery();
                Main_Load(sender, e);
                ind -= 1;
                if (ind >= 0)
                    listBox1.SelectedIndex = ind;
            }
            else
            {
                MessageBox.Show("Не выбрано ни одной даты!");
            }
        }
    }
    public class XmlConnections
    {
        public XmlConnections()
        {

        }
        private static String filename = Application.StartupPath + "\\DBConnections.xml";
        private static XmlDocument doc;
        public static string GetConnection(string s)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Файл с подключениями 'DBConnections.xml' не найден.");
            }

            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch
            {
                throw new Exception("Ошибка загрузки файла DBConnections.xml"); ;
                
            }
            XmlNode node;
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("Узел " + s + " не найден в файле DBConnections.xml"); ;
            }

            return node.InnerText;
        }
    }

}
