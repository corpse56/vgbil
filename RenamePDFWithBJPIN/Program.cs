using Bytescout.PDFExtractor;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
namespace RenamePDFWithBJPIN
{
    class Program
    {

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Drive API .NET Quickstart";

        static void Main(string[] args)
        {

            bool ebana = File.Exists(@"M:\Общие диски\Эл.книги НЭБ\2017-2018\Флинта-2018 PDF\Novikova_Glag6ol.pdf");

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            
            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //DoTheJob("1xXd29WijUSDzCAG7zXZhborD-VgdS6h6", "ЦК_Рудомино книги для НЭБ 2017-2018", "f:\\ЦК_Рудомино книги для НЭБ 2017-2018.xlsx", service);
            DoTheJob("1ylpFLChZrDBiX_pkLH8xYWNay3fERftH", "Флинта-ВГБИЛ книги для НЭБ 2017-2018", "f:\\Флинта-ВГБИЛ книги для НЭБ 2017-2018.xlsx", service);
            //DoTheJob("1V-FkGyvTWMjfyeJtRu662ZywWF-edOKV", "Златоуст-ВГБИЛ книги для НЭБ 2017-2018", "f:\\Златоуст-ВГБИЛ книги для НЭБ 2017-2018.xlsx", service);


            Console.Read();

        }
        public static void DoTheJob(string TeamFolderID, string FolderName, string ExcelFilePath, DriveService service)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, kind, mimeType )";
            //Files.List request = mService.files().list().setQ("'Your Folder ID Here' in parents");
            //FileList files = request.execute();
            //ЦК_Рудомино книги для НЭБ 2017 - 2018
            listRequest.Q = "'"+ TeamFolderID + "' in parents and mimeType = 'application/pdf'";


            listRequest.IncludeTeamDriveItems = true;
            listRequest.Corpora = "teamDrive";
            listRequest.SupportsTeamDrives = true;
            listRequest.TeamDriveId = "0AObEmuax3_3MUk9PVA";
            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1}) {2} ", file.Name, file.Id, file.MimeType);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }

            Excel.Application excel = new Excel.Application();
            Excel.Workbook wb = excel.Workbooks.Open(ExcelFilePath);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];



            string PinExcel = "";
            string ExcelFileID = "";
            string Url = "";
            int i = 1;
            do
            {
                i++;
                if (ws.Cells[i, 2].Value == null) break;
                PinExcel = ws.Cells[i, 2].Value.ToString();
                ExcelFileID = ws.Cells[i, 13].Value.ToString();
                Url = ws.Cells[i, 13].Value.ToString();
                int pos = ExcelFileID.IndexOf("id=") + 3;
                ExcelFileID = ExcelFileID.Substring(pos);
                Google.Apis.Drive.v3.Data.File pdf = files.Where(x => x.Id == ExcelFileID).First();
                Console.WriteLine("{0}   {1}   {2}", PinExcel, ExcelFileID, pdf.Name);
                if (File.Exists("i:\\PDF\\" + FolderName + "\\" + PinExcel + ".pdf"))
                {
                    Console.WriteLine("Already exists. Skiping...");
                    continue;
                }
                DownloadFile(pdf, service, "i:\\PDF\\"+ FolderName+"\\" + PinExcel + ".pdf");
            } while (true);

        }
        public static void DownloadFile(Google.Apis.Drive.v3.Data.File file, DriveService driveService, string path)
        {
            var fileId = file.Id;//"0BwwA4oUTeiV1UVNwOHItT0xfa2M";
            var request = driveService.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded / 1024 + " Kbytes downloaded");
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                                {
                                    stream.Flush();
                                    stream.Position = 0;
                                    stream.CopyTo(fileStream);
                                }
                                stream.Close();
                                Console.WriteLine("Download complete.");
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            request.Download(stream);
        }

        //static void Main(string[] args)
        //{
        //    //// Create Bytescout.PDFExtractor.PDFAValidator instance
        //    //PDFAValidator validator = new PDFAValidator();
        //    //validator.RegistrationName = "demo";
        //    //validator.RegistrationKey = "demo";

        //    //// Load sample PDF document
        //    //validator.LoadDocumentFromFile(@"f:\3ndfl14.pdf");
        //    ////validator.LoadDocumentFromFile(@"f:\148_Сказки Луны.pdf");

        //    //if (validator.IsPDFA)
        //    //    Console.WriteLine("This file conforms to the PDF/A standard");
        //    //else
        //    //    Console.WriteLine("This file doesn't conform to the PDF/A standard. Check .ValidationErros for the list of errors");

        //    //Console.WriteLine();
        //    //Console.WriteLine("Press any key to continue...");
        //    //Console.ReadLine();

    }
}
