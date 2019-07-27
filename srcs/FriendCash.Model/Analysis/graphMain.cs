#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphMain
   {
      public viewData data { get; set; }

      public graphDailyFlow dailyFlow { get; set; }
      public graphCategoryDetails categoryExpenseDetails { get; set; }
      public graphCategoryDetails categoryIncomeDetails { get; set; }
      public graphCategoryTrend categoryTrend { get; set; }
      public graphCategoryGoal categoryGoal { get; set; }
      public graphBalanceTrend balanceTrend { get; set; }
      public graphCumulatedEntries cumulatedEntries { get; set; }

   }

   public class graphLimits
   {
      public graphLimits() { this.limitsValue = new List<double>(); }
      public double minValue { get; set; }
      public double maxValue { get; set; }
      public double tickInterval { get; set; }
      public List<double> limitsValue { get; set; }
   }

}