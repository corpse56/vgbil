using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing;
using Utilities;

namespace WriteoffExcel.Classes
{
    public class ExcelWork : IDisposable
    {
        private string ActNumber;
        private Excel.Worksheet _ws3, _ws2, _ws1, _currentWS;
        Excel.Application _excelApp;
        Workbooks _workbooks;
        Excel.Workbook _wb;
        string _fileName;
        decimal _cost = 0, _costPerPage = 0;
        int _countPerPage;
        double _pageHeight = 0;
        int _pageNumber = 1;
        int _rowIncrement = 9;
        public ExcelWork(string ActNumber)
        {
            if (ActNumber.Contains("Акт"))
            {
                ActNumber = ActNumber.Replace("Акт", "");
            }
            if (ActNumber.Contains("акт"))
            {
                ActNumber = ActNumber.Replace("акт", "");
            }
            ActNumber.Trim();
            this.ActNumber = ActNumber;
        }
        public void Init()
        {
            _excelApp = new Excel.Application();
            _excelApp.DisplayAlerts = false;
            _excelApp.Visible = false;
            _workbooks = _excelApp.Workbooks;
            _wb = _workbooks.Open($@"{AppDomain.CurrentDomain.BaseDirectory}\blank2.xls");
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = $"Акт списания {Extensions.RemoveIllegalCharsFromFilename(ActNumber)}. Сформировано {DateTime.Now.ToShortDateString()}.xls";
            dialog.Filter = "Файлы Excel(*.xls; *.xlsx) | *.xls; *.xlsx";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//@"e:\";//
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                throw new Exception("Файл не сохранён!");
            }
            _wb.SaveAs(dialog.FileName);
            this._fileName = dialog.FileName;
            _wb.Close();
            _wb = _excelApp.Workbooks.Open($@"{dialog.FileName}");
            _excelApp.Visible = true;

            _ws1 = _wb.Worksheets["стр.1"];
            _ws2 = _wb.Worksheets["стр.2"];
            _ws3 = _wb.Worksheets["стр.3"];
            _ws2.Select();
            _currentWS = _ws2;
        }
        public void InsertExemplar(BJExemplarInfo exemplar, BJBookInfo book, int RowIndex)
        {
            _countPerPage++;
            SetRowIncrement(_pageNumber);
            string Title = GetTitle(book);
            double RowHeight = GetMeasuredRowHeight(Title);
            _pageHeight += RowHeight + 5;
            if (_pageHeight > 620)
            {
                InsertPageSumm();
                MakeNewPage();
                SetRowIncrement(_pageNumber);
                AddEmptyRow(_currentWS);
                InsertRowData(RowIndex, Title, exemplar, book, RowHeight);
            }
            else
            {
                AddEmptyRow(_currentWS);
                InsertRowData(RowIndex, Title, exemplar, book, RowHeight);
            }
        }


        private void MakeNewPage()
        {
            _pageNumber++;
            _countPerPage = 1;
            _costPerPage = 0;
            _pageHeight = 0;
            _ws3.Copy(Type.Missing, _ws3);
            Worksheet copySheet = _wb.Sheets.get_Item(_ws3.Index + 1);
            copySheet.Name = $"стр.{_pageNumber + 2}";
            _currentWS = _ws3;
            _currentWS.Range["DD1"].Value = $"Форма 0504144 с. {_pageNumber+1}";
            _ws3 = copySheet;
            _currentWS.Select();
        }
        private string GetTitle(BJBookInfo book)
        {
            string author = book.Fields["700$a"].ToString();
            string title = book.Fields["200$a"].ToString();
            if (author == string.Empty)
            {
                title = book.Fields["200$a"].ToString();
            }
            else
            {
                title = $"{author} / {title}";
            }
            return title;
        }

        private void SetRowIncrement(int pageNumber)
        {
            if (_pageNumber == 1)
            {
                _rowIncrement = 9;
            }
            else
            {
                _rowIncrement = 3;
            }
        }

        private double GetMeasuredRowHeight(string Title)
        {
            return MeasureTextHeight(Title, _ws2.Range[$"T10"].Font, 148);
        }
        private void InsertRowData(int RowIndex, string Title, BJExemplarInfo exemplar, BJBookInfo book, double RowHeight)
        {
            _currentWS.Range[$"A{_countPerPage + _rowIncrement}"].Value = RowIndex;
            _currentWS.Range[$"F{_countPerPage + _rowIncrement}"].Value = exemplar.Fields["899$p"].ToString();
            _currentWS.Range[$"T{_countPerPage + _rowIncrement}"].Value = Title;
            _currentWS.Range[$"T{_countPerPage + _rowIncrement}"].WrapText = true;
            _currentWS.Range[$"BA{_countPerPage + _rowIncrement}"].Value = "шт";
            _currentWS.Range[$"BK{_countPerPage + _rowIncrement}"].Value = 1;
            decimal Price = 0;
            string Currency = "";
            if (exemplar.Fields["922$c"].ToString() == string.Empty)
            {
                if (book.Fields["101$a"].ToString() == "Рус.")
                {
                    Price = 261;
                    Currency = "RUB";
                }
                else
                {
                    Price = 1475;
                    Currency = "RUB";
                }
            }
            else
            {
                string str = exemplar.Fields["922$c"].ToString();
                Currency = exemplar.Fields["922$d"].ToString();
                Price = Decimal.Parse(str.Replace(".", ","));
            }
            _currentWS.Range[$"BS{_countPerPage + _rowIncrement}"].Value = Price.ToString("0.00");
            _cost += Price;
            _costPerPage += Price;
            _currentWS.Range[$"CL{_countPerPage + _rowIncrement}"].Value = Price.ToString("0.00");
            _currentWS.Range[$"T{_countPerPage + _rowIncrement}"].RowHeight = RowHeight + 5;

        }

        private void AddEmptyRow(Worksheet Ws)
        {
            Range line = (Range)_currentWS.Rows[_countPerPage + _rowIncrement];
            line.Insert();
            Ws.Range[$"A{_countPerPage + _rowIncrement}:E{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"F{_countPerPage + _rowIncrement}:S{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"T{_countPerPage + _rowIncrement}:AZ{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"T{_countPerPage + _rowIncrement}:AZ{_countPerPage + _rowIncrement}"].Cells.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            Ws.Range[$"BA{_countPerPage + _rowIncrement}:BJ{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"BK{_countPerPage + _rowIncrement}:BR{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"BS{_countPerPage + _rowIncrement}:CC{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"CD{_countPerPage + _rowIncrement}:CK{_countPerPage + _rowIncrement}"].Merge();
            Ws.Range[$"CL{_countPerPage + _rowIncrement}:DD{_countPerPage + _rowIncrement}"].Merge();
            Excel.Borders border = _currentWS.Range[$"A{_countPerPage + _rowIncrement}:CL{_countPerPage + _rowIncrement - 1}"].Borders;
            border.LineStyle = Excel.XlLineStyle.xlContinuous;
            border.Weight = 2d;
        }

        public void InsertDocumentHeader(int Count, string Department, int Cost)
        {
            _ws1.Range["AU6"].Value = ActNumber;
            //_ws1.Range["AE9"].Value = DateTime.Now.Day;
            //_ws1.Range["AL9"].Value = DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru"));
            //_ws1.Range["AL9"].Value = Utilities.Extensions.IntMonthToRusString(DateTime.Now.Month);
            //_ws1.Range["BG9"].Value = DateTime.Now.Year - 2000;
            _ws1.Range["Y13"].Value = Department;
            _ws1.Range["BB12"].Value = 7709102090;
            _ws1.Range["CG25"].Value = Count;
            _ws1.Range["CG28"].Value = _cost.ToString("0.00");
            int IntCost = Convert.ToInt32(_cost);
            decimal decPenny = _cost % 1.0m;
            int IntPenny = Convert.ToInt32(decPenny * 100);
            string Penny = IntPenny.ToString();
            Penny = (Penny.Length == 1) ? $"0{Penny}" : Penny;
            _ws1.Range["S26"].Value = RusNumber.Str(IntCost) + " руб. " + Penny + " коп." ;
            _ws1.Range["W32"].Value = "Главный хранитель фондов";
            _ws1.Range["BZ32"].Value = "Баулина А. В.";
            _ws1.Range["W34"].Value = "Зав. сектором";
            _ws1.Range["BZ34"].Value = "Позднышев А. Е.";
            _ws1.Range["W36"].Value = "Зав. сектором";
            _ws1.Range["BZ36"].Value = "Русакова Л. В.";
            _ws1.Range["W38"].Value = "Ведущий бухгалтер";
            _ws1.Range["BZ38"].Value = "Беркетова Е. А.";
            _ws1.Range["W40"].Value = "Главный библиотекарь";
            _ws1.Range["BZ40"].Value = "Почкина М. В.";
            _ws1.Range["W42"].Value = "Ведущий библиограф";
            _ws1.Range["BZ42"].Value = "Базилевская И. Н.";

            _ws1.Range["W58"].Value = "Главный хранитель фондов";
            _ws1.Range["BZ58"].Value = "Баулина А. В.";
            _ws1.Range["W60"].Value = "Зав. сектором";
            _ws1.Range["BZ60"].Value = "Позднышев А. Е.";
            _ws1.Range["W62"].Value = "Зав. сектором";
            _ws1.Range["BZ62"].Value = "Русакова Л. В.";
            _ws1.Range["W64"].Value = "Ведущий бухгалтер";
            _ws1.Range["BZ64"].Value = "Беркетова Е. А.";
            _ws1.Range["W66"].Value = "Главный библиотекарь";
            _ws1.Range["BZ66"].Value = "Почкина М. В.";
            _ws1.Range["W68"].Value = "Ведущий библиограф";
            _ws1.Range["BZ68"].Value = "Базилевская И. Н.";

            _ws1.Range["R18"].Value = "Главный хранитель фондов Баулина А. В.; Зав. сектором Позднышев А. Е.;";
            _ws1.Range["A20"].Value = "Зав. сектором Русакова Л. В.; Ведущий бухгалтер Беркетова Е. А.;";
            _ws1.Range["A21"].Value = "Главный библиотекарь Почкина М. В.; Ведущий библиограф Базилевская И. Н.";

            _ws2.Range["AI2"].Value = DateTime.Now.Day;
            _ws2.Range["T2"].Value = ActNumber;
            _ws2.Range["AO2"].Value = Utilities.Extensions.IntMonthToRusString(DateTime.Now.Month);
            _ws2.Range["BH2"].Value = DateTime.Now.Year - 2000;

            AddSignature();
            _currentWS.Range[$"BK{_countPerPage + _rowIncrement + 1}"].Value = _countPerPage;
            _currentWS.Range[$"CL{_countPerPage + _rowIncrement + 1}"].Value = _costPerPage.ToString("0.00");
            _currentWS.Range[$"CL{_countPerPage + _rowIncrement + 2}"].Value = _cost.ToString("0.00");
            _currentWS.Range[$"BK{_countPerPage + _rowIncrement + 2}"].Value = Count;
        }
        private void InsertPageSumm()
        {
            _currentWS.Range[$"BK{_countPerPage + _rowIncrement}"].Value = _countPerPage - 1;
            _currentWS.Range[$"CL{_countPerPage + _rowIncrement}"].Value = _costPerPage.ToString("0.00");
        }

        private void AddSignature()
        {
            //добавить членов комиссии в конце

            for(int i = 0;i<13;i++)
            {
                Range line = (Range)_currentWS.Rows[_countPerPage + _rowIncrement+3];
                line.Insert();
                _currentWS.Range[$"A{_countPerPage + _rowIncrement + 3}:B{_countPerPage + _rowIncrement+3}"].Merge();
            }

            Excel.Range from = _ws1.Range["A58:CY69"];
            Excel.Range to = _currentWS.Range[$"A{ _countPerPage + _rowIncrement + 4}:CY{_countPerPage + _rowIncrement + 16}"];
            from.Copy(to);

            from = _currentWS.Range[$"AM{ _countPerPage + _rowIncrement + 1}:DD{ _countPerPage +_rowIncrement + 1}"];
            to = _currentWS.Range[$"AM{ _countPerPage + _rowIncrement + 2}:DD{_countPerPage + _rowIncrement + 2}"];
            from.Copy(to);
            _currentWS.Range[$"AM{ _countPerPage + _rowIncrement + 2}"].Value = "Всего";

        }
        public double MeasureTextHeight(string text, Microsoft.Office.Interop.Excel.Font font, int width)
        {
            if (string.IsNullOrEmpty(text)) return 0.0;
            var bitmap = new Bitmap(1, 1);
            var graphics = Graphics.FromImage(bitmap);

            var pixelWidth = Convert.ToInt32(width / 1);  //7.5 pixels per excel column width
            var drawingFont = new System.Drawing.Font(font.Name, (float)font.Size);
            var size = graphics.MeasureString(text, drawingFont, pixelWidth);
            
            //72 DPI and 96 points per inch.  Excel height in points with max of 409 per Excel requirements.
            return Math.Min(Convert.ToDouble(size.Height) * 72 / 96, 409);
        }
        public void Dispose()
        {
            _ws3.Delete();
            _wb.Save();
            //_wb.Close(0);
            //_excelApp.Quit();
            //Marshal.ReleaseComObject(_wb);
            //Marshal.ReleaseComObject(_workbooks);
            //Marshal.ReleaseComObject(_excelApp);
        }
        internal void OpenFolderWithGeneratedFile()
        {
            string argument = "/select, \"" + _fileName + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        
    }
}
