using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{
   partial class Results
   {

      public static BadRequestObjectResult Info(string resource, params string[] messageList) =>
         new BadRequestObjectResult(GetResults(resource, enResultType.Info, messageList));

      public static BadRequestObjectResult Warning(string resource, params string[] messageList) =>
         new BadRequestObjectResult(GetResults(resource, enResultType.Warning, messageList));

      internal static Results GetResults(string resource, enResultType type, params string[] messageList) =>
         new Results(type, GetResultsMessages(resource, messageList));

      internal static string[] GetResultsMessages(string resource, params string[] messageList) =>
         messageList.Select(text => $"{resource}.{text}").ToArray();

      public static BadRequestObjectResult Exception(Exception ex) =>
         new BadRequestObjectResult(new Results(ex));

   }
}
