#region Using
using System.Collections.Generic;
#endregion

namespace FriendCash.Auth.Model
{
   public class viewRoleUsers
   {      
      public string ID { get; set; }
      public List<string> EnrolledUsers { get; set; }
      public List<string> RemovedUsers { get; set; }
   }
}