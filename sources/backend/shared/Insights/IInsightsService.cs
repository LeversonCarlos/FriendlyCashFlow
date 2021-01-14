using System;
using System.Collections.Generic;

namespace Elesse.Shared
{
   public interface IInsightsService
   {

      void TrackEvent(string eventName, Dictionary<string, string> properties);
      void TrackEvent(string eventName, params string[] propertyList);

      void TrackMetric(string name, double value, Dictionary<string, string> properties);
      void TrackMetric(string name, double value, params string[] propertyList);

      void TrackException(Exception ex, Dictionary<string, string> properties);
      void TrackException(Exception ex, params string[] propertyList);

   }
}
