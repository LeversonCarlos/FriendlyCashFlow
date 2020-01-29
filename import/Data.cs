using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Import
{
   public class Data
   {

      public static List<T> ToList<T>(string fileName)
      {
         try
         {
            Console.WriteLine($" Info: Reading file [{fileName}]");
            using (var fileReader = File.OpenText(fileName))
            {
               var csv = new CsvReader(fileReader, System.Globalization.CultureInfo.CurrentCulture);
               csv.Configuration.HasHeaderRecord = false;
               csv.Configuration.Delimiter = ";";
               // csv.Read();
               var result = csv.GetRecords<T>().ToList();
               Console.WriteLine($" RowsCount: {result?.Count ?? 0}");
               return result;
            }
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); return null; }
      }

   }
}
