using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Categories
{

   [Authorize]
   [Route("api/categories")]
   public partial class CategoriesController : Base.BaseController
   {
      public CategoriesController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }



      [HttpPut("{id:long}")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<CategoryVM>> UpdateDataAsync(long id, [FromBody]CategoryVM value)
      {
         using (var service = new CategoriesService(this.serviceProvider))
         { return await service.UpdateDataAsync(id, value); }
      }

      [HttpDelete("{id:long}")]
      // [Authorize(Roles = "ActiveUser")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         using (var service = new CategoriesService(this.serviceProvider))
         { return await service.RemoveDataAsync(id); }
      }

   }

}
