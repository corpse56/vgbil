using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Drawing;
using ExportBJ_XML.classes.BJ;
using ExportBJ_XML.ValueObjects;
using ExportBJ_XML.classes.DB;
using LibflClassLibrary.ExportToVufind.classes;
using LibflClassLibrary.ExportToVufind.classes.BJ;
using System.Windows.Forms;
using Utilities;

namespace ExportBJ_XML.classes
{
    public class BJVuFindConverter : VuFindConverter
    {

        public BJVuFindConverter(string fund)
        {
            this.Fund = fund;
            this.dbWrapper = new DatabaseWrapper(fund);
        }

        private int _lastID = 1;
        private DatabaseWrapper dbWrapper;
        private List<string> errors = new List<string>();
        private VufindXMLWriter writer;
        public override void Export()
        {
            writer = new VufindXMLWriter(this.Fund);
            writer.StartVufindXML();
            /////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////BJVVV/////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            _lastID = 1;
            this.StartExportFrom( _lastID );
        }

        private void StartExportFrom( int previous )
        {
            int step = 1;
            int MaxIDMAIN = GetMaxIDMAIN();
            VufindDoc vfDoc = new VufindDoc();
            for (int i = previous; i < MaxIDMAIN; i += step)
            {
                _lastID = i;
                DataTable record = dbWrapper.GetBJRecord(_lastID);
                if (record.Rows.Count == 0) continue;
                try
                {
                    vfDoc = CreateVufindDoc( record );
                    if (vfDoc == null) continue;
                }
                catch (Exception ex)
                {
                    errors.Add(this.Fund + "_" + i);
                    continue;
                }

                writer.AppendVufindDoc(vfDoc);

                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = this.Fund + "_" + i;
                OnRecordExported(args);
            }
            writer.FinishWriting();
            File.WriteAllLines(@"f:\import\importErrors\" + this.Fund + "Errors.txt", errors.ToArray());

        }
        public override void ExportSingleRecord( int idmain )
        {
            VufindXMLWriter writer = new VufindXMLWriter(this.Fund);
            VufindDoc vfDoc = new VufindDoc();

            DataTable record = dbWrapper.GetBJRecord(idmain); 
            vfDoc = CreateVufindDoc(record);
            if (vfDoc == null)
            {
                MessageBox.Show("Запись не имеет экземпляров.");
                return;
            }
            writer.WriteSingleRecord(vfDoc);
            MessageBox.Show("Завершено");
        }

        public VufindDoc CreateVufindDoc( DataTable BJBook )
        {
            int currentIDMAIN = (int)BJBook.Rows[0]["IDMAIN"];
            string level = BJBook.Rows[0]["Level"].ToString();
            string level_id = BJBook.Rows[0]["level_id"].ToString();
            int lev_id = int.Parse(level_id);
            if (lev_id < 0) return null;
            StringBuilder allFields = new StringBuilder();
            AuthoritativeFile AF_all = new AuthoritativeFile();
            bool wasTitle = false;//встречается ошибка: два заглавия в одном пине
            bool wasAuthor = false;//был ли автор. если был, то сортировочное поле уже заполнено
            string description = "";//все 3хх поля
            DataTable clarify;
            string query = "";
            string Annotation = "";
            int CarrierCode;
            VufindDoc result = new VufindDoc();

#region field analyse
            //BJBookInfo book = new BJBookInfo();
            foreach (DataRow r in BJBook.Rows)
            {

                allFields.AppendFormat(" {0}", r["PLAIN"].ToString());
                switch (r["code"].ToString())
                {
                    //=======================================================================Родные поля вуфайнд=======================
                    case "200$a":
                        if (wasTitle) break;
                        result.title.Add(r["PLAIN"].ToString());
                        result.title_short.Add(r["PLAIN"].ToString());
                        result.title_sort.Add(r["SORT"].ToString());
                        wasTitle = true;
                        break;
                    case "700$a":
                        result.author.Add(r["PLAIN"].ToString());
                        if (!wasAuthor)
                        {
                            //AddField("author_sort", r["SORT"].ToString());
                            result.author_sort.Add(r["SORT"].ToString());
                        }
                        wasAuthor = true;
                        //забрать все варианты написания автора из авторитетного файла и вставить в скрытое, но поисковое поле
                        break;
                    case "701$a":
                        result.author2.Add(r["PLAIN"].ToString());
                        break;
                    case "710$a":
                        result.author_corporate.Add(r["PLAIN"].ToString());
                        break;
                    case "710$4":
                        result.author_corporate_role.Add(r["PLAIN"].ToString());
                        break;
                    case "700$4":
                        result.author_role.Add(r["PLAIN"].ToString());
                        break;
                    case "701$4":
                        result.author2_role.Add(r["PLAIN"].ToString());
                        break;
                    case "921$a":
                        CarrierCode = KeyValueMapping.CarrierNameToCode.GetValueOrDefault(r["PLAIN"].ToString(), 3001);
                        result.format.Add(CarrierCode.ToString());
                        break;
                    case "922$e":
                        result.genre.Add(r["PLAIN"].ToString());
                        result.genre_facet.Add(r["PLAIN"].ToString());
                        break;
                    case "10$a":
                        clarify = dbWrapper.Clarify_10a((int)r["IDDATA"]);
                        string add = r["PLAIN"].ToString();
                        if (clarify.Rows.Count != 0)
                        {
                            add = r["PLAIN"].ToString() + " (" + clarify.Rows[0]["PLAIN"].ToString() + ")";
                        }
                        result.isbn.Add(add);
                        break;
                    case "11$a":
                        result.issn.Add(r["PLAIN"].ToString());
                        break;
                    case "101$a":
                        clarify = dbWrapper.Clarify_101a((int)r["IDINLIST"]);
                        if (clarify.Rows.Count == 0)
                        {
                            result.language.Add(r["PLAIN"].ToString());
                        }
                        else
                        {
                            result.language.Add(clarify.Rows[0]["NAME"].ToString());
                        }
                        break;
                    case "2100$d":
                        result.publishDate.Add(r["PLAIN"].ToString());
                        break;
                    case "210$c":
                        result.publisher.Add(r["PLAIN"].ToString());
                        break;
                    case "517$a":
                        clarify = dbWrapper.Clarify_517a((int)r["IDDATA"]);
                        string fieldValue;
                        fieldValue = (clarify.Rows.Count != 0) ?
                            "(" + clarify.Rows[0]["PLAIN"].ToString() + ")" + r["PLAIN"].ToString() :
                            r["PLAIN"].ToString();

                        result.title_alt.Add(fieldValue);
                        //нужно специальным образом обрабатывать
                        break;
                    //=======================================================================добавленные в индекс=======================
                    case "210$a":
                        result.PlaceOfPublication.Add(r["PLAIN"].ToString());
                        break;
                    case "200$6":
                        result.Title_another_chart.Add(r["PLAIN"].ToString());
                        break;
                    case "200$b":
                        result.Title_same_author.Add(r["PLAIN"].ToString());
                        break;
                    case "200$d":
                        result.Parallel_title.Add(r["PLAIN"].ToString());
                        break;
                    case "200$e":
                        result.Info_pertaining_title.Add(r["PLAIN"].ToString());
                        break;
                    case "200$f":
                        result.Responsibility_statement.Add(r["PLAIN"].ToString());
                        break;
                    case "200$h":
                        result.Part_number.Add(r["PLAIN"].ToString());
                        break;
                    case "200$i":
                        result.Part_title.Add(r["PLAIN"].ToString());
                        break;
                    case "200$z":
                        result.Language_title_alt.Add(r["PLAIN"].ToString());
                        break;
                    case "500$a":
                        result.Title_unified.Add(r["PLAIN"].ToString());
                        break;
                    case "500$3"://$3 is deprecated!!!
                        AF_all = GetAFAll( (int)r["AFLINKID"], "AFHEADERVAR");
                        result.Title_unified.Add(AF_all.ToString());
                        break;
                    case "517$e":
                        result.Info_title_alt.Add(r["PLAIN"].ToString());
                        break;
                    case "517$z":
                        result.Language_title_alt.Add(r["PLAIN"].ToString());
                        break;
                    case "700$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        foreach (string av in AF_all.AFValues)
                        {
                            result.author_variant.Add(av);
                        }
                        break;
                    case "701$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        result.Another_author_AF_all.Add(AF_all.ToString());//хранить но не отображать
                        break;
                    case "501$a":
                        result.Another_title.Add(r["PLAIN"].ToString());
                        break;
                    case "501$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFHEADERVAR");
                        result.Another_title_AF_All.Add(AF_all.ToString());
                        break;
                    case "503$a":
                        result.Unified_Caption.Add(r["PLAIN"].ToString());
                        break;
                    case "503$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFHEADERVAR");
                        result.Unified_Caption_AF_All.Add(AF_all.ToString());
                        break;
                    case "700$6":
                        result.Author_another_chart.Add(r["PLAIN"].ToString());
                        break;
                    case "702$a":
                        result.Editor.Add(r["PLAIN"].ToString());
                        break;
                    case "702$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        result.Editor_AF_all.Add(AF_all.ToString());
                        break;
                    case "702$4":
                        result.Editor_role.Add(r["PLAIN"].ToString());
                        break;
                    case "710$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        result.Collective_author_all.Add(AF_all.ToString());
                        break;
                    case "710$9":
                        result.Organization_nature.Add(r["PLAIN"].ToString());
                        break;
                    case "11$9":
                        result.Printing.Add(r["PLAIN"].ToString());
                        break;
                    case "205$a":
                        string PublicationInfo = r["PLAIN"].ToString();

                        // 205$b
                         
                        clarify = dbWrapper.Clarify_205a_1((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        // 205$f
                        clarify = dbWrapper.Clarify_205a_2((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += " / " + rr["PLAIN"].ToString();
                        }
                        // 205$g
                        clarify = dbWrapper.Clarify_205a_3((int)r["IDDATA"]);
                        foreach (DataRow rr in clarify.Rows)
                        {
                            PublicationInfo += "; " + rr["PLAIN"].ToString();
                        }
                        result.Publication_info.Add(PublicationInfo);
                        break;
                    case "921$b":
                        result.EditionType.Add(r["PLAIN"].ToString());
                        break;
                    case "102$a":
                        result.Country.Add(r["PLAIN"].ToString());
                        break;
                    case "210$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        result.PlaceOfPublication_AF_All.Add(AF_all.ToString());
                        break;
                    case "2110$g":
                        result.PrintingHouse.Add(r["PLAIN"].ToString()); 
                        break;
                    case "2110$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        result.PrintingHouse_AF_All.Add(AF_all.ToString());
                        break;
                    case "2111$e":
                        result.GeoNamePlaceOfPublication.Add(r["PLAIN"].ToString());
                        break;
                    case "2111$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFGEOVAR");
                        result.GeoNamePlaceOfPublication_AF_All.Add(AF_all.ToString());
                        break;
                    case "10$z":
                        result.IncorrectISBN.Add(r["PLAIN"].ToString()); 
                        break;
                    case "11$z":
                        result.IncorrectISSN.Add(r["PLAIN"].ToString());
                        break;
                    case "11$y":
                        result.CanceledISSN.Add(r["PLAIN"].ToString());
                        break;
                    case "101$b":
                        result.IntermediateTranslateLanguage.Add(r["PLAIN"].ToString());
                        break;
                    case "101$d":
                        result.SummaryLanguage.Add(r["PLAIN"].ToString());
                        break;
                    case "101$e":
                        result.TableOfContentsLanguage.Add(r["PLAIN"].ToString()); 
                        break;
                    case "101$f":
                        result.TitlePageLanguage.Add(r["PLAIN"].ToString());
                        break;
                    case "101$g":
                        result.BasicTitleLanguage.Add(r["PLAIN"].ToString()); 
                        break;
                    case "101$i":
                        result.AccompayingMaterialLanguage.Add(r["PLAIN"].ToString());
                        break;
                    case "215$a":
                        result.Volume.Add(r["PLAIN"].ToString());
                        break;
                    case "215$b":
                        result.Illustrations.Add(r["PLAIN"].ToString());
                        break;
                    case "215$c":
                        result.Dimensions.Add(r["PLAIN"].ToString()); 
                        break;
                    case "215$d":
                        result.AccompayingMaterial.Add(r["PLAIN"].ToString()); 
                        break;
                    case "225$a":
                        if (r["PLAIN"].ToString() == "") break;
                        if (r["PLAIN"].ToString() == "-1") break;
                        //AddHierarchyFields(Convert.ToInt32(r["PLAIN"]), Convert.ToInt32(r["IDMAIN"]));
                        break;
                    case "225$h":
                        result.NumberInSeries.Add(r["PLAIN"].ToString()); 
                        break;
                    case "225$v":
                        result.NumberInSubseries.Add(r["PLAIN"].ToString()); 
                        break;
                    case "300$a":
                    case "301$a":
                    case "316$a":
                    case "320$a":
                    case "326$a":
                    case "336$a":
                    case "337$a":
                        description += r["PLAIN"].ToString() + " ; ";
                        break;
                    case "327$a":
                    case "330$a":
                        Annotation += r["PLAIN"].ToString() + " ; ";
                        break;
                    case "830$a":
                        result.CatalogerNote.Add(r["PLAIN"].ToString());
                        break;
                    case "831$a":
                        result.DirectoryNote.Add(r["PLAIN"].ToString());
                        break;
                    case "924$a":
                        result.AdditionalBibRecord.Add(r["PLAIN"].ToString());
                        break;
                    case "940$a":
                        result.HyperLink.Add(r["PLAIN"].ToString()); 
                        break;
                    case "606$a"://"""""" • """"""
                        clarify = dbWrapper.Clarify_606a(Convert.ToInt32(r["SORT"]));
                        if (clarify.Rows.Count == 0) break;
                        string TPR = "";
                        foreach (DataRow rr in clarify.Rows)
                        {
                            TPR += rr["VALUE"].ToString() + " • ";
                        }
                        TPR = TPR.Substring(0, TPR.Length - 2);
                        result.topic.Add(TPR);
                        result.topic_facet.Add(TPR);
                        break;
                    case "3000$a":
                        result.OwnerPerson.Add(r["PLAIN"].ToString());
                        break;
                    case "3000$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFNAMESVAR");
                        result.OwnerPerson_AF_All.Add(AF_all.ToString());
                        break;
                    case "3001$a":
                        result.OwnerOrganization.Add(r["PLAIN"].ToString());
                        break;
                    case "3001$3":
                        AF_all = GetAFAll((int)r["AFLINKID"], "AFORGSVAR");
                        result.OwnerOrganization_AF_All.Add(AF_all.ToString());
                        break;
                    case "3002$a":
                        result.Ownership.Add(r["PLAIN"].ToString()); 
                        break;
                    case "3003$a":
                        result.OwnerExemplar.Add(r["PLAIN"].ToString());
                        break;
                    case "3200$a":
                        result.IllustrationMaterial.Add(r["PLAIN"].ToString());
                        break;
                }

            }
#endregion
            result.id = this.Fund + "_" + currentIDMAIN;
            string rusFund = GetFundId(this.Fund);

            result.fund = rusFund;
            result.allfields = allFields.ToString();
            result.Level = level;
            result.Level_id = level_id;
            result.Annotation.Add(Annotation);

            if (description != "")
            {
                result.description.Add(description);
            }

            AddExemplarFields(currentIDMAIN, result, this.Fund);

            if (result.Exemplars.Count == 0)
            {
                return null;
            }

            return result;
        }


        private void AddExemplarFields(int idmain, VufindDoc result, string fund)
        {

            DataTable table = dbWrapper.GetAllExemplars(idmain);
            if (table.Rows.Count == 0) return;
            int IDMAIN = (int)table.Rows[0]["IDMAIN"];

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            JsonWriter writer = new JsonTextWriter(sw);

            //3901 - проверить, почему у этого пина удаленный доступ

            //{"1":
            //    {
            //     "exemplar_location":"Абонемент",
            //     "exemplar_collection":"ОФ",
            //     "exemplar_placing_cipher":"08.B 4650",
            //     "exemplar_carrier":"Бумага",
            //     "exemplar_inventory_number":"2494125",
            //     "exemplar_class_edition":"Для выдачи"
            //    },
            // "2":
            //    {
            //        "exemplar_location":"Абонемент",
            //        "exemplar_collection":"ОФ",
            //        "exemplar_placing_cipher":"08.B 4651",
            //        "exemplar_carrier":"Бумага",
            //        "exemplar_inventory_number":"2494126",
            //        "exemplar_class_edition":"Для выдачи"
            //    }
            //}


            writer.WriteStartObject();

            DataTable exemplar;
            int cnt = 1;
            string CarrierCode = "";
            List<DateTime> ExemplarsCreatedDateList = new List<DateTime>();

            foreach (DataRow iddata in table.Rows)
            {
                exemplar = dbWrapper.GetExemplar((int)iddata["IDDATA"]);
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();

                //ExemplarInfo bjExemplar = new ExemplarInfo((int)iddata["IDDATA"]);
                ExemplarInfo Convolute = null;
                foreach (DataRow r in exemplar.Rows)
                {
                    string code = r["MNFIELD"].ToString() + r["MSFIELD"].ToString();
                    switch (code)
                    {
                        case "899$a":
                            string plain = r["PLAIN"].ToString();
                            string UL = KeyValueMapping.UnifiedLocation.GetValueOrDefault(r["NAME"].ToString(), "отсутствует в словаре");
                            int LocationCode = KeyValueMapping.UnifiedLocationCode.GetValueOrDefault(UL, 2999);

                            writer.WritePropertyName("exemplar_location");
                            writer.WriteValue(LocationCode);
                            result.Location.Add(LocationCode.ToString());
                            break;
                        case "482$a":
                            writer.WritePropertyName("exemplar_interlaced_to");
                            writer.WriteValue(r["PLAIN"].ToString());
                            
                            writer.WritePropertyName("exemplar_convolute");
                            Convolute = ExemplarInfo.GetExemplarByInventoryNumber(r["PLAIN"].ToString(), this.Fund);
                            if (Convolute == null)
                            {
                                writer.WriteValue("Ошибка заполнения библиографического описания конволюта");
                                errors.Add(this.Fund + "_" + idmain);
                            }
                            else
                            {
                                writer.WriteValue(this.Fund + "_"+Convolute.IDMAIN);
                            }
                            break;
                        case "899$b"://тут надо оставить только коллекции
                            string fnd = r["PLAIN"].ToString();
                            writer.WritePropertyName("exemplar_collection");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$c":
                            writer.WritePropertyName("exemplar_rack_location");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$d":
                            writer.WritePropertyName("exemplar_direction_temporary_storage");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$j":
                            result.PlacingCipher.Add(r["PLAIN"].ToString());
                            writer.WritePropertyName("exemplar_placing_cipher");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "899$p":
                            writer.WritePropertyName("exemplar_inventory_number");
                            writer.WriteValue(r["PLAIN"].ToString());
                            ExemplarsCreatedDateList.Add((DateTime)r["Created"]);
                            break;
                        case "899$w":
                            writer.WritePropertyName("exemplar_barcode");
                            writer.WriteValue(r["PLAIN"].ToString());
                            ExemplarsCreatedDateList.Add((DateTime)r["Created"]);
                            break;
                        case "899$x":
                            writer.WritePropertyName("exemplar_inv_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$a":
                            writer.WritePropertyName("exemplar_carrier");
                            CarrierCode = KeyValueMapping.CarrierNameToCode.GetValueOrDefault(r["PLAIN"].ToString(), 3001).ToString();
                            writer.WriteValue(CarrierCode);
                            break;
                        case "921$c":
                            writer.WritePropertyName("exemplar_class_edition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "921$d":

                            //writer.WritePropertyName("exemplar_class_edition");
                            //writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        //case "922$b":
                            //Exemplar += "Трофей\\Принадлежность к:" + r["PLAIN"].ToString() + "#";
                            //writer.WritePropertyName("exemplar_trophy");
                           // writer.WriteValue(r["PLAIN"].ToString());
                           // break;
                        case "3300$a":
                            writer.WritePropertyName("exemplar_binding_kind");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$b":
                            writer.WritePropertyName("exemplar_binding_age");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$c":
                            writer.WritePropertyName("exemplar_binding_type");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$d":
                            writer.WritePropertyName("exemplar_cover_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$e":
                            writer.WritePropertyName("exemplar_material_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$f":
                            writer.WritePropertyName("exemplar_spine_material");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$g":
                            writer.WritePropertyName("exemplar_bandages");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$h":
                            writer.WritePropertyName("exemplar_stamping_on_cover");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$i":
                            writer.WritePropertyName("exemplar_embossing_on_spine");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$j":
                            writer.WritePropertyName("exemplar_fittings");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$k":
                            writer.WritePropertyName("exemplar_bugs");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$l":
                            writer.WritePropertyName("exemplar_forth");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$m":
                            writer.WritePropertyName("exemplar_cutoff");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$n":
                            writer.WritePropertyName("exemplar_condition");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$o":
                            writer.WritePropertyName("exemplar_case");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$p":
                            writer.WritePropertyName("exemplar_embossing_on_edge");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                        case "3300$r":
                            writer.WritePropertyName("exemplar_binding_note");
                            writer.WriteValue(r["PLAIN"].ToString());
                            break;
                    }

                }
                //Exemplar += "exemplar_id:" + ds.Tables["t"].Rows[0]["IDDATA"].ToString() + "#";
                writer.WritePropertyName("exemplar_id");
                writer.WriteValue(iddata["IDDATA"].ToString());

                ExemplarInfo bjExemplar = ExemplarInfo.GetExemplarByIdData((int)iddata["IDDATA"], this.Fund);

                if (bjExemplar.Fields["482$a"].ToString() == string.Empty)//обычный экземпляр
                {
                    result.MethodOfAccess.Add(bjExemplar.ExemplarAccess.MethodOfAccess.ToString());
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue(bjExemplar.ExemplarAccess.Access);
                    writer.WritePropertyName("exemplar_access_group");
                    writer.WriteValue(KeyValueMapping.AccessCodeToGroup[bjExemplar.ExemplarAccess.Access]);
                    result.Exemplars.Add(bjExemplar);
                }
                else
                {
                    if (Convolute != null)//если книга аллигат и удалось найти конволют
                    {
                        result.MethodOfAccess.Add(Convolute.ExemplarAccess.MethodOfAccess.ToString());
                        writer.WritePropertyName("exemplar_access");
                        writer.WriteValue(Convolute.ExemplarAccess.Access);
                        writer.WritePropertyName("exemplar_access_group");
                        writer.WriteValue(KeyValueMapping.AccessCodeToGroup[Convolute.ExemplarAccess.Access]);
                        result.Exemplars.Add(Convolute);
                    }
                    else//если книга аллигат, но нельзя найти конволют. Это и есть тип доступа 1016.
                    {
                        result.MethodOfAccess.Add("4005");
                        writer.WritePropertyName("exemplar_access");
                        writer.WriteValue("1999");
                        writer.WritePropertyName("exemplar_access_group");
                        writer.WriteValue(KeyValueMapping.AccessCodeToGroup[1999]);
                    }
                }
                writer.WriteEndObject();
            }

            //смотрим есть ли гиперссылка
            table = dbWrapper.GetHyperLink(IDMAIN);
            if (table.Rows.Count != 0)//если есть - вставляем отдельным экземпляром. после электронной инвентаризации этот кусок можно будет убрать
            {
                //ExemplarInfo bjExemplar = new ExemplarInfo(-1);
                writer.WritePropertyName(cnt++.ToString());
                writer.WriteStartObject();


                //Exemplar += "Электронная копия: есть#";
                writer.WritePropertyName("exemplar_electronic_copy");
                writer.WriteValue("да");
                //Exemplar += "Гиперссылка: " + ds.Tables["t"].Rows[0]["PLAIN"].ToString() + " #";
                writer.WritePropertyName("exemplar_hyperlink");
                writer.WriteValue(table.Rows[0]["PLAIN"].ToString());

                writer.WritePropertyName("exemplar_hyperlink_newviewer");
                writer.WriteValue("/Bookreader/Viewer?bookID="+result.id+"&view_mode=HQ");
                result.HyperLinkNewViewer.Add("/Bookreader/Viewer?bookID=" + result.id + "&view_mode=HQ");

                DataTable hyperLinkTable = dbWrapper.GetBookScanInfo(IDMAIN);
                bool ForAllReader = false;
                if (hyperLinkTable.Rows.Count != 0)
                {
                    ForAllReader = (bool)hyperLinkTable.Rows[0]["ForAllReader"];
                    //Exemplar += "Авторское право: " + ((ds.Tables["t"].Rows[0]["ForAllReader"].ToString() == "1") ? "нет" : "есть");
                    writer.WritePropertyName("exemplar_copyright");
                    writer.WriteValue( (ForAllReader) ? "нет" : "есть" );
                    //Exemplar += "Ветхий оригинал: " + ((ds.Tables["t"].Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_old_original");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["OldBook"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Наличие PDF версии: " + ((ds.Tables["t"].Rows[0]["PDF"].ToString() == "1") ? "да" : "нет");
                    writer.WritePropertyName("exemplar_PDF_exists");
                    writer.WriteValue(((hyperLinkTable.Rows[0]["PDF"].ToString() == "1") ? "да" : "нет"));
                    //Exemplar += "Доступ: Заказать через личный кабинет";
                    writer.WritePropertyName("exemplar_access");
                    writer.WriteValue(
                        (ForAllReader) ?
                        "1001" : "1002");
                        //"Для прочтения онлайн необходимо перейти по ссылке" :
                        //"Для прочтения онлайн необходимо положить в корзину и заказать через личный кабинет");
                    writer.WritePropertyName("exemplar_access_group");
                    writer.WriteValue((ForAllReader) ? KeyValueMapping.AccessCodeToGroup[1001] : KeyValueMapping.AccessCodeToGroup[1002]);

                    writer.WritePropertyName("exemplar_carrier");
                    //writer.WriteValue("Электронная книга");
                    writer.WriteValue("3011");

                    writer.WritePropertyName("exemplar_id");
                    writer.WriteValue("ebook");//для всех у кого есть электронная копия. АПИ когда это значение встретит, сразу вернёт "доступно"
                    writer.WritePropertyName("exemplar_location");
                    writer.WriteValue("2030");

                }
                writer.WriteEndObject();
                result.MethodOfAccess.Add("4002");

                //определить структуру, в которую полностью запись ложится и здесь её проверять, чтобы правильно вычисляемые поля проставить.
            }
            writer.WriteEndObject();
            writer.Flush();
            writer.Close();

            result.ExemplarsJSON = sb.ToString();
            if (ExemplarsCreatedDateList.Count != 0)
            {
                result.NewArrivals = ExemplarsCreatedDateList.Min();
            }
        }


        public override void ExportCovers()
        {
            StringBuilder sb = new StringBuilder();
            DataTable table = dbWrapper.GetAllIdmainWithImages();
            List<string> errors = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                DataTable pics = dbWrapper.GetImage((int)row["IDMAIN"]);
                int i = 0;
                foreach (DataRow pic in pics.Rows)
                {
                    byte[] p = (byte[])pic["PIC"];
                    MemoryStream ms = new MemoryStream(p);
                    Image img = Image.FromStream(ms);
                    string path = @"f:\import\covers\"+this.Fund.ToLower()+"\\" + row["IDMAIN"].ToString() + @"\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (i == 0)
                    {
                        i++;
                        try
                        {
                            img.Save(path + "cover.jpg");
                        }
                        catch (Exception ex)
                        {
                            errors.Add(pic["PIC"].ToString() + " " + ex.Message + ex.InnerException);

                        }
                    }
                    else
                    {
                        img.Save(path + "cover"+i++.ToString() + ".jpg");
                    }
                }

            }
            File.WriteAllLines(@"f:\import\importErrors\" + this.Fund + "Errors.txt", errors.ToArray());

        }

        private void AddHierarchyFields(int ParentPIN, int CurrentPIN)
        {
            //DataTable table = dbWrapper.GetBJRecord(ParentPIN);
            //int TopHierarchyId = GetTopId( ParentPIN );
            //AddField("hierarchy_top_id", this.Fund + "_" + TopHierarchyId);

            //table = dbWrapper.GetTitle(TopHierarchyId);
            //if (table.Rows.Count != 0)
            //{
            //    string hierarchy_top_title = table.Rows[0]["PLAIN"].ToString();
            //    AddField("hierarchy_top_title", hierarchy_top_title);
            //}
            //AddField("hierarchy_parent_id", this.Fund + "_" + ParentPIN);

            //table = dbWrapper.GetTitle(ParentPIN);
            //if (table.Rows.Count != 0)
            //{
            //    string hierarchy_parent_title = table.Rows[0]["PLAIN"].ToString();
            //    AddField("hierarchy_parent_title", hierarchy_parent_title);
            //}

            //bool metka = false;
            //foreach (XmlNode n in _doc.ChildNodes)
            //{
            //    if (n.Attributes["name"].Value == "is_hierarchy_id")
            //    {
            //        metka = true;
            //    }
            //}
            //if (!metka)
            //{
            //    AddField("is_hierarchy_id", this.Fund + "_" + CurrentPIN);//пометка о том, что это серия
            //}

            //table = dbWrapper.GetTitle(CurrentPIN);
            //if (table.Rows.Count != 0)
            //{
            //    string is_hierarchy_title = table.Rows[0]["PLAIN"].ToString();

            //    metka = false;
            //    foreach (XmlNode n in _doc.ChildNodes)
            //    {
            //        if (n.Attributes["name"].Value == "is_hierarchy_id")
            //        {
            //            metka = true;
            //        }
            //    }
            //    if (!metka) AddField("is_hierarchy_title", is_hierarchy_title);
            //}
        }
        private int GetTopId(int ParentPIN)
        {
            DataTable table = dbWrapper.GetParentIDMAIN(ParentPIN);
            if (table.Rows.Count == 0)
            {
                return ParentPIN;
            }
            ParentPIN = Convert.ToInt32(table.Rows[0]["SORT"]);
            return GetTopId(ParentPIN);
        }
        private int GetMaxIDMAIN()
        {
            DataTable table = dbWrapper.GetMaxIDMAIN();
            return int.Parse(table.Rows[0][0].ToString());
        }
        private AuthoritativeFile GetAFAll( int AFLinkId, string AFTable)
        {
            //NAMES: (1) Персоналии
            //ORGS: (2) Организации
            //HEADER: (3) Унифицированное заглавие
            //DEL: (5) Источник списания
            //GEO: (6) Географическое название

            AuthoritativeFile result = new AuthoritativeFile();
            DataTable table = dbWrapper.GetAFAllValues(AFTable, AFLinkId);
            foreach (DataRow r in table.Rows)
            {
                result.Add(r["PLAIN"].ToString());
            }
            return result;
        }




    }
}
