#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphCumulatedEntries
   {
      public List<string> categoryList { get; set; }
      public List<double> data { get; set; }
      public List<double> pareto { get; set; }
      public graphLimits limits { get; set; }
   }

}