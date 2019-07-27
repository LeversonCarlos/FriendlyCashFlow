#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
#endregion

namespace FriendCash.Service.Dashboard.Model
{

   public class viewCategory
   {

      public short Type { get; set; }
      public string Category { get; set; }

      public double PaidValue { get; set; }
      public int PaidPercent { get; set; }

      public double UnpaidValue { get; set; }
      public int UnpaidPercent { get; set; }

      public double GoalValue { get; set; }

   }

}