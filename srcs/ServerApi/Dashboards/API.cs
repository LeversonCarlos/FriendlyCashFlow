using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Dashboards
{

   [Authorize]
   [Route("api/dashboards")]
   public partial class DashboardsController : Base.BaseController
   {
      public DashboardsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class DashboardsService : Base.BaseService
   {
      public DashboardsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
