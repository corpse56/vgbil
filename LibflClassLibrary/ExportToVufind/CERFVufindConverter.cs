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
        public CERFVufindConverter(string Fund)
        {
            this.Fund = Fund;
        }

        public override VufindDoc CreateVufindDocument(object Record)
        {
            HtmlNode row = (HtmlNode)Record;

            VufindDoc result = new VufindDoc();
            result.id = "1";
            result.fund = this.Fund;
            result.allfields = "";
            result.title.Add("");
            result.author.Add("1");
            result.author.Add("2");



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

            vfWriter.StartVufindXML(@"D:\HOME\ВГБИЛ\" + Fund.ToLower() + ".xml");

            //string[] JHB = File.ReadAllLines(@"D:\HOME\ВГБИЛ\jbh_source_pasted.txt");


            HtmlDocument BooksDocument = new HtmlDocument();
            BooksDocument.Load(@"d:\HOME\ВГБИЛ\WATERBEAR\waterbear.htm");
            HtmlDocument ExemplarsDocument = new HtmlDocument();
            ExemplarsDocument.Load(@"d:\HOME\ВГБИЛ\WATERBEAR\waterbear (exemplaires).htm");
            //Linq

            var books = BooksDocument.DocumentNode.Descendants("tr");
            
            foreach (HtmlNode book in books)
            {
                VufindDoc doc = this.CreateVufindDocument(book);
                vfWriter.AppendVufindDoc(doc);


                //OnRecordExported
                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = doc.id;
                OnRecordExported(args);
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