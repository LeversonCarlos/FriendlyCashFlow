using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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

      public static async Task<ActionResult<Translations>> CreateAsync(Microsoft.AspNetCore.Http.HttpContext _HttpContext)
      {
         try
         {
            var translations = new Translations
            {
               Version = GetVersion(),
               Language = GetLanguageID(_HttpContext)
            };

            var resourceName = GetResourceName(_HttpContext);
            var resourcePath = GetResourcePath(resourceName, translations.Language);
            translations.Values = await GetResourceValues(resourcePath);

            return translations;
         }
         catch (Exception ex) { return new BadRequestObjectResult(new string[] { $"EXCEPTION_{ex.Message}" }); }
      }


   }

}
