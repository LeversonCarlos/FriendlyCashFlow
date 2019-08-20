using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{

   [Authorize]
   [Route("api/accounts")]
   public partial class AccountController : Base.BaseController
   {
      public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }


      [HttpPost("")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<AccountVM>> CreateDataAsync([FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.CreateDataAsync(value); }
      }

      [HttpPut("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<AccountVM>> UpdateDataAsync(long id, [FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.UpdateDataAsync(id, value); }
      }

      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.RemoveDataAsync(id); }
      }

   }

}
