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


      [HttpDelete("{id:long}")]
      [Authorize(Roles = "Editor")]
      public async Task<ActionResult<bool>> RemoveDataAsync(long id)
      {
         using (var service = new CategoriesService(this.serviceProvider))
         { return await service.RemoveDataAsync(id); }
      }

   }

}
