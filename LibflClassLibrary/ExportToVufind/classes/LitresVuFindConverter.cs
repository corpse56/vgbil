using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using ExportBJ_XML.classes.BJ;

namespace ExportBJ_XML.classes
{
    public class LitresVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////*/
            //////////////////////////////////LITRES/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\litres.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);
            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");
            _doc = _exportDocument.CreateElement("doc");



            XDocument xdoc = XDocument.Load(@"f:\litres_source.xml");


            var removedBooks = xdoc.Descendants("removed-book");
            List<string> removedBookIDs = new List<string>();
            foreach (XElement elt in removedBooks)
            {
                removedBookIDs.Add(elt.Attribute("id").Value);
            }



            var books = xdoc.Descendants("updated-book");
            int cnt = 1;
            string current = "";
            StringBuilder work = new StringBuilder();
            foreach (XElement elt in books)
            {
                current = elt.Attribute("id").Value;
                if (removedBookIDs.Contains(current))
                {
                    continue;
                }
                if (elt.Element("title-info") != null)
                {
                    if (elt.Element("title-info").Element("book-title") != null)
                    {
                        AddField("title", elt.Element("title-info").Element("book-title").Value);
                        AddField("title_short", elt.Element("title-info").Element("book-title").Value);
                        AddField("title_sort", elt.Element("title-info").Element("book-title").Value);
                    }
                }
                else
                {
                    AddField("title", "Заглавие не найдено");
                    AddField("title_short", "Заглавие не найдено");
                }

                //if (elt.Element("document-info").Element("src-url") != null)
                //{
                //    AddField("HyperLink", elt.Element("document-info").Element("src-url").Value);
                //}
                work.Length = 0;
                work.Append(@"http://al.litres.ru/").Append(elt.Attribute("id").Value);
                string hypLink = work.ToString();
                AddField("HyperLink", hypLink);
                work.Length = 0;
                if (elt.Element("title-info") != null)
                {
                    if (elt.Element("title-info").Element("author") != null)
                    {
                        if (elt.Element("title-info").Element("author").Element("last-name") != null)
                        {
                            work.Append(elt.Element("title-info").Element("author").Element("last-name").Value).Append(" ");
                        }
                        if (elt.Element("title-info").Element("author").Element("first-name") != null)
                        {
                            work.Append(elt.Element("title-info").Element("author").Element("first-name").Value).Append(" ");
                        }
                        if (elt.Element("title-info").Element("author").Element("middle-name") != null)
                        {
                            work.Append(elt.Element("title-info").Element("author").Element("middle-name").Value);
                        }
                    }
                }
                AddField("author", work.ToString());
                if (work.ToString() != string.Empty)
                {
                    if (work.ToString()[0] == '(')
                    {
                        AddField("author_sort", work.ToString());
                    }
                    else
                    {
                        AddField("author_sort", work.ToString().Substring(1));
                    }
                }

                


                work.Length = 0;

                if (elt.Element("title-info") != null)
                {
                    if (elt.Element("title-info").Element("annotation") != null)
                    {
                        work.Append(elt.Element("title-info").Element("annotation").Value);
                    }
                }
                AddField("Annotation", work.ToString());
                work.Length = 0;

                if (elt.Element("publish-info") != null)
                {
                    if (elt.Element("publish-info").Element("year") != null)
                    {
                        work.Append(elt.Element("publish-info").Element("year").Value);
                    }
                }
                AddField("publishDate", work.ToString());
                work.Length = 0;

                if (elt.Element("publish-info") != null)
                {
                    if (elt.Element("publish-info").Element("city") != null)
                    {
                        work.Append(elt.Element("publish-info").Element("city").Value);
                    }
                }
                AddField("PlaceOfPublication", work.ToString());
                work.Length = 0;

                if (elt.Element("publish-info") != null)
                {
                    if (elt.Element("publish-info").Element("publisher") != null)
                    {
                        work.Append(elt.Element("publish-info").Element("publisher").Value);
                    }
                }
                AddField("publisher", work.ToString());
                work.Length = 0;

                if (elt.Element("publish-info") != null)
                {
                    if (elt.Element("publish-info").Element("isbn") != null)
                    {
                        work.Append(elt.Element("publish-info").Element("isbn").Value);
                    }
                }
                AddField("isbn", work.ToString());
                work.Length = 0;

                if (elt.Element("title-info") != null)
                {
                    if (elt.Element("title-info").Element("lang") != null)
                    {
                        work.Append(GetLitresLanguageRus(elt.Element("title-info").Element("lang").Value));
                    }
                }
                AddField("language", work.ToString());
                work.Length = 0;
                if (elt.Element("genres") != null)
                {
                    if (elt.Element("genres").Element("genre") != null)
                    {
                        work.Append(elt.Element("genres").Element("genre").Attribute("title").Value);
                    }
                }
                AddField("genre", work.ToString());
                AddField("genre_facet", work.ToString());
                work.Length = 0;

                //описание экземпляра Litres
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
                //writer.WriteValue("Для прочтения онлайн необходимо перейти по ссылке");
                writer.WriteValue("1004");
                writer.WritePropertyName("exemplar_access_group");
                writer.WriteValue(KeyValueMapping.AccessCodeToGroup[1004]);

                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue(hypLink);

                writer.WritePropertyName("exemplar_copyright");
                writer.WriteValue("Да");
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue("ebook");//вообще это iddata, но тут любой можно,поскольку всегда свободно
                writer.WritePropertyName("exemplar_location");
                writer.WriteValue("Интернет");

                writer.WriteEndObject();
                writer.WriteEndObject();


                AddField("MethodOfAccess", "4002");
                AddField("Location", "2040");
                AddField("Exemplar", sb.ToString());
                AddField("id", "Litres_" + elt.Attribute("id").Value);
                AddField("fund", "5007");
                AddField("Level", "Монография");
                AddField("format", "3012");


                _doc.WriteTo(_objXmlWriter);
                _doc = _exportDocument.CreateElement("doc");
                //OnRecordExported
                cnt++;
                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "Litres_" + elt.Attribute("id").Value;
                OnRecordExported(args);
            }

            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }


        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {
            //XDocument xdoc = XDocument.Load(@"f:\api_litres.xml");
            XDocument xdoc = XDocument.Load(@"f:\litres_source.xml");


            var allBook = xdoc.Descendants("updated-book");
            foreach (XElement elt in allBook)
            {
                //Uri url = new Uri("http://partnersdnld.litres.ru/static/bookimages/00/01/23/00012345.bin.dir/00012345.cover.jpg");
                //Uri url = new Uri("http://partnersdnld.litres.ru/static/bookimages/21/10/35/21103582.bin.dir/21103582.cover.jpg");

                string id = "";
                if (elt.Attribute("file_id") != null)
                {
                    id = elt.Attribute("file_id").Value;
                }
                else
                {
                    continue;
                }
                switch (id.Length)
                {
                    case 0:
                        id = "00000000";
                        break;
                    case 1:
                        id = "0000000"+id;
                        break;
                    case 2:
                        id = "000000"+id;
                        break;
                    case 3:
                        id = "00000"+id;
                        break;
                    case 4:
                        id = "0000"+id;
                        break;
                    case 5:
                        id = "000"+id;
                        break;
                    case 6:
                        id = "00"+id;
                        break;
                    case 7:
                        id = "0"+id;
                        break;
                }

                StringBuilder sb = new StringBuilder("http://partnersdnld.litres.ru/static/bookimages/");
                string coverType = (elt.Attribute("cover") == null) ? "jpg" : elt.Attribute("cover").Value;
                sb.Append(id[0]).Append(id[1]).Append("/").Append(id[2]).Append(id[3]).Append("/").Append(id[4]).Append(id[5]).Append("/").Append(id).Append(".bin.dir/").Append(id).Append(".cover.").Append(coverType);
                string coverUrl = sb.ToString();

                string path = @"f:\import\covers\litres\" + elt.Attribute("id").Value +@"\";

                StringBuilder fileName = new StringBuilder();
                fileName.Append(path).Append("cover.").Append(coverType);
                

                Extensions.DownloadRemoteImageFile(coverUrl, fileName.ToString(), path);

                VuFindConverterEventArgs e = new VuFindConverterEventArgs();
                e.RecordId = "litres_" + elt.Attribute("id").Value;
                OnRecordExported(e);
                GC.Collect();
            }
        }

        public void GetLitresSourceData()
        {
            //внимание! Timestamp нужен от текущего времени, а не от чекпоинта! SHA генерируется тоже от текущего времени, а не от чекпоинта
            string stamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString();
            string key = "geqop45m))AZvb23zerhgjj76cvc##PFbbfqorptqskj";
            DateTime checkpointDate = new DateTime(2013, 1, 1, 0, 0, 0);
            string checkpoint = checkpointDate.ToString("yyyy-MM-dd HH:mm:ss");//"2017-01-01 00:00:00";
            string endpoint = checkpointDate.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");

            string inputString = stamp + ":" + key + ":" + checkpoint;
            string sha256 = Extensions.sha256(inputString);


            Uri apiUrl =
            new Uri("http://partnersdnld.litres.ru/get_fresh_book/?checkpoint=" + checkpoint +
                                                                 "&place=GTCTL" +
                                                                 "&timestamp=" + stamp +
                                                                 "&sha=" + sha256);
            //"&enpoint="+endpoint);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpWebRequest request = HttpWebRequest.Create(apiUrl) as HttpWebRequest;
            request.Timeout = 120000000;
            request.KeepAlive = true;
            request.ProtocolVersion = HttpVersion.Version10;
            request.ServicePoint.ConnectionLimit = 24;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            XDocument xdoc = XDocument.Load(new StreamReader(response.GetResponseStream()));

            //читать простым текстом по частям
            //using (Stream output = File.OpenWrite("litres.dat"))
            //using (Stream input = response.GetResponseStream())
            //{
            //    byte[] buffer = new byte[8192];
            //    int bytesRead;
            //    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        output.Write(buffer, 0, bytesRead);
            //    }
            //}

            XmlWriter writ = XmlTextWriter.Create(@"F:\litres_source.xml");
            xdoc.WriteTo(writ);
            writ.Flush();
            writ.Close();
        }
        private string GetLitresLanguageRus(string lng)
        {
            switch (lng)
            {
                case "uk":
                    return "Украинский";
                case "ru":
                    return "Русский";
                case "en":
                    return "Английский";
                case "de":
                    return "Немецкий";
                case "fr":
                    return "Французский";
                case "ab":
                    return "Абхазский";
                case "az":
                    return "Азербайджанский";
                case "ay":
                    return "Аймара";
                case "sq":
                    return "Албанский";
                case "ar":
                    return "Арабский";
                case "hy":
                    return "Армянский";
                case "as":
                    return "Ассамский";
                case "af":
                    return "Африкаанс";
                case "ts":
                    return "Банту";
                case "eu":
                    return "Баскский";
                case "ba":
                    return "Башкирский";
                case "be":
                    return "Белорусский";
                case "bn":
                    return "Бенгальский";
                case "my":
                    return "Бирманский";
                case "bh":
                    return "Бихарский";
                case "bg":
                    return "Болгарский";
                case "br":
                    return "Бретонский";
                case "cy":
                    return "Валлийский";
                case "hu":
                    return "Венгерский";
                case "wo":
                    return "Волоф";
                case "vi":
                    return "Вьетнамский";
                case "gd":
                    return "Гаэльский";
                case "nl":
                    return "Голландский";
                case "el":
                    return "Греческий";
                case "ka":
                    return "Грузинский";
                case "gn":
                    return "Гуарани";
                case "da":
                    return "Датский";
                case "gr":
                    return "Древнегреческий";
                case "iw":
                    return "Древнееврейский";
                case "dr":
                    return "Древнерусский";
                case "zu":
                    return "Зулу";
                case "he":
                    return "Иврит";
                case "yi":
                    return "Идиш";
                case "in":
                    return "Индонезийский";
                case "ia":
                    return "Интерлингва";
                case "ga":
                    return "Ирландский";
                case "is":
                    return "Исландский";
                case "es":
                    return "Испанский";
                case "it":
                    return "Итальянский";
                case "kk":
                    return "Казахский";
                case "kn":
                    return "Каннада";
                case "ca":
                    return "Каталанский";
                case "ks":
                    return "Кашмири";
                case "qu":
                    return "Кечуа";
                case "ky":
                    return "Киргизский";
                case "zh":
                    return "Китайский";
                case "ko":
                    return "Корейский";
                case "kw":
                    return "Корнский";
                case "co":
                    return "Корсиканский";
                case "ku":
                    return "Курдский";
                case "km":
                    return "Кхмерский";
                case "xh":
                    return "Кхоса";
                case "la":
                    return "Латинский";
                case "lv":
                    return "Латышский";
                case "lt":
                    return "Литовский";
                case "mk":
                    return "Македонский";
                case "mg":
                    return "Малагасийский";
                case "ms":
                    return "Малайский";
                case "mt":
                    return "Мальтийский";
                case "mi":
                    return "Маори";
                case "mr":
                    return "Маратхи";
                case "mo":
                    return "Молдавский";
                case "mn":
                    return "Монгольский";
                case "na":
                    return "Науру";
                case "ne":
                    return "Непали";
                case "no":
                    return "Норвежский";
                case "pa":
                    return "Панджаби";
                case "fa":
                    return "Персидский";
                case "pl":
                    return "Польский";
                case "pt":
                    return "Португальский";
                case "ps":
                    return "Пушту";
                case "rm":
                    return "Ретороманский";
                case "ro":
                    return "Румынский";
                case "rn":
                    return "Рунди";
                case "sm":
                    return "Самоанский";
                case "sa":
                    return "Санскрит";
                case "sr":
                    return "Сербский";
                case "si":
                    return "Сингальский";
                case "sd":
                    return "Синдхи";
                case "sk":
                    return "Словацкий";
                case "sl":
                    return "Словенский";
                case "so":
                    return "Сомали";
                case "st":
                    return "Сото";
                case "sw":
                    return "Суахили";
                case "su":
                    return "Сунданский";
                case "tl":
                    return "Тагальский";
                case "tg":
                    return "Таджикский";
                case "th":
                    return "Тайский";
                case "ta":
                    return "Тамильский";
                case "tt":
                    return "Татарский";
                case "te":
                    return "Телугу";
                case "bo":
                    return "Тибетский";
                case "tr":
                    return "Турецкий";
                case "tk":
                    return "Туркменский";
                case "uz":
                    return "Узбекский";
                case "ug":
                    return "Уйгурский";
                case "ur":
                    return "Урду";
                case "fo":
                    return "Фарерский";
                case "fj":
                    return "Фиджи";
                case "fi":
                    return "Финский";
                case "fy":
                    return "Фризский";
                case "ha":
                    return "Хауса";
                case "hi":
                    return "Хинди";
                case "hr":
                    return "Хорватскосербский";
                case "cs":
                    return "Чешский";
                case "sv":
                    return "Шведский";
                case "sn":
                    return "Шона";
                case "eo":
                    return "Эсперанто";
                case "et":
                    return "Эстонский";
                case "jv":
                    return "Яванский";
                case "ja":
                    return "Японский";
                default:
                    return "Русский";
            }

        }

    }
}
