#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{
   public class viewDimension<T> 
   {
      public viewDimension() { this.Selected = true; }

      #region ID
      public T ID { get; set; }
      public T keyID { get; set; }
      #endregion

      #region Text
      public string ParentText { get; set; }
      public string Text { get; set; }
      #endregion

      #region Type
      public short Type { get; set; }
      #endregion  

      #region Selected
      public bool Selected { get; set; }
      #endregion

   }
}