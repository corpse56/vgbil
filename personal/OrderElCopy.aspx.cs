using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataProviderAPI.Loaders;
using BookForOrder;
using InvOfBookForOrder;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using DataProviderAPI.ValueObjects;
using System.Web.Security;
using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.ExportToVufind;

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
            //Response.Write("User.Identity.Name " + IDReader);
            if (IDReader == string.Empty)
            {
                Response.Write("Неизвестная ошибка");
                return;
            }
        }
        //else
        //{
        //    Response.Write("Request" + IDReader);
        //}
        //string vkey = Request["vkey"];
        string BaseName = (IDBASE == "1") ? "BJVVV" : "REDKOSTJ";


        ReaderInfo readerAPI = ReaderInfo.GetReader(int.Parse(IDReader));


        ExemplarLoader loader = new ExemplarLoader(BaseName);
        DataProviderAPI.ValueObjects.ElectronicExemplarInfoAPI exemplar = loader.GetElectronicExemplarInfo(BaseName + "_" + IDMAIN);



        if (exemplar.ForAllReader)//открытый БЕЗ авторского права
        {

            RedirectToNewViewer(IDMAIN, true, "", IDReader);

        }
        else    //ЗАКРЫТЫЕ АВТОРСКИМ ПРАВОМ
        {

            //Book b = new Book(IDMAIN);

            //if (this.IsFiveElBooksIssued(idr, rtype))
            //{
            //    return "Нельзя заказать больше 5 электронных книг! Сдайте какие-либо выданные Вам электронные копии на вкладке \"Электронные книги\" и повторите заказ! ";
            //}
            //if (this.IsELOrderedByCurrentReader(idr, rtype))
            //{
            //    return "Электронная копия этого документа уже выдана Вам!";
            //}
            //if (b.GetExemplarCount() - b.GetBusyExemplarCount() <= 0)
            //{
            //    return "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право." +
            //        " Ближайшая свободная дата " + b.GetNearestFreeDate().ToString("dd.MM.yyyy") + ". Попробуйте заказать в указанную дату.";

            //}
            //if (!this.IsDayPastAfterReturn(idr, rtype))
            //{
            //    return "Вы не можете заказать эту электронную копию, поскольку запрещено заказывать ту же копию, если не прошли сутки с момента её возврата. Попробуйте на следующий день.";
            //}
            //return "";







            BJBookInfo book = BJBookInfo.GetBookInfoByPIN(int.Parse(IDMAIN), BaseName);
            ReaderInfo reader = ReaderInfo.GetReader(int.Parse(IDReader));

            if (!book.IsElectronicCopyIssued())//если книга не выдана никому, то проверяем ограничения, потом неявно выдаём и перенаправляем на вьювер
            {
                if (CheckLimitations(book, reader))
                {
                    return;
                }
                book.IssueElectronicCopyToReader(reader.NumberReader);
                string ViewKey = book.GetElectronicViewKeyForReader(reader.NumberReader);
                RedirectToNewViewer(IDMAIN, false, ViewKey, IDReader);
            }
            else
            {
                if (!book.IsElectronicCopyIsuuedToReader(reader.NumberReader))//если этому читателю не выдана эта книга, то проверяем ограничения
                {
                    if (CheckLimitations(book, reader))
                    {
                        return;
                    }
                    //если ограничения не сработали, то выдаём и перенаправляем
                    book.IssueElectronicCopyToReader(reader.NumberReader);
                    string ViewKey = book.GetElectronicViewKeyForReader(reader.NumberReader);
                    RedirectToNewViewer(IDMAIN, false, ViewKey, IDReader);
                }
                else//если этому читателю выдана эта книга
                {
                    string ViewKey = book.GetElectronicViewKeyForReader(reader.NumberReader);
                    RedirectToNewViewer(IDMAIN, false, ViewKey, IDReader);
                }
            }
        }
    }

    private bool CheckLimitations(BJBookInfo book, ReaderInfo reader)
    {
        
        if (reader.IsFiveElBooksIssued())
        {
            Panel1.Visible = true;
            Label1.Text = "Нельзя взять более пяти электронных книг. Сдайте в личном кабинете любую электронную книгу и попробуйте снова.";
            HyperLink1.NavigateUrl = @"https://catalog.libfl.ru/Record/" + book.Fund + "_" + book.ID;
            return true;
        }
        if (book.Exemplars.Count - book.GetBusyElectronicExemplarCount() <= 0)
        {
            Panel1.Visible = true;
            Label1.Text = "Все экземпляры выданы. Нельзя выдать электронных экземпляров больше чем бумажных, так как это нарушит авторское право." +
                        " Ближайшая свободная дата " + book.GetNearestFreeDateForElectronicIssue().ToString("dd.MM.yyyy") + ". Попробуйте заказать в указанную дату.";
            HyperLink1.NavigateUrl = @"https://catalog.libfl.ru/Record/" + book.Fund + "_" + book.ID;
            return true;
        }
        if (!book.IsOneDayPastAfterReturn(reader.NumberReader))
        {
            Panel1.Visible = true;
            Label1.Text = "Вы не можете заказать эту электронную копию, поскольку запрещено заказывать ту же копию, если не прошли сутки с момента её возврата. Попробуйте на следующий день.";
            HyperLink1.NavigateUrl = @"https://catalog.libfl.ru/Record/" + book.Fund + "_" + book.ID;
            return true;
        }
        return false;
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
