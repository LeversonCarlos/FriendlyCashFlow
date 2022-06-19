using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
namespace Lewio.CashFlow.Shared;

public class Localizator : ILocalization
{

   public Localizator(IServiceProvider serviceProvider)
   {
      _StringLocalizer = serviceProvider.GetService<IStringLocalizer<Lewio.CashFlow.Shared.Localization.Resources.Strings>>()!;
   }
   readonly IStringLocalizer<Lewio.CashFlow.Shared.Localization.Resources.Strings> _StringLocalizer;

   public async Task<string> GetString(string key)
   {
      try
      {
         await Task.CompletedTask;
         var value = _StringLocalizer[key];
         if (string.IsNullOrEmpty(value) || value == key)
            throw new Exception("Not Found");
         return value;
      }
      catch { return $"{key.ToUpper().Replace(" ", "_")}"; }
   }

   public void Dispose()
   {
   }

}
