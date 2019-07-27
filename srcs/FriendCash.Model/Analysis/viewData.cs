#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class viewData
   {
      public viewParam currentMonth { get; set; }
      public viewDataDimensions Dimensions { get; set; }

   }

   public class viewDataDimensions
   {
      public List<viewDimension<long>> Accounts { get; set; }
      public List<viewDimension<long>> Categories { get; set; }
      public List<viewDimension<long>> Patterns { get; set; }
      public List<viewDimension<short>> Planned { get; set; }
      public List<viewDimension<short>> Paid { get; set; }
   }

}