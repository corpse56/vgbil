
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using LibflClassLibrary.ExportToVufind.Vufind;
using Newtonsoft.Json;

namespace LibflClassLibrary.ExportToVufind
{
    public class JBHVuFindConverter : VuFindConverter
    {
        public override void Export()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////// JBH  /////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////


            VufindXMLWriter vfWriter = new VufindXMLWriter("JBH");//Jewish Book House

            vfWriter.StartVufindXML(@"D:\HOME\ВГБИЛ\" + Fund.ToLower() + ".xml");

            string[] JHB = File.ReadAllLines(@"D:\HOME\ВГБИЛ\jbh_source_pasted.txt");
            
            List<VufindDoc> docs = new List<VufindDoc>();
            foreach (string line in JHB)
            {
                VufindDoc doc = new VufindDoc();
                if (doc.id != line)//если встретили новый ID
                {
                    docs.Add(doc);
                    doc = new VufindDoc();
                }
                doc.id = "JHB_000001";
                doc.id = "JHB_" + line;

                string FieldCode = line.Substring(0, line.IndexOf(":")-1);
                string FieldValue = line.Substring(line.IndexOf(":"));

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                JsonWriter writer = new JsonTextWriter(sw);

                switch (FieldCode)
                {
                    case "700":
                        doc.author.Add(FieldValue);
                        break;
                    case "200":
                        doc.title.Add(FieldValue);
                        break;
                    case "901":
                        break;
                }
                vfWriter.AppendVufindDoc(doc);
                //OnRecordExported
                VuFindConverterEventArgs args = new VuFindConverterEventArgs();
                args.RecordId = "JBH_" + doc.id;
                OnRecordExported(args);

                docs.Add(doc);
            }
            vfWriter.FinishWriting();

            //добавили новый комментарий.




        }

        public void GetSource()
        {
            string[] JHB = File.ReadAllLines(@"f:\jbh_source.rtf");

            string[] lines = File.ReadAllLines(@"f:\jbh_source.rtf", Encoding.Default);
            string[] OutLines = new string[lines.Length];
            RichTextBox rtb = new RichTextBox();

            int i = 0;
            foreach (string line in lines)
            {
                rtb.Rtf = line;
                OutLines[i] = rtb.Text;
                i++;
            }

            File.WriteAllLines(@"f:\jbh_source.txt", OutLines, Encoding.Default);


        }

        public override void ExportSingleRecord(int idmain)
        {
            throw new NotImplementedException();
        }
        public override void ExportCovers()
        {
            throw new NotImplementedException();
        }

        public override void ExportSingleCover(object idRecord)
        {
            throw new NotImplementedException();
        }

        public override VufindDoc CreateVufindDocument(object Record)
        {
            throw new NotImplementedException();
        }

        public override void Export(List<string> idSet, string exportFilename)
        {
            throw new NotImplementedException();
        }

        public override List<VufindDoc> Export(List<string> idSet)
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



