using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FriendlyCashFlow.API.Translations
{

   // [Authorize]
   [Route("api/translations")]
   public class TranslationController : Base.BaseController
   {
      private readonly IStringLocalizer<FriendlyCashFlow.Translations.Strings> localizer;
      public TranslationController(IServiceProvider _serviceProvider, IStringLocalizer<FriendlyCashFlow.Translations.Strings> _localizer) : base(_serviceProvider)
      {
         this.localizer = _localizer;
      }

      [HttpGet("{key}")]
      public string[] GetData(string key)
      {
         try
         {
            var result = this.localizer[key];
            return new string[] { result };
         }
         catch { return new string[] { $"{key.ToUpper().Replace(" ", "_")}" }; }
      }

   }

}
