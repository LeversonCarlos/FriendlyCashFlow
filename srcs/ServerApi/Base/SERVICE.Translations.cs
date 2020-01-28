using Microsoft.Extensions.Localization;

namespace FriendlyCashFlow.API.Base
{
   partial class BaseService
   {

      public string GetTranslation(string key)
      {
         try
         {
            var localizer = this.GetService<IStringLocalizer<FriendlyCashFlow.Translations.Strings>>();
            var result = localizer[key];
            return result;
         }
         catch { return $"{key.ToUpper().Replace(" ", "_")}"; }
      }

   }
}
