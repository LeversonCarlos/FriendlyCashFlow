using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace FriendlyCashFlow.Helpers.AppInsights
{
   public static class Extension
   {
      public static IServiceCollection AddApplicationInsights(this IServiceCollection services, Settings settings)
      {
         return services
            .AddSingleton<ITelemetryInitializer, TelemetryInitializer>((serviceProvider) =>
            {
               return new TelemetryInitializer { GlobalProperties = settings.GlobalProperties };
            })
            .AddApplicationInsightsTelemetry(opt =>
            {
               opt.InstrumentationKey = settings.InstrumentationKey;
               // opt.EnablePerformanceCounterCollectionModule
               // opt.EnableAdaptiveSampling = true;
               // opt.AddAutoCollectedMetricExtractor = true;
            });
      }
   }
}
