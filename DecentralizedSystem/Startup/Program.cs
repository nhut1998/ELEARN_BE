using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace DecentralizedSystem.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string rootdir = Directory.GetCurrentDirectory();
            try
            {
                string filePath = rootdir + "\\Resources\\Aspose.Total.txt";

                using (Stream licStream = new FileStream(filePath, FileMode.Open))
                {
                    licStream.Seek(0L, SeekOrigin.Begin);
                    Aspose.Words.License wordLicense = new Aspose.Words.License();
                    wordLicense.SetLicense(licStream);

                    licStream.Seek(0L, SeekOrigin.Begin);
                    Aspose.Cells.License cellLicense = new Aspose.Cells.License();
                    cellLicense.SetLicense(licStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere was an error setting the license: " + ex.Message);
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
