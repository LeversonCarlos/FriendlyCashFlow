using Microsoft.AspNetCore.Mvc;

namespace Lewio.CashFlow.Services;

public static class ServicesExtensions
{

   public static async Task<ActionResult<TResponse>> ExecuteAsync<TRequest, TResponse>(this SharedService<TRequest, TResponse> self, TRequest request, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
      where TRequest : SharedRequestModel
      where TResponse : SharedResponseModel
   {
      if (!modelState.IsValid)
      {
         TResponse response = Activator.CreateInstance<TResponse>();
         response.OK = false;
         response.Messages = modelState.Values
            .SelectMany(x => x.Errors)
            .Select(x => SharedResponseModel.Message.CreateWarning(x.ErrorMessage))
            .ToArray();
         return response;
      }
      else
      {
         try
         {
            var response = await self.ExecuteAsync(request);
            if (response == null)
               throw new Exception("Unexpected empty response");
            if (!response.OK)
               return new BadRequestObjectResult(response);
            return new OkObjectResult(response);
         }
         catch (Exception ex) { return new InternalServerErrorObjectResult(ex); }
      }
   }

   public class InternalServerErrorObjectResult : ObjectResult
   {
      public InternalServerErrorObjectResult(object value) : base(value)
      {
         StatusCode = StatusCodes.Status500InternalServerError;
      }
   }

}
