#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphCategoryTrend
   {
      public List<string> categoryList { get; set; }
      public List<graphCategoryTrendData> seriesData { get; set; }
      public graphLimits limits { get; set; }
   }

   public class graphCategoryTrendData
   {
      public string name { get; set; }
      public List<double> data { get; set; }
      public int colorIndex { get; set; }
   }

}