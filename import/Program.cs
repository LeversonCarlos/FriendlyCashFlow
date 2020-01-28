using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Import
{
   class Program
   {
      async static Task Main(string[] args)
      {
         try
         {
            Console.WriteLine("");
            Console.WriteLine("Friendly Cash Flow: Data Import");

            // APP SETTINGS
            var appSettings = AppSettings.Create();
            if (appSettings == null) { return; }

            // API CLIENT
            var apiClient = new ApiClient(appSettings.Api);
            if (!await apiClient.AuthAsync()) { return; }

            var entries = Data.ToList<Entry>("entries.csv");
            if (entries == null) { return; }
            Console.WriteLine($" Entries: {entries.Count}");


         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); }
         finally { Console.WriteLine(""); }
      }
   }
}
