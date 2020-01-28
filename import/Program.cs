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
            var appSettings = AppSettings.Create();
            if (appSettings == null) { return; }


         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); }
         finally { Console.WriteLine(""); }
      }
   }
}
