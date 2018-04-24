using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExportBJ_XML.ValueObjects;
using DataProviderAPI.Loaders;
using ExportBJ_XML.classes;
using BookForOrder;
using InvOfBookForOrder;
using DataProviderAPI.ValueObjects;
using System.Data.SqlClient;
using System.Data;
public partial class OrderElCopy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string IDMAIN = Request["pin"];
        string IDBASE = Request["idbase"];

        string IDReader = Request["idreader"];
        if (IDReader == null)
        {
            IDReader = User.Identity.Name;
        }
        //string vkey = Request["vkey"];
        string BaseName = (IDBASE == "1")? "BJVVV" : "REDKOSTJ";

        LibflAPI.ServiceSoapClient api = new LibflAPI.ServiceSoapClient();
        LibflAPI.ReaderInfo reader = api.GetUser(IDReader);


        ExemplarLoader loader = new ExemplarLoader(BaseName);
        DataProviderAPI.ValueObjects.ElectronicExemplarInfo exemplar = loader.GetElectronicExemplarInfo(BaseName + "_" + IDMAIN);
        
        string HostName = HttpContext.Current.Server.MachineName;
        string ElBookViewerServer = "";
        if (HostName == "VGBIL-OPAC")
        {
            ElBookViewerServer = AppSettings.ExternalElectronicBookViewer;
        }
        else
        {
            ElBookViewerServer = AppSettings.IndoorElectronicBookViewer;
        }

        if (exemplar.ForAllReader)
        {
            string redirect = ElBookViewerServer + "?pin=" + IDMAIN + "&idbase=1&idr=" + IDReader;
            Response.Redirect(redirect);
        }
        else
        {

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(AppSettings.ConnectionString);
            cmd.CommandText = "select * from Reservation_R..ELISSUED where IDREADER = "+IDReader+" and IDMAIN = "+IDMAIN;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            int cnt = da.Fill(table);

            if (cnt == 0)
            {
                Book b = new Book(IDMAIN);
                InvOfBook inv = new InvOfBook();
                inv.inv = "Электронная копия";
                inv.IDMAIN = IDMAIN;
                b.Ord(inv, 30, DateTime.Now, int.Parse(IDReader), (reader.IsRemoteReader) ? 1 : 0);//заказали книгу автоматически
            }

            table = new DataTable();
            cnt = da.Fill(table);

            string ViewKey = table.Rows[0]["VIEWKEY"].ToString();
            string redirect = ElBookViewerServer + "?pin=" + IDMAIN + "&idbase=1&idr=" + IDReader + "&vkey=" + Server.UrlEncode(ViewKey);
            Response.Redirect(redirect);

            //добавим в заказы и на просмотровщик направим
            //string redirect = ElBookViewerServer + "?pin=" + row["idm"].ToString() + "&idbase=1&idr=" + row["idr"].ToString() + "&type=" + row["rtype"]
            //    + "&vkey=" + HttpUtility.UrlEncode(row["vkey"].ToString()) + "\" Target = \"_blank\">Просмотр</a>";
        }
        


    }
}
