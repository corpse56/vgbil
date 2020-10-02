
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jpgToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            Document document = new Document();
            document.SetMargins(0, 0, 0, 0);
            using (var stream = new FileStream(@"e:\jpgtopdf\test.pdf", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();
                using (var imageStream = new FileStream(@"e:\jpgtopdf\sirenevyi-tuman.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Image image = Image.GetInstance(imageStream);
                    image.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height); // set the height and width of image to PDF page size
                    document.Add(image);
                }
                using (var imageStream = new FileStream(@"e:\jpgtopdf\tico-tico1.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    Image image = Image.GetInstance(imageStream);
                    image.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height); // set the height and width of image to PDF page size
                    document.Add(image);
                }
                document.Close();
            }
        }
    }
}
