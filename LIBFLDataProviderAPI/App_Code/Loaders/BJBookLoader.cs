using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DataProviderAPI.Queries;
using System.Configuration;
using DataProviderAPI.ValueObjects;


namespace DataProviderAPI.Loaders
{
    /// <summary>
    /// Сводное описание для BookLoader
    /// </summary>
    public class BJBookLoader
    {
        public BJBookLoader(string baseName)
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

        public BookInfo GetBJBookByID(int iDMAIN)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string queryText = new BJBookQueries(this.BaseName).GET_BOOK_BY_IDMAIN;
            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = iDMAIN;
            da.Fill(result);
            BookInfo bi = new BookInfo();
            string fieldCode;
            List<int> ExemplarIdDataList = new List<int>();
            int currentIDDATA = (int)result.Rows[0]["IDDATA"];
            foreach (DataRow row in result.Rows)
            {
                fieldCode = row["MNFIELD"].ToString() + row["MSFIELD"].ToString();
                if (((int)row["IDBLOCK"] == 260) && (currentIDDATA != (int)row["IDDATA"]))
                {
                    ExemplarIdDataList.Add((int)row["IDDATA"]);//собираем IDDATA всех экземпляров
                }
                switch (fieldCode)//пока только автор заглавие
                {
                    case "200$a":
                        bi.Title = row["PLAIN"].ToString();
                        break;
                    case "700$a":
                    case "701$a":
                        bi.Author.Add(row["PLAIN"].ToString());
                        break;
                    case "330$a":
                        bi.Annotation.Add(row["PLAIN"].ToString());
                        break;
                }
                currentIDDATA = (int)row["IDDATA"];
            }
            ExemplarLoader loader = new ExemplarLoader(this._baseName);
            foreach (int iddata in ExemplarIdDataList)
            {
                ExemplarInfo ei = loader.GetExemplarInfoByIdData(iddata);
                bi.Exemplars.Add(ei);
            }
            //проверим есть ли электронный экземпляр. если есть, то добавим его. пока только так можно определить электронный экземпляр. когда они проинвентаризируются, будет создаваться сам в предыдущем цикле
            queryText = new BJBookQueries(this.BaseName).IS_HYPERLINK_EXISTS;
            result = new DataTable();
            da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("IDMAIN", SqlDbType.Int).Value = iDMAIN;
            int IsHyperlinkExists = da.Fill(result);
            if (IsHyperlinkExists > 0)
            {
                ExemplarInfo ei = new ExemplarInfo(-1);//пока для всех электронных будет -1
                ei = loader.GetElectronicExemplarInfo(this._baseName + "_" + iDMAIN.ToString());
                bi.Exemplars.Add(ei);
            }
            return bi;
        }
        public BookInfo GetBJBookByINV(string invNumber)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BookStatusConnection"].ConnectionString;
            string queryText = new BJBookQueries(this.BaseName).GET_IDMAIN_BY_INVNUMBER;
            DataTable result = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(queryText, connectionString);
            da.SelectCommand.Parameters.Add("InvNumber", SqlDbType.NVarChar).Value = invNumber;
            da.Fill(result);
            if (result.Rows.Count == 0) return null;
            int IDMAIN = (int)result.Rows[0]["IDMAIN"];
            return this.GetBJBookByID(IDMAIN);
        }

    }
}