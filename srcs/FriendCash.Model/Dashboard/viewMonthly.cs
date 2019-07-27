#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Reports.Model
{

   public class viewMonthlyParam 
   {

      public viewMonthlyParam() { }
      public viewMonthlyParam(DateTime date)
      {
         this.Year = date.Year;
         this.Month = (short)date.Month;
         this.Text = date.ToString("MMMM/yyyy", System.Globalization.CultureInfo.CurrentCulture).ToUpper();
      }

      public int Year { get; set; }
      public short Month { get; set; }
      public string Text { get; set; }
   }

   public class viewMonthSelector
   {
      public viewMonthlyParam PreviousMonth { get; set; }
      public viewMonthlyParam CurrentMonth { get; set; }
      public viewMonthlyParam NextMonth { get; set; }
   }

   public class viewMonthly
   {

      #region PlannedValue
      [Display(Description = "LABEL_REPORTS_MONTHLY_PLANNEDVALUE")]
      public double PlannedValue { get; set; }
      #endregion

      #region UnplannedValue
      [Display(Description = "LABEL_REPORTS_MONTHLY_UNPLANNEDVALUE")]
      public double UnplannedValue { get; set; }
      #endregion

      #region YearlyAverage
      [Display(Description = "LABEL_REPORTS_MONTHLY_YEARLYAVERAGE")]
      public double YearlyAverage { get; set; }
      #endregion

      #region MonthlyAverage
      [Display(Description = "LABEL_REPORTS_MONTHLY_MONTHLYAVERAGE")]
      public double MonthlyAverage { get; set; }
      #endregion

   }

}