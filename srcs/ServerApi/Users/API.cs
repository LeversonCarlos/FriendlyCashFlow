using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Users
{

   // [Authorize]
   [Route("api/users")]
   public class UserController : Base.BaseController
   {
      public UserController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

      /*
      [HttpGet("{id:long}")]
      public async Task<ActionResult<AccountVM>> GetDataAsync(long id)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetDataAsync(id); }
      }
      */

      [HttpPost("")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<UserVM>> CreateDataAsync([FromBody]CreateVM value)
      {
         using (var service = new UsersService(this.serviceProvider))
         { return await service.CreateDataAsync(value); }
      }

      /*
      [HttpPut("{id:long}")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<AccountVM>> UpdateDataAsync(long id, [FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.UpdateDataAsync(id, value); }
      }
      */

      /*
      [HttpDelete("{id:long}")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.RemoveDataAsync(id); }
      }
      */

   }

}
