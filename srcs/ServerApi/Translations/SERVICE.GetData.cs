
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FriendlyCashFlow.API.Translations
{
   partial class TranslationService
   {
      private const string langID = "pt-BR";

      public async Task<ActionResult<string>> GetDataAsync(string key)
      {
         return this.OkResponse(key);
      }

   }
}
