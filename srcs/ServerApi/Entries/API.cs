using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Entries
{

   [Authorize]
   [Route("api/entries")]
   public partial class EntriesController : Base.BaseController
   {
      public EntriesController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class EntriesService : Base.BaseService
   {
      public EntriesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
