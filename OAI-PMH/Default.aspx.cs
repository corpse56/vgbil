using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class _Default : System.Web.UI.Page 
{
    SqlDataAdapter DA,DAT;
    DataSet DS;
    DateTime from, until;
    string BAZA;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        //?verb=ListRecords&from=2015-03-05&metadataPrefix=marc21&until=2015-03-05
        string verb="";
        if (Request["verb"] != null)
        {
            verb = Request["verb"].ToLower();
        }
        if ((verb != "listrecords") && (verb != "getrecord") && (verb != "identify") && (verb != "listidentifiers"))
        {
            badVerb();
            return;
        }
        foreach (string param in Request.QueryString.AllKeys)
        {
            string par=param.ToLower();
            if ((par != "resumptiontoken") &&
                (par != "from") &&
                (par != "verb") &&
                (par != "until") &&
                (par != "identifier") &&
                (par != "identify") &&
                (par != "metadataprefix"))
            {
                badArgument();
                return;
            }
        }


        from = new DateTime(1, 1, 1);
        until = new DateTime(1,1,1);
        XmlDocument doc=new XmlDocument();
        bool noRecordsMatch = false;
        switch (verb)
        {
            case "listrecords":

                switch (CheckListArguments())
                {
                    case CheckResult.badArgument:
                        badArgument();
                        return;
                    case CheckResult.badFormat:
                        cannotDisseminateFormat();
                        return;
                    case CheckResult.badResumptionToken:
                        badResumptionToken();
                        return;
                    case CheckResult.FirstRequest:
                        doc = ListRecords(true, "", out noRecordsMatch);//true - первый раз,false - по токену token
                        if (noRecordsMatch)
                        {
                            noRecordsMatchFunc();
                            return;
                        }
                        break;
                    case CheckResult.ResumeRequest:
                        doc = ListRecords(false, Request["resumptiontoken"].ToLower(), out noRecordsMatch);
                        if (noRecordsMatch)
                        {
                            noRecordsMatchFunc();
                            return;
                        }
                        break;
                }
                break;
            case "listidentifiers":
                switch (CheckListArguments())
                {
                    case CheckResult.badArgument:
                        badArgument();
                        return;
                    case CheckResult.badFormat:
                        cannotDisseminateFormat();
                        return;
                    case CheckResult.badResumptionToken:
                        badResumptionToken();
                        return;
                    case CheckResult.FirstRequest:
                        doc = ListIdentifiers(true, "",out noRecordsMatch);//true - первый раз,false - по токену token
                        if (noRecordsMatch)
                        {
                            noRecordsMatchFunc();
                            return;
                        }
                        break;
                    case CheckResult.ResumeRequest:
                        doc = ListIdentifiers(false, Request["resumptiontoken"].ToLower(),out noRecordsMatch);
                        if (noRecordsMatch)
                        {
                            noRecordsMatchFunc();
                            return;
                        }
                        break;
                }
                break;
            case "getrecord":
                if (Request["metadataPrefix"] == null)
                {
                    badArgument();
                    return;// CheckResult.badArgument;
                }
                if (Request["metadataPrefix"] != "marc21")
                {
                    cannotDisseminateFormat();
                    return;// CheckResult.badFormat;
                }
                if (Request["identifier"] == null)
                {
                    badArgument();
                    return;
                }
                string identifier = Request["identifier"];
                if (!RecordExist(identifier))
                {
                    idDoesNotExist();
                    return;
                }

                doc = GetRecord(identifier);

                break;
            case "identify":
                doc = Identify();
                break;

        }


        Response.Clear(); 
        Response.ContentType = "text/xml"; 
        Response.ContentEncoding = System.Text.Encoding.UTF8; 
        doc.Save(Response.Output);
        Response.End();
    }

    private void noRecordsMatchFunc()
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
        attribute.Value = "noRecordsMatch";
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End();
    }
    private void idDoesNotExist()
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
        attribute.Value = "idDoesNotExist";
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End();
    }

    private bool RecordExist(string identifier)
    {
        //identifier = identifier.Substring(13);//oai:aleph.nlr.ru:BJVVV1410763//oai:libfl.ru:BJVVV1410763
        string  IDMAIN;
        DS = new DataSet();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        if (identifier[0] == 'B')
        {
            IDMAIN = identifier.Substring(5);
            DA.SelectCommand.CommandText = "select * from  BJVVV..MAIN where ID = " + IDMAIN;
        }
        else
        {
            IDMAIN = identifier.Substring(8);
            DA.SelectCommand.CommandText = "select * from  REDKOSTJ..MAIN where ID = " + IDMAIN;
        }
        int i = DA.Fill(DS, "rec");
        return (i == 0) ? false : true;
    }
    private XmlDocument Identify()
    {
//<OAI-PMH xmlns="http://www.openarchives.org/OAI/2.0/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.openarchives.org/OAI/2.0/ http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd">
//<responseDate>2015-11-30T16:08:57Z</responseDate>
//<request>http://aleph.nlr.ru/OAI</request>
//<Identify>
//<repositoryName>NLR Repository</repositoryName>
//<baseURL>http://aleph.nlr.ru/OAI</baseURL>
//<protocolVersion>2.0</protocolVersion>
//<adminEmail>alla@nlr.ru</adminEmail>
//<earliestDatestamp>2014-03-30T18:09:51Z</earliestDatestamp>
//<deletedRecord>transient</deletedRecord>
//<granularity>YYYY-MM-DDThh:mm:ssZ</granularity>
//<description>
//<oai-identifier xmlns="http://www.openarchives.org/OAI/2.0/oai-identifier" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.openarchives.org/OAI/2.0/oai-identifier http://www.openarchives.org/OAI/2.0/oai-identifier.xsd">
//<scheme>oai</scheme>
//<repositoryIdentifier>aleph.nlr.ru</repositoryIdentifier>
//<delimiter>:</delimiter>
//<sampleIdentifier>oai:aleph.nlr.ru:NLR01-000000001</sampleIdentifier>
//</oai-identifier>
//</description>
//</Identify>
//</OAI-PMH>
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

        XmlNode IdentifyNode = xmlDoc.CreateElement("Identify");
        rootNode.AppendChild(IdentifyNode);

        node = xmlDoc.CreateElement("repositoryName");
        node.InnerText = "LIBFL Repository";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("baseURL");
        node.InnerText = "http://80.250.173.145/OAI-PMH/default.aspx";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("protocolVersion");
        node.InnerText = "2.0";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("adminEmail");
        node.InnerText = "support@libfl.ru";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("earliestDatestamp");
        node.InnerText = "2009-05-02T22:16:41Z";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("deletedRecord");
        node.InnerText = "no";
        IdentifyNode.AppendChild(node);

        node = xmlDoc.CreateElement("granularity");
        node.InnerText = "YYYY-MM-DDThh:mm:ssZ";
        IdentifyNode.AppendChild(node);

        XmlNode descriptionNode = xmlDoc.CreateElement("description");
        IdentifyNode.AppendChild(descriptionNode);


        XmlNode oai_identifier = xmlDoc.CreateElement("oai-identifier");
        attribute = xmlDoc.CreateAttribute("xmlns");
        attribute.Value = "http://www.openarchives.org/OAI/2.0/";
        oai_identifier.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("xmlns:xsi");
        attribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
        oai_identifier.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        attribute.Value = "http://www.openarchives.org/OAI/2.0/ http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd";
        oai_identifier.Attributes.Append(attribute);
        descriptionNode.AppendChild(oai_identifier);

        node = xmlDoc.CreateElement("scheme");
        node.InnerText = "oai";
        oai_identifier.AppendChild(node);

        node = xmlDoc.CreateElement("repositoryIdentifier");
        node.InnerText = "libfl.ru";
        oai_identifier.AppendChild(node);

        node = xmlDoc.CreateElement("delimiter");
        node.InnerText = ":";
        oai_identifier.AppendChild(node);

        node = xmlDoc.CreateElement("sampleIdentifier");
        node.InnerText = "oai:libfl.ru:BJVVV123";
        oai_identifier.AppendChild(node);

        return xmlDoc;
    }
    private XmlDocument GetRecord(string identifier)
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
        attribute = xmlDoc.CreateAttribute("identifier");
        attribute.Value = Request["identifier"];
        node.Attributes.Append(attribute);
        attribute = xmlDoc.CreateAttribute("metadataPrefix");
        attribute.Value = Request["metadataPrefix"];
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        XmlNode GetRecord = xmlDoc.CreateElement("GetRecord");
        rootNode.AppendChild(GetRecord);

        //identifier = identifier.Substring(13);//oai:aleph.nlr.ru:BJVVV1410763//oai:libfl.ru:BJVVV1410763
        string BAZA, IDMAIN;
        DS = new DataSet();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        if (identifier[0] == 'B')
        {
            BAZA = "BJVVV";
            IDMAIN = identifier.Substring(5);
            DA.SelectCommand.CommandText = "select * from  BJVVV..MAIN where ID = " + IDMAIN;
        }
        else
        {
            BAZA = "REDKOSTJ";
            IDMAIN = identifier.Substring(8);
            DA.SelectCommand.CommandText = "select * from  REDKOSTJ..MAIN where ID = " + IDMAIN;
        }
        int i = DA.Fill(DS, "rec");
        DateTime datestamp = Convert.ToDateTime(DS.Tables["rec"].Rows[0]["DateChange"]);
        XmlNode RecordNode = AppendRecordNode(xmlDoc, IDMAIN, BAZA, datestamp,GetRecord);
        GetRecord.AppendChild(RecordNode);
        return xmlDoc;


    }


    private enum CheckResult {FirstRequest,ResumeRequest,badArgument,badResumptionToken,badFormat }
    private CheckResult CheckListArguments()
    {
        if ((Request["from"] == null) && (Request["until"] == null) && (Request["resumptiontoken"] == null))
        {
            //badArgument();
            return CheckResult.badArgument;
        }
        if ((Request["from"] == null) && (Request["until"] == null) && (Request["resumptiontoken"] != null))
        {
            DS = new DataSet();
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand();
            DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
            DA.SelectCommand.CommandText = "select * from EXPORTNEB..PINSFORREQUEST where TOKEN = '" + Request["resumptiontoken"] + "'";
            int i = DA.Fill(DS, "tok");
            if (i == 0)
            {
                //badResumptionToken();
                return CheckResult.badResumptionToken;
            }

            //doc = ListRecords(false, Request["resumptiontoken"].ToLower());
            //Response.Clear();
            //Response.ContentType = "text/xml";
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //doc.Save(Response.Output);
            //Response.End();
            return CheckResult.ResumeRequest;
        }
        if ((Request["from"] != null) && (Request["until"] == null) && (Request["resumptiontoken"] == null))
        {
            bool gooddate = DateTime.TryParse(Request["from"], out from);
            if (!gooddate)
            {
                //badArgument();
                return CheckResult.badArgument;
            }
            until = DateTime.Today;
        }
        if ((Request["from"] != null) && (Request["until"] == null) && (Request["resumptiontoken"] != null))
        {
            badArgument();
        }
        if ((Request["from"] == null) && (Request["until"] != null) && (Request["resumptiontoken"] == null))
        {
            bool gooddate = DateTime.TryParse(Request["until"], out until);
            if (!gooddate)
            {
                //badArgument();
                return CheckResult.badArgument;
            }
            from = new DateTime(2008, 12, 31);
        }
        if ((Request["from"] == null) && (Request["until"] != null) && (Request["resumptiontoken"] != null))
        {
            //badArgument();
            return CheckResult.badArgument;
        }
        if ((Request["from"] != null) && (Request["until"] != null) && (Request["resumptiontoken"] == null))
        {
            bool gooddate = DateTime.TryParse(Request["from"], out from);
            if (!gooddate)
            {
                //badArgument();
                return CheckResult.badArgument;
            }
            gooddate = DateTime.TryParse(Request["until"], out until);
            if (!gooddate)
            {
                //badArgument();
                return CheckResult.badArgument;
            }
        }
        if ((Request["from"] != null) && (Request["until"] != null) && (Request["resumptiontoken"] != null))
        {
            //badArgument();
            return CheckResult.badArgument;
        }
        if (from > until)
        {
            //badArgument();
            return CheckResult.badArgument;
        }

        if (Request["metadataPrefix"] == null)
        {
            //badArgument();
            return CheckResult.badArgument;
        }
        if (Request["metadataPrefix"] != "marc21")
        {
            //cannotDisseminateFormat();
            return CheckResult.badFormat;
        }
        return CheckResult.FirstRequest;
    }

    private void cannotDisseminateFormat()
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
        attribute.Value = "cannotDisseminateFormat";
        node.Attributes.Append(attribute);
        node.InnerText = "unsupported metadataPrefix";
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End();
    }

    private void badResumptionToken()
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
        attribute.Value = "badResumptionToken";
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End();
    }

    private void badArgument()
    {
            //<?xml version="1.0" encoding="UTF-8"?>
            //<OAI-PMH xmlns="http://www.openarchives.org/OAI/2.0/" 
            //         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
            //         xsi:schemaLocation="http://www.openarchives.org/OAI/2.0/
            //         http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd">
            //  <responseDate>2002-06-01T19:20:30Z</responseDate> 
            //  <request verb="ListRecords" from="2002-06-01T02:00:00Z"
            //           until="2002-06-01T03:020:00Z"
            //           metadataPrefix="oai_marc">
            //           http://memory.loc.gov/cgi-bin/oai</request>
            //  <error code="badArgument"/>
            //</OAI-PMH>    
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
        attribute.Value = "badArgument";
        node.Attributes.Append(attribute);
        rootNode.AppendChild(node);

        Response.Clear();
        Response.ContentType = "text/xml";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output);
        Response.End(); 
    }

    private void badVerb()
    {
        //<?xml version="1.0" encoding="UTF-8"?>
        //<OAI-PMH xmlns="http://www.openarchives.org/OAI/2.0/" 
        //         xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
        //         xsi:schemaLocation="http://www.openarchives.org/OAI/2.0/
        //         http://www.openarchives.org/OAI/2.0/OAI-PMH.xsd">
        //  <responseDate>2002-05-01T09:18:29Z</responseDate>
        //  <request>http://arXiv.org/oai2</request>
        //  <error code="badVerb">Illegal OAI verb</error>
        //</OAI-PMH>
        
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
        attribute.Value = "badVerb";
        node.InnerText = "Illegal OAI verb";
        rootNode.AppendChild(node);

        Response.Clear(); 
        Response.ContentType = "text/xml"; 
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        xmlDoc.Save(Response.Output); 
        Response.End(); 
        //return xmlDoc;

    }
    private XmlDocument ListIdentifiers(bool IsFirst, string ResumptionToken,out bool noRecordsMatch)
    {
        noRecordsMatch = false;
        int IDRequest;
        DataTable Records;
        if (IsFirst)
        {
            IDRequest = InsertSessionPINS();
            Records = GetPinsByToken(IDRequest + "token1");
        }
        else
        {
            Records = GetPinsByToken(ResumptionToken);
            IDRequest = int.Parse(ResumptionToken.Substring(0, ResumptionToken.IndexOf("token"))); ;
        }
        if (Records.Rows.Count == 0)
        {
            noRecordsMatch = true;
            return new XmlDocument();
        }
        int numtoken;
        if (ResumptionToken != "")
        {
            numtoken = int.Parse(ResumptionToken.Substring(ResumptionToken.IndexOf("token") + 5));
        }
        string lasttoken = GetLastToken(IDRequest);
        string currToken = Records.Rows[0]["TOKEN"].ToString();
        if (lasttoken == currToken)
        {
            ResumptionToken = "";//последний резумптионтокен
        }
        else
        {
            ResumptionToken = Records.Rows[0]["NEXTTOKEN"].ToString();
        }
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
        if (IsFirst)
        {
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
        }
        else
        {
            node = xmlDoc.CreateElement("request");
            attribute = xmlDoc.CreateAttribute("verb");
            attribute.Value = Request["verb"];
            node.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("resumptiontoken");
            attribute.Value = Request["resumptiontoken"];
            node.Attributes.Append(attribute);

            rootNode.AppendChild(node);
        }
        XmlNode ListIdentifiers = xmlDoc.CreateElement("ListIdentifiers");
        rootNode.AppendChild(ListIdentifiers);

        foreach (DataRow r in Records.Rows)
        {

            //RMCONVERT rm = new RMCONVERT(r["BAZA"].ToString());
            //rm.FormRUSM(Convert.ToInt32(r["IDMAIN"]));
            //DS = new DataSet();
            //DA = new SqlDataAdapter();
            //DA.SelectCommand = new SqlCommand();
            //DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base01"));
            //DA.SelectCommand.CommandText = "select distinct MET,IND1,IND2,IDBLOCK from TECHNOLOG_VVV..RUSM where IDMAIN = " + r["IDMAIN"].ToString();
            //int i = DA.Fill(DS, "rusm");

            XmlNode HeaderNode = xmlDoc.CreateElement("header");
            ListIdentifiers.AppendChild(HeaderNode);

            node = xmlDoc.CreateElement("identifier");
            node.InnerText = "oai:libfl.ru:" + r["BAZA"].ToString() + r["IDMAIN"].ToString();
            HeaderNode.AppendChild(node);
            node = xmlDoc.CreateElement("datestamp");
            node.InnerText = Convert.ToDateTime(r["DATESTAMP"]).ToString("yyyy-MM-ddTHH:mm:ssZ");//"2015-03-05T02:04:40Z");
            HeaderNode.AppendChild(node);

        }

        node = xmlDoc.CreateElement("resumptionToken");
        node.InnerText = ResumptionToken;
        ListIdentifiers.AppendChild(node);



        //<request verb="ListRecords" from="2015-03-05" until="2015-03-05" metadataPrefix="marc21">http://aleph.nlr.ru/OAI</request>
        //<request metadataPrefix="marc21" from="2015-04-14" until="2015-05-31" verb="ListRecords">http://aleph.nlr.ru/OAI</request>

        return xmlDoc;
        
    }
    private XmlDocument ListRecords(bool IsFirst,string ResumptionToken,out bool noRecordsMatch)
    {
        noRecordsMatch = false;
        int IDRequest;
        DataTable Records;
        if (IsFirst)
        {
            IDRequest = InsertSessionPINS();
            Records = GetPinsByToken(IDRequest+"token1");
        }
        else
        {
            Records = GetPinsByToken(ResumptionToken);
            IDRequest = int.Parse(ResumptionToken.Substring(0,ResumptionToken.IndexOf("token")));;
        }
        if (Records.Rows.Count == 0)
        {
            noRecordsMatch = true;
            return new XmlDocument();
        }
        int numtoken;
        if (ResumptionToken != "")
        {
            numtoken = int.Parse(ResumptionToken.Substring(ResumptionToken.IndexOf("token") + 5));
        }
        string lasttoken = GetLastToken(IDRequest);
        string currToken = Records.Rows[0]["TOKEN"].ToString();
        if (lasttoken == currToken)
        {
            ResumptionToken = "";//последний резумптионтокен
        }
        else
        {
            ResumptionToken = Records.Rows[0]["NEXTTOKEN"].ToString();
        }
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

        if (IsFirst)
        {
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
        }
        else
        {
            node = xmlDoc.CreateElement("request");
            attribute = xmlDoc.CreateAttribute("verb");
            attribute.Value = Request["verb"];
            node.Attributes.Append(attribute);
            attribute = xmlDoc.CreateAttribute("resumptiontoken");
            attribute.Value = Request["resumptiontoken"];
            node.Attributes.Append(attribute);

            rootNode.AppendChild(node);
        }

        XmlNode ListRecords = xmlDoc.CreateElement("ListRecords");
        rootNode.AppendChild(ListRecords);

        foreach (DataRow r in Records.Rows)
        {
            
            XmlNode RecordNode = AppendRecordNode(xmlDoc,r["IDMAIN"].ToString(),r["BAZA"].ToString(),Convert.ToDateTime(r["datestamp"]),ListRecords);
            if (RecordNode == null)
            {
                continue;
            }
            ListRecords.AppendChild(RecordNode);

        }

        node = xmlDoc.CreateElement("resumptionToken");
        node.InnerText = ResumptionToken;
        ListRecords.AppendChild(node);



        //<request verb="ListRecords" from="2015-03-05" until="2015-03-05" metadataPrefix="marc21">http://aleph.nlr.ru/OAI</request>
        //<request metadataPrefix="marc21" from="2015-04-14" until="2015-05-31" verb="ListRecords">http://aleph.nlr.ru/OAI</request>

        return xmlDoc;
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

    private XmlNode AppendRecordNode(XmlDocument xmlDoc,string IDMAIN,string BAZA,DateTime datestamp,XmlNode VerbNode)
    {
        RMCONVERT rm = new RMCONVERT(BAZA);
        try
        {
            rm.FormRUSM(Convert.ToInt32(IDMAIN));
        }
        catch (Exception ex)
        {
            //convertError();
            return null;

        }
        DS = new DataSet();
        DA = new SqlDataAdapter();
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
                path = "000000" + path;
                break;
            case 2:
                path = "00000" + path;
                break;
            case 3:
                path = "0000" + path;
                break;
            case 4:
                path = "000" + path;
                break;
            case 5:
                path = "00" + path;
                break;
            case 6:
                path = "0" + path;
                break;
        }
        if (rm.BAZA == "BJVVV")
        {
            path = "/mnt/fs-share/BJVVV/" + path[0] + @"/" + path[1] + path[2] + path[3] + @"/" + path[4] + path[5] + path[6]+@"/PDF_A";
        }
        else
        {
            path = "/mnt/fs-share/REDKOSTJ/" + path[0] + @"/" + path[1] + path[2] + path[3] + @"/" + path[4] + path[5] + path[6] + @"/PDF_A";
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
        subpdf.InnerText = ForAllReader ?  "0"  :  "1";
        node.AppendChild(subpdf);



        return RecordNode;
    }

    private string GetLastToken(int IDRequest)
    {
        DS = new DataSet();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.SelectCommand.CommandText = "select distinct TOKEN from EXPORTNEB..PINSFORREQUEST where IDREQUEST = " + IDRequest;
        int i = DA.Fill(DS, "pinsforrequest");
        string token = DS.Tables["pinsforrequest"].Rows[0]["TOKEN"].ToString();
        int max = int.Parse(token.Substring(token.IndexOf("token")+5));

        foreach (DataRow r in DS.Tables["pinsforrequest"].Rows)
        {
            string nexttoken = r["TOKEN"].ToString();
            int next = int.Parse(nexttoken.Substring(nexttoken.IndexOf("token")+5));

            if (max < next)
            {
                max = next;
            }
        }
        return IDRequest.ToString()+"token"+max.ToString();
    }

    private DataTable GetPinsByToken(string token)
    {
        DS = new DataSet();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.SelectCommand.CommandText = "select * from EXPORTNEB..PINSFORREQUEST where TOKEN = '"+token+"'";
        int i = DA.Fill(DS, "pinsforrequest");
        return DS.Tables["pinsforrequest"];
    }

    private int InsertSessionPINS()
    {
        DS = new DataSet();
        DA = new SqlDataAdapter();
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.SelectCommand.Parameters.Add("SESSIONID", SqlDbType.NVarChar);
        DA.SelectCommand.Parameters.Add("HOSTIP", SqlDbType.NVarChar);
        DA.SelectCommand.Parameters.Add("VERB", SqlDbType.NVarChar);
        DA.SelectCommand.Parameters["SESSIONID"].Value = Session.SessionID;
        DA.SelectCommand.Parameters["HOSTIP"].Value = Request.UserHostAddress;
        DA.SelectCommand.Parameters["VERB"].Value = Request["verb"];

        DA.SelectCommand.CommandText = "select * from EXPORTNEB..CURRENTREQUESTS where verb = @VERB and SESSIONID = @SESSIONID and HOSTIP = @HOSTIP";
        int i = DA.Fill(DS, "sess");
        DA.SelectCommand.Parameters.Clear();
        if (i != 0)
        {
            DA.InsertCommand = new SqlCommand();
            DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
            DA.InsertCommand.CommandText = "insert into EXPORTNEB..REQUESTSHISTORY (SESSIONID,HOSTIP,VERB,FROMDATE,UNTILDATE,REQUESTDATE,FIRSTTOKEN,TOKENEXPIRE,COMPLETELISTSIZE) " +
                                           " select SESSIONID,HOSTIP,VERB,FROMDATE,UNTILDATE,REQUESTDATE,FIRSTTOKEN,TOKENEXPIRE,COMPLETELISTSIZE from EXPORTNEB..CURRENTREQUESTS where ID = " + DS.Tables["sess"].Rows[0]["ID"].ToString();
            DA.InsertCommand.Connection.Open();
            DA.InsertCommand.ExecuteNonQuery();
            DA.InsertCommand.Connection.Close();

            DA.DeleteCommand = new SqlCommand();
            DA.DeleteCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
            DA.DeleteCommand.CommandText = "delete from EXPORTNEB..CURRENTREQUESTS where ID = "+DS.Tables["sess"].Rows[0]["ID"].ToString()+"; "+
                                           "delete from EXPORTNEB..PINSFORREQUEST where IDREQUEST = "+DS.Tables["sess"].Rows[0]["ID"].ToString() ;
            DA.DeleteCommand.Connection.Open();
            DA.DeleteCommand.ExecuteNonQuery();
            DA.DeleteCommand.Connection.Close();

        }
        DA.InsertCommand = new SqlCommand();
        DA.InsertCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.InsertCommand.Parameters.Clear();
        DA.InsertCommand.Parameters.Add("SESSIONID", SqlDbType.NVarChar);
        DA.InsertCommand.Parameters.Add("HOSTIP", SqlDbType.NVarChar);
        DA.InsertCommand.Parameters.Add("VERB", SqlDbType.NVarChar);
        DA.InsertCommand.Parameters.Add("FROMDATE", SqlDbType.DateTime);
        DA.InsertCommand.Parameters.Add("UNTILDATE", SqlDbType.DateTime);
        DA.InsertCommand.Parameters.Add("REQUESTDATE", SqlDbType.DateTime);
        DA.InsertCommand.Parameters.Add("FIRSTTOKEN", SqlDbType.NVarChar);
        DA.InsertCommand.Parameters.Add("TOKENEXPIRE", SqlDbType.DateTime);
        DA.InsertCommand.Parameters.Add("COMPLETELISTSIZE", SqlDbType.Int);

        DA.InsertCommand.Parameters["SESSIONID"].Value = Session.SessionID;
        DA.InsertCommand.Parameters["HOSTIP"].Value = Request.UserHostAddress;
        DA.InsertCommand.Parameters["VERB"].Value = Request["verb"];
        DA.InsertCommand.Parameters["FROMDATE"].Value = from;
        DA.InsertCommand.Parameters["UNTILDATE"].Value = until;
        DA.InsertCommand.Parameters["REQUESTDATE"].Value = DateTime.Now;
        DA.InsertCommand.Parameters["FIRSTTOKEN"].Value = "1";
        DA.InsertCommand.Parameters["TOKENEXPIRE"].Value = DateTime.Now.AddDays(7);
        DA.InsertCommand.Parameters["COMPLETELISTSIZE"].Value = -1;

        DA.InsertCommand.CommandText = "insert into EXPORTNEB..CURRENTREQUESTS (SESSIONID,HOSTIP,VERB,FROMDATE,UNTILDATE,REQUESTDATE,FIRSTTOKEN,TOKENEXPIRE,COMPLETELISTSIZE) " +
                                       " values (@SESSIONID,@HOSTIP,@VERB,@FROMDATE,@UNTILDATE,@REQUESTDATE,@FIRSTTOKEN,@TOKENEXPIRE,@COMPLETELISTSIZE); select scope_identity()";
        DA.InsertCommand.Connection.Open();
        object o = DA.InsertCommand.ExecuteScalar();
        int IDRequest = Convert.ToInt32(o);
        DA.InsertCommand.Connection.Close();



        DA.InsertCommand.Parameters.Clear();
        DA.InsertCommand.CommandText =
            " with A as ( "+
        //SELECT IDMAIN,'BJVVV' baza  FROM BJVVV..DATAEXT A where IDMAIN in (1365206,1365214,1365215,1365225,1365351,1365357,1365372) union all " +
           "  SELECT DISTINCT IDMAIN,'BJVVV' baza,B.DateChange datestamp  FROM BJVVV..DATAEXT A" +
           "     LEFT join BJVVV..MAIN B on A.IDMAIN = B.ID " +
           "     WHERE SORT = 'Длявыдачи' AND MNFIELD=921 AND MSFIELD='$c'  " +
           "     AND IDMAIN NOT IN  " +
           "    (SELECT IDMAIN FROM  BJVVV..DATAEXT " +
           "     WHERE SORT = 'Учетнаязапись' AND MNFIELD=899 AND MSFIELD='$x')       " +
           "  union all " +
           "  SELECT DISTINCT IDMAIN ,'REDKOSTJ' baza,C.DateChange datestamp FROM REDKOSTJ..DATAEXT A" +
           "     LEFT join REDKOSTJ..MAIN C on A.IDMAIN = C.ID " +
           "     WHERE SORT = 'Длявыдачи' AND MNFIELD=921 AND MSFIELD='$c'  " +
           "     AND IDMAIN NOT IN  " +
           "    (SELECT IDMAIN FROM  REDKOSTJ..DATAEXT " +
           "     WHERE SORT = 'Учетнаязапись' AND MNFIELD=899 AND MSFIELD='$x') " +
           "  ), " +
           "  B as ( " +
           "  select row_number() over (order by A.IDMAIN) num ,A.IDMAIN,baza, " +
           "     datestamp " +
           "     from A " +
           "  ) " +
           "  insert into EXPORTNEB..PINSFORREQUEST ([CURSOR],IDREQUEST, IDMAIN,BAZA,DATESTAMP,TOKEN,NEXTTOKEN) " +
           "  select num," + IDRequest + ",IDMAIN,baza,datestamp, '" + IDRequest + "'+'token' + cast(((row_number() over(order by num) - 1) / 30) + 1 as nvarchar(100)) as TOKEN, " +
           " '"+IDRequest+"'+'token' + cast(((row_number() over(order by num) - 1) / 30) + 2 as nvarchar(100)) as NEXTTOKEN " +
           "  from B where CAST(CAST(datestamp AS date) AS datetime) between '" + from.ToString("yyyyMMdd") + "' and '" + until.ToString("yyyyMMdd") + "' order by datestamp";
        //DA.Fill(DS, "pins");
        DA.InsertCommand.CommandTimeout = 1200;
        DA.InsertCommand.Connection.Open();
        int ListSize = DA.InsertCommand.ExecuteNonQuery();
        DA.InsertCommand.Connection.Close();


        DA.UpdateCommand = new SqlCommand();
        DA.UpdateCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
        DA.UpdateCommand.CommandText = "update EXPORTNEB..CURRENTREQUESTS set FIRSTTOKEN = '" + IDRequest.ToString() + "token1" + "',COMPLETELISTSIZE = "+ListSize+" where ID = " + IDRequest;
        DA.UpdateCommand.Connection.Open();
        DA.UpdateCommand.ExecuteNonQuery();
        DA.UpdateCommand.Connection.Close();
        return IDRequest;


    }
}
