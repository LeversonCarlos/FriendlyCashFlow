using Microsoft.AspNetCore.Mvc;

namespace Elesse.Shared
{
   partial class Results
   {

      public static OkObjectResult Ok<T>(T value) =>
         new OkObjectResult(value);

      public static OkResult Ok() =>
         new OkResult();

   }
}
