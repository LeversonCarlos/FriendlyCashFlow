using System.Collections.Generic;

namespace Elesse.Shared
{
   public interface IInsightsService
   {
      void TrackEvent(string eventName, Dictionary<string, string> properties);
   }
}
