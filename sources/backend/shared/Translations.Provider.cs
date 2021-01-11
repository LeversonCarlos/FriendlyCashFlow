using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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

   }

   public interface ITranslationsProvider
   {
      Task<Dictionary<string, string>> GetTranslationsResource(string resourceName, string languageID);
   }

   public class TranslationsProvider : ITranslationsProvider
   {

      public TranslationsProvider(IWebHostEnvironment environment)
      {
         _HostEnvironment = environment;
      }

      readonly IWebHostEnvironment _HostEnvironment;

      public async Task<Dictionary<string, string>> GetTranslationsResource(string resourceName, string languageID)
      {
         var rootPath = Assembly.GetEntryAssembly().Location;
         rootPath = System.IO.Path.GetDirectoryName(rootPath);
         var resourcePath = Path.Combine(rootPath, "Translations", $"{resourceName}.{languageID}.json");
         var resourceContent = await File.ReadAllTextAsync(resourcePath);
         var resourceValues = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resourceContent);
         return resourceValues;
      }

   }

}
