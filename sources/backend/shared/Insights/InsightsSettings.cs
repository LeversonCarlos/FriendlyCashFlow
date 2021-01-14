using System.Collections.Generic;

namespace Elesse.Shared
{
   internal class InsightsSettings
   {
      public bool Activated { get; set; }
      public string Key { get; set; }
      public Dictionary<string, string> GlobalProperties { get; set; }
   }
}
