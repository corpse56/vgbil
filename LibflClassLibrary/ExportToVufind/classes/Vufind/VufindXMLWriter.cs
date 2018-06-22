using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ExportBJ_XML.classes;
using System.Data;
using ExportBJ_XML.classes.DB;
using System.Security;
using ExportBJ_XML.classes.BJ.Vufind;
using System.Reflection;
using Utilities;

namespace LibflClassLibrary.ExportToVufind.classes.BJ
{
    public class VufindXMLWriter
    {
        private XmlWriter _objXmlWriter;
        private XmlDocument _exportDocument;
        private XmlNode _doc;
        private XmlNode _root;
        private string Fund { get; set; }

        public VufindXMLWriter(string fund)
        {
            this.Fund = fund;
            _exportDocument = new XmlDocument();

        }


        public void StartVufindXML(string path)
        {
            _objXmlWriter = XmlTextWriter.Create(path);

            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");
        }
        public void WriteSingleRecord(VufindDoc vfDoc)
        {
            vfDoc.SpecialProcessing();

            _objXmlWriter = XmlTextWriter.Create(@"F:\import\singleRecords\"+ vfDoc.id + ".xml");

            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");

            this.AppendVufindDoc(vfDoc);


            //_doc.WriteTo(_objXmlWriter);
            //_doc = _exportDocument.CreateElement("doc");
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }
        public void AppendVufindDoc(VufindDoc vfDoc)
        {
            if (vfDoc == null) return;
            _doc = vfDoc.CreateExportXmlNode();
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
        }


        //private void AddMultipleValueField(VufindField field)
        //{
        //    foreach (string val in field.ValueList)
        //    {
        //        string fieldName = Extensions.GetMemberName((VufindField c) => c.ValueList);
        //        string propertyName = Extensions.GetMemberName((VufindField c) => c.FieldName);
        //    }
        //}


        public void FinishWriting()
        {
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

       
    }
}
