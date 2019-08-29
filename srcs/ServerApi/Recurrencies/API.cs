using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Recurrencies
{

   /*
   [Authorize]
   [Route("api/recurrencies")]
   public partial class RecurrenciesController : Base.BaseController
   {
      public RecurrenciesController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }
   */

   internal partial class RecurrenciesService : Base.BaseService
   {
      public RecurrenciesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
