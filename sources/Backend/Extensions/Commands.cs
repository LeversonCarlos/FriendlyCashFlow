using Microsoft.AspNetCore.Mvc;
using Lewio.CashFlow.Shared;
namespace Lewio.CashFlow;

public static class CommandsExtensions
{

   public static async Task<ActionResult<TResponse>> ExecuteAsync<TRequest, TResponse>(
         this SharedCommand<TRequest, TResponse> self,
         TRequest request,
         Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState
      )
      where TRequest : SharedRequestModel
      where TResponse : SharedResponseModel
   {
      if (!modelState.IsValid)
         return ActionResults.Warning(modelState);
      else
      {
         try
         {
            var response = await self.ExecuteAsync(request);
            if (response == null)
               throw new Exception("Unexpected empty response");
            if (!response.OK)
               return ActionResults.Warning(response);
            return ActionResults.Ok(response);
         }
         catch (Exception ex) { return ActionResults.Error(ex); }
      }
   }

}
