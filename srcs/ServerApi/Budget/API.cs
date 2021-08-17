using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Budget
{

   [Authorize]
   [Route("api/budget")]
   public partial class BudgetController : Base.BaseController
   {
      public BudgetController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class BudgetService : Base.BaseService
   {
      public BudgetService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
