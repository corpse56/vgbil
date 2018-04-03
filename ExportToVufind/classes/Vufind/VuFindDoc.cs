using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExportBJ_XML.classes.BJ.Vufind;
using ExportBJ_XML.ValueObjects;

namespace ExportBJ_XML.classes
{
    public class VuFindDoc
    {
        public VuFindDoc() { }

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
        public VufindField topic_facet { get; set; }
        public VufindField OwnerPerson { get; set; }
        public VufindField OwnerPerson_AF_All { get; set; }
        public VufindField OwnerOrganization { get; set; }
        public VufindField OwnerOrganization_AF_All { get; set; }
        public VufindField Ownership { get; set; }
        public VufindField OwnerExemplar { get; set; }
        public VufindField IllustrationMaterial { get; set; }

        public List<ExemplarInfo> Exemplars = new List<ExemplarInfo>();


    }
}
