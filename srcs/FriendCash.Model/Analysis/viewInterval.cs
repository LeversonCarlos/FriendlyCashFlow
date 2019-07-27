#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#endregion

namespace FriendCash.Service.Analysis.Model
{
   public class viewInterval
   {
      public viewParam PreviousMonth { get; set; }
      public viewParam CurrentMonth { get; set; }
      public viewParam NextMonth { get; set; }
   }
}