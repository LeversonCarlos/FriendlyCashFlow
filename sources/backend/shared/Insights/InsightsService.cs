using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

      [DebuggerStepThrough]
      public void TrackEvent(string eventName, params string[] propertyList)
      {
         try
         {
            var properties = GetPropertiesDictionary(propertyList);
            this.TrackEvent(eventName, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      public void TrackMetric(string name, double value, Dictionary<string, string> properties)
      {
         try
         {
            if (_TelemetryClient != null)
               _TelemetryClient.TrackMetric(name, value, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      public void TrackMetric(string name, double value, params string[] propertyList)
      {
         try
         {
            var properties = GetPropertiesDictionary(propertyList);
            this.TrackMetric(name, value, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      Dictionary<string, string> GetPropertiesDictionary(params string[] propertyList)
      {
         var properties = propertyList
            .Select(prop => prop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries))
            .Select(prop => new
            {
               Key = (prop.Length == 2 ? prop[0] : "Property"),
               Value = (prop.Length == 2 ? prop[1] : prop[0])
            })
            .ToDictionary(k => k.Key, v => v.Value);
         return properties;
      }

   }
}
