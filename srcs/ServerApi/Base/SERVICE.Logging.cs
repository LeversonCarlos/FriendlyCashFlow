using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FriendlyCashFlow.API.Base
{
   partial class BaseService
   {

      [DebuggerStepThrough]
      protected void TrackEvent(string eventName, params string[] propertyList)
      {
         try
         {
            var properties = propertyList
               .Select(prop => prop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries))
               .Select(prop => new
               {
                  Key = (prop.Length == 2 ? prop[0] : "Property"),
                  Value = (prop.Length == 2 ? prop[1] : prop[0])
               })
               .ToDictionary(k => k.Key, v => v.Value);
            this.TrackEvent(eventName, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      protected void TrackEvent(string eventName, Dictionary<string, string> properties)
      {
         try
         {
            var telemetry = this.GetService<TelemetryClient>();
            if (telemetry != null)
            { telemetry.TrackEvent(eventName, properties); }
         }
         catch { }
      }

      [DebuggerStepThrough]
      protected void TrackMetric(string name, double value, params string[] propertyList)
      {
         try
         {
            var properties = propertyList
               .Select(prop => prop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries))
               .Select(prop => new
               {
                  Key = (prop.Length == 2 ? prop[0] : "Property"),
                  Value = (prop.Length == 2 ? prop[1] : prop[0])
               })
               .ToDictionary(k => k.Key, v => v.Value);
            this.TrackMetric(name, value, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      protected void TrackMetric(string name, double value, Dictionary<string, string> properties)
      {
         try
         {
            var telemetry = this.GetService<TelemetryClient>();
            if (telemetry != null)
            { telemetry.TrackMetric(name, value, properties); }
         }
         catch { }
      }

      [DebuggerStepThrough]
      protected void TrackException(Exception ex, params string[] propertyList)
      {
         try
         {
            var properties = propertyList
               .Select(prop => prop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries))
               .Select(prop => new
               {
                  Key = (prop.Length == 2 ? prop[0] : "Property"),
                  Value = (prop.Length == 2 ? prop[1] : prop[0])
               })
               .ToDictionary(k => k.Key, v => v.Value);
            this.TrackException(ex, properties);
         }
         catch { }
      }

      [DebuggerStepThrough]
      protected void TrackException(Exception ex, Dictionary<string, string> properties = null)
      {
         try
         {
            var telemetry = this.GetService<TelemetryClient>();
            if (telemetry != null)
            { telemetry.TrackException(ex, properties); }
         }
         catch { }
      }

   }
}
