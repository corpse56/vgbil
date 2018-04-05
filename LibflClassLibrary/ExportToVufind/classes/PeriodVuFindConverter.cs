using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace ExportBJ_XML.classes
{
    public class PeriodVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////PERIOD/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            string allFields = "";
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\period.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection();
            da.SelectCommand.Connection.ConnectionString = "Data Source=192.168.4.25,1443;Initial Catalog=PERIOD;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200";
            da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 120";
            DataSet ds = new DataSet();
            int cnt = da.Fill(ds, "t");
            DataTable val = new DataTable();
            foreach (DataRow row in ds.Tables["t"].Rows)
            {
                allFields = "";
                string pin = row["POLE"].ToString();
                if (pin == "")
                    continue;
                if ((Convert.ToInt32(row["POLE"]) < 2305) || (Convert.ToInt32(row["POLE"]) > 2308))
                {
                    //continue;
                }
                //title
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 121 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("title", val.Rows[0]["POLE"].ToString());
                    AddField("title_short", val.Rows[0]["POLE"].ToString());
                    AddField("title_sort", val.Rows[0]["POLE"].ToString());
                }
                //вид издания (журнал, газета и  т.п.)
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 124 and VVERH = " + row["IDZ"].ToString();
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_ModeOfPublication", val.Rows[0]["POLE"].ToString());//
                }
                //язык
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 128 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    //AddField("period_Language", val.Rows[0]["POLE"].ToString());//зачем отдельное поле, когда уже есть поле language. см. строкой ниже
                    AddField("language", val.Rows[0]["POLE"].ToString());
                }
                //периодичность
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 130 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    AddField("period_Periodicity", val.Rows[0]["POLE"].ToString());//
                }
                //года 
                da.SelectCommand.CommandText = "select * from PERIOD..PI where IDF = 131 and VVERH = " + row["IDZ"];
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    string result = "";
                    foreach (DataRow r in val.Rows)
                    {
                        result = result + r["POLE"].ToString() + "; ";

                    }
                    AddField("period_Years", result);//добавить в индекс
                }
                //гиперссылки 
                da.SelectCommand.CommandText = "select E.POLE " +
                                               "  from PERIOD..PI A " +
                                               "  left join PERIOD..PI B on A.IDZ = B.VVERH " +
                                               "  left join PERIOD..PI C on B.IDZ = C.VVERH " +
                                               "  left join PERIOD..PI D on C.IDZ = D.VVERH " +
                                               "  left join PERIOD..PI E on C.IDZ = E.VVERH " +
                                               "  where A.IDF = 120 and A.POLE = '" + row["POLE"] + "' " +
                                               " and D.IDF = 363  " +
                                               " and D.POLE = 'e-book' " +
                                               "  and E.IDF = 219";
                val = new DataTable();
                cnt = da.Fill(val);
                if (cnt != 0)
                {
                    string result = "";
                    foreach (DataRow r in val.Rows)
                    {
                        result = result + r["POLE"].ToString() + "; ";
                    }
                    AddField("HyperLink", result);
                }
                string PIN = row["POLE"].ToString();
                string IDZ = row["IDZ"].ToString();

                string RecordTree = GetRecordTree(PIN, IDZ);



                AddField("id", "period_" + row["POLE"]);
                AddField("fund", GetFundId("period"));
                AddField("allfields", allFields);
                AddField("period_RecordTree", RecordTree);
                allFields = "";
                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");

                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "period_" + row["POLE"];
                OnRecordExported(args);
            }
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

        private string GetRecordTree(string pin, string idz)
        {
            int IDZ = int.Parse(idz);
            SqlConnection Connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=PERIOD;Persist Security Info=True;User ID=sasha;Password=Corpse536;Connect Timeout=1200");
            PeriodTreeLoader loader = new PeriodTreeLoader(IDZ,Connection);
            loader.LoadChilds(IDZ, loader._rootNode);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            //формируем дерево в формате JSON для компонента jstree.com. Соблюдаем один из форматов представления дерева.
            writer.WriteStartObject();
            writer.WritePropertyName("core");
            writer.WriteStartObject();
            writer.WritePropertyName("data");
            writer.WriteStartArray();
            
            loader.WriteTreeJSON(writer, loader._rootNode);

            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();

            writer.Flush();
            writer.Close();
            AddField("PeriodRecordTree", sb.ToString());

            return sb.ToString();
        }

        public override void ExportSingleRecord(int idmain)
        {
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\singleRecords\" + this.Fund + "_" + idmain + ".xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////TEST/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            //string q = GetQuery(int.Parse(idmain));
            //DataTable table = ExecuteQuery(q);
            //int check = CreateBJDoc(table);
            //if (check == 1) return;
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }
        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }
    }
}
