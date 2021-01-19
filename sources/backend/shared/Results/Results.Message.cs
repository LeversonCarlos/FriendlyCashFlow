using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Elesse.Shared
{

   partial class Results
   {

      public static BadRequestObjectResult Info(params string[] messageList) =>
         new BadRequestObjectResult(Message(enMessageType.Info, messageList));

      public static BadRequestObjectResult Warning(params string[] messageList) =>
         new BadRequestObjectResult(Message(enMessageType.Warning, messageList));

      public static BadRequestObjectResult Error(params string[] messageList) =>
         new BadRequestObjectResult(Message(enMessageType.Error, messageList));

      internal static Message[] Message(enMessageType type, params string[] messageList) =>
         messageList.Select(text => new Message(type, text)).ToArray();

   }

   public enum enMessageType : short { Info = 0, Warning = 1, Error = 2 }

   public class Message : ValueObject
   {
      internal Message(enMessageType type, string text)
      {
         Type = type;
         Text = text;
      }
      public enMessageType Type { get; }
      public string Text { get; }
      protected override IEnumerable<object> GetAtomicValues()
      {
         yield return Type;
         yield return Text;
      }
   }

}
