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
        private XmlNode AddField(string name, string val)
        {
            XmlNode field = _exportDocument.CreateElement("field");
            XmlAttribute att = _exportDocument.CreateAttribute("name");
            att.Value = name;
            field.Attributes.Append(att);
            field.InnerText = SecurityElement.Escape(val);
            val = Extensions.XmlCharacterWhitelist(val);
            field.InnerText = val;
            return field;
        }
        public void AppendVufindDoc(VufindDoc vfDoc)
        {
            _doc = CreateXmlNode(vfDoc);
            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
        }

        public XmlNode CreateXmlNode(VufindDoc vfDoc)
        {
            vfDoc.SpecialProcessing();
            Type vfDocType = typeof(VufindDoc);
            XmlNode node;
            foreach (PropertyInfo propertyInfo in vfDocType.GetProperties())
            {
                string pname = propertyInfo.Name;
                VufindField vfField = (VufindField)propertyInfo.GetValue(vfDoc, null);
                foreach (string val in vfField.ValueList)
                {
                    node = AddField(propertyInfo.Name, val);
                    _doc.AppendChild(node);
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
                    node = AddField("Exemplar", fieldInfo.GetValue(vfDoc).ToString());
                    _doc.AppendChild(node);
                    continue;
                }
                if (fieldInfo.Name == "NewArrivals")
                {
                    if (fieldInfo.GetValue(vfDoc) != null)
                    {
                        node = AddField(fieldInfo.Name, ((DateTime)fieldInfo.GetValue(vfDoc)).ToString("yyyy-MM-ddThh:mm:ssZ"));
                        _doc.AppendChild(node);
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                node = AddField(fieldInfo.Name, fieldInfo.GetValue(vfDoc).ToString());
                _doc.AppendChild(node);
            }
            return _doc;
        }

        private void AddMultipleValueField(VufindField field)
        {
            foreach (string val in field.ValueList)
            {
                string fieldName = Extensions.GetMemberName((VufindField c) => c.ValueList);
                string propertyName = Extensions.GetMemberName((VufindField c) => c.FieldName);
            }
        }


        public void FinishWriting()
        {
            _objXmlWriter.Flush();
            _objXmlWriter.Close();
        }

       
    }
}
