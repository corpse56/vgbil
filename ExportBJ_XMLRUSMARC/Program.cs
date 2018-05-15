using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibflClassLibrary;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ExportBJ_XMLRUSMARC
{
    class Program
    {
        class FullIdentifier
        {
            public FullIdentifier(string Identifier, string Attribute)
            {
                this.Attribute = Attribute;
                this.Identifier = Identifier;
            }
            public string Identifier;
            public string Attribute;//dllocal - когда лицензия закончилась, dlopen - когда для всех всегда можно
        }

        static void Main(string[] args)
        {
            List<FullIdentifier> identifiers = new List<FullIdentifier>();

            //=====================================================================================================================
            DataSet DS = new DataSet();
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
            DA.SelectCommand.CommandText = "select * from  BookAddInf..ScanInfo where PDF = 1 and ForAllReader = 1";
            int i = DA.Fill(DS, "rec");

            foreach (DataRow row in DS.Tables["rec"].Rows)
            {
                if (row["IDBase"].ToString() == "1")
                {
                    FullIdentifier id = new FullIdentifier("BJVVV" + row["IDBook"].ToString(), "dlopen");
                    identifiers.Add(id);
                }
                else
                {
                    FullIdentifier id = new FullIdentifier("REDKOSTJ" + row["IDBook"].ToString(), "dlopen");
                    identifiers.Add(id);
                }
            }

            //=====================================================================================================================
            string[] lines = File.ReadAllLines("f:\\PinsForNeb_dllocal.txt");

            foreach (string line in lines)
            {
                identifiers.Add(new FullIdentifier("BJVVV"+line, "dllocal"));
            }
            //=====================================================================================================================
            lines = File.ReadAllLines("f:\\PinsForNeb_dlopen.txt");

            foreach (string line in lines)
            {
                identifiers.Add(new FullIdentifier("BJVVV" + line, "dlopen"));
            }


            XmlDocument xdoc = GetXmlDocument(identifiers);

            xdoc.Save("f:\\elar_electronic_pins.xml");

            Console.WriteLine("Готово");
            Console.ReadKey();

        }

        private static XmlDocument GetXmlDocument(List<FullIdentifier> identifiers)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("OAI-PMH");
            XmlAttribute attribute = xmlDoc.CreateAttribute("xmlns");
            attribute.Value = "http://www.openarchives.org/OAI/2.0/";
            rootNode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("xmlns:xsi");
            attribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
            rootNode.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            attribute.Value = "http://www.openarchives.org/OAI/2.0/ http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd";
            rootNode.Attributes.Append(attribute);
            xmlDoc.AppendChild(rootNode);
            XmlNode node = xmlDoc.CreateElement("responseDate");
            node.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            rootNode.AppendChild(node);

            //node = xmlDoc.CreateElement("request");
            //attribute = xmlDoc.CreateAttribute("verb");
            //attribute.Value = Request["verb"];
            //node.Attributes.Append(attribute);
            //attribute = xmlDoc.CreateAttribute("identifier");
            //attribute.Value = Request["identifier"];
            //node.Attributes.Append(attribute);
            //attribute = xmlDoc.CreateAttribute("metadataPrefix");
            //attribute.Value = Request["metadataPrefix"];
            //node.Attributes.Append(attribute);
            //rootNode.AppendChild(node);

            int cnt = 1;
            foreach (FullIdentifier identifier in identifiers)
            {
                Console.WriteLine((cnt++).ToString() + " из " + identifiers.Count.ToString());
                XmlNode GetRecord = xmlDoc.CreateElement(identifier.Attribute);
                rootNode.AppendChild(GetRecord);

                //identifier = identifier.Substring(13);//oai:aleph.nlr.ru:BJVVV1410763//oai:libfl.ru:BJVVV1410763
                string BAZA, IDMAIN;
                DataSet DS = new DataSet();
                SqlDataAdapter DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
                if (identifier.Identifier[0] == 'B')
                {
                    BAZA = "BJVVV";
                    IDMAIN = identifier.Identifier.Substring(5);
                    DA.SelectCommand.CommandText = "select * from  BJVVV..MAIN where ID = " + IDMAIN;
                }
                else
                {
                    BAZA = "REDKOSTJ";
                    IDMAIN = identifier.Identifier.Substring(8);
                    DA.SelectCommand.CommandText = "select * from  REDKOSTJ..MAIN where ID = " + IDMAIN;
                }
                int i = DA.Fill(DS, "rec");
                if (i == 0)
                {
                    continue;
                }
                DateTime datestamp = Convert.ToDateTime(DS.Tables["rec"].Rows[0]["DateChange"]);
                XmlNode RecordNode = AppendRecordNode(xmlDoc, IDMAIN, BAZA, datestamp, GetRecord);
                GetRecord.AppendChild(RecordNode);
            }
            return xmlDoc;


        }
        static private XmlNode AppendRecordNode(XmlDocument xmlDoc, string IDMAIN, string BAZA, DateTime datestamp, XmlNode VerbNode)
        {
            RMCONVERT rm = new RMCONVERT(BAZA);
            try
            {
                rm.FormRUSM(Convert.ToInt32(IDMAIN));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            DataSet DS = new DataSet();
            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection("Data Source=192.168.4.7;Initial Catalog=TECHNOLOG_VVV;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
            DA.SelectCommand.CommandText = "select distinct MET,IND1,IND2,IDBLOCK from TECHNOLOG_VVV..RUSM where IDMAIN = " + IDMAIN;
            int i = DA.Fill(DS, "rusm");

            XmlNode RecordNode = xmlDoc.CreateElement("record");
            VerbNode.AppendChild(RecordNode);
            XmlNode HeaderNode = xmlDoc.CreateElement("header");
            RecordNode.AppendChild(HeaderNode);

            XmlNode node = xmlDoc.CreateElement("identifier");
            node.InnerText = "oai:libfl.ru:" + BAZA + IDMAIN;
            HeaderNode.AppendChild(node);
            node = xmlDoc.CreateElement("datestamp");
            node.InnerText = datestamp.ToString("yyyy-MM-ddTHH:mm:ssZ");//"2015-03-05T02:04:40Z");
            HeaderNode.AppendChild(node);
            XmlNode MetaData = xmlDoc.CreateElement("metadata");
            RecordNode.AppendChild(MetaData);
            XmlNode MarcRecord = xmlDoc.CreateElement("marc", "record", "http://www.loc.gov/MARC21/slim");
            XmlAttribute attribute = xmlDoc.CreateAttribute("xmlns:xsi");
            attribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
            MarcRecord.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            attribute.Value = "http://www.loc.gov/MARC21/slim http://www.loc.gov/standards/marcxml/schema/MARC21slim.xsd";
            MarcRecord.Attributes.Append(attribute);

            MetaData.AppendChild(MarcRecord);

            char c31 = (char)31;

            foreach (DataRow row in DS.Tables["rusm"].Rows)
            {
                if ((row["MET"].ToString() == "0") && (row["IND1"].ToString() == "0") && (row["IND2"].ToString() == "0"))
                {
                    DA.SelectCommand.CommandText = "select * from TECHNOLOG_VVV..RUSM where MET = " + row["MET"].ToString() + " and IND1 = '" + row["IND1"].ToString() + "' and IND2='" + row["IND2"].ToString() + "'";
                    if (DS.Tables["controlfield"] != null) { DS.Tables["controlfield"].Clear(); DS.Tables["controlfield"].AcceptChanges(); }//{ while (DS.Tables["controlfield"].Rows.Count > 0) DS.Tables["controlfield"].Rows.Remove(DS.Tables["controlfield"].Rows[0]); DS.Tables["controlfield"].AcceptChanges(); }
                    DA.Fill(DS, "controlfield");
                    node = xmlDoc.CreateElement("marc", "leader", "http://www.loc.gov/MARC21/slim");
                    node.InnerText = DS.Tables["controlfield"].Rows[0]["POL"].ToString();
                    MarcRecord.AppendChild(node);
                    continue;
                }

                if ((Convert.ToInt32(row["MET"]) < 10) && (Convert.ToInt32(row["MET"]) > 0))
                {
                    node = xmlDoc.CreateElement("marc", "controlfield", "http://www.loc.gov/MARC21/slim");
                    DA.SelectCommand.CommandText = "select * from TECHNOLOG_VVV..RUSM where MET = " + row["MET"].ToString() + " and IND1 = '" + row["IND1"].ToString() + "' and IND2='" + row["IND2"].ToString() + "'";
                    if (DS.Tables["controlfield"] != null)
                    {
                        while (DS.Tables["controlfield"].Rows.Count > 0)
                        {
                            DS.Tables["controlfield"].Rows.RemoveAt(0);
                        }
                    }
                    DA.Fill(DS, "controlfield");
                    attribute = xmlDoc.CreateAttribute("tag");
                    string tag = row["MET"].ToString();
                    if (tag.Length == 1) tag = "00" + tag;
                    if (tag.Length == 2) tag = "0" + tag;
                    attribute.Value = tag;
                    node.Attributes.Append(attribute);
                    if (tag == "001")
                    {
                        node.InnerText = BAZA + DS.Tables["controlfield"].Rows[0]["POL"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    }
                    else
                    {
                        node.InnerText = DS.Tables["controlfield"].Rows[0]["POL"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                    }
                    MarcRecord.AppendChild(node);
                    continue;
                }

                DA.SelectCommand.CommandText = "select * from TECHNOLOG_VVV..RUSM where MET = " + row["MET"].ToString() + " and IND1 = '" + row["IND1"].ToString() + "' and IND2='" + row["IND2"].ToString() + "' and IDBLOCK='" + row["IDBLOCK"].ToString() + "'";
                if (DS.Tables["subfield"] != null)
                {
                    while (DS.Tables["subfield"].Rows.Count > 0)
                    {
                        DS.Tables["subfield"].Rows.RemoveAt(0);
                    }
                }

                int j = DA.Fill(DS, "subfield");

                node = xmlDoc.CreateElement("marc", "datafield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("tag");
                attribute.Value = row["MET"].ToString();
                node.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ind1");
                attribute.Value = row["IND1"].ToString();
                node.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ind2");
                attribute.Value = row["IND2"].ToString();
                node.Attributes.Append(attribute);
                MarcRecord.AppendChild(node);

                bool FirstSubField = true;
                XmlNode subf;
                foreach (DataRow rsub in DS.Tables["subfield"].Rows)
                {
                    if (((row["MET"].ToString() == "606") || (row["MET"].ToString() == "675") || (row["MET"].ToString() == "600")) && (!FirstSubField))
                    {
                        node = xmlDoc.CreateElement("marc", "datafield", "http://www.loc.gov/MARC21/slim");
                        attribute = xmlDoc.CreateAttribute("tag");
                        attribute.Value = row["MET"].ToString();
                        node.Attributes.Append(attribute);
                        attribute = xmlDoc.CreateAttribute("ind1");
                        attribute.Value = row["IND1"].ToString();
                        node.Attributes.Append(attribute);
                        attribute = xmlDoc.CreateAttribute("ind2");
                        attribute.Value = row["IND2"].ToString();
                        node.Attributes.Append(attribute);
                        MarcRecord.AppendChild(node);
                    }
                    FirstSubField = false;

                    string pol = rsub["POL"].ToString();
                    int k = pol.IndexOf(c31);

                    if (k > 0)
                    {
                        bool df;
                        if (IDMAIN == "1410763")
                        {
                            df = true;
                        }
                        subf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                        attribute = xmlDoc.CreateAttribute("code");
                        attribute.Value = rsub["IDENT"].ToString();
                        subf.Attributes.Append(attribute);
                        subf.InnerText = pol.Substring(0, k).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                        node.AppendChild(subf);

                        //pol = pol.Substring(k);

                        while (k > 0)
                        {
                            char ident = pol.Substring(k + 1, 1)[0];
                            string ppol = pol.Substring(k + 1);
                            if (ppol.IndexOf(c31) > 0)
                            {
                                ppol = ppol.Substring(1, ppol.IndexOf(c31) - 1);
                            }

                            pol = pol.Substring(k + 1);
                            k = pol.IndexOf(c31);
                            subf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                            attribute = xmlDoc.CreateAttribute("code");
                            attribute.Value = ident.ToString();
                            subf.Attributes.Append(attribute);
                            if (k < 0)
                                ppol = ppol.Substring(1);
                            subf.InnerText = ppol.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                            node.AppendChild(subf);

                        }
                    }
                    else
                    {
                        subf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                        attribute = xmlDoc.CreateAttribute("code");
                        attribute.Value = rsub["IDENT"].ToString();
                        subf.Attributes.Append(attribute);
                        subf.InnerText = pol.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                        node.AppendChild(subf);
                    }
                    //ident = pol.Substring(0, 1)[0];
                    //ppol = pol.Substring(1);
                    //fsout_str(fsout, "<marc:subfield code=\"" + ident + "\">" + ppol.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;") + "</marc:subfield>");
                }

            }//foreach RUSM
            //$d /mnt/fs-share/BJVVV/1/319/837 – абсолютный путь до папки с файлом
            //$f book.pdf – имя файла
            //$y file – тип доступа
            int PdfExists = 0;
            bool ForAllReader = false;
            if (rm.BAZA == "BJVVV")
            {
                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
                DA.SelectCommand.CommandText = "select * from [BookAddInf].[dbo].[ScanInfo] where IDBook = " + IDMAIN +
                                               " and IDBASE = 1 and PDF = 1"; ;
                PdfExists = DA.Fill(DS, "pdf");
            }
            else
            {
                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection("Data Source=192.168.4.25,1443;Initial Catalog=EXPORTNEB;Persist Security Info=True;User ID=OAIUSER;Password=User_OAI;Connection Timeout = 1200");
                DA.SelectCommand.CommandText = "select * from [BookAddInf].[dbo].[ScanInfo] where IDBook = " + IDMAIN +
                                               " and IDBASE = 2 and PDF = 1";
                PdfExists = DA.Fill(DS, "pdf");
            }

            if (PdfExists == 0)
            {
                return RecordNode;
            }
            ForAllReader = (bool)DS.Tables["pdf"].Rows[0]["ForAllReader"];
            string path = DS.Tables["PDF"].Rows[0]["IDBook"].ToString();
            XmlNode subpdf;
            switch (path.Length)
            {
                case 1:
                    path = "00000000" + path;
                    break;
                case 2:
                    path = "0000000" + path;
                    break;
                case 3:
                    path = "000000" + path;
                    break;
                case 4:
                    path = "00000" + path;
                    break;
                case 5:
                    path = "0000" + path;
                    break;
                case 6:
                    path = "000" + path;
                    break;
                case 7:
                    path = "00" + path;
                    break;
                case 8:
                    path = "0" + path;
                    break;
            }
            if (rm.BAZA == "BJVVV")
            {
                path = "/mnt/fs-share/BJVVV/" + path[0] + path[1] + path[2] + @"/" + path[3] + path[4] + path[5] + @"/" + path[6] + path[7] + path[8] + @"/PDF_A";
            }
            else
            {
                path = "/mnt/fs-share/REDKOSTJ/" + path[0] + path[1] + path[2] + @"/" + path[3] + path[4] + path[5] + @"/" + path[6] + path[7] + path[8] + @"/PDF_A";
            }
            if (ForAllReader)
            {
                node = xmlDoc.CreateElement("marc", "datafield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("tag");
                attribute.Value = "856";
                node.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ind1");
                attribute.Value = "7";
                node.Attributes.Append(attribute);
                attribute = xmlDoc.CreateAttribute("ind2");
                attribute.Value = "2";
                node.Attributes.Append(attribute);
                MarcRecord.AppendChild(node);

                subpdf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("code");
                attribute.Value = "d";
                subpdf.Attributes.Append(attribute);
                subpdf.InnerText = path;
                node.AppendChild(subpdf);
                subpdf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("code");
                attribute.Value = "f";
                subpdf.Attributes.Append(attribute);
                subpdf.InnerText = "book.pdf";
                node.AppendChild(subpdf);
                subpdf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("code");
                attribute.Value = "y";
                subpdf.Attributes.Append(attribute);
                subpdf.InnerText = "file";
                node.AppendChild(subpdf);
                subpdf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
                attribute = xmlDoc.CreateAttribute("code");
                attribute.Value = "u";
                subpdf.Attributes.Append(attribute);
                subpdf.InnerText = path + "/book.pdf";
                node.AppendChild(subpdf);
            }

            node = xmlDoc.CreateElement("marc", "datafield", "http://www.loc.gov/MARC21/slim");
            attribute = xmlDoc.CreateAttribute("tag");
            attribute.Value = "371";
            node.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("ind1");
            attribute.Value = "0";
            node.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("ind2");
            attribute.Value = " ";
            node.Attributes.Append(attribute);
            MarcRecord.AppendChild(node);
            subpdf = xmlDoc.CreateElement("marc", "subfield", "http://www.loc.gov/MARC21/slim");
            attribute = xmlDoc.CreateAttribute("code");
            attribute.Value = "a";
            subpdf.Attributes.Append(attribute);
            subpdf.InnerText = ForAllReader ? "0" : "1";
            node.AppendChild(subpdf);



            return RecordNode;
        }
    }
}
