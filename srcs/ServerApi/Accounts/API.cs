using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{

   // [Authorize]
   [Route("api/accounts")]
   public partial class AccountController : Base.BaseController
   {
      public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

      [HttpGet("")]
      public async Task<ActionResult<List<AccountVM>>> GetDataAsync()
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetDataAsync(); }
      }

      [HttpGet("{searchText}")]
      public async Task<ActionResult<List<AccountVM>>> GetDataAsync(string searchText)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetDataAsync(searchText); }
      }

      [HttpGet("{id:long}")]
      public async Task<ActionResult<AccountVM>> GetDataAsync(long id)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetDataAsync(id); }
      }

      [HttpPost("")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<AccountVM>> CancelLoadAsync([FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.CreateDataAsync(value); }
      }

   }

}
