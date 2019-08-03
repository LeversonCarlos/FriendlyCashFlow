using System;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Base
{
   public partial class BaseController : Controller
   {

      // protected readonly AppSettings appSettings;
      protected readonly IServiceProvider serviceProvider;
      public BaseController(IServiceProvider _serviceProvider)
      {
         this.serviceProvider = _serviceProvider;
         // this.appSettings = this.GetInjectedService<IOptions<AppSettings>>().Value;
      }

   }
}
