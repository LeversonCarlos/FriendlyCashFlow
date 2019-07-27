using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriendCash.Web
{
   public class Search
   {

      #region New
      public Search() { }
      public Search(object sValue) { if (sValue != null) { this.Value = sValue.ToString(); } }
      public Search(object sValue, bool bUseSettled) { this.UseSettled = bUseSettled; if (sValue != null) { this.Value = sValue.ToString(); } }
      #endregion

      #region Value
      public string Value
      {
         get
         {
            return "" +
               (this.UseSettled == true ? (this.ValueSettled == true ? "S1!" : "S0!") : "") +
               this.ValueSearch +
               "";
         }
         set
         {

            if (!string.IsNullOrEmpty(value))
            {

               // SETTLED
               ValueSettled = false;
               if (value.StartsWith("S0!") || value.StartsWith("S1!"))
               {
                  if (value.Substring(0, 3) == "S1!") { ValueSettled = true; }
                  if (value.Length == 3) { value = string.Empty; }
                  else { value = value.Substring(3); }
               }

               // VALUE
               if (!string.IsNullOrEmpty(value))
               {
                  ValueSearch = value;
               }

            }
         }
      }
      #endregion

      #region ValueSearch
      public string ValueSearch { get; set; }
      #endregion

      #region ValueSettled
      public bool ValueSettled { get; set; }
      #endregion

      #region UseSettled
      public bool UseSettled { get; set; }
      #endregion

    }
}