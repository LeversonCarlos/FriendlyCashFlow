using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Elesse.Shared
{

   partial class Results
   {

      public static BadRequestObjectResult Info(params string[] messageList) =>
         Message(enMessageType.Info, messageList);

      static BadRequestObjectResult Message(enMessageType type, string[] messageList) =>
         Message(messageList.Select(text => new Message(type, text)).ToArray());

      static BadRequestObjectResult Message(Message[] messageList) =>
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
