using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;

namespace Elesse.Shared
{
   internal class TelemetryInitializer : ITelemetryInitializer
   {

      public TelemetryInitializer(InsightsSettings insightsSettings)
      {
         GlobalProperties = insightsSettings.GlobalProperties;
      }
      Dictionary<string, string> GlobalProperties { get; set; }

      public void Initialize(ITelemetry telemetry)
      {
         if (telemetry.Context.GlobalProperties.Count == 0 && this.GlobalProperties != null && this.GlobalProperties.Count != 0)
         {
            foreach (var globalProperty in this.GlobalProperties)
            { telemetry.Context.GlobalProperties.Add(globalProperty); }
         }
         // telemetry.Context.Component.Version = "something"
      }

   }
}
