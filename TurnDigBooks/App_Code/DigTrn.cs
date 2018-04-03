using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.IO;

/// <summary>
/// Сводное описание для WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из сценария с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
// [System.Web.Script.Services.ScriptService]
public class DigTrn : System.Web.Services.WebService {

    public DigTrn()
    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }

    
    [WebMethod(Description = "Возвращает статус книги в очереди на оцифровку.<br/><b>Входные параметры:</b> PIN (int) - IDMAIN издания; BAZA (int) : 1 - BJVVV, 2 - REDKOSTJ<br/> "+
                            "<b>Выходной параметр (int):</b>0 - книга не в очереди и не оцифрована; 1 - книга в очереди на оцифровку; 2 - книга уже оцифрована; 3 - ошибка входных данных; 4 - под таким ПИНом нет экземпляра")]
    public int GetBookStatus(int PIN,int BAZA)
    {
        if ((BAZA != 1) && (BAZA != 2)) return 3;
        if (PIN <= 0) return 3;

        SqlDataAdapter DA = new SqlDataAdapter();

        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        if (BAZA == 1)
        {
            DA.SelectCommand.CommandText = "select * " +
                                               " from BJVVV..DATAEXT A " +
                                               " where A.IDMAIN = " + PIN + 
                                               " and exists (select 1 from BJVVV..DATAEXT B where A.IDMAIN = B.IDMAIN and B.MNFIELD = 899 and B.MSFIELD = '$p')";
        }
        else
        {
            DA.SelectCommand.CommandText = "select * " +
                                               " from REDKOSTJ..DATAEXT A " +
                                               " where A.IDMAIN = " + PIN +
                                               " and exists (select 1 from REDKOSTJ..DATAEXT B where A.IDMAIN = B.IDMAIN and B.MNFIELD = 899 and B.MSFIELD = '$p')";
        }
        DataSet DS = new DataSet();
        int i = DA.Fill(DS, "data");
        if (i == 0)
        {
            return 4;
        }
        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.SelectCommand.CommandText = "select * " +
                                           " from BookAddInf..ScanInfo " +
                                           " where IDBook = " + PIN+" and IDBase = "+BAZA;

        DS = new DataSet();
        i = DA.Fill(DS, "data");
        if (i > 0) return 2;
        i = 0;

        DA.SelectCommand = new SqlCommand();
        DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/Turn"));
        DA.SelectCommand.CommandText = "select * " +
                                           " from Reservation_R..TURNTODIGITIZE  " +
                                           " where IDMAIN = " + PIN + " and BAZA = "+BAZA;

        DS = new DataSet();
        i = DA.Fill(DS, "data");
        if (i > 0) return 1;

        return 0;
    }
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

}

