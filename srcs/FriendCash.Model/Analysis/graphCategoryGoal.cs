#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphCategoryGoal
   {
      public short maxValue { get; set; }
      public List<string> categoryList { get; set; }
      public graphCategoryGoalData paidData { get; set; }
      public graphCategoryGoalData unpaidData { get; set; }
   }

   public class graphCategoryGoalData
   {
      public string name { get; set; }
      public List<graphCategoryGoalDataValue> data { get; set; }
   }

   public class graphCategoryGoalDataValue
   {
      public short y { get; set; }
      public double realValue { get; set; }
      public double goalValue { get; set; }
      public int colorIndex { get; set; }
      public double colorBrightness { get; set; }
   }


}