using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace FriendlyCashFlow.Identity
{

   partial class IdentityService
   {

      public async Task<ActionResult<TokenVM>> TokenAuthAsync(TokenAuthVM param)
      {
         try
         {

            // RESULT
            await Task.CompletedTask;
            TokenVM result = null;
            return new OkObjectResult(result);
         }
         catch (Exception ex) { return new BadRequestObjectResult(new string[] { ex.Message }); }
      }

   }

   partial interface IIdentityService
   {
      Task<ActionResult<TokenVM>> TokenAuthAsync(TokenAuthVM param);
   }

   partial struct WARNINGS
   {
   }

}
