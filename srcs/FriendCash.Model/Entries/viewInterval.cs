#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Entries.Model
{

   public class viewFilter
   {
      public viewFilterMonth PreviousMonth { get; set; }
      public viewFilterMonth CurrentMonth { get; set; }
      public viewFilterMonth NextMonth { get; set; }
      public Base.viewRelated CurrentAccount { get; set; }
      public List<Base.viewRelated> AccountList { get; set; }
   }

   public class viewFilterParam
   {
      public int Year { get; set; }
      public short Month { get; set; }
      public long Account { get; set; }
   }

   public class viewFilterMonth : viewFilterParam
   {

      public viewFilterMonth() { }
      public viewFilterMonth(DateTime date, System.Globalization.CultureInfo cultureInfo)
      {

         this.Text = date.ToString("MMM/yyyy", cultureInfo).ToUpper();
         this.Year = date.Year;
         this.Month = (short)date.Month;

         this.InitialDate = new DateTime(this.Year, this.Month, 1);
         this.FinalDate = this.InitialDate.AddMonths(1).AddDays(-1);

      }

      public string Text { get; set; }
      public DateTime InitialDate { get; set; }
      public DateTime FinalDate { get; set; }

   }

}