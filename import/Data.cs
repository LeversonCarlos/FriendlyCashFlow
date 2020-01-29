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
            Console.WriteLine($" Reading: {fileName}");
            using (var fileReader = File.OpenText(fileName))
            {
               var csv = new CsvReader(fileReader, System.Globalization.CultureInfo.CurrentCulture);
               csv.Configuration.HasHeaderRecord = false;
               csv.Configuration.Delimiter = ";";
               // csv.Read();
               return csv.GetRecords<T>().ToList();
            }
         }
         catch (Exception ex) { Console.WriteLine($" Exception: {ex.Message}"); return null; }
      }

   }
}
