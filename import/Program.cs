using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Import
{
   class Program
   {
      static void Main(string[] args)
      {

         var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
         var configuration = builder.Build();
         var appSettingsSection = configuration.GetSection("AppSettings");
         var appSettings = appSettingsSection.Get<AppSettings>();

         Console.WriteLine("Hello World!");
      }
   }
}
