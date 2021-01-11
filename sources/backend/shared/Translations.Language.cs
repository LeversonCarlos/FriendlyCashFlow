using System;
using System.Collections.Generic;
using System.Linq;

namespace Elesse.Shared
{
   partial class Translations
   {

      static string GetLanguageID(Microsoft.AspNetCore.Http.HttpContext _HttpContext) =>
         GetAcceptedLanguageIDs(_HttpContext)
            .Where(x => GetAvailabledLanguageID().Contains(x.Key))
            .OrderBy(x => x.Value)
            .Select(x => x.Key)
            .FirstOrDefault();

      static string[] GetAvailabledLanguageID() =>
         new string[] { "en-US", "pt-BR" };

      static Dictionary<string, double> GetAcceptedLanguageIDs(Microsoft.AspNetCore.Http.HttpContext _HttpContext)
      {
         var languageIDs = new Dictionary<string, double>();
         try
         {

            // FUNCTION TO ADD LANGUAGES AND ITS PRIORITY
            var add = new Action<string, double>((langID, order) =>
            {
               if (languageIDs.ContainsKey(langID)) languageIDs.Remove(langID);
               languageIDs.Add(langID, order);
            });

            // SET en-US WITH LOWEST PRIORITY AS A FALLBACK
            add("en-US", 9);

            // LOAD SETTINGS FROM BROWSER HEADERS
            // "pt-BR,pt;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6"
            var acceptLanguageHeader = _HttpContext?.Request.Headers
               ?.Where(h => h.Key == "Accept-Language")
               ?.Select(h => h.Value.ToString())
               ?.FirstOrDefault()
               ?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
               ?.ToArray();
            var acceptLanguages = acceptLanguageHeader
               ?.Select(x => x.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
               ?.Select(x => new { LangID = x.GetValue(0), Order = (x.Length == 1 ? "q=1.0" : x.GetValue(1)) })
               ?.Select(x => new { LangID = (string)x.LangID, Order = ((string)x.Order).Replace("q=", "") })
               ?.Select(x => new { x.LangID, x.Order })
               ?.ToArray();
            var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            foreach (var acceptLanguage in acceptLanguages)
            {
               if (!double.TryParse(acceptLanguage.Order, System.Globalization.NumberStyles.Float, cultureInfo, out var order)) order = 0;

               // invert order so 1.0 gets priority over 0.6
               order = (1 - order);

               add(acceptLanguage.LangID, order);
            }

         }
         catch { }
         return languageIDs;
      }

   }
}
