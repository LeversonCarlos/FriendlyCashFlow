#region Using
using System;
#endregion 

namespace FriendCash.Service.Base
{

   public class viewRelated : IDisposable
   {

      public string ID { get; set; }

      public string textValue { get; set; }

      public string htmlValue { get; set; }

      public string jsonValue { get; set; }

      #region Dispose
      public void Dispose()
      {
         //throw new NotImplementedException();
      }
      #endregion

   }

}
