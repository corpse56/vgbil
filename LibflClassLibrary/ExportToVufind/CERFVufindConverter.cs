using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Text;
using Newtonsoft.Json;

namespace LibflClassLibrary.ExportToVufind
{
    public class CERFVufindConverter : VuFindConverter
    {
        private int idbook;
        public CERFVufindConverter(string Fund)
        {
            this.Fund = Fund;
        }

        public override VufindDoc CreateVufindDocument(object Record)
        {
            HtmlNode row = (HtmlNode)Record;
            string Imagette, Titre, Auteur, Editeur, Collection, Annee;

            var td_elements = row.SelectNodes("td");
            //var record = new Record();
            Imagette = td_elements[0].InnerText;
            Titre = td_elements[1].InnerText;
            Auteur = td_elements[2].InnerText;
            Editeur = td_elements[3].InnerText;
            Collection = td_elements[4].InnerText;
            Annee = td_elements[5].InnerText;



            VufindDoc result = new VufindDoc();
            result.id = "CERF_"+idbook.ToString();
            result.fund = this.Fund;
            result.allfields = "";
            result.title.Add(Titre);
            result.author.Add(Auteur);
            result.publishDate.Add(Annee);
            result.publisher.Add(Editeur);



            //описание экземпляра Litres
            StringBuilder sb = new StringBuilder();
            StringWriter strwriter = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(strwriter);

            writer.WriteStartObject();
            writer.WritePropertyName("1");
            writer.WriteStartObject();

            writer.WritePropertyName("exemplar_carrier");
            //writer.WriteValue("Электронная книга");
            writer.WriteValue("3001");
            writer.WritePropertyName("exemplar_access");
            //writer.WriteValue("Для прочтения онлайн необходимо перейти по ссылке");
            writer.WriteValue("1007");
            writer.WritePropertyName("exemplar_access_group");
            writer.WriteValue(KeyValueMapping.AccessCodeToGroup[1007]);

            writer.WritePropertyName("exemplar_id");
            writer.WriteValue("1");//вообще это iddata, но тут любой можно,поскольку всегда свободно
            writer.WritePropertyName("exemplar_location");
            writer.WriteValue("2046");



            //Exemplar; exemplar_rack_location
            //Exemplar; exemplar_placing_cipher
            //Exemplar; exemplar_inventory_number
            

            writer.WriteEndObject();
            writer.WriteEndObject();


            result.MethodOfAccess.Add("4002");
            result.Location.Add("2046");
            result.ExemplarsJSON = sb.ToString();
            result.fund = "5010";
            result.Level = "Монография";


            //result.ExemplarsJSON = "";
            return result;
        }

        public override void Export()
        {
            VufindXMLWriter vfWriter = new VufindXMLWriter("CERF");//Jewish Book House

            vfWriter.StartVufindXML(@"D:\VGBIL\" + Fund.ToLower() + ".xml");

           
            //string[] JHB = File.ReadAllLines(@"D:\HOME\ВГБИЛ\jbh_source_pasted.txt");

            HtmlDocument BooksDocument = new HtmlDocument();
            BooksDocument.Load(@"d:\VGBIL\WATERBEAR\waterbear.htm");
            HtmlDocument ExemplarsDocument = new HtmlDocument();
            ExemplarsDocument.Load(@"d:\VGBIL\WATERBEAR\waterbear (exemplaires).htm");
            //Linq

            var books = BooksDocument.DocumentNode.Descendants("tr");
            idbook = 0;
            foreach (HtmlNode book in books)
            {

               if (idbook > 0)
                {
                    VufindDoc doc = this.CreateVufindDocument(book);
                    vfWriter.AppendVufindDoc(doc);

                    //OnRecordExported
                    VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                    args.RecordId = doc.id;
                    OnRecordExported(args);
                }
                idbook++;
            }
            vfWriter.FinishWriting();

        }

        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }

        public override void ExportSingleCover(object idRecord)
        {
            throw new NotImplementedException();
        }

        public override void ExportSingleRecord(int idRecord)
        {
            throw new NotImplementedException();
        }
    }
}