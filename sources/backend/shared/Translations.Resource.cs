using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Elesse.Shared
{

   partial class Translations
   {

      static string GetResourceName(Microsoft.AspNetCore.Http.HttpContext _HttpContext) =>
         _HttpContext.Request.Path
            .ToString()
            .ToLower()
            .Replace("/api/", "")
            .Replace("/translations", "");

      static string GetResourcePath(string resourceName, string languageID)
      {
         var rootPath = Assembly.GetEntryAssembly().Location;
         rootPath = Path.GetDirectoryName(rootPath);
         var resourcePath = Path.Combine(rootPath, "Translations", $"{resourceName}.{languageID}.json");
         return resourcePath;
      }

      static async Task<Dictionary<string, string>> GetResourceValues(string resourcePath)
      {
         var resourceContent = await File.ReadAllTextAsync(resourcePath);
         var resourceValues = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resourceContent);
         return resourceValues;
      }

   }

}
