
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{
   partial class AccountsService
   {

      public async Task<ActionResult<List<AccountTypeVM>>> GetAccountTypesAsync()
      {
         try
         {
            var accountText = $"{"Categories".ToUpper()}_{"enAccountType".ToUpper()}";
            var accountTypes = new enAccountType[] { enAccountType.General, enAccountType.Bank, enAccountType.CreditCard, enAccountType.Investment, enAccountType.Service };
            var result = accountTypes
                .Select(x => new AccountTypeVM
                {
                   Value = x,
                   Text = this.GetTranslation($"{accountText}_{x.ToString().ToUpper()}")
                })
                .OrderBy(x => x.Text)
                .ToList();
            result = await Task.FromResult(result); // just to keep it async
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }
}
