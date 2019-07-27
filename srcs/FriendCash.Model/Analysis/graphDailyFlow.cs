#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphDailyFlow
   {

      public graphDailyFlow()
      {
         this.category = new List<int>();
         this.unplanned = new graphDailyFlow_Serie();
         this.unpaid = new graphDailyFlow_Serie();
         this.paid = new graphDailyFlow_Serie();
      }

      public List<int> category { get; set; }
      public int currentDay { get; set; }
      public graphDailyFlow_Serie unplanned { get; set; }
      public graphDailyFlow_Serie unpaid { get; set; }
      public graphDailyFlow_Serie paid { get; set; }

   }

   public class graphDailyFlow_Serie
   {
      public graphDailyFlow_Serie() { this.data = new List<double>(); }
      public string name { get; set; }
      public List<double> data { get; set; }
   }

}