using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
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
            Console.ForegroundColor = System.ConsoleColor.Blue;
            Console.WriteLine("");
            Console.WriteLine("Friendly Cash Flow: Data Import");
            var clearDataBefore = args?.Contains("--clear");

            // APP SETTINGS
            var appSettings = AppSettings.Create();
            if (appSettings == null) { return; }

            // API CLIENT
            var apiClient = new ApiClient(appSettings.Api);
            if (!await apiClient.AuthAsync()) { return; }

            // DATA
            var entries = Data.ToList<Entry>("entries.csv");
            var transfers = Data.ToList<Transfer>("transfers.csv");

            // IMPORT
            await apiClient.ImportAsync(entries, transfers, clearDataBefore);

         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); }
         finally { Console.WriteLine(""); Console.ForegroundColor = System.ConsoleColor.White; }
      }
   }
}
