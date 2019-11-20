using LibflClassLibrary.ExportToVufind;
using LibflClassLibrary.ExportToVufind.BJ;
using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VufindConditionalBulkUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string fund = "BJVVV";
            BJVufindIndexUpdater bj = new BJVufindIndexUpdater(@"dev-catalog.libfl.ru", fund);

            List<string> unknownLocation = findUnknownLocationBookId(fund,99);
            //все эти надо пройти на проде завтра
            //100    99    26
            //удаляем из индекса все найденные функцией пины
            bj.DeleteFromIndex(unknownLocation);

            //далее перевыгружаем их в отдельный файл
            BJVuFindConverter converter = new BJVuFindConverter(fund);
            List<VufindDoc> docs = converter.Export(unknownLocation);

            //далее загружаем их обратно с исправленным местонахождением
            bj.AddToIndex(docs);


        }

        //ищет все пины по заданному неизвестному местонахождению
        private static List<string> findUnknownLocationBookId(string fund, int idOfUnknownLocation)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection connection = new SqlConnection(AppSettings.ConnectionString);
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = connection;
            DataTable table = new DataTable();
            using (connection)
            {
                da.SelectCommand.CommandText = "select distinct IDMAIN from "+fund+"..DATAEXT " +
                                               " where MNFIELD = 899 and MSFIELD = '$a' and IDINLIST = " +idOfUnknownLocation;
                da.Fill(table);
            }
            List<string> result = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                string id = $"BJVVV_{row[0].ToString()}";
                result.Add(id);
            }
            return result;

        }
    }
}
