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
        public ExemplarInfo GetExemplarInfoByIdData(int IDDATA)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string queryText = new BJExemplarQueries(this.BaseName).GET_EXEMPLAR_BY_IDDATA;
            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("IDDATA", SqlDbType.Int).Value = IDDATA;
            da.Fill(result);
            ExemplarInfo ei = new ExemplarInfo(IDDATA);
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
            return ei;
        }
        public ElectronicExemplarInfo GetElectronicExemplarInfo(string id)
        //временно получаем так, пока не будут проинвентаризированы электронные копии
        {
            string ip = ConfigurationManager.ConnectionStrings["IPAddressFileServer"].ConnectionString;
            string login = ConfigurationManager.ConnectionStrings["LoginFileServer"].ConnectionString;
            string pwd = ConfigurationManager.ConnectionStrings["PasswordFileServer"].ConnectionString;
            string _directoryPath = @"\\" + ip + @"\BookAddInf\";

            ElectronicExemplarInfo result = new ElectronicExemplarInfo(-1);//пока что так мы создаем электронный экземпляр
            //когда появится инвентаризация электронных копий, то сюда надо вставить получение инфы об электронной копии
            FileInfo[] fi;
            using (new NetworkConnection(_directoryPath, new NetworkCredential("BJStor01\\imgview", "Image_123Viewer")))
            {
                _directoryPath = @"\\" + ip + @"\BookAddInf\" + ElectronicExemplarInfo.GetPathToElectronicCopy(id);
                
                DirectoryInfo di = new DirectoryInfo(_directoryPath);
                if (!di.Exists)
                {
                    return null;//каталога с картинками страниц не существует
                }
                DirectoryInfo hq = new DirectoryInfo(_directoryPath + @"\JPEG_HQ");
                result.IsExistsHQ = (hq.Exists) ? true : false;

                DirectoryInfo lq = new DirectoryInfo(_directoryPath + @"\JPEG_LQ");
                result.IsExistsLQ = (lq.Exists) ? true : false;
                
                
                fi = di.GetFiles("*.jpg").OrderBy(f => f.LastWriteTime).ToArray(); //сортируем по дате изменения. именно в таком порядке они сканировались. а вообще вопрос непростой, поскольку попадаются файлы, выпадающие из этого условия
                
                foreach (FileInfo f in fi)
                {
                    result.JPGFiles.Add(f.Name);
                }
                result.Path = di.FullName.Substring(di.FullName.IndexOf("BookAddInf") + 11).Replace(@"\", @"/");

            }
            Image img = Image.FromFile(fi[0].FullName);
            result.WidthFirstFile = img.Width;
            result.HeightFirstFile = img.Height;
            result.IsElectronicCopy = true;
            return result;
            //return JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
        }
    }
}