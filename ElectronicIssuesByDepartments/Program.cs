using LibflClassLibrary.Books.BJBooks;
using LibflClassLibrary.Books.BJBooks.BJExemplars;
using LibflClassLibrary.Circulation;
using LibflClassLibrary.ExportToVufind;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicIssuesByDepartments
{

    //Программа, которая парсит статистику, выданную гугл-аналитикс по электронным книговыдачам в виде csv
    //здесь нужно выдать количество электронных выдач по отделам
    //то есть две колонки: название отдела и количество выдач
    class Program
    {
        static void Main(string[] args)
        {

            //MakeStat(@"e:\Analytics Все данные по веб-сайту Ежедневный отчет в минкульт, выдач 20200601-20200630.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\июнь с авторскими правами.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\июнь без авторских прав.csv");
            //MakeStat(@"e:\Analytics Все данные по веб-сайту Ежедневный отчет в минкульт, выдач 20200701-20200731.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\июль с авторскими правами.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\июль без авторских прав.csv");
            //MakeStat(@"e:\Analytics Все данные по веб-сайту Ежедневный отчет в минкульт, выдач 20200801-20200831.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\август с авторскими правами.csv",
            //            @"c:\Users\admin\Documents\elIssueStatistic\август без авторских прав.csv");

            MakeStat(@"e:\Analytics Все данные по веб-сайту Ежедневный отчет в минкульт, выдач 20200901-20200930.csv",
                        @"c:\Users\admin\Documents\elIssueStatistic\Сентябрь с авторскими правами.csv",
                        @"c:\Users\admin\Documents\elIssueStatistic\Сентябрь без авторских прав.csv");

            // Bookreader / Viewer ? bookID = BJVVV_1365375 & view_mode = HQ
            // Bookreader / Viewer ? OrderId = 82549 & view_mode = HQ
        }

        private static void MakeStat(string source, string res, string resWar)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            Dictionary<string, int> resultWAR = new Dictionary<string, int>();

            using (var reader = new StreamReader(source))
            {
                List<string> orders = new List<string>();
                List<string> books = new List<string>();


                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!line.Contains("Bookreader"))
                    {
                        continue;
                    }
                    if (line.ToLower().Contains("orderid"))
                    {
                        int pos = line.ToLower().IndexOf("orderid") + 8;
                        int posEnd = line.IndexOf("&");
                        if (posEnd < 0) continue;
                        int orderId = Convert.ToInt32(line.Substring(pos, posEnd - pos));

                        pos = line.IndexOf(",") + 1;
                        int times = Convert.ToInt32(line.Substring(pos));
                        CirculationInfo ci = new CirculationInfo();
                        OrderInfo order;
                        try
                        {
                            order = ci.GetOrder(orderId);
                        }
                        catch { continue; }
                        BJBookInfo book = BJBookInfo.GetBookInfoByPIN(order.BookId);
                        string department = GetBestDepartment(book);

                        for (int i = 0; i < times; i++)
                        {
                            if (result.ContainsKey(department))
                            {
                                result[department]++;
                            }
                            else
                            {
                                result.Add(department, 1);
                            }
                        }
                    }
                    else
                    {
                        int pos = line.ToLower().IndexOf("bookid") + 7;
                        int posEnd = line.IndexOf("&");
                        posEnd = (posEnd < pos) ? line.IndexOf(",") : posEnd;
                        var bookId = line.Substring(pos, posEnd - pos);
                        if (bookId == string.Empty) continue;
                        pos = line.LastIndexOf(",") + 1;
                        int times = Convert.ToInt32(line.Substring(pos));
                        BJBookInfo book = BJBookInfo.GetBookInfoByPIN(bookId);
                        string department = GetBestDepartment(book);

                        for (int i = 0; i < times; i++)
                        {
                            if (resultWAR.ContainsKey(department))
                            {
                                resultWAR[department]++;
                            }
                            else
                            {
                                resultWAR.Add(department, 1);
                            }
                        }

                    }
                }
            }
            SaveToFile(result, res);
            SaveToFile(resultWAR, resWar);
        }

        private static void SaveToFile(Dictionary<string, int> result, string fileName)
        {
            StringBuilder fileContent = new StringBuilder();
            foreach (var item in result)
            {
                fileContent.Append($"{item.Key};{item.Value}{Environment.NewLine}");
            }
            File.WriteAllText(fileName, fileContent.ToString(), Encoding.UTF8);
        }

        private static string GetBestDepartment(BJBookInfo book)
        {
            string result = string.Empty;
            foreach (BJExemplarInfo exemplar in book.Exemplars)
            {
                if (exemplar.Location == string.Empty)
                {
                    continue;
                }

                var dicValue = KeyValueMapping.UnifiedLocationAccess[exemplar.Location];

                if (dicValue.Contains("Книгохранение"))
                {
                    result = exemplar.Fields["899$a"].ToString();
                    continue;
                }
                if (dicValue == "Служебные подразделения")
                {
                    continue;
                }
                result = exemplar.Fields["899$a"].ToString();
                break;
            }
            if (result == string.Empty)
            {
                result = "Бумажные экземпляры списаны, местонахождение \"Сервер ВГБИЛ\"";
            }
            return result;
        }
    }
}
