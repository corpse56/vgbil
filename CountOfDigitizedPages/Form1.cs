using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;
using iTextSharp.text.pdf;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)//BJVVV jpg за период
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo from BookAddInf..ScanInfo A "+
                                           " left join BJVVV..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'"+
                                           " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = B.ID"+
                                           " where A.IDBase = 1 and cast(cast(A.DateEBook as varchar(11)) as datetime) between '20190401' and '20190630'";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\BJVVV\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 3) + @"\" + PIN.Substring(3, 3) + @"\" + PIN.Substring(6, 3) + @"\JPEG_HQ\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
                {
                    FileInfo[] fi = Target.GetFiles("*.jpg");
                    row["kolvo"] = fi.Length;
                }
            }
            dataGridView1.DataSource = DS.Tables["t"];

        }
        private void button3_Click(object sender, EventArgs e)//REDKOSTJ jpg за период
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo from BookAddInf..ScanInfo A " +
                                           " left join REDKOSTJ..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                                           " left join REDKOSTJ..DATAEXTPLAIN C on C.IDDATAEXT = B.ID" +
                                           " where A.IDBase = 2 and cast(cast(A.DateEBook as varchar(11)) as datetime) between '20190401' and '20190630'";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\REDKOSTJ\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 3) + @"\" + PIN.Substring(3, 3) + @"\" + PIN.Substring(6, 3) + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
                {
                    FileInfo[] fi = Target.GetFiles("*.jpg");
                    row["kolvo"] = fi.Length;
                }
            }
            dataGridView1.DataSource = DS.Tables["t"];
        }

//====================================================================================================================================================================================
        private void button8_Click(object sender, EventArgs e)//BJVVV PDF за период
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo from BookAddInf..ScanInfo A " +
                                           " left join BJVVV..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                                           " left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = B.ID" +
                                           " where A.IDBase = 1 and cast(cast(A.DatePDF as varchar(11)) as datetime) between '20170401' and '20170731' and PDF = 1";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\BJVVV\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\sasha", "Corpse536")))
                {
                    FileInfo[] fi = Target.GetFiles("*.pdf");
                    PdfReader reader = new PdfReader(fi[0].FullName);
                    row["kolvo"] = reader.NumberOfPages;
                }
            }
            dataGridView1.DataSource = DS.Tables["t"];
        }

        private void button9_Click(object sender, EventArgs e)//REDKOSTJ PDF за период
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo from BookAddInf..ScanInfo A " +
                                           " left join REDKOSTJ..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a'" +
                                           " left join REDKOSTJ..DATAEXTPLAIN C on C.IDDATAEXT = B.ID" +
                                           " where A.IDBase = 2 and cast(cast(A.DatePDF as varchar(11)) as datetime) between '20170401' and '20170731' and PDF = 1";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\REDKOSTJ\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\sasha", "Corpse536")))
                {
                    FileInfo[] fi = Target.GetFiles("*.pdf");
                    PdfReader reader = new PdfReader(fi[0].FullName);
                    row["kolvo"] = reader.NumberOfPages;
                }
            }
            dataGridView1.DataSource = DS.Tables["t"];
        }

        private string PINFormat(string pin)
        {
            switch (pin.Length)
            {
                case 1:
                    pin = "00000000" + pin;
                    break;
                case 2:
                    pin = "0000000" + pin;
                    break;
                case 3:
                    pin = "000000" + pin;
                    break;
                case 4:
                    pin = "00000" + pin;
                    break;
                case 5:
                    pin = "0000" + pin;
                    break;
                case 6:
                    pin = "000" + pin;
                    break;
                case 7:
                    pin = "00" + pin;
                    break;
                case 8:
                    pin = "0" + pin;
                    break;
            }
            return pin;
        }

        private void button2_Click(object sender, EventArgs e)//Larisa. То что оцифровали из бумаги
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.ID) num,D.POLE tit,B.POLE IDMAIN,A.ID kolvo,A.YEAR,len(A.PACKAGE)-len(replace(A.PACKAGE,';','')) nomera " +
                                           " from BookAddInf..DgtPeriodic A " +
                                           " left join PERIOD..PI B on B.POLE = A.PIN and B.IDF = 120" +
                                           " left join PERIOD..PI C on C.IDZ = B.VNIZ" +
                                           " left join PERIOD..PI D on D.IDZ = C.VPRAVO"+
                                           //" where A.CREATOR in (324,1)";
                                           " where cast(cast(A.DATEADD as varchar(11)) as datetime) between '20170401' and '20170731' and A.CREATOR in (324)";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\PERIOD\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\" + row["YEAR"].ToString() + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                DirectoryInfo[] Numbers = Target.GetDirectories();
                int kolvo = 0;
                foreach (DirectoryInfo di in Numbers)
                {
                    using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\sasha", "Corpse536")))
                    {
                        FileInfo[] fi = di.GetFiles("*.jpg");
                        kolvo += fi.Length;
                    }
                }
                row["kolvo"] = kolvo;
            }
            dataGridView1.DataSource = DS.Tables["t"];
        }

        private void button4_Click(object sender, EventArgs e)//BJVVV по тематике
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,DD.PLAIN tema, " +
                                           " C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo  " +
                                           " from BookAddInf..ScanInfo A  " +
                                           " left join BJVVV..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                           "  left join BJVVV..DATAEXTPLAIN C on C.IDDATAEXT = B.ID " +
                                           " left join BJVVV..DATAEXT D on A.IDBook = D.IDMAIN and D.MNFIELD = 922 and D.MSFIELD = '$e' " +
                                           "  left join BJVVV..DATAEXTPLAIN DD on DD.IDDATAEXT = D.ID " +
                                           " where A.IDBase = 1 and cast(cast(A.DateEBook as varchar(11)) as datetime) between '20160101' and '20160630' and C.PLAIN is not null";
            DA.Fill(DS, "t");


            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\BJVVV\";
                string outside_ip_connect = @"\\192.168.4.30\BookAddInf\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip_connect, new NetworkCredential("bj\\sasha", "Corpse536")))
                {
                    FileInfo[] fi = Target.GetFiles("*.jpg");
                    row["kolvo"] = fi.Length;
                    
                }
            }
            List<TemaKolvo> list = new List<TemaKolvo>();
            bool first = false;
            
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                int incr = 0;
                int index = -1;
                /*int i = 0;
                if (!int.TryParse(row["kolvo"].ToString(), out i))
                {
                    i=0;
                }*/

                //TemaKolvo tmp = new TemaKolvo(row["tema"].ToString(), i, 1);
                TemaKolvo tmp = new TemaKolvo(row["tema"].ToString(), (int)row["kolvo"], 1);
                foreach (TemaKolvo tk in list)
                {

                    if (tk.tema == row["tema"].ToString())
                    {
                        incr = tk.kolvo;
                        incr += (int)row["kolvo"];
                        index = list.IndexOf(tk);
                        break;
                    }
                }
                if (!first)
                {
                    list.Add(tmp);
                    first = true;
                    continue;
                }
                if (index != -1)
                {
                    list[index].kolvo = incr;
                    list[index].kolvopins++;
                }
                else
                {
                    list.Add(tmp);
                }
            }

            var bind = new BindingSource();
            bind.DataSource = list;

            dataGridView1.DataSource = bind;
        }
        private class TemaKolvo
        {
            public string tema {get;set;}
            public int kolvo { get; set; }
            public int kolvopins { get; set; }
            public TemaKolvo(string tema_, int kolvo_,int kolvopins_)
            {
                tema = tema_;
                kolvo = kolvo_;
                kolvopins = kolvopins_;
            }
        }

        private void button5_Click(object sender, EventArgs e)//Redkostj по тематике
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.IDBook) num,DD.PLAIN tema, " +
                                           " C.PLAIN tit,B.IDMAIN IDMAIN,C.ID kolvo  " +
                                           " from BookAddInf..ScanInfo A  " +
                                           " left join REDKOSTJ..DATAEXT B on A.IDBook = B.IDMAIN and B.MNFIELD = 200 and B.MSFIELD = '$a' " +
                                           "  left join REDKOSTJ..DATAEXTPLAIN C on C.IDDATAEXT = B.ID " +
                                           " left join REDKOSTJ..DATAEXT D on A.IDBook = D.IDMAIN and D.MNFIELD = 922 and D.MSFIELD = '$e' " +
                                           "  left join REDKOSTJ..DATAEXTPLAIN DD on DD.IDDATAEXT = D.ID " +
                                           " where A.IDBase = 2 and cast(cast(A.DateEBook as varchar(11)) as datetime) between '20160101' and '20160630' and C.PLAIN is not null";
            DA.Fill(DS, "t");


            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\REDKOSTJ\";
                string outside_ip_connect = @"\\192.168.4.30\BookAddInf\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                //using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\CopyPeriodAddInf", "Period_Copy")))
                using (new NetworkConnection(outside_ip_connect, new NetworkCredential("bj\\sasha", "Corpse536")))
                {
                    FileInfo[] fi = Target.GetFiles("*.jpg");
                    row["kolvo"] = fi.Length;
                }
            }
            List<TemaKolvo> list = new List<TemaKolvo>();
            bool first = false;
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                int incr = 0;
                int index = -1;
                TemaKolvo tmp = new TemaKolvo(row["tema"].ToString(), (int)row["kolvo"],1);
                foreach (TemaKolvo tk in list)
                {

                    if (tk.tema == row["tema"].ToString())
                    {
                        incr = tk.kolvo;
                        incr += (int)row["kolvo"];
                        index = list.IndexOf(tk);
                        break;
                    }
                }
                if (!first)
                {
                    list.Add(tmp);
                    first = true;
                    continue;
                }
                if (index != -1)
                {
                    list[index].kolvo = incr;
                    list[index].kolvopins++;                    
                }
                else
                {
                    list.Add(tmp);
                }
            }

            var bind = new BindingSource();
            bind.DataSource = list;

            dataGridView1.DataSource = bind;
        }

        private void button6_Click(object sender, EventArgs e)//То что оцифровали с бумаги и с микрофильмов
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.ID) num,D.POLE tit,B.POLE IDMAIN,A.ID kolvo,A.YEAR,len(A.PACKAGE)-len(replace(A.PACKAGE,';','')) nomera " +
                                           " from BookAddInf..DgtPeriodic A " +
                                           " left join PERIOD..PI B on B.POLE = A.PIN and B.IDF = 120" +
                                           " left join PERIOD..PI C on C.IDZ = B.VNIZ" +
                                           " left join PERIOD..PI D on D.IDZ = C.VPRAVO" +
                                           " where 1=1";
                                           //" where cast(cast(A.DATEADD as varchar(11)) as datetime) between '20150101' and '20151231'";
             
            /*DA.SelectCommand.CommandText = "select D.POLE tit,sum(len(A.PACKAGE)-len(replace(A.PACKAGE,';',''))) nomera " +
                                           " from BookAddInf..DgtPeriodic A " +
                                           " left join PERIOD..PI B on B.POLE = A.PIN and B.IDF = 120" +
                                           " left join PERIOD..PI C on C.IDZ = B.VNIZ" +
                                           " left join PERIOD..PI D on D.IDZ = C.VPRAVO" +
                                           " group by D.POLE " +
                                           " where 1=1 ";
                                           //" where cast(cast(A.DATEADD as varchar(11)) as datetime) between '20150101' and '20151231'";
             */
            
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\PERIOD\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\" + row["YEAR"].ToString() + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                DirectoryInfo[] Numbers = Target.GetDirectories();
                int kolvo = 0;
                foreach (DirectoryInfo di in Numbers)
                {
                    using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\sasha", "Corpse536")))
                    {
                        FileInfo[] fi = di.GetFiles("*.jpg");
                        kolvo += fi.Length;
                    }
                }
                row["kolvo"] = kolvo;
            }
            dataGridView1.DataSource = DS.Tables["t"];
        }

        private void button7_Click(object sender, EventArgs e)//Олег. То что оцифровали с микрофильмов
        {
            SqlDataAdapter DA;
            DataSet DS;
            DA = new SqlDataAdapter();
            DS = new DataSet();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/CirculationACC"));
            DA.SelectCommand.CommandText = "select row_number() over (order by A.ID) num,D.POLE tit,B.POLE IDMAIN,A.ID kolvo,A.YEAR,len(A.PACKAGE)-len(replace(A.PACKAGE,';','')) nomera " +
                                           " from BookAddInf..DgtPeriodic A " +
                                           " left join PERIOD..PI B on B.POLE = A.PIN and B.IDF = 120" +
                                           " left join PERIOD..PI C on C.IDZ = B.VNIZ" +
                                           " left join PERIOD..PI D on D.IDZ = C.VPRAVO" +
                                           //" where A.CREATOR in (468)";
                                           " where cast(cast(A.DATEADD as varchar(11)) as datetime) between '20150101' and '20151231' and A.CREATOR in (468)";
            DA.Fill(DS, "t");
            foreach (DataRow row in DS.Tables["t"].Rows)
            {
                if (row["IDMAIN"].ToString() == "")
                {
                    continue;
                }
                string outside_ip = @"\\192.168.4.30\BookAddInf\PERIOD\";
                string PIN = PINFormat(row["IDMAIN"].ToString());
                outside_ip += PIN.Substring(0, 1) + @"\" + PIN.Substring(1, 3) + @"\" + PIN.Substring(4, 3) + @"\" + row["YEAR"].ToString() + @"\";

                DirectoryInfo Target = new DirectoryInfo(outside_ip);
                DirectoryInfo[] Numbers = Target.GetDirectories();
                int kolvo = 0;
                foreach (DirectoryInfo di in Numbers)
                {
                    using (new NetworkConnection(outside_ip, new NetworkCredential("bj\\sasha", "Corpse536")))
                    {
                        FileInfo[] fi = di.GetFiles("*.jpg");
                        kolvo += fi.Length;
                    }
                }
                row["kolvo"] = kolvo;
            }
            dataGridView1.DataSource = DS.Tables["t"];
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
    public class XmlConnections
    {
        private static String filename = System.AppDomain.CurrentDomain.BaseDirectory + "DBConnections.xml";
        private static XmlDocument doc;
        public static string GetConnection(string s)
        {
            //filename = "F:\\projects\\circulationACC_svn\\trunk\\CirculationACC\\bin\\debug\\dbconnections.xml";
            if (!File.Exists(filename))
            {
                throw new Exception("Файл с подключениями 'DBConnections.xml' не найден." + filename);
            }

            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
            XmlNode node = doc.SelectSingleNode(s);
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("Узел " + s + " не найден в файле DBConnections.xml"); ;
            }
            while (node == null)
            {
                node = doc.SelectSingleNode(s);
            }
            return node.InnerText;
        }
        public XmlConnections()
        {

        }
    }
}
