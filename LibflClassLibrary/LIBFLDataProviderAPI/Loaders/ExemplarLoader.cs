using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using DataProviderAPI.Queries;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Drawing;
using Newtonsoft.Json;
using DataProviderAPI.ValueObjects;
using Utilities;
using LibflClassLibrary.ExportToVufind;

namespace DataProviderAPI.Loaders
{
    /// <summary>
    /// Сводное описание для ExemplarLoader
    /// </summary>
    public class ExemplarLoader
    {
        public ExemplarLoader(string baseName)
        {
            this._baseName = baseName;
        }

        public string BaseName
        {
            get
            {
                return _baseName;
            }
        }
        private string _baseName;
        public ExemplarInfoAPI GetExemplarInfoByIdData(int IDDATA)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string connectionString = AppSettings.BookStatusConnection;
            string queryText = new BJExemplarQueries(this.BaseName).GET_EXEMPLAR_BY_IDDATA;
            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("IDDATA", SqlDbType.Int).Value = IDDATA;
            da.Fill(result);
            ExemplarInfoAPI ei = new ExemplarInfoAPI(IDDATA);
            string fieldCode;
            foreach (DataRow row in result.Rows)
            {
                fieldCode = row["MNFIELD"].ToString() + row["MSFIELD"].ToString();
                switch (fieldCode)//пока только автор заглавие
                {
                    case "899$a":
                        ei.Location = row["PLAIN"].ToString();
                        break;
                    case "899$p":
                        ei.InventoryNumber = row["PLAIN"].ToString();
                        break;
                    case "899$w":
                        ei.Barcode = row["PLAIN"].ToString();
                        break;
                }
            }
            queryText = new BJExemplarQueries(this.BaseName).GET_EXEMPLAR_ISSUE_INFO;
            da.SelectCommand.CommandText = queryText;
            result.Clear();
            int cnt = da.Fill(result);
            if (cnt == 0)
            {
                ei.IsIssued = false;
            }
            else
            {
                ei.IsIssued = true;
                ei.IDReaderTooked = Extensions.HashReaderId(result.Rows[0]["IDREADER"].ToString());
                ei.DateReturn = ((DateTime)result.Rows[0]["DATE_RET"]).ToString("dd.MM.yyyy");
            }

            return ei;
        }
        public ElectronicExemplarInfoAPI GetElectronicExemplarInfo(string id)
        //временно получаем так, пока не будут проинвентаризированы электронные копии
        {

            //string ip = ConfigurationManager.ConnectionStrings["IPAddressFileServer"].ConnectionString;
            string ip = AppSettings.IPAddressFileServer;
            //string login = ConfigurationManager.ConnectionStrings["LoginFileServer"].ConnectionString;
            string login = AppSettings.LoginFileServerRead;
            //string pwd = ConfigurationManager.ConnectionStrings["PasswordFileServer"].ConnectionString;
            string pwd = AppSettings.PasswordFileServerRead;
            string _directoryPath = @"\\" + ip + @"\BookAddInf\";

            ElectronicExemplarInfoAPI result = new ElectronicExemplarInfoAPI(-1);//пока что так мы создаем электронный экземпляр
            //когда появится инвентаризация электронных копий, то сюда надо вставить получение инфы об электронной копии
            FileInfo[] fi;
            using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
            {
                _directoryPath = @"\\" + ip + @"\BookAddInf\" + ElectronicExemplarInfoAPI.GetPathToElectronicCopy(id);
                
                DirectoryInfo di = new DirectoryInfo(_directoryPath);
                if (!di.Exists)
                {
                    return null;//каталога с картинками страниц не существует
                }
                DirectoryInfo hq = new DirectoryInfo(_directoryPath + @"\JPEG_HQ\");
                result.IsExistsHQ = (hq.Exists) ? true : false;
                result.Path_HQ = (hq.Exists) ? hq.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;

                DirectoryInfo lq = new DirectoryInfo(_directoryPath + @"\JPEG_LQ\");
                result.IsExistsLQ = (lq.Exists) ? true : false;
                result.Path_LQ = (lq.Exists)? lq.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;

                fi = hq.GetFiles("*.jpg").OrderBy(f => f.LastWriteTime).ToArray(); //сортируем по дате изменения. именно в таком порядке они сканировались. а вообще вопрос непростой, поскольку попадаются файлы, выпадающие из этого условия
                
                foreach (FileInfo f in fi)
                {
                    result.JPGFiles.Add(f.Name);
                }
                FileInfo coverPath = new FileInfo(_directoryPath + @"\JPEG_AB\cover.jpg");
                result.Path_Cover = (coverPath.Exists) ? coverPath.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/") : null;
                //di.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/");

            }
            //string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string connectionString = AppSettings.BookStatusConnection;
            string queryText = new BJExemplarQueries(this.BaseName).GET_ELECTRONIC_EXEMPLAR_BOOKADDINF;
            DataTable table = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = id.Substring(id.LastIndexOf("_") + 1);
            da.Fill(table);
            result.ForAllReader = (bool)table.Rows[0]["ForAllReader"];


            da.SelectCommand.CommandText = new BJExemplarQueries(this.BaseName).GET_ELECTRONIC_EXEMPLAR_STATUS;
            table = new DataTable();
            int cnt = da.Fill(table);
            result.Status = (cnt > 0) ? "unavailable" : "available";


            Image img = Image.FromFile(fi[0].FullName);
            result.WidthFirstFile = img.Width;
            result.HeightFirstFile = img.Height;
            result.IsElectronicCopy = true;

            queryText = new BJExemplarQueries(this.BaseName).GET_ELECTRONIC_EXEMPLAR_ISSUE_INFO;
            da.SelectCommand.CommandText = queryText;
            table.Clear();
            cnt = da.Fill(table);
            if (cnt == 0)
            {
                result.IsIssued = false;
            }
            else
            {
                result.IsIssued = true;
                result.IDReaderTooked = Extensions.HashReaderId(table.Rows[0]["IDREADER"].ToString());
                result.DateReturn = ((DateTime)table.Rows[0]["DATERETURN"]).ToString("dd.MM.yyyy");
            }


            return result;
            //return JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
        }
    }
}