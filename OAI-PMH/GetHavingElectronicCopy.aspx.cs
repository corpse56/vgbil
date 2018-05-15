using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using LibflClassLibrary;

public partial class GetHavingElectronicCopy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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

        node = xmlDoc.CreateElement("request");
        attribute = xmlDoc.CreateAttribute("verb");
        attribute.Value = Request["verb"];
        node.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("from");
        attribute.Value = Request["from"];
        node.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("until");
        attribute.Value = Request["until"];
        node.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("metadataPrefix");
        attribute.Value = Request["metadataPrefix"];
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        XmlNode ListRecords = xmlDoc.CreateElement("ListRecords");
        rootNode.AppendChild(ListRecords);


        DataTable Records;
        DataSet DS = new DataSet();
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.SelectCommand.CommandText = "select IDBook IDMAIN, case when IDBase = 1 then 'BJVVV' else 'REDKOSTJ' end BAZA " +
                                       " from BookAddInf..ScanInfo A " +
                                       " left join BJVVV..MAIN B on A.IDBook = B.ID and A.IDBase = 1" +
                                       " left join REDKOSTJ..MAIN C on A.IDBook = C.ID and A.IDBase = 2" +
                                       " where (A.IDBase = 1 and B.ID is not null or A.IDBase = 2 and C.ID is not null) and A.PDF = 1";
                                       //" where A.IDBook = 5095";
        int i = DA.Fill(DS, "pinsforrequest");
        Records = DS.Tables["pinsforrequest"];

        foreach (DataRow r in Records.Rows)
        {

            XmlNode RecordNode = AppendRecordNode(xmlDoc, r["IDMAIN"].ToString(), r["BAZA"].ToString(), DateTime.Now, ListRecords);
            ListRecords.AppendChild(RecordNode);

        }
        Response.Clear();
        Response.ContentType = "text/xml";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Now + ".xml");
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.Write(xmlDoc.InnerXml);
        Response.End();

        //Response.Clear();
        //Response.ContentType = "text/xml";
        //Response.ContentEncoding = System.Text.Encoding.UTF8;
        //xmlDoc.Save(Response.Output);
        //Response.End();


    }
    private XmlNode AppendRecordNode(XmlDocument xmlDoc, string IDMAIN, string BAZA, DateTime datestamp, XmlNode VerbNode)
    {
        RMCONVERT rm = new RMCONVERT(BAZA);
        //if (IDMAIN != "23083")
        //{
        //    return xmlDoc.CreateElement("record");
        //}

        try
        {
            rm.FormRUSM(Convert.ToInt32(IDMAIN));
        }
        catch (Exception ex)
        {
            convertError();
            return null;

        }
        DataSet DS = new DataSet();
        SqlDataAdapter DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base01"));
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
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
            DA.SelectCommand.CommandText = "select * from [BookAddInf].[dbo].[ScanInfo] where IDBook = " + IDMAIN +
                                           " and IDBASE = 1 and PDF = 1"; ;
            PdfExists = DA.Fill(DS, "pdf");
        }
        else
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
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
            case 9:
                path = path;
                break;
        }
        if (rm.BAZA == "BJVVV")
        {
            path = "/mnt/fs-share/BJVVV/" + path[0] +  path[1] + path[2]+ @"/" + path[3] + path[4] + path[5] + @"/" + path[6] + path[7] + path[8] + @"/PDF_A";
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
    private void convertError()
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
        node = xmlDoc.CreateElement("request");
        node.InnerText = Request.Url.AbsoluteUri;
        rootNode.AppendChild(node);
        node = xmlDoc.CreateElement("error");
        attribute = xmlDoc.CreateAttribute("code");
        attribute.Value = "RUSMARCConvertingError";
        node.InnerText = "RUSMARCConvertingError";
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End();
        //return xmlDoc;

    }


}
