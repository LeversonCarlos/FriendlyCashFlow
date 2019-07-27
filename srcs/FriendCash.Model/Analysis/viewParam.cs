#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{
   public class viewParam
   {

      public viewParam() { }
      public viewParam(DateTime date, System.Globalization.CultureInfo cultureInfo)
      {

         this.Text = date.ToString("MMM/yyyy", cultureInfo).ToUpper();
         this.Year = date.Year;
         this.Month = (short)date.Month;

         this.InitialDate = new DateTime(this.Year, this.Month, 1);
         this.FinalDate = this.InitialDate.AddMonths(1).AddDays(-1);

      }

      public string Text { get; set; }
      public int Year { get; set; }
      public short Month { get; set; }

      public DateTime InitialDate { get; set; }
      public DateTime FinalDate { get; set; }

   }
}