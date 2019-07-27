#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphBalanceTrend
   {
      public List<string> categoryList { get; set; }
      public graphBalanceTrendData IncomeData { get; set; }
      public graphBalanceTrendData ExpenseData { get; set; }
      public graphBalanceTrendData BalanceData { get; set; }
      public graphLimits limits { get; set; }
   }

   public class graphBalanceTrendData
   {
      public string name { get; set; }
      public List<double> data { get; set; }
   }

}