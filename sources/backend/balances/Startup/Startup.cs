using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Balances
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddBalanceService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<IBalanceRepository, BalanceRepository>()
            .AddScoped<IBalanceService, BalanceService>();
         try { MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<BalanceEntity>(); } catch { }
         return mvcBuilder;
      }

   }

}
