using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Xml;

/// <summary>
/// Сводное описание для XMLConnections
/// </summary>
public class XmlConnections
{
    private static String filename = System.AppDomain.CurrentDomain.BaseDirectory + "DBConnections.xml";
    private static XmlDocument doc;
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

