using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.ApplicationInsights;

namespace Elesse.Shared
{
   internal class InsightsService : IInsightsService
   {

      public InsightsService(TelemetryClient telemetryClient)
      {
         _TelemetryClient = telemetryClient;
      }

      readonly TelemetryClient _TelemetryClient;

      [DebuggerStepThrough]
      public void TrackEvent(string eventName, Dictionary<string, string> properties)
      {
         try
         {
            if (_TelemetryClient != null)
               _TelemetryClient.TrackEvent(eventName, properties);
         }
         catch { }
      }

   }
}
