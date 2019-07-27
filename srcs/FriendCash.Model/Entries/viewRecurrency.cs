#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Entries.Model
{
   public class viewRecurrency 
   {

      #region New
      public viewRecurrency() {
         this.Type = enRecurrencyType.Monthly;
      }
      public viewRecurrency(bindRecurrency Value) : this()
      {
         this.idRecurrency = Value.idRecurrency;
         this.Type = Value.Type;
         this.Fixed = Value.Fixed;
         this.Quantity = Value.Quantity;
         this.Update = enRecurrencyUpdate.Current;
         this.hasRecurrency = true;
      }
      #endregion

      #region idRecurrency
      [Display(Description = "LABEL_RECURRENCIES_IDRECURRENCY")]
      public long idRecurrency { get; set; }
      #endregion

      #region hasRecurrency
      [Display(Description = "LABEL_RECURRENCIES_HASRECURRENCY")]
      public bool hasRecurrency { get; set; }
      #endregion

      #region Type
      [Display(Description = "LABEL_RECURRENCIES_TYPE")]
      public enRecurrencyType Type { get; set; }
      #endregion

      #region Fixed
      [Display(Description = "LABEL_RECURRENCIES_FIXED")]
      public bool Fixed { get; set; }
      #endregion

      #region Quantity
      [Display(Description = "LABEL_RECURRENCIES_QUANTITY")]
      public int? Quantity { get; set; }
      #endregion

      #region Update
      [Display(Description = "LABEL_RECURRENCIES_UPDATE")]
      public enRecurrencyUpdate Update { get; set; }
      #endregion

      /*
      #region Create

      public static bindPattern Create(viewEntry value)
      {
         return new Model.bindPattern()
         {
            Type = value.Type,
            Text = value.Text,
            idCategory = value.idCategory.Value
         };
      }

      #endregion
      */

   }

}
