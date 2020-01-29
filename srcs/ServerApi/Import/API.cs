using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Import
{

   [Authorize]
   [Route("api/import")]
   public partial class ImportController : Base.BaseController
   {
      public ImportController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class ImportService : Base.BaseService
   {
      public ImportService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
