using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Import
{
   class Program
   {
      static void Main(string[] args)
      {
         try
         {
            Console.WriteLine("");
            Console.WriteLine("Friendly Cash Flow: Data Import");

            // APP SETTINGS
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            if (appSettings == null) { Console.WriteLine("Missing application settings file"); return; }

            // API SETTINGS
            if (appSettings.Api == null ||
               string.IsNullOrEmpty(appSettings.Api.Url) ||
               string.IsNullOrEmpty(appSettings.Api.Username) ||
               string.IsNullOrEmpty(appSettings.Api.Password))
            {
               Console.WriteLine("Invalid api settings"); return;
            }
            Console.WriteLine($" API: {appSettings.Api.Url}");
            Console.WriteLine($" User: {appSettings.Api.Username}");

         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); }
         finally { Console.WriteLine(""); }
      }
   }
}
