#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphCategoryDetails
   {
      public List<graphCategoryDetailsData> seriesData { get; set; }
      // public List<graphCategoryDetailsDrilldown> drilldownData { get; set; }
   }

   /*
   public class graphCategoryDetailsDrilldown : graphCategoryDetailsData
   {
      public List<graphCategoryDetailsDrilldown> drilldata { get; set; }
   }
   */

   public class graphCategoryDetailsData
   {
      public string id { get; set; }
      public string name { get; set; }
      public double value { get; set; }
      public double y { get { return this.value; } }
      public int colorIndex { get; set; }
      public double colorBrightness { get; set; }
      public string drilldown { get; set; }
      public List<graphCategoryDetailsData> drillData { get; set; }
   }

}