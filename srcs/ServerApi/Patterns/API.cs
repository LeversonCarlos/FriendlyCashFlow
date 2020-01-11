using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Patterns
{

   [Authorize]
   [Route("api/patterns")]
   public partial class PatternsController : Base.BaseController
   {
      public PatternsController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class PatternsService : Base.BaseService
   {
      public PatternsService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
