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

   public async Task<string> GetString(string value)
   {
      try
      {
         await Task.CompletedTask;
         return _StringLocalizer[value];
      }
      catch { return $"{value.ToUpper().Replace(" ", "_")}"; }
   }

   public void Dispose()
   {
   }

}
