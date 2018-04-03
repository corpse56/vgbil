using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Interop;
using System.Reflection;

using Word = Microsoft.Office.Interop.Word;
using BookClasses;

namespace WriteOff
{
    class MSWordInserter
    {
        private Word._Application oWord;
        private Word._Document oDoc;
        private object oMissing;
        private object oEndOfDoc;
        private Word.Range wrdRng;
        private string baza;
        public MSWordInserter(string baza)
        {
            this.baza = baza;
            oMissing = System.Reflection.Missing.Value;
            oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

            oWord = new Word.Application();
            oWord.Visible = true;
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
        }

        public void CreateAct(string actNum,string dep,int countAccNums, int countOfPages, int counttopographiccards,string direction, string cause)
        {
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientPortrait;
            object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            //object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            Word.Paragraph oPara = oDoc.Content.Paragraphs.Add(ref oRng);
            //oPara.Range.Font.Name = "Times New Roman";
            oPara.Range.Font.Bold = 1;
            oPara.Range.Font.Size = 12;
            oPara.Range.Text = "ВСЕРОССИЙСКАЯ ГОСУДАРСТВЕННАЯ БИБЛИОТЕКА ИНОСТРАННОЙ ЛИТЕРАТУРЫ";
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara.Range.InsertParagraphAfter();
            //oPara.Range.Font.Bold = 1;
            //oPara.Range.Font.Size = 14;
            //oPara.Range.Text = "ИНОСТРАННОЙ ЛИТЕРАТУРЫ";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            oPara.Range.Font.Bold = 0;
            oPara.Range.Font.Size = 12;
            oPara.Range.Text = "УТВЕРЖДАЮ";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Генеральный директор";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Библиотеки иностранной литературы";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "________________В. В. Дуда";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "\"_____\"___________20   г.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara.Range.Font.Bold = 1;
            oPara.Range.Font.Size = 14;
            oPara.Range.Text = "А К Т № " + actNum;
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Font.Size = 12;
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
            oPara.SpaceBefore = 30;
            oPara.LineSpacing = 30;
            oPara.Range.Font.Bold = 0;
            //switch ()
            
            oPara.Range.Text = (counttopographiccards >0) ?
            "            Настоящий акт составлен " + DateTime.Now.ToLongDateString() +
            " в том, что из " + dep +
            " исключается " + countAccNums +
            " учетных единиц печатных изданий по причине " + cause +
            " и передаются в " + direction +
            ". Приложение: Список на " + countOfPages +
            " листе(ах) и " + counttopographiccards.ToString() +
            " топографических(ой) карточках(е)." : 
            "            Настоящий акт составлен " + DateTime.Now.ToLongDateString() +
            " в том, что из " + dep +
            " исключается " + countAccNums +
            " учетных единиц печатных изданий по причине " + cause +
            " и передаются в " + direction +
            ". Приложение: Список на " + countOfPages +
            " листе(ах).";
            oPara.Range.InsertParagraphAfter();
            oPara.SpaceBefore = 0;
            oPara.SpaceAfter = 0;
            oPara.LineSpacing = 12;
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            //oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Главный хранитель фондов ";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "(ответственный за фонд):                                 ______________________ Баулина А. В.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Зав. комплексным отделом хранения,";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "консервации и реставрации фондов             ______________________ Сальникова Р. М.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Зам. генерального директора ";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Библиотеки иностранной литературы";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "по работе с библиотечными фондами           ______________________ Айгистов Р. А.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            //oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Акт отработан:                                                                              \"____\"______________20   г.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "Издания исключены из инвентарных книг:                            _____________________";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            /*oPara.Range.Text = "Акт отработан по сист.кат.:                    \"____\"______________20   г.";
                        oPara.Range.InsertParagraphAfter();
                        oPara.Range.Text = "                                                                   Зав. сектором систематизации ЦОД";
                        oPara.Range.InsertParagraphAfter();*/
            oPara.Range.Text = "По эл. каталогам:                                                                           \"____\"______________20   г.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "                                                                                                             _____________________";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "По карточным каталогам:                                                            \"____\"______________20   г.";
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "(ГАК, ЧАК, СК)                                                                                    _____________________";











        }

        public Word.Table CreateTemplateTable(string act, string dep)
        {
            object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            Word.Paragraph oPara;
            //oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            oPara = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara.Range.Text = "К акту № " + act;
            oPara.Range.Font.Bold = 1;
            oPara.Range.Font.Size = 10;
            
            oPara.Range.InsertParagraphAfter();
            if (this.baza == "BRIT_SOVET")
            {
                oPara.Range.Text = "СПИСОК КНИГ ИСКЛЮЧАЕМЫХ ИЗ ФОНДА БРИТАНСКОГО СОВЕТА - " + dep;
            }
            else
            {
                oPara.Range.Text = "СПИСОК КНИГ ИСКЛЮЧАЕМЫХ ИЗ ОСНОВНОГО ХРАНЕНИЯ " + dep;
            }
            oPara.Range.Font.Bold = 1;
            oPara.Range.Font.Size = 12;
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara.Range.InsertParagraphAfter();
            oPara.Range.Text = "";
            oPara.Range.InsertParagraphAfter();


            Word.Table oTable;
            wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            
            oTable = oDoc.Tables.Add( wrdRng, 4, 11, ref oMissing, ref oMissing);
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            oTable.Rows.AllowBreakAcrossPages = 0;
            oTable.Range.Font.Size = 10;
            oTable.Columns[1].Width = 30;
            oTable.Columns[2].Width = 90;
            oTable.Columns[3].Width = 140;
            oTable.Columns[4].Width = 60;
            oTable.Columns[5].Width = 40;
            oTable.Columns[6].Width = 85;
            oTable.Columns[7].Width = 65;
            oTable.Columns[8].Width = 70;
            oTable.Columns[9].Width = 50;
            oTable.Columns[10].Width = 50;
            oTable.Columns[11].Width = 55;
            
            oTable.Borders.Enable = 1;
            oTable.Range.Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            oTable.Range.Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            //шапка
            oTable.Rows[1].Range.Font.Bold = 1;
            oTable.Rows[2].Range.Font.Bold = 1;
            oTable.Rows[3].Range.Font.Bold = 0;
            oTable.Rows[4].Range.Font.Bold = 0;
            oTable.Rows[1].Shading.Texture = WdTextureIndex.wdTexture20Percent;
            oTable.Rows[2].Shading.Texture = WdTextureIndex.wdTexture20Percent;
            oTable.Rows[1].HeadingFormat = -1;
            oTable.Rows[2].HeadingFormat = -1;
            oTable.Cell(1, 1).Merge(oTable.Cell(2, 1));
            oTable.Cell(1, 2).Merge(oTable.Cell(1, 3));
            oTable.Cell(1, 2).Merge(oTable.Cell(1, 3));
            oTable.Cell(1, 2).Merge(oTable.Cell(1, 3));
            oTable.Cell(1, 3).Merge(oTable.Cell(2, 6));
            oTable.Cell(1, 4).Merge(oTable.Cell(2, 7));
            oTable.Cell(1, 5).Merge(oTable.Cell(2, 8));
            oTable.Cell(1, 6).Merge(oTable.Cell(2, 9));
            oTable.Cell(1, 7).Merge(oTable.Cell(2, 10));
            oTable.Cell(1, 8).Merge(oTable.Cell(2, 11));
            oTable.Cell(1, 1).Range.Text = "№ п/п";
            oTable.Cell(1, 2).Range.Text = "Описание издания";
            oTable.Cell(1, 3).Range.Text = "Инвентарный номер";
            oTable.Cell(1, 4).Range.Text = "Кол-во экз. оставшихся в фонде";
            oTable.Cell(2, 2).Range.Text = "Автор/Колл. автор";
            oTable.Cell(2, 3).Range.Text = "Заглавие";
            oTable.Cell(2, 4).Range.Text = "Место изд.";
            oTable.Cell(2, 5).Range.Text = "Год";
            //oTable.Cell(1, 6).Merge(oTable.Cell(2, 6));
            //oTable.Cell(1, 4).Merge(oTable.Cell(2, 4));
            //oTable.Cell(2, 7).Range.Text = "Метка";
            oTable.Cell(1, 5).Range.Text = "Инв. номера оставшихся в фонде экземпляров";
            //oTable.Cell(1, 6).LeftPadding = 30;
            oTable.Cell(1, 6).Range.Text = "Цена";
            oTable.Cell(1, 7).Range.Text = "Коэффициент";
            oTable.Cell(1, 8).Range.Text = "Сумма";
            //oTable.Cell(1, 10).Range.Text = "";
            //номера страниц
            object alignment = new object();
            object bFirstPage = new object();
            oWord.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageHeader; 
            oWord.Selection.HeaderFooter.PageNumbers.Add(ref     alignment, ref     bFirstPage);
            oWord.ActiveWindow.ActivePane.View.SeekView = WdSeekView.wdSeekMainDocument;
            //запрет переноса строк
            //oTable.Rows.AllowBreakAcrossPages = 0;

            return oTable;
        }
        
        public KeyValuePair<int,int> AddBooksToTableTemplate(List<Book> l,string act,string dep)
        {
            l.Sort();
            Word.Table oTable = this.CreateTemplateTable(act,dep);
            string currentLanguage = l[0].Language;
            if (currentLanguage == null)
            {
                currentLanguage = "Не заполнено поле язык";
            }

            //oTable.Rows[3].Range.Font.Bold = 0;
            oTable.Cell(3, 1).Merge(oTable.Cell(3, 11));
            oTable.Cell(3, 1).Range.Text = currentLanguage.ToUpper();
            int ID = 0;
            int CountWriteOff = 0;
            foreach (Book b in l)
            {
                if (currentLanguage != b.Language)
                {
                    
                    oTable.Rows.Add(ref oMissing);
                    oTable.Cell(oTable.Rows.Count - 1, 1).Merge(oTable.Cell(oTable.Rows.Count - 1, 11));
                    currentLanguage = b.Language;
                    if (currentLanguage == null)
                    {
                        currentLanguage = "Не заполнено поле язык";
                    }
                    oTable.Cell(oTable.Rows.Count - 1, 1).Range.Text = currentLanguage.ToUpper();
                }
                ID++;
                CountWriteOff += b.accNums_.Count - b.InvsLeftInFund;
                this.AddBookToTable(oTable, b, ID);
                oTable.Rows.Add(ref oMissing);
            }
            oTable.Cell(oTable.Rows.Count, 1).Merge(oTable.Cell(oTable.Rows.Count, 10));
            oTable.Cell(oTable.Rows.Count, 1).Range.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            oTable.Cell(oTable.Rows.Count, 1).Range.Text = "Итого списано по акту: "+CountWriteOff.ToString() + " инвентарных номера(ов)";
            return new KeyValuePair<int,int>(CountWriteOff,oDoc.ComputeStatistics(WdStatistic.wdStatisticPages,ref oMissing));
        }
        private void AddBookToTable(Word.Table oTable, Book b, int id)
        {
            oTable.Cell(oTable.Rows.Count, 1).Range.Text = id.ToString();
            oTable.Cell(oTable.Rows.Count, 2).Range.Text = b.Author;
            oTable.Cell(oTable.Rows.Count, 3).Range.Text = b.Title;
            oTable.Cell(oTable.Rows.Count, 4).Range.Text = b.PlaceOfPublish;
            oTable.Cell(oTable.Rows.Count, 5).Range.Text = b.YearOfPublish;

            //oTable.Cell(oTable.Rows.Count, 6).Range.Text = oTable.Cell(oTable.Rows.Count, 6).Range.Text.Remove(1,1);
            string tmpN = "";
            string tmpL = "";
            string tmpNL = "";
            string refnum = "";

            StringBuilder price = new StringBuilder();
            int summ = 0;
            string currency = b.accNums_[0].Currency;
            bool diffCurrency = false;
            string trueCurrency = "";
            foreach (AccessionNumber num in b.accNums_)
            {
                
                if (num.IsWriteOff)
                {
                    tmpN += num.AccessionNum + "\r";
                    tmpL += num.AccessionLabel + "\r";

                    if (num.Price == -1)
                    {
                        price.AppendFormat("ошибка данных\r");//невозможно в число кастануть
                        diffCurrency = true;
                    }
                    else if (num.Price == -2)
                    {
                        price.AppendFormat("нет поля\r");
                        diffCurrency = true;
                    }
                    else
                    {
                        price.AppendFormat("{0} {1}\r", num.Price.ToString(), num.Currency);
                        trueCurrency = num.Currency;
                    }
                    summ += num.Price;
                    if (num.Price == 100)
                    {
                        summ += 1;
                        summ -= 1;
                    }
                    if (currency != num.Currency)
                    {
                        diffCurrency = true;
                    }

                }
                else
                {
                    tmpNL += num.AccessionNum + "\r";
                }
                if ((num.Fund == "O") && (num.IsWriteOff))
                {
                    refnum += b.ReferenceNumberOF + "\r";
                }
                if ((num.Fund == "A") && (num.IsWriteOff))
                {
                    refnum += b.ReferenceNumberAB + "\r";
                }
                if ((num.Fund == "R") && (num.IsWriteOff))
                {
                    refnum += b.ReferenceNumberR + "\r";
                }
            }
            tmpN = (tmpN == string.Empty) ? tmpN : tmpN.Remove(tmpN.Length - 1);
            tmpL = (tmpL == string.Empty) ? tmpL : tmpL.Remove(tmpL.Length - 1);
            tmpNL = (tmpNL == string.Empty) ? tmpNL : tmpNL.Remove(tmpNL.Length - 1);
            refnum = (refnum == string.Empty) ? refnum : refnum.Remove(refnum.Length - 1);
            oTable.Cell(oTable.Rows.Count, 6).Range.Text = tmpN;
            //oTable.Cell(oTable.Rows.Count, 7).Range.Text = tmpL;
            oTable.Cell(oTable.Rows.Count, 8).Range.Text = tmpNL;
            //oTable.Cell(oTable.Rows.Count, 8).Range.Text = refnum;
            oTable.Cell(oTable.Rows.Count, 7).Range.Text = b.InvsLeftInFund.ToString();
            oTable.Cell(oTable.Rows.Count, 9).Range.Text = price.ToString().Remove(price.Length-1);
            if (diffCurrency)
            {
                oTable.Cell(oTable.Rows.Count, 11).Range.Text = "Невозможно подсчитать полную сумму.";
            }
            else
            {
                oTable.Cell(oTable.Rows.Count, 11).Range.Text = summ.ToString() + " " + trueCurrency;
            }

        }
        //public void Example()
        //{

        //    //Insert a paragraph at the beginning of the document.
        //    Word.Paragraph oPara1;
        //    oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
        //    oPara1.Range.Text = "Heading 1";
        //    oPara1.Range.Font.Bold = 1;
        //    oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
        //    oPara1.Range.InsertParagraphAfter();

        //    //Insert a paragraph at the end of the document.
        //    Word.Paragraph oPara2;
        //    object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    oPara2 = oDoc.Content.Paragraphs.Add(ref oRng);
        //    oPara2.Range.Text = "Heading 2";
        //    oPara2.Format.SpaceAfter = 6;
        //    oPara2.Range.InsertParagraphAfter();

        //    //Insert another paragraph.
        //    Word.Paragraph oPara3;
        //    oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

        //    oPara3 = oDoc.Content.Paragraphs.Add(ref oRng);
        //    oPara3.Range.Text = "This is a sentence of normal text. Now here is a table:";
        //    oPara3.Range.Font.Bold = 0;
        //    oPara3.Format.SpaceAfter = 24;
        //    oPara3.Range.InsertParagraphAfter();

        //    //Insert a 3 x 5 table, fill it with data, and make the first row
        //    //bold and italic.
        //    Word.Table oTable;
        //    Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    oTable = oDoc.Tables.Add(wrdRng, 3, 5, ref oMissing, ref oMissing);
        //    oTable.Range.ParagraphFormat.SpaceAfter = 6;
        //    int r, c;
        //    string strText;
        //    for (r = 1; r <= 3; r++)
        //        for (c = 1; c <= 5; c++)
        //        {
        //            strText = "r" + r + "c" + c;
        //            oTable.Cell(r, c).Range.Text = strText;
        //        }
        //    oTable.Rows[1].Range.Font.Bold = 1;
        //    oTable.Rows[1].Range.Font.Italic = 1;

        //    //Add some text after the table.
        //    Word.Paragraph oPara4;
        //    oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    oPara4 = oDoc.Content.Paragraphs.Add(ref oRng);
        //    oPara4.Range.InsertParagraphBefore();
        //    oPara4.Range.Text = "And here's another table:";
        //    oPara4.Format.SpaceAfter = 24;
        //    oPara4.Range.InsertParagraphAfter();

        //    //Insert a 5 x 2 table, fill it with data, and change the column widths.
        //    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    oTable = oDoc.Tables.Add(wrdRng, 5, 2, ref oMissing, ref oMissing);
        //    oTable.Range.ParagraphFormat.SpaceAfter = 6;
        //    for (r = 1; r <= 5; r++)
        //        for (c = 1; c <= 2; c++)
        //        {
        //            strText = "r" + r + "c" + c;
        //            oTable.Cell(r, c).Range.Text = strText;
        //        }
        //    oTable.Columns[1].Width = oWord.InchesToPoints(2); //Change width of columns 1 & 2
        //    oTable.Columns[2].Width = oWord.InchesToPoints(3);

        //    //Keep inserting text. When you get to 7 inches from top of the
        //    //document, insert a hard page break.
        //    object oPos;
        //    double dPos = oWord.InchesToPoints(7);
        //    oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
        //    do
        //    {
        //        wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //        wrdRng.ParagraphFormat.SpaceAfter = 6;
        //        wrdRng.InsertAfter("A line of text");
        //        wrdRng.InsertParagraphAfter();
        //        oPos = wrdRng.get_Information
        //                       (Word.WdInformation.wdVerticalPositionRelativeToPage);
        //    }
        //    while (dPos >= Convert.ToDouble(oPos));
        //    object oCollapseEnd = Word.WdCollapseDirection.wdCollapseEnd;
        //    object oPageBreak = Word.WdBreakType.wdPageBreak;
        //    wrdRng.Collapse(ref oCollapseEnd);
        //    wrdRng.InsertBreak(ref oPageBreak);
        //    wrdRng.Collapse(ref oCollapseEnd);
        //    wrdRng.InsertAfter("We're now on page 2. Here's my chart:");
        //    wrdRng.InsertParagraphAfter();

        //    //Insert a chart.
        //    Word.InlineShape oShape;
        //    object oClassType = "MSGraph.Chart.8";
        //    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    oShape = wrdRng.InlineShapes.AddOLEObject(ref oClassType, ref oMissing,
        //        ref oMissing, ref oMissing, ref oMissing,
        //        ref oMissing, ref oMissing, ref oMissing);

        //    //Demonstrate use of late bound oChart and oChartApp objects to
        //    //manipulate the chart object with MSGraph.
        //    object oChart;
        //    object oChartApp;
        //    oChart = oShape.OLEFormat.Object;
        //    oChartApp = oChart.GetType().InvokeMember("Application",
        //        BindingFlags.GetProperty, null, oChart, null);

        //    //Change the chart type to Line.
        //    object[] Parameters = new Object[1];
        //    Parameters[0] = 4; //xlLine = 4
        //    oChart.GetType().InvokeMember("ChartType", BindingFlags.SetProperty,
        //        null, oChart, Parameters);

        //    //Update the chart image and quit MSGraph.
        //    oChartApp.GetType().InvokeMember("Update",
        //        BindingFlags.InvokeMethod, null, oChartApp, null);
        //    oChartApp.GetType().InvokeMember("Quit",
        //        BindingFlags.InvokeMethod, null, oChartApp, null);
        //    //... If desired, you can proceed from here using the Microsoft Graph 
        //    //Object model on the oChart and oChartApp objects to make additional
        //    //changes to the chart.

        //    //Set the width of the chart.
        //    oShape.Width = oWord.InchesToPoints(6.25f);
        //    oShape.Height = oWord.InchesToPoints(3.57f);

        //    //Add text after the chart.
        //    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
        //    wrdRng.InsertParagraphAfter();
        //    wrdRng.InsertAfter("THE END.");

        //    //Close this form.
        //    //this.Close();

        //}
    }
}
