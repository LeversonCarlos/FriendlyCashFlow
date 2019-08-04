using Microsoft.AspNetCore.Mvc;
using System;

namespace FriendlyCashFlow.API.Translations
{

   // [Authorize]
   [Route("api/translations")]
   public class TranslationController : Base.BaseController
   {
      public TranslationController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

      [HttpGet("{key}")]
      public string[] GetData(string key)
      {
         using (var service = new TranslationsService(this.serviceProvider))
         {
            return new string[] { service.GetTranslation(key) };
         }
      }

   }

   internal partial class TranslationsService : Base.BaseService
   {
      public TranslationsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
