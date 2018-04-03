using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Collections.Generic;

public partial class viewer : System.Web.UI.Page
{
    string IDZ;
    string PIN;
    string YEAR;
    string number;
    BookAddInf bai;
    protected void Page_Load(object sender, EventArgs e)
    
    {

        if (!Page.IsPostBack)
        {
           // Page_Load(sender, e);
        }
        

        //IDZ = Request["idz"];
        int idz;
        PIN = Request["pin"];
        YEAR = Request["YEAR"];
        //hlEBook.NavigateUrl = "~/default.aspx?idz=" + IDZ;
        if (!int.TryParse(PIN, out idz) || (PIN == "") || (PIN == null) || (PIN == "0") || !int.TryParse(YEAR, out idz) || (YEAR == "") || (YEAR == null) || (YEAR == "0"))
        {
            lError.Text = "Неверные входные данные!";
            Panel1.Visible = false;
            Panel2.Visible = true;
            return;
        }


        Label4.Text = "Заглавие: <b>\"" + GetTitle() + "\"</b>    &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp    Год: <b>" + GetYear() + "</b>";
            //SlideShowExtender1.ContextKey = bai.GetPath();
            //return;
        

        string sessionId = this.Session.SessionID;
        if (!Page.IsPostBack)
        {
            //bai.InsertELOPENEDPERIOD(sessionId);
        }
        if (number != null)
        {
            if (DropDownList1.SelectedItem == null)
            {
                DropDownList1.SelectedIndex = 0;
            }
            SlideShowExtender1.ContextKey = GetPath() + DropDownList1.SelectedItem.Text;

        }
        else
        {


            List<DirectoryInfo> di = new List<DirectoryInfo>();
            //ListItem li = new ListItem(
            //DropDownList1.Items.AddRange(di);
            string ip = XmlConnections.GetConnection("/Connections/img_ip");
            string _directoryPath = @"\\" + ip + @"\BookAddInf\" + GetPath();
            //_directoryPath = ValidPath(_directoryPath);
            DirectoryInfo path = new DirectoryInfo(_directoryPath);
            try
            {
                di = path.GetDirectories().ToList();
            }
            catch
            {
                try
                {
                    using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
                    {
                        di = path.GetDirectories().ToList();
                    }
                }

                catch (Exception ex)
                {
                    lError.Text = "Произошла ошибка. Попробуйте перезагрузить страницу!";// ex.Message;
                    return;
                }
            }
            if (di.Count == 0)
            {
                lError.Text = "Произошла ошибка. Попробуйте перезагрузить страницу!";
                return;
            }
            //DropDownList1.Items.Clear();
            if (DropDownList1.Items.Count == 0)
            {
                foreach (DirectoryInfo d in di)
                {
                    ListItem tmp = new ListItem(d.Name, d.FullName);
                    DropDownList1.Items.Add(tmp);
                }
            }
        }
        if (DropDownList1.SelectedItem == null)
        {
            DropDownList1.SelectedIndex = 0;
        }
        this.number = DropDownList1.SelectedItem.ToString();

        SlideShowExtender1.ContextKey = GetPath()+DropDownList1.SelectedItem.Text+"\\";
        

       
    }

    //private string GetTitle(int idz)
    //{
    //    SqlDataAdapter da;
    //    DataSet ds;
    //    da = new SqlDataAdapter();
    //    da.SelectCommand = new SqlCommand();
    //    da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
    //    ds = new DataSet();
    //    da.SelectCommand.CommandText = "select top 1 * from BookAddInf..ScanInfo_PER_UPLOAD where IDZ = " + idz + " order by ID desc";
    //    int i = da.Fill(ds, "t");
    //    if (i == 0)
    //    {
    //        throw new Exception("В базе нет записей по изданию.");
    //    }
    //    return ds.Tables["t"].Rows[0]["TITLE"].ToString();
    //}
    private string GetTitle()
    {
        SqlDataAdapter da;
        DataSet ds;
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        ds = new DataSet();
        da.SelectCommand.CommandText = " select B.POLE TITLE from PERIOD..PI A " +
                                       " left join PERIOD..PI B on B.VVERH = A.IDZ" +
                                       " where A.POLE = " + Request["pin"] + " and A.IDF=120 and B.IDF = 121 ";
        int i = da.Fill(ds, "t");
        if (i == 0)
        {
            throw new Exception("В базе нет записей по изданию.");
        }
        return ds.Tables["t"].Rows[0]["TITLE"].ToString();
    }
    //private object GetYear(int idz)
    //{
    //    SqlDataAdapter da;
    //    DataSet ds;
    //    da = new SqlDataAdapter();
    //    da.SelectCommand = new SqlCommand();
    //    da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
    //    ds = new DataSet();
    //    da.SelectCommand.CommandText = "select top 1 * from BookAddInf..ScanInfo_PER_UPLOAD where IDZ = " + idz + " order by ID desc";
    //    int i = da.Fill(ds, "t");
    //    if (i == 0)
    //    {
    //        throw new Exception("В базе нет записей по изданию.");
    //    }
    //    return ds.Tables["t"].Rows[0]["YEAR"].ToString();
    //}
    private object GetYear()
    {
        
        return Request["year"];
    }
    private string GetPath()
    {
        //string idz = this.IDZ.ToString();
        //return @"PERIOD\" + ValidPath(GetFolderByIDZ(idz)) + @"\";
        string path = PINFormat(Request["pin"]);
        path = @"PERIOD\" + path.Substring(0, 1) + @"\" + path.Substring(1, 3) + @"\" + path.Substring(4, 3) + @"\" + Request["year"] + @"\";

        return path;

    }
    private string PINFormat(string pin)
    {
        switch (pin.Length)
        {
            case 1:
                pin = "000000" + pin;
                break;
            case 2:
                pin = "00000" + pin;
                break;
            case 3:
                pin = "0000" + pin;
                break;
            case 4:
                pin = "000" + pin;
                break;
            case 5:
                pin = "00" + pin;
                break;
            case 6:
                pin = "0" + pin;
                break;
        }
        return pin;
    }
    private string GetFolderByIDZ(string idz)
    {
        SqlDataAdapter da;
        DataSet ds;
        da = new SqlDataAdapter();
        da.SelectCommand = new SqlCommand();
        da.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/BJVVV"));
        ds = new DataSet();
        da.SelectCommand.CommandText = "select top 1 * from BookAddInf..ScanInfo_PER_UPLOAD where IDZ = "+idz+" order by ID desc";
        int i = da.Fill(ds, "t");
        if (i == 0) 
        {
            throw new Exception("В базе нет записей по изданию.");
        }
        return ds.Tables["t"].Rows[0]["TITLE"].ToString() + "\\" + ds.Tables["t"].Rows[0]["YEAR"].ToString();
        
    }
    private string ValidPath(string p)
    {
        string ValidPath_Title = "";
        char[] invalid_chars = System.IO.Path.GetInvalidFileNameChars();
        
        foreach (char c in p)
        {
            if (invalid_chars.Contains(c) && (c.ToString() != @"\"))
            {
                continue;
            }
            else
            {
                ValidPath_Title += c;
            }
        }
        return ValidPath_Title;
    }










    /// <summary>
    /// Represents a network connection along with authentication to a network share.
    /// </summary>
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
