using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Base
{
   public partial class BaseController : Controller
   {

      protected readonly IServiceProvider serviceProvider;
      public BaseController(IServiceProvider _serviceProvider)
      {
         this.serviceProvider = _serviceProvider;
      }

      [DebuggerStepThrough]
      protected T GetService<T>() where T : class
      { return (T)this.serviceProvider.GetService(typeof(T)); }

   }
}
