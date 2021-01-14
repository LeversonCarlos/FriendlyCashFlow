using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Shared
{

   public static partial class StartupExtentions
   {

      public static IMvcBuilder AddInsightsService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            // .AddSingleton(s => configs.GetSection("Identity").Get<IdentitySettings>())
            .AddScoped<IInsightsService, InsightsService>();
         return mvcBuilder;
      }

   }

}
