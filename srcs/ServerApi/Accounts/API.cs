using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Accounts
{

   [Authorize]
   [Route("api/accounts")]
   public class AccountController : Base.BaseController
   {
      public AccountController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }


      [HttpGet("types")]
      public async Task<ActionResult<List<AccountTypeVM>>> GetCategoryTypeAsync()
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetAccountTypesAsync(); }
      }


      [HttpGet("search")]
      public async Task<ActionResult<List<AccountVM>>> GetDataAsync()
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.GetDataAsync(); }
      }

      [HttpGet("search/{searchText}")]
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
