using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Analytics
{

   [Authorize]
   [Route("api/monthlyResult")]
   public partial class AnalyticsController : Base.BaseController
   {
      public AnalyticsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class AnalyticsService : Base.BaseService
   {
      public AnalyticsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
