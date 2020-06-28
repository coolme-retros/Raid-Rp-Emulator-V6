using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plus.HabboHotel;
using System.Reflection;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO.Compression;
using System.Diagnostics;

namespace Plus
{
    class Auto_Updater
    {
        private const string AppFileName = "Raid.zip";
        private const string UpdateUrl = "https://raw.githubusercontent.com/coolme-retros/Raid-Rp-Emulator-V6/master/update";

        private static void ChangeStats(string stats)
        {
            AbstractBar bar = new AnimatedBar();
            const int wait = 15, end = 5;
            Game.Progress(bar, wait, end, stats);
        }
        private void GetVersion()
        {
            var current = Assembly.GetExecutingAssembly().GetName().Version;
            Console.Write($"Current version: {current}");
            ChangeStats("");
        }




        public static void CheckUpdate()
        {
            try
            {
                //ChangeStats("Checking for updates...");
                AbstractBar bar = new AnimatedBar();
                Game.Progress(bar, wait: 15, end: 5, "Checking for updates..");

                //Get the current version
                var current = Assembly.GetExecutingAssembly().GetName().Version;
                using (var client = new WebClient())
                {
                    var stream = client.OpenRead(UpdateUrl);

                    if (stream != null)
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            var result = sr.ReadToEnd();

                            //Deserialize Object to ApplicationsInfo
                            var app = JsonConvert.DeserializeObject<ApplicationInfo>(result);
                            var version = Version.Parse(app.Version);
                            var versionResult = version.CompareTo(current);

                            //check for new version available
                            if (versionResult > 0)
                            {
                                Console.WriteLine("There is a new version available, would you like to download it?\r\n" +
                                                              $"What's new?\r\n" +
                                                             $"Description:\r\n{app.Descriptions}\r\n\r\n" +
                                                           $"Version:{app.Version}" +
                                                         $"Pre Release?:{app.Pre}");
                                string response = Console.ReadLine();
                                //Optional if you want to inform the user to download the update.
                                //var response = MessageBox.Show("There is a new version available, would you like to download it?\r\n" +
                                //                              $"What's new?\r\n" +
                                //                            $"Description:\r\n{app.Descriptions}\r\n\r\n" +
                                //                          $"Version:{app.Version}"
                                //, "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (response == "Yes" | response == "yes" | response == "y" | response == "Y")
                                {
                                    ChangeStats("Downloading update...");
                                    //Download the update
                                    DownloadUpdate(app.DownloadUrl);
                                }
                                else
                                {
                                    Console.Write("Okay...");
                                }
                            }
                            else
                            {
                                Console.Write("Your are using the latest version.");
                                ChangeStats("");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //If it could not connect to update link, check for google connection. If it fails aswell. send a Netowrk connection else, update service unavailable
                using (var client = new WebClient())
                {
                    try
                    {
                        client.OpenRead("https://www.google.com");
                    }
                    catch (Exception ee3)
                    {
                        Console.WriteLine("Could not check for updates. Please Check your network connection.");
                        return;
                    }
                }
             
                Console.Write("The update service is unavaliable at this time. Please try again later.");
                return;
            }
        }
        private static void DownloadUpdate(string downloadUrl)
        {

            var url = new Uri(downloadUrl);

            using (var client = new WebClient())
            {
                client.DownloadFileAsync(url, AppFileName);
                client.DownloadProgressChanged += ClientOnDownloadProgressChanged;
                client.DownloadFileCompleted += ClientOnDownloadFileCompleted;
                
            }
        }
        private static void ClientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.Write(e.ToString());
                
            }

            var fileInfo = new FileInfo(AppFileName);

            var fileNameWithoutExt = fileInfo.Name.Replace(fileInfo.Extension, "");

            //	This will extract in the current directory->baseDir / Updates / FileNameWithoutExt->extracted files
            var outputDir = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Updates", fileNameWithoutExt);
            Console.Write(outputDir);
            //Prevent UI threads locking
            Task.Run(async () =>
            {

                ChangeStats("Extracting file...");
                try
                {
                    //TODO: Find better extractor tool

                    if (Directory.Exists(outputDir))
                    {
                        Directory.Delete(outputDir, true);
                    }

                    //Extract the downloaded to the current directory
                   // ZipFile.Extract working only in .zip  file
                    ZipFile.ExtractToDirectory(AppFileName, outputDir);

                    ChangeStats("File Extracted!");
                    await Task.Delay(1000);

                }
                catch (Exception exception)
                {
                    Console.Write("The File is corrupted or: \r\n" + exception);
                }

            }).ContinueWith(_ =>
            {
                //Delete the downloaded file
                if (File.Exists(AppFileName))
                {
                    File.Delete(AppFileName);
                }

                
                    ExecuteNewUpdate(outputDir);
               


            });

        }
        private static void ClientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // var stats = string.Concat("Downloading ", e.ProgressPercentage, " %", "...");
            //ChangeStats(stats);
            AbstractBar bar = new AnimatedBar();
            Game.Progress(bar, wait: 15, end: 5, "Downloading " + e.ProgressPercentage + "%" + "...");
            //progressBar1.Value = e.ProgressPercentage;
        }
        private static void ExecuteNewUpdate(string appPath)
        {

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var updateSource = $"{appPath}";
            var updateDestination = $"{appDirectory}";

            //See more details in https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/xcopy

            string arg = $"/c timeout /t 2& xcopy {updateSource} {updateDestination} /y/c/q&  Plus Emulator.exe";

            var process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = updateDestination;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = arg;

            ////Display no windows
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            process.StartInfo = startInfo;
            process.Start();

            //Close();
        }
        public class ApplicationInfo
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string Descriptions { get; set; }
            public string DownloadUrl { get; set; }
            public string Pre { get; set; }
        }



    }
}
