using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FriendCash.Web.Helper
{
   public class Json
   {

      #region SerializeObject
      public static string SerializeObject<T>(T value)
      { return Newtonsoft.Json.JsonConvert.SerializeObject(value); }
      #endregion

      #region DeserializeObject
      public static T DeserializeObject<T>(string value)
      { return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value); }
      #endregion

   }
}