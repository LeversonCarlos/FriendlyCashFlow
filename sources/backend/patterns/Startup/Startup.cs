using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elesse.Patterns
{

   public static class StartupExtentions
   {

      public static IMvcBuilder AddPatternService(this IMvcBuilder mvcBuilder, IConfiguration configs)
      {
         var services =
            mvcBuilder.Services;
         services
            .AddScoped<IPatternRepository, PatternRepository>()
            .AddScoped<IPatternService, PatternService>();
         return mvcBuilder;
      }

   }

}
