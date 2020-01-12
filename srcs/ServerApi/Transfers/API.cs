using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Transfers
{

   [Authorize]
   [Route("api/transfers")]
   public partial class TransfersController : Base.BaseController
   {
      public TransfersController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class TransfersService : Base.BaseService
   {
      public TransfersService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
