using System.Reflection;
using Microsoft.ApplicationInsights.Extensibility;
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
            .AddSingleton(s => configs.GetSection("AppInsights").Get<InsightsSettings>())
            .AddScoped<IInsightsService, InsightsService>()
            .AddSingleton<ITelemetryInitializer, TelemetryInitializer>()
            .AddApplicationInsightsTelemetry(opt =>
            {
               opt.InstrumentationKey = configs.GetSection("AppInsights").Get<InsightsSettings>().Key;
               // opt.EnablePerformanceCounterCollectionModule
               // opt.EnableAdaptiveSampling = true;
               // opt.AddAutoCollectedMetricExtractor = true;
            });
         return mvcBuilder;
      }

   }

}
