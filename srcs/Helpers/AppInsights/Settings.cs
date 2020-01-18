using System.Collections.Generic;

namespace FriendlyCashFlow.Helpers.AppInsights
{
   public class Settings
   {
      public string InstrumentationKey { get; set; }
      public Dictionary<string, string> GlobalProperties { get; set; }
   }
}
