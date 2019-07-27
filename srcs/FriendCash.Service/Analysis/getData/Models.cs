#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{

   public class viewDataTemp : viewData
   {
      internal List<viewEntry> Daily { get; set; }
      internal List<viewEntry> Monthly { get; set; }
   }

}