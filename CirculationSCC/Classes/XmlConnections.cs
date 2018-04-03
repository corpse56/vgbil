using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Circulation
{
    public class XmlConnections
    {
        private static String filename = System.AppDomain.CurrentDomain.BaseDirectory + "DBConnections.xml";
        private static XmlDocument doc;
        public static string GetConnection(string s)
        {
            //filename = "F:\\projects\\circulationACC_svn\\trunk\\CirculationACC\\bin\\debug\\dbconnections.xml";
            if (!File.Exists(filename))
            {
                throw new Exception("Файл с подключениями 'DBConnections.xml' не найден."+filename);
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
            XmlNode node = doc.SelectSingleNode(s);
            try
            {
                node = doc.SelectSingleNode(s);
            }
            catch
            {
                throw new Exception("Узел " + s + " не найден в файле DBConnections.xml"); ;
            }
            while (node == null)
            {
                node = doc.SelectSingleNode(s);
            }
            return node.InnerText;
        }
        public XmlConnections()
        {

        }
    }
   

}
