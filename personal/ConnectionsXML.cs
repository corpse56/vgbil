using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;
namespace Pers
{
    public class XmlConnections
    {
        private static String filename = Application.StartupPath + "\\DBConnections.xml";
        private static XmlDocument doc;
        /*public string GetZakazCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/Zakaz");
            return node.InnerText;
        }
        public string GetBRIT_SOVETtestCon()
        {
            XmlNode node = this.doc.SelectSingleNode("/Connections/BRIT_SOVET");
            return node.InnerText;
        }*/
        public static string GetConnection(string s)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("Файл с подключениями 'DBConnections.xml' не найден.");
            }

            try
            {
                doc = new XmlDocument();
                doc.Load(filename);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                throw;
            }
            XmlNode node;
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("Узел " + s + " не найден в файле DBConnections.xml"); ;
            }

            return node.InnerText;
        }
        public XmlConnections()
        {

        }
    }

   
}
