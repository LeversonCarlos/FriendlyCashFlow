using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

      [DebuggerStepThrough]
      public void TrackEvent(string eventName, params string[] propertyList)
      {
         this.TrackEvent(eventName, GetPropertiesDictionary(propertyList));
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
      public void TrackMetric(string name, double value, params string[] propertyList) =>
         this.TrackMetric(name, value, GetPropertiesDictionary(propertyList));


      [DebuggerStepThrough]
      public void TrackException(Exception ex, Dictionary<string, string> properties)
      {
         try
         {
            if (_TelemetryClient != null)
               _TelemetryClient.TrackException(ex, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      public void TrackException(Exception ex, params string[] propertyList) =>
         this.TrackException(ex, GetPropertiesDictionary(propertyList));


      [DebuggerStepThrough]
      internal Dictionary<string, string> GetPropertiesDictionary(params string[] propertyList)
      {
         if (propertyList == null || propertyList.Length == 0)
            return null;
         var properties = propertyList
            .Select(prop => prop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries))
            .Select(prop => new
            {
               Key = (prop.Length == 2 ? prop[0] : "Property"),
               Value = (prop.Length == 2 ? prop[1] : string.Join(":", prop))
            })
            .ToDictionary(k => k.Key, v => v.Value);
         return properties;
      }

   }
}
