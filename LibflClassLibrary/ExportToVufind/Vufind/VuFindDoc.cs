using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Security;
using Utilities;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using Newtonsoft.Json;

namespace LibflClassLibrary.ExportToVufind.Vufind
{

    [JsonObject(MemberSerialization.Fields)]
    public class VufindDoc
    {
        public VufindDoc()
        {
            this.AccompayingMaterial = new VufindField();
            this.AccompayingMaterialLanguage = new VufindField();
            this.AdditionalBibRecord = new VufindField();
            this.Annotation = new VufindField();
            this.Another_author_AF_all = new VufindField();
            this.Another_title = new VufindField();
            this.Another_title_AF_All = new VufindField();
            this.author = new VufindField();
            this.Author_another_chart = new VufindField();
            this.author_corporate = new VufindField();
            this.author_corporate_role = new VufindField();
            this.author_role = new VufindField();
            this.author_sort = new VufindField();
            this.author_variant = new VufindField();
            this.author2 = new VufindField();
            this.author2_role = new VufindField();
            this.BasicTitleLanguage = new VufindField();
            this.CanceledISSN = new VufindField();
            this.CatalogerNote = new VufindField();
            this.collection = new VufindField();
            this.Collective_author_all = new VufindField();
            this.Country = new VufindField();
            this.description = new VufindField();
            this.Dimensions = new VufindField();
            this.DirectoryNote = new VufindField();
            this.EditionType = new VufindField();
            this.Editor = new VufindField();
            this.Editor_AF_all = new VufindField();
            this.Editor_role = new VufindField();
            this.format = new VufindField();
            this.genre = new VufindField();
            this.genre_facet = new VufindField();
            this.GeoNamePlaceOfPublication = new VufindField();
            this.GeoNamePlaceOfPublication_AF_All = new VufindField();
            this.HyperLink = new VufindField();
            this.HyperLinkNewViewer = new VufindField();
            this.id = "";
            this.IllustrationMaterial = new VufindField();
            this.Illustrations = new VufindField();
            this.IncorrectISBN = new VufindField();
            this.IncorrectISSN = new VufindField();
            this.Info_pertaining_title = new VufindField();
            this.Info_title_alt = new VufindField();
            this.IntermediateTranslateLanguage = new VufindField();
            this.isbn = new VufindField();
            this.issn = new VufindField();
            this.language = new VufindField();
            this.Language_title_alt = new VufindField();
            this.Level = "";
            this.Level_id = "";
            this.Location = new VufindField();
            this.MethodOfAccess = new VufindField();
            this.NumberInSeries = new VufindField();
            this.NumberInSubseries = new VufindField();
            this.Organization_nature = new VufindField();
            this.OwnerExemplar = new VufindField();
            this.OwnerOrganization = new VufindField();
            this.OwnerOrganization_AF_All = new VufindField();
            this.OwnerPerson = new VufindField();
            this.OwnerPerson_AF_All = new VufindField();
            this.Ownership = new VufindField();
            this.Parallel_title = new VufindField();
            this.Part_number = new VufindField();
            this.Part_title = new VufindField();
            this.PlaceOfPublication = new VufindField();
            this.PlaceOfPublication_AF_All = new VufindField();
            this.PlacingCipher = new VufindField();
            this.Printing = new VufindField();
            this.PrintingHouse = new VufindField();
            this.PrintingHouse_AF_All = new VufindField();
            this.Publication_info = new VufindField();
            this.publishDate = new VufindField();
            this.publisher = new VufindField();
            this.Responsibility_statement = new VufindField();
            this.SummaryLanguage = new VufindField();
            this.TableOfContentsLanguage = new VufindField();
            this.title = new VufindField();
            this.title_alt = new VufindField();
            this.title_alt = new VufindField();
            this.Title_another_chart = new VufindField();
            this.Title_same_author = new VufindField();
            this.title_short = new VufindField();
            this.title_sort = new VufindField();
            this.Title_unified = new VufindField();
            this.TitlePageLanguage = new VufindField();
            this.topic = new VufindField();
            this.topic_facet = new VufindField();
            this.Unified_Caption = new VufindField();
            this.Unified_Caption_AF_All = new VufindField();
            this.Volume = new VufindField();
            this.ExemplarsJSON = "";

            this.hierarchy_top_id = new VufindField();
            this.hierarchy_top_title = new VufindField();
            this.hierarchy_parent_title = new VufindField();
            this.hierarchy_parent_id = new VufindField();
            this.is_hierarchy_id = new VufindField();
            this.is_hierarchy_title = new VufindField();


            //this.NewArrivals = "";
        }

        public string id { get; set; }
        public string fund ;
        public string allfields ;
        public string Level ;
        public string Level_id;
        public VufindField title { get; set; }
        public VufindField title_short { get; set; }
        public VufindField title_sort { get; set; }
        public VufindField author { get; set; }
        public VufindField author_variant { get; set; }
        public VufindField author_sort { get; set; }
        public VufindField author2 { get; set; }
        public VufindField author_corporate { get; set; }
        public VufindField author_corporate_role { get; set; }
        public VufindField author_role { get; set; }
        public VufindField author2_role { get; set; }
        public VufindField format { get; set; }
        public VufindField genre { get; set; }
        public VufindField genre_facet { get; set; }
        public VufindField isbn { get; set; }
        public VufindField issn { get; set; }
        public VufindField language { get; set; }
        public VufindField publishDate { get; set; }
        public VufindField publisher { get; set; }
        public VufindField title_alt { get; set; }
        public VufindField collection { get; set; }

        public VufindField PlaceOfPublication { get; set; }
        public VufindField Title_another_chart { get; set; }
        public VufindField Title_same_author { get; set; }
        public VufindField Parallel_title { get; set; }
        public VufindField Info_pertaining_title { get; set; }
        public VufindField Responsibility_statement { get; set; }
        public VufindField Part_number { get; set; }
        public VufindField Part_title { get; set; }
        public VufindField Language_title_alt { get; set; }
        public VufindField Title_unified { get; set; }
        public VufindField Info_title_alt { get; set; }
        public VufindField Another_author_AF_all { get; set; }
        public VufindField Another_title { get; set; }
        public VufindField Another_title_AF_All { get; set; }
        public VufindField Unified_Caption { get; set; }
        public VufindField Unified_Caption_AF_All { get; set; }
        public VufindField Author_another_chart { get; set; }
        public VufindField Editor { get; set; }
        public VufindField Editor_AF_all { get; set; }
        public VufindField Editor_role { get; set; }
        public VufindField Collective_author_all { get; set; }
        public VufindField Organization_nature { get; set; }
        public VufindField Printing { get; set; }
        public VufindField Publication_info { get; set; }
        public VufindField EditionType { get; set; }
        public VufindField Country { get; set; }
        public VufindField PlaceOfPublication_AF_All { get; set; }
        public VufindField PrintingHouse { get; set; }
        public VufindField PrintingHouse_AF_All { get; set; }
        public VufindField GeoNamePlaceOfPublication { get; set; }
        public VufindField GeoNamePlaceOfPublication_AF_All { get; set; }
        public VufindField IncorrectISBN { get; set; }
        public VufindField IncorrectISSN { get; set; }
        public VufindField CanceledISSN { get; set; }
        public VufindField IntermediateTranslateLanguage { get; set; }
        public VufindField SummaryLanguage { get; set; }
        public VufindField TableOfContentsLanguage { get; set; }
        public VufindField TitlePageLanguage { get; set; }
        public VufindField BasicTitleLanguage { get; set; }
        public VufindField AccompayingMaterialLanguage { get; set; }
        public VufindField Volume { get; set; }
        public VufindField Illustrations { get; set; }
        public VufindField Dimensions { get; set; }
        public VufindField AccompayingMaterial { get; set; }
        public VufindField NumberInSeries { get; set; }
        public VufindField NumberInSubseries { get; set; }
        public VufindField description { get; set; }
        public VufindField Annotation { get; set; }
        public VufindField CatalogerNote { get; set; }
        public VufindField DirectoryNote { get; set; }
        public VufindField AdditionalBibRecord { get; set; }
        public VufindField HyperLink { get; set; }
        public VufindField HyperLinkNewViewer { get; set; }
        public VufindField topic { get; set; }
        public VufindField topic_facet { get; set; }
        public VufindField OwnerPerson { get; set; }
        public VufindField OwnerPerson_AF_All { get; set; }
        public VufindField OwnerOrganization { get; set; }
        public VufindField OwnerOrganization_AF_All { get; set; }
        public VufindField Ownership { get; set; }
        public VufindField OwnerExemplar { get; set; }
        public VufindField IllustrationMaterial { get; set; }
        public DateTime? NewArrivals;
        
        public VufindField Location { get; set; }
        public VufindField PlacingCipher { get; set; }
        public VufindField MethodOfAccess { get; set; }
        public List<BJExemplarInfo> Exemplars = new List<BJExemplarInfo>();
        public string ExemplarsJSON;

        //поля для сводного уровня
        public VufindField hierarchy_top_id { get; set; }
        public VufindField hierarchy_top_title { get; set; }
        public VufindField hierarchy_parent_title { get; set; }
        public VufindField hierarchy_parent_id { get; set; }
        public VufindField is_hierarchy_id { get; set; }
        public VufindField is_hierarchy_title { get; set; }
        //специальная обработка 
        //перед записью в файл экспорта нужно вызвать этот метод
        //1. Фасетные поля. Если фасетное поле пустое, то вставлять <нет данных>
        //это касается только фасетных полей.
        //2. ещё что-то
        public void SpecialProcessing()
        {
            //========================================================================PlaceOfPublication=========================
            string SpecialDataProcessing = this.PlaceOfPublication.ToString();
            if ((SpecialDataProcessing == "-") || (SpecialDataProcessing == string.Empty))
            {
                this.PlaceOfPublication = new VufindField("<нет данных>");
            }
            else if (SpecialDataProcessing == "М.")
            {
                this.PlaceOfPublication = new VufindField("Москва");
            }
            else if (SpecialDataProcessing == "М.:")
            {
                this.PlaceOfPublication = new VufindField("Москва");
            }
            //========================================================================Location=========================
            SpecialDataProcessing = this.Location.ToString();
            if (SpecialDataProcessing == string.Empty)
            {
                this.Location = new VufindField("<нет данных>");
            }
            //======================================================================Language======================================
            SpecialDataProcessing = this.language.ToString();
            if (SpecialDataProcessing == string.Empty)
            {
                this.language = new VufindField("<нет данных>");
            }
            //======================================================================Author======================================
            SpecialDataProcessing = this.author.ToString();
            if (SpecialDataProcessing == string.Empty)
            {
                this.author = new VufindField("<нет данных>");
            }

        }


        //создание XMLNode <doc> для экспорта
        public XmlNode CreateExportXmlNode()
        {
            this.SpecialProcessing();
            Type vfDocType = typeof(VufindDoc);
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode node;
            XmlNode result = xmlDoc.CreateElement("doc");
            foreach (PropertyInfo propertyInfo in vfDocType.GetProperties())
            {
                if (propertyInfo.Name == "id")
                {
                    node = AddXmlField(propertyInfo.Name, propertyInfo.GetValue(this, null).ToString());
                    node = xmlDoc.ImportNode(node, true);
                    result.AppendChild(node);
                    continue;
                }
                VufindField vfField = (VufindField)propertyInfo.GetValue(this, null);
                foreach (string val in vfField.ValueList)
                {
                    node = AddXmlField(propertyInfo.Name, val);
                    node = xmlDoc.ImportNode(node, true);
                    result.AppendChild(node);
                }
            }
            foreach (FieldInfo fieldInfo in vfDocType.GetFields())
            {
                if (fieldInfo.Name == "Exemplars")
                {
                    continue;
                }
                if (fieldInfo.Name == "ExemplarsJSON")
                {
                    node = AddXmlField("Exemplar", fieldInfo.GetValue(this).ToString());
                    node = xmlDoc.ImportNode(node, true);
                    result.AppendChild(node);
                    continue;
                }
                if (fieldInfo.Name == "NewArrivals")
                {
                    if (fieldInfo.GetValue(this) != null)
                    {
                        node = AddXmlField(fieldInfo.Name, ((DateTime)fieldInfo.GetValue(this)).ToString("yyyy-MM-ddThh:mm:ssZ"));
                        node = xmlDoc.ImportNode(node, true);
                        result.AppendChild(node);
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                node = AddXmlField(fieldInfo.Name, fieldInfo.GetValue(this).ToString());
                node = xmlDoc.ImportNode(node, true);
                result.AppendChild(node);
            }
            return result;
        }

        private XmlNode AddXmlField(string name, string val)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode field = xmlDoc.CreateElement("field");
            XmlAttribute att = xmlDoc.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            field.InnerText = SecurityElement.Escape(val);
            val = Extensions.XmlCharacterWhitelist(val);
            field.InnerText = val;
            return field;
        }


    }
}
