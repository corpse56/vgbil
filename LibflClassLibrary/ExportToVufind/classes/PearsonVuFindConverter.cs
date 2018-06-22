using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Net;
using ExportBJ_XML.classes.BJ;
using LibflClassLibrary.ExportToVufind.classes.BJ;

namespace ExportBJ_XML.classes
{
    public class PearsonVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////Pearson////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            VufindXMLWriter vfWriter = new VufindXMLWriter("pearson");

            vfWriter.StartVufindXML(@"F:\import\" + Fund.ToLower() + ".xml");
            string Pearson = File.ReadAllText(@"f:/pearson_source.json");

            JArray desPearson = (JArray)JsonConvert.DeserializeObject(Pearson);

            string tmp = desPearson.First["licensePackage"].ToString();
            tmp = desPearson.First["catalog"]["options"]["Supported platforms"].ToString();
            int cnt = 1;
            VufindDoc vfDoc = new VufindDoc();
            StringBuilder AllFields = new StringBuilder();
            foreach (JToken token in desPearson)
            {
                vfDoc = new VufindDoc();
                vfDoc.title.Add(token["catalog"]["title"]["default"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.title);
                vfDoc.title_short.Add(token["catalog"]["title"]["default"].ToString());
                vfDoc.title_sort.Add(token["catalog"]["title"]["default"].ToString());
                vfDoc.author.Add(token["catalog"]["options"]["Authors"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.author);
                vfDoc.author_sort.Add(token["catalog"]["options"]["Authors"].ToString());

                vfDoc.Country.Add(token["catalog"]["options"]["Country of publication"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.Country);

                vfDoc.publisher.Add(token["catalog"]["options"]["Publisher"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.publisher);

                vfDoc.publishDate.Add(token["catalog"]["options"]["Publishing date"].ToString().Split('.')[2]);
                AllFields.AppendFormat(" {0}", vfDoc.publishDate);

                vfDoc.isbn.Add(token["catalog"]["options"]["ISBN"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.isbn);

                vfDoc.Volume.Add(token["catalog"]["options"]["Number of pages"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.Volume);

                vfDoc.Annotation.Add(token["catalog"]["options"]["Desk"].ToString() + " ; " +
                                              token["catalog"]["description"]["default"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.Annotation);

                vfDoc.genre.Add(token["catalog"]["options"]["Subject"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.genre);
                vfDoc.genre_facet.Add(token["catalog"]["options"]["Subject"].ToString());
                
                vfDoc.topic.Add(token["catalog"]["options"]["Catalogue section"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.topic);

                vfDoc.topic_facet.Add(token["catalog"]["options"]["Catalogue section"].ToString());
                
                vfDoc.collection.Add(token["catalog"]["options"]["Collection"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.collection);

                vfDoc.language.Add(token["catalog"]["options"]["Language"].ToString());
                AllFields.AppendFormat(" {0}", vfDoc.language);


                vfDoc.allfields = AllFields.ToString();

                //описание экземпляра Пирсон
                StringBuilder sb = new StringBuilder();
                StringWriter strwriter = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(strwriter);

                writer.WriteStartObject();
                writer.WritePropertyName("1");
                writer.WriteStartObject();

                writer.WritePropertyName("exemplar_carrier");
                //writer.WriteValue("Электронная книга");
                writer.WriteValue("3012");
                writer.WritePropertyName("exemplar_access");
                writer.WriteValue("1008");
                writer.WritePropertyName("exemplar_access_group");
                writer.WriteValue(KeyValueMapping.AccessCodeToGroup[1008]);

                //writer.WriteValue("Для прочтения онлайн необходимо перейти по ссылке");
                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue("https://ebooks.libfl.ru/product/" + token["id"].ToString());
                writer.WritePropertyName("exemplar_copyright");
                writer.WriteValue("Да");
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue("ebook");
                writer.WritePropertyName("exemplar_location");
                writer.WriteValue("2042");

                writer.WriteEndObject();
                writer.WriteEndObject();


                vfDoc.MethodOfAccess.Add("4002");
                vfDoc.Location.Add("2042");
                vfDoc.ExemplarsJSON = sb.ToString();
                vfDoc.id = "Pearson_" + token["id"].ToString();
                vfDoc.HyperLink.Add("https://ebooks.libfl.ru/product/" + token["id"].ToString());
                vfDoc.fund = "5008";
                vfDoc.Level = "Монография";
                vfDoc.format.Add("3012");

                vfWriter.AppendVufindDoc(vfDoc);
                

                //OnRecordExported
                cnt++;
                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "Pearson_" + token["id"].ToString();
                OnRecordExported(args);
            }
            vfWriter.FinishWriting();
        }

        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {

            string Pearson = File.ReadAllText(@"f:/pearson_source.json");

            JArray desPearson = (JArray)JsonConvert.DeserializeObject(Pearson);

            foreach (JToken token in desPearson)
            {
                Uri uri = new Uri("https://storage.aggregion.com/api/files/" + token["catalog"]["cover"].ToString() + "/shared/data");
                string str = uri.ToString();
                Utilities.Extensions.DownloadRemoteImageFile(uri.ToString(), @"f:\import\covers\pearson\" + token["id"].ToString()+@"\cover.jpg", @"f:\import\covers\pearson\" + token["id"].ToString());

                VuFindConverterEventArgs e = new VuFindConverterEventArgs();
                e.RecordId = "pearson_"+token["id"].ToString();
                OnRecordExported(e);

                GC.Collect();
            }



        }
        public class Item
        {
            public string ResourceId;
            
        }
        public void GetPearsonSourceData()
        {

            Uri apiUrl =
            new Uri("http://market.aggregion.com/api/public/goods?filter=licensePackage(\"59401a585e737d0a2cb05d4e\",equals)&extend=catalog");

            HttpWebRequest request = HttpWebRequest.Create(apiUrl) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;


            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string dwn = sr.ReadToEnd();

            //читать простым текстом по частям
            using (StreamWriter output = new StreamWriter(@"f:\pearson_source.json"))
            {
                using (StreamReader input = new StreamReader(response.GetResponseStream()))
                {
                    
                    //byte[] buffer = new byte[8192];
                    //int bytesRead;
                    while ( !input.EndOfStream )
                    {
                        output.WriteLine(input.ReadLine());
                    }
                }
            }

        }


        public override void ExportSingleCover(object idRecord)
        {
            throw new NotImplementedException();
        }
    }
}
