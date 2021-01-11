using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elesse.Shared
{

   public partial class Translations
   {
      public string Version { get; private set; }
      public string Language { get; private set; }
      public Dictionary<string, string> Values { get; private set; }
   }

   partial class Translations
   {

      public static async Task<Translations> CreateAsync(ITranslationsProvider translationsProvider, Microsoft.AspNetCore.Http.HttpContext _HttpContext)
      {

         var translations = new Translations
         {
            Version = GetVersion(),
            Language = GetLanguageID(_HttpContext)
         };

         var resourceName = GetResourceName(_HttpContext);
         var resurcePath = GetResourcePath(resourceName, translations.Language);
         translations.Values = await translationsProvider.GetTranslationsResource(resourceName, translations.Language);

         return translations;
      }


   }

}
