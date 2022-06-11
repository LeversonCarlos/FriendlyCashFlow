using Lewio.CashFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lewio.CashFlow.Controllers;

public static class ControllersExtensions
{

   public static async Task<ActionResult<TResponse>> ToActionResult<TResponse>(this Task<TResponse> responseTask)
      where TResponse : SharedResponseModel
   {
      try
      {
         var response = await responseTask;
         if (response == null)
            throw new Exception("Unexpected empty response");
         if (!response.OK)
            return new BadRequestObjectResult(response);
         return new OkObjectResult(response);
      }
      catch (Exception ex) { return new InternalServerErrorObjectResult(ex); }
   }

   public class InternalServerErrorObjectResult : ObjectResult
   {
      public InternalServerErrorObjectResult(object value) : base(value)
      {
         StatusCode = StatusCodes.Status500InternalServerError;
      }
   }

}
