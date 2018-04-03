using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
namespace NEBSynchronization
{
    class Program
    {
        static void Main(string[] args)
        {

            /*AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => MessageBox.Show(e.ExceptionObject.ToString());
            */
            StreamWriter fs;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "_log.txt"))
            {
                fs = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "_log.txt");
            }
            else
            {
                fs = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "_log.txt");
            }
            try
            {
                

                fs.WriteLine("=======================================================================");
                fs.WriteLine("=======================================================================");
                fs.WriteLine("Начало синхронизации");
                fs.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                fs.WriteLine();
                Console.WriteLine("Начало подготовки запроса");
                SqlDataAdapter DA;
                DataSet DS = new DataSet();
                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand();
                DA.SelectCommand.Connection = new SqlConnection(XmlConnections.GetConnection("/Connections/base03"));
                DA.SelectCommand.CommandTimeout = 1200;
                DA.SelectCommand.CommandText = "select * from BJVVV..MAIN WHERE ID=1";//File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "bjvvvsync.sql");
                //DA.SelectCommand.CommandText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "bjvvvsync_xml.sql");
                Console.WriteLine("Начало выполнения запроса");

                try
                {
                    int i = DA.Fill(DS, "sync");
                }
                catch (Exception ex)
                {
                    fs.WriteLine(ex.Message);
                    fs.Flush();
                    fs.Close();
                    return;
                }
                Console.WriteLine("Запрос выполнен");

                MySqlDataAdapter damy = new MySqlDataAdapter();
                var MySqlCommand = new MySqlCommand();
                //MySqlCommand.Connection = new MySqlConnection("Server=192.168.4.165;Port=3306;Character Set=utf8;Uid=root;Pwd=kerensky2015112;Database=neb;");
                MySqlCommand.Connection = new MySqlConnection(XmlConnections.GetConnection("/Connections/MySQL"));//new MySqlConnection("Server=192.168.6.211;Port=3306;Character Set=utf8;Uid=root;Pwd=kerensky2015112;Database=neb;");
                MySqlCommand.Connection.Open();
                Console.WriteLine("MySqlCommand.Connection.Open();");
                damy.SelectCommand = new MySqlCommand();
                //damy.SelectCommand.Connection = new MySqlConnection("Server=192.168.4.165;Port=3306;Character Set=utf8;Uid=root;Pwd=kerensky2015112;Database=neb;");
                damy.SelectCommand.Connection = new MySqlConnection(XmlConnections.GetConnection("/Connections/MySQL"));//new MySqlConnection("Server=192.168.6.211;Port=3306;Character Set=utf8;Uid=root;Pwd=kerensky2015112;Database=neb;");
                Console.WriteLine("MySQL подключения открыты. Начало синхронизации");

                foreach (DataRow r in DS.Tables["sync"].Rows)
                {
                    if (DS.Tables["neb"] != null)
                    {
                        while (DS.Tables["neb"].Rows.Count > 0)
                        {
                            DS.Tables["neb"].Rows.RemoveAt(0);
                        }
                    }
                    damy.SelectCommand.CommandText = "select * from tbl_common_biblio_card where IdFromALIS = '" + r["IdFromALIS"].ToString() + "'";
                    int j = 0;
                    try
                    {
                        j = damy.Fill(DS, "neb");
                    }
                    catch (Exception ex)
                    {
                        fs.WriteLine(ex.Message);
                        fs.Flush();
                        fs.Close();
                        return;
                    }
                    if (j == 0)
                    {
                        MySqlCommand.CommandText = "insert into tbl_common_biblio_card (ALIS,IdFromALIS,Author,Title,Responsibility,PublicationInformation, " +
                                                                                            "PublishYear,Publisher,PublishPlace,ISBN,LanguageText,CountPages,Format, " +
                                                                                            "UDKText,Series,VolumeNumber,VolumeName,Collection,AccessType,isLicenseAgreement, " +
                                                                                            "ContentRemark,CommonRemark,Annotation,CreationDateTime,UpdatingDateTime,pdfLink ) " +
                                                                                   "values (@ALIS,@IdFromALIS,@Author,@Title,@Responsibility,@PublicationInformation, " +
                                                                                            "@PublishYear,@Publisher,@PublishPlace,@ISBN,@LanguageText,@CountPages,@Format, " +
                                                                                            "@UDKText,@Series,@VolumeNumber,@VolumeName,@Collection,@AccessType,@isLicenseAgreement, " +
                                                                                            "@ContentRemark,@CommonRemark,@Annotation,@CreationDateTime,@UpdatingDateTime,@pdfLink ) ";
                        MySqlCommand.Parameters.Clear();
                        MySqlCommand.Parameters.Add("ALIS", MySqlDbType.Int32);
                        MySqlCommand.Parameters["ALIS"].Value = r["ALIS"];
                        MySqlCommand.Parameters.Add("IdFromALIS", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["IdFromALIS"].Value = r["IdFromALIS"];
                        MySqlCommand.Parameters.Add("Author", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Author"].Value = r["Author"];
                        MySqlCommand.Parameters.Add("Title", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Title"].Value = r["Title"];
                        MySqlCommand.Parameters.Add("Responsibility", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Responsibility"].Value = r["Responsibility"];
                        MySqlCommand.Parameters.Add("PublicationInformation", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublicationInformation"].Value = r["PublicationInformation"];
                        MySqlCommand.Parameters.Add("PublishYear", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublishYear"].Value = r["PublishYear"];
                        MySqlCommand.Parameters.Add("Publisher", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Publisher"].Value = r["Publisher"];
                        MySqlCommand.Parameters.Add("PublishPlace", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublishPlace"].Value = r["PublishPlace"];
                        MySqlCommand.Parameters.Add("ISBN", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["ISBN"].Value = r["ISBN"];
                        MySqlCommand.Parameters.Add("LanguageText", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["LanguageText"].Value = r["LanguageText"];
                        MySqlCommand.Parameters.Add("CountPages", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["CountPages"].Value = r["CountPages"];
                        MySqlCommand.Parameters.Add("Format", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Format"].Value = r["Format"];
                        MySqlCommand.Parameters.Add("UDKText", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["UDKText"].Value = r["UDKText"];
                        MySqlCommand.Parameters.Add("Series", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Series"].Value = r["Series"];
                        MySqlCommand.Parameters.Add("VolumeNumber", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["VolumeNumber"].Value = r["VolumeNumber"];
                        MySqlCommand.Parameters.Add("VolumeName", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["VolumeName"].Value = r["VolumeName"];
                        MySqlCommand.Parameters.Add("Collection", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Collection"].Value = r["Collection"];
                        MySqlCommand.Parameters.Add("AccessType", MySqlDbType.Int32);
                        MySqlCommand.Parameters["AccessType"].Value = r["AccessType"];
                        MySqlCommand.Parameters.Add("isLicenseAgreement", MySqlDbType.Bit);
                        MySqlCommand.Parameters["isLicenseAgreement"].Value = r["isLicenseAgreement"];
                        MySqlCommand.Parameters.Add("ContentRemark", MySqlDbType.Text);
                        MySqlCommand.Parameters["ContentRemark"].Value = r["ContentRemark"];
                        MySqlCommand.Parameters.Add("CommonRemark", MySqlDbType.Text);
                        MySqlCommand.Parameters["CommonRemark"].Value = r["CommonRemark"];
                        MySqlCommand.Parameters.Add("Annotation", MySqlDbType.Text);
                        MySqlCommand.Parameters["Annotation"].Value = r["Annotation"];
                        MySqlCommand.Parameters.Add("CreationDateTime", MySqlDbType.DateTime);
                        MySqlCommand.Parameters["CreationDateTime"].Value = r["CreationDateTime"];
                        MySqlCommand.Parameters.Add("UpdatingDateTime", MySqlDbType.DateTime);
                        MySqlCommand.Parameters["UpdatingDateTime"].Value = r["CreationDateTime"];
                        MySqlCommand.Parameters.Add("pdfLink", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["pdfLink"].Value = GetPDFLink(r["IdFromALIS"].ToString());




                        try
                        {
                            MySqlCommand.ExecuteNonQuery();
                        }
                        catch
                        {
                            fs.WriteLine("Ошибка при добавлении записи: " + r["IdFromALIS"].ToString());
                            continue;
                        }
                        fs.WriteLine("Добавлено: " + r["IdFromALIS"].ToString());

                    }
                    else
                    {
                        MySqlCommand.CommandText = "update tbl_common_biblio_card set Author=@Author,Title=@Title,Responsibility=@Responsibility,PublicationInformation=@PublicationInformation, " +
                                                                        " PublishYear = @PublishYear,Publisher=@Publisher,PublishPlace = @PublishPlace,ISBN=@ISBN,LanguageText=@LanguageText,CountPages=@CountPages,Format=@Format, " +
                                                                        " UDKText=@UDKText,Series=@Series,VolumeNumber=@VolumeNumber,VolumeName=@VolumeName,Collection=@Collection,AccessType=@AccessType, " +
                                                                        " isLicenseAgreement=@isLicenseAgreement,ContentRemark=@ContentRemark,CommonRemark=@CommonRemark,Annotation=@Annotation, " +
                                                                        " UpdatingDateTime=@UpdatingDateTime,pdfLink=@pdfLink " +
                                                                        " where IdFromALIS = @IdFromALIS  ";
                        MySqlCommand.Parameters.Clear();
                        //MySqlCommand.Parameters.Add("ALIS", MySqlDbType.Int32);
                        //MySqlCommand.Parameters["ALIS"].Value = r["ALIS"];
                        MySqlCommand.Parameters.Add("IdFromALIS", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["IdFromALIS"].Value = r["IdFromALIS"];
                        MySqlCommand.Parameters.Add("Author", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Author"].Value = r["Author"];
                        MySqlCommand.Parameters.Add("Title", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Title"].Value = r["Title"];
                        MySqlCommand.Parameters.Add("Responsibility", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Responsibility"].Value = r["Responsibility"];
                        MySqlCommand.Parameters.Add("PublicationInformation", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublicationInformation"].Value = r["PublicationInformation"];
                        MySqlCommand.Parameters.Add("PublishYear", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublishYear"].Value = r["PublishYear"];
                        MySqlCommand.Parameters.Add("Publisher", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Publisher"].Value = r["Publisher"];
                        MySqlCommand.Parameters.Add("PublishPlace", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["PublishPlace"].Value = r["PublishPlace"];
                        MySqlCommand.Parameters.Add("ISBN", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["ISBN"].Value = r["ISBN"];
                        MySqlCommand.Parameters.Add("LanguageText", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["LanguageText"].Value = r["LanguageText"];
                        MySqlCommand.Parameters.Add("CountPages", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["CountPages"].Value = r["CountPages"];
                        MySqlCommand.Parameters.Add("Format", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Format"].Value = r["Format"];
                        MySqlCommand.Parameters.Add("UDKText", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["UDKText"].Value = r["UDKText"];
                        MySqlCommand.Parameters.Add("Series", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Series"].Value = r["Series"];
                        MySqlCommand.Parameters.Add("VolumeNumber", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["VolumeNumber"].Value = r["VolumeNumber"];
                        MySqlCommand.Parameters.Add("VolumeName", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["VolumeName"].Value = r["VolumeName"];
                        MySqlCommand.Parameters.Add("Collection", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["Collection"].Value = r["Collection"];
                        MySqlCommand.Parameters.Add("AccessType", MySqlDbType.Int32);
                        MySqlCommand.Parameters["AccessType"].Value = r["AccessType"];
                        MySqlCommand.Parameters.Add("isLicenseAgreement", MySqlDbType.Bit);
                        MySqlCommand.Parameters["isLicenseAgreement"].Value = r["isLicenseAgreement"];
                        MySqlCommand.Parameters.Add("ContentRemark", MySqlDbType.Text);
                        MySqlCommand.Parameters["ContentRemark"].Value = r["ContentRemark"];
                        MySqlCommand.Parameters.Add("CommonRemark", MySqlDbType.Text);
                        MySqlCommand.Parameters["CommonRemark"].Value = r["CommonRemark"];
                        MySqlCommand.Parameters.Add("Annotation", MySqlDbType.Text);
                        MySqlCommand.Parameters["Annotation"].Value = r["Annotation"];
                        //MySqlCommand.Parameters.Add("CreationDateTime", MySqlDbType.DateTime);
                        //MySqlCommand.Parameters["CreationDateTime"].Value = r["CreationDateTime"];
                        MySqlCommand.Parameters.Add("UpdatingDateTime", MySqlDbType.DateTime);
                        MySqlCommand.Parameters["UpdatingDateTime"].Value = DateTime.Now;
                        MySqlCommand.Parameters.Add("pdfLink", MySqlDbType.VarChar);
                        MySqlCommand.Parameters["pdfLink"].Value = GetPDFLink(r["IdFromALIS"].ToString());

                        try
                        {
                            MySqlCommand.ExecuteNonQuery();
                        }
                        catch
                        {
                            fs.WriteLine("Ошибка при обновлении записи: " + r["IdFromALIS"].ToString());
                            continue;
                        }
                        fs.WriteLine("Обновлено: " + r["IdFromALIS"].ToString());

                    }

                }
                MySqlCommand.Connection.Close();
                Console.WriteLine("Окончание синхронизации");
                fs.WriteLine("Окончание синхронизации");
                fs.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
                fs.WriteLine("=======================================================================");
                fs.WriteLine("=======================================================================");
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                fs.WriteLine(ex.Message);
                fs.Flush();
                fs.Close();

                //Console.WriteLine(ex.Message);
                //Console.ReadKey();

            }
        }
        private static object GetPDFLink(string IdFromALIS)
        {
            string path = IdFromALIS;
            if (IdFromALIS[0] == 'B')
            {
                path = path.Substring(5);
            }
            else
            {
                path = path.Substring(8);
            }

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
            if (IdFromALIS[0] == 'B')
            {
                path = "/mnt/fs-share/BJVVV/" + path[0] + @"/" + path[1] + path[2] + path[3] + @"/" + path[4] + path[5] + path[6] + @"/book.pdf";
            }
            else
            {
                path = "/mnt/fs-share/REDKOSTJ/" + path[0] + @"/" + path[1] + path[2] + path[3] + @"/" + path[4] + path[5] + path[6]+@"/book.pdf";
            }
            return (object)path;
        }
    
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
