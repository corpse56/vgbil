using LibflClassLibrary.ExportToVufind.Vufind;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

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
            string Imagette, Imag,  Titre, Titre_s, Auteur, Editeur, Collection, Annee;

            var td_elements = row.SelectNodes("td");
            
            Imag = td_elements[0].InnerHtml.ToString();
            
            //Imagette = td_elements[0].InnerText;

            Titre = td_elements[1].InnerText.Trim(' '); 
            Auteur = td_elements[2].InnerText;
            Editeur = td_elements[3].InnerText;
            Collection = td_elements[4].InnerText;
            Annee = td_elements[5].InnerText;


            //' ', '*', '.', ';' , ':', '-', '"'
            char[] arr = Titre.ToCharArray();
            arr = Array.FindAll<char>(arr, c => char.IsLetterOrDigit(c));
            Titre_s = new string(arr);

            VufindDoc result = new VufindDoc();
            result.id = "CERF_"+idbook.ToString();
            result.fund = this.Fund;
            result.allfields = Titre + " " + Auteur + " " + Annee + " " + Editeur;
            result.title.Add(Titre);
            result.title_short.Add(Titre);
            result.title_sort.Add(Titre_s); // артикли и ???
            result.author.Add(Auteur);
            result.author_sort.Add(Auteur.Replace("L' ", "").Replace("D' ", "").Replace(" d'", "").Replace(" l'", "").Replace(" ", ""));
            result.publishDate.Add(Annee);
            result.publisher.Add(Editeur);
            result.language.Add("Французский");
            if (Imag.IndexOf("img src=") > 0)
            {
                Imag = Imag.Substring(10).Remove(Imag.Length-12);
                result.HyperLink.Add(Imag);
            }

            //////////////////////////////////////////////////////////////////
            //описание экземпляра Litres
            StringBuilder sb = new StringBuilder();
            StringWriter strwriter = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(strwriter);

            string exTitre, barcode, description, price, doctype, library, section, place, status, messages, liability;
            int k = 1;
            HtmlDocument ExemplarsDocument = new HtmlDocument();
            ExemplarsDocument.Load(@"d:\VGBIL\WATERBEAR\waterbear (exemplaires).htm");
            var exemplars = ExemplarsDocument.DocumentNode.Descendants("tr");
            foreach (HtmlNode exemplar in exemplars)
            {
                var td_exemplar = exemplar.SelectNodes("td");
                exTitre = td_exemplar[1].InnerText.Trim(' ');
                if (exTitre.StartsWith(Titre) == true)
                {
                    barcode = td_exemplar[0].InnerText.Trim(' ');
                    description = td_exemplar[1].InnerText.Trim(' ');
                    price = td_exemplar[2].InnerHtml.ToString();

                    if (price.IndexOf("value=\"") > 0)
                    {
                        price = price.Substring(price.IndexOf("value=\"") + 7);
                        price = price.Remove(price.IndexOf("\">"));
                    }

                    doctype = td_exemplar[3].InnerHtml.ToString();
                    if (doctype.IndexOf("selected=\"selected\">") > 0)
                    {
                        doctype = doctype.Substring(doctype.IndexOf("selected=\"selected\">") + 20);
                        doctype = doctype.Remove(doctype.IndexOf("</option>"));
                    }
                    else doctype = "-";

                    library = td_exemplar[4].InnerHtml.ToString();
                    if (library.IndexOf("selected=\"selected\">") > 0)
                    {
                        library = library.Substring(library.IndexOf("selected=\"selected\">") + 20);
                        library = library.Remove(library.IndexOf("</option>"));
                    }
                    else library = "-";

                    section = td_exemplar[5].InnerHtml.ToString();
                    if (section.IndexOf("selected=\"selected\">") > 0)
                    {
                        section = section.Substring(section.IndexOf("selected=\"selected\">") + 20);
                        section = section.Remove(section.IndexOf("</option>"));
                    }
                    else section = "-";

                    place = td_exemplar[6].InnerHtml.ToString();
                    if (place.IndexOf("selected=\"selected\">") > 0)
                    {
                        place = place.Substring(place.IndexOf("selected=\"selected\">") + 20);
                        place = place.Remove(place.IndexOf("</option>"));
                    }
                    else place = "-";

                    status = td_exemplar[7].InnerHtml.ToString();
                    if (status.IndexOf("selected=\"selected\">") > 0)
                    {
                        status = status.Substring(status.IndexOf("selected=\"selected\">") + 20);
                        status = status.Remove(status.IndexOf("</option>"));
                    }
                    else status = "-";

                    messages = td_exemplar[8].InnerText.Trim(' ');
                    liability = td_exemplar[9].InnerText.Trim(' ');

                    //MessageBox.Show(barcode +"              "+ description + "              " + price + "            " + doctype +
                    //    "                     " + library + "                                  " +
                    //    section + "                   " + place + "                        " + status + "                  " + 
                    //    messages + "              " + liability);

                    writer.WriteStartObject();
                    writer.WritePropertyName(k.ToString()); //"1"
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
                    writer.WriteValue(barcode);//вообще это iddata, но тут любой можно,поскольку всегда свободно "1"
                    writer.WritePropertyName("exemplar_location");
                    writer.WriteValue("2046");

                    //Exemplar; exemplar_rack_location
                    //Exemplar; exemplar_placing_cipher
                    //Exemplar; exemplar_inventory_number

                    writer.WriteEndObject();
                    writer.WriteEndObject();
                    k++;
                }
            }



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
            //HtmlDocument ExemplarsDocument = new HtmlDocument();
            //ExemplarsDocument.Load(@"d:\VGBIL\WATERBEAR\waterbear (exemplaires).htm");
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