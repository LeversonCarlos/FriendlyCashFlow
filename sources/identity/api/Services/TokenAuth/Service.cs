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

            // LOCATE TOKEN
            var token = await _RefreshTokenCollection.FindOneAndDeleteAsync($"{{'TokenID':'{param.RefreshToken}'}}");
            if (token == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

            // VALIDATE TOKEN
            if (token.ExpirationDate < DateTime.UtcNow)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

            // LOCATE USER
            var userCursor = await _Collection.FindAsync($"{{'UserID':'{token.UserID}'}}");
            if (userCursor == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });
            var user = await userCursor.FirstOrDefaultAsync();
            if (user == null)
               return new BadRequestObjectResult(new string[] { WARNINGS.AUTHENTICATION_HAS_FAILED });

            // VALIDATE USER
            // TODO: CHECK ACTIVATION

            // CREATE TOKEN
            var result = await CreateAccessTokenAsync(user);

            // RESULT
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
