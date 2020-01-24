using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Dashboard
{

   [Authorize]
   [Route("api/dashboard")]
   public partial class DashboardController : Base.BaseController
   {
      public DashboardController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class DashboardService : Base.BaseService
   {
      public DashboardService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
