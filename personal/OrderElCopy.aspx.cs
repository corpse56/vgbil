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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
public partial class OrderElCopy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string IDMAIN = Request["pin"];
        string IDBASE = Request["idbase"];

        string IDReader = Request.QueryString["idreader"];

        if (IDReader == null)
        {
            IDReader = User.Identity.Name;
            Response.Write("User.Identity.Name" + IDReader);
        }
        else
        {
            Response.Write("Request" + IDReader);
        }
        //string vkey = Request["vkey"];
        string BaseName = (IDBASE == "1")? "BJVVV" : "REDKOSTJ";

        LibflAPI.ServiceSoapClient api = new LibflAPI.ServiceSoapClient();
        LibflAPI.ReaderInfo reader;
        try
        {
            reader = api.GetUser(IDReader);
        }
        catch
        {
            Response.Write(IDReader);
            return;
        }


        ExemplarLoader loader = new ExemplarLoader(BaseName);
        DataProviderAPI.ValueObjects.ElectronicExemplarInfoAPI exemplar = loader.GetElectronicExemplarInfo(BaseName + "_" + IDMAIN);
        


        if (exemplar.ForAllReader)//открытый БЕЗ авторского права
        {
            
            RedirectToNewViewer(IDMAIN, true,"", IDReader);

        }
        else    //ЗАКРЫТЫЕ АВТОРСКИМ ПРАВОМ
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
            RedirectToNewViewer(IDMAIN, false, ViewKey, IDReader);

        }
            //добавим в заказы и на просмотровщик направим
            //string redirect = ElBookViewerServer + "?pin=" + row["idm"].ToString() + "&idbase=1&idr=" + row["idr"].ToString() + "&type=" + row["rtype"]
            //    + "&vkey=" + HttpUtility.UrlEncode(row["vkey"].ToString()) + "\" Target = \"_blank\">Просмотр</a>";
    }

    private void RedirectToNewViewer(string IDMAIN, bool ForAllReader, string vkey, string IDReader)
    {
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
        string redirect;
        if (ForAllReader)
        {
            redirect = ElBookViewerServer + "?pin=" + IDMAIN + "&idbase=1&idr=" + IDReader;
        }
        else
        {
            redirect = ElBookViewerServer + "?pin=" + IDMAIN + "&idbase=1&idr=" + IDReader + "&vkey=" + Server.UrlEncode(vkey);
        }

        if (HttpContext.Current.Server.MachineName == "VGBIL-OPAC")
        {
            bool IsExistsLQ = GetIsExistsLQ(IDMAIN);
            if (IsExistsLQ)
            {
                Response.Redirect(@"http://catalog.libfl.ru/Bookreader/Viewer?bookID=BJVVV_" + IDMAIN + "&view_mode=LQ");
            }
            else
            {
                Response.Redirect(@"http://catalog.libfl.ru/Bookreader/Viewer?bookID=BJVVV_" + IDMAIN + "&view_mode=HQ");
            }
        }
        else
        {
            Response.Redirect(redirect);
        }
    }
    private bool GetIsExistsLQ(string IDMAIN)
    {
        LibflAPI.ServiceSoapClient api = new LibflAPI.ServiceSoapClient();
        string book = api.GetBookInfoByID("BJVVV_" + IDMAIN);
        JObject jbook = JObject.Parse(book);
        JArray Exemplars = (JArray)jbook["Exemplars"];
        bool IsExistsLQ = false;
        foreach (JToken exm in Exemplars)
        {
            if (exm["IsElectronicCopy"].ToString().ToLower() == "false")
            {
                continue;
            }
            if (exm["IsExistsLQ"].ToString().ToLower() == "true")
            {
                IsExistsLQ = true;
            }
        }
        return IsExistsLQ;
    }
}
