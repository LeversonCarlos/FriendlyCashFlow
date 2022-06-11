using Friendly.CashFlow.Services;
using Microsoft.AspNetCore.Mvc;

namespace Friendly.CashFlow.Controllers;

public static class ControllersExtensions
{

   public static async Task<ActionResult<TResponse>> ToActionResult<TResponse>(this Task<TResponse> responseTask)
      where TResponse : SharedResponseModel
   {
      var response = await responseTask;
      return response;
   }

}
