using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SoftArtisans.HTMLToWord;
using System.Text;
using BookForOrder;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        
       StringBuilder strBody = new StringBuilder();

        strBody.Append("<html "  +
            "xmlns:o='urn:schemas-microsoft-com:office:office' " +
            "xmlns:w='urn:schemas-microsoft-com:office:word'" +
            "xmlns='http://www.w3.org/TR/REC-html40'>" +
            "<head><title>БО</title>") ;

        //'The setting specifies document's view after it is downloaded as Print
        //'instead of the default Web Layout
        strBody.Append("<!--[if gte mso 9]>" +
                             "<xml>" +
                             "<w:WordDocument>" +
                             "<w:View>Print</w:View>" +
                             "<w:Zoom>90</w:Zoom>" +
                             "<w:DoNotOptimizeForBrowser/>" +
                             "<w:style w:type=\"paragraph\" w:default=\"on\" w:styleId=\"Normal\">" +
                            "</w:style>" +
                            " <w:p>"+
                             " <w:pPr>" +
                              "  <w:spacing w:before=\"0\" w:after=\"0\" />" +
                             " </w:pPr>" +
                            " </w:p>" +
                             "</w:WordDocument>" +
                             "</xml>" +
                             "<![endif]-->");

        strBody.Append("<style>" +
                            "<!-- /* Style Definitions */" +
                            "@page Section1" +
                            "   {size:8.5in 11.0in; " +
                            "   margin:1.0in 1.25in 1.0in 1.25in ; " +
                            "   mso-header-margin:.5in; " +
                            "   mso-footer-margin:.5in; mso-paper-source:0;}" +
                            " div.Section1" +
                            "   {page:Section1;}" +
                            "-->" +
                           "</style>"+
                           "<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">"+
                           "</head>");




        strBody.Append("<body><div>");
                        

        List<KeyValuePair<string, Book>> spisok = (List<KeyValuePair<string, Book>>)Session["spisok"];
        string firstLang = spisok[0].Key;
        strBody.Append("<h1>" + firstLang + "</h1>");
        int i = 1;
        foreach (KeyValuePair<string, Book> b in spisok)
        {
            if (firstLang != b.Key)
            {
                i = 1;
                firstLang = b.Key;
                strBody.Append("<h1>"+b.Key+"</h1>");
                if ((b.Value.Avt != null) || (b.Value.Avt != ""))
                {
                    //strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + (i++) + ". " + b.Value.Avt);
                    strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + b.Value.Name);
                }
                else
                {
                    //strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + (i++) + ". " + b.Value.Name);
                }
                if (b.Value.InvsOfBook.Count != 0)
                {
                    strBody.Append("<p style=\"margin-top: 0px;margin-bottom: 10px;padding-bottom: 10px;padding-top: 0px;\">" + b.Value.InvsOfBook[0].inv);
                }
            }
            else
            {
                if ((b.Value.Avt != null) || (b.Value.Avt != ""))
                {
                    //strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + (i++) + ". " + b.Value.Avt);
                    strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + b.Value.Name);
                }
                else
                {
                    strBody.Append("<p style=\"margin: 0px;padding: 0px;\">" + (i++) + ". " + b.Value.Name);
                }
                if (b.Value.InvsOfBook.Count != 0)
                {
                    strBody.Append("<p style=\"margin-top: 0px;margin-bottom: 10px;padding-bottom: 10px;padding-top: 0px;\">" + b.Value.InvsOfBook[0].inv);
                }
            }
        }


        strBody.Append("</div></body></html>") ;

        //'Force this content to be downloaded 
        //'as a Word document with the name of your choice
        Response.AppendHeader("Content-Type", "application/msword");
        Response.AppendHeader("Content-disposition ",
                               "attachment; filename=BibliogrphicalDescriptionList.doc");
        Response.Write(strBody);
        //Response.AddHeader("Refresh", "5; url=~/default.aspx");
        //Response.Redirect("~/default.aspx", true);
        Response.End();

    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        

        

    }
}
