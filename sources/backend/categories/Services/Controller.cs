using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elesse.Categories
{

   [Route("api/categories")]
   [Authorize]
   public partial class CategoryController : Shared.BaseController
   {

      internal readonly ICategoryService _CategoryService;

      public CategoryController(ICategoryService categoryService)
      {
         _CategoryService = categoryService;
      }

   }

}
