using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Elesse.Shared
{

   partial class Results
   {

      public static BadRequestObjectResult Info(string resource, params string[] messageList) =>
         new BadRequestObjectResult(Message(resource, enMessageType.Info, messageList));

      public static BadRequestObjectResult Warning(string resource, params string[] messageList) =>
         new BadRequestObjectResult(Message(resource, enMessageType.Warning, messageList));

      public static BadRequestObjectResult Error(string resource, params string[] messageList) =>
         new BadRequestObjectResult(Message(resource, enMessageType.Error, messageList));

      internal static Message[] Message(string resource, enMessageType type, params string[] messageList) =>
         messageList.Select(text => new Message(type, $"{resource}.{text}")).ToArray();

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
