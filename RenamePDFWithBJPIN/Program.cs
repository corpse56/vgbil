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
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 1000;
            listRequest.Fields = "nextPageToken, files(id, name, kind, mimeType )";
            //Files.List request = mService.files().list().setQ("'Your Folder ID Here' in parents");
            //FileList files = request.execute();

            listRequest.Q = "'1V-FkGyvTWMjfyeJtRu662ZywWF-edOKV' in parents and mimeType = 'application/pdf'";


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
                    Console.WriteLine("{0} ({1}) {2} ", file.Name, file.Id, file.MimeType );
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }


            Console.Read();

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

        //    //GoogleDriveFileDownloader.DownloadFileFromURLToPath(@"https://drive.google.com/open?id=1MGrR-qfw8d1OmWaGkBYwgcEa30NcfFrC", "f:\\pdf.pdf");
        //    //GoogleDriveFileDownloader.DownloadFileFromURLToPath(@"11wVkCUopwu8SCle3SXQIfp9g2bfTpShx", "f:\\pdf.pdf");

        //    //эти данные сгенерировал сам гугл для доступа к апи
        //    //client id
        //    //283699318704-qrftch1i3n2o79c0ondge04c21jaq8sk.apps.googleusercontent.com
        //    //client secret
        //    jg837iDhFwF - BE9nJLC4XZ8z













        //    Google.Apis.Drive.v3.DriveService driveService = new DriveService();
        //    var fileId = "1p972hdQj37j1vLDlzGorEpnQlLrenWZ7";
        //    var request = driveService.Files.Export(fileId, "application/pdf");
        //    var stream = new System.IO.MemoryStream();
        //    // Add a handler which will be notified on progress changes.
        //    // It will notify on each chunk download and when the
        //    // download is completed or failed.
        //    request.MediaDownloader.ProgressChanged +=
        //            (IDownloadProgress progress) =>
        //            {
        //                switch (progress.Status)
        //                {
        //                    case DownloadStatus.Downloading:
        //                        {
        //                            Console.WriteLine(progress.BytesDownloaded);
        //                            break;
        //                        }
        //                    case DownloadStatus.Completed:
        //                        {
        //                            Console.WriteLine("Download complete.");
        //                            using (var fileStream = new FileStream("f:\\pdf.pdf", FileMode.Create, FileAccess.Write))
        //                            {
        //                                stream.CopyTo(fileStream);
        //                            }
        //                            break;
        //                        }
        //                    case DownloadStatus.Failed:
        //                        {
        //                            Console.WriteLine("Download failed.");
        //                            break;
        //                        }
        //                }
        //            };
        //    request.Download(stream);

        //    Console.ReadLine();
        //}
    }
}
