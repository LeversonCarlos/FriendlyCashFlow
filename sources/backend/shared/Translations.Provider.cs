using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Elesse.Shared
{

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
         var resourcePath = Path.Combine(_HostEnvironment.ContentRootPath, "Translations", $"{resourceName}.{languageID}.json");
         var resourceContent = await File.ReadAllTextAsync(resourcePath);
         var resourceValues = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resourceContent);
         return resourceValues;
      }

   }

}
