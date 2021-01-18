using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{

   partial class Results
   {

      public static BadRequestObjectResult Message<Message>(params Message messageList) =>
         new BadRequestObjectResult(messageList);

   }

   public enum enMessageType : short { Info = 0, Warning = 1, Error = 2 }
   public class Message
   {
      internal Message(enMessageType type, string text)
      {
         Type = type;
         Text = text;
      }
      public enMessageType Type { get; }
      public string Text { get; }
   }

}
