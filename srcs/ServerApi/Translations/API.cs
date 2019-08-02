using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Translations
{

   // [Authorize]
   [Route("api/translations")]
   public class TranslationController : Base.BaseController
   {
      public TranslationController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

      [HttpGet("{key}")]
      public async Task<ActionResult<string>> GetDataAsync(string key)
      {
         using (var service = new TranslationService(this.serviceProvider))
         { return await service.GetDataAsync(key); }
      }

   }

}
