using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Service.Base
{

   public class Bundle<T>
   {

      #region New
      public Bundle() 
      {
         this.Result = false;
         this.Messages = new List<BundleMessage>();
      }
      #endregion

      #region Data
      public T Data { get; set; }
      #endregion

      #region Result
      public bool Result { get; set; }
      #endregion

      #region Messages
      public List<BundleMessage> Messages { get; set; }
      #endregion

   }

   public class BundleMessage
   {
      public BundleMessage(string sText) { this.Text = sText; }
      public BundleMessage(string sText, enumType iType) { this.Text = sText; this.Type = iType; }
      public string Text { get; set; }
      public enumType Type { get; set; }
      public enum enumType : short { Message = 0, Information = 1, Warning = 2, Alert = 3 }
   }

}