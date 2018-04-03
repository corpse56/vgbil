using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// Сводное описание для Slideshow
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из сценария с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
[System.Web.Script.Services.ScriptService]
public class Slideshow : System.Web.Services.WebService
{

    public Slideshow()
    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    
    public AjaxControlToolkit.Slide[] GetSlides(string contextKey)
    {
        
        DirectoryInfo di;
        //if (contextKey == "2")
        //    di = new DirectoryInfo("//80.250.173.142/scim/2/");
        // else
        //    di = new DirectoryInfo("//80.250.173.142/scim/");

        string ip = XmlConnections.GetConnection("/Connections/img_ip");
        string _directoryPath = @"\\"+ip+@"\BookAddInf\";
        try
        {
            _directoryPath = @"\\" + ip + @"\BookAddInf\" + contextKey;
            di = new DirectoryInfo(_directoryPath);
            FileInfo[] fi = di.GetFiles("*.jpg");
            string[] imagenames;


            imagenames = System.IO.Directory.GetFiles(@"\\" + ip + @"\BookAddInf\" + contextKey, "*.jpg");
            AjaxControlToolkit.Slide[] photos = new AjaxControlToolkit.Slide[imagenames.Length];
            for (int i = 0; i < imagenames.Length; i++)
            {
                string[] file = imagenames[i].Split('\\');
                string parametr = contextKey + file[file.Length - 1];
                byte[] par = MyEncrypt(parametr);
                parametr = HttpServerUtility.UrlTokenEncode(par);
                photos[i] = new AjaxControlToolkit.Slide(@"ImgHandler.ashx?img=" + parametr, "", "");

                //photos[i] = new AjaxControlToolkit.Slide(@"ImgHandler.ashx?img=" + contextKey + file[file.Length - 1], "", "");
            }
            return photos;
        }
        catch
        {
            using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
            {
                _directoryPath = @"\\" + ip + @"\BookAddInf\" + contextKey;
                di = new DirectoryInfo(_directoryPath);
                FileInfo[] fi = di.GetFiles("*.jpg");
                string[] imagenames;


                imagenames = System.IO.Directory.GetFiles(@"\\" + ip + @"\BookAddInf\" + contextKey, "*.jpg");
                AjaxControlToolkit.Slide[] photos = new AjaxControlToolkit.Slide[imagenames.Length];
                for (int i = 0; i < imagenames.Length; i++)
                {
                    string[] file = imagenames[i].Split('\\');

                    string parametr = contextKey + file[file.Length - 1];
                    byte[] par = MyEncrypt(parametr);
                    parametr = HttpServerUtility.UrlTokenEncode(par);
                    photos[i] = new AjaxControlToolkit.Slide(@"ImgHandler.ashx?img=" + parametr, "", "");
                    //photos[i] = new AjaxControlToolkit.Slide(@"ImgHandler.ashx?img=" + contextKey + file[file.Length - 1], "", "");
                }
                return photos;
            }
        }
    }
    public static byte[] MyEncrypt(string strToBeEncrypted)
    {
        //Plain Text to be encrypted
        byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(strToBeEncrypted);

        //In live, get the persistant passphrase from other isolated source
        //This example has hardcoded passphrase just for demo purpose
        StringBuilder sb = new StringBuilder();
        sb.Append("030320150303");

        //Generate the Salt, with any custom logic and
        //using the above string
        StringBuilder _sbSalt = new StringBuilder();
        for (int i = 0; i < 8; i++)
        {
            _sbSalt.Append("," + sb.Length.ToString());
        }
        byte[] Salt = Encoding.ASCII.GetBytes(_sbSalt.ToString());

        //Key generation:- default iterations is 1000 
        //and recomended is 10000
        Rfc2898DeriveBytes pwdGen = new Rfc2898DeriveBytes(sb.ToString(), Salt, 1000);

        //The default key size for RijndaelManaged is 256 bits, 
        //while the default blocksize is 128 bits.
        RijndaelManaged _RijndaelManaged = new RijndaelManaged();
        _RijndaelManaged.BlockSize = 256; //Increased it to 256 bits- max and more secure

        byte[] key = pwdGen.GetBytes(_RijndaelManaged.KeySize / 8);   //This will generate a 256 bits key
        byte[] iv = pwdGen.GetBytes(_RijndaelManaged.BlockSize / 8);  //This will generate a 256 bits IV

        //On a given instance of Rfc2898DeriveBytes class,
        //GetBytes() will always return unique byte array.
        _RijndaelManaged.Key = key;
        _RijndaelManaged.IV = iv;

        //Now encrypt
        byte[] cipherText2 = null;
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, _RijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(PlainText, 0, PlainText.Length);
            }
            cipherText2 = ms.ToArray();
        }
        //string cipherText2_str = Convert.ToBase64String(cipherText2);
        return cipherText2;
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

