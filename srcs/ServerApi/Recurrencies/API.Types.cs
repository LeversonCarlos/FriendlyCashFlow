using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Recurrencies
{

   partial class RecurrenciesService
   {

      internal async Task<ActionResult<List<RecurrencyTypeVM>>> GetRecurrencyTypesAsync()
      {
         try
         {
            var recurrencyText = $"{"Recurrency".ToUpper()}_{"enRecurrencyType".ToUpper()}";
            var recurrencyTypes = new enRecurrencyType[] { enRecurrencyType.Weekly, enRecurrencyType.Monthly, enRecurrencyType.Bimonthly, enRecurrencyType.Quarterly, enRecurrencyType.SemiYearly, enRecurrencyType.Yearly };
            var result = recurrencyTypes
                .Select(x => new RecurrencyTypeVM
                {
                   Value = x,
                   Text = this.GetTranslation($"{recurrencyText}_{x.ToString().ToUpper()}")
                })
                .OrderBy(x => x.Text)
                .ToList();
            result = await Task.FromResult(result); // just to keep it async
            return this.OkResponse(result);

         }
         catch (Exception ex) { return this.ExceptionResponse(ex); }
      }

   }

   partial class RecurrenciesController
   {
      [HttpGet("types")]
      public async Task<ActionResult<List<RecurrencyTypeVM>>> GetRecurrencyTypesAsync()
      {
         return await this.GetService<RecurrenciesService>().GetRecurrencyTypesAsync();
      }
   }

}
