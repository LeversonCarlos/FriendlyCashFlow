#region Using
using Newtonsoft.Json;
#endregion

namespace FriendCash.Model.Base
{
   public class Json
   {

      #region Serialize
      [System.Diagnostics.DebuggerStepThrough]
      public static string Serialize<T>(T value)
      {
         return JsonConvert.SerializeObject(value, Formatting.None, 
            new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
      }
      #endregion

      #region Deserialize
      [System.Diagnostics.DebuggerStepThrough]
      public static T Deserialize<T>(string value)
      {
         return JsonConvert.DeserializeObject<T>(value);
      }
      #endregion

   }
}
