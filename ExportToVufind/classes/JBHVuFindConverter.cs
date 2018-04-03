
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ExportBJ_XML.classes
{
    public class JBHVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////// JBH  /////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////////
            string allFields = "";
            _objXmlWriter = XmlTextWriter.Create(@"F:\import\jbh.xml");
            _exportDocument = new XmlDocument();
            XmlNode decalrationNode = _exportDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            _exportDocument.AppendChild(decalrationNode);
            decalrationNode.WriteTo(_objXmlWriter);

            _root = _exportDocument.CreateElement("add");
            _exportDocument.AppendChild(_root);
            _objXmlWriter.WriteStartElement("add");

            _doc = _exportDocument.CreateElement("doc");

            string[] JHB = File.ReadAllLines(@"f:\jbh_source.txt");

            int cnt = 1;
            List<string> Languages2 = new List<string>();
            List<string> Languages3 = new List<string>();
            string FieldCode = "";
            string CurrentId = "";
            string FieldNumber = "";
            string FieldValue = "";
            foreach (string line in JHB)
            {
                if (line == string.Empty)
                {
                    continue;
                }
                if (line.Length == 6)//закончилась предыдущая запись
                {
                    cnt++;
                    _doc.WriteTo(_objXmlWriter);
                    _doc = _exportDocument.CreateElement("doc");
                    VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                    args.RecordId = "JHB_" + CurrentId;
                    OnRecordExported(args);
                    CurrentId = line;
                    continue;
                }

                FieldCode = line.Substring(0, line.IndexOf("/"));
                FieldNumber = line.Substring(line.IndexOf("/"), line.IndexOf(":"));
                switch (FieldCode)
                {
                    case "#101":
                        FieldValue = line.Substring(line.IndexOf("_"));
                        Languages3.Add(FieldValue);
                        break;
                    case "#102":
                        FieldValue = line.Substring(line.IndexOf("_"));
                        Languages2.Add(FieldValue);
                        break;
                }
            }

            _doc.WriteTo(_objXmlWriter);
            _doc = _exportDocument.CreateElement("doc");
            VuFindConverterEventArgs arg = new VuFindConverterEventArgs();
            arg.RecordId = "JHB_" + CurrentId;
            OnRecordExported(arg);


        }

        public void GetSource()
        {
            string[] JHB = File.ReadAllLines(@"f:\jbh_source.rtf");

            string rtf = File.ReadAllText(@"f:\jbh_source.rtf");
            RichTextBox rtb = new RichTextBox();
            rtb.Rtf = rtf;
            string plainText = rtb.Text;

            File.WriteAllText(@"f:\jbh_source.txt", plainText);

        }

        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }
    }
}

//Инвентарные номера в полях 910. Если экземпляров несколько, то это поле повторяется.
//Инвентарные номера книг - цифровые.
//Инвентарные номера Брошюры начинаются с буквы Б
//Разные экземпляры одной и той же книги могут иметь инвентарные номера разных видов
//Поле 675 - УДК
//Поле 908 - Авторский знак
//Поле 686 - Расстановочный шифр. УДК пробел Авторский знак
//Поле 678 - Начинается с Расстановочного шифра. Затем идёт что-то непонятное. Например: /70-276746
//Хорошо бы выяснить у них, что это такое.
//Хорошо бы получить скан обложки, титульного листа и обратной стороны титульного листа со штампами, расстановочным шифром и инвентарным номером хотя бы для одной книги.
//Поле 60/1 - аналог нашей тематики
//Поле 510 - Параллельное заглавие
//Поле 331 - Реферат (Аннотация)
//Если экземпляр 1 - выдаётся только в читальном зале
//Если экземпляров несколько - книга может быть выдана на дом
//Поле 101 - 3-х буквенный код языка
//Каждый выпуск периодики описан отдельно.

//Записи сводного уровня на журнал в целом не нашёл.



