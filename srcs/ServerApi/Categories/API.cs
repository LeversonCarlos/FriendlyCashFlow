using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Categories
{

   // [Authorize]
   [Route("api/categories")]
   public class CategoriesController : Base.BaseController
   {
      public CategoriesController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }

      [HttpGet("types")]
      public async Task<ActionResult<List<CategoryTypeVM>>> GetCategoryTypeAsync()
      {
         using (var service = new CategoriesService(this.serviceProvider))
         { return await service.GetCategoryTypesAsync(); }
      }

      /*
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
      public async Task<ActionResult<AccountVM>> CreateDataAsync([FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.CreateDataAsync(value); }
      }

      [HttpPut("{id:long}")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<AccountVM>> UpdateDataAsync(long id, [FromBody]AccountVM value)
      {
         using (var service = new AccountsService(this.serviceProvider))
         { return await service.UpdateDataAsync(id, value); }
      }

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
