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

            // VALIDATE PARAMETERS
            if (param == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER });
            if (string.IsNullOrWhiteSpace(param.RefreshToken))
               return new BadRequestObjectResult(new string[] { WARNINGS.INVALID_TOKENAUTH_PARAMETER });

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
      internal const string INVALID_TOKENAUTH_PARAMETER = "WARNING_IDENTITY_INVALID_TOKENAUTH_PARAMETER";
   }

}
