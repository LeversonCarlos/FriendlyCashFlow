using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FriendlyCashFlow.API.Categories
{

   [Authorize]
   [Route("api/categories")]
   public partial class CategoriesController : Base.BaseController
   {
      public CategoriesController(IServiceProvider _serviceProvider) : base(_serviceProvider) { }
   }

   internal partial class CategoriesService : Base.BaseService
   {
      public CategoriesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
   }

}
