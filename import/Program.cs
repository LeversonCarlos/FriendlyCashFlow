using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
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

            // ENTRIES
            var entries = Data.ToList<Entry>("entries.csv");            

            // TRANSFERS
            // TODO

            // IMPORT
            var importResult = await apiClient.ImportAsync(entries);


         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); }
         finally { Console.WriteLine(""); }
      }
   }
}
