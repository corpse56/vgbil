using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace OneTimeProjectForGlueGoogleExportXMLs
{
    class Program
    {
        static void Main(string[] args)
        {
            //example with short files
            //string[] inputFiles = {
            //    @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\1.xml",
            //    @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\2.xml"
            //};
            //using (XmlWriter xw = XmlWriter.Create(@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180913_B_ALL.xml"))
            //{
            //    xw.WriteStartDocument();
            //    xw.WriteStartElement("GoogleExport");
            //    foreach (string inputFile in inputFiles)
            //    {
            //        using (XmlReader xr = XmlReader.Create(inputFile))
            //        {
            //            //xr.MoveToContent();
            //            //xr.ReadToFollowing("GoogleExport");
            //            while (xr.ReadToFollowing("record"))
            //            {
            //                xw.WriteNode(xr, true);
            //            }
            //        }
            //    }
            //    xw.WriteEndElement();
            //    xw.WriteEndDocument();
            //}
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //XDocument xdoc = XDocument.Load(@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180919-04.03_S.xml");
            //XDocument xdoc1 = XDocument.Load(@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180913_B_ALL.xml");

            //var records = xdoc.Descendants("record").Count();
            //var records1 = xdoc1.Descendants("record").Count();


            string[] inputFiles = {

                 //"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180919-04.03_S.xml",
                 //@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\1.xml",
                 //@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\2.xml",
                @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180916-04.24_B.xml",
                @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180913-04.43_B.xml",
                @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180914-11.28_B.xml",
                @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180914-12.29_B.xml",
                @"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180915-02.51_B.xml",
            };

            using (XmlWriter xw = XmlWriter.Create(@"f:\google export\20180919-20180919T145955Z-001\20180919\BJ2M21GOOGL_B\GoogleExport20180913_B_ALL.xml"))
            {
                
                xw.WriteStartDocument();
                xw.WriteStartElement("GoogleExport");
                foreach (string inputFile in inputFiles)
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    using (XmlReader xr = XmlReader.Create(inputFile))
                    {
                        XElement record = null;

                        xr.MoveToContent();

                        // Loop through <Campaign /> elements
                        xr.Read();
                        while (!xr.EOF)
                        {
                            if (xr.NodeType == XmlNodeType.Element && xr.Name == "record")
                            {
                                record = XNode.ReadFrom(xr) as XElement;
                                record.WriteTo(xw);
                            }
                            else
                            {
                                xr.Read();
                            }
                        }


                        //xr.MoveToContent();
                        //xr.ReadToFollowing("record");
                        //while (xr.Read())
                        //{
                        //    if (xr.NodeType == XmlNodeType.Element && xr.Name == "GoogleExport")
                        //        continue;
                        //    if (xr.NodeType == XmlNodeType.Element)
                        //    {
                        //        xw.WriteNode(xr, true);
                        //    }

                        //    //if (xr.NodeType == XmlNodeType.Element && xr.Name == "record")
                        //    //{
                        //    //    xw.WriteNode(xr, true);
                        //    //}
                        //}
                        //xw.Flush();
                    }
                }
                xw.Flush();
                xw.WriteEndElement();
                xw.WriteEndDocument();
                xw.Flush();
                xw.Close();
            }
        }
        
    }
}
