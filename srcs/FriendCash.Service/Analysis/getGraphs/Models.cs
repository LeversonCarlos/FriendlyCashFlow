#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class graphMainTemp : graphMain
   {
      public new viewDataTemp data { get; set; }
      public Dictionary<string, short> mainCategories { get; set; }
   }

}