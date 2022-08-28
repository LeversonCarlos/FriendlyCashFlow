using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lewio.Shared;

partial class ResponseModelExtension
{

   public static async Task<ActionResult<TResponse>> HandleAsync<TRequest, TResponse>(this Command<TRequest, TResponse> self, TRequest request, ModelStateDictionary modelState)
      where TRequest : RequestModel
      where TResponse : ResponseModel
   {
      if (!modelState.IsValid)
      {
         TResponse response = Activator.CreateInstance<TResponse>();
         response.OK = false;
         response.Messages = modelState.Values
            .SelectMany(x => x.Errors)
            .Select(x => Message.CreateWarning(x.ErrorMessage))
            .ToArray();
         return StatusObjectResult.BadRequest(response);
      }
      else
      {
         try
         {
            var response = await self.HandleAsync(request);
            response.EnsureValidResponse();
            return StatusObjectResult.Ok(response);
         }
         catch (Exception ex) { return StatusObjectResult.InternalError(ex); }
      }
   }

}
