using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Import
{

   public class AppSettings
   {
      public AppSettings_Api Api { get; set; }

      public static AppSettings Create()
      {

         // LOAD SETTINGS
         var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
         var configuration = builder.Build();
         var appSettingsSection = configuration.GetSection("AppSettings");
         var appSettings = appSettingsSection.Get<AppSettings>();
         if (appSettings == null) { Console.WriteLine(" Warning: Missing application settings file"); return null; }

         // VALIDATE API SETTINGS
         if (appSettings.Api == null ||
            string.IsNullOrEmpty(appSettings.Api.Url) ||
            string.IsNullOrEmpty(appSettings.Api.Username) ||
            string.IsNullOrEmpty(appSettings.Api.Password))
         {
            Console.WriteLine(" Warning: Invalid api settings"); return null;
         }

         // LOG
         Console.WriteLine($" API: {appSettings.Api.Url}");
         Console.WriteLine($" User: {appSettings.Api.Username}");
         return appSettings;
      }

   }

   public class AppSettings_Api
   {
      public string Url { get; set; }
      public string Username { get; set; }
      public string Password { get; set; }
   }

}
