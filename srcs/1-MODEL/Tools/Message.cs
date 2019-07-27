using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendCash.Model.Tools
{
   public class Message
   {

      #region Text
      public string Text { get; set; }
      #endregion

      #region Type
      public enum enType : short { Message = 0, Information = 1, Warning = 2, Exception = 3 }
      public enType Type { get; set; }
      #endregion

      #region Contextual
      public string Information { set { this.Text = value; this.Type = enType.Information; } }
      public string Warning { set { this.Text = value; this.Type = enType.Warning; } }
      public string Exception { set { this.Text = value; this.Type = enType.Exception; } }
      #endregion

   }
}
