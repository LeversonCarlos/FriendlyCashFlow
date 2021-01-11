using System.Reflection;
using System.Threading.Tasks;

namespace Elesse.Shared
{
   partial class Translations
   {
      public static async Task<Translations> CreateAsync(Microsoft.AspNetCore.Http.HttpContext _HttpContext)
      {
         var version = Assembly.GetEntryAssembly().GetName().Version;
         var languageID = GetLanguageID(_HttpContext);

         var translation = new Translations
         {
            Version = $"{version.Major}.{version.Minor}.{version.Build}",
            Language = languageID
         };
         await Task.CompletedTask;
         return translation;

      }
   }
}
