#region Using
using System;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{
   public class viewEntry
   {
      public viewEntry() { this.Selected = true; }

      #region SearchDate

      public string SearchDate { get; set; }

      public int SearchYear
      {
         get
         {
            if (this.SearchDate.Length < 4) { return 0; }
            else { return int.Parse(this.SearchDate.Substring(0, 4)); }
         }
      }

      public int SearchMonth
      {
         get
         {
            if (this.SearchDate.Length < 6) { return 0; }
            else { return int.Parse(this.SearchDate.Substring(4, 2)); }
         }
      }

      public int SearchDay
      {
         get
         {
            if (this.SearchDate.Length < 8) { return 0; }
            else { return int.Parse(this.SearchDate.Substring(6, 2)); }
         }
      }

      #endregion

      #region idCategory
      public long idCategory { get; set; }
      #endregion

      #region idPattern
      public long idPattern { get; set; }
      #endregion

      #region Type
      public short Type { get; set; }
      #endregion

      #region Planned
      public short Planned { get; set; }
      #endregion

      #region Paid
      public short Paid { get; set; }
      #endregion

      #region idAccount
      public long idAccount { get; set; }
      #endregion

      #region Value
      public double Value { get; set; }
      #endregion

      #region Selected
      public bool Selected { get; set; }
      #endregion

   }
}