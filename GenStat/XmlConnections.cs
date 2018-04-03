using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace GenStat
{
    public class XmlConnections
    {
        public XmlTextReader reader;
        static String filename = Application.StartupPath + "\\DBConnections.xml";
        public XmlDocument doc;
        public string GetReaderCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/Readers");
            return node.InnerText;
        }
        public string GetZakazCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
            return node.InnerText;
        }
        public string GetBRIT_SOVETCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
            return node.InnerText;
        }
        internal string GetBJVVVCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/BJVVV");
            return node.InnerText;
        }

        public XmlConnections()
        {
            try
            {
                doc = new XmlDocument();
                doc.Load(filename);// (reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
